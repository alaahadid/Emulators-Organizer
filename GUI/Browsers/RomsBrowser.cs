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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MLV;
using MMB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace EmulatorsOrganizer.GUI
{
    public partial class RomsBrowser : IBrowserControl
    {
        public RomsBrowser()
        {
            InitializeComponent();
            base.InitializeEvents();
            //   InitializeEvents(); 
            managedListView1.AutoSetWheelScrollSpeed = true;
            // TODO: don't forget the download games from db
            getDataFromTheGamesDBForThisGameToolStripMenuItem.Visible = false;
        }

        private RomsCollection current_roms = null;
        public IEOElement element;
        private EOStyle defaultStyle = new EOStyle();
        private List<string> draggedItemIDS = new List<string>();
        private bool oneOfDraggedIsParent;
        private List<string> consoleImageIDs = new List<string>();
        private Dictionary<string, RomThumbnailInfo> roms_thumbs;
        private int oldP = -1;
        private int selectionTimer;
        const int selectionTimerReload = 1;
        private bool itemsDrag = false;
        private bool inListDrop = false;
        private bool doSearch = false;
        private bool autoCycleThumbnails = true;
        private SearchRequestArgs searchParameters;
        private bool isSelecting;
        private bool isLoading;
        private List<Image> ThumbnailsCache = new List<Image>();
        private List<string> ThumbnailsCacheIDS = new List<string>();
        private List<Image> DetailsCache = new List<Image>();
        private List<string> DetailsCacheIDS = new List<string>();
        private List<string> DetailsCacheNames = new List<string>();
        private List<string> selectedRomsIDS = new List<string>();
        private int ThumbnailsCacheMaxSize = 100;
        private int DetailsCacheMaxSize = 100;
        private ThumbnailsMode currentSelectedThumbMode;
        private Thread reload_thread;
        private Color list_view_temp_color;
        private bool list_view_parentMode;
        private bool list_view_selectRoms;
        private List<string> list_view_selectedRoms;
        private bool thumb_info_show_all_info;
        private bool thumb_info_show_rating;
        private bool SelectTheSameRoms;

        private delegate void AddListViewItem(ManagedListViewItem item);
        private delegate void InsertListViewItem(int index, ManagedListViewItem item);
        private delegate void ShowProgressDelegate(string status, int prec);

        private void AddNewItem(ManagedListViewItem item)
        {
            if (!this.InvokeRequired)
                managedListView1.Items.AddNoEvent(item);
            else
                this.Invoke(new AddListViewItem(managedListView1.Items.AddNoEvent), item);
        }
        private void InsertNewItem(int index, ManagedListViewItem item)
        {
            if (!this.InvokeRequired)
                managedListView1.Items.InsertNoEvent(index, item);
            else
                this.Invoke(new InsertListViewItem(managedListView1.Items.InsertNoEvent), index, item);
        }
        private void RefreshListViewScrollBars()
        {
            if (!this.InvokeRequired)
                managedListView1.RefreshScrollBarsView();
            else
                this.Invoke(new Action(managedListView1.RefreshScrollBarsView));
        }
        private void OnProgressThreaded(string status, int prec)
        {
            if (!this.InvokeRequired)
                OnProgress(status, prec);
            else
                this.Invoke(new ShowProgressDelegate(OnProgress), status, prec);
        }
        public override void InitializeEvents()
        {
            profileManager.Profile.ConsolesGroupSelectionChanged += Profile_ConsolesGroupSelectionChanged;
            profileManager.Profile.ConsoleSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsoleRemoved += Profile_ConsoleRemoved;
            profileManager.Profile.PlaylistsGroupSelectionChanged += Profile_PlaylistsGroupSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged += Profile_PlaylistSelectionChanged;
            profileManager.Profile.RomsRefreshRequest += Profile_RomsRefreshRequest;
            profileManager.Profile.RomsAdded += Profile_RomsAdded;
            profileManager.Profile.ConsolePropertiesChanged += Profile_ConsolePropertiesChanged;
            profileManager.Profile.RomsRemoved += Profile_RomsRemoved;
            profileManager.Profile.RomIconsChanged += Profile_RomIconsChanged;
            profileManager.Profile.RomRenamed += Profile_RomRenamed;
            profileManager.Profile.RomsRemovedFromPlaylist += Profile_RomsRemovedFromPlaylist;
            profileManager.Profile.RomPropertiesChanged += Profile_RomPropertiesChanged;
            profileManager.Profile.RomMultiplePropertiesChanged += Profile_RomMultiplePropertiesChanged;
            profileManager.Profile.RomFinishedPlayed += Profile_RomFinishedPlayed;
            profileManager.Profile.RequestSearch += Profile_RequestSearch;
            profileManager.Profile.InformationContainerItemsDetected += Profile_InformationContainerItemsDetected;
            profileManager.Profile.InformationContainerItemsModified += Profile_InformationContainerItemsModified;
            profileManager.Profile.ProfileCleanUpFinished += Profile_ProfileCleanUpFinished;
            profileManager.Profile.DatabaseImported += Profile_DatabaseImported;
            profileManager.Profile.RomSelectionChanged += Profile_RomSelectionChanged;
            profileManager.Profile.PlaylistRemoved += Profile_PlaylistRemoved;
            profileManager.Profile.PlaylistsGroupRemoved += Profile_PlaylistsGroupRemoved;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.ConsolesGroupSelectionChanged -= Profile_ConsolesGroupSelectionChanged;
            profileManager.Profile.ConsoleSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsoleRemoved -= Profile_ConsoleRemoved;
            profileManager.Profile.PlaylistsGroupSelectionChanged -= Profile_PlaylistsGroupSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged -= Profile_PlaylistSelectionChanged;
            profileManager.Profile.RomsRefreshRequest -= Profile_RomsRefreshRequest;
            profileManager.Profile.RomsAdded -= Profile_RomsAdded;
            profileManager.Profile.ConsolePropertiesChanged -= Profile_ConsolePropertiesChanged;
            profileManager.Profile.RomsRemoved -= Profile_RomsRemoved;
            profileManager.Profile.RomIconsChanged -= Profile_RomIconsChanged;
            profileManager.Profile.RomRenamed -= Profile_RomRenamed;
            profileManager.Profile.RomsRemovedFromPlaylist -= Profile_RomsRemovedFromPlaylist;
            profileManager.Profile.RomPropertiesChanged -= Profile_RomPropertiesChanged;
            profileManager.Profile.RomMultiplePropertiesChanged -= Profile_RomMultiplePropertiesChanged;
            profileManager.Profile.RomFinishedPlayed -= Profile_RomFinishedPlayed;
            profileManager.Profile.RequestSearch -= Profile_RequestSearch;
            profileManager.Profile.InformationContainerItemsDetected -= Profile_InformationContainerItemsDetected;
            profileManager.Profile.InformationContainerItemsModified -= Profile_InformationContainerItemsModified;
            profileManager.Profile.ProfileCleanUpFinished -= Profile_ProfileCleanUpFinished;
            profileManager.Profile.DatabaseImported -= Profile_DatabaseImported;
            profileManager.Profile.RomSelectionChanged -= Profile_RomSelectionChanged;
            profileManager.Profile.PlaylistRemoved -= Profile_PlaylistRemoved;
            profileManager.Profile.PlaylistsGroupRemoved -= Profile_PlaylistsGroupRemoved;
        }
        public override void LoadControlSettings()
        {
            base.LoadControlSettings();
            bool thumb = (bool)settings.GetValue("RomsBrowser:IsThumbnailViewMode", true, false);
            managedListView1.ViewMode = thumb ? ManagedListViewViewMode.Thumbnails : ManagedListViewViewMode.Details;
            trackBar_thumbSize.Value = (int)settings.GetValue("RomsBrowser:ThumbnailViewSize", true, 100);
            managedListView1.ThunmbnailsHeight = managedListView1.ThunmbnailsWidth = trackBar_thumbSize.Value;
            autoCycleThumbnails = (bool)settings.GetValue("RomsBrowser:AutoCycleThumbs", true, true);
            timer_thumbCycle.Interval = (int)settings.GetValue("RomsBrowser:AutoCycleThumbsInterval", true, 2000);
            managedListView1.ShowSubItemToolTip = (bool)settings.GetValue("RomsBrowser:ShowSubItemToolTip", true, true);
            // Tooltip on thumb mode
            managedListView1.ShowItemInfoOnThumbnailMode = (bool)settings.GetValue("RomsBrowser:ShowTooltipThumbMode", true, true);
            thumb_info_show_all_info = (bool)settings.GetValue("RomsBrowser:ShowTooltipThumbModeAllInfo", true, false);
            thumb_info_show_rating = (bool)settings.GetValue("RomsBrowser:ShowTooltipThumbModeRating", true, false);
        }
        public void RefreshParentSelection(bool selectRoms, List<string> selectedRoms)
        {
            if (profileManager.IsSaving)
                return;// refreshing is not allowing while saving current element.
            if (isLoading)
                return;
            RefreshThumbModes();
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.ConsolesGroup:
                    {
                        element = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        if (element == null) return;
                        RefreshColumns();

                        // Status
                        SelectionStatus.Image = element.Icon != null ? element.Icon : Properties.Resources.folder;
                        SelectionStatus.Text = ls["Status_ConsolesGroup"] + ": " + element.Name;
                        if (profileManager.Profile.ActiveCategories.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedCategories"] + "]";
                        }
                        if (profileManager.Profile.ActiveFilters.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedFilters"] + "]";
                        }

                        ReloadRoms(element.Style.listviewTextsColor, false, selectRoms, selectedRoms);
                        break;
                    }
                case SelectionType.Console:
                    {
                        element = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        if (element == null) return;
                        RefreshColumns();
                        // Status
                        SelectionStatus.Image = element.Icon != null ? element.Icon : Properties.Resources.Console;
                        if (((Core.Console)element).ParentGroupID != "")
                        {
                            string parentName = profileManager.Profile.ConsoleGroups[((Core.Console)element).ParentGroupID].Name;
                            SelectionStatus.Text = ls["Status_Console"] + ": " + parentName + "/" + element.Name;
                        }
                        else
                        {
                            SelectionStatus.Text = ls["Status_Console"] + ": " + element.Name;
                        }
                        if (profileManager.Profile.ActiveCategories.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedCategories"] + "]";
                        }
                        if (profileManager.Profile.ActiveFilters.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedFilters"] + "]";
                        }
                        ReloadRoms(element.Style.listviewTextsColor, ((EmulatorsOrganizer.Core.Console)element).ParentAndChildrenMode, selectRoms, selectedRoms);

                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        element = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                        if (element == null) return;
                        RefreshColumns();

                        // Status
                        SelectionStatus.Image = element.Icon != null ? element.Icon : Properties.Resources.folder;
                        SelectionStatus.Text = ls["Name_PlaylistsGroup"] + ": " + element.Name;
                        if (profileManager.Profile.ActiveCategories.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedCategories"] + "]";
                        }
                        if (profileManager.Profile.ActiveFilters.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedFilters"] + "]";
                        }

                        ReloadRoms(element.Style.listviewTextsColor, false, selectRoms, selectedRoms);
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        element = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        if (element == null) return;
                        RefreshColumns();

                        // Status
                        SelectionStatus.Image = element.Icon != null ? element.Icon : Properties.Resources.Favorites;
                        if (((Playlist)element).ParentGroupID != "")
                        {
                            string parentName = profileManager.Profile.PlaylistGroups[((Playlist)element).ParentGroupID].Name;
                            SelectionStatus.Text = ls["Status_Playlist"] + ": " + parentName + "/" + element.Name;
                        }
                        else
                        {
                            SelectionStatus.Text = ls["Status_Playlist"] + ": " + element.Name;
                        }
                        if (profileManager.Profile.ActiveCategories.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedCategories"] + "]";
                        }
                        if (profileManager.Profile.ActiveFilters.Count > 0)
                        {
                            SelectionStatus.Text += " [" + ls["Status_FilteredBySelectedFilters"] + "]";
                        }
                        ReloadRoms(element.Style.listviewTextsColor, false, selectRoms, selectedRoms);
                        break;
                    }

                case SelectionType.None:
                default:
                    {
                        element = null; RefreshColumns();
                        // Clear
                        ThumbnailsCache = new List<Image>();
                        ThumbnailsCacheIDS = new List<string>();
                        if (roms_thumbs == null)
                            roms_thumbs = new Dictionary<string, RomThumbnailInfo>();
                        roms_thumbs.Clear();
                        DetailsCache = new List<Image>();
                        DetailsCacheNames = new List<string>();
                        DetailsCacheIDS = new List<string>();
                        managedListView1.Items.Clear();
                        imageList_listView.Images.Clear();
                        consoleImageIDs.Clear();
                        OnProgressStarted();
                        profileManager.Profile.SelectedRomIDS.Clear();
                        break;
                    }
            }
            if (doSearch && profileManager.Profile.RecentSelectedType != SelectionType.None)
            {
                SelectionStatus.Text += " [" + ls["Status_SearchResult"] + "]";
            }
            RefreshStyle();
            OnEnableDisableButtons();
        }
        private void ReloadRoms(Color textColor, bool parentMode, bool selectRoms, List<string> selectedRoms)
        {
            if (isLoading)
                return;
            if (reload_thread != null)
            {
                if (reload_thread.IsAlive)
                {
                    reload_thread.Abort();
                    reload_thread = null;
                }
            }
            ThumbnailsCache = new List<Image>();
            ThumbnailsCacheIDS = new List<string>();
            if (roms_thumbs == null)
                roms_thumbs = new Dictionary<string, RomThumbnailInfo>();
            roms_thumbs.Clear();
            DetailsCache = new List<Image>();
            DetailsCacheNames = new List<string>();
            DetailsCacheIDS = new List<string>();
            managedListView1.Items.Clear();
            imageList_listView.Images.Clear();
            consoleImageIDs.Clear();
            OnProgressStarted();
            profileManager.Profile.SelectedRomIDS.Clear();

            // Start the thread
            list_view_temp_color = textColor;
            list_view_parentMode = parentMode;
            list_view_selectRoms = selectRoms;
            list_view_selectedRoms = selectedRoms;
            reload_thread = new Thread(new ThreadStart(ReloadRomsThreaded));
            reload_thread.CurrentUICulture = ls.CultureInfo;
            reload_thread.Start();
        }
        private void ReloadRomsThreaded()
        {
            isLoading = true;
            profileManager.Profile.OnRomsLoadingStarted();
            if (current_roms != null)
            {
                current_roms.Clear();
                current_roms = null;
            }

            OnProgressThreaded(ls["Status_LoadingRomsFromCache"] + "...", 0);
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.ConsolesGroup:
                    {
                        current_roms = new RomsCollection(null, false, profileManager.Profile.Roms.GetRomsByConsolesGroup(profileManager.Profile.SelectedConsolesGroupID));
                        break;
                    }
                case SelectionType.Console:
                    {
                        current_roms = new RomsCollection(null, false, profileManager.Profile.Roms[profileManager.Profile.SelectedConsoleID, false]);
                        /*current_roms = new RomsCollection(null, false, profileManager.Profile.Roms.GetRomsForConsoleNotCahced(profileManager.Profile.SelectedConsoleID));
                        if (current_roms.Count == 0)
                        {
                            if (profileManager.IsCacheAvailable)
                            {
                                // Load the roms in the old fashion way
                                profileManager.OpenCacheStream();

                                // Get the console info
                                ProfileManager.CachedConsoleInfo cahced_cons = profileManager.GetConsoleInfo(profileManager.Profile.SelectedConsoleID);
                                if (cahced_cons.ID != "")
                                {
                                    // Get the roms and add them !
                                    for (int r = 0; r < cahced_cons.RomsCount; r++)
                                    {
                                        int x = (r * 100) / cahced_cons.RomsCount;
                                        if (x != oldP)
                                        {
                                            OnProgressThreaded(ls["Status_LoadingRomsFromCache"] + "...", x);
                                            oldP = x;
                                        }
                                        ProfileManager.CachedRomData rominf = profileManager.GetNextRom();
                                        if (rominf.ID != "")
                                        {
                                            //Add it !
                                            Rom theRom = profileManager.GetRomFromData(rominf.DataCompressed);

                                            profileManager.Profile.Roms.Add(theRom, false);
                                            current_roms.Add(theRom);
                                        }
                                    }
                                }

                                profileManager.CloseCacheStream();
                            }
                        }
                        */
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        current_roms = new RomsCollection(null, false);
                        foreach (Playlist pl in profileManager.Profile.Playlists)
                        {
                            if (pl.ParentGroupID == element.ID)
                            {
                                current_roms.AddRange(profileManager.Profile.Roms[pl.RomIDS.ToArray()]);
                            }
                        }
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        current_roms = new RomsCollection(null, false, profileManager.Profile.Roms[((Playlist)element).RomIDS.ToArray()]);
                        break;
                    }
            }


            bool temp_select = false;
            #region Normal Mode
            if (!list_view_parentMode)
            {
                int i = 0;
                oldP = -1;
                foreach (Rom rom in current_roms)
                {
                    bool addThisRom = false;
                    if (profileManager.Profile.ActiveCategories.Count == 0)
                    {
                        profileManager.Profile.OnRomShowed(rom);
                        addThisRom = true;
                    }
                    else// FILTER BY CATEGORIES
                    {
                        foreach (string cat in profileManager.Profile.ActiveCategories)
                        {
                            /*if (rom.Categories.Contains(cat))
                            {
                                addThisRom = true;
                                break;
                            }*/
                            bool bb = false;
                            foreach (string romCat in rom.Categories)
                            {
                                // Decode category
                                if (romCat == cat)
                                {
                                    addThisRom = true; bb = true;
                                    break;
                                }
                            }
                            if (bb) break;
                        }
                    }
                    if (!addThisRom)// Rom is not added at all, skip next filters
                        goto AddRoms;

                    // Data Filters filtering ...
                    // Loop through filters, see what's match our search ...
                    foreach (Filter f in profileManager.Profile.ActiveFilters)
                    {
                        addThisRom = FilterSearch(rom, f.Parameters);

                        if (profileManager.Profile.IsAllFilterMatch)
                        {
                            if (!addThisRom)
                            {
                                goto AddRoms;// Rom is not added, skip ...
                            }
                        }
                        else
                        {
                            if (addThisRom)
                            {
                                break; ;// Rom is added, skip ...
                            }
                        }
                    }

                    if (!addThisRom)// Rom is not added at all, skip next filter
                        goto AddRoms;

                    // Search Filtering
                    if (doSearch)
                    {
                        addThisRom = FilterSearch(rom, searchParameters);
                    }

                AddRoms:
                    // Add this rom if we can
                    if (addThisRom)
                    {
                        ManagedListViewItem item = GetItemFromRom(rom, list_view_temp_color, false);
                        AddNewItem(item);
                        if (list_view_selectRoms)
                        {
                            if (list_view_selectedRoms.Contains(rom.ID))
                            {
                                item.Selected = true;
                                if (!this.InvokeRequired)
                                    UpdateRomsSelection();
                                else
                                    this.Invoke(new Action(UpdateRomsSelection));
                                if (!temp_select)
                                {
                                    RefreshListViewScrollBars();
                                    if (!this.InvokeRequired)
                                        managedListView1.ScrollToItem(item);
                                    else
                                        this.Invoke(new AddListViewItem(managedListView1.ScrollToItem), item);

                                    temp_select = true;
                                }
                            }
                        }
                    }
                    i++;
                    if ((i % 50) == 0)
                        RefreshListViewScrollBars();
                    int x = (i * 100) / current_roms.Count;
                    if (x != oldP)
                    {
                        OnProgressThreaded(ls["Status_LoadingRoms"] + "...", x);
                        oldP = x;
                        Application.DoEvents();
                    }
                }
            }
            #endregion
            #region Parent And Children Mode
            else
            {
                int i = 0;
                oldP = -1;
                bool addThisRom = false;
                List<Rom> children;
                foreach (Rom rom in current_roms)
                {
                    addThisRom = false;
                    #region Not Parent nor Child
                    if (!rom.IsParentRom && !rom.IsChildRom)
                    {
                        // This rom doesn't belong to the Parent And Children mode ...
                        // Add it anyway ?
                        if (profileManager.Profile.ActiveCategories.Count == 0)
                        {
                            profileManager.Profile.OnRomShowed(rom);
                            addThisRom = true;
                        }
                        else// FILTER BY CATEGORIES
                        {
                            foreach (string cat in profileManager.Profile.ActiveCategories)
                            {
                                /*if (rom.Categories.Contains(cat))
                                {
                                    addThisRom = true;
                                    break;
                                }*/
                                bool bb = false;
                                foreach (string romCat in rom.Categories)
                                {
                                    // Decode category
                                    if (romCat == cat)
                                    {
                                        addThisRom = true; bb = true;
                                        break;
                                    }
                                }
                                if (bb) break;
                            }
                        }
                        if (!addThisRom)// Rom is not added at all, skip next filters
                            goto AddRoms;

                        // Data Filters filtering ...
                        // Loop through filters, see what's match our search ...
                        foreach (Filter f in profileManager.Profile.ActiveFilters)
                        {
                            addThisRom = FilterSearch(rom, f.Parameters);

                            if (profileManager.Profile.IsAllFilterMatch)
                            {
                                if (!addThisRom)
                                {
                                    goto AddRoms;// Rom is not added, skip ...
                                }
                            }
                            else
                            {
                                if (addThisRom)
                                {
                                    break; ;// Rom is added, skip ...
                                }
                            }
                        }

                        if (!addThisRom)// Rom is not added at all, skip next filter
                            goto AddRoms;

                        // Search Filtering
                        if (doSearch)
                        {
                            addThisRom = FilterSearch(rom, searchParameters);
                            // Remove it to fast things up
                        }


                    AddRoms:
                        // Add this rom if we can
                        if (addThisRom)
                        {
                            ManagedListViewItem item = GetItemFromRom(rom, list_view_temp_color, false);
                            AddNewItem(item);
                            if (list_view_selectRoms)
                            {
                                if (list_view_selectedRoms.Contains(rom.ID))
                                {
                                    item.Selected = true;
                                    if (!this.InvokeRequired)
                                        UpdateRomsSelection();
                                    else
                                        this.Invoke(new Action(UpdateRomsSelection));
                                    if (!temp_select)
                                    {
                                        RefreshListViewScrollBars();
                                        if (!this.InvokeRequired)
                                            managedListView1.ScrollToItem(item);
                                        else
                                            this.Invoke(new AddListViewItem(managedListView1.ScrollToItem), item);
                                        temp_select = true;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Parent
                    if (rom.IsParentRom)
                    {
                        // This rom is a parent rom, find all children
                        children = new List<Rom>();
                        foreach (string child in rom.ChildrenRoms)
                        {
                            Rom childRom = current_roms[child];
                            if (childRom != null)
                                children.Add(childRom);
                        }
                        //
                        // Add it anyway ?
                        if (profileManager.Profile.ActiveCategories.Count == 0)
                        {
                            profileManager.Profile.OnRomShowed(rom);
                            addThisRom = true;
                        }
                        else// FILTER BY CATEGORIES
                        {
                            foreach (string cat in profileManager.Profile.ActiveCategories)
                            {
                                /*if (rom.Categories.Contains(cat))
                                {
                                    addThisRom = true;
                                    break;
                                }*/
                                bool bb = false;
                                foreach (string romCat in rom.Categories)
                                {
                                    // Decode category
                                    if (romCat == cat)
                                    {
                                        addThisRom = true; bb = true;
                                        break;
                                    }
                                }
                                if (bb) break;
                            }
                        }
                        if (!addThisRom)// Rom is not added at all, skip next filters
                            goto AddParentRom;

                        // Data Filters filtering ...
                        // Loop through filters, see what's match our search ...
                        foreach (Filter f in profileManager.Profile.ActiveFilters)
                        {
                            addThisRom = FilterSearch(rom, f.Parameters);

                            if (profileManager.Profile.IsAllFilterMatch)
                            {
                                if (!addThisRom)
                                {
                                    goto AddParentRom;// Rom is not added, skip ...
                                }
                            }
                            else
                            {
                                if (addThisRom)
                                {
                                    break; ;// Rom is added, skip ...
                                }
                            }
                        }

                        if (!addThisRom)// Rom is not added at all, skip next filter
                            goto AddParentRom;

                        // Search Filtering
                        if (doSearch)
                        {
                            addThisRom = FilterSearch(rom, searchParameters);
                        }


                    AddParentRom:
                        // Add this rom if we can
                        if (addThisRom)
                        {
                            ManagedListViewItem item = GetItemFromRom(rom, list_view_temp_color, true);
                            AddNewItem(item);
                            if (list_view_selectRoms)
                            {
                                if (list_view_selectedRoms.Contains(rom.ID))
                                {
                                    item.Selected = true;
                                    if (!this.InvokeRequired)
                                        UpdateRomsSelection();
                                    else
                                        this.Invoke(new Action(UpdateRomsSelection));
                                    if (!temp_select)
                                    {
                                        RefreshListViewScrollBars();
                                        if (!this.InvokeRequired)
                                            managedListView1.ScrollToItem(item);
                                        else
                                            this.Invoke(new AddListViewItem(managedListView1.ScrollToItem), item);
                                        temp_select = true;
                                    }
                                }
                            }
                        }
                        // else { }
                        // Should we add the children while the parent can't be added ?
                        bool addThisChildRom = false;
                        bool oneChildAdded = false;
                        int firstChildIndex = managedListView1.Items.Count - 1;
                        // Now check out children and add them
                        foreach (Rom childRom in children)
                        {
                            addThisChildRom = false;
                            // Add it anyway ?
                            if (profileManager.Profile.ActiveCategories.Count == 0)
                            {
                                profileManager.Profile.OnRomShowed(childRom);
                                addThisChildRom = true;
                            }
                            else// FILTER BY CATEGORIES
                            {
                                foreach (string cat in profileManager.Profile.ActiveCategories)
                                {
                                    /*if (rom.Categories.Contains(cat))
                                    {
                                        addThisRom = true;
                                        break;
                                    }*/
                                    bool bb = false;
                                    foreach (string romCat in childRom.Categories)
                                    {
                                        // Decode category
                                        if (romCat == cat)
                                        {
                                            addThisChildRom = true; bb = true;
                                            break;
                                        }
                                    }
                                    if (bb) break;
                                }
                            }
                            if (!addThisChildRom)// Rom is not added at all, skip next filters
                                goto AddChildRom;

                            // Data Filters filtering ...
                            // Loop through filters, see what's match our search ...
                            foreach (Filter f in profileManager.Profile.ActiveFilters)
                            {
                                addThisChildRom = FilterSearch(childRom, f.Parameters);

                                if (profileManager.Profile.IsAllFilterMatch)
                                {
                                    if (!addThisChildRom)
                                    {
                                        goto AddChildRom;// Rom is not added, skip ...
                                    }
                                }
                                else
                                {
                                    if (addThisChildRom)
                                    {
                                        break; ;// Rom is added, skip ...
                                    }
                                }
                            }

                            if (!addThisChildRom)// Rom is not added at all, skip next filter
                                goto AddChildRom;

                            // Search Filtering
                            if (doSearch)
                            {
                                addThisChildRom = FilterSearch(childRom, searchParameters);
                            }


                        AddChildRom:
                            // Add this rom if we can
                            if (addThisChildRom)
                            {
                                ManagedListViewItem item = GetItemFromRom(childRom, list_view_temp_color, true);
                                AddNewItem(item);
                                if (list_view_selectRoms)
                                {
                                    if (list_view_selectedRoms.Contains(rom.ID))
                                    {
                                        item.Selected = true;
                                        if (!this.InvokeRequired)
                                            UpdateRomsSelection();
                                        else
                                            this.Invoke(new Action(UpdateRomsSelection));
                                        if (!temp_select)
                                        {
                                            RefreshListViewScrollBars();
                                            if (!this.InvokeRequired)
                                                managedListView1.ScrollToItem(item);
                                            else
                                                this.Invoke(new AddListViewItem(managedListView1.ScrollToItem), item);
                                            temp_select = true;
                                        }
                                    }
                                }
                                oneChildAdded = true;
                            }
                        }
                        if (oneChildAdded && !addThisRom)
                        {
                            if (firstChildIndex < 0)
                                firstChildIndex = 0;
                            // We should add the parent rom now, whatever the condition ...
                            ManagedListViewItem item = GetItemFromRom(rom, list_view_temp_color, true);
                            if (managedListView1.Items.Count > 0)
                                InsertNewItem(firstChildIndex, item);
                            else
                                AddNewItem(item);

                            if (list_view_selectRoms)
                            {
                                if (list_view_selectedRoms.Contains(rom.ID))
                                {
                                    item.Selected = true;
                                    if (!this.InvokeRequired)
                                        UpdateRomsSelection();
                                    else
                                        this.Invoke(new Action(UpdateRomsSelection));
                                    if (!temp_select)
                                    {
                                        RefreshListViewScrollBars();
                                        if (!this.InvokeRequired)
                                            managedListView1.ScrollToItem(item);
                                        else
                                            this.Invoke(new AddListViewItem(managedListView1.ScrollToItem), item);
                                        temp_select = true;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    i++;
                    if ((i % 50) == 0)
                        RefreshListViewScrollBars();
                    int x = (i * 100) / current_roms.Count;
                    if (x != oldP)
                    {
                        OnProgressThreaded(ls["Status_LoadingRoms"] + "...", x);
                        oldP = x;
                    }
                }
            }
            #endregion

            OnProgressThreaded(ls["Status_Done"], 100);

            RefreshListViewScrollBars();

            if (!this.InvokeRequired)
                UpdateRomsSelection();
            else
                this.Invoke(new Action(UpdateRomsSelection));

            if (!this.InvokeRequired)
                OnProgressFinished();
            else
                this.Invoke(new Action(OnProgressFinished));
            doSearch = false;
            isLoading = false;
            profileManager.Profile.OnRomsLoadingFinished();
        }
        private ManagedListViewItem GetItemFromRom(Rom rom, Color textColor, bool useChildish)
        {
            ManagedListViewItem item = new ManagedListViewItem();
            item.DrawMode = ManagedListViewItemDrawMode.UserDraw;
            item.Color = textColor;
            item.Tag = rom.ID;
            item.IsSubitemsReady = false;
            if (useChildish)
            {
                if (rom.IsChildRom)
                {
                    item.IsChildItem = true;
                }
                else if (rom.IsParentRom)
                {
                    item.IsParentItem = true;
                    item.IsParentCollapsed = true;
                }
            }
            EmulatorsOrganizer.Core.Console parentConsole = profileManager.Profile.Consoles[rom.ParentConsoleID];
            // Refresh images list (for parent console)
            if (!consoleImageIDs.Contains(rom.ParentConsoleID))
            {
                consoleImageIDs.Add(rom.ParentConsoleID);
                if (parentConsole != null)
                {
                    if (parentConsole.Icon != null)
                        imageList_listView.Images.Add(parentConsole.IconThumbnail);
                    else
                        imageList_listView.Images.Add(imageList1.Images[0]);
                }
                else
                {
                    imageList_listView.Images.Add(imageList1.Images[0]);
                }

            }
            if (managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails)
                FillRomThumbInfos(rom.ID);
            // item.IsChildItem = rom.IsChildRom;
            //FillRomItemSubitems(rom, item, textColor);
            return item;
        }
        private void FillRomItemSubitems(string romID, int itemIndex)
        {
            Rom rom = profileManager.Profile.Roms[romID];
            if (rom == null)
                return;

            managedListView1.Items[itemIndex].Text = rom.Name;
            if (roms_thumbs.ContainsKey(romID))
                roms_thumbs[romID].RomName = rom.Name;

            if (DetailsCacheIDS.Count > 0)
            {
                if (DetailsCacheIDS.Contains(romID))
                {
                    int index = DetailsCacheIDS.IndexOf(romID);
                    DetailsCacheNames[index] = rom.Name;
                }
            }

            /*SUBITEMS*/
            managedListView1.Items[itemIndex].SubItems.Clear();
            //name
            ManagedListViewSubItem subItem = new ManagedListViewSubItem();
            subItem.ColumnID = "name";
            subItem.DrawMode = ManagedListViewItemDrawMode.UserDraw;
            subItem.Text = rom.Name;
            subItem.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem);
            //size
            ManagedListViewSubItem subItem1 = new ManagedListViewSubItem();
            subItem1.ColumnID = "size";
            subItem1.Text = rom.SizeLable;
            subItem1.DrawMode = ManagedListViewItemDrawMode.Text;
            subItem1.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem1);
            //path
            ManagedListViewSubItem subItem2 = new ManagedListViewSubItem();
            subItem2.ColumnID = "path";
            subItem2.DrawMode = ManagedListViewItemDrawMode.Text;
            subItem2.Text = rom.Path;

            subItem2.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem2);
            //file type
            ManagedListViewSubItem subItem3 = new ManagedListViewSubItem();
            subItem3.ColumnID = "file type";
            subItem3.DrawMode = ManagedListViewItemDrawMode.Text;
            subItem3.Text = Path.GetExtension(rom.Path).Replace(".", "");
            subItem3.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem3);
            //rating
            ManagedListViewRatingSubItem subItem4 = new ManagedListViewRatingSubItem();
            subItem4.ColumnID = "rating";
            subItem4.Rating = rom.Rating;
            subItem4.RatingChanged += subItem4_RatingChanged;
            subItem4.UpdateRatingRequest += subItem4_UpdateRatingRequest;
            subItem4.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem4);
            //categories
            subItem3 = new ManagedListViewSubItem();
            subItem3.ColumnID = "categories";
            subItem3.DrawMode = ManagedListViewItemDrawMode.Text;
            string categories = "";
            if (rom.Categories != null)
                foreach (string cat in rom.Categories)
                    categories += cat + ", ";
            if (categories.EndsWith(", "))
                categories = categories.Substring(0, categories.Length - 2);
            subItem3.Text = categories;
            subItem3.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem3);
            //played times
            ManagedListViewSubItem subItem5 = new ManagedListViewSubItem();
            subItem5.ColumnID = "played times";
            subItem5.DrawMode = ManagedListViewItemDrawMode.Text;
            switch (rom.PlayedTimes)
            {
                case 0: subItem5.Text = ls["Status_NotPlayed"]; break;
                case 1: subItem5.Text = rom.PlayedTimes + " " + ls["Word_Time"]; break;
                default: subItem5.Text = rom.PlayedTimes + " " + ls["Word_Times"]; break;
            }
            subItem5.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem5);

            // Last played time
            subItem5 = new ManagedListViewSubItem();
            subItem5.ColumnID = "last played";
            subItem5.DrawMode = ManagedListViewItemDrawMode.Text;
            if (rom.LastPlayed != null)
            {
                if (rom.LastPlayed != DateTime.MinValue)
                    subItem5.Text = rom.LastPlayed.ToLocalTime().ToString();
                else
                    subItem5.Text = ls["Status_NotPlayed"];
            }
            else
            {
                subItem5.Text = ls["Status_NotPlayed"];
            }
            subItem5.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem5);
            //play time
            ManagedListViewSubItem subItem6 = new ManagedListViewSubItem();
            subItem6.ColumnID = "play time";
            subItem6.DrawMode = ManagedListViewItemDrawMode.Text;
            if (rom.PlayedTimeLength > 0)
            {
                string time = TimeSpan.FromSeconds((double)rom.PlayedTimeLength / 1000).ToString();
                if (time.Length > 12)
                    time = time.Substring(0, 12);
                subItem6.Text = time;
            }
            else
            {
                subItem6.Text = ls["Status_NotPlayed"];
            }

            subItem6.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem6);
            //parent console
            ManagedListViewSubItem subItem7 = new ManagedListViewSubItem();
            subItem7.ColumnID = "console";
            subItem7.DrawMode = ManagedListViewItemDrawMode.TextAndImage;
            subItem7.ImageIndex = consoleImageIDs.IndexOf(rom.ParentConsoleID);
            subItem7.Text = profileManager.Profile.Consoles[rom.ParentConsoleID].Name;
            subItem7.Color = managedListView1.Items[itemIndex].Color;
            managedListView1.Items[itemIndex].SubItems.Add(subItem7);
            // Rom data info elements
            EmulatorsOrganizer.Core.Console parentConsole = profileManager.Profile.Consoles[rom.ParentConsoleID];
            if (parentConsole.RomDataInfoElements != null)
            {
                foreach (RomData d in parentConsole.RomDataInfoElements)
                {
                    subItem7 = new ManagedListViewSubItem();
                    subItem7.ColumnID = d.ID;
                    subItem7.DrawMode = ManagedListViewItemDrawMode.Text;

                    object v = rom.GetDataItemValue(d.ID);
                    if (v != null)
                        subItem7.Text = v.ToString();
                    else
                        subItem7.Text = "";

                    subItem7.Color = managedListView1.Items[itemIndex].Color;
                    managedListView1.Items[itemIndex].SubItems.Add(subItem7);
                }
            }
            // Rom informaiton containers
            if (parentConsole.InformationContainers != null)
            {
                foreach (InformationContainer ic in parentConsole.InformationContainers)
                {
                    subItem7 = new ManagedListViewSubItem();
                    subItem7.ColumnID = ic.ID;
                    subItem7.DrawMode = ManagedListViewItemDrawMode.Text;
                    // Do check
                    if (rom.IsInformationContainerItemExist(ic.ID))
                    {
                        subItem7.Color = Color.Green;
                        if (ic is InformationContainerFiles)
                        {
                            InformationContainerItemFiles icitem = (InformationContainerItemFiles)rom.GetInformationContainerItem(ic.ID);
                            int coun = 0;
                            if (icitem != null)
                            {
                                if (icitem.Files != null)
                                    coun = icitem.Files.Count;
                            }
                            subItem7.Text = string.Format("[{0}]", coun);
                        }
                        else if (ic is InformationContainerLinks)
                        {
                            InformationContainerItemLinks icitem = (InformationContainerItemLinks)rom.GetInformationContainerItem(ic.ID);
                            int coun = 0;
                            if (icitem != null)
                            {
                                if (icitem.Links != null)
                                    coun = icitem.Links.Count;
                            }
                            subItem7.Text = string.Format("[{0}]", coun);
                        }
                        else if (ic is InformationContainerReviewScore)
                        {
                            InformationContainerItemReviewScore icitem = (InformationContainerItemReviewScore)rom.GetInformationContainerItem(ic.ID);
                            int coun = 0;
                            if (icitem != null)
                            {
                                coun = icitem.TotalScore;
                            }
                            subItem7.Text = string.Format("{0} %", coun);
                        }
                        else
                        {
                            subItem7.Text = "•";
                        }
                    }
                    else
                    {
                        subItem7.Color = Color.Red;
                        subItem7.Text = "X";
                    }
                    //subItem7.Color = textColor;
                    managedListView1.Items[itemIndex].SubItems.Add(subItem7);
                }
            }
            managedListView1.Items[itemIndex].IsSubitemsReady = true;
        }
        private void FillRomThumbInfos(string romID)
        {

            Rom rom = profileManager.Profile.Roms[romID];
            if (rom == null)
                return;

            EmulatorsOrganizer.Core.Console parentConsole = profileManager.Profile.Consoles[rom.ParentConsoleID];
            // Make 'thumbnail draw' through tag
            RomThumbnailInfo thumbInf = new RomThumbnailInfo();
            thumbInf.RomName = rom.Name;
            List<string> files = new List<string>();
            string targetIC = "auto";
            if (currentSelectedThumbMode != null)
                targetIC = currentSelectedThumbMode.ICID;
            if (targetIC == "auto")
            {
                // Get all possible images !
                // Priority matters so we use console ics order.
                foreach (InformationContainer con in parentConsole.InformationContainers)
                {
                    if (con is InformationContainerImage)
                    {
                        if (rom.IsInformationContainerItemExist(con.ID))
                        {
                            // Add the files
                            InformationContainerItemFiles icitem = (InformationContainerItemFiles)rom.GetInformationContainerItem(con.ID);
                            if (icitem != null)
                                if (icitem.Files != null)
                                    files.AddRange(icitem.Files);
                        }
                    }
                }
            }
            else
            {
                // Get images for selected ic !
                InformationContainerItemFiles icitem = (InformationContainerItemFiles)rom.GetInformationContainerItem(targetIC);
                if (icitem != null)
                    if (icitem.Files != null)
                        files.AddRange(icitem.Files);
            }

            thumbInf.ThumbnailFiles = files.ToArray();
            thumbInf.ThumbFileIndex = 0;
            if (!roms_thumbs.ContainsKey(rom.ID))
                roms_thumbs.Add(rom.ID, thumbInf);
            else
                roms_thumbs[rom.ID] = thumbInf;
        }
        private void RefreshThumbModes()
        {
            ThumbnailsCache = new List<Image>();
            ThumbnailsCacheIDS = new List<string>();
            comboBox_thumbsMode.Items.Clear();
            // Add auto. 
            ThumbnailsMode mode = new ThumbnailsMode();
            mode.Name = ls["Word_AutoSelect"];
            mode.ICID = "auto";
            comboBox_thumbsMode.Items.Add(mode);

            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        comboBox_thumbsMode.Enabled = true;
                        // Add viewble ics; ordered by priority.
                        EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        if (console == null)
                        {
                            comboBox_thumbsMode.SelectedIndex = 0;
                            return;
                        }
                        int i = 0;
                        int select_index = 0;
                        foreach (InformationContainer ic in console.InformationContainers)
                        {
                            if (ic is InformationContainerImage)
                            {
                                mode = new ThumbnailsMode();
                                mode.Name = ic.DisplayName;
                                mode.ICID = ic.ID;

                                comboBox_thumbsMode.Items.Add(mode);

                                i++;

                                if (console.ThumbModeSelectionID == ic.ID)
                                    select_index = i;
                            }
                        }
                        // If the mode is not specified, select auto.
                        if (select_index < comboBox_thumbsMode.Items.Count)
                            comboBox_thumbsMode.SelectedIndex = select_index;

                        if (comboBox_thumbsMode.SelectedIndex < 0)
                            comboBox_thumbsMode.SelectedIndex = 0;
                        break;
                    }
                default:
                    {
                        comboBox_thumbsMode.Enabled = false;
                        // Keep auto. This is all we can do for none console selection.
                        comboBox_thumbsMode.SelectedIndex = 0;
                        break;
                    }
            }
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            // When saving a profile, disable editing and moving
            contextMenuStrip_columns.Enabled = false;
            // contextMenuStrip_normal.Enabled = false;
            deleteToolStripMenuItem.Enabled =
                renameToolStripMenuItem.Enabled =
                applyRomNamesToFilesToolStripMenuItem.Enabled =
                getNameAndDataFromMobyGamescomToolStripMenuItem.Enabled =
                getDataFromTheGamesDBForThisGameToolStripMenuItem.Enabled =
                changeIconToolStripMenuItem.Enabled =
                clearIconToolStripMenuItem.Enabled =
                resetToolStripMenuItem.Enabled =
                allCountersToolStripMenuItem.Enabled =
                playTimesToolStripMenuItem.Enabled =
                playTimeToolStripMenuItem.Enabled =
                lastPlayedTimeToolStripMenuItem.Enabled =
                sortToolStripMenuItem.Enabled =
                sendToPlaylistToolStripMenuItem.Enabled =
                addDataInfoAsCategoriesToolStripMenuItem.Enabled =
                editCategoriesToolStripMenuItem.Enabled =
                sendToToolStripMenuItem.Enabled = clearRelatedFilesToolStripMenuItem.Enabled =
                propertiesToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            if (!InvokeRequired)
                AfterSaveFinished();
            else
                Invoke(new Action(AfterSaveFinished));
        }
        private void AfterSaveFinished()
        {
            contextMenuStrip_columns.Enabled = true;
            deleteToolStripMenuItem.Enabled =
         renameToolStripMenuItem.Enabled =
         applyRomNamesToFilesToolStripMenuItem.Enabled =
         getNameAndDataFromMobyGamescomToolStripMenuItem.Enabled =
         getDataFromTheGamesDBForThisGameToolStripMenuItem.Enabled =
         changeIconToolStripMenuItem.Enabled =
         clearIconToolStripMenuItem.Enabled =
         resetToolStripMenuItem.Enabled =
         allCountersToolStripMenuItem.Enabled =
         playTimesToolStripMenuItem.Enabled =
         playTimeToolStripMenuItem.Enabled =
         lastPlayedTimeToolStripMenuItem.Enabled =
         sortToolStripMenuItem.Enabled =
         sendToPlaylistToolStripMenuItem.Enabled =
          addDataInfoAsCategoriesToolStripMenuItem.Enabled =
         editCategoriesToolStripMenuItem.Enabled =
         sendToToolStripMenuItem.Enabled = clearRelatedFilesToolStripMenuItem.Enabled =
         propertiesToolStripMenuItem.Enabled = true;
        }
        private void ExportNames()
        {
            List<Rom> roms = new List<Rom>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                roms.Add(profileManager.Profile.Roms[item.Tag.ToString()]);
            }
            FrmExportNames frm = new FrmExportNames(roms.ToArray());
            frm.ShowDialog(this);
        }

        // Filter Methods
        private bool FilterSearch(Rom rom, SearchRequestArgs parameters)
        {
            if (parameters == null)
                return false;
            // Let's see what's the mode
            string searchWord = parameters.CaseSensitive ? parameters.SearchWhat : parameters.SearchWhat.ToLower();
            bool isNumberSearch = false;
            string searchTargetText = "";
            long searchTargetNumber = 0;
            if (!parameters.IsDataItem)
            {
                switch (parameters.SearchMode)
                {
                    case SearchMode.Name:
                        {
                            isNumberSearch = false;
                            searchTargetText = rom.Name;
                            break;
                        }
                    case SearchMode.FileType:
                        {
                            isNumberSearch = false;
                            searchTargetText = Path.GetExtension(HelperTools.GetPathFromAIPath(rom.Path)).Replace(".", "");
                            break;
                        }
                    case SearchMode.FilePath:
                        {
                            isNumberSearch = false;
                            searchTargetText = HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path));
                            break;
                        }
                    case SearchMode.Rating:
                        {
                            isNumberSearch = true;
                            searchTargetNumber = rom.Rating;
                            break;
                        }
                    case SearchMode.Size:
                        {
                            isNumberSearch = true;
                            searchTargetNumber = rom.FileSize;
                            break;
                        }
                    case SearchMode.PlayedTimes:
                        {
                            isNumberSearch = true;
                            searchTargetNumber = rom.PlayedTimes;
                            break;
                        }
                    case SearchMode.PlayTime:
                        {
                            isNumberSearch = true;
                            searchTargetNumber = rom.PlayedTimeLength;
                            break;
                        }
                    case SearchMode.LastPlayed:
                        {
                            isNumberSearch = true;
                            searchTargetNumber = rom.LastPlayed.Ticks;
                            break;
                        }
                }
            }
            else
            {
                // Let's see if this rom include the data item we seek
                // Get parent console
                Core.Console pcon = profileManager.Profile.Consoles[rom.ParentConsoleID];
                // Loop through rom data info... see which one is the target !
                bool found = false;
                foreach (RomData d in pcon.RomDataInfoElements)
                {
                    if (d.Name == parameters.DataItemName)
                    {
                        // Now see if the rom have one ...
                        if (rom.IsDataItemExist(d.ID))
                        {
                            found = true;
                            // See the type
                            try
                            {
                                switch (d.Type)
                                {
                                    case RomDataType.Text: isNumberSearch = false; searchTargetText = (string)rom.GetDataItemValue(d.ID); break;
                                    case RomDataType.Number: isNumberSearch = true; searchTargetNumber = (int)rom.GetDataItemValue(d.ID); break;
                                }
                            }
                            catch
                            {
                                return false;// Something wrong with rom data item !
                            }
                        }
                        else
                        {
                            return false;// No need to continue, the rom doesn't have any data item related to the given one
                        }
                        break;
                    }
                }
                if (!found) return false;
            }
            if (!isNumberSearch)
            {
                if (!parameters.CaseSensitive)
                    searchTargetText = searchTargetText.ToLower();
                // Decode user code
                string[] searchCodes = searchWord.Split(new char[] { '+' });
                // Do the search
                switch (parameters.ConditionForText)
                {
                    case TextSearchCondition.Contains:// The target contains the search word
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.Contains(s))
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.DoesNotContain:// The target doesn't contain the search word
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.Contains(s))
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                    case TextSearchCondition.Is:// Match the word !
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText == s)
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.IsNot:// Don't match the word !
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText == s)
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                    case TextSearchCondition.StartWith:// The target starts the search word
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.StartsWith(s))
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.DoesNotStartWith:// The target doesn't start the search word
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.StartsWith(s))
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                    case TextSearchCondition.EndWith:// The target ends the search word
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.EndsWith(s))
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.DoesNotEndWith:// The target doesn't end with the search word
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.EndsWith(s))
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                }
            }
            else// Number search
            {
                long searchNumber = DecodeSizeLabel(searchWord);
                switch (parameters.ConditionForNumber)
                {
                    case NumberSearchCondition.Equal:
                        {
                            if (searchTargetNumber == searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.DoesNotEqual:
                        {
                            if (searchTargetNumber != searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.EqualSmaller:
                        {
                            if (searchTargetNumber <= searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.EuqalLarger:
                        {
                            if (searchTargetNumber >= searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.Larger:
                        {
                            if (searchTargetNumber > searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.Smaller:
                        {
                            if (searchTargetNumber < searchNumber)
                                return true;
                            break;
                        }
                }
            }
            return false;
        }
        private long DecodeSizeLabel(string sizeLabel)
        {
            // Let's see given parameter (size)
            string t = sizeLabel.ToLower();
            t = t.Replace("kb", "");
            t = t.Replace("mb", "");
            t = t.Replace("gb", "");
            t = t.Replace(" ", "");
            double value = 0;
            double.TryParse(t, out value);

            if (sizeLabel.ToLower().Contains("kb"))
                value *= 1024;
            else if (sizeLabel.ToLower().Contains("mb"))
                value *= 1024 * 1024;
            else if (sizeLabel.ToLower().Contains("gb"))
                value *= 1024 * 1024 * 1024;

            return (long)value;
        }
        private void RefreshColumns()
        {
            managedListView1.Columns.Clear();
            if (element == null) return;
            foreach (ColumnItem column in element.Columns)
            {
                if (column.Visible)
                {
                    ManagedListViewColumn listColumn = new ManagedListViewColumn();
                    listColumn.HeaderText = column.ColumnName;
                    listColumn.ID = column.ColumnID;
                    listColumn.SortMode = ManagedListViewSortMode.None;
                    listColumn.Width = column.Width;
                    managedListView1.Columns.Add(listColumn);
                }
            }
        }
        private void SaveColumns()
        {
            if (profileManager.IsSaving)
                return;// saving columns is not allowing while saving profile.

            if (element == null)
                return;

            List<ColumnItem> oldCollection = element.Columns;
            //create new, save the visible columns first
            element.Columns = new List<ColumnItem>();
            foreach (ManagedListViewColumn column in managedListView1.Columns)
            {
                ColumnItem item = new ColumnItem();
                item.ColumnID = column.ID;
                item.ColumnName = column.HeaderText;
                item.Visible = true;
                item.Width = column.Width;

                element.Columns.Add(item);
                //look for the same item in the old collection then remove it
                foreach (ColumnItem olditem in oldCollection)
                {
                    if (olditem.ColumnID == column.ID)
                    {
                        oldCollection.Remove(olditem);
                        break;
                    }
                }
            }
            //now add the rest of the items (not visible)
            foreach (ColumnItem olditem in oldCollection)
            {
                ColumnItem item = new ColumnItem();
                item.ColumnID = olditem.ColumnID;
                item.ColumnName = olditem.ColumnName;
                item.Visible = false;
                item.Width = olditem.Width;
                element.Columns.Add(item);
            }
        }
        private void RefreshStyle()
        {
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.ConsolesGroup:
                    {
                        element = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        ApplyStyle();
                        break;
                    }
                case SelectionType.Console:
                    {
                        element = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        ApplyStyle();
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        element = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        ApplyStyle();
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        element = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                        ApplyStyle();
                        break;
                    }
                default:
                    {
                        // Reload settings
                        SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
                        this.defaultStyle = (EOStyle)settings.GetValue("Default Style", true, new EOStyle());

                        managedListView1.BackgroundImage = defaultStyle.image_RomsBrowser;
                        switch (defaultStyle.imageMode_RomsBrowser)
                        {
                            case BackgroundImageMode.NormalStretchNoAspectRatio:
                                {
                                    managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio;
                                    break;
                                }
                            case BackgroundImageMode.StretchIfLarger:
                                {
                                    managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioIfLarger;
                                    break;
                                }
                            case BackgroundImageMode.StretchToFit:
                                {
                                    managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioToFit;
                                    break;
                                }
                        }
                        managedListView1.ForeColor = defaultStyle.listviewTextsColor;
                        // texts color
                        //foreach (ManagedListViewItem item in managedListView1.Items)
                        //{
                        //    foreach (ManagedListViewSubItem subItem in item.SubItems)
                        //        subItem.Color = defaultStyle.ListviewTextsColor;
                        //}
                        foreach (ManagedListViewColumn column in managedListView1.Columns)
                            column.HeaderTextColor = defaultStyle.listviewColumnTextColor;
                        managedListView1.ColumnClickColor = defaultStyle.listviewColumnClickColor;
                        managedListView1.ColumnColor = defaultStyle.listviewColumnColor;
                        managedListView1.ColumnHighlightColor = defaultStyle.listviewColumnHighlightColor;
                        managedListView1.ItemHighlightColor = defaultStyle.listviewHighlightColor;
                        managedListView1.ItemMouseOverColor = defaultStyle.listviewMouseOverColor;
                        managedListView1.ItemSpecialColor = defaultStyle.listviewSpecialColor;
                        managedListView1.DrawHighlight = defaultStyle.listviewDrawHighlight;
                        managedListView1.Invalidate();
                        break;
                    }
            }
        }
        private void ApplyStyle()
        {
            managedListView1.BackgroundImage = element.Style.image_RomsBrowser;
            switch (element.Style.imageMode_RomsBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio;
                        break;
                    }
                case BackgroundImageMode.StretchIfLarger:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioIfLarger;
                        break;
                    }
                case BackgroundImageMode.StretchToFit:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioToFit;
                        break;
                    }
            }
            managedListView1.ForeColor = element.Style.listviewTextsColor;
            // texts color
            //foreach (ManagedListViewItem item in managedListView1.Items)
            //{
            //    foreach (ManagedListViewSubItem subItem in item.SubItems)
            //        subItem.Color = element.Style.ListviewTextsColor;
            //}
            foreach (ManagedListViewColumn column in managedListView1.Columns)
                column.HeaderTextColor = element.Style.listviewColumnTextColor;
            managedListView1.ColumnClickColor = element.Style.listviewColumnClickColor;
            managedListView1.ColumnColor = element.Style.listviewColumnColor;
            managedListView1.ColumnHighlightColor = element.Style.listviewColumnHighlightColor;
            managedListView1.ItemHighlightColor = element.Style.listviewHighlightColor;
            managedListView1.ItemMouseOverColor = element.Style.listviewMouseOverColor;
            managedListView1.ItemSpecialColor = element.Style.listviewSpecialColor;
            managedListView1.DrawHighlight = element.Style.listviewDrawHighlight;
            // Font
            try
            {
                FontConverter conv = new FontConverter();
                managedListView1.Font = (Font)conv.ConvertFromString(element.Style.font_RomsBrowser);
            }
            catch { }
            managedListView1.Invalidate();
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                statusStrip1.BackColor = managedListView1.BackColor = base.BackColor = value;
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
                managedListView1.BackgroundImage = base.BackgroundImage = value;
            }
        }
        private void UpdateRomsSelection()
        {
            isSelecting = true;

            StatusLabel_romSelection.Text = managedListView1.SelectedItems.Count + " / " +
                managedListView1.Items.Count + " " + ls["Status_Selected"];
            profileManager.Profile.SelectedRomIDS = new List<string>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                profileManager.Profile.SelectedRomIDS.Add(item.Tag.ToString());
            }
            profileManager.Profile.OnRomSelectionChanged();

            // if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup ||
            //     profileManager.Profile.RecentSelectedType == SelectionType.Playlist ||
            //     profileManager.Profile.RecentSelectedType == SelectionType.PlaylistsGroup)
            {
                // Select emulators
                if (managedListView1.SelectedItems.Count == 1)
                {
                    Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[0].Tag.ToString()];
                    string cID = rom.ParentConsoleID;
                    Emulator[] emulators = profileManager.Profile.Emulators[cID, false];
                    List<string> emuIDs = new List<string>();
                    foreach (Emulator em in emulators) emuIDs.Add(em.ID);
                    profileManager.Profile.OnEmulatorsRefreshRequest(emuIDs.ToArray());
                }
                else// Clear emulators
                {
                    // Refresh emulators with 0 in order to clear
                    if (profileManager.Profile.RecentSelectedType != SelectionType.Console)
                        profileManager.Profile.OnEmulatorsRefreshRequest(new string[0]);
                }
            }

            profileManager.Profile.OnRefreshPriorityRequest();

            // For thumbnails, reset all counters !
            if (managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails)
            {
                //foreach (string key in roms_thumbs.Keys)
                //     roms_thumbs[key].ThumbFileIndex = 0;
                foreach (string id in selectedRomsIDS)
                    if (roms_thumbs.ContainsKey(id))
                        roms_thumbs[id].ThumbFileIndex = 0;
            }
            // Update selection
            if (selectedRomsIDS == null)
                selectedRomsIDS = new List<string>();
            selectedRomsIDS.Clear();
            List<ManagedListViewItem> selecteditem = managedListView1.SelectedItems;
            foreach (ManagedListViewItem item in selecteditem)
                selectedRomsIDS.Add(item.Tag.ToString());
            isSelecting = false;

            OnEnableDisableButtons();
        }
        public void OnColumnMenuItemClick(string name)
        {
            if (profileManager.IsSaving)
                return;// saving columns is not allowing while saving profile.

            if (element == null)
                return;

            int i = 0;
            foreach (ColumnItem item in element.Columns)
            {
                if (item.ColumnName == name)
                {
                    item.Visible = !item.Visible;
                    RefreshColumns();
                    break;
                }
                i++;
            }
            managedListView1.Invalidate();
            SaveColumns();
            if (element is Core.Console)
                profileManager.Profile.OnConsoleColumnVisibleChanged(element.Name);
            else if (element is Core.ConsolesGroup)
                profileManager.Profile.OnConsolesGroupColumnVisibleChanged(element.Name);
            else if (element is Core.Playlist)
                profileManager.Profile.OnPlaylistColumnVisibleChanged(element.Name);
            else if (element is Core.PlaylistsGroup)
                profileManager.Profile.OnPlaylistsGroupColumnVisibleChanged(element.Name);
        }
        public override void ApplyStyle(EOStyle style)
        {
            this.BackColor = style.bkgColor_RomsBrowser;
            this.BackgroundImage = style.image_RomsBrowser;
            switch (style.imageMode_RomsBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio;
                        break;
                    }
                case BackgroundImageMode.StretchIfLarger:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioIfLarger;
                        break;
                    }
                case BackgroundImageMode.StretchToFit:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioToFit;
                        break;
                    }
            }
            statusStrip1.ForeColor = style.txtColor_RomsBrowser;
            // Font
            try
            {
                FontConverter conv = new FontConverter();
                managedListView1.Font = (Font)conv.ConvertFromString(style.font_RomsBrowser);
            }
            catch { }
            managedListView1.BackgroundImage = style.image_RomsBrowser;
            managedListView1.ForeColor = style.txtColor_RomsBrowser;
            foreach (ManagedListViewColumn column in managedListView1.Columns)
                column.HeaderTextColor = style.listviewColumnTextColor;
            managedListView1.ColumnClickColor = style.listviewColumnClickColor;
            managedListView1.ColumnColor = style.listviewColumnColor;
            managedListView1.ColumnHighlightColor = style.listviewColumnHighlightColor;
            managedListView1.ItemHighlightColor = style.listviewHighlightColor;
            managedListView1.ItemMouseOverColor = style.listviewMouseOverColor;
            managedListView1.ItemSpecialColor = style.listviewSpecialColor;
            managedListView1.DrawHighlight = style.listviewDrawHighlight;
            managedListView1.Invalidate();
        }
        public override bool CanChangeIcon
        {
            get
            {
                if (profileManager.IsSaving)
                    return false;

                return managedListView1.SelectedItems.Count > 0;
            }
        }
        public override bool CanDelete
        {
            get
            {
                if (profileManager.IsSaving)
                    return false;
                return managedListView1.SelectedItems.Count > 0;
            }
        }
        public override bool CanRename
        {
            get
            {
                if (profileManager.IsSaving)
                    return false;
                return managedListView1.SelectedItems.Count == 1;
            }
        }
        public override bool CanShowProperties
        {
            get
            {
                if (profileManager.IsSaving)
                    return false;
                return managedListView1.SelectedItems.Count > 0;
            }
        }
        public bool CanPlayRom
        {
            get { return managedListView1.SelectedItems.Count == 1; }
        }
        public ManagedListViewViewMode ListViewMode
        {
            get { return managedListView1.ViewMode; }
            set
            {
                managedListView1.ViewMode = value;
                if (value == ManagedListViewViewMode.Thumbnails)
                {
                    if (current_roms != null)
                    {
                        foreach (Rom rom in current_roms)
                        {
                            FillRomThumbInfos(rom.ID);
                        }
                    }
                }
            }
        }
        public override void DeleteSelected()
        {
            if (profileManager.IsSaving)
                return;
            base.DeleteSelected();
            if (managedListView1.SelectedItems.Count == 0)
                return;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                case SelectionType.ConsolesGroup:
                    {
                        Form_DeleteRoms frm = new Form_DeleteRoms();
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {
                            OnProgressStarted();
                            Trace.WriteLine("Deleting roms progress started " + DateTime.Now.ToLocalTime(), "Roms browser");
                            int i = 0;
                            oldP = -1;
                            int max = managedListView1.SelectedItems.Count;
                            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                            {
                                Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                                DeleteRom(rom, frm.DeleteRomFiles, frm.DeleteRomRelatedFiles, frm.DeleteParentChildren);
                                managedListView1.Items.Remove(item);
                                i++;
                                int x = (i * 100) / max;
                                if (i != oldP)
                                {
                                    OnProgress(ls["Status_Deleting"] + "...", x);
                                    oldP = i;
                                }
                            }
                            OnProgressFinished();
                            profileManager.Profile.OnRomsRemoved(max);
                        }
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(
                            ls["Message_AreYouSureYouWantToRemoveSelectedRomsFromPlaylist"] + " '" +
                            profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Name +
                            "' ?\n" + ls["Message_ThisWillNotEffectTheRomsAndOnlyRemoveFromThisPlaylist"], ls["MessageCaption_DeleteRoms"]);
                        if (result.ClickedButtonIndex == 0)
                        {
                            List<Rom> roms = new List<Rom>();
                            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                            {
                                roms.Add(profileManager.Profile.Roms[item.Tag.ToString()]);
                            }
                            profileManager.Profile.RemoveRomsFromPlaylist(roms.ToArray(),
                                profileManager.Profile.SelectedPlaylistID);
                        }
                        break;
                    }
            }
        }
        private void DeleteRom(Rom rom, bool deleteFromDisk, bool deleteRelatedFiles, bool deleteChildren)
        {
            if (profileManager.IsSaving)
                return;
            Trace.WriteLine("Removing rom: [" + rom.ID + "] " + rom.Name, "Roms browser");
            // Remove relationships ...
            if (rom.IsParentRom)
            {
                // Remove all children relationships
                foreach (string child in rom.ChildrenRoms)
                {
                    profileManager.Profile.Roms[child].IsChildRom = false;
                    profileManager.Profile.Roms[child].ParentRomID = "";

                    if (deleteChildren)
                    {
                        DeleteRom(profileManager.Profile.Roms[child], deleteFromDisk, deleteRelatedFiles, false);
                    }
                }
            }
            else if (rom.IsChildRom)
            {
                // Remove this child form the parent !
                profileManager.Profile.Roms[rom.ParentRomID].ChildrenRoms.Remove(rom.ID);
                if (profileManager.Profile.Roms[rom.ParentRomID].ChildrenRoms.Count == 0)
                {
                    profileManager.Profile.Roms[rom.ParentRomID].IsParentRom = false;
                    profileManager.Profile.Roms[rom.ParentRomID].AlwaysChooseChildWhenPlay = false;
                }
                profileManager.Profile.Roms[rom.ParentRomID].Modified = true;
            }
            profileManager.Profile.Roms.Remove(rom.ID, false);

            // Delete from disk
            if (deleteFromDisk && !HelperTools.IsAIPath(rom.Path))
            {
                Trace.WriteLine(">Removing rom file: " + rom.Path, "Roms browser");
                try
                {
                    File.Delete(HelperTools.GetFullPath(rom.Path));
                    Trace.WriteLine(">>file removed: " + rom.Path, "Roms browser");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(">UNABLE to remove rom file: " + rom.Path, "Roms browser");
                    Trace.WriteLine(">" + ex.Message, "Roms browser");
                }
            }
            // Delete related files !
            if (deleteRelatedFiles)
            {
                Trace.WriteLine(">Removing rom related files ... ", "Roms browser");
                if (rom.RomInfoItems != null)
                {
                    foreach (InformationContainerItem rinf in rom.RomInfoItems)
                    {
                        if (rinf is InformationContainerItemFiles)
                        {
                            if (((InformationContainerItemFiles)rinf).Files != null)
                            {
                                foreach (string rr in ((InformationContainerItemFiles)rinf).Files)
                                {
                                    try
                                    {
                                        File.Delete(HelperTools.GetFullPath(rr));
                                        Trace.WriteLine(">>file removed: " + rr, "Roms browser");
                                    }
                                    catch (Exception ex)
                                    {
                                        Trace.WriteLine(">>UNABLE to remove file: " + rr, "Roms browser");
                                        Trace.WriteLine(">>" + ex.Message, "Roms browser");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Trace.WriteLine(">Rom has no related file.", "Roms browser");
                }
            }
        }
        public void ApplyNameToRoms()
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count <= 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectTheRomsToApplyNamesFor"],
                  ls["MessageCaption_ApplyRomNamesToFiles"]);
                return;
            }
            Form_ApplyNames frm = new Form_ApplyNames();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                ServicesManager.OnDisableWindowListner();
                string logPath = ".\\Logs\\" + DateTime.Now.ToLocalTime().ToString() + "-apply rom names to files.txt";
                logPath = logPath.Replace(":", "");
                logPath = logPath.Replace("/", "-");
                TextWriterTraceListener listner = new TextWriterTraceListener(HelperTools.GetFullPath(logPath));
                Trace.Listeners.Add(listner);
                OnProgressStarted();
                for (int i = 0; i < managedListView1.SelectedItems.Count; i++)
                {
                    Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[i].Tag.ToString()];
                    try
                    {
                        Trace.WriteLine("Applying rom name on rom file at:", "Apply rom names to files");
                        Trace.WriteLine(rom.Path, "Apply rom names to files");
                        string newPath = "";
                        string failEx = "";
                        if (HelperTools.RenameFile(HelperTools.GetFullPath(rom.Path), rom.Name, out newPath, out failEx))
                        {
                            profileManager.Profile.Roms[rom.ID].Path = HelperTools.GetDotPath(newPath);
                            Trace.WriteLine("Rom file renamed successfully.", "Apply rom names to files");
                            Trace.WriteLine("Rom path set to:", "Apply rom names to files");
                            Trace.WriteLine(rom.Path, "Apply rom names to files");
                        }
                        else
                        {
                            //ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRomFile"] + ": " + failEx,
                            //    ls["MessageCaption_RenameRom"]);
                            Trace.WriteLine("UNABLE to rename rom file: " + failEx, "Apply rom names to files");
                        }
                    }
                    catch (Exception ex)
                    {
                        //ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRomFile"] + ": " + ex.Message,
                        //        ls["MessageCaption_RenameRom"]);
                        Trace.WriteLine("UNABLE to rename rom file: " + ex.Message, "Apply rom names to files");
                    }

                    if (frm.RenameRelatedFiles)
                    {
                        try
                        {
                            Trace.WriteLine("Renaming related files ....", "Apply rom names to files");
                            if (rom.RomInfoItems != null)
                            {
                                foreach (InformationContainerItem it in rom.RomInfoItems)
                                {
                                    if (it is InformationContainerItemFiles)
                                    {
                                        InformationContainerItemFiles itf = (InformationContainerItemFiles)it;
                                        // Get parent console
                                        EmulatorsOrganizer.Core.Console con = profileManager.Profile.Consoles[rom.ParentConsoleID];
                                        // Get ic name
                                        InformationContainer ic = con.GetInformationContainer(it.ParentID);
                                        if (ic == null)
                                        {
                                            Trace.WriteLine("Information container not exist with id " + it.ParentID, "Apply rom names to files");
                                            continue;
                                        }
                                        if (itf.Files != null)
                                            for (int f = 0; f < itf.Files.Count; f++)
                                            {
                                                string newPath = "";
                                                string failEx = "";
                                                string newName = "";
                                                switch (frm.RenameingMethodChosen)
                                                {
                                                    case Form_RenameRoms.RenameingMethod.NewRomName_InfoFileIndex:
                                                        {
                                                            newName = rom.Name + "(" + f + ")";
                                                            break;
                                                        }
                                                    case Form_RenameRoms.RenameingMethod.NewRomName_InfoName_InfoFileIndex:
                                                        {
                                                            newName = rom.Name + "_" + ic.Name + "(" + f + ")";
                                                            break;
                                                        }
                                                    case Form_RenameRoms.RenameingMethod.InfoName_NewRomName_InfoFileIndex:
                                                        {
                                                            newName = ic.Name + "_" + rom.Name + "(" + f + ")";
                                                            break;
                                                        }
                                                }
                                                if (HelperTools.RenameFile(HelperTools.GetFullPath(itf.Files[f]), newName, out newPath, out failEx))
                                                {
                                                    itf.Files[f] = HelperTools.GetDotPath(newPath);
                                                    Trace.WriteLine(">Related file renamed successfully.", "Apply rom names to files");
                                                    Trace.WriteLine(">Related path set to:", "Apply rom names to files");
                                                    Trace.WriteLine(">" + newPath, "Apply rom names to files");
                                                }
                                                else
                                                {
                                                    //ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRelatedFile"] + ": " + failEx,
                                                    //    ls["MessageCaption_RenameRom"]);
                                                    Trace.WriteLine("UNABLE to rename related file: " + failEx, "Apply rom names to files");
                                                }
                                            }
                                    }
                                }
                            }
                            else
                            {
                                Trace.WriteLine("UNABLE to rename related files, no file included in this rom.", "Apply rom names to files");
                            }
                        }
                        catch (Exception ex)
                        {
                            //ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRomFile"] + ": " + ex.Message,
                            //        ls["MessageCaption_RenameRom"]);
                            Trace.WriteLine("UNABLE to rename rom file: " + ex.Message, "Apply rom names to files");
                        }
                    }
                    int x = i * 100 / managedListView1.SelectedItems.Count;
                    OnProgress("Applying rom names to files ....", x);
                }
                listner.Flush();
                Trace.Listeners.Remove(listner);
                ServicesManager.OnEnableWindowListner();
                ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(ls["Message_Done"] + ", " + ls["Message_LogFileSavedTo"] + " '" + logPath + "'",
                ls["MessageCaption_ApplyRomNamesToFiles"], new string[] { ls["Button_Ok"], ls["Button_OpenLog"] },
                0, null, ManagedMessageBoxIcon.Info);
                if (res.ClickedButtonIndex == 1) { try { Process.Start(HelperTools.GetFullPath(logPath)); } catch { } }
                profileManager.Profile.OnDatabaseImported();
                if (frm.RenameRelatedFiles)
                {
                    profileManager.Profile.OnInformationContainerItemsModified("");
                }
                OnProgressFinished();
            }
        }
        public override void RenameSelected()
        {
            if (profileManager.IsSaving)
                return;
            base.RenameSelected();
            if (managedListView1.SelectedItems.Count != 1)
                return;
            Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[0].Tag.ToString()];
            Form_RenameRoms frm = new Form_RenameRoms(rom.Name, rom.ParentConsoleID, !HelperTools.IsAIPath(rom.Path));
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                string oldName = rom.Name;
                rom.Name = frm.RomName;
                if (frm.ApplyNameOnFile)
                {
                    try
                    {
                        Trace.WriteLine("Applying rom name on rom file at:", "Rom rename");
                        Trace.WriteLine(rom.Path, "Rom rename");
                        string newPath = "";
                        string failEx = "";
                        if (HelperTools.RenameFile(HelperTools.GetFullPath(rom.Path), frm.RomName, out newPath, out failEx))
                        {
                            profileManager.Profile.Roms[rom.ID].Path = HelperTools.GetDotPath(newPath);
                            Trace.WriteLine("Rom file renamed successfully.", "Rom rename");
                            Trace.WriteLine("Rom path set to:", "Rom rename");
                            Trace.WriteLine(rom.Path, "Rom rename");
                        }
                        else
                        {
                            ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRomFile"] + ": " + failEx,
                                ls["MessageCaption_RenameRom"]);
                            Trace.WriteLine("UNABLE to rename rom file: " + failEx, "Rom rename");
                        }
                    }
                    catch (Exception ex)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRomFile"] + ": " + ex.Message,
                                ls["MessageCaption_RenameRom"]);
                        Trace.WriteLine("UNABLE to rename rom file: " + ex.Message, "Rom rename");
                    }
                }
                if (frm.RenameRelatedFiles)
                {
                    try
                    {
                        Trace.WriteLine("Renaming related files ....", "Rom rename");
                        if (rom.RomInfoItems != null)
                        {
                            foreach (InformationContainerItem it in rom.RomInfoItems)
                            {
                                if (it is InformationContainerItemFiles)
                                {
                                    InformationContainerItemFiles itf = (InformationContainerItemFiles)it;
                                    // Get parent console
                                    EmulatorsOrganizer.Core.Console con = profileManager.Profile.Consoles[rom.ParentConsoleID];
                                    // Get ic name
                                    InformationContainer ic = con.GetInformationContainer(it.ParentID);
                                    if (ic == null)
                                    {
                                        Trace.WriteLine("Information container not exist with id " + it.ParentID, "Rom rename");
                                        continue;
                                    }
                                    if (itf.Files != null)
                                        for (int f = 0; f < itf.Files.Count; f++)
                                        {
                                            string newPath = "";
                                            string failEx = "";
                                            string newName = "";
                                            switch (frm.RenameingMethodChosen)
                                            {
                                                case Form_RenameRoms.RenameingMethod.NewRomName_InfoFileIndex:
                                                    {
                                                        newName = frm.RomName + "(" + f + ")";
                                                        break;
                                                    }
                                                case Form_RenameRoms.RenameingMethod.NewRomName_InfoName_InfoFileIndex:
                                                    {
                                                        newName = frm.RomName + "_" + ic.Name + "(" + f + ")";
                                                        break;
                                                    }
                                                case Form_RenameRoms.RenameingMethod.InfoName_NewRomName_InfoFileIndex:
                                                    {
                                                        newName = ic.Name + "_" + frm.RomName + "(" + f + ")";
                                                        break;
                                                    }
                                            }
                                            if (HelperTools.RenameFile(HelperTools.GetFullPath(itf.Files[f]), newName, out newPath, out failEx))
                                            {
                                                itf.Files[f] = HelperTools.GetDotPath(newPath);
                                                Trace.WriteLine(">Related file renamed successfully.", "Rom rename");
                                                Trace.WriteLine(">Related path set to:", "Rom rename");
                                                Trace.WriteLine(">" + newPath, "Rom rename");
                                            }
                                            else
                                            {
                                                ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRelatedFile"] + ": " + failEx,
                                                    ls["MessageCaption_RenameRom"]);
                                                Trace.WriteLine("UNABLE to rename related file: " + failEx, "Rom rename");
                                            }
                                        }
                                }
                            }
                        }
                        else
                        {
                            Trace.WriteLine("UNABLE to rename related files, no file included in this rom.", "Rom rename");
                        }
                    }
                    catch (Exception ex)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_UNABLEToRenameRomFile"] + ": " + ex.Message,
                                ls["MessageCaption_RenameRom"]);
                        Trace.WriteLine("UNABLE to rename rom file: " + ex.Message, "Rom rename");
                    }
                }
                profileManager.Profile.OnRomRenamed(oldName, rom.Name);
            }
        }
        public override void ShowItemProperties()
        {
            if (profileManager.IsSaving)
                return;
            base.ShowItemProperties();
            if (managedListView1.SelectedItems.Count > 0)
            {
                Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[0].Tag.ToString()];
                RomProperties frm = new RomProperties(rom.ID, rom.ParentConsoleID);
                frm.ShowDialog(this);
                profileManager.Profile.OnRequestCategoriesListClear();
                // Update categories
                foreach (ManagedListViewItem item in managedListView1.Items)
                {
                    profileManager.Profile.OnRomShowed(rom);
                }
            }
        }
        public override void ChangeIcon()
        {
            if (profileManager.IsSaving)
                return;
            OpenFileDialog Op = new OpenFileDialog();
            Op.Title = ls["Title_BrowseForAnIcon"];
            Op.Filter = ls["Filter_Icon"]; ;
            Op.Multiselect = false;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                Bitmap icon = null;
                if (Path.GetExtension(Op.FileName).ToLower() == ".exe" | Path.GetExtension(Op.FileName).ToLower() == ".ico")
                {
                    icon = Icon.ExtractAssociatedIcon(Op.FileName).ToBitmap();
                }
                else
                {
                    //Stream str = new FileStream(Op.FileName, FileMode.Open, FileAccess.Read);
                    //byte[] buff = new byte[str.Length];
                    //str.Read(buff, 0, (int)str.Length);
                    //str.Dispose();
                    //str.Close();
                    //icon = (Bitmap)Image.FromStream(new MemoryStream(buff));

                    using (var bmpTemp = new Bitmap(Op.FileName))
                    {
                        icon = new Bitmap(bmpTemp);
                    }
                }
                OnProgressStarted();
                int i = 0;
                oldP = -1;
                foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                {
                    profileManager.Profile.Roms[item.Tag.ToString()].Icon = icon;
                    i++;
                    int x = (i * 100) / managedListView1.SelectedItems.Count;
                    if (oldP != x)
                    {
                        OnProgress(ls["Status_ChangingIcon"] + "...", x);
                        oldP = x;
                    }
                }
                OnProgressFinished();
                profileManager.Profile.OnRomIconsChanged(managedListView1.SelectedItems.Count);

            }
        }
        public override void ClearIcon()
        {
            if (profileManager.IsSaving)
                return;
            base.ClearIcon();
            ManagedMessageBoxResult resul = ManagedMessageBox.ShowQuestionMessage(
                ls["Message_AreYouSureYouWantToClearIconsForSelectedItems"] +
                "\n" + managedListView1.SelectedItems.Count + " " + ls["Status_RomsSelected"],
                ls["MessageCaption_ClearIcon"]);
            if (resul.ClickedButtonIndex == 0)
            {
                OnProgressStarted();
                int i = 0;
                oldP = -1;
                foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                {
                    profileManager.Profile.Roms[item.Tag.ToString()].Icon = null;
                    i++;
                    int x = (i * 100) / managedListView1.SelectedItems.Count;
                    if (oldP != x)
                    {
                        OnProgress(ls["Status_ClearingIcon"] + "...", x);
                        oldP = x;
                    }
                }
                OnProgressFinished();
                profileManager.Profile.OnRomIconsChanged(managedListView1.SelectedItems.Count);
            }
        }
        protected override void OnEnableDisableButtons()
        {
            base.OnEnableDisableButtons();
            deleteToolStripMenuItem.Enabled = CanDelete;
            renameToolStripMenuItem.Enabled = CanRename;
            clearIconToolStripMenuItem.Enabled = changeIconToolStripMenuItem.Enabled = CanChangeIcon;
            propertiesToolStripMenuItem.Enabled = CanShowProperties;
            playToolStripMenuItem.Enabled = CanPlayRom;
            addDataInfoAsCategoriesToolStripMenuItem.Enabled = editCategoriesToolStripMenuItem.Enabled = managedListView1.SelectedItems.Count > 0;
        }
        public void ChangeExtension()
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectRomsFirst"],
                    ls["MessageCaption_ChangeExtension"]);
                return;
            }
            Form_EnterName nam = new Form_EnterName(ls["Title_EnterTheNewExtension"], ".exe", true, false);
            if (nam.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Changing extension of roms ...", "Change Extension");
                string NewExtension = nam.EnteredName;
                if (!NewExtension.StartsWith("."))
                    NewExtension = "." + NewExtension;
                List<string> blackListedConsoles = new List<string>();
                foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                {
                    Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                    EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
                    if (blackListedConsoles.Contains(console.ID))
                        continue;
                    if (!console.Extensions.Contains(NewExtension.ToLower()))
                    {
                        ManagedMessageBoxResult result = ManagedMessageBox.ShowMessage(this,
                            ls["Message_ThisExtensioinIsntExistInTheConsole"] + " (" + console.Name + "), " +
                            ls["Message_DoYouWantToAddThisExtensionToThisConsole"],
                            ls["MessageCaption_ChangeExtension"], new string[]
                            {
                                ls["Button_Yes"],ls["Button_No"],ls["Button_SkipConsole"]
                            }, 1, ManagedMessageBoxIcon.Question);
                        if (result.ClickedButtonIndex == 0)// Yes
                        {
                            profileManager.Profile.Consoles[rom.ParentConsoleID].Extensions.Add(NewExtension);
                            Trace.WriteLine(string.Format("> Extension '{0}' added to console '{1}'", NewExtension, console.Name),
                                "Change Extension");
                        }
                        else if (result.ClickedButtonIndex == 1)// No
                        {
                            //skip this rom
                            continue;
                        }
                        else if (result.ClickedButtonIndex == 2)// Skip console
                        {
                            //skip this console... add to the black list
                            blackListedConsoles.Add(console.ID);
                            Trace.WriteLine(string.Format("> Console '{0}' skipped by user.", console.Name),
                                "Change Extension");
                            continue;
                        }
                    }
                    try
                    {
                        string fol = Path.GetDirectoryName(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)));
                        if (fol == "")
                            fol = Path.GetPathRoot(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)));

                        string Orgenal = HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path));
                        string Neww = fol + "\\" + Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)))
                            + "." + NewExtension;
                        File.Copy(Orgenal, Neww);
                        FileInfo inf = new FileInfo(Orgenal);
                        inf.IsReadOnly = false;
                        File.Delete(Orgenal);

                        // To ensure rom get path probably, we better do it like this:
                        profileManager.Profile.Roms[rom.ID].Path = HelperTools.GetDotPath(Neww);
                        // Refresh the item, no need to reload roms ....
                        FillRomItemSubitems(rom.ID, managedListView1.Items.IndexOf(item));

                        Trace.WriteLine(string.Format("> Rom '{0}' extension changed from '{1}' to '{2}'", rom.Name,
                            Path.GetExtension(Orgenal), Path.GetExtension(Neww)), "Change Extension");
                    }
                    catch { }
                }
                profileManager.Profile.OnExtensionChange();
                managedListView1.Invalidate();
                Trace.WriteLine("Extension change done.", "Change Extension");
            }
        }
        public void SelectAllRoms()
        {
            foreach (ManagedListViewItem item in managedListView1.Items)
                item.Selected = true;
            managedListView1.Invalidate();
        }
        public int SelectedRomsCount()
        { return managedListView1.SelectedItems.Count; }
        public void GetRomDataFromMobyGames()
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectOneRomFirst"],
                  ls["MessageCaption_GetDataFromMobyGames"]);
                return;
            }
            Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[0].Tag.ToString()];
            Form_MobyGames frm = new Form_MobyGames(rom.Name);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (!frm.AddDescriptionAsTabInfo)
                {
                    AddNewIC("Description", rom.ParentConsoleID);
                    AddDataToRom("description", frm.i_Description, rom);
                }
                else
                {
                    InformationContainerInfoText Cont = null;
                    Core.Console selectedConsole = profileManager.Profile.Consoles[rom.ParentConsoleID];
                    if (frm.InfoTabID == "Descripion (Create New)")
                    {
                        // Create new
                        Cont = new InformationContainerInfoText(profileManager.Profile.GenerateID());
                        Cont.DisplayName = ls["Tab_Description"];
                        Cont.Name = ls["Tab_Description"];
                        // Make it visible
                        if (!selectedConsole.InformationContainersMap.AddNewContainerID(Cont.ID))
                        {
                            if (selectedConsole.InformationContainersMap.ContainerIDS == null)
                                selectedConsole.InformationContainersMap.ContainerIDS = new List<string>();
                            selectedConsole.InformationContainersMap.ContainerIDS.Add(Cont.ID);
                        }
                        selectedConsole.InformationContainers.Add(Cont);
                    }
                    else
                    {
                        Cont = (InformationContainerInfoText)selectedConsole.GetInformationContainer(frm.InfoTabID);
                    }
                    // Add the memory folder
                    string folder = Path.GetDirectoryName(frm.InfoTabFilePath);
                    if (Cont.FoldersMemory == null)
                        Cont.FoldersMemory = new List<string>();
                    if (!Cont.FoldersMemory.Contains(folder))
                        Cont.FoldersMemory.Add(folder);
                    // Save the file first !
                    string ex = Path.GetExtension(frm.InfoTabFilePath).ToLower();
                    RichTextBox richTextBox1 = new RichTextBox();
                    richTextBox1.Text = frm.i_Description;
                    switch (ex)
                    {
                        case ".rtf": richTextBox1.SaveFile(frm.InfoTabFilePath, RichTextBoxStreamType.RichText); break;
                        case ".doc": richTextBox1.SaveFile(frm.InfoTabFilePath, RichTextBoxStreamType.RichText); break;
                        default: File.WriteAllLines(frm.InfoTabFilePath, richTextBox1.Lines); break;
                    }

                    // Add the element to the rom
                    if (!rom.IsInformationContainerItemExist(Cont.ID))
                    {
                        // Create new
                        InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), Cont.ID);
                        item.Files.Add(frm.InfoTabFilePath);
                        rom.RomInfoItems.Add(item);
                        rom.Modified = true;
                    }
                    else
                    {
                        // Update
                        foreach (InformationContainerItem item in rom.RomInfoItems)
                        {
                            if (item.ParentID == Cont.ID)
                            {
                                InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                                if (ictem.Files == null)
                                    ictem.Files = new List<string>();
                                if (!ictem.Files.Contains(frm.InfoTabFilePath))
                                    ictem.Files.Add(frm.InfoTabFilePath);
                                break;
                            }
                        }
                    }
                    profileManager.Profile.OnInformationContainerItemsModified(Cont.Name);
                }
                AddNewIC("Developed By", rom.ParentConsoleID);
                AddDataToRom("developed by", frm.i_DevelopedBy, rom);

                AddNewIC("Genre", rom.ParentConsoleID);
                AddDataToRom("genre", frm.i_Genre, rom);
                if (frm.IncludeCategories)
                {
                    string[] cats = frm.i_Genre.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string cat in cats)
                    {
                        if (cat != "")
                        {
                            if (!rom.Categories.Contains(cat))
                            {
                                rom.Categories.Add(cat);
                            }
                        }
                    }
                }

                AddNewIC("Platform", rom.ParentConsoleID);
                AddDataToRom("platform", frm.i_Platform, rom);
                AddNewIC("Published By", rom.ParentConsoleID);
                AddDataToRom("published by", frm.i_PublishedBy, rom);
                AddNewIC("Released", rom.ParentConsoleID);
                AddDataToRom("released", frm.i_Released, rom);

                SelectTheSameRoms = true;

                SaveColumns();
                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
                console.FixColumnsForRomDataInfo();
                // Refresh columns
                profileManager.Profile.OnConsolePropertiesChanged(console.Name);
            }
        }
        public void GetRomDataFromTheGamesDB()
        {
            ManagedMessageBox.ShowMessage("This feature is disabled temporary in this version, working on updating the api handler.");
            return;
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectRomsFirstToExportToDatabase"],
                  ls["MessageCaption_GetDataFromTheGamesDB"]);
                return;
            }
            if (managedListView1.SelectedItems.Count == 1)
            {
                // For a single rom ...
                Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[0].Tag.ToString()];
                Form_SearchTheGamesDB frm = new Form_SearchTheGamesDB(rom.ID);
                string rID = rom.ID;
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    SelectTheSameRoms = true;
                    Form_AssignDataFromTheGamesDB frm2 = new Form_AssignDataFromTheGamesDB(rom.ID, frm.SelectedResultID);
                    if (frm2.ShowDialog(this) == DialogResult.OK)
                    {
                        SaveColumns();
                        EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
                        console.FixColumnsForRomDataInfo();
                        RefreshParentSelection(list_view_selectRoms = true, list_view_selectedRoms = selectedRomsIDS = new List<string>(new string[] { rID }));
                    }
                }
            }
            else
            {
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.Console:
                        {
                            // This is it !!
                            // Collect the roms
                            List<Rom> roms = new List<Rom>();
                            foreach (ManagedListViewItem it in managedListView1.SelectedItems)
                            {
                                roms.Add(profileManager.Profile.Roms[it.Tag.ToString()]);
                            }
                            // Do it !!
                            Form_DetectAndDownloadFromTheGameDBRoms frm = new Form_DetectAndDownloadFromTheGameDBRoms(profileManager.Profile.SelectedConsoleID, roms.ToArray());
                            if (frm.ShowDialog(this) == DialogResult.OK)
                            {
                                profileManager.Profile.OnInformationContainerItemsDetected();
                            }
                            break;
                        }
                    case SelectionType.ConsolesGroup:
                    case SelectionType.Playlist:
                    case SelectionType.PlaylistsGroup:
                        {
                            // We need to check that all roms have the same parent !
                            // Collect the roms
                            List<Rom> roms = new List<Rom>();
                            string consoleID = "";
                            foreach (ManagedListViewItem it in managedListView1.SelectedItems)
                            {
                                if (consoleID != "")
                                {
                                    if (profileManager.Profile.Roms[it.Tag.ToString()].ParentConsoleID != consoleID)
                                    {
                                        ManagedMessageBox.ShowErrorMessage(ls["Message_AllSelectedRomsMustBelongToTheSameParentConsole"], ls["MessageCaption_GetDataFromTheGamesDB"]);
                                        return;
                                    }
                                }
                                consoleID = profileManager.Profile.Roms[it.Tag.ToString()].ParentConsoleID;
                                roms.Add(profileManager.Profile.Roms[it.Tag.ToString()]);
                            }
                            // Do it !!
                            Form_DetectAndDownloadFromTheGameDBRoms frm = new Form_DetectAndDownloadFromTheGameDBRoms(consoleID, roms.ToArray());
                            if (frm.ShowDialog(this) == DialogResult.OK)
                            {
                                profileManager.Profile.OnInformationContainerItemsDetected();
                            }
                            break;
                        }
                }

            }
        }
        private void AddNewIC(string newic, string consoleID)
        {
            if (profileManager.IsSaving)
                return;
            //search the profile for information container that match the same name
            bool found = false;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            foreach (RomData ic in console.RomDataInfoElements)
            {
                if (ic.Name.ToLower() == newic.ToLower())
                {
                    found = true; break;
                }
            }
            if (!found)
            {
                //add new ic with this project
                RomData newIC = new RomData(profileManager.Profile.GenerateID(), newic, RomDataType.Text);
                console.RomDataInfoElements.Add(newIC);
            }
        }
        private void AddDataToRom(string icName, string data, Rom rom)
        {
            if (profileManager.IsSaving)
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
            foreach (RomData ic in console.RomDataInfoElements)
            {
                if (ic.Name.ToLower() == icName.ToLower())
                {
                    rom.UpdateDataInfoItemValue(ic.ID, data);
                    break;
                }
            }
        }
        public void ExportToDatabase()
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectRomsFirstToExportToDatabase"],
                    ls["MessageCaption_ExportToDatabaseFile"]);
                return;
            }
            List<string> ids = new List<string>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                ids.Add(item.Tag.ToString());
            }
            Form_ExportToDatabase frm = new Form_ExportToDatabase(ids.ToArray());
            frm.ShowDialog();
        }
        protected override void OnNewProfileCreated()
        {
            base.OnNewProfileCreated();
            if (isLoading)
                return;
            // Clear
            managedListView1.Columns.Clear();
            managedListView1.Items.Clear();
            imageList_listView.Images.Clear();
            consoleImageIDs.Clear();
        }
        protected override void OnProfileOpened()
        {
            base.OnProfileOpened();
            if (isLoading)
                return;
            // Clear
            managedListView1.Items.Clear();
            imageList_listView.Images.Clear();
            consoleImageIDs.Clear();
            // See if profile request memory
            if (profileManager.Profile.RememberLatestSelectedRomOnProfileOpen)
            {
                if (profileManager.Profile.SelectedRomIDS == null)
                    profileManager.Profile.SelectedRomIDS = new List<string>();
                List<string> selectedRoms = new List<string>(profileManager.Profile.SelectedRomIDS);
                RefreshParentSelection(true, selectedRoms);

                //managedListView1.Invalidate();
                //UpdateRomsSelection();
                //OnEnableDisableButtons();
            }
        }
        private Image TakeThumbnailImageFromCache(string romID, string filePath)
        {
            try
            {
                string id = string.Format("{0}+{1}", romID, filePath);
                // Try to get the image from the cache first ...
                if (ThumbnailsCacheIDS.Contains(id))
                {
                    // This is it !! We have it :D
                    int index = ThumbnailsCacheIDS.IndexOf(id);
                    return ThumbnailsCache[index];
                }
                else
                {
                    //Stream str = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    //byte[] buff = new byte[str.Length];
                    //str.Read(buff, 0, (int)str.Length);
                    //str.Dispose();
                    //str.Close();

                    int x = 0;
                    int y = 0;
                    int w = 0;
                    int h = 0;

                    //Image img = Image.FromStream(new MemoryStream(buff));
                    Image img = null;
                    using (var bmpTemp = new Bitmap(filePath))
                    {
                        img = new Bitmap(bmpTemp);
                    }
                    GetRatioStretchRectangle(img.Width, img.Height, 500, 500, ref x, ref y, ref w, ref h);

                    // Add it normally and to the cache ...
                    ThumbnailsCacheIDS.Add(id);
                    if (h > 0 && w > 0)
                        ThumbnailsCache.Add(img.GetThumbnailImage(w, h, null, IntPtr.Zero));
                    else
                        ThumbnailsCache.Add(img);

                    // Keep the count to ThumbnailsCacheMaxSize to protect memory ...
                    if (ThumbnailsCache.Count > ThumbnailsCacheMaxSize)
                    {
                        ThumbnailsCacheIDS.RemoveAt(0);
                        ThumbnailsCache.RemoveAt(0);
                    }

                    return img;
                }
            }
            catch { }
            return null;
        }
        private Image TakeDetailsImageFromCache(string romID)
        {
            try
            {
                // Try to get the image from the cache first ...
                if (DetailsCacheIDS.Contains(romID))
                {
                    // This is it !! We have it :D
                    int index = DetailsCacheIDS.IndexOf(romID);
                    return DetailsCache[index];
                }
                else
                {
                    Rom rom = profileManager.Profile.Roms[romID];
                    Image img = null;
                    if (rom.Icon != null)
                        img = rom.IconThumbnail;
                    else
                        img = imageList1.Images[1];

                    // Add it normally and to the cache ...
                    DetailsCacheIDS.Add(romID);
                    DetailsCacheNames.Add(rom.Name);
                    DetailsCache.Add(img);
                    // Keep the count to ThumbnailsCacheMaxSize to protect memory ...
                    if (DetailsCache.Count > DetailsCacheMaxSize)
                    {
                        DetailsCacheIDS.RemoveAt(0);
                        DetailsCache.RemoveAt(0);
                        DetailsCacheNames.RemoveAt(0);
                    }

                    return img;
                }
            }
            catch { }
            return null;
        }
        private string TakeDetailsTextFromCache(string romID)
        {
            try
            {
                // Try to get the image from the cache first ...
                if (DetailsCacheIDS.Contains(romID))
                {
                    // This is it !! We have it :D
                    int index = DetailsCacheIDS.IndexOf(romID);
                    return DetailsCacheNames[index];
                }
                else
                {
                    Rom rom = profileManager.Profile.Roms[romID];
                    Image img = null;
                    if (rom.Icon != null)
                        img = rom.IconThumbnail;
                    else
                        img = imageList1.Images[1];

                    // Add it normally and to the cache ...
                    DetailsCacheIDS.Add(romID);
                    DetailsCacheNames.Add(rom.Name);
                    DetailsCache.Add(img);
                    // Keep the count to ThumbnailsCacheMaxSize to protect memory ...
                    if (DetailsCache.Count > DetailsCacheMaxSize)
                    {
                        DetailsCacheIDS.RemoveAt(0);
                        DetailsCache.RemoveAt(0);
                        DetailsCacheNames.RemoveAt(0);
                    }

                    return rom.Name;
                }
            }
            catch { }
            return null;
        }
        private void GetRatioStretchRectangle(int orgWidth, int orgHeight, int maxWidth, int maxHeight,
                                                   ref int out_x, ref int out_y, ref int out_w, ref int out_h)
        {
            float hRatio = orgHeight / maxHeight;
            float wRatio = orgWidth / maxWidth;
            bool touchTargetFromOutside = false;
            if ((wRatio > hRatio) ^ touchTargetFromOutside)
            {
                out_w = maxWidth;
                out_h = (orgHeight * maxWidth) / orgWidth;
            }
            else
            {
                out_h = maxHeight;
                out_w = (orgWidth * maxHeight) / orgHeight;
            }
            out_x = (maxWidth - out_w) / 2;
            out_y = (maxHeight - out_h) / 2;
        }

        private void Profile_ConsoleRemoved(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_PlaylistsGroupRemoved(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_PlaylistRemoved(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_ConsolePropertiesChanged(object sender, EventArgs e)
        {
            RefreshParentSelection(SelectTheSameRoms, selectedRomsIDS);
            RefreshStyle();

            SelectTheSameRoms = false;
        }
        private void subItem4_UpdateRatingRequest(object sender, ManagedListViewRatingChangedArgs e)
        {
            if (profileManager.IsSaving)
                return;
            Rom rom = profileManager.Profile.Roms[managedListView1.Items[e.ItemIndex].Tag.ToString()];
            if (rom != null)
                ((ManagedListViewRatingSubItem)managedListView1.Items[e.ItemIndex].GetSubItemByID("rating")).Rating = rom.Rating;
        }
        private void subItem4_RatingChanged(object sender, ManagedListViewRatingChangedArgs e)
        {
            Rom rom = profileManager.Profile.Roms[managedListView1.Items[e.ItemIndex].Tag.ToString()];
            rom.Rating = e.Rating;
            profileManager.Profile.OnRomRatingChanged(rom.Name);
        }
        private void Profile_PlaylistSelectionChanged(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_PlaylistsGroupSelectionChanged(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_ConsoleSelectionChanged(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_RomsRefreshRequest(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_ConsolesGroupSelectionChanged(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_RomsAdded(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_RomsRemoved(object sender, EventArgs e)
        {

        }
        private void Profile_RomIconsChanged(object sender, EventArgs e)
        {
            managedListView1.Invalidate();
        }
        private void Profile_RomRenamed(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count != 1)
                return;
            Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[0].Tag.ToString()];
            managedListView1.SelectedItems[0].Text = rom.Name;

            if (roms_thumbs.ContainsKey(managedListView1.SelectedItems[0].Tag.ToString()))
                roms_thumbs[managedListView1.SelectedItems[0].Tag.ToString()].RomName = rom.Name;

            if (DetailsCacheIDS.Contains(managedListView1.SelectedItems[0].Tag.ToString()))
            {
                // This is it !! We have it :D
                int index = DetailsCacheIDS.IndexOf(managedListView1.SelectedItems[0].Tag.ToString());
                DetailsCacheNames[index] = rom.Name;
            }

            managedListView1.Invalidate();
        }
        private void Profile_RomsRemovedFromPlaylist(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_RomPropertiesChanged(object sender, RomPropertiesChangedArgs e)
        {
            if (!e.RefreshRequired)
            {
                for (int i = 0; i < managedListView1.Items.Count; i++)
                {
                    if (managedListView1.Items[i].Tag.ToString() == e.RomID)
                    {
                        FillRomItemSubitems(e.RomID, i);
                        break;
                    }
                }
                managedListView1.Invalidate();
            }
            else
                RefreshParentSelection(false, null);
        }
        private void Profile_RomMultiplePropertiesChanged(object sender, EventArgs e)
        {
            // Update selected roms
            for (int i = 0; i < managedListView1.Items.Count; i++)
            {
                FillRomItemSubitems(managedListView1.Items[i].Tag.ToString(), i);
            }
            managedListView1.Invalidate();
        }
        private void Profile_RomFinishedPlayed(object sender, RomFinishedPlayArgs e)
        {
            for (int i = 0; i < managedListView1.Items.Count; i++)
            {
                if (managedListView1.Items[i].Tag.ToString() == e.RomID)
                {
                    FillRomItemSubitems(e.RomID, i);
                    break;
                }
            }
            managedListView1.Invalidate();
        }
        private void Profile_InformationContainerItemsDetected(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
            // Refresh all items to get thumb info
            if (managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails)
            {
                if (current_roms != null)
                {
                    foreach (Rom rom in current_roms)
                    {
                        FillRomThumbInfos(rom.ID);
                    }
                }
            }

            managedListView1.Invalidate();
        }
        private void Profile_InformationContainerItemsModified(object sender, EventArgs e)
        {
            // Refresh selection.
            for (int i = 0; i < managedListView1.SelectedItems.Count; i++)
            {
                FillRomItemSubitems(managedListView1.SelectedItems[i].Tag.ToString(), managedListView1.Items.IndexOf(managedListView1.SelectedItems[i]));
            }
            // Refresh all items to get thumb info
            if (managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails)
            {
                if (current_roms != null)
                {
                    foreach (Rom rom in current_roms)
                    {
                        FillRomThumbInfos(rom.ID);
                    }
                }
            }
            managedListView1.Invalidate();
        }
        private void Profile_ProfileCleanUpFinished(object sender, EventArgs e)
        {
            RefreshParentSelection(false, null);
        }
        private void Profile_DatabaseImported(object sender, EventArgs e)
        {
            SaveColumns();
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
            console.FixColumnsForRomDataInfo();
            RefreshParentSelection(false, null);
        }
        // SEARCH request
        private void Profile_RequestSearch(object sender, SearchRequestArgs e)
        {
            doSearch = true;
            searchParameters = e;
            RefreshParentSelection(false, null);
        }
        private void Profile_RomSelectionChanged(object sender, EventArgs e)
        {
            if (isSelecting) return;
            if (profileManager.Profile.SelectedRomIDS == null) return;
            List<string> selectedRoms = new List<string>(profileManager.Profile.SelectedRomIDS);
            // Select the roms that was selected
            bool select = false;
            foreach (ManagedListViewItem item in managedListView1.Items)
            {
                if (selectedRoms.Contains(item.Tag.ToString()))
                {
                    item.Selected = true;
                    if (!select)
                    {
                        managedListView1.ScrollToItem(item);
                        select = true;
                    }
                }
            }
            managedListView1.Invalidate();
        }

        /*Draw stuff*/
        private void managedListView1_DrawSubItem(object sender, ManagedListViewSubItemDrawArgs e)
        {
            if (e.ParentItem == null) return;
            if (e.ColumnID == "name")
            {
                if (e.ImageToDraw != null)
                    e.ImageToDraw.Dispose();
                e.TextToDraw = TakeDetailsTextFromCache(e.ParentItem.Tag.ToString());
                e.ImageToDraw = TakeDetailsImageFromCache(e.ParentItem.Tag.ToString());
            }
        }
        private void managedListView1_DrawItem(object sender, ManagedListViewItemDrawArgs e)
        {
            if (e.ImageToDraw != null)
                e.ImageToDraw.Dispose();
            RomThumbnailInfo thumbIf = roms_thumbs[managedListView1.Items[e.ItemIndex].Tag.ToString()];

            // Draw image; Search for first item that contain drawable files
            if (thumbIf != null)
            {
                // Draw text
                e.TextToDraw = thumbIf.RomName;
                if (thumbIf.ThumbnailFiles != null)
                {
                    if (thumbIf.ThumbFileIndex >= 0 && thumbIf.ThumbFileIndex < thumbIf.ThumbnailFiles.Length)
                    {
                        Image img = TakeThumbnailImageFromCache(managedListView1.Items[e.ItemIndex].Tag.ToString(),
                            HelperTools.GetFullPath(thumbIf.ThumbnailFiles[thumbIf.ThumbFileIndex]));
                        if (img != null)
                        {
                            e.ImageToDraw = (Bitmap)img;
                            return;
                        }
                    }
                }
            }
            Rom rom = profileManager.Profile.Roms[managedListView1.Items[e.ItemIndex].Tag.ToString()];
            e.TextToDraw = rom.Name;
            // Reached here means we can't draw anything but rom icon
            if (rom.IconThumbnail != null)
                e.ImageToDraw = rom.IconThumbnail;
            else
                e.ImageToDraw = imageList1.Images[1];
        }
        // Thumbnails timer
        private void timer_thumbCycle_Tick(object sender, EventArgs e)
        {
            if (managedListView1.ViewMode != ManagedListViewViewMode.Thumbnails)
            {
                timer_thumbCycle.Stop();
                return;
            }
            if (!autoCycleThumbnails)
            {
                timer_thumbCycle.Stop();
                return;
            }
            showNextThumbnailToolStripMenuItem_Click(this, null);
        }
        // Show next thumbnail
        private void showNextThumbnailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 1)
            {
                RomThumbnailInfo thumbIf = roms_thumbs[managedListView1.SelectedItems[0].Tag.ToString()];
                if (thumbIf != null)
                {
                    if (thumbIf.ThumbnailFiles != null)
                    {
                        if (thumbIf.ThumbnailFiles.Length > 0)
                        {
                            thumbIf.ThumbFileIndex = (thumbIf.ThumbFileIndex + 1) % thumbIf.ThumbnailFiles.Length;
                            managedListView1.Invalidate();
                        }
                    }
                }
            }
        }
        private void managedListView1_AfterColumnResize(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            SaveColumns();
            if (element == null)
                return;
            if (element is Core.Console)
                profileManager.Profile.OnConsoleColumnResized(element.Name);
            else if (element is Core.ConsolesGroup)
                profileManager.Profile.OnConsolesGroupColumnResized(element.Name);
            else if (element is Core.Playlist)
                profileManager.Profile.OnPlaylistColumnResized(element.Name);
            else if (element is Core.PlaylistsGroup)
                profileManager.Profile.OnPlaylistsGroupColumnResized(element.Name);
        }
        private void managedListView1_AfterColumnReorder(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            SaveColumns();
            if (element == null)
                return;
            if (element is Core.Console)
                profileManager.Profile.OnConsoleColumnReorder(element.Name);
            else if (element is Core.ConsolesGroup)
                profileManager.Profile.OnConsolesGroupColumnReorder(element.Name);
            else if (element is Core.Playlist)
                profileManager.Profile.OnPlaylistColumnReorder(element.Name);
            else if (element is Core.PlaylistsGroup)
                profileManager.Profile.OnPlaylistsGroupColumnReorder(element.Name);
        }
        private void managedListView1_SwitchToNormalContextMenu(object sender, EventArgs e)
        {
            managedListView1.ContextMenuStrip = contextMenuStrip_normal;
        }
        private void managedListView1_SwitchToColumnsContextMenu(object sender, EventArgs e)
        {
            managedListView1.ContextMenuStrip = contextMenuStrip_columns;
        }
        private void contextMenuStrip_columns_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip_columns.Items.Clear();
            if (element == null)
                return;
            foreach (ColumnItem item in element.Columns)
            {
                ToolStripMenuItem mitem = new ToolStripMenuItem();
                mitem.Text = item.ColumnName;
                mitem.Checked = item.Visible;
                contextMenuStrip_columns.Items.Add(mitem);
            }
        }
        private void contextMenuStrip_columns_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            OnColumnMenuItemClick(e.ClickedItem.Text);
        }
        private void managedListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionTimer = selectionTimerReload;
            timer_selection.Start();
        }
        private void timer_selection_Tick(object sender, EventArgs e)
        {
            if (selectionTimer > 0)
                selectionTimer--;
            else
            {
                timer_selection.Stop();
                UpdateRomsSelection();
                OnEnableDisableButtons();
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameSelected();
        }
        private void changeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeIcon();
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowItemProperties();
        }
        private void clearIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearIcon();
        }
        private void contextMenuStrip_normal_Opening(object sender, CancelEventArgs e)
        {
            OnEnableDisableButtons();
            openLocationToolStripMenuItem.Enabled = managedListView1.SelectedItems.Count == 1;
            getNameAndDataFromMobyGamescomToolStripMenuItem.Enabled =
            managedListView1.SelectedItems.Count == 1;

            getDataFromTheGamesDBForThisGameToolStripMenuItem.Enabled = clearRelatedFilesToolStripMenuItem.Enabled = sendToToolStripMenuItem.Enabled =
            applyRomNamesToFilesToolStripMenuItem.Enabled = exportNamesToolStripMenuItem.Enabled = managedListView1.SelectedItems.Count > 0;
            if (managedListView1.SelectedItems.Count == 0)
            {
                sendToPlaylistToolStripMenuItem.Enabled = false;
                openLocationToolStripMenuItem.Enabled = false;
                return;
            }
            #region Load playlists
            sendToPlaylistToolStripMenuItem.DropDownItems.Clear();

            if (profileManager.Profile.Playlists.Count == 0)
            {
                sendToPlaylistToolStripMenuItem.Enabled = false;
            }
            else
            {
                sendToPlaylistToolStripMenuItem.Enabled = true;

                // load groups first
                foreach (PlaylistsGroup gr in profileManager.Profile.PlaylistGroups)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Text = gr.Name;
                    item.Image = Properties.Resources.folder;
                    // now add playlists that belong to this group
                    Playlist[] playlists = profileManager.Profile.Playlists[gr.ID, false];
                    foreach (Playlist pl in playlists)
                    {
                        ToolStripMenuItem plitem = new ToolStripMenuItem();
                        plitem.Text = pl.Name;
                        if (pl.Icon != null)
                            plitem.Image = pl.Icon;
                        else
                            plitem.Image = Properties.Resources.Favorites;
                        plitem.Tag = pl.ID;
                        item.DropDownItems.Add(plitem);
                    }
                    item.DropDownItemClicked += sendToPlaylistToolStripMenuItem_DropDownItemClicked;
                    sendToPlaylistToolStripMenuItem.DropDownItems.Add(item);
                }
                // load playlists without parent
                Playlist[] playlistsNoParent = profileManager.Profile.Playlists["", false];
                foreach (Playlist pl in playlistsNoParent)
                {
                    ToolStripMenuItem plitem = new ToolStripMenuItem();
                    plitem.Text = pl.Name;
                    if (pl.Icon != null)
                        plitem.Image = pl.Icon;
                    else
                        plitem.Image = Properties.Resources.Favorites;
                    plitem.Tag = pl.ID;
                    sendToPlaylistToolStripMenuItem.DropDownItems.Add(plitem);
                }
            }
            #endregion
            #region Load sort columns
            sortToolStripMenuItem.DropDownItems.Clear();
            if (element == null)
                return;
            foreach (ColumnItem item in element.Columns)
            {
                ToolStripMenuItem mitem = new ToolStripMenuItem();
                mitem.Text = item.ColumnName;
                mitem.Tag = item.ColumnID;
                sortToolStripMenuItem.DropDownItems.Add(mitem);
            }
            #endregion
        }
        // Handle playlist add
        private void sendToPlaylistToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Get playlist id
            string plID = (string)e.ClickedItem.Tag;
            if (!profileManager.Profile.Playlists.ContainsID(plID))
                return;
            // Get roms
            List<Rom> items = new List<Rom>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                items.Add(profileManager.Profile.Roms[item.Tag.ToString()]);
            }
            // Set items to playlist
            profileManager.Profile.AddRomsToPlaylist(items.ToArray(), plID);
        }
        private void editCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                ids.Add(item.Tag.ToString());
            RomPropertiesMultibleEdit frm = new RomPropertiesMultibleEdit(ids.ToArray(), null);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                profileManager.Profile.OnRequestCategoriesListClear();
                // Update categories
                foreach (ManagedListViewItem item in managedListView1.Items)
                {
                    Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                    profileManager.Profile.OnRomShowed(rom);
                }
            }
        }
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                profileManager.Profile.PlayRom();
            }
            catch (Exception ex)
            {
                ManagedMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        private void managedListView1_ItemDoubleClick(object sender, ManagedListViewItemDoubleClickArgs e)
        {
            playToolStripMenuItem_Click(this, null);
        }
        private void openLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[managedListView1.SelectedItems[0].Tag.ToString()];
                try { Process.Start("explorer.exe", @"/select, " + HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path))); }
                catch { }
            }
        }
        // DRAG AND DROP
        private void CheckDrop(DragEventArgs e)
        {
            if (profileManager.IsSaving)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (!itemsDrag)
                {
                    if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
                    {
                        Point target = managedListView1.PointToClient(new Point(e.X, e.Y));
                        int targetItemIndex = managedListView1.GetItemIndexAtPoint(target);
                        if (targetItemIndex >= 0)
                        {
                            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                            if (!console.ParentAndChildrenMode)
                            {
                                e.Effect = DragDropEffects.Move;
                            }
                            else
                            {
                                // We can drag and drop parents only
                                Rom targetRom = profileManager.Profile.Roms[managedListView1.Items[targetItemIndex].Tag.ToString()];
                                if (targetRom.IsParentRom)
                                {
                                    if (oneOfDraggedIsParent)
                                        e.Effect = DragDropEffects.Move;
                                    else
                                        e.Effect = DragDropEffects.None;
                                }
                                else
                                {
                                    e.Effect = DragDropEffects.None;
                                }
                            }
                        }
                        else
                            e.Effect = DragDropEffects.None;
                    }
                    else if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup ||
                             profileManager.Profile.RecentSelectedType == SelectionType.Playlist)
                    {
                        Point target = managedListView1.PointToClient(new Point(e.X, e.Y));
                        int targetItemIndex = managedListView1.GetItemIndexAtPoint(target);
                        if (targetItemIndex >= 0)
                            e.Effect = DragDropEffects.Move;
                        else
                            e.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void managedListView1_DragDrop(object sender, DragEventArgs e)
        {
            if (profileManager.IsSaving)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (!itemsDrag)
                {
                    // We can add roms to consoles only
                    if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
                    {
                        // Add roms !
                        List<string> files = new List<string>();
                        string[] droppedfiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                        bool askBeforeSubfolder = true;
                        bool addSubFolders = false;
                        foreach (string f in droppedfiles)
                        {
                            if (Directory.Exists(f))
                            {
                                if (askBeforeSubfolder)
                                {
                                    ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                                       ls["Message_DoYouWantToAddAllRomsInSubFolders"] + "\n\n" + f,
                                       ls["MessageCaption_AddRoms"], true, false,
                                       ls["CheckBox_UseMyAnswerForTheRestOfDroppedFolders"]);
                                    if (res.ClickedButtonIndex == 0) // yes
                                    {
                                        addSubFolders = true;
                                    }
                                    askBeforeSubfolder = !res.Checked;
                                }
                                if (addSubFolders)
                                    files.AddRange(Directory.GetFiles(f, "*", SearchOption.AllDirectories));
                                else
                                    files.AddRange(Directory.GetFiles(f, "*", SearchOption.TopDirectoryOnly));
                            }
                            else
                            {
                                files.Add(f);
                            }
                        }
                        Form_AddRoms frm = new Form_AddRoms(profileManager.Profile.SelectedConsoleID, files.ToArray());
                        frm.ShowDialog(this);
                    }
                }
                else
                {
                    // Get the target item index
                    Point target = managedListView1.PointToClient(new Point(e.X, e.Y));
                    int targetItemIndex = managedListView1.GetItemIndexAtPoint(target);
                    if (targetItemIndex >= 0)
                    {
                        if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup ||
                            profileManager.Profile.RecentSelectedType == SelectionType.Console)
                        {
                            // Get the rom original index within the roms collection
                            int originalTargetIndex = profileManager.Profile.Roms[managedListView1.Items[targetItemIndex].Tag.ToString()].IndexWithinConsole;
                            // Move the items
                            foreach (string mid in draggedItemIDS)
                            {
                                profileManager.Profile.Roms[mid].IndexWithinConsole = originalTargetIndex;
                                profileManager.Profile.Roms[mid].Modified = true;
                                originalTargetIndex++;
                            }
                            // Refresh
                            RefreshParentSelection(false, null);
                            managedListView1.ScrollToItem(targetItemIndex);
                        }
                        else// Playlist reorder
                        {
                            Playlist pl = (Playlist)element;
                            if (pl != null)
                            {
                                // Get the rom original index within the roms collection
                                int originalTargetIndex = pl.RomIDS.IndexOf(managedListView1.Items[targetItemIndex].Tag.ToString());
                                // Move the items
                                int i = originalTargetIndex;
                                foreach (string mid in draggedItemIDS)
                                {
                                    int romIndex = pl.RomIDS.IndexOf(mid);
                                    pl.MoveRom(romIndex, i);
                                    i++;
                                }
                                // Refresh
                                RefreshParentSelection(false, null);
                                managedListView1.ScrollToItem(targetItemIndex);

                                profileManager.Profile.OnPlaylistRomsReorder(pl.Name);
                            }
                        }
                    }
                }
            }
        }
        private void managedListView1_DragEnter(object sender, DragEventArgs e)
        {
            inListDrop = true;
            CheckDrop(e);
        }
        private void managedListView1_DragOver(object sender, DragEventArgs e)
        {
            inListDrop = true;
            CheckDrop(e);
        }
        private void managedListView1_DragLeave(object sender, EventArgs e)
        {
            inListDrop = false;
        }
        private void managedListView1_ItemsDrag(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;

            if (managedListView1.SelectedItems.Count == 0)
                return;
            itemsDrag = true;
            oneOfDraggedIsParent = false;
            //get items
            List<string> dragedItems = new List<string>();
            draggedItemIDS = new List<string>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                dragedItems.Add(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)));
                draggedItemIDS.Add(rom.ID);
                if (rom.IsParentRom && !oneOfDraggedIsParent)
                    oneOfDraggedIsParent = true;
            }
            DragDropEffects eff = DoDragDrop(new DataObject(DataFormats.FileDrop, dragedItems.ToArray()),
                DragDropEffects.Copy | DragDropEffects.Move);

            if (!inListDrop)
            {
                object str = settings.GetValue("RomsBrowser:ClearRemovedRomsAfterDragAndDrop", true, true);

                if ((bool)str)
                {
                    // If it was a files move, delete the roms. 
                    OnProgressStarted();
                    Trace.WriteLine("Clearing roms progress started " + DateTime.Now.ToLocalTime(), "Roms browser");
                    int i = 0;
                    oldP = -1;
                    int max = managedListView1.SelectedItems.Count;
                    foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                    {
                        Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                        if (!File.Exists(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path))))
                        {
                            Trace.WriteLine("Removing rom: [" + rom.ID + "] " + rom.Name, "Roms browser");
                            profileManager.Profile.Roms.Remove(rom.ID, false);
                            managedListView1.Items.Remove(item);
                            i++;
                            int x = (i * 100) / max;
                            if (i != oldP)
                            {
                                OnProgress(ls["Status_Deleting"] + "...", x);
                                oldP = i;
                            }
                        }
                    }
                    OnProgressFinished();
                    profileManager.Profile.OnRomsRemoved(max);
                }
            }
            itemsDrag = false; inListDrop = false;
        }
        // Sort using column click
        private void managedListView1_ColumnClicked(object sender, ManagedListViewColumnClickArgs e)
        {
            if (profileManager.IsSaving)
                return;
            //get column and detect sort information
            ManagedListViewColumn column = managedListView1.Columns.GetColumnByID(e.ColumnID);
            if (column == null) return;
            bool az = false;
            switch (column.SortMode)
            {
                case ManagedListViewSortMode.AtoZ: az = false; break;
                case ManagedListViewSortMode.None:
                case ManagedListViewSortMode.ZtoA: az = true; break;
            }
            foreach (ManagedListViewColumn cl in managedListView1.Columns)
                cl.SortMode = ManagedListViewSortMode.None;
            // Get roms for selected element
            Rom[] roms = null;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        EmulatorsOrganizer.Core.Console pConsole = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        if (pConsole.ParentAndChildrenMode)
                        {
                            // Get parents only
                            roms = profileManager.Profile.Roms[pConsole.ID, false, false, true];
                            RomsCollection allCollection = new RomsCollection(null, false, roms);
                            // Delete the parent roms from the roms collection
                            profileManager.Profile.Roms.Remove(roms, false);
                            // Sort !
                            allCollection.Sort(new RomComparer(az, column.ID));

                            // Get the children
                            roms = profileManager.Profile.Roms.GetChildrenRoms(pConsole.ID);
                            RomsCollection childrenCollection = new RomsCollection(null, false, roms);
                            // Remove the children from the original collection
                            profileManager.Profile.Roms.Remove(roms, false);
                            // Sort children as well
                            childrenCollection.Sort(new RomComparer(az, column.ID));
                            // Add sorted children to parent sorted collection
                            allCollection.AddRange(childrenCollection);

                            // Get nor parents or children
                            roms = profileManager.Profile.Roms.GetSingleRoms(pConsole.ID);
                            RomsCollection singlesCollection = new RomsCollection(null, false, roms);
                            // Remove the singles from the original collection
                            profileManager.Profile.Roms.Remove(roms, false);
                            // Sort singles as well
                            singlesCollection.Sort(new RomComparer(az, column.ID));
                            // Add sorted singles to parent sorted collection
                            allCollection.AddRange(singlesCollection);

                            // Apply the new indexes for the console 
                            for (int i = 0; i < allCollection.Count; i++)
                            {
                                allCollection[i].IndexWithinConsole = i;
                                allCollection[i].Modified = true;
                            }
                            // Re-Add all collection (sorted parents, children and singles)
                            profileManager.Profile.Roms.AddRange(allCollection);
                        }
                        else// Normal sort
                        {
                            roms = profileManager.Profile.Roms[profileManager.Profile.SelectedConsoleID, false];
                            RomsCollection collection = new RomsCollection(null, false, roms);
                            // Delete the roms from the roms collection
                            profileManager.Profile.Roms.Remove(roms, false);
                            // Sort !
                            collection.Sort(new RomComparer(az, column.ID));
                            // Apply indexes
                            for (int i = 0; i < collection.Count; i++)
                            {
                                collection[i].IndexWithinConsole = i;
                                collection[i].Modified = true;
                            }
                            // Re-Add
                            profileManager.Profile.Roms.AddRange(collection);
                        }
                        // Refresh !
                        RefreshParentSelection(false, null);
                        if (az)
                            managedListView1.Columns.GetColumnByID(e.ColumnID).SortMode = ManagedListViewSortMode.AtoZ;
                        else
                            managedListView1.Columns.GetColumnByID(e.ColumnID).SortMode = ManagedListViewSortMode.ZtoA;
                        profileManager.Profile.OnRomsSort();
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        Playlist pl = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        roms = profileManager.Profile.Roms[pl.RomIDS.ToArray()];
                        // Sort the roms
                        RomsCollection collection = new RomsCollection(null, false, roms);
                        // Sort !
                        collection.Sort(new RomComparer(az, column.ID));
                        // Add the new order to the playlist
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].RomIDS = new List<string>();
                        foreach (Rom r in roms)
                            profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].RomIDS.Add(r.ID);
                        // Refresh !
                        RefreshParentSelection(false, null);
                        if (az)
                            managedListView1.Columns.GetColumnByID(e.ColumnID).SortMode = ManagedListViewSortMode.AtoZ;
                        else
                            managedListView1.Columns.GetColumnByID(e.ColumnID).SortMode = ManagedListViewSortMode.ZtoA;
                        profileManager.Profile.OnPlaylistRomsReorder(element.Name);
                        return;
                    }
                case SelectionType.ConsolesGroup:
                case SelectionType.PlaylistsGroup:
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_SortInGroupsIsForbidden"]);
                        return;
                    }
                case SelectionType.None: return;
            }
        }
        private void sortToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            managedListView1_ColumnClicked(this, new ManagedListViewColumnClickArgs((string)e.ClickedItem.Tag));
        }
        private void managedListView1_ViewModeChanged(object sender, EventArgs e)
        {
            resetThumbnailIndexToolStripMenuItem.Visible = showNextThumbnailToolStripMenuItem.Visible =
                panel_thumbnails.Visible = managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails;
            settings.AddValue("RomsBrowser:IsThumbnailViewMode", managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails);
            if (managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails)
            {
                timer_thumbCycle.Start();
            }
            else
            {
                timer_thumbCycle.Stop();
            }
        }
        private void trackBar_thumbSize_Scroll(object sender, EventArgs e)
        {
            settings.AddValue("RomsBrowser:ThumbnailViewSize", trackBar_thumbSize.Value);
            managedListView1.ThunmbnailsHeight = managedListView1.ThunmbnailsWidth = trackBar_thumbSize.Value;
            /*// Clear
            ThumbnailsCache = new List<Image>();
            ThumbnailsCacheIDS = new List<string>();
            if (roms_thumbs == null)
                roms_thumbs = new Dictionary<string, RomThumbnailInfo>();
            roms_thumbs.Clear();
            
            // Refresh all items to get thumb info
            if (managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails)
            {
                if (current_roms != null)
                {
                    foreach (Rom rom in current_roms)
                    {
                        FillRomThumbInfos(rom.ID);
                    }
                }
            }*/
            managedListView1.Invalidate();
        }
        private void resetThumbnailIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 1)
            {
                RomThumbnailInfo thumbIf = roms_thumbs[managedListView1.SelectedItems[0].Tag.ToString()];
                if (thumbIf != null)
                {
                    thumbIf.ThumbFileIndex = 0;
                    managedListView1.Invalidate();
                }
            }
        }
        private void comboBox_thumbsMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_thumbsMode.SelectedIndex >= 0)
                currentSelectedThumbMode = (ThumbnailsMode)comboBox_thumbsMode.SelectedItem;
            else
                currentSelectedThumbMode = null;
            // Save !
            if (currentSelectedThumbMode != null)
            {
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.Console:
                        {
                            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                            if (console == null) return;
                            console.ThumbModeSelectionID = currentSelectedThumbMode.ICID;
                            break;
                        }
                }
            }
            // Refresh all items to get thumb info
            if (managedListView1.ViewMode == ManagedListViewViewMode.Thumbnails)
            {
                if (current_roms != null)
                {
                    foreach (Rom rom in current_roms)
                    {
                        FillRomThumbInfos(rom.ID);
                    }
                }
            }
            //for (int i = 0; i < managedListView1.Items.Count; i++)
            //{
            //    FillRomItemSubitems(profileManager.Profile.Roms[managedListView1.Items[i].Tag.ToString()],
            //        managedListView1.Items[i], managedListView1.Items[i].Color);
            //}
            managedListView1.Invalidate();
        }
        private void getNameAndDataFromMobyGamescomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRomDataFromMobyGames();
        }

        private void clearRelatedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectRomsFirstToCleareRelatedFiles"],
                    ls["MessageCaption_ClearRelatedFiles"]);
                return;
            }
            List<string> ids = new List<string>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                ids.Add(item.Tag.ToString());
            }
            Form_ClearRelatedFiles frm = new Form_ClearRelatedFiles(ids.ToArray());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                profileManager.Profile.OnInformationContainerItemsModified("<multible>");
            }
        }
        private void applyRomNamesToFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyNameToRoms();
        }
        // Reset play times count
        private void playTimesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectRomsFirst"]);
                return;
            }
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                profileManager.Profile.Roms[rom.ID].PlayedTimes = 0;
            }
            profileManager.Profile.OnRomMultiplePropertiesChanged();
            // Select
            selectionTimer = selectionTimerReload;
            timer_selection.Start();
        }
        // Reset play time
        private void playTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectRomsFirst"]);
                return;
            }
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                profileManager.Profile.Roms[rom.ID].PlayedTimeLength = 0;
            }
            profileManager.Profile.OnRomMultiplePropertiesChanged();
            // Select
            selectionTimer = selectionTimerReload;
            timer_selection.Start();
        }
        // Reset last played time
        private void lastPlayedTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectRomsFirst"]);
                return;
            }
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                profileManager.Profile.Roms[rom.ID].LastPlayed = DateTime.MinValue;
            }
            profileManager.Profile.OnRomMultiplePropertiesChanged();
            // Select
            selectionTimer = selectionTimerReload;
            timer_selection.Start();
        }
        // Reset all counters
        private void allCountersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectRomsFirst"]);
                return;
            }
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                profileManager.Profile.Roms[rom.ID].PlayedTimes = 0;
                profileManager.Profile.Roms[rom.ID].LastPlayed = DateTime.MinValue;
                profileManager.Profile.Roms[rom.ID].PlayedTimeLength = 0;
            }
            profileManager.Profile.OnRomMultiplePropertiesChanged();
            // Select
            selectionTimer = selectionTimerReload;
            timer_selection.Start();
        }
        // Send to
        private void sendToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectRomsFirst"],
                    ls["MessageCaption_SendTo"]);
                return;
            }
            List<Rom> roms = new List<Rom>();
            foreach (ManagedListViewItem item in managedListView1.SelectedItems)
            {
                roms.Add(profileManager.Profile.Roms[item.Tag.ToString()]);
            }
            Form_SendTo frm = new Form_SendTo(roms.ToArray());
            frm.ShowDialog(this);
        }
        private void managedListView1_EnterPressed(object sender, EventArgs e)
        {
            try
            {
                profileManager.Profile.PlayRom();
            }
            catch (Exception ex)
            {
                ManagedMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        private void getDataFromTheGamesDBForThisGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GetRomDataFromTheGamesDB();
        }
        private void managedListView1_FillSubitemsRequest(object sender, ManagedListViewItemSelectArgs e)
        {
            FillRomItemSubitems(managedListView1.Items[e.ItemIndex].Tag.ToString(), e.ItemIndex);
        }
        private void managedListView1_ShowThumbnailInfoRequest(object sender, ManagedListViewShowThumbnailTooltipArgs e)
        {
            if (thumb_info_show_all_info || thumb_info_show_rating)
            {
                // Get rom 
                Rom rom = profileManager.Profile.Roms[managedListView1.Items[e.ItemsIndex].Tag.ToString()];
                if (!thumb_info_show_all_info)
                {
                    // Show rom name ...
                    e.TextToShow = rom.Name;
                }
                else
                {
                    EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];

                    e.TextToShow = string.Format("{0}\n{1}\n{2}", "Name: " + rom.Name, "Parent Console: " + console.Name, "Path: " + rom.Path);
                }
                if (thumb_info_show_rating)
                    e.Rating = rom.Rating;
            }
        }
        private void addDataInfoAsCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the data infos
            EmulatorsOrganizer.Core.Console cons = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
            List<string> infos = new List<string>();
            List<string> ids = new List<string>();
            Form_SelectRomDataInfo frm1 = new Form_SelectRomDataInfo(cons.RomDataInfoElements.ToArray());
            if (frm1.ShowDialog() == DialogResult.OK)
            {
                foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                {
                    Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                    ids.Add(item.Tag.ToString());

                    foreach (RomData romData in frm1.SelectedElements)
                    {
                        if (rom.IsDataItemExist(romData.ID))
                        {
                            object val = rom.GetDataItemValue(romData.ID);
                            if (val != null)
                            {
                                if (val.ToString() != "")
                                {
                                    string theCat = romData.Name + "/" + val.ToString();
                                    // if (!infos.Contains(theCat))
                                    //    infos.Add(theCat);
                                    if (!rom.Categories.Contains(theCat))
                                        rom.Categories.Add(theCat);
                                }
                            }
                        }
                    }
                    rom.Modified = true;
                    profileManager.Profile.OnRomShowed(rom);
                }
                profileManager.Profile.OnRomMultiplePropertiesChanged();
            }
            /*RomPropertiesMultibleEdit frm = new RomPropertiesMultibleEdit(ids.ToArray(), infos.ToArray());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                profileManager.Profile.OnRequestCategoriesListClear();
                // Update categories
                foreach (ManagedListViewItem item in managedListView1.Items)
                {
                    Rom rom = profileManager.Profile.Roms[item.Tag.ToString()];
                    profileManager.Profile.OnRomShowed(rom);
                }
            }*/
        }
        private void exportNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportNames();
        }
    }
}
