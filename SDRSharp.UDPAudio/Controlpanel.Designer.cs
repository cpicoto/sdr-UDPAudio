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
namespace SDRSharp.UDPAudio
{
    partial class Controlpanel
    {
        /// <summary>
        /// Satnogs Tracker Control Panel
        /// </summary>
        private System.ComponentModel.IContainer components = null;

       /// <summary>
       /// Dispose plugin
       /// </summary>
       /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Control Panel Forms

        /// <summary>
        /// Control Panel Forms
        /// </summary>
        private void InitializeComponent()
        {
            this.labelVersion = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxStreamAF = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(6, 0);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(38, 20);
            this.labelVersion.TabIndex = 8;
            this.labelVersion.TabStop = true;
            this.labelVersion.Text = "v0.0";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.labelVersion);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBoxStreamAF);
            this.groupBox1.Location = new System.Drawing.Point(15, 16);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(296, 227);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // checkBoxStreamAF
            // 
            this.checkBoxStreamAF.AutoSize = true;
            this.checkBoxStreamAF.Location = new System.Drawing.Point(39, 26);
            this.checkBoxStreamAF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxStreamAF.Name = "checkBoxStreamAF";
            this.checkBoxStreamAF.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxStreamAF.Size = new System.Drawing.Size(204, 24);
            this.checkBoxStreamAF.TabIndex = 32;
            this.checkBoxStreamAF.Text = "Stream Audio over UDP";
            this.checkBoxStreamAF.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxStreamAF.UseVisualStyleBackColor = true;
            this.checkBoxStreamAF.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 20);
            this.label1.TabIndex = 33;
            this.label1.Text = "IP";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(75, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(104, 26);
            this.textBox1.TabIndex = 34;
            this.textBox1.Text = "127.0.0.1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(65, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 38);
            this.button1.TabIndex = 35;
            this.button1.Text = "Change IP/PORT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 20);
            this.label2.TabIndex = 36;
            this.label2.Text = ":";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(204, 79);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(50, 26);
            this.textBox2.TabIndex = 37;
            this.textBox2.Text = "7355";
            // 
            // Controlpanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(278, 85);
            this.Name = "Controlpanel";
            this.Size = new System.Drawing.Size(324, 273);
            this.Load += new System.EventHandler(this.Controlpanel_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.LinkLabel labelVersion;
        public System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox checkBoxStreamAF;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}
