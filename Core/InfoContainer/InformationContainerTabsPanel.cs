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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using MTC;

/*Can be used as tabs maps too*/
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Represents a tabs panel !
    /// </summary>
    [Serializable]
    public class InformationContainerTabsPanel
    {
        /*
         * Warning !
         * * You MUST assign 2 panels, NEVER assign one panel alone ! Add control ids instead.
         * * If you add control ids (at least 1 id), the panels get ignored.
         * 
         * If **>both<** panels are presented do the following:
         * * Create a splitter (2 panels, aligned with 'IsHorizontal') then put each panel at it's location.
         * * Tell each panel to refresh it's view (add controls or panels of it's own)
         * 
         * If **>one<** of the panels is exist do the following:
         * * Treate that panel like this one (delete it, get its control ids then go to the first phase)
         * 
         * If both panels are null; do the following:
         * * Nothing to do ... leave the control empty.
         */
        public InformationContainerTabsPanel()
        {
            this.ContainerIDS = new List<string>();
            firstTime = true;
        }
        ~InformationContainerTabsPanel()
        { Dispose(); }
        [NonSerialized]
        private ManagedTabControl mtc;
        [NonSerialized]
        private SplitContainer splitter;
        private bool firstTime;
        [NonSerialized]
        private bool isLoading;
        [NonSerialized]
        private bool isParentOfDraggedPanel;
        [NonSerialized]
        private const int dragAndDropSensivity = 40;

        /// <summary>
        /// Show this panel
        /// </summary>
        /// <param name="container">The container to hold panel content</param>
        /// <param name="console">The EO element that contain information containers and EO style</param>
        /// <param name="getControl">Delegate to get the IC control</param>
        /// <param name="getImageList">Delegate to get image list object that hold control icons</param>
        /// <param name="getImageIndex">Delegate to get icon index within given image list</param>
        /// <returns>True if the panel show successfully otherwise this panel is useless and need to be removed.</returns>
        public bool ShowPanel(Control container, Console console, GetICControl getControl,
            GetICImageList getImageList, GetICControlImageIndex getImageIndex)
        {
            isLoading = true;
            // Clear the controls in the container ...
            container.Controls.Clear();

            if (ContainerIDS != null)
            {
                if (ContainerIDS.Count > 0)
                {
                    Trace.WriteLine("Container ids mode.");
                    // This is it ! add the tab control
                    // Dispose first
                    if (mtc != null)
                        if (!mtc.IsDisposed)
                            mtc.Dispose();
                    // Create new
                    mtc = new ManagedTabControl();
                    container.Controls.Add(mtc);
                    // Apply style
                    mtc.ImagesList = getImageList();
                    mtc.Dock = DockStyle.Fill;
                    mtc.AllowAutoTabPageDragAndDrop = true;
                    mtc.AllowDrop = true;
                    mtc.DrawStyle = MTCDrawStyle.Flat;
                    mtc.CloseBoxAlwaysVisible = false;
                    mtc.BackColor = console.Style.bkgColor_InformationContainerTabs;
                    mtc.TabPageColor = console.Style.TabPageColor;
                    mtc.TabPageSelectedColor = console.Style.TabPageSelectedColor;
                    mtc.TabPageHighlightedColor = console.Style.TabPageHighlightedColor;
                    mtc.TabPageSplitColor = console.Style.TabPageSplitColor;
                    mtc.ForeColor = console.Style.TabPageTextsColor;
                    // Events
                    mtc.TabPageClose += mtc_TabPageClose;
                    mtc.SelectedTabPageIndexChanged += mtc_TabIndexChanged;
                    mtc.BeforeAutoTabDragAndDrop += mtc_BeforeAutoTabDragAndDrop;
                    mtc.AfterAutoTabDragAndDrop += mtc_AfterAutoTabDragAndDrop;
                    mtc.SelectedTabPageIndexChanged += mtc_SelectedTabPageIndexChanged;
                    mtc.DragDrop += mtc_DragDrop;
                    mtc.DragEnter += mtc_DragEnter;
                    mtc.DragLeave += mtc_DragLeave;
                    mtc.DragOver += mtc_DragOver;
                    mtc.GiveFeedback += mtc_GiveFeedback;
                    // Add the tab pages
                    foreach (string id in this.ContainerIDS)
                    {
                        // Get the id
                        InformationContainer ic = console.GetInformationContainer(id);
                        Control con = getControl(ic, console.ID);
                        if (con != null)
                        {
                            MTCTabPage page = new MTCTabPage();
                            page.Panel = new Panel();
                            page.DrawType = MTCTabPageDrawType.TextAndImage;
                            page.Text = ic.DisplayName;
                            page.ImageIndex = getImageIndex(ic);
                            page.Panel.Controls.Add(con);
                            con.Dock = DockStyle.Fill;
                            con.AllowDrop = true;
                            con.DragOver += mtc_DragOver;
                            con.DragDrop += mtc_DragDrop;
                            page.Tag = id;
                            mtc.TabPages.Add(page);
                        }
                    }
                    isLoading = false;
                    return true;// Done
                }
            }
            int makeSplitter = 0;
            bool isTop = false;
            if (this.BottomPanel != null)
                if (this.BottomPanel.IsVisible())
                { isTop = false; makeSplitter++; }
            if (this.TopPanel != null)
                if (this.TopPanel.IsVisible())
                { isTop = true; makeSplitter++; }
            switch (makeSplitter)
            {
                case 0: return false;// No panel to show.
                case 1:
                    {
                        // One panel to show
                        if (isTop)
                        {
                            this.TopPanel.ShowPanel(container, console, getControl, getImageList, getImageIndex);
                            this.TopPanel.SplitterMoved += TopPanel_SplitterMoved;
                            this.TopPanel.TabClosed += TopPanel_TabClosed;
                            this.TopPanel.TabReordered += TopPanel_TabReordered;
                            this.TopPanel.ReuqestRefresh += TopPanel_ReuqestRefresh;
                            this.TopPanel.TabDragged += BottomPanel_TabDragged;
                            this.TopPanel.TabSelectionChanged += TopPanel_TabSelectionChanged;
                            this.TopPanel.RequestNewPanel += TopPanel_RequestNewPanel;
                        }
                        else
                        {
                            this.BottomPanel.ShowPanel(container, console, getControl, getImageList, getImageIndex);
                            this.BottomPanel.SplitterMoved += TopPanel_SplitterMoved;
                            this.BottomPanel.TabClosed += TopPanel_TabClosed;
                            this.BottomPanel.TabReordered += TopPanel_TabReordered;
                            this.BottomPanel.ReuqestRefresh += TopPanel_ReuqestRefresh;
                            this.BottomPanel.TabDragged += BottomPanel_TabDragged;
                            this.BottomPanel.TabSelectionChanged += BottomPanel_TabSelectionChanged;
                            this.BottomPanel.RequestNewPanel += BottomPanel_RequestNewPanel;
                        }
                        return true;
                    }
                case 2:
                    {
                        // 2 Panels; Make splitters
                        if (splitter != null)
                            if (!splitter.IsDisposed)
                                splitter.Dispose();
                        splitter = new SplitContainer();
                        container.Controls.Add(splitter);
                        splitter.Dock = DockStyle.Fill;
                        splitter.BackColor = console.Style.bkgColor_InformationContainerTabs;
                        splitter.FixedPanel = FixedPanel.Panel1;
                        // Set the splitter to do half. The parent control should do a second stage to update distance
                        splitter.Orientation = IsHorizontal ? Orientation.Horizontal : Orientation.Vertical;
                        if (IsHorizontal)
                            splitter.SplitterDistance = splitter.Height / 2;
                        else
                            splitter.SplitterDistance = splitter.Width / 2;
                        if (firstTime)
                            this.SplitterDistance = splitter.SplitterDistance;// Save value
                        else
                            splitter.SplitterDistance = this.SplitterDistance;// Load value
                        // Events
                        splitter.SplitterMoved += splitter_SplitterMoved;

                        this.TopPanel.SplitterMoved += TopPanel_SplitterMoved;
                        this.TopPanel.TabClosed += TopPanel_TabClosed;
                        this.TopPanel.TabReordered += TopPanel_TabReordered;
                        this.TopPanel.ReuqestRefresh += TopPanel_ReuqestRefresh;
                        this.TopPanel.TabDragged += BottomPanel_TabDragged;
                        this.TopPanel.RequestNewPanel += TopPanel_RequestNewPanel;
                        this.TopPanel.TabSelectionChanged += TopPanel_TabSelectionChanged;

                        this.BottomPanel.SplitterMoved += TopPanel_SplitterMoved;
                        this.BottomPanel.TabClosed += TopPanel_TabClosed;
                        this.BottomPanel.TabReordered += TopPanel_TabReordered;
                        this.BottomPanel.ReuqestRefresh += TopPanel_ReuqestRefresh;
                        this.BottomPanel.TabDragged += BottomPanel_TabDragged;
                        this.BottomPanel.RequestNewPanel += BottomPanel_RequestNewPanel;
                        this.BottomPanel.TabSelectionChanged += BottomPanel_TabSelectionChanged;
                        // Top panel
                        this.TopPanel.ShowPanel(splitter.Panel1, console, getControl, getImageList, getImageIndex);
                        // Bottom panel
                        this.BottomPanel.ShowPanel(splitter.Panel2, console, getControl, getImageList, getImageIndex);
                        return true;
                    }
            }
            // Reached here means nothing to do.
            return false;
        }

        /// <summary>
        /// Add container id to this panel. Calling this assumes this panel has at least one container id.
        /// </summary>
        /// <param name="id">The container id to add</param>
        /// <param name="location">The location where to add; the location can't be IN nor NONE</param>
        public void AddContainerID(string id, TabDragAndDropLocation location)
        {
            // Calling this assumes this panel has at least one container id
            // and the location is not IN nor NONE
            // 1 clear both panels
            if (this.BottomPanel != null)
                this.BottomPanel.Dispose();
            this.BottomPanel = new InformationContainerTabsPanel();
            if (this.TopPanel != null)
                this.TopPanel.Dispose();
            this.TopPanel = new InformationContainerTabsPanel();

            switch (location)
            {
                case TabDragAndDropLocation.Right:
                case TabDragAndDropLocation.Bottom:
                    {
                        // Transfer the ids to the top panel
                        foreach (string cid in this.ContainerIDS)
                        {
                            if (isParentOfDraggedPanel && cid == id)
                                continue;
                            this.TopPanel.ContainerIDS.Add(cid);
                        }
                        this.ContainerIDS.Clear();// Clear ids
                        // Add the new id to the bottom/right panel
                        this.BottomPanel.ContainerIDS.Add(id);
                        // Alignment
                        this.IsHorizontal = location == TabDragAndDropLocation.Bottom;
                        // Done !! request a refresh ...
                        InformationContainerTabsPanel.LastDragDropSuccessed = true;
                        break;
                    }
                case TabDragAndDropLocation.Left:
                case TabDragAndDropLocation.Top:
                    {
                        // Transfer the ids to the bottom panel
                        foreach (string cid in this.ContainerIDS)
                        {
                            if (isParentOfDraggedPanel && cid == id)
                                continue;
                            this.BottomPanel.ContainerIDS.Add(cid);
                        }
                        this.ContainerIDS.Clear();// Clear ids
                        // Add the new id to the top/left panel
                        this.TopPanel.ContainerIDS.Add(id);
                        // Alignment
                        this.IsHorizontal = location == TabDragAndDropLocation.Top;
                        // Done !! request a refresh ...
                        InformationContainerTabsPanel.LastDragDropSuccessed = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Add new container id to a first panel or child panel that already contain container ids, automatically find the proper panel to add.
        /// </summary>
        /// <param name="id">The container id</param>
        /// <returns>True if panel added successfully otherwise false</returns>
        public bool AddNewContainerID(string id)
        {
            // The first priority to the child panel that already contains container ids
            if (this.ContainerIDS != null)
            {
                if (this.ContainerIDS.Count > 0)
                {
                    if (!this.ContainerIDS.Contains(id))
                    {
                        this.ContainerIDS.Insert(0, id);
                        return true;
                    }
                }
            }
            // If the panel doesn't contain any id, check the child panels ...
            if (this.TopPanel != null)
                if (this.TopPanel.AddNewContainerID(id))
                    return true;
            if (this.BottomPanel != null)
                if (this.BottomPanel.AddNewContainerID(id))
                    return true;
            // Reched here means no panel can add id by normal way
            return false;
        }
        /// <summary>
        /// Update actual splitters with the save values.
        /// </summary>
        public void UpdateSplitters()
        {
            int makeSplitters = 0;
            // First priority is for children
            if (this.TopPanel != null)
            { this.TopPanel.UpdateSplitters(); makeSplitters++; }
            if (this.BottomPanel != null)
            { this.BottomPanel.UpdateSplitters(); makeSplitters++; }

            if (makeSplitters == 2)
                if (splitter != null)
                    if (!splitter.IsDisposed)
                        if (!firstTime)
                        {
                            try { splitter.SplitterDistance = this.SplitterDistance; }
                            catch (Exception ex)
                            {
                                if (IsHorizontal)
                                    this.SplitterDistance = splitter.SplitterDistance = splitter.Height / 2;
                                else
                                    this.SplitterDistance = splitter.SplitterDistance = splitter.Width / 2;
                            }
                        }
            if (firstTime)
                this.firstTime = false;
        }
        /// <summary>
        /// Save the splitter values
        /// </summary>
        public void SaveSplitters()
        {
            if (splitter != null)
                if (!splitter.IsDisposed)
                    this.SplitterDistance = splitter.SplitterDistance;
            if (this.BottomPanel != null)
                this.BottomPanel.SaveSplitters();

            if (this.TopPanel != null)
                this.TopPanel.SaveSplitters();
        }
        /// <summary>
        /// Make a fix to remove odd panels.
        /// </summary>
        public void FixPanels()
        { }
        /// <summary>
        /// Get if this panel is visible (or can be visible)
        /// </summary>
        /// <returns></returns>
        public bool IsVisible()
        {
            if (this.ContainerIDS != null)
                if (this.ContainerIDS.Count > 0)
                    return true;
            if (this.BottomPanel != null)
                if (this.BottomPanel.IsVisible())
                    return true;
            if (this.TopPanel != null)
                if (this.TopPanel.IsVisible())
                    return true;
            return false;
        }
        /// <summary>
        /// Search this panel and childs panels to see if a container is visible.
        /// </summary>
        /// <param name="id">The container id</param>
        /// <returns></returns>
        public bool IsContainerVisible(string id)
        {
            // Search this panel container ids
            if (this.ContainerIDS != null)
                foreach (string cid in this.ContainerIDS)
                    if (cid == id)
                        return true;
            // See the child panels...
            if (this.TopPanel != null)
                if (this.TopPanel.IsContainerVisible(id))
                    return true;
            if (this.BottomPanel != null)
                if (this.BottomPanel.IsContainerVisible(id))
                    return true;

            // Reach here means not visible at all.
            return false;
        }
        /// <summary>
        /// Check if a container is selected and visible.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsContainerVisibleAndSelected(string id)
        {
            // Search this panel container ids
            if (this.ContainerIDS != null)
            {
                if (this.ContainerIDS.Count > 0)
                {
                    int i = 0;
                    foreach (string cid in this.ContainerIDS)
                    {
                        if (cid == id)
                        {
                            if (mtc != null)
                            {
                                if (mtc.SelectedTabPageIndex == i)
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return false;
                        }
                        i++;
                    }
                }
            }

            // See the child panels...
            if (this.TopPanel != null)
                if (this.TopPanel.IsContainerVisibleAndSelected(id))
                    return true;
            if (this.BottomPanel != null)
                if (this.BottomPanel.IsContainerVisibleAndSelected(id))
                    return true;

            // Reach here means not visible at all.
            return false;
        }
        /// <summary>
        /// Remove container id from this panel or child panels
        /// </summary>
        /// <param name="id">The container id to remove</param>
        /// <returns></returns>
        public bool CloseContainerID(string id)
        {
            if (this.ContainerIDS != null)
            {
                if (this.ContainerIDS.Contains(id))
                {
                    this.ContainerIDS.Remove(id);
                    return true;
                }
            }
            if (this.BottomPanel != null)
                if (this.BottomPanel.CloseContainerID(id))
                { return true; }
            if (this.TopPanel != null)
                if (this.TopPanel.CloseContainerID(id))
                { return true; }
            return false;
        }
        /// <summary>
        /// Dispose the panel and release all resources.
        /// </summary>
        public void Dispose()
        {
            isLoading = false;
            // Events ! dispose all events !
            if (TopPanel != null)
            {
                TopPanel.Dispose();
                this.TopPanel.SplitterMoved -= TopPanel_SplitterMoved;
                this.TopPanel.TabClosed -= TopPanel_TabClosed;
                this.TopPanel.TabReordered -= TopPanel_TabReordered;
                this.TopPanel.ReuqestRefresh -= TopPanel_ReuqestRefresh;
                this.TopPanel.TabDragged -= BottomPanel_TabDragged;
                this.TopPanel.RequestNewPanel -= TopPanel_RequestNewPanel;
                this.TopPanel.TabSelectionChanged -= TopPanel_TabSelectionChanged;
            }
            if (BottomPanel != null)
            {
                BottomPanel.Dispose();
                this.BottomPanel.SplitterMoved -= TopPanel_SplitterMoved;
                this.BottomPanel.TabClosed -= TopPanel_TabClosed;
                this.BottomPanel.TabReordered -= TopPanel_TabReordered;
                this.BottomPanel.ReuqestRefresh -= TopPanel_ReuqestRefresh;
                this.BottomPanel.TabDragged -= BottomPanel_TabDragged;
                this.BottomPanel.RequestNewPanel -= BottomPanel_RequestNewPanel;
                this.BottomPanel.TabSelectionChanged -= BottomPanel_TabSelectionChanged;
            }
            SplitterMoved = null;
            TabClosed = null;
            TabReordered = null;
            TabDragged = null;
            ReuqestRefresh = null;
            RequestNewPanel = null;
            TabSelectionChanged = null;
            // Controls
            if (mtc != null)
            {
                // if (!mtc.IsDisposed)
                {
                    foreach (MTCTabPage page in mtc.TabPages)
                    {
                        if (page.Panel != null)
                        {
                            //if (!page.Panel.IsDisposed)
                            {
                                if (page.Panel.Controls.Count > 0)
                                {
                                    page.Panel.Controls[0].DragOver -= mtc_DragOver;
                                    page.Panel.Controls[0].DragDrop -= mtc_DragDrop;
                                    page.Panel.Controls[0].Dispose();
                                }
                                page.Panel.Controls.Clear();
                                page.Panel.Dispose();
                            }
                            page.Panel = null;
                        }
                    }
                    mtc.TabPageClose -= mtc_TabPageClose;
                    mtc.SelectedTabPageIndexChanged -= mtc_TabIndexChanged;
                    mtc.BeforeAutoTabDragAndDrop -= mtc_BeforeAutoTabDragAndDrop;
                    mtc.AfterAutoTabDragAndDrop -= mtc_AfterAutoTabDragAndDrop;
                    mtc.DragDrop -= mtc_DragDrop;
                    mtc.DragEnter -= mtc_DragEnter;
                    mtc.DragLeave -= mtc_DragLeave;
                    mtc.DragOver -= mtc_DragOver;
                    mtc.GiveFeedback -= mtc_GiveFeedback;
                    mtc.SelectedTabPageIndexChanged -= mtc_SelectedTabPageIndexChanged;
                    mtc.Dispose();
                }
                mtc = null;
            }
            if (splitter != null)
            {
                // if (!splitter.IsDisposed)
                {
                    splitter.SplitterMoved -= splitter_SplitterMoved;
                    splitter.Dispose();
                }
                splitter = null;
            }
            GC.Collect();// destroy all !
        }
        /// <summary>
        /// Get all available MTC controls that contain containers.
        /// </summary>
        /// <returns></returns>
        public List<ManagedTabControl> GetMTCControls()
        {
            List<ManagedTabControl> mtcs = new List<ManagedTabControl>();
            if (this.ContainerIDS.Count > 0 && this.mtc != null)
            {
                mtcs.Add(mtc); goto RETURNTOPARENT;
            }
            // Internal controls.
            if (this.BottomPanel != null)
                mtcs.AddRange(this.BottomPanel.GetMTCControls());
            if (this.TopPanel != null)
                mtcs.AddRange(this.TopPanel.GetMTCControls());

            RETURNTOPARENT:
            return mtcs;
        }
        /// <summary>
        /// Apply style for tabs
        /// </summary>
        /// <param name="style"></param>
        public void ApplyTabsStyle(EOStyle style)
        {
            if (mtc != null)
            {
                if (!mtc.IsDisposed)
                {
                    mtc.BackColor = style.bkgColor_InformationContainerTabs;
                    mtc.TabPageColor = style.TabPageColor;
                    mtc.TabPageSelectedColor = style.TabPageSelectedColor;
                    mtc.TabPageHighlightedColor = style.TabPageHighlightedColor;
                    mtc.TabPageSplitColor = style.TabPageSplitColor;
                    mtc.ForeColor = style.TabPageTextsColor;
                    foreach (MTCTabPage page in mtc.TabPages)
                    {
                        if (page.Panel != null)
                        {
                            if (!page.Panel.IsDisposed)
                            {
                                if (page.Panel.Controls.Count == 1)
                                {
                                    ((IStylable)page.Panel.Controls[0]).ApplyStyle(style);
                                }
                            }
                        }
                    }
                }
            }
            if (this.BottomPanel != null)
                this.BottomPanel.ApplyTabsStyle(style);

            if (this.TopPanel != null)
                this.TopPanel.ApplyTabsStyle(style);
        }
        public void ToggleTabsCloseButtonVisible(bool closeButtonVisible)
        {
            if (mtc != null)
            {
                if (!mtc.IsDisposed)
                {
                    mtc.CloseBoxOnEachPageVisible = closeButtonVisible;
                    mtc.AllowAutoTabPageDragAndDrop = closeButtonVisible;
                    mtc.AllowTabPageDragAndDrop = closeButtonVisible;
                    mtc.AllowTabPagesReorder = closeButtonVisible;
                }
            }
            if (this.BottomPanel != null)
                this.BottomPanel.ToggleTabsCloseButtonVisible(closeButtonVisible);

            if (this.TopPanel != null)
                this.TopPanel.ToggleTabsCloseButtonVisible(closeButtonVisible);
        }
        public void ReplaceID(string id, string newID, bool createNewIfNotExist)
        {
            if (this.ContainerIDS != null)
            {
                if (this.ContainerIDS.Contains(id))
                {
                    if (this.ContainerIDS.Count > 1)
                    {
                        int index = this.ContainerIDS.IndexOf(id);

                        this.ContainerIDS.Remove(id);
                        this.ContainerIDS.Insert(index, newID);
                    }
                    else
                    {
                        this.ContainerIDS.Remove(id);
                        this.ContainerIDS.Add(newID);
                    }
                    return;
                }
            }
            // Check if already there
            if (this.BottomPanel != null)
            {
                if (this.BottomPanel.IsContainerVisible(id))
                {
                    this.BottomPanel.ReplaceID(id, newID, createNewIfNotExist);
                    return;
                }
            }
            if (this.TopPanel != null)
            {
                if (this.TopPanel.IsContainerVisible(id))
                {
                    this.TopPanel.ReplaceID(id, newID, createNewIfNotExist);
                    return;
                }
            }
            // Reached here means this ic is not found at all
            if (createNewIfNotExist)
            {
                this.ContainerIDS.Add(newID);
            }
        }
        /// <summary>
        /// Remove an id if found
        /// </summary>
        /// <param name="id"></param>
        public void RemoveID(string id)
        {
            if (this.ContainerIDS != null)
            {
                if (this.ContainerIDS.Contains(id))
                {
                    this.ContainerIDS.Remove(id);
                }
            }
            if (this.BottomPanel != null)
                this.BottomPanel.RemoveID(id);
            if (this.TopPanel != null)
                this.TopPanel.RemoveID(id);
        }
        public bool HasControls
        {
            get
            {
                if (this.ContainerIDS != null)
                {
                    if (ContainerIDS.Count > 0)
                        return true;
                }
                if (this.BottomPanel != null)
                    if (this.BottomPanel.HasControls)
                        return true;
                if (this.TopPanel != null)
                    if (this.TopPanel.HasControls)
                        return true;
                return false;
            }
        }
        public void ToggleCloseButtonVisible(bool showCloseButtons, bool closeButtonsAlwaysVisible, bool showIcons)
        {
            // Search this panel mtc
            if (this.ContainerIDS != null)
            {
                if (mtc != null)
                {
                    mtc.CloseBoxOnEachPageVisible = showCloseButtons;
                    mtc.CloseBoxAlwaysVisible = closeButtonsAlwaysVisible;
                    foreach (MTCTabPage tab in mtc.TabPages)
                        tab.DrawType = showIcons ? MTCTabPageDrawType.TextAndImage : MTCTabPageDrawType.Text;
                }
            }
            // See the child panels...
            if (this.TopPanel != null)
                this.TopPanel.ToggleCloseButtonVisible(showCloseButtons, closeButtonsAlwaysVisible, showIcons);

            if (this.BottomPanel != null)
                this.BottomPanel.ToggleCloseButtonVisible(showCloseButtons, closeButtonsAlwaysVisible, showIcons);
        }
        public InformationContainerTabsPanel Clone()
        {
            InformationContainerTabsPanel newTAB = new InformationContainerTabsPanel();
            newTAB.ContainerIDS = new List<string>(ContainerIDS);
            newTAB.IsHorizontal = IsHorizontal;
            newTAB.SplitterDistance = SplitterDistance;
            if (TopPanel != null)
                newTAB.TopPanel = TopPanel.Clone();
            if (BottomPanel != null)
                newTAB.BottomPanel = BottomPanel.Clone();

            return newTAB;
        }

        // Events handling.
        private void mtc_TabPageClose(object sender, MTCTabPageCloseArgs e)
        {
            if (isLoading) return;
            // Tab closed ... Remove the id from the container ids.
            ContainerIDS.Remove((string)mtc.TabPages[e.TabPageIndex].Tag);
            // If containers count become zero, request a refresh from parent to handle panels delete.
            if (this.ContainerIDS.Count == 0)
            {
                if (ReuqestRefresh != null)
                    ReuqestRefresh(this, new EventArgs());
            }
            // Raise the event
            if (TabClosed != null)
                TabClosed(this, new EventArgs());
        }
        private void mtc_TabIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            // Tab reordered ... Update container ids order.
            // Clear all
            ContainerIDS.Clear();
            // Add page by page
            foreach (MTCTabPage t in mtc.TabPages)
                ContainerIDS.Add((string)t.Tag);
            if (TabReordered != null)
                TabReordered(this, new EventArgs());
        }
        private void splitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitter != null)
                // Update value
                this.SplitterDistance = splitter.SplitterDistance;
            // Raise event
            if (SplitterMoved != null)
                SplitterMoved(this, new EventArgs());
        }
        private void TopPanel_SplitterMoved(object sender, EventArgs e)
        {
            if (splitter != null)
                // Update value
                this.SplitterDistance = splitter.SplitterDistance;
            // Raise event
            if (SplitterMoved != null)
                SplitterMoved(this, new EventArgs());
        }
        private void TopPanel_TabClosed(object sender, EventArgs e)
        {
            // Deliver the event to parent
            if (TabClosed != null)
                TabClosed(this, new EventArgs());
        }
        private void TopPanel_TabReordered(object sender, EventArgs e)
        {
            if (TabReordered != null)
                TabReordered(this, new EventArgs());
        }
        private void TopPanel_ReuqestRefresh(object sender, EventArgs e)
        {
            // Tell the parent that we need a refresh !
            if (ReuqestRefresh != null)
                ReuqestRefresh(this, new EventArgs());
        }
        private void BottomPanel_TabDragged(object sender, EventArgs e)
        {
            if (TabDragged != null)
                TabDragged(this, new EventArgs());
        }
        /*Drag and drop; user can use drag and drop to change tab locations*/
        private void mtc_BeforeAutoTabDragAndDrop(object sender, EventArgs e)
        {
            InformationContainerTabsPanel.DoingDragAndDrop = isParentOfDraggedPanel = true;
            InformationContainerTabsPanel.LastDragDropSuccessed = false;
            InformationContainerTabsPanel.LastDragDropFinished = false;
            InformationContainerTabsPanel.DraggedControlID = (string)mtc.TabPages[mtc.SelectedTabPageIndex].Tag;
        }
        private void mtc_AfterAutoTabDragAndDrop(object sender, EventArgs e)
        {
            ProfileManager pmanager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
            if (pmanager.IsSaving)
                return;
            if (isParentOfDraggedPanel)
            {
                InformationContainerTabsPanel.DoingDragAndDrop = isParentOfDraggedPanel = false;
                if (InformationContainerTabsPanel.LastDragDropSuccessed)
                {
                    InformationContainerTabsPanel.LastDragDropSuccessed = false;
                    // Delete the container id from this panel
                    if (this.ContainerIDS.Contains(InformationContainerTabsPanel.DraggedControlID))
                        this.ContainerIDS.Remove(InformationContainerTabsPanel.DraggedControlID);
                    if (ReuqestRefresh != null)
                        ReuqestRefresh(this, new EventArgs());

                    if (TabDragged != null)
                        TabDragged(this, new EventArgs());
                    InformationContainerTabsPanel.LastDragDropFinished = true;
                }
            }
        }
        private void mtc_DragOver(object sender, DragEventArgs e)
        {
            if (!InformationContainerTabsPanel.DoingDragAndDrop)
                return;
            if (mtc == null)// just in case ...
                return;
            ProfileManager pmanager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
            if (pmanager.IsSaving)
                return;
            if (e.Data.GetDataPresent(typeof(MTCTabPage)))
            {
                Point point = mtc.PointToClient(new Point(e.X, e.Y));

                e.Effect = DragDropEffects.Move;
                if (point.Y < dragAndDropSensivity)
                {
                    InformationContainerTabsPanel.CurrentDragAndDropLocation = TabDragAndDropLocation.Top;
                }
                else if (point.Y > mtc.Height - dragAndDropSensivity)
                {
                    InformationContainerTabsPanel.CurrentDragAndDropLocation = TabDragAndDropLocation.Bottom;
                }
                else if (point.X < dragAndDropSensivity)
                {
                    InformationContainerTabsPanel.CurrentDragAndDropLocation = TabDragAndDropLocation.Left;
                }
                else if (point.X > mtc.Width - dragAndDropSensivity)
                {
                    InformationContainerTabsPanel.CurrentDragAndDropLocation = TabDragAndDropLocation.Right;
                }
                else
                {
                    if (!isParentOfDraggedPanel)
                        InformationContainerTabsPanel.CurrentDragAndDropLocation = TabDragAndDropLocation.In;
                    else
                        InformationContainerTabsPanel.CurrentDragAndDropLocation = TabDragAndDropLocation.None;
                }
            }
        }
        private void mtc_DragLeave(object sender, EventArgs e)
        {

        }
        private void mtc_DragEnter(object sender, DragEventArgs e)
        {
        }
        private void mtc_DragDrop(object sender, DragEventArgs e)
        {
            if (InformationContainerTabsPanel.LastDragDropFinished) return;
            if (e.Data.GetDataPresent(typeof(MTCTabPage)))
            {
                switch (InformationContainerTabsPanel.CurrentDragAndDropLocation)
                {
                    case TabDragAndDropLocation.Top:
                    case TabDragAndDropLocation.Right:
                    case TabDragAndDropLocation.Left:
                    case TabDragAndDropLocation.Bottom:
                        {
                            // Request a new panel from parent panel
                            string id = (string)((MTCTabPage)e.Data.GetData(typeof(MTCTabPage))).Tag;
                            if (RequestNewPanel != null)
                                RequestNewPanel(this, new NewICPanelRequestArgs(id, InformationContainerTabsPanel.CurrentDragAndDropLocation));
                            break;
                        }
                    case TabDragAndDropLocation.In:// The user want to drop the control here !
                        {
                            if (isParentOfDraggedPanel) return;// This control is the one that doing the drag and drop !

                            string id = (string)((MTCTabPage)e.Data.GetData(typeof(MTCTabPage))).Tag;
                            if (!ContainerIDS.Contains(id))
                            {
                                // Add control to the collection
                                ContainerIDS.Add(id);

                                InformationContainerTabsPanel.LastDragDropSuccessed = true;
                            }
                            Trace.TraceWarning("Drag Drop: add control");
                            break;
                        }
                }
            }
        }
        private void mtc_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (!InformationContainerTabsPanel.DoingDragAndDrop)
                return;
            if (e.Effect == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                switch (InformationContainerTabsPanel.CurrentDragAndDropLocation)
                {
                    case TabDragAndDropLocation.Bottom:
                        {
                            Cursor.Current = Cursors.PanSouth;
                            break;
                        }
                    case TabDragAndDropLocation.Left:
                        {
                            Cursor.Current = Cursors.PanWest;
                            break;
                        }
                    case TabDragAndDropLocation.Right:
                        {
                            Cursor.Current = Cursors.PanEast;
                            break;
                        }
                    case TabDragAndDropLocation.Top:
                        {
                            Cursor.Current = Cursors.PanNorth;
                            break;
                        }
                    case TabDragAndDropLocation.In:
                        {
                            Cursor.Current = Cursors.Cross;
                            break;
                        }
                    case TabDragAndDropLocation.None:
                        {
                            Cursor.Current = Cursors.No;
                            break;
                        }
                }
            }
        }
        private void mtc_SelectedTabPageIndexChanged(object sender, EventArgs e)
        {
            if (TabSelectionChanged != null)
                TabSelectionChanged(sender, e);
        }
        private void BottomPanel_TabSelectionChanged(object sender, EventArgs e)
        {
            if (TabSelectionChanged != null)
                TabSelectionChanged(sender, e);
        }
        private void TopPanel_TabSelectionChanged(object sender, EventArgs e)
        {
            if (TabSelectionChanged != null)
                TabSelectionChanged(sender, e);
        }
        // New panel request from internal panel;
        private void BottomPanel_RequestNewPanel(object sender, NewICPanelRequestArgs e)
        {
            ProfileManager pmanager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
            if (pmanager.IsSaving)
                return;
            if (InformationContainerTabsPanel.LastDragDropFinished) return;

            this.BottomPanel.AddContainerID(e.ContainerID, e.Location);
        }
        private void TopPanel_RequestNewPanel(object sender, NewICPanelRequestArgs e)
        {
            ProfileManager pmanager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
            if (pmanager.IsSaving)
                return;
            if (InformationContainerTabsPanel.LastDragDropFinished) return;

            this.TopPanel.AddContainerID(e.ContainerID, e.Location);
        }
        // Events
        /// <summary>
        /// Raised when the splitter control is moved.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler SplitterMoved;
        /// <summary>
        /// Raised when a tab closed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler TabClosed;
        /// <summary>
        /// Raised when a tab selected
        /// </summary>
        [field: NonSerialized]
        public event EventHandler TabSelectionChanged;
        /// <summary>
        /// Raised when a tab get reorderedd
        /// </summary>
        [field: NonSerialized]
        public event EventHandler TabReordered;
        /// <summary>
        /// Drag and drop operation finsihed; a tab get dragged.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler TabDragged;
        /// <summary>
        /// Raised when the panel needs to be refreshed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ReuqestRefresh;
        /// <summary>
        /// Raised when an internal panel request a new panel from parent panel.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<NewICPanelRequestArgs> RequestNewPanel;

        // Properties
        public List<string> ContainerIDS { get; set; }
        public bool IsHorizontal { get; set; }
        public InformationContainerTabsPanel TopPanel { get; set; }
        public InformationContainerTabsPanel BottomPanel { get; set; }
        public int SplitterDistance { get; set; }

        #region Static members; important for static operations such as drag-and-drop
        /// <summary>
        /// Current drag and drop location, set when a drag and drop get over a tabs control.
        /// </summary>
        [NonSerialized]
        public static TabDragAndDropLocation CurrentDragAndDropLocation;
        /// <summary>
        /// Set when a drag-and-drop operation finished successfully.
        /// </summary>
        [NonSerialized]
        public static bool LastDragDropSuccessed;
        /// <summary>
        /// Set when a drag-and-drop operation finished.
        /// </summary>
        [NonSerialized]
        public static bool LastDragDropFinished;
        /// <summary>
        /// Set when a drag-and-drop operation occuring
        /// </summary>
        [NonSerialized]
        public static bool DoingDragAndDrop;
        /// <summary>
        /// The dragged control id
        /// </summary>
        [NonSerialized]
        public static string DraggedControlID;
        #endregion
    }
    public enum TabDragAndDropLocation
    {
        None, Bottom, Left, Top, Right, In
    }
    public class NewICPanelRequestArgs : EventArgs
    {
        public NewICPanelRequestArgs(string containerID, TabDragAndDropLocation location)
        {
            this.containerID = containerID;
            this.location = location;
        }
        private string containerID;
        private TabDragAndDropLocation location;

        public string ContainerID { get { return containerID; } }
        public TabDragAndDropLocation Location { get { return location; } }
    }
}
