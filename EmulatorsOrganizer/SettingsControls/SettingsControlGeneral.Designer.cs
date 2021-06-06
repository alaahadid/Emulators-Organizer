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
namespace EmulatorsOrganizer
{
    partial class SettingsControlGeneral
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsControlGeneral));
            this.checkBox_autoMinmize = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_loadRecentProfileInsteadOfWelcomWindow = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_autoMinmize
            // 
            resources.ApplyResources(this.checkBox_autoMinmize, "checkBox_autoMinmize");
            this.checkBox_autoMinmize.Name = "checkBox_autoMinmize";
            this.toolTip1.SetToolTip(this.checkBox_autoMinmize, resources.GetString("checkBox_autoMinmize.ToolTip"));
            this.checkBox_autoMinmize.UseVisualStyleBackColor = true;
            // 
            // checkBox_loadRecentProfileInsteadOfWelcomWindow
            // 
            resources.ApplyResources(this.checkBox_loadRecentProfileInsteadOfWelcomWindow, "checkBox_loadRecentProfileInsteadOfWelcomWindow");
            this.checkBox_loadRecentProfileInsteadOfWelcomWindow.Name = "checkBox_loadRecentProfileInsteadOfWelcomWindow";
            this.toolTip1.SetToolTip(this.checkBox_loadRecentProfileInsteadOfWelcomWindow, resources.GetString("checkBox_loadRecentProfileInsteadOfWelcomWindow.ToolTip"));
            this.checkBox_loadRecentProfileInsteadOfWelcomWindow.UseVisualStyleBackColor = true;
            // 
            // SettingsControlGeneral
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_loadRecentProfileInsteadOfWelcomWindow);
            this.Controls.Add(this.checkBox_autoMinmize);
            this.Name = "SettingsControlGeneral";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_autoMinmize;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_loadRecentProfileInsteadOfWelcomWindow;
    }
}
