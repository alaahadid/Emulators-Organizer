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
    partial class ConsolePropertiesRomRenaming
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsolePropertiesRomRenaming));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_copyRom = new System.Windows.Forms.CheckBox();
            this.textBox_copy_folder = new System.Windows.Forms.TextBox();
            this.checkBox_renameTheRom = new System.Windows.Forms.CheckBox();
            this.textBox_renaimgName = new System.Windows.Forms.TextBox();
            this.checkBox_include_extension = new System.Windows.Forms.CheckBox();
            this.checkBox_useEmuFolderIfAvailable = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // checkBox_copyRom
            // 
            resources.ApplyResources(this.checkBox_copyRom, "checkBox_copyRom");
            this.checkBox_copyRom.Name = "checkBox_copyRom";
            this.toolTip1.SetToolTip(this.checkBox_copyRom, resources.GetString("checkBox_copyRom.ToolTip"));
            this.checkBox_copyRom.UseVisualStyleBackColor = true;
            this.checkBox_copyRom.CheckedChanged += new System.EventHandler(this.checkBox_copyRom_CheckedChanged);
            // 
            // textBox_copy_folder
            // 
            resources.ApplyResources(this.textBox_copy_folder, "textBox_copy_folder");
            this.textBox_copy_folder.Name = "textBox_copy_folder";
            this.textBox_copy_folder.ReadOnly = true;
            this.toolTip1.SetToolTip(this.textBox_copy_folder, resources.GetString("textBox_copy_folder.ToolTip"));
            // 
            // checkBox_renameTheRom
            // 
            resources.ApplyResources(this.checkBox_renameTheRom, "checkBox_renameTheRom");
            this.checkBox_renameTheRom.Name = "checkBox_renameTheRom";
            this.toolTip1.SetToolTip(this.checkBox_renameTheRom, resources.GetString("checkBox_renameTheRom.ToolTip"));
            this.checkBox_renameTheRom.UseVisualStyleBackColor = true;
            // 
            // textBox_renaimgName
            // 
            resources.ApplyResources(this.textBox_renaimgName, "textBox_renaimgName");
            this.textBox_renaimgName.Name = "textBox_renaimgName";
            this.toolTip1.SetToolTip(this.textBox_renaimgName, resources.GetString("textBox_renaimgName.ToolTip"));
            // 
            // checkBox_include_extension
            // 
            resources.ApplyResources(this.checkBox_include_extension, "checkBox_include_extension");
            this.checkBox_include_extension.Name = "checkBox_include_extension";
            this.toolTip1.SetToolTip(this.checkBox_include_extension, resources.GetString("checkBox_include_extension.ToolTip"));
            this.checkBox_include_extension.UseVisualStyleBackColor = true;
            // 
            // checkBox_useEmuFolderIfAvailable
            // 
            resources.ApplyResources(this.checkBox_useEmuFolderIfAvailable, "checkBox_useEmuFolderIfAvailable");
            this.checkBox_useEmuFolderIfAvailable.Name = "checkBox_useEmuFolderIfAvailable";
            this.toolTip1.SetToolTip(this.checkBox_useEmuFolderIfAvailable, resources.GetString("checkBox_useEmuFolderIfAvailable.ToolTip"));
            this.checkBox_useEmuFolderIfAvailable.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            // 
            // ConsolePropertiesRomRenaming
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_useEmuFolderIfAvailable);
            this.Controls.Add(this.checkBox_include_extension);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_renaimgName);
            this.Controls.Add(this.checkBox_renameTheRom);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_copy_folder);
            this.Controls.Add(this.checkBox_copyRom);
            this.Name = "ConsolePropertiesRomRenaming";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_copyRom;
        private System.Windows.Forms.TextBox textBox_copy_folder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_renameTheRom;
        private System.Windows.Forms.TextBox textBox_renaimgName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox_include_extension;
        private System.Windows.Forms.CheckBox checkBox_useEmuFolderIfAvailable;
    }
}
