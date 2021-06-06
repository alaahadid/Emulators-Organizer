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
using System.Diagnostics;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
using MTC;

namespace EmulatorsOrganizer.GUI
{
    public partial class InformationContainersBrowser : IBrowserControl
    {
        public InformationContainersBrowser()
        {
            InitializeComponent();
            base.InitializeEvents();
            //   InitializeEvents();
        }
        private bool isLoading;
        private bool showCloseButtons;
        private bool closeButtonsAlwaysVisible;
        private bool showIcons;
        private string currentConsoleID;
        public string ConsoleID { get { return currentConsoleID; } }
        public event EventHandler SaveRequest;
        private ImageViewMode bkgMode;
        private delegate void RefreshTabsDelegate(string id);
        private Image backgroundThumbnail;
        private int drawX;
        private int drawY;
        private int drawH;
        private int drawW;
        private bool temp_ignore;

        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_InformationContainerBrowser;
            this.BackgroundImage = style.image_InformationContainerBrowser;
            switch (style.imageMode_InformationContainerBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio:
                    {
                        bkgMode = ImageViewMode.Normal;
                        break;
                    }
                case BackgroundImageMode.StretchIfLarger:
                    {
                        bkgMode = ImageViewMode.StretchIfLarger;
                        break;
                    }
                case BackgroundImageMode.StretchToFit:
                    {
                        bkgMode = ImageViewMode.StretchToFit;
                        break;
                    }
            }
            if (currentConsoleID != null && currentConsoleID != "")
            {
                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
                if (console != null)
                {
                    if (console.InformationContainersMap != null)
                        console.InformationContainersMap.ApplyTabsStyle(style);
                }
            }
            CalculateBackgroundBounds();
        }
        public override void InitializeEvents()
        {
            profileManager.Profile.ConsoleSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolePropertiesChanged += Profile_ConsolePropertiesChanged;
            profileManager.Profile.ConsoleAdded += Profile_ConsoleSelectionChanged;
            profileManager.Profile.InformationContainerAdded += Profile_InformationContainerAdded;
            profileManager.Profile.InformationContainerRemoved += Profile_InformationContainerRemoved;
            profileManager.Profile.InformationContainerVisibiltyChanged += Profile_InformationContainerVisibiltyChanged;
            profileManager.Profile.RomSelectionChanged += Profile_RomSelectionChanged;
            profileManager.Profile.RefreshPriorityRequest += Profile_RefreshPriority;
            profileManager.Profile.ConsoleRemoved += Profile_ConsoleRemoved;

            profileManager.Profile.PlaylistsGroupRemoved += Profile_PlaylistsGroupRemoved;
            profileManager.Profile.PlaylistRemoved += Profile_PlaylistRemoved;
        }

        public override void DisposeEvents()
        {
            profileManager.Profile.ConsoleSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsoleAdded -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsoleAdded -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.InformationContainerAdded -= Profile_InformationContainerAdded;
            profileManager.Profile.InformationContainerRemoved -= Profile_InformationContainerRemoved;
            profileManager.Profile.InformationContainerVisibiltyChanged -= Profile_InformationContainerVisibiltyChanged;
            profileManager.Profile.RomSelectionChanged -= Profile_RomSelectionChanged;
            profileManager.Profile.RefreshPriorityRequest -= Profile_RefreshPriority;
            profileManager.Profile.ConsoleRemoved -= Profile_ConsoleRemoved;
        }
        protected override void OnCreatingNewProfile()
        {
            base.OnCreatingNewProfile();
            currentConsoleID = "";
            foreach (Control con in this.Controls)
                con.Dispose();
            this.Controls.Clear();
        }
        protected override void OnOpeningProfile()
        {
            base.OnOpeningProfile();
            currentConsoleID = "";
            foreach (Control con in this.Controls)
                con.Dispose();
            this.Controls.Clear();
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
            if (console != null)
            {
                if (console.InformationContainersMap != null)
                {
                    if (InformationContainerTabsPanel.LastDragDropFinished) return;

                    console.InformationContainersMap.ToggleTabsCloseButtonVisible(false);
                }
            }
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            base.OnProfileSavingStarted();
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
            if (console != null)
            {
                if (console.InformationContainersMap != null)
                {
                    if (InformationContainerTabsPanel.LastDragDropFinished) return;

                    console.InformationContainersMap.ToggleTabsCloseButtonVisible(true);
                }
            }
        }

        /* List every ic control here ! */
        private Control GetControl(InformationContainer ic, string consoleID)
        {
            //if (currentConsoleID == null || currentConsoleID == "")
            //    return null;
            //if (!profileManager.Profile.Consoles.Contains(currentConsoleID))
            //    return null;
            ICControl con = null;

            if (ic is InformationContainerImage)
                con = new ICImage(ic.ID, consoleID);

            if (ic is InformationContainerInfoText)
                con = new ICInfoText(ic.ID, consoleID);

            if (ic is InformationContainerLinks)
                con = new ICLinks(ic.ID, consoleID);

            if (ic is InformationContainerMedia)
                con = new ICMedia(ic.ID, consoleID);

            if (ic is InformationContainerPDF)
                con = new ICPDF(ic.ID, consoleID);

            if (ic is InformationContainerRomInfo)
                con = new ICRomInfo(ic.ID, consoleID);

            if (ic is InformationContainerYoutubeVideo)
                con = new ICYoutube(ic.ID, consoleID);

            if (ic is InformationContainerReviewScore)
                con = new ICReviewScore(ic.ID, consoleID);

            if (con != null)
                con.InitializeEvents();
            if (currentConsoleID != null)
            {
                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
                if (console != null && con != null)
                    con.ApplyStyle(console.Style);
            }
            return con;
        }
        private int GetImageIndex(InformationContainer ic)
        {
            if (ic is InformationContainerImage)
                return 0;

            if (ic is InformationContainerInfoText)
                return 1;

            if (ic is InformationContainerLinks)
                return 2;

            if (ic is InformationContainerMedia)
                return 3;

            if (ic is InformationContainerPDF)
                return 4;

            if (ic is InformationContainerRomInfo)
                return 5;

            if (ic is InformationContainerYoutubeVideo)
                return 6;

            if (ic is InformationContainerReviewScore)
                return 7;

            return 0;
        }
        private ImageList GetImagelist()
        { return imageList_tabs; }
        private void RefreshTabs(bool ignoreIfSameConsole)
        {
            temp_ignore = ignoreIfSameConsole;
            if (!InvokeRequired)
                RefreshTabsThreaded();
            else
                Invoke(new Action(RefreshTabsThreaded));
        }
        private void RefreshTabsThreaded()
        {
            if (isLoading)
                return;// Don't reload if it's loading !
            ServicesManager.OnDisableWindowListner();
            Trace.WriteLine("Refreshing ic tabs...", "Information Containers Browser");
            isLoading = true;
            string consoleID = "";
            bool isSameConsole = false;
            // Save the splitters of old console shown
            if (currentConsoleID != "")
            {
                EmulatorsOrganizer.Core.Console pcons = profileManager.Profile.Consoles[currentConsoleID];
                if (pcons != null)
                    if (pcons.InformationContainersMap != null)
                        pcons.InformationContainersMap.SaveSplitters();
            }
            // Refresh the new console show
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        consoleID = profileManager.Profile.SelectedConsoleID;
                        break;
                    }
                case SelectionType.ConsolesGroup:
                case SelectionType.PlaylistsGroup:
                case SelectionType.Playlist:
                    {
                        if (profileManager.Profile.SelectedRomIDS == null)
                            break;
                        // Determining the console id depends on selected roms
                        else if (profileManager.Profile.SelectedRomIDS.Count == 1)
                        {
                            consoleID = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]].ParentConsoleID;
                        }
                        else
                        {
                            // Clear both to reset !
                            currentConsoleID = consoleID = "";
                        }
                        break;
                    }
                case SelectionType.None:
                default: break;
            }
            if (consoleID == "")
            {
                this.Controls.Clear(); isLoading = false; ServicesManager.OnEnableWindowListner(); return;
            }
            if (currentConsoleID != null)
                if (temp_ignore && consoleID == currentConsoleID)
                {
                    isLoading = false; ServicesManager.OnEnableWindowListner(); return;
                }
            isSameConsole = consoleID == currentConsoleID;
            this.Controls.Clear();
            // Get console
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console != null)
            {
                if (console.InformationContainersMap != null)
                {
                    console.InformationContainersMap.Dispose();// Clear events and resources.
                    console.InformationContainersMap.FixPanels();// Make a fix to clear odd panels.
                    console.InformationContainersMap.ShowPanel(this, console,
                        new GetICControl(GetControl), new GetICImageList(GetImagelist), new GetICControlImageIndex(GetImageIndex));
                    console.InformationContainersMap.UpdateSplitters();// Update splitters to take saved distances
                    //console.InformationContainersMap.UpdateSplitters();// Update splitters again to ensure
                    console.InformationContainersMap.ToggleCloseButtonVisible(showCloseButtons, closeButtonsAlwaysVisible, showIcons);
                    // Implement events.
                    console.InformationContainersMap.SplitterMoved += InformationContainersMap_SplitterMoved;
                    console.InformationContainersMap.TabClosed += InformationContainersMap_TabClosed;
                    console.InformationContainersMap.TabReordered += InformationContainersMap_TabReordered;
                    console.InformationContainersMap.ReuqestRefresh += InformationContainersMap_ReuqestRefresh;
                    console.InformationContainersMap.TabDragged += InformationContainersMap_TabDragged;
                    console.InformationContainersMap.RequestNewPanel += InformationContainersMap_RequestNewPanel;
                    if (!isSameConsole)
                        console.InformationContainersMap.ApplyTabsStyle(console.Style);
                }
            }
            currentConsoleID = consoleID;
            isLoading = false;
            ServicesManager.OnEnableWindowListner();
        }
        protected override void OnProfileOpened()
        {
            base.OnProfileOpened();
            this.Controls.Clear();
            if (profileManager.Profile.RememberLatestSelectedConsoleOnProfileOpen)
            {
                RefreshTabs(false);
                if (profileManager.Profile.RememberLatestSelectedRomOnProfileOpen)
                {
                    // Raise event again !
                    profileManager.Profile.OnRomSelectionChanged();
                }
            }
            CalculateBackgroundBounds();
        }
        protected override void OnNewProfileCreated()
        {
            base.OnNewProfileCreated();
            this.Controls.Clear(); CalculateBackgroundBounds();
        }
        public void ShowHideCloseButtons(bool showCloseButtons, bool closeButtonsAlwaysVisible, bool showIcons)
        {
            this.showCloseButtons = showCloseButtons;
            this.closeButtonsAlwaysVisible = closeButtonsAlwaysVisible;
            this.showIcons = showIcons;
            RefreshTabs(false);
        }
        private void UpdateTabsPriority()
        {
            if (currentConsoleID != null && currentConsoleID != "")
            {
                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
                if (console.AutoSwitchTabPriorityDepend)
                {
                    // Get all mtc controls
                    List<ManagedTabControl> mtcs = console.InformationContainersMap.GetMTCControls();
                    // Loop through them !
                    foreach (ManagedTabControl mtc in mtcs)
                    {
                        // Loop through information container with order, see what we have
                        foreach (InformationContainer container in console.InformationContainers)
                        {
                            // Loop through mtc pages
                            int tabIbdex = 0;
                            bool ok = false;
                            foreach (MTCTabPage page in mtc.TabPages)
                            {
                                // Get the ic control
                                if (page.Panel.Controls.Count > 0)
                                {
                                    ICControl control = (ICControl)page.Panel.Controls[0];
                                    // See if the id match !
                                    if (control.ICID == container.ID)
                                    {
                                        // This is it ! see if this control is showing files
                                        if (control.GotFilesToShow)
                                        {
                                            // This is it ! switch to it.
                                            ok = true;
                                            mtc.SelectedTabPageIndex = tabIbdex;
                                            // Tell the control about the priority
                                            control.OnPriorityActive();
                                            break;
                                        }
                                    }
                                }
                                tabIbdex++;
                            }
                            if (ok)
                                break;
                            else
                                mtc.SelectedTabPageIndex = 0;
                        }
                    }
                }
            }
        }
        private void InformationContainersMap_SplitterMoved(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (isLoading)
                return;
            if (SaveRequest != null)
                SaveRequest(this, new EventArgs());
        }
        private void InformationContainersMap_TabClosed(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (isLoading)
                return;
            if (SaveRequest != null)
                SaveRequest(this, new EventArgs());
        }
        private void InformationContainersMap_TabReordered(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (isLoading)
                return;
            if (SaveRequest != null)
                SaveRequest(this, new EventArgs());
        }
        private void InformationContainersMap_ReuqestRefresh(object sender, EventArgs e)
        {
            RefreshTabs(false);
        }
        private void InformationContainersMap_RequestNewPanel(object sender, NewICPanelRequestArgs e)
        {
            if (profileManager.IsSaving)
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
            if (console != null)
            {
                if (console.InformationContainersMap != null)
                {
                    if (InformationContainerTabsPanel.LastDragDropFinished) return;

                    console.InformationContainersMap.AddContainerID(e.ContainerID, e.Location);
                }
            }
        }
        private void Profile_RefreshPriority(object sender, EventArgs e)
        {
            UpdateTabsPriority();
        }
        private void Profile_ConsoleSelectionChanged(object sender, EventArgs e)
        {
            /*if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
  {
      EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
      if (console != null)
      {
          if (console.InformationContainersMap != null)
              console.InformationContainersMap.ApplyTabsStyle(console.Style);
      }
  }*/
            // Clear all infos
            this.Controls.Clear(); CalculateBackgroundBounds();
        }
        private void Profile_RomSelectionChanged(object sender, EventArgs e)
        {
            RefreshTabs(true); CalculateBackgroundBounds();
        }
        private void Profile_InformationContainerAdded(object sender, EventArgs e)
        {
            RefreshTabs(false);
        }
        private void Profile_InformationContainerRemoved(object sender, EventArgs e)
        {
            RefreshTabs(false);
        }
        private void Profile_InformationContainerVisibiltyChanged(object sender, EventArgs e)
        {
            RefreshTabs(false);
        }
        private void InformationContainersMap_TabDragged(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (isLoading)
                return;
            if (SaveRequest != null)
                SaveRequest(this, new EventArgs());
        }
        private void Profile_ConsoleRemoved(object sender, EventArgs e)
        {
            // Clear
            RefreshTabs(true);
        }
        private void Profile_PlaylistRemoved(object sender, EventArgs e)
        {
            RefreshTabs(true);
        }
        private void Profile_PlaylistsGroupRemoved(object sender, EventArgs e)
        {
            RefreshTabs(true);
        }
        private void Profile_ConsolePropertiesChanged(object sender, EventArgs e)
        {
            RefreshTabs(false);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalculateBackgroundBounds();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            e.Graphics.Clear(this.BackColor);
            if (this.Controls.Count == 0)
                if (backgroundThumbnail != null)
                {
                    e.Graphics.DrawImage(backgroundThumbnail, drawX, drawY, drawW, drawH);
                    e.Graphics.DrawString("Double-mouse click to refresh tabs.", Font, Brushes.Blue, new Point(drawX, drawY + drawH + 4));
                }
                else
                {
                    // Use this to display default image since the background image property didn't work.
                    Size S = CalculateStretchImageValues(250, 250, this.Width, this.Height);
                    int x = (this.Width / 2) - (S.Width / 2);
                    int y = (this.Height / 2) - (S.Height / 2);
                    //e.Graphics.DrawImage(Properties.Resources.EmulatorsOrganizer, new Rectangle(x, y, S.Width, S.Height));
                    e.Graphics.DrawString("Double-mouse click to refresh tabs.", Font, Brushes.Blue, new Point(x, y + S.Height + 4));
                }
        }
        private void CalculateStretchedImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)this.BackgroundImage.Width / this.BackgroundImage.Height;

            if (this.Width >= this.BackgroundImage.Width && this.Height >= this.BackgroundImage.Height)
            {
                drawW = BackgroundImage.Width;
                drawH = BackgroundImage.Height;
            }
            else if (this.Width > BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                drawH = this.Height;
                drawW = (int)(this.Height * imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height > BackgroundImage.Height)
            {
                drawW = this.Width;
                drawH = (int)(this.Width / imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                }
            }
        }
        private void CalculateStretchToFitImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)BackgroundImage.Width / BackgroundImage.Height;

            if (this.Width >= BackgroundImage.Width && this.Height >= BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                }
            }
            else if (this.Width > BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                drawH = this.Height;
                drawW = (int)(this.Height * imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height > BackgroundImage.Height)
            {
                drawW = this.Width;
                drawH = (int)(this.Width / imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                }
            }

        }
        private void CenterImage()
        {
            drawY = (int)((this.Height - drawH) / 2.0);
            drawX = (int)((this.Width - drawW) / 2.0);
        }
        public void CalculateBackgroundBounds()
        {
            if (this.BackgroundImage != null)
            {
                switch (bkgMode)
                {
                    case ImageViewMode.Normal:// Stretch image without aspect ratio, always..
                        {
                            drawX = drawY = 0;
                            drawH = this.Height;
                            drawW = this.Width;
                            break;
                        }
                    case ImageViewMode.StretchIfLarger:
                        {
                            CalculateStretchedImageValues();
                            CenterImage();
                            break;
                        }
                    case ImageViewMode.StretchToFit:
                        {
                            CalculateStretchToFitImageValues();
                            CenterImage();
                            break;
                        }
                }
                backgroundThumbnail = this.BackgroundImage.GetThumbnailImage(drawW, drawH, null, IntPtr.Zero);
            }
            else
                backgroundThumbnail = null;
            Invalidate();
        }
        private Size CalculateStretchImageValues(int imgW, int imgH, int mW, int mH)
        {
            float pRatio = (float)mW / mH;
            float imRatio = (float)imgW / imgH;
            int viewImageWidth = 0;
            int viewImageHeight = 0;

            if (mW >= imgW && mH >= imgH)
            {
                viewImageWidth = imgW;
                viewImageHeight = imgH;
            }
            else if (mW > imgW && mH < imgH)
            {
                viewImageHeight = mH;
                viewImageWidth = (int)(mH * imRatio);
            }
            else if (mW < imgW && mH > imgH)
            {
                viewImageWidth = mW;
                viewImageHeight = (int)(mW / imRatio);
            }
            else if (mW < imgW && mH < imgH)
            {
                if (mW >= mH)
                {
                    //width image
                    if (imgW >= imgH && imRatio >= pRatio)
                    {
                        viewImageWidth = mW;
                        viewImageHeight = (int)(mW / imRatio);
                    }
                    else
                    {
                        viewImageHeight = mH;
                        viewImageWidth = (int)(mH * imRatio);
                    }
                }
                else
                {
                    if (imgW < imgH && imRatio < pRatio)
                    {
                        viewImageHeight = mH;
                        viewImageWidth = (int)(mH * imRatio);
                    }
                    else
                    {
                        viewImageWidth = mW;
                        viewImageHeight = (int)(mW / imRatio);
                    }
                }
            }

            return new Size(viewImageWidth, viewImageHeight);
        }
        // Refresh tabs
        private void linkLabel_refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshTabs(false);
        }
        private void InformationContainersBrowser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Controls.Count == 0)
                RefreshTabs(false);
        }
    }
}
