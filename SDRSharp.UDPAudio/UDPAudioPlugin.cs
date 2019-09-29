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
using SDRSharp.Common;
using SDRSharp.PanView;
using SDRSharp.Radio;
using SDRSharp.WavRecorder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SDRSharp.UDPAudio
{
    public partial class UDPAudioPlugin : ISharpPlugin
    {
        private const string _displayName = "UDP Audio Stream";
        private Controlpanel _controlpanel;
        private ISharpControl control_;
        public Action<String> UpdateStatus;

        public void Initialize(ISharpControl control)
        {
            Console.WriteLine("Initialize Plugin\r\n");
            control_ = control;
            _UDPaudioProcessor.Enabled = false;
            control_.RegisterStreamHook(_UDPaudioProcessor, ProcessorType.FilteredAudioOutput);
            _UDPaudioStreamer = new SimpleStreamer(_UDPaudioProcessor, "127.0.0.1", 7355);

            _controlpanel = new Controlpanel();          
            _controlpanel.StartStreamingAF += SDRSharp_StreamerChanged;


        }
        #region Control Panel Methods
        public UserControl GuiControl
        {
            get { return _controlpanel; }
        }

        public UserControl Gui
        {
            get
            {
                return _controlpanel;
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public void Close()
        {
            StopUDPStreamer();
        }

        public bool HasGui
        {
            get { return true; }
        }
        #endregion 

    }
    //End of Class UDPAudioPlugin
}
