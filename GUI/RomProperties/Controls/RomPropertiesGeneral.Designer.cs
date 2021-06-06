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
    partial class RomPropertiesGeneral
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RomPropertiesGeneral));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_romName = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_ignorePathNotExist = new System.Windows.Forms.CheckBox();
            this.textBox_filepath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_romName
            // 
            resources.ApplyResources(this.textBox_romName, "textBox_romName");
            this.textBox_romName.Name = "textBox_romName";
            this.toolTip1.SetToolTip(this.textBox_romName, resources.GetString("textBox_romName.ToolTip"));
            // 
            // checkBox_ignorePathNotExist
            // 
            resources.ApplyResources(this.checkBox_ignorePathNotExist, "checkBox_ignorePathNotExist");
            this.checkBox_ignorePathNotExist.Name = "checkBox_ignorePathNotExist";
            this.toolTip1.SetToolTip(this.checkBox_ignorePathNotExist, resources.GetString("checkBox_ignorePathNotExist.ToolTip"));
            this.checkBox_ignorePathNotExist.UseVisualStyleBackColor = true;
            // 
            // textBox_filepath
            // 
            resources.ApplyResources(this.textBox_filepath, "textBox_filepath");
            this.textBox_filepath.Name = "textBox_filepath";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RomPropertiesGeneral
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_ignorePathNotExist);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_romName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_filepath);
            this.Controls.Add(this.label2);
            this.Name = "RomPropertiesGeneral";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_romName;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBox_filepath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_ignorePathNotExist;
    }
}
