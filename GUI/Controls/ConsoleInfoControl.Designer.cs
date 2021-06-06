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
    partial class ConsoleInfoControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleInfoControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imagePanel1 = new EmulatorsOrganizer.Core.ImagePanel();
            this.label_favoriteGame = new System.Windows.Forms.Label();
            this.label_most_played = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel_favoriteGame = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel_mostplayed = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel_highRated = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel_name = new System.Windows.Forms.LinkLabel();
            this.textBox_tabs = new System.Windows.Forms.TextBox();
            this.textBox_roms_AI = new System.Windows.Forms.TextBox();
            this.textBox_roms_ind = new System.Windows.Forms.TextBox();
            this.textBox_roms = new System.Windows.Forms.TextBox();
            this.textBox_console_play_time = new System.Windows.Forms.TextBox();
            this.textBox_extensions = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_rtf = new System.Windows.Forms.Label();
            this.label_manual = new System.Windows.Forms.Label();
            this.rating1 = new EmulatorsOrganizer.GUI.Rating();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imagePanel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // imagePanel1
            // 
            this.imagePanel1.DefaultImage = null;
            resources.ApplyResources(this.imagePanel1, "imagePanel1");
            this.imagePanel1.ImageToView = null;
            this.imagePanel1.ImageViewMode = EmulatorsOrganizer.Core.ImageViewMode.StretchIfLarger;
            this.imagePanel1.Name = "imagePanel1";
            // 
            // label_favoriteGame
            // 
            resources.ApplyResources(this.label_favoriteGame, "label_favoriteGame");
            this.label_favoriteGame.Name = "label_favoriteGame";
            // 
            // label_most_played
            // 
            resources.ApplyResources(this.label_most_played, "label_most_played");
            this.label_most_played.Name = "label_most_played";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // linkLabel_favoriteGame
            // 
            resources.ApplyResources(this.linkLabel_favoriteGame, "linkLabel_favoriteGame");
            this.linkLabel_favoriteGame.Name = "linkLabel_favoriteGame";
            this.linkLabel_favoriteGame.TabStop = true;
            this.toolTip1.SetToolTip(this.linkLabel_favoriteGame, resources.GetString("linkLabel_favoriteGame.ToolTip"));
            this.linkLabel_favoriteGame.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_favoriteGame_LinkClicked);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // linkLabel_mostplayed
            // 
            resources.ApplyResources(this.linkLabel_mostplayed, "linkLabel_mostplayed");
            this.linkLabel_mostplayed.Name = "linkLabel_mostplayed";
            this.linkLabel_mostplayed.TabStop = true;
            this.toolTip1.SetToolTip(this.linkLabel_mostplayed, resources.GetString("linkLabel_mostplayed.ToolTip"));
            this.linkLabel_mostplayed.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_favoriteGame_LinkClicked);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // linkLabel_highRated
            // 
            resources.ApplyResources(this.linkLabel_highRated, "linkLabel_highRated");
            this.linkLabel_highRated.Name = "linkLabel_highRated";
            this.linkLabel_highRated.TabStop = true;
            this.toolTip1.SetToolTip(this.linkLabel_highRated, resources.GetString("linkLabel_highRated.ToolTip"));
            this.linkLabel_highRated.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_favoriteGame_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabel_name);
            this.panel1.Controls.Add(this.textBox_tabs);
            this.panel1.Controls.Add(this.textBox_roms_AI);
            this.panel1.Controls.Add(this.textBox_roms_ind);
            this.panel1.Controls.Add(this.textBox_roms);
            this.panel1.Controls.Add(this.textBox_console_play_time);
            this.panel1.Controls.Add(this.textBox_extensions);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // linkLabel_name
            // 
            resources.ApplyResources(this.linkLabel_name, "linkLabel_name");
            this.linkLabel_name.Name = "linkLabel_name";
            this.linkLabel_name.TabStop = true;
            this.toolTip1.SetToolTip(this.linkLabel_name, resources.GetString("linkLabel_name.ToolTip"));
            this.linkLabel_name.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_name_LinkClicked);
            // 
            // textBox_tabs
            // 
            this.textBox_tabs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox_tabs, "textBox_tabs");
            this.textBox_tabs.Name = "textBox_tabs";
            this.textBox_tabs.ReadOnly = true;
            // 
            // textBox_roms_AI
            // 
            this.textBox_roms_AI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox_roms_AI, "textBox_roms_AI");
            this.textBox_roms_AI.Name = "textBox_roms_AI";
            this.textBox_roms_AI.ReadOnly = true;
            // 
            // textBox_roms_ind
            // 
            this.textBox_roms_ind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox_roms_ind, "textBox_roms_ind");
            this.textBox_roms_ind.Name = "textBox_roms_ind";
            this.textBox_roms_ind.ReadOnly = true;
            // 
            // textBox_roms
            // 
            this.textBox_roms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox_roms, "textBox_roms");
            this.textBox_roms.Name = "textBox_roms";
            this.textBox_roms.ReadOnly = true;
            // 
            // textBox_console_play_time
            // 
            this.textBox_console_play_time.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox_console_play_time, "textBox_console_play_time");
            this.textBox_console_play_time.Name = "textBox_console_play_time";
            this.textBox_console_play_time.ReadOnly = true;
            // 
            // textBox_extensions
            // 
            this.textBox_extensions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox_extensions, "textBox_extensions");
            this.textBox_extensions.Name = "textBox_extensions";
            this.textBox_extensions.ReadOnly = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_rtf);
            this.panel2.Controls.Add(this.label_manual);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label_favoriteGame);
            this.panel2.Controls.Add(this.rating1);
            this.panel2.Controls.Add(this.label_most_played);
            this.panel2.Controls.Add(this.linkLabel_highRated);
            this.panel2.Controls.Add(this.linkLabel_favoriteGame);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.linkLabel_mostplayed);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // label_rtf
            // 
            resources.ApplyResources(this.label_rtf, "label_rtf");
            this.label_rtf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_rtf.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.File_RTF;
            this.label_rtf.Name = "label_rtf";
            this.toolTip1.SetToolTip(this.label_rtf, resources.GetString("label_rtf.ToolTip"));
            this.label_rtf.Click += new System.EventHandler(this.label_rtf_Click);
            // 
            // label_manual
            // 
            resources.ApplyResources(this.label_manual, "label_manual");
            this.label_manual.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_manual.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.File_PDF;
            this.label_manual.Name = "label_manual";
            this.toolTip1.SetToolTip(this.label_manual, resources.GetString("label_manual.ToolTip"));
            this.label_manual.Click += new System.EventHandler(this.label_manual_Click);
            // 
            // rating1
            // 
            resources.ApplyResources(this.rating1, "rating1");
            this.rating1.Name = "rating1";
            this.rating1.rating = 0;
            this.toolTip1.SetToolTip(this.rating1, resources.GetString("rating1.ToolTip"));
            // 
            // ConsoleInfoControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ConsoleInfoControl";
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Core.ImagePanel imagePanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_favoriteGame;
        private System.Windows.Forms.Label label_most_played;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel_favoriteGame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel_mostplayed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel_highRated;
        private Rating rating1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_tabs;
        private System.Windows.Forms.TextBox textBox_roms_AI;
        private System.Windows.Forms.TextBox textBox_roms_ind;
        private System.Windows.Forms.TextBox textBox_roms;
        private System.Windows.Forms.TextBox textBox_console_play_time;
        private System.Windows.Forms.TextBox textBox_extensions;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel linkLabel_name;
        private System.Windows.Forms.Label label_manual;
        private System.Windows.Forms.Label label_rtf;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
