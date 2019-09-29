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
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;

namespace SDRSharp.UDPAudio
{

    public partial class Controlpanel : UserControl
    {

        public Action<Boolean,String,String> StartStreamingAF;
        public String HostIP = "127.0.0.1";
        public String HostPort = "7355";
        public Controlpanel()
        {
            InitializeComponent();
            //TODO: Read from File
            this.textBox1.Text = HostIP;
            this.textBox2.Text = HostPort;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.labelVersion.Text = "v" + fvi.FileMajorPart + "." + fvi.FileMinorPart + "." + fvi.FileBuildPart;
        }

        public void SatPC32ServerStreamAFChanged(Boolean StreamAF)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Boolean>(SatPC32ServerStreamAFChanged), new object[] { StreamAF });
                return;
            }
            else
            {
                this.checkBoxStreamAF.Checked = StreamAF;
            }
        }


        private void Controlpanel_Load(object sender, EventArgs e)
        {
            //
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStreamAF.Checked) StartStreamingAF?.Invoke(true, HostIP, HostPort);
            else StartStreamingAF?.Invoke(false, HostIP, HostPort);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String gr_ip = this.textBox1.Text;
            String gr_port = this.textBox2.Text;
            int port;
            IPAddress validIP;
            try 
            { 
                validIP = IPAddress.Parse(gr_ip);
                HostIP = gr_ip;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid IP: {0}:{1}", gr_ip, ex.Message);
                this.textBox1.Text = HostIP;
            }
            try
            {
                port=int.Parse(gr_port);
                if ((port > 6999) && (port < 50001))
                    HostPort = gr_port;
                else
                    this.textBox2.Text = HostPort;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Port: {0}:{1}", gr_port, ex.Message);
                this.textBox2.Text = HostPort;
            }
            
            if (checkBoxStreamAF.Checked)
            {
               //Stop & Start if already running
                StartStreamingAF?.Invoke(false, HostIP, HostPort);
                StartStreamingAF?.Invoke(true, HostIP, HostPort);
            }
        }
    }
}
