/* 
    Copyright(c) Carlos Picoto (AD7NP), Inc. All rights reserved. 

    The MIT License(MIT) 

    Permission is hereby granted, free of charge, to any person obtaining a copy 
    of this software and associated documentation files(the "Software"), to deal 
    in the Software without restriction, including without limitation the rights 
    to use, copy, modify, merge, publish, distribute, sublicense, and / or sell 
    copies of the Software, and to permit persons to whom the Software is 
    furnished to do so, subject to the following conditions : 
    The above copyright notice and this permission notice shall be included in 
    all copies or substantial portions of the Software. 

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE 
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
    THE SOFTWARE. 
*/
using NAudio.Wave;
using NAudio.Wave.Compression;
using SDRSharp.Common;
using SDRSharp.Radio;
using SDRSharp.WavRecorder;
using System;
using System.Threading;

namespace SDRSharp.UDPAudio
{
    public partial class UDPAudioPlugin : ISharpPlugin
    {
        private readonly RecordingAudioProcessor _UDPaudioProcessor = new RecordingAudioProcessor();
        private SimpleStreamer _UDPaudioStreamer;
        private readonly WavSampleFormat _wavSampleFormat = WavSampleFormat.PCM16;
    }

    public unsafe class SimpleUDPAudio : IDisposable
    {
        private const int DefaultAudioGain = 27;
        private const long MaxStreamLength = int.MaxValue;
        private static readonly int _bufferCount = Utils.GetIntSetting("RecordingBufferCount", 8);
        private readonly float _audioGain = (float)Math.Pow(DefaultAudioGain / 10.0, 10);
        private readonly SharpEvent _bufferEvent = new SharpEvent(false);
        private readonly UnsafeBuffer[] _circularBuffers = new UnsafeBuffer[_bufferCount];
        private readonly Complex*[] _complexCircularBufferPtrs = new Complex*[_bufferCount];
        private readonly float*[] _floatCircularBufferPtrs = new float*[_bufferCount];
        private int _circularBufferTail;
        private int _circularBufferHead;
        private int _circularBufferLength;
        private volatile int _circularBufferUsedCount;
        private long _skippedBuffersCount;
        private bool _diskWriterRunning;
        private string _fileName;
        private double _sampleRate;
        private WavSampleFormat _wavSampleFormat;
        public WaveFileWriter TrxwaveFile = null;
        private Thread _diskWriter;
        private readonly RecordingMode _recordingMode;
        private readonly RecordingAudioProcessor _AFProcessor;
        private byte[] _outputBuffer = null;

        public bool IsRecording
        {
            get { return _diskWriterRunning; }
        }

        public RecordingMode Mode
        {
            get { return _recordingMode; }
        }

        public WavSampleFormat Format
        {
            get { return _wavSampleFormat; }
            set
            {
                if (_diskWriterRunning)
                {
                    throw new ArgumentException("Format cannot be set while recording");
                }
                _wavSampleFormat = value;
            }
        }

        public double SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (_diskWriterRunning)
                {
                    throw new ArgumentException("SampleRate cannot be set while recording");
                }

                _sampleRate = value;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_diskWriterRunning)
                {
                    throw new ArgumentException("FileName cannot be set while recording");
                }
                _fileName = value;
            }
        }


        #region Initialization and Termination

        public SimpleUDPAudio(RecordingAudioProcessor audioProcessor)
        {
            _AFProcessor = audioProcessor;
            _recordingMode = RecordingMode.Audio;
        }

        ~SimpleUDPAudio()
        {
            Dispose();
        }

        public void Dispose()
        {
            FreeBuffers();
        }

        #endregion

        #region Audio Event / Scaling

        public void AudioSamplesIn(float* audio, int length)
        {
            #region Buffers

            var sampleCount = length / 2;
            if (_circularBufferLength != sampleCount)
            {
                FreeBuffers();
                CreateBuffers(sampleCount);

                _circularBufferTail = 0;
                _circularBufferHead = 0;
            }

            #endregion

            if (_circularBufferUsedCount == _bufferCount)
            {
                _skippedBuffersCount++;
                return;
            }

            Utils.Memcpy(_floatCircularBufferPtrs[_circularBufferHead], audio, length * sizeof(float));
            _circularBufferHead++;
            _circularBufferHead &= (_bufferCount - 1);
            _circularBufferUsedCount++;
            _bufferEvent.Set();
        }

        public void ScaleAudio(float* audio, int length)
        {
            for (var i = 0; i < length; i++)
            {
                audio[i] *= _audioGain;
            }
        }

        #endregion

        #region Worker Thread
        public AcmStream resampleStream;
        private void DiskWriterThread()
        {
            if (_recordingMode == RecordingMode.Audio)
            {
                _AFProcessor.AudioReady += AudioSamplesIn;
                _AFProcessor.Enabled = true;
            }
            int input_rate = (int)_sampleRate;

            WaveFormat outFormat = new WaveFormat(48000, 16, 1);
            resampleStream = new AcmStream(new WaveFormat(input_rate, 16, 1), outFormat);
            TrxwaveFile = new WaveFileWriter(FileName, outFormat);

            while (_diskWriterRunning)
            {
                if (_circularBufferTail == _circularBufferHead)
                {
                    _bufferEvent.WaitOne();
                }

                if (_diskWriterRunning && _circularBufferTail != _circularBufferHead)
                {
                    if (_recordingMode == RecordingMode.Audio)
                    {
                        ScaleAudio(_floatCircularBufferPtrs[_circularBufferTail], _circularBuffers[_circularBufferTail].Length * 2);
                    }

                    Write(_floatCircularBufferPtrs[_circularBufferTail], _circularBuffers[_circularBufferTail].Length);


                    _circularBufferUsedCount--;
                    _circularBufferTail++;
                    _circularBufferTail &= (_bufferCount - 1);
                }
            }

            while (_circularBufferTail != _circularBufferHead)
            {
                if (_floatCircularBufferPtrs[_circularBufferTail] != null)
                {
                    Write(_floatCircularBufferPtrs[_circularBufferTail], _circularBuffers[_circularBufferTail].Length);
                }
                _circularBufferTail++;
                _circularBufferTail &= (_bufferCount - 1);
            }

            if (_recordingMode == RecordingMode.Audio)
            {
                _AFProcessor.Enabled = false;
                _AFProcessor.AudioReady -= AudioSamplesIn;
            }
            _diskWriterRunning = false;
        }

        public void Write(float* data, int length)
        {
            if (TrxwaveFile != null)
            {
                switch (_wavSampleFormat)
                {
                    case WavSampleFormat.PCM8:
                        Console.WriteLine("Format not imlemented");
                        throw new InvalidOperationException("Format not imlemented");
                    case WavSampleFormat.PCM16: //This is the only supported one
                        WritePCM16(data, length);
                        break;
                    case WavSampleFormat.Float32:
                        Console.WriteLine("Format not imlemented");
                        throw new InvalidOperationException("Format not imlemented");
                }
                return;
            }
            throw new InvalidOperationException("Stream not open");
        }
        private void WritePCM16(float* data, int length)
        {
            if (_outputBuffer == null || _outputBuffer.Length != (length * sizeof(Int16)))
            {
                _outputBuffer = null;
                _outputBuffer = new byte[length * sizeof(Int16)];
            }
            var ptr = data;
            for (var i = 0; i < length; i++)
            {
                var average = (Int16)(((*ptr++ + *ptr++) / 2) * 32767.0f);
                _outputBuffer[(i * 2)] = (byte)(average & 0x00ff);
                _outputBuffer[(i * 2) + 1] = (byte)(average >> 8);
            }
            int buffer_length = length * sizeof(Int16);
            System.Buffer.BlockCopy(_outputBuffer, 0, resampleStream.SourceBuffer, 0, buffer_length);
            int sourceBytesConverted = 0;

            var convertedBytes = resampleStream.Convert(buffer_length, out sourceBytesConverted);
            if (sourceBytesConverted != buffer_length)
            {
                Console.WriteLine("We didn't convert everything");
            }
            TrxwaveFile.Write(resampleStream.DestBuffer, 0, convertedBytes);
        }

        private void Flush()
        {
            if (TrxwaveFile != null)
            {
                TrxwaveFile.Close();
            }
        }

        private void CreateBuffers(int size)
        {
            for (var i = 0; i < _bufferCount; i++)
            {
                _circularBuffers[i] = UnsafeBuffer.Create(size, sizeof(Complex));
                _complexCircularBufferPtrs[i] = (Complex*)_circularBuffers[i];
                _floatCircularBufferPtrs[i] = (float*)_circularBuffers[i];
            }

            _circularBufferLength = size;
        }

        private void FreeBuffers()
        {
            _circularBufferLength = 0;
            for (var i = 0; i < _bufferCount; i++)
            {
                if (_circularBuffers[i] != null)
                {
                    _circularBuffers[i].Dispose();
                    _circularBuffers[i] = null;
                    _complexCircularBufferPtrs[i] = null;
                    _floatCircularBufferPtrs[i] = null;
                }
            }
        }

        #endregion

        #region Public Methods

        public void StartRecording()
        {
            if (_diskWriter == null)
            {
                _circularBufferHead = 0;
                _circularBufferTail = 0;

                _skippedBuffersCount = 0;

                _bufferEvent.Reset();

                _diskWriter = new Thread(DiskWriterThread);

                _diskWriterRunning = true;
                _diskWriter.Start();
            }
        }

        public void StopRecording()
        {
            _diskWriterRunning = false;

            if (_diskWriter != null)
            {
                _bufferEvent.Set();
                _diskWriter.Join();
            }

            Flush();
            FreeBuffers();

            _diskWriter = null;
        }

        #endregion
    }
}

