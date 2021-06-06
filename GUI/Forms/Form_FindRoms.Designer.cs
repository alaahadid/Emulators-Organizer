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
    partial class Form_FindRoms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_FindRoms));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_findWhat = new System.Windows.Forms.TextBox();
            this.checkBox_caseSensitive = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_searchBy = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox_condition = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_findWhat
            // 
            resources.ApplyResources(this.textBox_findWhat, "textBox_findWhat");
            this.textBox_findWhat.Name = "textBox_findWhat";
            // 
            // checkBox_caseSensitive
            // 
            resources.ApplyResources(this.checkBox_caseSensitive, "checkBox_caseSensitive");
            this.checkBox_caseSensitive.Name = "checkBox_caseSensitive";
            this.checkBox_caseSensitive.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // comboBox_searchBy
            // 
            this.comboBox_searchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_searchBy.FormattingEnabled = true;
            this.comboBox_searchBy.Items.AddRange(new object[] {
            resources.GetString("comboBox_searchBy.Items"),
            resources.GetString("comboBox_searchBy.Items1"),
            resources.GetString("comboBox_searchBy.Items2"),
            resources.GetString("comboBox_searchBy.Items3"),
            resources.GetString("comboBox_searchBy.Items4"),
            resources.GetString("comboBox_searchBy.Items5"),
            resources.GetString("comboBox_searchBy.Items6"),
            resources.GetString("comboBox_searchBy.Items7")});
            resources.ApplyResources(this.comboBox_searchBy, "comboBox_searchBy");
            this.comboBox_searchBy.Name = "comboBox_searchBy";
            this.comboBox_searchBy.SelectedIndexChanged += new System.EventHandler(this.comboBox_searchBy_SelectedIndexChanged);
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
            // comboBox_condition
            // 
            this.comboBox_condition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_condition.FormattingEnabled = true;
            this.comboBox_condition.Items.AddRange(new object[] {
            resources.GetString("comboBox_condition.Items"),
            resources.GetString("comboBox_condition.Items1"),
            resources.GetString("comboBox_condition.Items2"),
            resources.GetString("comboBox_condition.Items3"),
            resources.GetString("comboBox_condition.Items4"),
            resources.GetString("comboBox_condition.Items5"),
            resources.GetString("comboBox_condition.Items6"),
            resources.GetString("comboBox_condition.Items7")});
            resources.ApplyResources(this.comboBox_condition, "comboBox_condition");
            this.comboBox_condition.Name = "comboBox_condition";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Form_FindRoms
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox_condition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox_searchBy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_caseSensitive);
            this.Controls.Add(this.textBox_findWhat);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_FindRoms";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_findWhat;
        private System.Windows.Forms.CheckBox checkBox_caseSensitive;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_searchBy;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox_condition;
        private System.Windows.Forms.Label label3;
    }
}