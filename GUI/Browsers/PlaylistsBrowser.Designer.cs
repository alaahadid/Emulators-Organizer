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
    partial class PlaylistsBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaylistsBrowser));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistsGroupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rootPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.changeIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistGroupsByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rootPlaylistsByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistsByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.cloneSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new EmulatorsOrganizer.GUI.OptimizedTreeview();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackBar_zoom = new System.Windows.Forms.TrackBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.playlistsGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rootPlaylistToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_moveUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_moveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.playlistGroupsByNameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rootPlaylistsByNameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistsByNameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.importPlaylistFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportPlaylistToolStripMenuItem,
            this.toolStripSeparator5,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripSeparator6,
            this.changeIconToolStripMenuItem,
            this.clearIconToolStripMenuItem,
            this.toolStripSeparator7,
            this.sortToolStripMenuItem,
            this.toolStripSeparator8,
            this.cloneSettingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlistsGroupToolStripMenuItem1,
            this.rootPlaylistToolStripMenuItem,
            this.playlistToolStripMenuItem,
            this.importPlaylistFromFileToolStripMenuItem});
            resources.ApplyResources(this.addToolStripMenuItem, "addToolStripMenuItem");
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            // 
            // playlistsGroupToolStripMenuItem1
            // 
            this.playlistsGroupToolStripMenuItem1.Name = "playlistsGroupToolStripMenuItem1";
            resources.ApplyResources(this.playlistsGroupToolStripMenuItem1, "playlistsGroupToolStripMenuItem1");
            this.playlistsGroupToolStripMenuItem1.Click += new System.EventHandler(this.playlistsGroupToolStripMenuItem_Click);
            // 
            // rootPlaylistToolStripMenuItem
            // 
            this.rootPlaylistToolStripMenuItem.Name = "rootPlaylistToolStripMenuItem";
            resources.ApplyResources(this.rootPlaylistToolStripMenuItem, "rootPlaylistToolStripMenuItem");
            this.rootPlaylistToolStripMenuItem.Click += new System.EventHandler(this.rootPlaylistToolStripMenuItem_Click);
            // 
            // playlistToolStripMenuItem
            // 
            this.playlistToolStripMenuItem.Name = "playlistToolStripMenuItem";
            resources.ApplyResources(this.playlistToolStripMenuItem, "playlistToolStripMenuItem");
            this.playlistToolStripMenuItem.Click += new System.EventHandler(this.playlistToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.cross;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.textfield_rename;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            resources.ApplyResources(this.renameToolStripMenuItem, "renameToolStripMenuItem");
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // changeIconToolStripMenuItem
            // 
            this.changeIconToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder_picture;
            this.changeIconToolStripMenuItem.Name = "changeIconToolStripMenuItem";
            resources.ApplyResources(this.changeIconToolStripMenuItem, "changeIconToolStripMenuItem");
            this.changeIconToolStripMenuItem.Click += new System.EventHandler(this.changeIconToolStripMenuItem_Click);
            // 
            // clearIconToolStripMenuItem
            // 
            this.clearIconToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.picture_delete;
            this.clearIconToolStripMenuItem.Name = "clearIconToolStripMenuItem";
            resources.ApplyResources(this.clearIconToolStripMenuItem, "clearIconToolStripMenuItem");
            this.clearIconToolStripMenuItem.Click += new System.EventHandler(this.clearIconToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlistGroupsByNameToolStripMenuItem,
            this.rootPlaylistsByNameToolStripMenuItem,
            this.playlistsByNameToolStripMenuItem});
            this.sortToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.text_linespacing;
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            resources.ApplyResources(this.sortToolStripMenuItem, "sortToolStripMenuItem");
            // 
            // playlistGroupsByNameToolStripMenuItem
            // 
            this.playlistGroupsByNameToolStripMenuItem.Name = "playlistGroupsByNameToolStripMenuItem";
            resources.ApplyResources(this.playlistGroupsByNameToolStripMenuItem, "playlistGroupsByNameToolStripMenuItem");
            this.playlistGroupsByNameToolStripMenuItem.Click += new System.EventHandler(this.playlistGroupsByNameToolStripMenuItem_Click);
            // 
            // rootPlaylistsByNameToolStripMenuItem
            // 
            this.rootPlaylistsByNameToolStripMenuItem.Name = "rootPlaylistsByNameToolStripMenuItem";
            resources.ApplyResources(this.rootPlaylistsByNameToolStripMenuItem, "rootPlaylistsByNameToolStripMenuItem");
            this.rootPlaylistsByNameToolStripMenuItem.Click += new System.EventHandler(this.rootPlaylistsByNameToolStripMenuItem_Click);
            // 
            // playlistsByNameToolStripMenuItem
            // 
            this.playlistsByNameToolStripMenuItem.Name = "playlistsByNameToolStripMenuItem";
            resources.ApplyResources(this.playlistsByNameToolStripMenuItem, "playlistsByNameToolStripMenuItem");
            this.playlistsByNameToolStripMenuItem.Click += new System.EventHandler(this.playlistsByNameToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // cloneSettingsToolStripMenuItem
            // 
            this.cloneSettingsToolStripMenuItem.Name = "cloneSettingsToolStripMenuItem";
            resources.ApplyResources(this.cloneSettingsToolStripMenuItem, "cloneSettingsToolStripMenuItem");
            this.cloneSettingsToolStripMenuItem.Click += new System.EventHandler(this.cloneSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.properties;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            resources.ApplyResources(this.propertiesToolStripMenuItem, "propertiesToolStripMenuItem");
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.BackgroundImageMode = EmulatorsOrganizer.Core.ImageViewMode.Normal;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.LabelEdit = true;
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowLines = false;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_BeforeLabelEdit);
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView1_DrawNode);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.treeView1.Enter += new System.EventHandler(this.PlaylistsBrowser_Enter);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlaylistsBrowser_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.trackBar_zoom);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // trackBar_zoom
            // 
            resources.ApplyResources(this.trackBar_zoom, "trackBar_zoom");
            this.trackBar_zoom.Maximum = 255;
            this.trackBar_zoom.Minimum = 16;
            this.trackBar_zoom.Name = "trackBar_zoom";
            this.trackBar_zoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_zoom.Value = 16;
            this.trackBar_zoom.Scroll += new System.EventHandler(this.trackBar_zoom_Scroll);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripSeparator2,
            this.toolStripButton_moveUp,
            this.toolStripButton_moveDown,
            this.toolStripSeparator4,
            this.toolStripSplitButton2});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlistsGroupToolStripMenuItem,
            this.rootPlaylistToolStripMenuItem1,
            this.playlistToolStripMenuItem1});
            resources.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.playlistsGroupToolStripMenuItem_Click);
            // 
            // playlistsGroupToolStripMenuItem
            // 
            this.playlistsGroupToolStripMenuItem.Name = "playlistsGroupToolStripMenuItem";
            resources.ApplyResources(this.playlistsGroupToolStripMenuItem, "playlistsGroupToolStripMenuItem");
            this.playlistsGroupToolStripMenuItem.Click += new System.EventHandler(this.playlistsGroupToolStripMenuItem_Click);
            // 
            // rootPlaylistToolStripMenuItem1
            // 
            this.rootPlaylistToolStripMenuItem1.Name = "rootPlaylistToolStripMenuItem1";
            resources.ApplyResources(this.rootPlaylistToolStripMenuItem1, "rootPlaylistToolStripMenuItem1");
            this.rootPlaylistToolStripMenuItem1.Click += new System.EventHandler(this.rootPlaylistToolStripMenuItem_Click);
            // 
            // playlistToolStripMenuItem1
            // 
            this.playlistToolStripMenuItem1.Name = "playlistToolStripMenuItem1";
            resources.ApplyResources(this.playlistToolStripMenuItem1, "playlistToolStripMenuItem1");
            this.playlistToolStripMenuItem1.Click += new System.EventHandler(this.playlistToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripButton_moveUp
            // 
            this.toolStripButton_moveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton_moveUp, "toolStripButton_moveUp");
            this.toolStripButton_moveUp.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_up;
            this.toolStripButton_moveUp.Name = "toolStripButton_moveUp";
            this.toolStripButton_moveUp.Click += new System.EventHandler(this.toolStripButton_moveUp_Click);
            // 
            // toolStripButton_moveDown
            // 
            this.toolStripButton_moveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton_moveDown, "toolStripButton_moveDown");
            this.toolStripButton_moveDown.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_down;
            this.toolStripButton_moveDown.Name = "toolStripButton_moveDown";
            this.toolStripButton_moveDown.Click += new System.EventHandler(this.toolStripButton_moveDown_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlistGroupsByNameToolStripMenuItem1,
            this.rootPlaylistsByNameToolStripMenuItem1,
            this.playlistsByNameToolStripMenuItem1});
            this.toolStripSplitButton2.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.text_linespacing;
            resources.ApplyResources(this.toolStripSplitButton2, "toolStripSplitButton2");
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.ButtonClick += new System.EventHandler(this.toolStripSplitButton2_ButtonClick_1);
            // 
            // playlistGroupsByNameToolStripMenuItem1
            // 
            this.playlistGroupsByNameToolStripMenuItem1.Name = "playlistGroupsByNameToolStripMenuItem1";
            resources.ApplyResources(this.playlistGroupsByNameToolStripMenuItem1, "playlistGroupsByNameToolStripMenuItem1");
            this.playlistGroupsByNameToolStripMenuItem1.Click += new System.EventHandler(this.playlistGroupsByNameToolStripMenuItem_Click);
            // 
            // rootPlaylistsByNameToolStripMenuItem1
            // 
            this.rootPlaylistsByNameToolStripMenuItem1.Name = "rootPlaylistsByNameToolStripMenuItem1";
            resources.ApplyResources(this.rootPlaylistsByNameToolStripMenuItem1, "rootPlaylistsByNameToolStripMenuItem1");
            this.rootPlaylistsByNameToolStripMenuItem1.Click += new System.EventHandler(this.rootPlaylistsByNameToolStripMenuItem_Click);
            // 
            // playlistsByNameToolStripMenuItem1
            // 
            this.playlistsByNameToolStripMenuItem1.Name = "playlistsByNameToolStripMenuItem1";
            resources.ApplyResources(this.playlistsByNameToolStripMenuItem1, "playlistsByNameToolStripMenuItem1");
            this.playlistsByNameToolStripMenuItem1.Click += new System.EventHandler(this.playlistsByNameToolStripMenuItem_Click);
            // 
            // exportPlaylistToolStripMenuItem
            // 
            this.exportPlaylistToolStripMenuItem.Name = "exportPlaylistToolStripMenuItem";
            resources.ApplyResources(this.exportPlaylistToolStripMenuItem, "exportPlaylistToolStripMenuItem");
            this.exportPlaylistToolStripMenuItem.Click += new System.EventHandler(this.exportPlaylistToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // importPlaylistFromFileToolStripMenuItem
            // 
            this.importPlaylistFromFileToolStripMenuItem.Name = "importPlaylistFromFileToolStripMenuItem";
            resources.ApplyResources(this.importPlaylistFromFileToolStripMenuItem, "importPlaylistFromFileToolStripMenuItem");
            this.importPlaylistFromFileToolStripMenuItem.Click += new System.EventHandler(this.importPlaylistFromFileToolStripMenuItem_Click);
            // 
            // PlaylistsBrowser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PlaylistsBrowser";
            this.Enter += new System.EventHandler(this.PlaylistsBrowser_Enter);
            this.Leave += new System.EventHandler(this.PlaylistsBrowser_Leave);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlaylistsBrowser_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OptimizedTreeview treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackBar_zoom;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_moveUp;
        private System.Windows.Forms.ToolStripButton toolStripButton_moveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem changeIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playlistsGroupToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem playlistsGroupToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem rootPlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playlistGroupsByNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rootPlaylistsByNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playlistsByNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rootPlaylistToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem playlistToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem playlistGroupsByNameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem rootPlaylistsByNameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem playlistsByNameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cloneSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem importPlaylistFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}
