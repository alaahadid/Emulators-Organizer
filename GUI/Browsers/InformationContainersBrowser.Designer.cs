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
    partial class InformationContainersBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformationContainersBrowser));
            this.imageList_tabs = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList_tabs
            // 
            this.imageList_tabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_tabs.ImageStream")));
            this.imageList_tabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_tabs.Images.SetKeyName(0, "images.png");
            this.imageList_tabs.Images.SetKeyName(1, "note.png");
            this.imageList_tabs.Images.SetKeyName(2, "link.png");
            this.imageList_tabs.Images.SetKeyName(3, "monitor.png");
            this.imageList_tabs.Images.SetKeyName(4, "book_open.png");
            this.imageList_tabs.Images.SetKeyName(5, "information.png");
            this.imageList_tabs.Images.SetKeyName(6, "film.png");
            this.imageList_tabs.Images.SetKeyName(7, "chart_bar.png");
            // 
            // InformationContainersBrowser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "InformationContainersBrowser";
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InformationContainersBrowser_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList_tabs;
    }
}
