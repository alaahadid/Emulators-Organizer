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
    partial class Form_DetectRomIcons
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DetectRomIcons));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_extensions = new System.Windows.Forms.TextBox();
            this.checkBox_includeSubFolders = new System.Windows.Forms.CheckBox();
            this.checkBox_matchCase = new System.Windows.Forms.CheckBox();
            this.checkBox_matchWord = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_status = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBox_dontAllowMoreThanOneFile = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.checkBox_useRomNameInstead = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_reverse = new System.Windows.Forms.CheckBox();
            this.radioButton_searchmode_rominfile = new System.Windows.Forms.RadioButton();
            this.radioButton_searchmode_fileinrom = new System.Windows.Forms.RadioButton();
            this.radioButton_searchmode_both = new System.Windows.Forms.RadioButton();
            this.radioButton_startWith = new System.Windows.Forms.RadioButton();
            this.radioButton_endwith = new System.Windows.Forms.RadioButton();
            this.radioButton_contains = new System.Windows.Forms.RadioButton();
            this.checkBox_removeSymbols = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.toolTip1.SetToolTip(this.button4, resources.GetString("button4.ToolTip"));
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_extensions
            // 
            resources.ApplyResources(this.textBox_extensions, "textBox_extensions");
            this.textBox_extensions.Name = "textBox_extensions";
            this.toolTip1.SetToolTip(this.textBox_extensions, resources.GetString("textBox_extensions.ToolTip"));
            // 
            // checkBox_includeSubFolders
            // 
            resources.ApplyResources(this.checkBox_includeSubFolders, "checkBox_includeSubFolders");
            this.checkBox_includeSubFolders.Name = "checkBox_includeSubFolders";
            this.toolTip1.SetToolTip(this.checkBox_includeSubFolders, resources.GetString("checkBox_includeSubFolders.ToolTip"));
            this.checkBox_includeSubFolders.UseVisualStyleBackColor = true;
            // 
            // checkBox_matchCase
            // 
            resources.ApplyResources(this.checkBox_matchCase, "checkBox_matchCase");
            this.checkBox_matchCase.Name = "checkBox_matchCase";
            this.toolTip1.SetToolTip(this.checkBox_matchCase, resources.GetString("checkBox_matchCase.ToolTip"));
            this.checkBox_matchCase.UseVisualStyleBackColor = true;
            // 
            // checkBox_matchWord
            // 
            resources.ApplyResources(this.checkBox_matchWord, "checkBox_matchWord");
            this.checkBox_matchWord.Name = "checkBox_matchWord";
            this.toolTip1.SetToolTip(this.checkBox_matchWord, resources.GetString("checkBox_matchWord.ToolTip"));
            this.checkBox_matchWord.UseVisualStyleBackColor = true;
            this.checkBox_matchWord.CheckedChanged += new System.EventHandler(this.checkBox_matchWord_CheckedChanged);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // label_status
            // 
            resources.ApplyResources(this.label_status, "label_status");
            this.label_status.Name = "label_status";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // checkBox_dontAllowMoreThanOneFile
            // 
            resources.ApplyResources(this.checkBox_dontAllowMoreThanOneFile, "checkBox_dontAllowMoreThanOneFile");
            this.checkBox_dontAllowMoreThanOneFile.Checked = true;
            this.checkBox_dontAllowMoreThanOneFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_dontAllowMoreThanOneFile.Name = "checkBox_dontAllowMoreThanOneFile";
            this.toolTip1.SetToolTip(this.checkBox_dontAllowMoreThanOneFile, resources.GetString("checkBox_dontAllowMoreThanOneFile.ToolTip"));
            this.checkBox_dontAllowMoreThanOneFile.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.toolTip1.SetToolTip(this.button5, resources.GetString("button5.ToolTip"));
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // checkBox_useRomNameInstead
            // 
            resources.ApplyResources(this.checkBox_useRomNameInstead, "checkBox_useRomNameInstead");
            this.checkBox_useRomNameInstead.Name = "checkBox_useRomNameInstead";
            this.toolTip1.SetToolTip(this.checkBox_useRomNameInstead, resources.GetString("checkBox_useRomNameInstead.ToolTip"));
            this.checkBox_useRomNameInstead.UseVisualStyleBackColor = true;
            // 
            // checkBox_reverse
            // 
            resources.ApplyResources(this.checkBox_reverse, "checkBox_reverse");
            this.checkBox_reverse.Name = "checkBox_reverse";
            this.toolTip1.SetToolTip(this.checkBox_reverse, resources.GetString("checkBox_reverse.ToolTip"));
            this.checkBox_reverse.UseVisualStyleBackColor = true;
            // 
            // radioButton_searchmode_rominfile
            // 
            resources.ApplyResources(this.radioButton_searchmode_rominfile, "radioButton_searchmode_rominfile");
            this.radioButton_searchmode_rominfile.Checked = true;
            this.radioButton_searchmode_rominfile.Name = "radioButton_searchmode_rominfile";
            this.radioButton_searchmode_rominfile.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_searchmode_rominfile, resources.GetString("radioButton_searchmode_rominfile.ToolTip"));
            this.radioButton_searchmode_rominfile.UseVisualStyleBackColor = true;
            // 
            // radioButton_searchmode_fileinrom
            // 
            resources.ApplyResources(this.radioButton_searchmode_fileinrom, "radioButton_searchmode_fileinrom");
            this.radioButton_searchmode_fileinrom.Name = "radioButton_searchmode_fileinrom";
            this.toolTip1.SetToolTip(this.radioButton_searchmode_fileinrom, resources.GetString("radioButton_searchmode_fileinrom.ToolTip"));
            this.radioButton_searchmode_fileinrom.UseVisualStyleBackColor = true;
            // 
            // radioButton_searchmode_both
            // 
            resources.ApplyResources(this.radioButton_searchmode_both, "radioButton_searchmode_both");
            this.radioButton_searchmode_both.Name = "radioButton_searchmode_both";
            this.toolTip1.SetToolTip(this.radioButton_searchmode_both, resources.GetString("radioButton_searchmode_both.ToolTip"));
            this.radioButton_searchmode_both.UseVisualStyleBackColor = true;
            // 
            // radioButton_startWith
            // 
            resources.ApplyResources(this.radioButton_startWith, "radioButton_startWith");
            this.radioButton_startWith.Checked = true;
            this.radioButton_startWith.Name = "radioButton_startWith";
            this.radioButton_startWith.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_startWith, resources.GetString("radioButton_startWith.ToolTip"));
            this.radioButton_startWith.UseVisualStyleBackColor = true;
            // 
            // radioButton_endwith
            // 
            resources.ApplyResources(this.radioButton_endwith, "radioButton_endwith");
            this.radioButton_endwith.Name = "radioButton_endwith";
            this.toolTip1.SetToolTip(this.radioButton_endwith, resources.GetString("radioButton_endwith.ToolTip"));
            this.radioButton_endwith.UseVisualStyleBackColor = true;
            // 
            // radioButton_contains
            // 
            resources.ApplyResources(this.radioButton_contains, "radioButton_contains");
            this.radioButton_contains.Name = "radioButton_contains";
            this.toolTip1.SetToolTip(this.radioButton_contains, resources.GetString("radioButton_contains.ToolTip"));
            this.radioButton_contains.UseVisualStyleBackColor = true;
            // 
            // checkBox_removeSymbols
            // 
            resources.ApplyResources(this.checkBox_removeSymbols, "checkBox_removeSymbols");
            this.checkBox_removeSymbols.Name = "checkBox_removeSymbols";
            this.toolTip1.SetToolTip(this.checkBox_removeSymbols, resources.GetString("checkBox_removeSymbols.ToolTip"));
            this.checkBox_removeSymbols.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_searchmode_rominfile);
            this.groupBox1.Controls.Add(this.radioButton_searchmode_both);
            this.groupBox1.Controls.Add(this.radioButton_searchmode_fileinrom);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_startWith);
            this.groupBox2.Controls.Add(this.radioButton_endwith);
            this.groupBox2.Controls.Add(this.radioButton_contains);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // Form_DetectRomIcons
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_removeSymbols);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox_reverse);
            this.Controls.Add(this.checkBox_useRomNameInstead);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.checkBox_dontAllowMoreThanOneFile);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.checkBox_matchWord);
            this.Controls.Add(this.checkBox_matchCase);
            this.Controls.Add(this.checkBox_includeSubFolders);
            this.Controls.Add(this.textBox_extensions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_DetectRomIcons";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Detect_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_extensions;
        private System.Windows.Forms.CheckBox checkBox_includeSubFolders;
        private System.Windows.Forms.CheckBox checkBox_matchCase;
        private System.Windows.Forms.CheckBox checkBox_matchWord;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox_dontAllowMoreThanOneFile;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox checkBox_useRomNameInstead;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_reverse;
        private System.Windows.Forms.RadioButton radioButton_searchmode_rominfile;
        private System.Windows.Forms.RadioButton radioButton_searchmode_fileinrom;
        private System.Windows.Forms.RadioButton radioButton_searchmode_both;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_startWith;
        private System.Windows.Forms.RadioButton radioButton_endwith;
        private System.Windows.Forms.RadioButton radioButton_contains;
        private System.Windows.Forms.CheckBox checkBox_removeSymbols;
    }
}