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

namespace SDRSharp.UDPAudio
{

    public partial class Controlpanel : UserControl
    {

        public Action<Boolean> StartStreamingAF;

        public Controlpanel()
        {
            InitializeComponent();
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
            if (checkBoxStreamAF.Checked) StartStreamingAF?.Invoke(true);
            else StartStreamingAF?.Invoke(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBoxStreamAF.Checked)
            {
                StartStreamingAF?.Invoke(false);
                //Change IP and Port
                StartStreamingAF?.Invoke(true);
            }
            else
            {
                // just chnge the IP and Port
            }
            //StartStreamingAF?.Invoke(false);
        }
    }
}
