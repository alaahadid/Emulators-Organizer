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
    partial class EmulatorsBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmulatorsBrowser));
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackBar_zoom = new System.Windows.Forms.TrackBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_moveUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_moveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.managedListView1 = new MLV.ManagedListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEmulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.changeIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripButton_moveUp,
            this.toolStripButton_moveDown,
            this.toolStripSeparator4,
            this.toolStripButton2});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.add;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.AddEmulatorClick);
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
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.text_linespacing;
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // managedListView1
            // 
            this.managedListView1.AllowColumnsReorder = true;
            this.managedListView1.AllowDrop = true;
            this.managedListView1.AllowItemsDragAndDrop = true;
            this.managedListView1.AutoSetWheelScrollSpeed = true;
            this.managedListView1.BackgroundRenderMode = MLV.ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio;
            this.managedListView1.ChangeColumnSortModeWhenClick = true;
            this.managedListView1.ColumnClickColor = System.Drawing.Color.PaleVioletRed;
            this.managedListView1.ColumnColor = System.Drawing.Color.Silver;
            this.managedListView1.ColumnHighlightColor = System.Drawing.Color.LightSkyBlue;
            this.managedListView1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.managedListView1, "managedListView1");
            this.managedListView1.DrawHighlight = true;
            this.managedListView1.ItemHighlightColor = System.Drawing.Color.LightSkyBlue;
            this.managedListView1.ItemMouseOverColor = System.Drawing.Color.LightGray;
            this.managedListView1.ItemSpecialColor = System.Drawing.Color.YellowGreen;
            this.managedListView1.Name = "managedListView1";
            this.managedListView1.ShowItemInfoOnThumbnailMode = true;
            this.managedListView1.ShowSubItemToolTip = true;
            this.managedListView1.StretchThumbnailsToFit = true;
            this.managedListView1.ThunmbnailsHeight = 36;
            this.managedListView1.ThunmbnailsWidth = 36;
            this.managedListView1.ViewMode = MLV.ManagedListViewViewMode.Thumbnails;
            this.managedListView1.WheelScrollSpeed = 19;
            this.managedListView1.DrawItem += new System.EventHandler<MLV.ManagedListViewItemDrawArgs>(this.managedListView1_DrawItem);
            this.managedListView1.SelectedIndexChanged += new System.EventHandler(this.managedListView1_SelectedIndexChanged);
            this.managedListView1.Click += new System.EventHandler(this.managedListView1_Click);
            this.managedListView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragDrop);
            this.managedListView1.DragOver += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragOver);
            this.managedListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.managedListView1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEmulatorToolStripMenuItem,
            this.toolStripSeparator1,
            this.runToolStripMenuItem,
            this.openFileLocationToolStripMenuItem,
            this.toolStripSeparator6,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripSeparator3,
            this.changeIconToolStripMenuItem,
            this.clearIconToolStripMenuItem,
            this.toolStripSeparator5,
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // addEmulatorToolStripMenuItem
            // 
            this.addEmulatorToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.add;
            this.addEmulatorToolStripMenuItem.Name = "addEmulatorToolStripMenuItem";
            resources.ApplyResources(this.addEmulatorToolStripMenuItem, "addEmulatorToolStripMenuItem");
            this.addEmulatorToolStripMenuItem.Click += new System.EventHandler(this.addEmulatorToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            resources.ApplyResources(this.runToolStripMenuItem, "runToolStripMenuItem");
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // openFileLocationToolStripMenuItem
            // 
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            resources.ApplyResources(this.openFileLocationToolStripMenuItem, "openFileLocationToolStripMenuItem");
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.openFileLocationToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
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
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.properties;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            resources.ApplyResources(this.propertiesToolStripMenuItem, "propertiesToolStripMenuItem");
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // EmulatorsBrowser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.managedListView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "EmulatorsBrowser";
            this.Enter += new System.EventHandler(this.EmulatorsBrowser_Enter);
            this.Leave += new System.EventHandler(this.managedListView1_Leave);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackBar_zoom;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_moveUp;
        private System.Windows.Forms.ToolStripButton toolStripButton_moveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private MLV.ManagedListView managedListView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addEmulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem changeIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;

    }
}
