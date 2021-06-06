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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using MTC;

namespace EmulatorsOrganizer.GUI
{
    public partial class ConsoleStatusBrowser : IBrowserControl
    {
        public ConsoleStatusBrowser(ManagedTabControl parentMTC)
        {
            InitializeComponent();
            base.InitializeEvents();
            this.parentMTC = parentMTC;

            this.consoleInfoControl1.Initialize(null, parentMTC);

            // Add tabs
            MTCTabPage page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.ID = "rtf";
            page.ImageIndex = 0;
            page.Text = ls["Tab_Information"];
            page.Panel = new Panel();
            managedTabControl1.TabPages.Add(page);
            // Add the information control
            informationView = new ConsoleInformationViewer();
            page.Panel.Controls.Add(informationView);
            informationView.Dock = DockStyle.Fill;

            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.ID = "pdf";
            page.ImageIndex = 0;
            page.Text = ls["Tab_Manual"];
            page.Panel = new Panel();
            managedTabControl1.TabPages.Add(page);
            // Add the manual control
            manualView = new ConsoleManualViewer();
            page.Panel.Controls.Add(manualView);
            manualView.Dock = DockStyle.Fill;

            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.ID = "links";
            page.ImageIndex = 0;
            page.Text = ls["Tab_Links"];
            page.Panel = new Panel();
            managedTabControl1.TabPages.Add(page);
            // Add the manual control
            linksViewer = new ConsoleLinksViewer();
            page.Panel.Controls.Add(linksViewer);
            linksViewer.Dock = DockStyle.Fill;
        }
        private ManagedTabControl parentMTC;
        private ConsoleInformationViewer informationView;
        private ConsoleManualViewer manualView;
        private ConsoleLinksViewer linksViewer;

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        public override void InitializeEvents()
        {
            profileManager.Profile.ConsoleSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolePropertiesChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsoleRemoved += Profile_ConsoleRemoved;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.ConsoleSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolePropertiesChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsoleRemoved -= Profile_ConsoleRemoved;
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_CategoriesBrowser;
            this.BackgroundImage = style.image_CategoriesBrowser;

            managedTabControl1.DrawStyle = MTCDrawStyle.Flat;
            managedTabControl1.CloseBoxAlwaysVisible = false;
            managedTabControl1.BackColor = style.bkgColor_InformationContainerTabs;
            managedTabControl1.TabPageColor = style.TabPageColor;
            managedTabControl1.TabPageSelectedColor = style.TabPageSelectedColor;
            managedTabControl1.TabPageHighlightedColor = style.TabPageHighlightedColor;
            managedTabControl1.TabPageSplitColor = style.TabPageSplitColor;
            managedTabControl1.ForeColor = style.TabPageTextsColor;
        }

        protected override void OnProfileOpened()
        {
            base.OnProfileOpened();
            //RefreshStatus();
        }
        public void RefreshStatus()
        {
            // Clear
            consoleInfoControl1.ClearInformation();
            informationView.ClearInformation();
            manualView.ClearInformation();
            linksViewer.ClearInformation();
            if (!this.Visible)
                return;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        // Load
                        consoleInfoControl1.RefreshInformation
                            (profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID]);
                        informationView.LoadInformation(profileManager.Profile.SelectedConsoleID);
                        manualView.LoadInformation(profileManager.Profile.SelectedConsoleID);
                        linksViewer.LoadInformation(profileManager.Profile.SelectedConsoleID);
                        break;
                    }
            }
        }
        private void Profile_ConsoleSelectionChanged(object sender, EventArgs e)
        {
            // RefreshStatus();
            // Clear
            consoleInfoControl1.ClearInformation();
            informationView.ClearInformation();
            manualView.ClearInformation();
            linksViewer.ClearInformation();
        }
        private void Profile_ConsoleRemoved(object sender, EventArgs e)
        {
            // Clear
            consoleInfoControl1.ClearInformation();
            informationView.ClearInformation();
            manualView.ClearInformation();
            linksViewer.ClearInformation();
        }
        // Refresh
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshStatus();
        }
    }
}
