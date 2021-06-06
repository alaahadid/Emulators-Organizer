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
    partial class ICImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ICImage));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_search_gmdb = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_previous = new System.Windows.Forms.ToolStripButton();
            this.StatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton_next = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.stretchToFitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysStretchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchnoAspectRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton_setOriginal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_pixelate = new System.Windows.Forms.ToolStripButton();
            this.trackBar_zoom = new System.Windows.Forms.TrackBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nextImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.addImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.editListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.openImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.searchGoogleForMoreImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchAFolderForMoreImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchTheGamesDBnetForImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.imageModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalstretchToFitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysStretchToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchnoAspectRatioToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pixilatedModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.showToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStatusbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imagePanel1 = new EmulatorsOrganizer.Core.ImagePanel();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripSeparator2,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripSeparator3,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripSeparator11,
            this.toolStripButton_search_gmdb});
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
            this.toolStripButton4.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.image;
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.google_icon1;
            resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click_1);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder_explore;
            resources.ApplyResources(this.toolStripButton7, "toolStripButton7");
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click_1);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // toolStripButton_search_gmdb
            // 
            this.toolStripButton_search_gmdb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_search_gmdb.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.the_game_db_icon;
            resources.ApplyResources(this.toolStripButton_search_gmdb, "toolStripButton_search_gmdb");
            this.toolStripButton_search_gmdb.Name = "toolStripButton_search_gmdb";
            this.toolStripButton_search_gmdb.Click += new System.EventHandler(this.toolStripButton_search_gmdb_Click);
            // 
            // toolStrip2
            // 
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_previous,
            this.StatusLabel,
            this.toolStripButton_next,
            this.toolStripSeparator4,
            this.toolStripSplitButton1,
            this.toolStripButton_setOriginal,
            this.toolStripButton_pixelate});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripButton_previous
            // 
            this.toolStripButton_previous.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton_previous, "toolStripButton_previous");
            this.toolStripButton_previous.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_left;
            this.toolStripButton_previous.Name = "toolStripButton_previous";
            this.toolStripButton_previous.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.AutoToolTip = true;
            this.StatusLabel.Name = "StatusLabel";
            // 
            // toolStripButton_next
            // 
            this.toolStripButton_next.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton_next, "toolStripButton_next");
            this.toolStripButton_next.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_right;
            this.toolStripButton_next.Name = "toolStripButton_next";
            this.toolStripButton_next.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stretchToFitToolStripMenuItem,
            this.alwaysStretchToolStripMenuItem,
            this.stretchnoAspectRatioToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.application_view_tile;
            resources.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            this.toolStripSplitButton1.DropDownOpening += new System.EventHandler(this.toolStripSplitButton1_DropDownOpening);
            // 
            // stretchToFitToolStripMenuItem
            // 
            this.stretchToFitToolStripMenuItem.Name = "stretchToFitToolStripMenuItem";
            resources.ApplyResources(this.stretchToFitToolStripMenuItem, "stretchToFitToolStripMenuItem");
            this.stretchToFitToolStripMenuItem.Click += new System.EventHandler(this.stretchToFitToolStripMenuItem_Click);
            // 
            // alwaysStretchToolStripMenuItem
            // 
            this.alwaysStretchToolStripMenuItem.Name = "alwaysStretchToolStripMenuItem";
            resources.ApplyResources(this.alwaysStretchToolStripMenuItem, "alwaysStretchToolStripMenuItem");
            this.alwaysStretchToolStripMenuItem.Click += new System.EventHandler(this.alwaysStretchToolStripMenuItem_Click);
            // 
            // stretchnoAspectRatioToolStripMenuItem
            // 
            this.stretchnoAspectRatioToolStripMenuItem.Name = "stretchnoAspectRatioToolStripMenuItem";
            resources.ApplyResources(this.stretchnoAspectRatioToolStripMenuItem, "stretchnoAspectRatioToolStripMenuItem");
            this.stretchnoAspectRatioToolStripMenuItem.Click += new System.EventHandler(this.stretchnoAspectRatioToolStripMenuItem_Click);
            // 
            // toolStripButton_setOriginal
            // 
            this.toolStripButton_setOriginal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton_setOriginal, "toolStripButton_setOriginal");
            this.toolStripButton_setOriginal.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_inout;
            this.toolStripButton_setOriginal.Name = "toolStripButton_setOriginal";
            this.toolStripButton_setOriginal.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton_pixelate
            // 
            this.toolStripButton_pixelate.Checked = true;
            this.toolStripButton_pixelate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_pixelate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_pixelate.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.bullet_picture;
            resources.ApplyResources(this.toolStripButton_pixelate, "toolStripButton_pixelate");
            this.toolStripButton_pixelate.Name = "toolStripButton_pixelate";
            this.toolStripButton_pixelate.CheckedChanged += new System.EventHandler(this.toolStripButton_pixelate_CheckedChanged);
            // 
            // trackBar_zoom
            // 
            resources.ApplyResources(this.trackBar_zoom, "trackBar_zoom");
            this.trackBar_zoom.Maximum = 500;
            this.trackBar_zoom.Minimum = 1;
            this.trackBar_zoom.Name = "trackBar_zoom";
            this.trackBar_zoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.trackBar_zoom, resources.GetString("trackBar_zoom.ToolTip"));
            this.trackBar_zoom.Value = 1;
            this.trackBar_zoom.Scroll += new System.EventHandler(this.trackBar_zoom_Scroll);
            // 
            // hScrollBar1
            // 
            resources.ApplyResources(this.hScrollBar1, "hScrollBar1");
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // vScrollBar1
            // 
            resources.ApplyResources(this.vScrollBar1, "vScrollBar1");
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextImageToolStripMenuItem,
            this.previousImageToolStripMenuItem,
            this.toolStripSeparator10,
            this.addImagesToolStripMenuItem,
            this.removeImageToolStripMenuItem,
            this.toolStripSeparator6,
            this.editListToolStripMenuItem,
            this.toolStripSeparator5,
            this.openImageToolStripMenuItem,
            this.openLocationToolStripMenuItem,
            this.toolStripSeparator7,
            this.searchGoogleForMoreImagesToolStripMenuItem,
            this.searchAFolderForMoreImagesToolStripMenuItem,
            this.searchTheGamesDBnetForImagesToolStripMenuItem,
            this.toolStripSeparator8,
            this.imageModeToolStripMenuItem,
            this.pixilatedModeToolStripMenuItem,
            this.toolStripSeparator9,
            this.showToolbarToolStripMenuItem,
            this.showStatusbarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // nextImageToolStripMenuItem
            // 
            this.nextImageToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_right;
            this.nextImageToolStripMenuItem.Name = "nextImageToolStripMenuItem";
            resources.ApplyResources(this.nextImageToolStripMenuItem, "nextImageToolStripMenuItem");
            this.nextImageToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // previousImageToolStripMenuItem
            // 
            this.previousImageToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_left;
            this.previousImageToolStripMenuItem.Name = "previousImageToolStripMenuItem";
            resources.ApplyResources(this.previousImageToolStripMenuItem, "previousImageToolStripMenuItem");
            this.previousImageToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // addImagesToolStripMenuItem
            // 
            this.addImagesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.add;
            this.addImagesToolStripMenuItem.Name = "addImagesToolStripMenuItem";
            resources.ApplyResources(this.addImagesToolStripMenuItem, "addImagesToolStripMenuItem");
            this.addImagesToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // removeImageToolStripMenuItem
            // 
            this.removeImageToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.cross;
            this.removeImageToolStripMenuItem.Name = "removeImageToolStripMenuItem";
            resources.ApplyResources(this.removeImageToolStripMenuItem, "removeImageToolStripMenuItem");
            this.removeImageToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // editListToolStripMenuItem
            // 
            this.editListToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.application_view_list;
            this.editListToolStripMenuItem.Name = "editListToolStripMenuItem";
            resources.ApplyResources(this.editListToolStripMenuItem, "editListToolStripMenuItem");
            this.editListToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // openImageToolStripMenuItem
            // 
            this.openImageToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.image;
            this.openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
            resources.ApplyResources(this.openImageToolStripMenuItem, "openImageToolStripMenuItem");
            this.openImageToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // openLocationToolStripMenuItem
            // 
            this.openLocationToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder;
            this.openLocationToolStripMenuItem.Name = "openLocationToolStripMenuItem";
            resources.ApplyResources(this.openLocationToolStripMenuItem, "openLocationToolStripMenuItem");
            this.openLocationToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // searchGoogleForMoreImagesToolStripMenuItem
            // 
            this.searchGoogleForMoreImagesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.google_icon;
            this.searchGoogleForMoreImagesToolStripMenuItem.Name = "searchGoogleForMoreImagesToolStripMenuItem";
            resources.ApplyResources(this.searchGoogleForMoreImagesToolStripMenuItem, "searchGoogleForMoreImagesToolStripMenuItem");
            this.searchGoogleForMoreImagesToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton6_Click_1);
            // 
            // searchAFolderForMoreImagesToolStripMenuItem
            // 
            this.searchAFolderForMoreImagesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder_explore;
            this.searchAFolderForMoreImagesToolStripMenuItem.Name = "searchAFolderForMoreImagesToolStripMenuItem";
            resources.ApplyResources(this.searchAFolderForMoreImagesToolStripMenuItem, "searchAFolderForMoreImagesToolStripMenuItem");
            this.searchAFolderForMoreImagesToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton7_Click_1);
            // 
            // searchTheGamesDBnetForImagesToolStripMenuItem
            // 
            this.searchTheGamesDBnetForImagesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.the_game_db_icon;
            this.searchTheGamesDBnetForImagesToolStripMenuItem.Name = "searchTheGamesDBnetForImagesToolStripMenuItem";
            resources.ApplyResources(this.searchTheGamesDBnetForImagesToolStripMenuItem, "searchTheGamesDBnetForImagesToolStripMenuItem");
            this.searchTheGamesDBnetForImagesToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_search_gmdb_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // imageModeToolStripMenuItem
            // 
            this.imageModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalstretchToFitToolStripMenuItem,
            this.alwaysStretchToolStripMenuItem1,
            this.stretchnoAspectRatioToolStripMenuItem1});
            this.imageModeToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.application_view_tile;
            this.imageModeToolStripMenuItem.Name = "imageModeToolStripMenuItem";
            resources.ApplyResources(this.imageModeToolStripMenuItem, "imageModeToolStripMenuItem");
            this.imageModeToolStripMenuItem.DropDownOpening += new System.EventHandler(this.imageModeToolStripMenuItem_DropDownOpening);
            // 
            // normalstretchToFitToolStripMenuItem
            // 
            this.normalstretchToFitToolStripMenuItem.Name = "normalstretchToFitToolStripMenuItem";
            resources.ApplyResources(this.normalstretchToFitToolStripMenuItem, "normalstretchToFitToolStripMenuItem");
            this.normalstretchToFitToolStripMenuItem.Click += new System.EventHandler(this.stretchToFitToolStripMenuItem_Click);
            // 
            // alwaysStretchToolStripMenuItem1
            // 
            this.alwaysStretchToolStripMenuItem1.Name = "alwaysStretchToolStripMenuItem1";
            resources.ApplyResources(this.alwaysStretchToolStripMenuItem1, "alwaysStretchToolStripMenuItem1");
            this.alwaysStretchToolStripMenuItem1.Click += new System.EventHandler(this.alwaysStretchToolStripMenuItem_Click);
            // 
            // stretchnoAspectRatioToolStripMenuItem1
            // 
            this.stretchnoAspectRatioToolStripMenuItem1.Name = "stretchnoAspectRatioToolStripMenuItem1";
            resources.ApplyResources(this.stretchnoAspectRatioToolStripMenuItem1, "stretchnoAspectRatioToolStripMenuItem1");
            this.stretchnoAspectRatioToolStripMenuItem1.Click += new System.EventHandler(this.stretchnoAspectRatioToolStripMenuItem_Click);
            // 
            // pixilatedModeToolStripMenuItem
            // 
            this.pixilatedModeToolStripMenuItem.Name = "pixilatedModeToolStripMenuItem";
            resources.ApplyResources(this.pixilatedModeToolStripMenuItem, "pixilatedModeToolStripMenuItem");
            this.pixilatedModeToolStripMenuItem.Click += new System.EventHandler(this.pixilatedModeToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // showToolbarToolStripMenuItem
            // 
            this.showToolbarToolStripMenuItem.Name = "showToolbarToolStripMenuItem";
            resources.ApplyResources(this.showToolbarToolStripMenuItem, "showToolbarToolStripMenuItem");
            this.showToolbarToolStripMenuItem.Click += new System.EventHandler(this.showToolbarToolStripMenuItem_Click);
            // 
            // showStatusbarToolStripMenuItem
            // 
            this.showStatusbarToolStripMenuItem.Name = "showStatusbarToolStripMenuItem";
            resources.ApplyResources(this.showStatusbarToolStripMenuItem, "showStatusbarToolStripMenuItem");
            this.showStatusbarToolStripMenuItem.Click += new System.EventHandler(this.showStatusbarToolStripMenuItem_Click);
            // 
            // imagePanel1
            // 
            this.imagePanel1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.imagePanel1, "imagePanel1");
            this.imagePanel1.ImageToView = null;
            this.imagePanel1.ImageViewMode = EmulatorsOrganizer.Core.ImageViewMode.StretchIfLarger;
            this.imagePanel1.Name = "imagePanel1";
            this.imagePanel1.DisableScrollBars += new System.EventHandler(this.imagePanel1_DisableScrollBars);
            this.imagePanel1.EnableScrollBars += new System.EventHandler(this.imagePanel1_EnableScrollBars);
            this.imagePanel1.CalculateScrollValues += new System.EventHandler(this.imagePanel1_CalculateScrollValues);
            this.imagePanel1.ImageViewModeChanged += new System.EventHandler(this.imagePanel1_ImageViewModeChanged);
            this.imagePanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imagePanel1_MouseClick);
            this.imagePanel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imagePanel1_MouseDoubleClick);
            this.imagePanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imagePanel1_MouseDown);
            this.imagePanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imagePanel1_MouseMove);
            this.imagePanel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imagePanel1_MouseUp);
            // 
            // ICImage
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imagePanel1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.trackBar_zoom);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ICImage";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Core.ImagePanel imagePanel1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton_previous;
        private System.Windows.Forms.ToolStripLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton_next;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem alwaysStretchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stretchToFitToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackBar_zoom;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ToolStripButton toolStripButton_setOriginal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showToolbarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showStatusbarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem removeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem editListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem openLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchGoogleForMoreImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem imageModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysStretchToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem normalstretchToFitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem nextImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem stretchnoAspectRatioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stretchnoAspectRatioToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripMenuItem searchAFolderForMoreImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton_pixelate;
        private System.Windows.Forms.ToolStripMenuItem pixilatedModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton toolStripButton_search_gmdb;
        private System.Windows.Forms.ToolStripMenuItem searchTheGamesDBnetForImagesToolStripMenuItem;
    }
}
