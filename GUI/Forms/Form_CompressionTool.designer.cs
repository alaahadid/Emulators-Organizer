// This file is part of Emulators Organizer
// A program that can organize roms and emulators
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
namespace EmulatorsOrganizer.GUI
{
    partial class Form_CompressionTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_CompressionTool));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_input = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_clearOutputFolder = new System.Windows.Forms.CheckBox();
            this.textBox_output = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox_compress = new System.Windows.Forms.GroupBox();
            this.checkBox_compreesPreserve = new System.Windows.Forms.CheckBox();
            this.checkBox_ignoreArchiveInCompress = new System.Windows.Forms.CheckBox();
            this.textBox_compressPassword = new System.Windows.Forms.TextBox();
            this.checkBox_CompressPassword = new System.Windows.Forms.CheckBox();
            this.comboBox_compressLevel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_compressFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_extract = new System.Windows.Forms.GroupBox();
            this.textBox_extractPass = new System.Windows.Forms.TextBox();
            this.checkBox_extractPassword = new System.Windows.Forms.CheckBox();
            this.checkBox_extractCreateFolders = new System.Windows.Forms.CheckBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_progress2 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label_progrss1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox_compress.SuspendLayout();
            this.groupBox_extract.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_input);
            this.groupBox1.Controls.Add(this.button1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // textBox_input
            // 
            resources.ApplyResources(this.textBox_input, "textBox_input");
            this.textBox_input.Name = "textBox_input";
            this.textBox_input.ReadOnly = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_clearOutputFolder);
            this.groupBox2.Controls.Add(this.textBox_output);
            this.groupBox2.Controls.Add(this.button2);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkBox_clearOutputFolder
            // 
            resources.ApplyResources(this.checkBox_clearOutputFolder, "checkBox_clearOutputFolder");
            this.checkBox_clearOutputFolder.Name = "checkBox_clearOutputFolder";
            this.checkBox_clearOutputFolder.UseVisualStyleBackColor = true;
            // 
            // textBox_output
            // 
            resources.ApplyResources(this.textBox_output, "textBox_output");
            this.textBox_output.Name = "textBox_output";
            this.textBox_output.ReadOnly = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox_compress
            // 
            this.groupBox_compress.Controls.Add(this.checkBox_compreesPreserve);
            this.groupBox_compress.Controls.Add(this.checkBox_ignoreArchiveInCompress);
            this.groupBox_compress.Controls.Add(this.textBox_compressPassword);
            this.groupBox_compress.Controls.Add(this.checkBox_CompressPassword);
            this.groupBox_compress.Controls.Add(this.comboBox_compressLevel);
            this.groupBox_compress.Controls.Add(this.label3);
            this.groupBox_compress.Controls.Add(this.comboBox_compressFormat);
            this.groupBox_compress.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox_compress, "groupBox_compress");
            this.groupBox_compress.Name = "groupBox_compress";
            this.groupBox_compress.TabStop = false;
            // 
            // checkBox_compreesPreserve
            // 
            resources.ApplyResources(this.checkBox_compreesPreserve, "checkBox_compreesPreserve");
            this.checkBox_compreesPreserve.Name = "checkBox_compreesPreserve";
            this.checkBox_compreesPreserve.UseVisualStyleBackColor = true;
            // 
            // checkBox_ignoreArchiveInCompress
            // 
            resources.ApplyResources(this.checkBox_ignoreArchiveInCompress, "checkBox_ignoreArchiveInCompress");
            this.checkBox_ignoreArchiveInCompress.Checked = true;
            this.checkBox_ignoreArchiveInCompress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ignoreArchiveInCompress.Name = "checkBox_ignoreArchiveInCompress";
            this.checkBox_ignoreArchiveInCompress.UseVisualStyleBackColor = true;
            // 
            // textBox_compressPassword
            // 
            resources.ApplyResources(this.textBox_compressPassword, "textBox_compressPassword");
            this.textBox_compressPassword.Name = "textBox_compressPassword";
            // 
            // checkBox_CompressPassword
            // 
            resources.ApplyResources(this.checkBox_CompressPassword, "checkBox_CompressPassword");
            this.checkBox_CompressPassword.Name = "checkBox_CompressPassword";
            this.checkBox_CompressPassword.UseVisualStyleBackColor = true;
            // 
            // comboBox_compressLevel
            // 
            this.comboBox_compressLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_compressLevel.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_compressLevel, "comboBox_compressLevel");
            this.comboBox_compressLevel.Name = "comboBox_compressLevel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBox_compressFormat
            // 
            this.comboBox_compressFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_compressFormat.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_compressFormat, "comboBox_compressFormat");
            this.comboBox_compressFormat.Name = "comboBox_compressFormat";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox_extract
            // 
            this.groupBox_extract.Controls.Add(this.textBox_extractPass);
            this.groupBox_extract.Controls.Add(this.checkBox_extractPassword);
            this.groupBox_extract.Controls.Add(this.checkBox_extractCreateFolders);
            resources.ApplyResources(this.groupBox_extract, "groupBox_extract");
            this.groupBox_extract.Name = "groupBox_extract";
            this.groupBox_extract.TabStop = false;
            // 
            // textBox_extractPass
            // 
            resources.ApplyResources(this.textBox_extractPass, "textBox_extractPass");
            this.textBox_extractPass.Name = "textBox_extractPass";
            // 
            // checkBox_extractPassword
            // 
            resources.ApplyResources(this.checkBox_extractPassword, "checkBox_extractPassword");
            this.checkBox_extractPassword.Name = "checkBox_extractPassword";
            this.checkBox_extractPassword.UseVisualStyleBackColor = true;
            // 
            // checkBox_extractCreateFolders
            // 
            resources.ApplyResources(this.checkBox_extractCreateFolders, "checkBox_extractCreateFolders");
            this.checkBox_extractCreateFolders.Checked = true;
            this.checkBox_extractCreateFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_extractCreateFolders.Name = "checkBox_extractCreateFolders";
            this.checkBox_extractCreateFolders.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label_progress2);
            this.groupBox3.Controls.Add(this.progressBar2);
            this.groupBox3.Controls.Add(this.label_progrss1);
            this.groupBox3.Controls.Add(this.progressBar1);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label_progress2
            // 
            resources.ApplyResources(this.label_progress2, "label_progress2");
            this.label_progress2.Name = "label_progress2";
            // 
            // progressBar2
            // 
            resources.ApplyResources(this.progressBar2, "progressBar2");
            this.progressBar2.Name = "progressBar2";
            // 
            // label_progrss1
            // 
            resources.ApplyResources(this.label_progrss1, "label_progrss1");
            this.label_progrss1.Name = "label_progrss1";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Frm_CompressionTool
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox_extract);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.groupBox_compress);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_CompressionTool";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_CompressionTool_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox_compress.ResumeLayout(false);
            this.groupBox_compress.PerformLayout();
            this.groupBox_extract.ResumeLayout(false);
            this.groupBox_extract.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_input;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_output;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox_compress;
        private System.Windows.Forms.GroupBox groupBox_extract;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.CheckBox checkBox_compreesPreserve;
        private System.Windows.Forms.CheckBox checkBox_ignoreArchiveInCompress;
        private System.Windows.Forms.TextBox textBox_compressPassword;
        private System.Windows.Forms.CheckBox checkBox_CompressPassword;
        private System.Windows.Forms.ComboBox comboBox_compressLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_compressFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_extractPass;
        private System.Windows.Forms.CheckBox checkBox_extractPassword;
        private System.Windows.Forms.CheckBox checkBox_extractCreateFolders;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_progress2;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label_progrss1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox_clearOutputFolder;
        private System.Windows.Forms.Label label1;
    }
}