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
    partial class RomPropertiesCommandlines
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
            SaveCurrent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RomPropertiesCommandlines));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_emulators = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.commandlinesEditor1 = new EmulatorsOrganizer.GUI.CommandlinesEditor();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Emulator:";
            // 
            // comboBox_emulators
            // 
            this.comboBox_emulators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_emulators.FormattingEnabled = true;
            this.comboBox_emulators.Location = new System.Drawing.Point(3, 16);
            this.comboBox_emulators.Name = "comboBox_emulators";
            this.comboBox_emulators.Size = new System.Drawing.Size(391, 21);
            this.comboBox_emulators.TabIndex = 1;
            this.comboBox_emulators.SelectedIndexChanged += new System.EventHandler(this.comboBox_emulators_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Special command-lines for this emulator:";
            // 
            // commandlinesEditor1
            // 
            this.commandlinesEditor1.AllowEdit = true;
            this.commandlinesEditor1.BackgroundImageMode = EmulatorsOrganizer.Core.ImageViewMode.Normal;
            this.commandlinesEditor1.CommandlineGroups = ((System.Collections.Generic.List<EmulatorsOrganizer.Core.CommandlinesGroup>)(resources.GetObject("commandlinesEditor1.CommandlineGroups")));
            this.commandlinesEditor1.Enabled = false;
            this.commandlinesEditor1.Location = new System.Drawing.Point(3, 56);
            this.commandlinesEditor1.Name = "commandlinesEditor1";
            this.commandlinesEditor1.Size = new System.Drawing.Size(391, 253);
            this.commandlinesEditor1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 312);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(272, 26);
            this.label3.TabIndex = 4;
            this.label3.Text = "* Changes applyed directly, default and cancel buttons\r\nhas no effect.";
            // 
            // RomPropertiesCommandlines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.commandlinesEditor1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_emulators);
            this.Controls.Add(this.label1);
            this.Name = "RomPropertiesCommandlines";
            this.Size = new System.Drawing.Size(397, 349);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_emulators;
        private System.Windows.Forms.Label label2;
        private CommandlinesEditor commandlinesEditor1;
        private System.Windows.Forms.Label label3;
    }
}
