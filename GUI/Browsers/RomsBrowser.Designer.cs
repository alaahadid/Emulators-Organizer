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
    partial class RomsBrowser
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
            if (reload_thread != null)
            {
                if (reload_thread.IsAlive)
                {
                    reload_thread.Abort();
                    reload_thread = null;
                }
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RomsBrowser));
            this.contextMenuStrip_normal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyRomNamesToFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getNameAndDataFromMobyGamescomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getDataFromTheGamesDBForThisGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.changeIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allCountersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.playTimesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastPlayedTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.sendToPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDataInfoAsCategoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCategoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.clearRelatedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.openLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNextThumbnailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetThumbnailIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel_romSelection = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SelectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip_columns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.managedListView1 = new MLV.ManagedListView();
            this.imageList_listView = new System.Windows.Forms.ImageList(this.components);
            this.panel_thumbnails = new System.Windows.Forms.Panel();
            this.comboBox_thumbsMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar_thumbSize = new System.Windows.Forms.TrackBar();
            this.timer_thumbCycle = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer_selection = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip_normal.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel_thumbnails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_thumbSize)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip_normal
            // 
            this.contextMenuStrip_normal.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip_normal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.applyRomNamesToFilesToolStripMenuItem,
            this.getNameAndDataFromMobyGamescomToolStripMenuItem,
            this.getDataFromTheGamesDBForThisGameToolStripMenuItem,
            this.toolStripSeparator2,
            this.changeIconToolStripMenuItem,
            this.clearIconToolStripMenuItem,
            this.toolStripSeparator3,
            this.resetToolStripMenuItem,
            this.toolStripSeparator8,
            this.sortToolStripMenuItem,
            this.toolStripSeparator6,
            this.sendToPlaylistToolStripMenuItem,
            this.addDataInfoAsCategoriesToolStripMenuItem,
            this.editCategoriesToolStripMenuItem,
            this.exportNamesToolStripMenuItem,
            this.sendToToolStripMenuItem,
            this.toolStripSeparator4,
            this.clearRelatedFilesToolStripMenuItem,
            this.toolStripSeparator7,
            this.openLocationToolStripMenuItem,
            this.showNextThumbnailToolStripMenuItem,
            this.resetThumbnailIndexToolStripMenuItem,
            this.toolStripSeparator5,
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip_normal.Name = "contextMenuStrip_normal";
            resources.ApplyResources(this.contextMenuStrip_normal, "contextMenuStrip_normal");
            this.contextMenuStrip_normal.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_normal_Opening);
            // 
            // playToolStripMenuItem
            // 
            resources.ApplyResources(this.playToolStripMenuItem, "playToolStripMenuItem");
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // deleteToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.cross;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            resources.ApplyResources(this.renameToolStripMenuItem, "renameToolStripMenuItem");
            this.renameToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.textfield_rename;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // applyRomNamesToFilesToolStripMenuItem
            // 
            resources.ApplyResources(this.applyRomNamesToFilesToolStripMenuItem, "applyRomNamesToFilesToolStripMenuItem");
            this.applyRomNamesToFilesToolStripMenuItem.Name = "applyRomNamesToFilesToolStripMenuItem";
            this.applyRomNamesToFilesToolStripMenuItem.Click += new System.EventHandler(this.applyRomNamesToFilesToolStripMenuItem_Click);
            // 
            // getNameAndDataFromMobyGamescomToolStripMenuItem
            // 
            resources.ApplyResources(this.getNameAndDataFromMobyGamescomToolStripMenuItem, "getNameAndDataFromMobyGamescomToolStripMenuItem");
            this.getNameAndDataFromMobyGamescomToolStripMenuItem.Name = "getNameAndDataFromMobyGamescomToolStripMenuItem";
            this.getNameAndDataFromMobyGamescomToolStripMenuItem.Click += new System.EventHandler(this.getNameAndDataFromMobyGamescomToolStripMenuItem_Click);
            // 
            // getDataFromTheGamesDBForThisGameToolStripMenuItem
            // 
            resources.ApplyResources(this.getDataFromTheGamesDBForThisGameToolStripMenuItem, "getDataFromTheGamesDBForThisGameToolStripMenuItem");
            this.getDataFromTheGamesDBForThisGameToolStripMenuItem.Name = "getDataFromTheGamesDBForThisGameToolStripMenuItem";
            this.getDataFromTheGamesDBForThisGameToolStripMenuItem.Click += new System.EventHandler(this.getDataFromTheGamesDBForThisGameToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // changeIconToolStripMenuItem
            // 
            resources.ApplyResources(this.changeIconToolStripMenuItem, "changeIconToolStripMenuItem");
            this.changeIconToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder_picture;
            this.changeIconToolStripMenuItem.Name = "changeIconToolStripMenuItem";
            this.changeIconToolStripMenuItem.Click += new System.EventHandler(this.changeIconToolStripMenuItem_Click);
            // 
            // clearIconToolStripMenuItem
            // 
            resources.ApplyResources(this.clearIconToolStripMenuItem, "clearIconToolStripMenuItem");
            this.clearIconToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.picture_delete;
            this.clearIconToolStripMenuItem.Name = "clearIconToolStripMenuItem";
            this.clearIconToolStripMenuItem.Click += new System.EventHandler(this.clearIconToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allCountersToolStripMenuItem,
            this.toolStripSeparator9,
            this.playTimesToolStripMenuItem,
            this.playTimeToolStripMenuItem,
            this.lastPlayedTimeToolStripMenuItem});
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resources.ApplyResources(this.resetToolStripMenuItem, "resetToolStripMenuItem");
            // 
            // allCountersToolStripMenuItem
            // 
            this.allCountersToolStripMenuItem.Name = "allCountersToolStripMenuItem";
            resources.ApplyResources(this.allCountersToolStripMenuItem, "allCountersToolStripMenuItem");
            this.allCountersToolStripMenuItem.Click += new System.EventHandler(this.allCountersToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // playTimesToolStripMenuItem
            // 
            this.playTimesToolStripMenuItem.Name = "playTimesToolStripMenuItem";
            resources.ApplyResources(this.playTimesToolStripMenuItem, "playTimesToolStripMenuItem");
            this.playTimesToolStripMenuItem.Click += new System.EventHandler(this.playTimesToolStripMenuItem_Click);
            // 
            // playTimeToolStripMenuItem
            // 
            this.playTimeToolStripMenuItem.Name = "playTimeToolStripMenuItem";
            resources.ApplyResources(this.playTimeToolStripMenuItem, "playTimeToolStripMenuItem");
            this.playTimeToolStripMenuItem.Click += new System.EventHandler(this.playTimeToolStripMenuItem_Click);
            // 
            // lastPlayedTimeToolStripMenuItem
            // 
            this.lastPlayedTimeToolStripMenuItem.Name = "lastPlayedTimeToolStripMenuItem";
            resources.ApplyResources(this.lastPlayedTimeToolStripMenuItem, "lastPlayedTimeToolStripMenuItem");
            this.lastPlayedTimeToolStripMenuItem.Click += new System.EventHandler(this.lastPlayedTimeToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            resources.ApplyResources(this.sortToolStripMenuItem, "sortToolStripMenuItem");
            this.sortToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.sortToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // sendToPlaylistToolStripMenuItem
            // 
            this.sendToPlaylistToolStripMenuItem.Name = "sendToPlaylistToolStripMenuItem";
            resources.ApplyResources(this.sendToPlaylistToolStripMenuItem, "sendToPlaylistToolStripMenuItem");
            this.sendToPlaylistToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.sendToPlaylistToolStripMenuItem_DropDownItemClicked);
            // 
            // addDataInfoAsCategoriesToolStripMenuItem
            // 
            resources.ApplyResources(this.addDataInfoAsCategoriesToolStripMenuItem, "addDataInfoAsCategoriesToolStripMenuItem");
            this.addDataInfoAsCategoriesToolStripMenuItem.Name = "addDataInfoAsCategoriesToolStripMenuItem";
            this.addDataInfoAsCategoriesToolStripMenuItem.Click += new System.EventHandler(this.addDataInfoAsCategoriesToolStripMenuItem_Click);
            // 
            // editCategoriesToolStripMenuItem
            // 
            resources.ApplyResources(this.editCategoriesToolStripMenuItem, "editCategoriesToolStripMenuItem");
            this.editCategoriesToolStripMenuItem.Name = "editCategoriesToolStripMenuItem";
            this.editCategoriesToolStripMenuItem.Click += new System.EventHandler(this.editCategoriesToolStripMenuItem_Click);
            // 
            // exportNamesToolStripMenuItem
            // 
            resources.ApplyResources(this.exportNamesToolStripMenuItem, "exportNamesToolStripMenuItem");
            this.exportNamesToolStripMenuItem.Name = "exportNamesToolStripMenuItem";
            this.exportNamesToolStripMenuItem.Click += new System.EventHandler(this.exportNamesToolStripMenuItem_Click);
            // 
            // sendToToolStripMenuItem
            // 
            resources.ApplyResources(this.sendToToolStripMenuItem, "sendToToolStripMenuItem");
            this.sendToToolStripMenuItem.Name = "sendToToolStripMenuItem";
            this.sendToToolStripMenuItem.Click += new System.EventHandler(this.sendToToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // clearRelatedFilesToolStripMenuItem
            // 
            resources.ApplyResources(this.clearRelatedFilesToolStripMenuItem, "clearRelatedFilesToolStripMenuItem");
            this.clearRelatedFilesToolStripMenuItem.Name = "clearRelatedFilesToolStripMenuItem";
            this.clearRelatedFilesToolStripMenuItem.Click += new System.EventHandler(this.clearRelatedFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // openLocationToolStripMenuItem
            // 
            this.openLocationToolStripMenuItem.Name = "openLocationToolStripMenuItem";
            resources.ApplyResources(this.openLocationToolStripMenuItem, "openLocationToolStripMenuItem");
            this.openLocationToolStripMenuItem.Click += new System.EventHandler(this.openLocationToolStripMenuItem_Click);
            // 
            // showNextThumbnailToolStripMenuItem
            // 
            this.showNextThumbnailToolStripMenuItem.Name = "showNextThumbnailToolStripMenuItem";
            resources.ApplyResources(this.showNextThumbnailToolStripMenuItem, "showNextThumbnailToolStripMenuItem");
            this.showNextThumbnailToolStripMenuItem.Click += new System.EventHandler(this.showNextThumbnailToolStripMenuItem_Click);
            // 
            // resetThumbnailIndexToolStripMenuItem
            // 
            this.resetThumbnailIndexToolStripMenuItem.Name = "resetThumbnailIndexToolStripMenuItem";
            resources.ApplyResources(this.resetThumbnailIndexToolStripMenuItem, "resetThumbnailIndexToolStripMenuItem");
            this.resetThumbnailIndexToolStripMenuItem.Click += new System.EventHandler(this.resetThumbnailIndexToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // propertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.propertiesToolStripMenuItem, "propertiesToolStripMenuItem");
            this.propertiesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.properties;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel_romSelection,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1,
            this.SelectionStatus});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.SizingGrip = false;
            // 
            // StatusLabel_romSelection
            // 
            this.StatusLabel_romSelection.Name = "StatusLabel_romSelection";
            resources.ApplyResources(this.StatusLabel_romSelection, "StatusLabel_romSelection");
            // 
            // toolStripStatusLabel2
            // 
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // SelectionStatus
            // 
            this.SelectionStatus.Name = "SelectionStatus";
            resources.ApplyResources(this.SelectionStatus, "SelectionStatus");
            // 
            // contextMenuStrip_columns
            // 
            this.contextMenuStrip_columns.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip_columns.Name = "contextMenuStrip_columns";
            resources.ApplyResources(this.contextMenuStrip_columns, "contextMenuStrip_columns");
            this.contextMenuStrip_columns.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_columns_Opening);
            this.contextMenuStrip_columns.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_columns_ItemClicked);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Console.png");
            this.imageList1.Images.SetKeyName(1, "Cart.png");
            // 
            // managedListView1
            // 
            this.managedListView1.AllowColumnsReorder = true;
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
            this.managedListView1.ImagesList = this.imageList_listView;
            this.managedListView1.ItemHighlightColor = System.Drawing.Color.LightSkyBlue;
            this.managedListView1.ItemMouseOverColor = System.Drawing.Color.LightGray;
            this.managedListView1.ItemSpecialColor = System.Drawing.Color.YellowGreen;
            this.managedListView1.Name = "managedListView1";
            this.managedListView1.ShowItemInfoOnThumbnailMode = true;
            this.managedListView1.ShowSubItemToolTip = true;
            this.managedListView1.StretchThumbnailsToFit = false;
            this.managedListView1.ThunmbnailsHeight = 50;
            this.managedListView1.ThunmbnailsWidth = 50;
            this.managedListView1.ViewMode = MLV.ManagedListViewViewMode.Details;
            this.managedListView1.WheelScrollSpeed = 48;
            this.managedListView1.DrawItem += new System.EventHandler<MLV.ManagedListViewItemDrawArgs>(this.managedListView1_DrawItem);
            this.managedListView1.DrawSubItem += new System.EventHandler<MLV.ManagedListViewSubItemDrawArgs>(this.managedListView1_DrawSubItem);
            this.managedListView1.SelectedIndexChanged += new System.EventHandler(this.managedListView1_SelectedIndexChanged);
            this.managedListView1.ColumnClicked += new System.EventHandler<MLV.ManagedListViewColumnClickArgs>(this.managedListView1_ColumnClicked);
            this.managedListView1.EnterPressed += new System.EventHandler(this.managedListView1_EnterPressed);
            this.managedListView1.ItemDoubleClick += new System.EventHandler<MLV.ManagedListViewItemDoubleClickArgs>(this.managedListView1_ItemDoubleClick);
            this.managedListView1.SwitchToColumnsContextMenu += new System.EventHandler(this.managedListView1_SwitchToColumnsContextMenu);
            this.managedListView1.SwitchToNormalContextMenu += new System.EventHandler(this.managedListView1_SwitchToNormalContextMenu);
            this.managedListView1.AfterColumnResize += new System.EventHandler(this.managedListView1_AfterColumnResize);
            this.managedListView1.ItemsDrag += new System.EventHandler(this.managedListView1_ItemsDrag);
            this.managedListView1.ViewModeChanged += new System.EventHandler(this.managedListView1_ViewModeChanged);
            this.managedListView1.AfterColumnReorder += new System.EventHandler(this.managedListView1_AfterColumnReorder);
            this.managedListView1.FillSubitemsRequest += new System.EventHandler<MLV.ManagedListViewItemSelectArgs>(this.managedListView1_FillSubitemsRequest);
            this.managedListView1.ShowThumbnailInfoRequest += new System.EventHandler<MLV.ManagedListViewShowThumbnailTooltipArgs>(this.managedListView1_ShowThumbnailInfoRequest);
            this.managedListView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragDrop);
            this.managedListView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragEnter);
            this.managedListView1.DragOver += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragOver);
            this.managedListView1.DragLeave += new System.EventHandler(this.managedListView1_DragLeave);
            // 
            // imageList_listView
            // 
            this.imageList_listView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imageList_listView, "imageList_listView");
            this.imageList_listView.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel_thumbnails
            // 
            this.panel_thumbnails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_thumbnails.Controls.Add(this.comboBox_thumbsMode);
            this.panel_thumbnails.Controls.Add(this.label1);
            this.panel_thumbnails.Controls.Add(this.trackBar_thumbSize);
            resources.ApplyResources(this.panel_thumbnails, "panel_thumbnails");
            this.panel_thumbnails.Name = "panel_thumbnails";
            // 
            // comboBox_thumbsMode
            // 
            this.comboBox_thumbsMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_thumbsMode.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_thumbsMode, "comboBox_thumbsMode");
            this.comboBox_thumbsMode.Name = "comboBox_thumbsMode";
            this.comboBox_thumbsMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_thumbsMode_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // trackBar_thumbSize
            // 
            resources.ApplyResources(this.trackBar_thumbSize, "trackBar_thumbSize");
            this.trackBar_thumbSize.Maximum = 500;
            this.trackBar_thumbSize.Minimum = 50;
            this.trackBar_thumbSize.Name = "trackBar_thumbSize";
            this.trackBar_thumbSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_thumbSize.Value = 50;
            this.trackBar_thumbSize.Scroll += new System.EventHandler(this.trackBar_thumbSize_Scroll);
            // 
            // timer_thumbCycle
            // 
            this.timer_thumbCycle.Interval = 2000;
            this.timer_thumbCycle.Tick += new System.EventHandler(this.timer_thumbCycle_Tick);
            // 
            // timer_selection
            // 
            this.timer_selection.Tick += new System.EventHandler(this.timer_selection_Tick);
            // 
            // RomsBrowser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.managedListView1);
            this.Controls.Add(this.panel_thumbnails);
            this.Controls.Add(this.statusStrip1);
            this.Name = "RomsBrowser";
            this.contextMenuStrip_normal.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel_thumbnails.ResumeLayout(false);
            this.panel_thumbnails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_thumbSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_romSelection;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_columns;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_normal;
        private System.Windows.Forms.ImageList imageList1;
        private MLV.ManagedListView managedListView1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem changeIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToPlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem editCategoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel SelectionStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ImageList imageList_listView;
        private System.Windows.Forms.Panel panel_thumbnails;
        private System.Windows.Forms.TrackBar trackBar_thumbSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_thumbCycle;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem showNextThumbnailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetThumbnailIndexToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox_thumbsMode;
        private System.Windows.Forms.ToolStripMenuItem getNameAndDataFromMobyGamescomToolStripMenuItem;
        private System.Windows.Forms.Timer timer_selection;
        private System.Windows.Forms.ToolStripMenuItem clearRelatedFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem applyRomNamesToFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playTimesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastPlayedTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem allCountersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem sendToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getDataFromTheGamesDBForThisGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDataInfoAsCategoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportNamesToolStripMenuItem;
    }
}
