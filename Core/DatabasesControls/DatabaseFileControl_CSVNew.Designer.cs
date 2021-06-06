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
    partial class DatabaseFileControl_CSVNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseFileControl_CSVNew));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox_useAlternativeNameForCompare = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_useAlternativeNameForRenaming = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Spliter :";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            ">",
            "<",
            ",",
            "."});
            this.comboBox1.Location = new System.Drawing.Point(74, 65);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(180, 28);
            this.comboBox1.TabIndex = 1;
            this.toolTip1.SetToolTip(this.comboBox1, "Select the spliter word used in the database for splitting entries.\r\nSimply open " +
        "the database file using text editer then check the \r\nsplit word used between ent" +
        "ries.");
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // checkBox_useAlternativeNameForCompare
            // 
            this.checkBox_useAlternativeNameForCompare.AutoSize = true;
            this.checkBox_useAlternativeNameForCompare.Checked = true;
            this.checkBox_useAlternativeNameForCompare.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_useAlternativeNameForCompare.Location = new System.Drawing.Point(3, 3);
            this.checkBox_useAlternativeNameForCompare.Name = "checkBox_useAlternativeNameForCompare";
            this.checkBox_useAlternativeNameForCompare.Size = new System.Drawing.Size(444, 24);
            this.checkBox_useAlternativeNameForCompare.TabIndex = 2;
            this.checkBox_useAlternativeNameForCompare.Text = "Use \"Alternative Name\" instead of \"Name\" for COMPARE.";
            this.toolTip1.SetToolTip(this.checkBox_useAlternativeNameForCompare, "If set, the program will use the second entry of line of the \r\ndatabase lines ins" +
        "tead of the first entry for comparing.\r\nPlease use this option if the comparing " +
        "doesn\'t work\r\n(i.e. not matched roms).");
            this.checkBox_useAlternativeNameForCompare.UseVisualStyleBackColor = true;
            this.checkBox_useAlternativeNameForCompare.CheckedChanged += new System.EventHandler(this.checkBox_useAlternativeName_CheckedChanged);
            // 
            // checkBox_useAlternativeNameForRenaming
            // 
            this.checkBox_useAlternativeNameForRenaming.AutoSize = true;
            this.checkBox_useAlternativeNameForRenaming.Location = new System.Drawing.Point(3, 33);
            this.checkBox_useAlternativeNameForRenaming.Name = "checkBox_useAlternativeNameForRenaming";
            this.checkBox_useAlternativeNameForRenaming.Size = new System.Drawing.Size(433, 24);
            this.checkBox_useAlternativeNameForRenaming.TabIndex = 3;
            this.checkBox_useAlternativeNameForRenaming.Text = "Use \"Alternative Name\" instead of \"Name\" for RENAME.";
            this.toolTip1.SetToolTip(this.checkBox_useAlternativeNameForRenaming, resources.GetString("checkBox_useAlternativeNameForRenaming.ToolTip"));
            this.checkBox_useAlternativeNameForRenaming.UseVisualStyleBackColor = true;
            this.checkBox_useAlternativeNameForRenaming.CheckedChanged += new System.EventHandler(this.checkBox_useAlternativeNameForRenaming_CheckedChanged);
            // 
            // DatabaseFileControl_CSVNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_useAlternativeNameForRenaming);
            this.Controls.Add(this.checkBox_useAlternativeNameForCompare);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DatabaseFileControl_CSVNew";
            this.Size = new System.Drawing.Size(496, 134);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox_useAlternativeNameForCompare;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_useAlternativeNameForRenaming;
    }
}
