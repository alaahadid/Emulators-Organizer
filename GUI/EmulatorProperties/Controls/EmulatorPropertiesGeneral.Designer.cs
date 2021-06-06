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
    partial class EmulatorPropertiesGeneral
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmulatorPropertiesGeneral));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_emulatorName = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton_useFile = new System.Windows.Forms.RadioButton();
            this.radioButton_useBat = new System.Windows.Forms.RadioButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_emulatorName
            // 
            resources.ApplyResources(this.textBox_emulatorName, "textBox_emulatorName");
            this.textBox_emulatorName.Name = "textBox_emulatorName";
            this.toolTip1.SetToolTip(this.textBox_emulatorName, resources.GetString("textBox_emulatorName.ToolTip"));
            // 
            // textBox_path
            // 
            resources.ApplyResources(this.textBox_path, "textBox_path");
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.ReadOnly = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton_useFile
            // 
            resources.ApplyResources(this.radioButton_useFile, "radioButton_useFile");
            this.radioButton_useFile.Checked = true;
            this.radioButton_useFile.Name = "radioButton_useFile";
            this.radioButton_useFile.TabStop = true;
            this.radioButton_useFile.UseVisualStyleBackColor = true;
            // 
            // radioButton_useBat
            // 
            resources.ApplyResources(this.radioButton_useBat, "radioButton_useBat");
            this.radioButton_useBat.Name = "radioButton_useBat";
            this.radioButton_useBat.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            // 
            // EmulatorPropertiesGeneral
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.radioButton_useBat);
            this.Controls.Add(this.radioButton_useFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.textBox_emulatorName);
            this.Controls.Add(this.label1);
            this.Name = "EmulatorPropertiesGeneral";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_emulatorName;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton_useFile;
        private System.Windows.Forms.RadioButton radioButton_useBat;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
