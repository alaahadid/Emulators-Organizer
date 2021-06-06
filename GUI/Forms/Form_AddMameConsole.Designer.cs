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
    partial class Form_AddMameConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AddMameConsole));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_databaseFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox_folders = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox_verifyFiles = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_status = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBox_subfolders = new System.Windows.Forms.CheckBox();
            this.textBox_consoleName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_bulidDefaultICS = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_databaseFile
            // 
            resources.ApplyResources(this.textBox_databaseFile, "textBox_databaseFile");
            this.textBox_databaseFile.Name = "textBox_databaseFile";
            this.textBox_databaseFile.ReadOnly = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // listBox_folders
            // 
            this.listBox_folders.FormattingEnabled = true;
            resources.ApplyResources(this.listBox_folders, "listBox_folders");
            this.listBox_folders.Name = "listBox_folders";
            this.toolTip1.SetToolTip(this.listBox_folders, resources.GetString("listBox_folders.ToolTip"));
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.toolTip1.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox_verifyFiles
            // 
            resources.ApplyResources(this.checkBox_verifyFiles, "checkBox_verifyFiles");
            this.checkBox_verifyFiles.Name = "checkBox_verifyFiles";
            this.toolTip1.SetToolTip(this.checkBox_verifyFiles, resources.GetString("checkBox_verifyFiles.ToolTip"));
            this.checkBox_verifyFiles.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            this.toolTip1.SetToolTip(this.progressBar1, resources.GetString("progressBar1.ToolTip"));
            // 
            // label_status
            // 
            resources.ApplyResources(this.label_status, "label_status");
            this.label_status.Name = "label_status";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // checkBox_subfolders
            // 
            resources.ApplyResources(this.checkBox_subfolders, "checkBox_subfolders");
            this.checkBox_subfolders.Name = "checkBox_subfolders";
            this.toolTip1.SetToolTip(this.checkBox_subfolders, resources.GetString("checkBox_subfolders.ToolTip"));
            this.checkBox_subfolders.UseVisualStyleBackColor = true;
            // 
            // textBox_consoleName
            // 
            resources.ApplyResources(this.textBox_consoleName, "textBox_consoleName");
            this.textBox_consoleName.Name = "textBox_consoleName";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // checkBox_bulidDefaultICS
            // 
            resources.ApplyResources(this.checkBox_bulidDefaultICS, "checkBox_bulidDefaultICS");
            this.checkBox_bulidDefaultICS.Checked = true;
            this.checkBox_bulidDefaultICS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_bulidDefaultICS.Name = "checkBox_bulidDefaultICS";
            this.toolTip1.SetToolTip(this.checkBox_bulidDefaultICS, resources.GetString("checkBox_bulidDefaultICS.ToolTip"));
            this.checkBox_bulidDefaultICS.UseVisualStyleBackColor = true;
            // 
            // Form_AddMameConsole
            // 
            this.AcceptButton = this.button5;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_bulidDefaultICS);
            this.Controls.Add(this.textBox_consoleName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox_subfolders);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.checkBox_verifyFiles);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox_folders);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_databaseFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_AddMameConsole";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_MameConsole_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_databaseFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox_folders;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox_verifyFiles;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox_subfolders;
        private System.Windows.Forms.TextBox textBox_consoleName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_bulidDefaultICS;
    }
}