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
    partial class Form_AddRomsFolderScan
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AddRomsFolderScan));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_includeSubfolders = new System.Windows.Forms.CheckBox();
            this.button_start = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBox_clearRomsCollectionFirst = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_use_ai_path = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_archive_extensions = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBox_useParent = new System.Windows.Forms.CheckBox();
            this.checkBox_alwaysChooseChild = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton_ignore_rom = new System.Windows.Forms.RadioButton();
            this.radioButton_replace_rom = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Name = "listView1";
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.toolTip1.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox_includeSubfolders
            // 
            resources.ApplyResources(this.checkBox_includeSubfolders, "checkBox_includeSubfolders");
            this.checkBox_includeSubfolders.Name = "checkBox_includeSubfolders";
            this.toolTip1.SetToolTip(this.checkBox_includeSubfolders, resources.GetString("checkBox_includeSubfolders.ToolTip"));
            this.checkBox_includeSubfolders.UseVisualStyleBackColor = true;
            // 
            // button_start
            // 
            resources.ApplyResources(this.button_start, "button_start");
            this.button_start.Name = "button_start";
            this.toolTip1.SetToolTip(this.button_start, resources.GetString("button_start.ToolTip"));
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.toolTip1.SetToolTip(this.button4, resources.GetString("button4.ToolTip"));
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // checkBox_clearRomsCollectionFirst
            // 
            resources.ApplyResources(this.checkBox_clearRomsCollectionFirst, "checkBox_clearRomsCollectionFirst");
            this.checkBox_clearRomsCollectionFirst.Name = "checkBox_clearRomsCollectionFirst";
            this.toolTip1.SetToolTip(this.checkBox_clearRomsCollectionFirst, resources.GetString("checkBox_clearRomsCollectionFirst.ToolTip"));
            this.checkBox_clearRomsCollectionFirst.UseVisualStyleBackColor = true;
            // 
            // checkBox_use_ai_path
            // 
            resources.ApplyResources(this.checkBox_use_ai_path, "checkBox_use_ai_path");
            this.checkBox_use_ai_path.Name = "checkBox_use_ai_path";
            this.toolTip1.SetToolTip(this.checkBox_use_ai_path, resources.GetString("checkBox_use_ai_path.ToolTip"));
            this.checkBox_use_ai_path.UseVisualStyleBackColor = true;
            this.checkBox_use_ai_path.CheckedChanged += new System.EventHandler(this.checkBox_use_ai_path_CheckedChanged);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBox_archive_extensions
            // 
            resources.ApplyResources(this.textBox_archive_extensions, "textBox_archive_extensions");
            this.textBox_archive_extensions.Name = "textBox_archive_extensions";
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBox_useParent
            // 
            resources.ApplyResources(this.checkBox_useParent, "checkBox_useParent");
            this.checkBox_useParent.Checked = true;
            this.checkBox_useParent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_useParent.Name = "checkBox_useParent";
            this.checkBox_useParent.UseVisualStyleBackColor = true;
            // 
            // checkBox_alwaysChooseChild
            // 
            resources.ApplyResources(this.checkBox_alwaysChooseChild, "checkBox_alwaysChooseChild");
            this.checkBox_alwaysChooseChild.Name = "checkBox_alwaysChooseChild";
            this.checkBox_alwaysChooseChild.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // radioButton_ignore_rom
            // 
            resources.ApplyResources(this.radioButton_ignore_rom, "radioButton_ignore_rom");
            this.radioButton_ignore_rom.Checked = true;
            this.radioButton_ignore_rom.Name = "radioButton_ignore_rom";
            this.radioButton_ignore_rom.UseVisualStyleBackColor = true;
            // 
            // radioButton_replace_rom
            // 
            resources.ApplyResources(this.radioButton_replace_rom, "radioButton_replace_rom");
            this.radioButton_replace_rom.Name = "radioButton_replace_rom";
            this.radioButton_replace_rom.UseVisualStyleBackColor = true;
            // 
            // Form_AddRomsFolderScan
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioButton_replace_rom);
            this.Controls.Add(this.radioButton_ignore_rom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox_alwaysChooseChild);
            this.Controls.Add(this.checkBox_useParent);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.textBox_archive_extensions);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBox_use_ai_path);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.checkBox_clearRomsCollectionFirst);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.checkBox_includeSubfolders);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_AddRomsFolderScan";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_AddRomsFolderScan_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_includeSubfolders;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.CheckBox checkBox_clearRomsCollectionFirst;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox_use_ai_path;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_archive_extensions;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkBox_useParent;
        private System.Windows.Forms.CheckBox checkBox_alwaysChooseChild;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton_ignore_rom;
        private System.Windows.Forms.RadioButton radioButton_replace_rom;
    }
}