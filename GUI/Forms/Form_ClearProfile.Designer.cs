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
    partial class Form_ClearProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ClearProfile));
            this.checkBox_deleteMissingRomFiles = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_dontDeleteRomIfHaveRelatedFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_deleteRelatedFilesFirst = new System.Windows.Forms.CheckBox();
            this.checkBox_removeUnneededAssignments = new System.Windows.Forms.CheckBox();
            this.checkBox_removeUnusedEmulators = new System.Windows.Forms.CheckBox();
            this.checkBox_removeEmptyPlaylists = new System.Windows.Forms.CheckBox();
            this.checkBox_removeEmptyConsoles = new System.Windows.Forms.CheckBox();
            this.checkBox_removeEmptyConsoleGroups = new System.Windows.Forms.CheckBox();
            this.checkBox_removePlaylistGroups = new System.Windows.Forms.CheckBox();
            this.checkBox_clearLogsFolder = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_status = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_deleteMissingRomFiles
            // 
            resources.ApplyResources(this.checkBox_deleteMissingRomFiles, "checkBox_deleteMissingRomFiles");
            this.checkBox_deleteMissingRomFiles.Checked = true;
            this.checkBox_deleteMissingRomFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_deleteMissingRomFiles.Name = "checkBox_deleteMissingRomFiles";
            this.toolTip1.SetToolTip(this.checkBox_deleteMissingRomFiles, resources.GetString("checkBox_deleteMissingRomFiles.ToolTip"));
            this.checkBox_deleteMissingRomFiles.UseVisualStyleBackColor = true;
            this.checkBox_deleteMissingRomFiles.CheckedChanged += new System.EventHandler(this.checkBox_deleteMissingRomFiles_CheckedChanged);
            // 
            // checkBox_dontDeleteRomIfHaveRelatedFiles
            // 
            resources.ApplyResources(this.checkBox_dontDeleteRomIfHaveRelatedFiles, "checkBox_dontDeleteRomIfHaveRelatedFiles");
            this.checkBox_dontDeleteRomIfHaveRelatedFiles.Checked = true;
            this.checkBox_dontDeleteRomIfHaveRelatedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_dontDeleteRomIfHaveRelatedFiles.Name = "checkBox_dontDeleteRomIfHaveRelatedFiles";
            this.toolTip1.SetToolTip(this.checkBox_dontDeleteRomIfHaveRelatedFiles, resources.GetString("checkBox_dontDeleteRomIfHaveRelatedFiles.ToolTip"));
            this.checkBox_dontDeleteRomIfHaveRelatedFiles.UseVisualStyleBackColor = true;
            this.checkBox_dontDeleteRomIfHaveRelatedFiles.CheckedChanged += new System.EventHandler(this.checkBox_dontDeleteRomIfHaveRelatedFiles_CheckedChanged);
            // 
            // checkBox_deleteRelatedFilesFirst
            // 
            resources.ApplyResources(this.checkBox_deleteRelatedFilesFirst, "checkBox_deleteRelatedFilesFirst");
            this.checkBox_deleteRelatedFilesFirst.Name = "checkBox_deleteRelatedFilesFirst";
            this.toolTip1.SetToolTip(this.checkBox_deleteRelatedFilesFirst, resources.GetString("checkBox_deleteRelatedFilesFirst.ToolTip"));
            this.checkBox_deleteRelatedFilesFirst.UseVisualStyleBackColor = true;
            // 
            // checkBox_removeUnneededAssignments
            // 
            resources.ApplyResources(this.checkBox_removeUnneededAssignments, "checkBox_removeUnneededAssignments");
            this.checkBox_removeUnneededAssignments.Checked = true;
            this.checkBox_removeUnneededAssignments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_removeUnneededAssignments.Name = "checkBox_removeUnneededAssignments";
            this.toolTip1.SetToolTip(this.checkBox_removeUnneededAssignments, resources.GetString("checkBox_removeUnneededAssignments.ToolTip"));
            this.checkBox_removeUnneededAssignments.UseVisualStyleBackColor = true;
            // 
            // checkBox_removeUnusedEmulators
            // 
            resources.ApplyResources(this.checkBox_removeUnusedEmulators, "checkBox_removeUnusedEmulators");
            this.checkBox_removeUnusedEmulators.Checked = true;
            this.checkBox_removeUnusedEmulators.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_removeUnusedEmulators.Name = "checkBox_removeUnusedEmulators";
            this.toolTip1.SetToolTip(this.checkBox_removeUnusedEmulators, resources.GetString("checkBox_removeUnusedEmulators.ToolTip"));
            this.checkBox_removeUnusedEmulators.UseVisualStyleBackColor = true;
            // 
            // checkBox_removeEmptyPlaylists
            // 
            resources.ApplyResources(this.checkBox_removeEmptyPlaylists, "checkBox_removeEmptyPlaylists");
            this.checkBox_removeEmptyPlaylists.Checked = true;
            this.checkBox_removeEmptyPlaylists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_removeEmptyPlaylists.Name = "checkBox_removeEmptyPlaylists";
            this.toolTip1.SetToolTip(this.checkBox_removeEmptyPlaylists, resources.GetString("checkBox_removeEmptyPlaylists.ToolTip"));
            this.checkBox_removeEmptyPlaylists.UseVisualStyleBackColor = true;
            // 
            // checkBox_removeEmptyConsoles
            // 
            resources.ApplyResources(this.checkBox_removeEmptyConsoles, "checkBox_removeEmptyConsoles");
            this.checkBox_removeEmptyConsoles.Name = "checkBox_removeEmptyConsoles";
            this.toolTip1.SetToolTip(this.checkBox_removeEmptyConsoles, resources.GetString("checkBox_removeEmptyConsoles.ToolTip"));
            this.checkBox_removeEmptyConsoles.UseVisualStyleBackColor = true;
            // 
            // checkBox_removeEmptyConsoleGroups
            // 
            resources.ApplyResources(this.checkBox_removeEmptyConsoleGroups, "checkBox_removeEmptyConsoleGroups");
            this.checkBox_removeEmptyConsoleGroups.Name = "checkBox_removeEmptyConsoleGroups";
            this.toolTip1.SetToolTip(this.checkBox_removeEmptyConsoleGroups, resources.GetString("checkBox_removeEmptyConsoleGroups.ToolTip"));
            this.checkBox_removeEmptyConsoleGroups.UseVisualStyleBackColor = true;
            // 
            // checkBox_removePlaylistGroups
            // 
            resources.ApplyResources(this.checkBox_removePlaylistGroups, "checkBox_removePlaylistGroups");
            this.checkBox_removePlaylistGroups.Name = "checkBox_removePlaylistGroups";
            this.toolTip1.SetToolTip(this.checkBox_removePlaylistGroups, resources.GetString("checkBox_removePlaylistGroups.ToolTip"));
            this.checkBox_removePlaylistGroups.UseVisualStyleBackColor = true;
            // 
            // checkBox_clearLogsFolder
            // 
            resources.ApplyResources(this.checkBox_clearLogsFolder, "checkBox_clearLogsFolder");
            this.checkBox_clearLogsFolder.Checked = true;
            this.checkBox_clearLogsFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_clearLogsFolder.Name = "checkBox_clearLogsFolder";
            this.toolTip1.SetToolTip(this.checkBox_clearLogsFolder, resources.GetString("checkBox_clearLogsFolder.ToolTip"));
            this.checkBox_clearLogsFolder.UseVisualStyleBackColor = true;
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
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_clearLogsFolder);
            this.groupBox1.Controls.Add(this.checkBox_deleteMissingRomFiles);
            this.groupBox1.Controls.Add(this.checkBox_dontDeleteRomIfHaveRelatedFiles);
            this.groupBox1.Controls.Add(this.checkBox_deleteRelatedFilesFirst);
            this.groupBox1.Controls.Add(this.checkBox_removeUnneededAssignments);
            this.groupBox1.Controls.Add(this.checkBox_removeUnusedEmulators);
            this.groupBox1.Controls.Add(this.checkBox_removeEmptyPlaylists);
            this.groupBox1.Controls.Add(this.checkBox_removeEmptyConsoles);
            this.groupBox1.Controls.Add(this.checkBox_removeEmptyConsoleGroups);
            this.groupBox1.Controls.Add(this.checkBox_removePlaylistGroups);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_status);
            this.groupBox2.Controls.Add(this.progressBar1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // Form_ClearProfile
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ClearProfile";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_ClearProfile_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_deleteMissingRomFiles;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_dontDeleteRomIfHaveRelatedFiles;
        private System.Windows.Forms.CheckBox checkBox_deleteRelatedFilesFirst;
        private System.Windows.Forms.CheckBox checkBox_removeUnneededAssignments;
        private System.Windows.Forms.CheckBox checkBox_removeUnusedEmulators;
        private System.Windows.Forms.CheckBox checkBox_removeEmptyPlaylists;
        private System.Windows.Forms.CheckBox checkBox_removeEmptyConsoles;
        private System.Windows.Forms.CheckBox checkBox_removeEmptyConsoleGroups;
        private System.Windows.Forms.CheckBox checkBox_removePlaylistGroups;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_clearLogsFolder;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}