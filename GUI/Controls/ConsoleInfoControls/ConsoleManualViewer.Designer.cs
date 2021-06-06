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
    partial class ConsoleManualViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleManualViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_openAssigne = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_remove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_viewUsingWindowsDefaultViewer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_openFileLocation = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_openAssigne,
            this.toolStripButton_remove,
            this.toolStripSeparator1,
            this.toolStripButton_viewUsingWindowsDefaultViewer,
            this.toolStripButton_openFileLocation,
            this.toolStripSeparator2,
            this.toolStripButton1});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton_openAssigne
            // 
            this.toolStripButton_openAssigne.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_openAssigne.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder;
            resources.ApplyResources(this.toolStripButton_openAssigne, "toolStripButton_openAssigne");
            this.toolStripButton_openAssigne.Name = "toolStripButton_openAssigne";
            this.toolStripButton_openAssigne.Click += new System.EventHandler(this.toolStripButton_openAssigne_Click);
            // 
            // toolStripButton_remove
            // 
            this.toolStripButton_remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_remove.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.cross;
            resources.ApplyResources(this.toolStripButton_remove, "toolStripButton_remove");
            this.toolStripButton_remove.Name = "toolStripButton_remove";
            this.toolStripButton_remove.Click += new System.EventHandler(this.toolStripButton_remove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButton_viewUsingWindowsDefaultViewer
            // 
            this.toolStripButton_viewUsingWindowsDefaultViewer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_viewUsingWindowsDefaultViewer.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.File_PDF;
            resources.ApplyResources(this.toolStripButton_viewUsingWindowsDefaultViewer, "toolStripButton_viewUsingWindowsDefaultViewer");
            this.toolStripButton_viewUsingWindowsDefaultViewer.Name = "toolStripButton_viewUsingWindowsDefaultViewer";
            this.toolStripButton_viewUsingWindowsDefaultViewer.Click += new System.EventHandler(this.toolStripButton_viewUsingWindowsDefaultViewer_Click);
            // 
            // toolStripButton_openFileLocation
            // 
            this.toolStripButton_openFileLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_openFileLocation.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.folder_page_white;
            resources.ApplyResources(this.toolStripButton_openFileLocation, "toolStripButton_openFileLocation");
            this.toolStripButton_openFileLocation.Name = "toolStripButton_openFileLocation";
            this.toolStripButton_openFileLocation.Click += new System.EventHandler(this.toolStripButton_openFileLocation_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::EmulatorsOrganizer.GUI.Properties.Resources.arrow_refresh;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // webBrowser1
            // 
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.Name = "webBrowser1";
            // 
            // ConsoleManualViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ConsoleManualViewer";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_openAssigne;
        private System.Windows.Forms.ToolStripButton toolStripButton_remove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_viewUsingWindowsDefaultViewer;
        private System.Windows.Forms.ToolStripButton toolStripButton_openFileLocation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}
