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
    partial class FormICCopyConflict
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
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton_do_nothing = new System.Windows.Forms.RadioButton();
            this.radioButton_replace_settings = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label_ic_names = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "These Information Containers shares the same name and type.\r\nWhat would you like " +
    "to do with the other container settings ?";
            // 
            // radioButton_do_nothing
            // 
            this.radioButton_do_nothing.AutoSize = true;
            this.radioButton_do_nothing.Checked = true;
            this.radioButton_do_nothing.Location = new System.Drawing.Point(12, 75);
            this.radioButton_do_nothing.Name = "radioButton_do_nothing";
            this.radioButton_do_nothing.Size = new System.Drawing.Size(306, 17);
            this.radioButton_do_nothing.TabIndex = 1;
            this.radioButton_do_nothing.TabStop = true;
            this.radioButton_do_nothing.Text = "Keep Information Container settings (do nothing, no copy)";
            this.radioButton_do_nothing.UseVisualStyleBackColor = true;
            // 
            // radioButton_replace_settings
            // 
            this.radioButton_replace_settings.AutoSize = true;
            this.radioButton_replace_settings.Location = new System.Drawing.Point(12, 98);
            this.radioButton_replace_settings.Name = "radioButton_replace_settings";
            this.radioButton_replace_settings.Size = new System.Drawing.Size(427, 17);
            this.radioButton_replace_settings.TabIndex = 2;
            this.radioButton_replace_settings.TabStop = true;
            this.radioButton_replace_settings.Text = "Replace the container settings (name will be the same, other settings will be cop" +
    "ied)";
            this.radioButton_replace_settings.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 155);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(218, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Do the same for other conflicts if found.";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label_ic_names
            // 
            this.label_ic_names.AutoSize = true;
            this.label_ic_names.Location = new System.Drawing.Point(12, 9);
            this.label_ic_names.Name = "label_ic_names";
            this.label_ic_names.Size = new System.Drawing.Size(201, 13);
            this.label_ic_names.TabIndex = 4;
            this.label_ic_names.Text = "ic 1 from console 1 to ic 2 from console 2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormICCopyConflict
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 184);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_ic_names);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.radioButton_replace_settings);
            this.Controls.Add(this.radioButton_do_nothing);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormICCopyConflict";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conflict found";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton_do_nothing;
        private System.Windows.Forms.RadioButton radioButton_replace_settings;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label_ic_names;
        private System.Windows.Forms.Button button1;
    }
}