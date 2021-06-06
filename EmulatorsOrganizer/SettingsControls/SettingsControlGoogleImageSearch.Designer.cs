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
    partial class SettingsControlGoogleImageSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsControlGoogleImageSearch));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_searchMethod = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_customName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // comboBox_searchMethod
            // 
            this.comboBox_searchMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_searchMethod.FormattingEnabled = true;
            this.comboBox_searchMethod.Items.AddRange(new object[] {
            resources.GetString("comboBox_searchMethod.Items"),
            resources.GetString("comboBox_searchMethod.Items1"),
            resources.GetString("comboBox_searchMethod.Items2"),
            resources.GetString("comboBox_searchMethod.Items3"),
            resources.GetString("comboBox_searchMethod.Items4"),
            resources.GetString("comboBox_searchMethod.Items5")});
            resources.ApplyResources(this.comboBox_searchMethod, "comboBox_searchMethod");
            this.comboBox_searchMethod.Name = "comboBox_searchMethod";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBox_customName
            // 
            resources.ApplyResources(this.textBox_customName, "textBox_customName");
            this.textBox_customName.Name = "textBox_customName";
            // 
            // SettingsControlGoogleImageSearch
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_customName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_searchMethod);
            this.Controls.Add(this.label1);
            this.Name = "SettingsControlGoogleImageSearch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_searchMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_customName;
    }
}
