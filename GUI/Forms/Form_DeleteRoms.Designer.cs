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
    partial class Form_DeleteRoms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DeleteRoms));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_deleteRomFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_deleteRelatedFiles = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.checkBox_delete_children_when_deleting_parent = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.MessageIcon_Warning;
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
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox_deleteRomFiles
            // 
            resources.ApplyResources(this.checkBox_deleteRomFiles, "checkBox_deleteRomFiles");
            this.checkBox_deleteRomFiles.Name = "checkBox_deleteRomFiles";
            this.checkBox_deleteRomFiles.UseVisualStyleBackColor = true;
            this.checkBox_deleteRomFiles.CheckedChanged += new System.EventHandler(this.checkBox_deleteRomFiles_CheckedChanged);
            // 
            // checkBox_deleteRelatedFiles
            // 
            resources.ApplyResources(this.checkBox_deleteRelatedFiles, "checkBox_deleteRelatedFiles");
            this.checkBox_deleteRelatedFiles.Name = "checkBox_deleteRelatedFiles";
            this.checkBox_deleteRelatedFiles.UseVisualStyleBackColor = true;
            this.checkBox_deleteRelatedFiles.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // checkBox_delete_children_when_deleting_parent
            // 
            resources.ApplyResources(this.checkBox_delete_children_when_deleting_parent, "checkBox_delete_children_when_deleting_parent");
            this.checkBox_delete_children_when_deleting_parent.Name = "checkBox_delete_children_when_deleting_parent";
            this.checkBox_delete_children_when_deleting_parent.UseVisualStyleBackColor = true;
            this.checkBox_delete_children_when_deleting_parent.CheckedChanged += new System.EventHandler(this.checkBox_delete_children_when_deleting_parent_CheckedChanged);
            // 
            // Form_DeleteRoms
            // 
            this.AcceptButton = this.button2;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.checkBox_delete_children_when_deleting_parent);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.checkBox_deleteRelatedFiles);
            this.Controls.Add(this.checkBox_deleteRomFiles);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form_DeleteRoms";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_deleteRomFiles;
        private System.Windows.Forms.CheckBox checkBox_deleteRelatedFiles;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox_delete_children_when_deleting_parent;
    }
}