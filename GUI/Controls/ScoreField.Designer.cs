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
    partial class ScoreField
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScoreField));
            this.label_name = new System.Windows.Forms.Label();
            this.panel_score = new System.Windows.Forms.Panel();
            this.linkLabel_score = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_name
            // 
            resources.ApplyResources(this.label_name, "label_name");
            this.label_name.Name = "label_name";
            // 
            // panel_score
            // 
            this.panel_score.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_score.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.panel_score, "panel_score");
            this.panel_score.Name = "panel_score";
            this.panel_score.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_score_Paint);
            this.panel_score.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_score_MouseClick);
            // 
            // linkLabel_score
            // 
            resources.ApplyResources(this.linkLabel_score, "linkLabel_score");
            this.linkLabel_score.Name = "linkLabel_score";
            this.linkLabel_score.TabStop = true;
            this.linkLabel_score.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_score_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel_score);
            this.panel1.Controls.Add(this.linkLabel_score);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // ScoreField
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label_name);
            this.MinimumSize = new System.Drawing.Size(138, 28);
            this.Name = "ScoreField";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Panel panel_score;
        private System.Windows.Forms.LinkLabel linkLabel_score;
        private System.Windows.Forms.Panel panel1;
    }
}
