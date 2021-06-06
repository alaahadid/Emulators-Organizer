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
using MMB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace EmulatorsOrganizer.GUI
{
    public partial class FiltersBrowser : IBrowserControl
    {
        public FiltersBrowser()
        {
            InitializeComponent();
            base.InitializeEvents();
            //  InitializeEvents();
        }
        private string oName;
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                toolStrip1.BackColor = optimizedTreeview1.BackColor = base.BackColor = value;
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
                optimizedTreeview1.BackgroundImage = base.BackgroundImage = value;
            }
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_FiltersBrowser;
            this.BackgroundImage = style.image_FiltersBrowser;
            optimizedTreeview1.ForeColor = style.txtColor_FiltersBrowser;
            switch (style.imageMode_FiltersBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.Normal; break;
                case BackgroundImageMode.StretchIfLarger: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.StretchIfLarger; break;
                case BackgroundImageMode.StretchToFit: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.StretchToFit; break;
            }
            // Font
            try
            {
                FontConverter conv = new FontConverter();
                optimizedTreeview1.Font = (Font)conv.ConvertFromString(style.font_FiltersBrowser);
            }
            catch { }
        }
        public override bool CanDelete
        {
            get
            {
                return optimizedTreeview1.SelectedNode != null;
            }
        }
        public override bool CanRename
        {
            get
            {
                return optimizedTreeview1.SelectedNode != null;
            }
        }
        public override bool CanShowProperties
        {
            get
            {
                return optimizedTreeview1.SelectedNode != null;
            }
        }
        public override void InitializeEvents()
        {
            profileManager.Profile.ConsoleSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ProfileCleanUpFinished += Profile_ProfileCleanUpFinished;
            profileManager.Profile.DatabaseImported += Profile_DatabaseImported;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.ConsoleSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ProfileCleanUpFinished -= Profile_ProfileCleanUpFinished;
            profileManager.Profile.DatabaseImported -= Profile_DatabaseImported;
        }
        protected override void OnProfileOpened()
        {
            base.OnProfileOpened();
            RefreshFiltersFromSelection();
        }
        protected override void OnNewProfileCreated()
        {
            base.OnNewProfileCreated();
            RefreshFiltersFromSelection();
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            // When saving a profile, disable editing and moving
            toolStrip1.Enabled = false;
            contextMenuStrip1.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            toolStrip1.Enabled = true;
            contextMenuStrip1.Enabled = true;
        }

        private void Profile_ConsoleSelectionChanged(object sender, EventArgs e)
        {
            profileManager.Profile.ActiveFilters = new List<Filter>();
            RefreshFiltersFromSelection();
        }
        private void Profile_DatabaseImported(object sender, EventArgs e)
        {
            profileManager.Profile.ActiveFilters = new List<Filter>();
            RefreshFiltersFromSelection();
        }
        private void Profile_ProfileCleanUpFinished(object sender, EventArgs e)
        {
            profileManager.Profile.ActiveFilters = new List<Filter>();
            RefreshFiltersFromSelection();
        }
        public void AddFilter()
        {
            oName = "";
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.None:
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_YouMustSelectElementToAddFilter"],
                           ls["MessageCaption_AddFilter"]);
                        OnEnableDisableButtons();
                        return;
                    }
            }
            // Show new filter window ...
            Form_AddFilter frm = new Form_AddFilter();
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console: frm.OkPressed += frm_OkPressedConsoles; break;
                case SelectionType.ConsolesGroup: frm.OkPressed += frm_OkPressedConsoleGroups; break;
                case SelectionType.Playlist: frm.OkPressed += frm_OkPressedPlaylists; break;
                case SelectionType.PlaylistsGroup: frm.OkPressed += frm_OkPressedPlaylistGroups; break;
            }

            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Add it !
                Filter newFilter = new Filter();
                newFilter.Name = frm.FilterName;
                newFilter.Parameters = frm.FilterParameters.Clone();
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.Console: profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Filters.Add(newFilter); break;
                    case SelectionType.ConsolesGroup: profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Filters.Add(newFilter); break;
                    case SelectionType.Playlist: profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Filters.Add(newFilter); break;
                    case SelectionType.PlaylistsGroup: profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Filters.Add(newFilter); break;
                }

                profileManager.Profile.OnFilterAdded();
                RefreshFiltersFromSelection();
            }
            OnEnableDisableButtons();
        }
        private void frm_OkPressedConsoles(object sender, AddFilterArgs e)
        {
            // Search through console filter, see if the added name is duplicated ...
            Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
            if (console.Filters == null)
                console.Filters = new List<Filter>();
            foreach (Filter f in console.Filters)
            {
                if (f.Name.ToLower() == ((Form_AddFilter)sender).FilterName.ToLower() && f.Name.ToLower() != oName.ToLower())
                {
                    e.Cancel = true;
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisFilterNameAlreadyTaken"],
                          ls["MessageCaption_AddFilter"]);
                    return;
                }
            }
        }
        private void frm_OkPressedConsoleGroups(object sender, AddFilterArgs e)
        {
            // Search through console filter, see if the added name is duplicated ...
            ConsolesGroup gr = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
            if (gr.Filters == null)
                gr.Filters = new List<Filter>();
            foreach (Filter f in gr.Filters)
            {
                if (f.Name.ToLower() == ((Form_AddFilter)sender).FilterName.ToLower() && f.Name.ToLower() != oName.ToLower())
                {
                    e.Cancel = true;
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisFilterNameAlreadyTaken"],
                          ls["MessageCaption_AddFilter"]);
                    return;
                }
            }
        }
        private void frm_OkPressedPlaylists(object sender, AddFilterArgs e)
        {
            // Search through console filter, see if the added name is duplicated ...
            Playlist pl = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
            if (pl.Filters == null)
                pl.Filters = new List<Filter>();
            foreach (Filter f in pl.Filters)
            {
                if (f.Name.ToLower() == ((Form_AddFilter)sender).FilterName.ToLower() && f.Name.ToLower() != oName.ToLower())
                {
                    e.Cancel = true;
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisFilterNameAlreadyTaken"],
                          ls["MessageCaption_AddFilter"]);
                    return;
                }
            }
        }
        private void frm_OkPressedPlaylistGroups(object sender, AddFilterArgs e)
        {
            // Search through console filter, see if the added name is duplicated ...
            PlaylistsGroup plg = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
            if (plg.Filters == null)
                plg.Filters = new List<Filter>();
            foreach (Filter f in plg.Filters)
            {
                if (f.Name.ToLower() == ((Form_AddFilter)sender).FilterName.ToLower() && f.Name.ToLower() != oName.ToLower())
                {
                    e.Cancel = true;
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisFilterNameAlreadyTaken"],
                          ls["MessageCaption_AddFilter"]);
                    return;
                }
            }
        }

        public override void DeleteSelected()
        {
            if (optimizedTreeview1.SelectedNode == null) return;
            IEOElement element = null;
            switch (profileManager.Profile.RecentSelectedType)
            {
                default:
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_YouMustSelectElementToDeleteFilter"],
                           ls["MessageCaption_AddFilter"]);
                        OnEnableDisableButtons();
                        return;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        element = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        break;
                    }
                case SelectionType.Console:
                    {
                        element = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        element = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        element = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        break;
                    }
            }
            if (element.Filters == null)
                element.Filters = new List<Filter>();
            if (element.Filters.Count > 0)
            {
                ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeleteSelectedFilter"],
                    ls["MessageCaption_DeleteFilter"]);
                if (result.ClickedButtonIndex == 0)
                {
                    for (int i = 0; i < element.Filters.Count; i++)
                    {
                        if (element.Filters[i].Name == ((Filter)optimizedTreeview1.SelectedNode.Tag).Name)
                        {
                            element.Filters.RemoveAt(i);

                            profileManager.Profile.OnFilterRemoved();
                            RefreshFiltersFromSelection();
                            break;
                        }
                    }
                }
            }
        }
        public override void RenameSelected()
        {
            base.RenameSelected();
            ShowItemProperties();
        }
        public override void ShowItemProperties()
        {
            if (optimizedTreeview1.SelectedNode == null) return;
            oName = ((Filter)optimizedTreeview1.SelectedNode.Tag).Name;

            Form_AddFilter frm = new Form_AddFilter(((Filter)optimizedTreeview1.SelectedNode.Tag));
            IEOElement element = null;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console: frm.OkPressed += frm_OkPressedConsoles; element = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID]; break;
                case SelectionType.ConsolesGroup: frm.OkPressed += frm_OkPressedConsoleGroups; element = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID]; break;
                case SelectionType.Playlist: frm.OkPressed += frm_OkPressedPlaylists; element = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID]; break;
                case SelectionType.PlaylistsGroup: frm.OkPressed += frm_OkPressedPlaylistGroups; element = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID]; break;
            }

            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                foreach (Filter f in element.Filters)
                {
                    if (f.Name == oName)
                    {
                        f.Name = frm.FilterName;
                        f.Parameters = frm.FilterParameters.Clone();

                        profileManager.Profile.OnFilterEdit();
                        RefreshFiltersFromSelection();
                        break;
                    }
                }
            }
            OnEnableDisableButtons();
        }
        private void RefreshFiltersFromSelection()
        {
            optimizedTreeview1.Nodes.Clear();
            IEOElement element = null;
            // See element ...
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.None: return;
                case SelectionType.ConsolesGroup:
                    {
                        element = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        break;
                    }
                case SelectionType.Console:
                    {
                        element = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        element = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        element = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        break;
                    }
            }
            if (element.Filters == null)
                element.Filters = new List<Filter>();

            foreach (Filter f in element.Filters)
            {
                TreeNode tr = new TreeNode();
                string fm = "";
                if (f.Parameters.IsDataItem)
                {
                    fm = f.Parameters.DataItemName;
                }
                else
                {
                    fm = f.Parameters.SearchMode.ToString();
                }
                tr.Text = f.Name + " [" + f.Parameters.SearchWhat + " " + ls["Word_By"] + " " + fm + "]";
                tr.Tag = f;
                optimizedTreeview1.Nodes.Add(tr);
            }
        }

        private void AddNewFilter(object sender, EventArgs e)
        {
            AddFilter();
        }
        private void RemoveSelectedFilter(object sender, EventArgs e)
        {
            DeleteSelected();
        }
        private void EditSelectedFilter(object sender, EventArgs e)
        {
            ShowItemProperties();
        }
        private void optimizedTreeview1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // See element ...
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.None: return;
                default:
                    {
                        profileManager.Profile.ActiveFilters = new List<Filter>();
                        foreach (TreeNode tr in optimizedTreeview1.Nodes)
                        {
                            if (tr.Checked)
                            {
                                profileManager.Profile.ActiveFilters.Add((Filter)tr.Tag);
                            }
                        }
                        profileManager.Profile.OnRomsRefreshRequest();
                        break;
                    }
            }
        }
        private void optimizedTreeview1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            toolStripButton_delete.Enabled = toolStripButton_edit.Enabled = optimizedTreeview1.SelectedNode != null;
            OnEnableDisableButtons();
        }
        private void AllMatch_CheckedChanged(object sender, EventArgs e)
        {
            profileManager.Profile.IsAllFilterMatch = AllMatch.Checked;
            optimizedTreeview1_AfterCheck(null, null);
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            optimizedTreeview1_AfterCheck(null, null);
        }
    }
}
