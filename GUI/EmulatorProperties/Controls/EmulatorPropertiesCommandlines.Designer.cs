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
    partial class EmulatorPropertiesCommandlines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmulatorPropertiesCommandlines));
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox_consoles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.commandlinesEditor1 = new EmulatorsOrganizer.GUI.CommandlinesEditor();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox_consoles);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 39);
            this.panel1.TabIndex = 0;
            // 
            // comboBox_consoles
            // 
            this.comboBox_consoles.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox_consoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_consoles.FormattingEnabled = true;
            this.comboBox_consoles.Location = new System.Drawing.Point(0, 13);
            this.comboBox_consoles.Name = "comboBox_consoles";
            this.comboBox_consoles.Size = new System.Drawing.Size(292, 21);
            this.comboBox_consoles.TabIndex = 1;
            this.comboBox_consoles.SelectedIndexChanged += new System.EventHandler(this.comboBox_consoles_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Console:";
            // 
            // commandlinesEditor1
            // 
            this.commandlinesEditor1.AllowEdit = true;
            this.commandlinesEditor1.CommandlineGroups = ((System.Collections.Generic.List<EmulatorsOrganizer.Core.CommandlinesGroup>)(resources.GetObject("commandlinesEditor1.CommandlineGroups")));
            this.commandlinesEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandlinesEditor1.Location = new System.Drawing.Point(0, 39);
            this.commandlinesEditor1.Name = "commandlinesEditor1";
            this.commandlinesEditor1.Size = new System.Drawing.Size(292, 272);
            this.commandlinesEditor1.TabIndex = 1;
            // 
            // EmulatorPropertiesCommandlines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.commandlinesEditor1);
            this.Controls.Add(this.panel1);
            this.Name = "EmulatorPropertiesCommandlines";
            this.Size = new System.Drawing.Size(292, 311);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox_consoles;
        private System.Windows.Forms.Label label1;
        private CommandlinesEditor commandlinesEditor1;
    }
}
