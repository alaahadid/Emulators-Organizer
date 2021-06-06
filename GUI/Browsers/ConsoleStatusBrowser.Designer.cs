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
    partial class ConsoleStatusBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleStatusBrowser));
            this.consoleInfoControl1 = new EmulatorsOrganizer.GUI.ConsoleInfoControl();
            this.managedTabControl1 = new MTC.ManagedTabControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // consoleInfoControl1
            // 
            this.consoleInfoControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.consoleInfoControl1, "consoleInfoControl1");
            this.consoleInfoControl1.Name = "consoleInfoControl1";
            // 
            // managedTabControl1
            // 
            this.managedTabControl1.AllowAutoTabPageDragAndDrop = false;
            this.managedTabControl1.AllowTabPageDragAndDrop = false;
            this.managedTabControl1.AllowTabPagesReorder = false;
            this.managedTabControl1.AutoSelectAddedTabPageAfterAddingIt = false;
            this.managedTabControl1.CloseBoxAlwaysVisible = false;
            this.managedTabControl1.CloseBoxOnEachPageVisible = false;
            this.managedTabControl1.CloseTabOnCloseButtonClick = false;
            resources.ApplyResources(this.managedTabControl1, "managedTabControl1");
            this.managedTabControl1.DrawStyle = MTC.MTCDrawStyle.Normal;
            this.managedTabControl1.DrawTabPageHighlight = true;
            this.managedTabControl1.Name = "managedTabControl1";
            this.managedTabControl1.SelectedTabPageIndex = 0;
            this.managedTabControl1.ShowTabPageToolTip = true;
            this.managedTabControl1.ShowTabPageToolTipAlways = false;
            this.managedTabControl1.TabPageColor = System.Drawing.Color.Silver;
            this.managedTabControl1.TabPageHighlightedColor = System.Drawing.Color.LightBlue;
            this.managedTabControl1.TabPageMaxWidth = 250;
            this.managedTabControl1.TabPageSelectedColor = System.Drawing.Color.SkyBlue;
            this.managedTabControl1.TabPageSplitColor = System.Drawing.Color.Gray;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripLabel1});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_refresh;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // ConsoleStatusBrowser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.managedTabControl1);
            this.Controls.Add(this.consoleInfoControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ConsoleStatusBrowser";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConsoleInfoControl consoleInfoControl1;
        private MTC.ManagedTabControl managedTabControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}
