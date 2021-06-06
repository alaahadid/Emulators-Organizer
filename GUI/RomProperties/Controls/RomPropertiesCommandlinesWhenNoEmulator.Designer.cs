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
    partial class RomPropertiesCommandlinesWhenNoEmulator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RomPropertiesCommandlinesWhenNoEmulator));
            this.commandlinesEditor1 = new EmulatorsOrganizer.GUI.CommandlinesEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // commandlinesEditor1
            // 
            this.commandlinesEditor1.AllowEdit = true;
            this.commandlinesEditor1.BackgroundImageMode = EmulatorsOrganizer.Core.ImageViewMode.Normal;
            this.commandlinesEditor1.CommandlineGroups = ((System.Collections.Generic.List<EmulatorsOrganizer.Core.CommandlinesGroup>)(resources.GetObject("commandlinesEditor1.CommandlineGroups")));
            resources.ApplyResources(this.commandlinesEditor1, "commandlinesEditor1");
            this.commandlinesEditor1.Name = "commandlinesEditor1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // RomPropertiesCommandlinesWhenNoEmulator
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label1);
            this.Controls.Add(this.commandlinesEditor1);
            this.Name = "RomPropertiesCommandlinesWhenNoEmulator";
            this.ResumeLayout(false);

        }

        #endregion

        private CommandlinesEditor commandlinesEditor1;
        private System.Windows.Forms.Label label1;
    }
}
