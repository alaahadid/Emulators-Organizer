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
namespace EmulatorsOrganizer.Core
{
    partial class DatabaseFileControl_MameDat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseFileControl_MameDat));
            this.checkBox_rename_rom = new System.Windows.Forms.CheckBox();
            this.checkBox_ad_desc = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_rename_rom
            // 
            resources.ApplyResources(this.checkBox_rename_rom, "checkBox_rename_rom");
            this.checkBox_rename_rom.Name = "checkBox_rename_rom";
            this.checkBox_rename_rom.UseVisualStyleBackColor = true;
            this.checkBox_rename_rom.CheckedChanged += new System.EventHandler(this.checkBox_rename_rom_CheckedChanged);
            // 
            // checkBox_ad_desc
            // 
            resources.ApplyResources(this.checkBox_ad_desc, "checkBox_ad_desc");
            this.checkBox_ad_desc.Name = "checkBox_ad_desc";
            this.checkBox_ad_desc.UseVisualStyleBackColor = true;
            this.checkBox_ad_desc.CheckedChanged += new System.EventHandler(this.checkBox_ad_desc_CheckedChanged);
            // 
            // DatabaseFileControl_MameDat
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_ad_desc);
            this.Controls.Add(this.checkBox_rename_rom);
            this.Name = "DatabaseFileControl_MameDat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_rename_rom;
        private System.Windows.Forms.CheckBox checkBox_ad_desc;
    }
}
