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
using System;
using System.Runtime.InteropServices;
namespace EmulatorsOrganizer.GUI
{
    partial class ICMedia
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
            DisposeMediaPlayer();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ICMedia));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_showList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.StatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_autoStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_autoHideToolstrip = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_repeat_list = new System.Windows.Forms.ToolStripButton();
            this.managedListView1 = new MLV.ManagedListView();
            this.playPauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.addMoreFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.editListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.autoStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoHideToolstripToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.showMediaControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolsbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripSeparator2,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripSeparator4,
            this.toolStripButton_showList,
            this.toolStripSeparator3,
            this.toolStripButton6,
            this.toolStripButton7,
            this.StatusLabel,
            this.toolStripSeparator5,
            this.toolStripButton_autoStart,
            this.toolStripButton_autoHideToolstrip,
            this.toolStripButton_repeat_list});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.add;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.cross;
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.application_view_list;
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.control_play;
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder;
            resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripButton_showList
            // 
            this.toolStripButton_showList.CheckOnClick = true;
            this.toolStripButton_showList.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.application_view_columns;
            resources.ApplyResources(this.toolStripButton_showList, "toolStripButton_showList");
            this.toolStripButton_showList.Name = "toolStripButton_showList";
            this.toolStripButton_showList.CheckedChanged += new System.EventHandler(this.toolStripButton_showList_CheckedChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_left;
            resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_right;
            resources.ApplyResources(this.toolStripButton7, "toolStripButton7");
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // toolStripButton_autoStart
            // 
            this.toolStripButton_autoStart.Checked = true;
            this.toolStripButton_autoStart.CheckOnClick = true;
            this.toolStripButton_autoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_autoStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_autoStart.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.control_play_blue;
            resources.ApplyResources(this.toolStripButton_autoStart, "toolStripButton_autoStart");
            this.toolStripButton_autoStart.Name = "toolStripButton_autoStart";
            // 
            // toolStripButton_autoHideToolstrip
            // 
            this.toolStripButton_autoHideToolstrip.Checked = true;
            this.toolStripButton_autoHideToolstrip.CheckOnClick = true;
            this.toolStripButton_autoHideToolstrip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_autoHideToolstrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButton_autoHideToolstrip, "toolStripButton_autoHideToolstrip");
            this.toolStripButton_autoHideToolstrip.Name = "toolStripButton_autoHideToolstrip";
            // 
            // toolStripButton_repeat_list
            // 
            this.toolStripButton_repeat_list.Checked = true;
            this.toolStripButton_repeat_list.CheckOnClick = true;
            this.toolStripButton_repeat_list.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_repeat_list.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButton_repeat_list, "toolStripButton_repeat_list");
            this.toolStripButton_repeat_list.Name = "toolStripButton_repeat_list";
            this.toolStripButton_repeat_list.CheckedChanged += new System.EventHandler(this.toolStripButton8_CheckedChanged);
            // 
            // managedListView1
            // 
            this.managedListView1.AllowColumnsReorder = false;
            this.managedListView1.AllowDrop = true;
            this.managedListView1.AllowItemsDragAndDrop = true;
            this.managedListView1.AutoSetWheelScrollSpeed = true;
            this.managedListView1.BackgroundRenderMode = MLV.ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio;
            this.managedListView1.ChangeColumnSortModeWhenClick = false;
            this.managedListView1.ColumnClickColor = System.Drawing.Color.PaleVioletRed;
            this.managedListView1.ColumnColor = System.Drawing.Color.Silver;
            this.managedListView1.ColumnHighlightColor = System.Drawing.Color.LightSkyBlue;
            resources.ApplyResources(this.managedListView1, "managedListView1");
            this.managedListView1.DrawHighlight = true;
            this.managedListView1.ItemHighlightColor = System.Drawing.Color.LightSkyBlue;
            this.managedListView1.ItemMouseOverColor = System.Drawing.Color.LightGray;
            this.managedListView1.ItemSpecialColor = System.Drawing.Color.YellowGreen;
            this.managedListView1.Name = "managedListView1";
            this.managedListView1.ShowItemInfoOnThumbnailMode = true;
            this.managedListView1.ShowSubItemToolTip = true;
            this.managedListView1.StretchThumbnailsToFit = false;
            this.managedListView1.ThunmbnailsHeight = 36;
            this.managedListView1.ThunmbnailsWidth = 36;
            this.managedListView1.ViewMode = MLV.ManagedListViewViewMode.Details;
            this.managedListView1.WheelScrollSpeed = 18;
            this.managedListView1.DrawSubItem += new System.EventHandler<MLV.ManagedListViewSubItemDrawArgs>(this.managedListView1_DrawSubItem);
            this.managedListView1.ItemDoubleClick += new System.EventHandler<MLV.ManagedListViewItemDoubleClickArgs>(this.managedListView1_ItemDoubleClick);
            this.managedListView1.AfterColumnResize += new System.EventHandler(this.managedListView1_AfterColumnResize);
            this.managedListView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragDrop);
            this.managedListView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragEnter);
            this.managedListView1.DragOver += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragOver);
            this.managedListView1.DragLeave += new System.EventHandler(this.managedListView1_DragLeave);
            // 
            // playPauseToolStripMenuItem
            // 
            this.playPauseToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.control_play;
            this.playPauseToolStripMenuItem.Name = "playPauseToolStripMenuItem";
            resources.ApplyResources(this.playPauseToolStripMenuItem, "playPauseToolStripMenuItem");
            this.playPauseToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // addMoreFilesToolStripMenuItem
            // 
            this.addMoreFilesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.add;
            this.addMoreFilesToolStripMenuItem.Name = "addMoreFilesToolStripMenuItem";
            resources.ApplyResources(this.addMoreFilesToolStripMenuItem, "addMoreFilesToolStripMenuItem");
            this.addMoreFilesToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // deleteSelectedToolStripMenuItem
            // 
            this.deleteSelectedToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.cross;
            this.deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
            resources.ApplyResources(this.deleteSelectedToolStripMenuItem, "deleteSelectedToolStripMenuItem");
            this.deleteSelectedToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playPauseToolStripMenuItem,
            this.toolStripSeparator10,
            this.nextToolStripMenuItem,
            this.previousToolStripMenuItem,
            this.toolStripSeparator6,
            this.addMoreFilesToolStripMenuItem,
            this.deleteSelectedToolStripMenuItem,
            this.toolStripSeparator7,
            this.editListToolStripMenuItem,
            this.toolStripSeparator8,
            this.openFileLocationToolStripMenuItem,
            this.toolStripSeparator9,
            this.autoStartToolStripMenuItem,
            this.autoHideToolstripToolStripMenuItem,
            this.toolStripSeparator11,
            this.showMediaControlsToolStripMenuItem,
            this.showListToolStripMenuItem,
            this.showToolsbarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_right;
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            resources.ApplyResources(this.nextToolStripMenuItem, "nextToolStripMenuItem");
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // previousToolStripMenuItem
            // 
            this.previousToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_left;
            this.previousToolStripMenuItem.Name = "previousToolStripMenuItem";
            resources.ApplyResources(this.previousToolStripMenuItem, "previousToolStripMenuItem");
            this.previousToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // editListToolStripMenuItem
            // 
            this.editListToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.application_view_list;
            this.editListToolStripMenuItem.Name = "editListToolStripMenuItem";
            resources.ApplyResources(this.editListToolStripMenuItem, "editListToolStripMenuItem");
            this.editListToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // openFileLocationToolStripMenuItem
            // 
            this.openFileLocationToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder;
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            resources.ApplyResources(this.openFileLocationToolStripMenuItem, "openFileLocationToolStripMenuItem");
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // autoStartToolStripMenuItem
            // 
            this.autoStartToolStripMenuItem.Name = "autoStartToolStripMenuItem";
            resources.ApplyResources(this.autoStartToolStripMenuItem, "autoStartToolStripMenuItem");
            this.autoStartToolStripMenuItem.Click += new System.EventHandler(this.autoStartToolStripMenuItem_Click);
            // 
            // autoHideToolstripToolStripMenuItem
            // 
            this.autoHideToolstripToolStripMenuItem.Name = "autoHideToolstripToolStripMenuItem";
            resources.ApplyResources(this.autoHideToolstripToolStripMenuItem, "autoHideToolstripToolStripMenuItem");
            this.autoHideToolstripToolStripMenuItem.Click += new System.EventHandler(this.autoHideToolstripToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // showMediaControlsToolStripMenuItem
            // 
            this.showMediaControlsToolStripMenuItem.Name = "showMediaControlsToolStripMenuItem";
            resources.ApplyResources(this.showMediaControlsToolStripMenuItem, "showMediaControlsToolStripMenuItem");
            this.showMediaControlsToolStripMenuItem.Click += new System.EventHandler(this.showMediaControlsToolStripMenuItem_Click);
            // 
            // showListToolStripMenuItem
            // 
            this.showListToolStripMenuItem.Name = "showListToolStripMenuItem";
            resources.ApplyResources(this.showListToolStripMenuItem, "showListToolStripMenuItem");
            this.showListToolStripMenuItem.Click += new System.EventHandler(this.showListToolStripMenuItem_Click);
            // 
            // showToolsbarToolStripMenuItem
            // 
            this.showToolsbarToolStripMenuItem.Name = "showToolsbarToolStripMenuItem";
            resources.ApplyResources(this.showToolsbarToolStripMenuItem, "showToolsbarToolStripMenuItem");
            this.showToolsbarToolStripMenuItem.Click += new System.EventHandler(this.showToolsbarToolStripMenuItem_Click);
            // 
            // ICMedia
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.managedListView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ICMedia";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton_showList;
        private System.Windows.Forms.ToolStripButton toolStripButton_autoStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private MLV.ManagedListView managedListView1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton_autoHideToolstrip;
        private System.Windows.Forms.ToolStripMenuItem playPauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem addMoreFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem editListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem showMediaControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolsbarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoHideToolstripToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton toolStripButton_repeat_list;
    }
}
