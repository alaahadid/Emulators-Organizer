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
using MMB;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
namespace EmulatorsOrganizer.GUI
{
    public partial class PlaylistsBrowser : IBrowserControl
    {
        public PlaylistsBrowser()
            : base()
        {
            InitializeComponent();
            base.InitializeEvents();
            //   InitializeEvents();
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                treeView1.BackColor = toolStrip1.BackColor = base.BackColor = value;
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
                treeView1.BackgroundImage = base.BackgroundImage = value;
            }
        }
        private bool isRefreshing = false;
        private int currentlySelectedNodeIndex;
        private int currentlySelectedNodeParentIndex;
        private bool playlistGroupsAtoZ = false;
        private bool playlistsAtoZ = false;
        private bool canRename = false;
        private bool canDoDragDrop = false;

        public override void InitializeEvents()
        {
            profileManager.Profile.PlaylistsGroupAdded += Profile_PlaylistsGroupAdded;
            profileManager.Profile.PlaylistsGroupRemoved += Profile_PlaylistsGroupRemoved;
            profileManager.Profile.PlaylistGroupsCleared += Profile_PlaylistsGroupsCleared;
            profileManager.Profile.PlaylistsGroupMoved += Profile_PlaylistsGroupsMoved;
            profileManager.Profile.PlaylistAdded += Profile_PlaylistAdded;
            profileManager.Profile.PlaylistRemoved += Profile_PlaylistRemoved;
            profileManager.Profile.PlaylistsCleared += Profile_PlaylistsCleared;
            profileManager.Profile.PlaylistMoved += Profile_PlaylistMoved;
            profileManager.Profile.PlaylistGroupsSorted += Profile_PlaylistsGroupsSorted;
            profileManager.Profile.PlaylistsSorted += Profile_PlaylistsSorted;
            profileManager.Profile.PlaylistPropertiesChanged += Profile_PlaylistPropertiesChanged;
            profileManager.Profile.PlaylistsGroupPropertiesChanged += Profile_PlaylistPropertiesChanged;
            profileManager.Profile.ProfileCleanUpFinished += Profile_ProfileCleanUpFinished;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.PlaylistsGroupAdded -= Profile_PlaylistsGroupAdded;
            profileManager.Profile.PlaylistsGroupRemoved -= Profile_PlaylistsGroupRemoved;
            profileManager.Profile.PlaylistGroupsCleared -= Profile_PlaylistsGroupsCleared;
            profileManager.Profile.PlaylistsGroupMoved -= Profile_PlaylistsGroupsMoved;
            profileManager.Profile.PlaylistAdded -= Profile_PlaylistAdded;
            profileManager.Profile.PlaylistRemoved -= Profile_PlaylistRemoved;
            profileManager.Profile.PlaylistsCleared -= Profile_PlaylistsCleared;
            profileManager.Profile.PlaylistMoved -= Profile_PlaylistMoved;
            profileManager.Profile.PlaylistGroupsSorted -= Profile_PlaylistsGroupsSorted;
            profileManager.Profile.PlaylistsSorted -= Profile_PlaylistsSorted;
            profileManager.Profile.PlaylistPropertiesChanged -= Profile_PlaylistPropertiesChanged;
            profileManager.Profile.PlaylistsGroupPropertiesChanged -= Profile_PlaylistPropertiesChanged;
            profileManager.Profile.ProfileCleanUpFinished -= Profile_ProfileCleanUpFinished;
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_PlaylistsBrowser;
            this.BackgroundImage = style.image_PlaylistsBrowser;
            treeView1.ForeColor = style.txtColor_PlaylistsBrowser;
            switch (style.imageMode_PlaylistsBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio: this.treeView1.BackgroundImageMode = ImageViewMode.Normal; break;
                case BackgroundImageMode.StretchIfLarger: this.treeView1.BackgroundImageMode = ImageViewMode.StretchIfLarger; break;
                case BackgroundImageMode.StretchToFit: this.treeView1.BackgroundImageMode = ImageViewMode.StretchToFit; break;
            }
            // Font
            try
            {
                FontConverter conv = new FontConverter();
                treeView1.Font = (Font)conv.ConvertFromString(style.font_PlaylistsBrowser);
            }
            catch { }
        }
        public void AddPlaylist()
        {
            string parentID = "";
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
                {
                    parentID = ((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID;
                }
                else if (treeView1.SelectedNode is TreeNodePlaylist)
                {
                    if (treeView1.SelectedNode.Parent != null)
                        parentID = ((TreeNodePlaylistsGroup)treeView1.SelectedNode.Parent).ID;
                }
            }
            profileManager.Profile.AddPlaylist(parentID);
        }
        public void AddRootPlaylist()
        {
            treeView1.SelectedNode = null;
            AddPlaylist();
        }
        public override void DeleteSelected()
        {
            if (treeView1.SelectedNode == null)
            { OnEnableDisableButtons(); return; }
            if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
            {
                TreeNodePlaylistsGroup node = (TreeNodePlaylistsGroup)treeView1.SelectedNode;
                ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeletePlaylistsGroup"],
                    ls["MessageCaption_DeletePlaylistsGroup"]);
                if (result.ClickedButtonIndex == 0)// yes
                {
                    if (profileManager.Profile.RecentSelectedType == SelectionType.PlaylistsGroup)
                    {
                        if (node.ID == profileManager.Profile.SelectedPlaylistsGroupID)
                        {
                            profileManager.Profile.RecentSelectedType = SelectionType.None;
                        }
                    }
                    profileManager.Profile.PlaylistGroups.Remove(node.ID);
                }
            }
            else if (treeView1.SelectedNode is TreeNodePlaylist)
            {
                TreeNodePlaylist node = (TreeNodePlaylist)treeView1.SelectedNode;
                ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeletePlaylist"],
                    ls["MessageCaption_DeletePlaylist"]);
                if (result.ClickedButtonIndex == 0)// yes
                {
                    if (profileManager.Profile.RecentSelectedType == SelectionType.Playlist)
                    {
                        if (node.ID == profileManager.Profile.SelectedPlaylistID)
                        {
                            profileManager.Profile.RecentSelectedType = SelectionType.None;
                        }
                    }
                    profileManager.Profile.Playlists.Remove(node.ID);
                }
            }
            OnEnableDisableButtons();
        }
        public override void RenameSelected()
        {
            if (treeView1.SelectedNode == null)
                return;
            treeView1.SelectedNode.BeginEdit();
        }
        public void RefreshPlaylistGroups()
        {
            if (isRefreshing) return;
            isRefreshing = true;
            treeView1.Nodes.Clear();
            if (profileManager.Profile != null)
            {
                foreach (PlaylistsGroup gr in profileManager.Profile.PlaylistGroups)
                {
                    TreeNodePlaylistsGroup node = new TreeNodePlaylistsGroup(gr.ID);
                    node.RefreshNodes();
                    treeView1.Nodes.Add(node);
                }
            }
            isRefreshing = false;
            RefreshPlaylists();
        }
        public void RefreshPlaylists()
        {
            if (isRefreshing) return;
            isRefreshing = true;
            // Delete playlists
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i] is TreeNodePlaylist)
                {
                    treeView1.Nodes.RemoveAt(i);
                    i--;
                }
                else if (treeView1.Nodes[i] is TreeNodePlaylistsGroup)
                {
                    ((TreeNodePlaylistsGroup)treeView1.Nodes[i]).RefreshNodes();
                }
            }
            if (profileManager.Profile != null)
            {
                // Add playlists that have no parent id
                foreach (Playlist playlist in profileManager.Profile.Playlists)
                {
                    if (playlist.ParentGroupID == "")
                    {
                        TreeNodePlaylist node = new TreeNodePlaylist(playlist.ID);
                        node.RefreshNodes();
                        treeView1.Nodes.Add(node);
                    }
                }
            }
            isRefreshing = false;
        }
        public void ActiveSelection()
        {
            if (treeView1.SelectedNode == null)
            {
                SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
                object _defaultStyle = settings.GetValue("Default Style", true, new EOStyle());
                if (_defaultStyle != null)
                    treeView1.BackgroundImage = ((EOStyle)_defaultStyle).image_PlaylistsBrowser;
                else
                    treeView1.BackgroundImage = null;
                //profileManager.Profile.OnEmulatorsRefreshRequest(new string[0]);// load none
                treeView1.Invalidate();
                return;
            }
            if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
            {
                profileManager.Profile.SelectedPlaylistsGroupID = ((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID;
                profileManager.Profile.RecentSelectedType = SelectionType.PlaylistsGroup;
                treeView1.BackgroundImage = profileManager.Profile.PlaylistGroups[((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID].Style.image_PlaylistsBrowser;
            }
            else if (treeView1.SelectedNode is TreeNodePlaylist)
            {
                profileManager.Profile.SelectedPlaylistID = ((TreeNodePlaylist)treeView1.SelectedNode).ID;
                profileManager.Profile.RecentSelectedType = SelectionType.Playlist;
                treeView1.BackgroundImage = profileManager.Profile.Playlists[((TreeNodePlaylist)treeView1.SelectedNode).ID].Style.image_PlaylistsBrowser;
            }
            profileManager.Profile.OnEmulatorsRefreshRequest(new string[0]);// load none, roms browser should take care of emulators
            treeView1.Invalidate();
            OnEnableDisableButtons();
        }
        public void MoveSelectedItemUp()
        {
            if (treeView1.SelectedNode == null)
            { OnEnableDisableButtons(); return; }
            if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
            {
                // Get current playlists group index
                int index = profileManager.Profile.PlaylistGroups.IndexOf(((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID);
                int newIndex = index - 1;
                if (newIndex < 0)
                    return;
                currentlySelectedNodeIndex = newIndex;
                profileManager.Profile.PlaylistGroups.Move(index, newIndex);
            }
            else if (treeView1.SelectedNode is TreeNodePlaylist)
            {
                // Get parent Playlists group id
                string parentID = "";
                if (treeView1.SelectedNode.Parent != null)
                    parentID = ((TreeNodePlaylistsGroup)treeView1.SelectedNode.Parent).ID;
                // Get all playlists that belong to this parent
                PlaylistsCollection collection = new PlaylistsCollection(null, profileManager.Profile.Playlists[parentID, false]);
                // Get index of current item
                int tempIndex = collection.IndexOf(((TreeNodePlaylist)treeView1.SelectedNode).ID);
                int index = profileManager.Profile.Playlists.IndexOf(collection[((TreeNodePlaylist)treeView1.SelectedNode).ID]);
                tempIndex = tempIndex - 1;
                if (tempIndex < 0) return;
                int newIndex = profileManager.Profile.Playlists.IndexOf(collection[tempIndex].ID);// index of previous item
                if (newIndex < 0) return;
                if (newIndex >= profileManager.Profile.Playlists.Count) return;
                if (treeView1.SelectedNode.Parent != null)
                {
                    currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode.Parent);
                    currentlySelectedNodeIndex = treeView1.SelectedNode.Parent.Nodes.IndexOf(treeView1.SelectedNode) - 1;
                }
                else
                {
                    currentlySelectedNodeParentIndex = -1;
                    currentlySelectedNodeIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode) - 1;
                }
                // Move the playlist within collection
                profileManager.Profile.Playlists.Move(index, newIndex);
            }
            OnEnableDisableButtons();
        }
        public void MoveSelectedItemDown()
        {
            if (treeView1.SelectedNode == null)
            { OnEnableDisableButtons(); return; }
            if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
            {
                // Get current playlists group index
                int index = profileManager.Profile.PlaylistGroups.IndexOf(((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID);
                int newIndex = index + 1;
                if (newIndex > profileManager.Profile.PlaylistGroups.Count - 1)
                    return;
                currentlySelectedNodeIndex = newIndex;
                profileManager.Profile.PlaylistGroups.Move(index, newIndex);
            }
            else if (treeView1.SelectedNode is TreeNodePlaylist)
            {
                // Get parent playlists group id
                string parentID = "";
                if (treeView1.SelectedNode.Parent != null)
                    parentID = ((TreeNodePlaylistsGroup)treeView1.SelectedNode.Parent).ID;
                // Get all playlists that belong to this parent
                PlaylistsCollection collection = new PlaylistsCollection(null, profileManager.Profile.Playlists[parentID, false]);
                // Get index of current item
                int tempIndex = collection.IndexOf(((TreeNodePlaylist)treeView1.SelectedNode).ID);
                int index = profileManager.Profile.Playlists.IndexOf(collection[((TreeNodePlaylist)treeView1.SelectedNode).ID]);
                tempIndex = tempIndex + 1;
                if (tempIndex > collection.Count - 1) return;
                int newIndex = profileManager.Profile.Playlists.IndexOf(collection[tempIndex].ID);// index of next item
                if (newIndex > profileManager.Profile.Playlists.Count - 1) return;
                if (treeView1.SelectedNode.Parent != null)
                {
                    currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode.Parent);
                    currentlySelectedNodeIndex = treeView1.SelectedNode.Parent.Nodes.IndexOf(treeView1.SelectedNode) + 1;
                }
                else
                {
                    currentlySelectedNodeParentIndex = -1;
                    currentlySelectedNodeIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode) + 1;
                }
                // Move the playlist within collection
                profileManager.Profile.Playlists.Move(index, newIndex);
            }
            OnEnableDisableButtons();
        }
        public void SortPlaylistsGroupByName()
        {
            profileManager.Profile.PlaylistGroups.Sort(new PlaylistGroupsComparer(playlistGroupsAtoZ, PlaylistGroupsCompareType.Name));
            playlistGroupsAtoZ = !playlistGroupsAtoZ;
            OnEnableDisableButtons();
        }
        public void SortRootPlaylistsByName()
        {
            currentlySelectedNodeParentIndex = -1;
            // Get all playlists of no-parent id
            PlaylistsCollection collection = new PlaylistsCollection(null, profileManager.Profile.Playlists["", false]);
            // Remove them from original collection
            profileManager.Profile.Playlists.RemoveItems(collection);
            // Sort them
            collection.Sort(new PlaylistsComparer(playlistsAtoZ, PlaylistCompareType.Name));
            playlistsAtoZ = !playlistsAtoZ;
            // Re-add add them
            profileManager.Profile.Playlists.AddRange(collection);
            // Raise event
            profileManager.Profile.OnPlaylistsSort();
            OnEnableDisableButtons();
        }
        public void SortPlaylistsByName()
        {
            if (treeView1.SelectedNode == null)
            {
                SortRootPlaylistsByName();
            }
            else
            {
                string parentid = "";
                currentlySelectedNodeParentIndex = -1;
                if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
                {
                    currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode);
                    parentid = ((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID;
                }
                else
                {
                    if (treeView1.SelectedNode.Parent != null)
                    {
                        currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode.Parent);
                        parentid = ((TreeNodePlaylistsGroup)treeView1.SelectedNode.Parent).ID;
                    }
                }
                // Get all Playlists of no-parent id
                PlaylistsCollection collection = new PlaylistsCollection(null, profileManager.Profile.Playlists[parentid, false]);
                // Remove them from original collection
                profileManager.Profile.Playlists.RemoveItems(collection);
                // Sort them
                collection.Sort(new PlaylistsComparer(playlistsAtoZ, PlaylistCompareType.Name));
                playlistsAtoZ = !playlistsAtoZ;
                // Re-add add them
                profileManager.Profile.Playlists.AddRange(collection);
                // Raise event
                profileManager.Profile.OnPlaylistsSort();
            }
            OnEnableDisableButtons();
        }
        public override void ChangeIcon()
        {
            if (treeView1.SelectedNode == null)
                return;
            IEOElement element;
            if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
                element = profileManager.Profile.PlaylistGroups[((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID];
            else if (treeView1.SelectedNode is TreeNodePlaylist)
                element = profileManager.Profile.Playlists[((TreeNodePlaylist)treeView1.SelectedNode).ID];
            else
                return;

            OpenFileDialog Op = new OpenFileDialog();
            Op.Title = ls["Title_BrowseForAnIcon"];
            Op.Filter = ls["Filter_Icon"]; ;
            Op.Multiselect = false;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                if (Path.GetExtension(Op.FileName).ToLower() == ".exe" | Path.GetExtension(Op.FileName).ToLower() == ".ico")
                { element.Icon = Icon.ExtractAssociatedIcon(Op.FileName).ToBitmap(); }
                else
                {
                    //Stream str = new FileStream(Op.FileName, FileMode.Open, FileAccess.Read);
                    //byte[] buff = new byte[str.Length];
                    //str.Read(buff, 0, (int)str.Length);
                    //str.Dispose();
                    //str.Close();

                    //element.Icon = (Bitmap)Image.FromStream(new MemoryStream(buff));
                    using (var bmpTemp = new Bitmap(Op.FileName))
                    {
                        element.Icon = new Bitmap(bmpTemp);
                    }
                }
                profileManager.Profile.OnElementIconChanged(element);
                treeView1.Invalidate();
            }
        }
        public override void ClearIcon()
        {
            base.ClearIcon();
            if (treeView1.SelectedNode == null)
                return;
            IEOElement element;
            string name = "";
            if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
            {
                element = profileManager.Profile.PlaylistGroups[((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID];
                name = ls["Status_PlaylistsGroup"];
            }
            else if (treeView1.SelectedNode is TreeNodePlaylist)
            {
                element = profileManager.Profile.Playlists[((TreeNodePlaylist)treeView1.SelectedNode).ID];
                name = ls["Name_Playlist"];
            }
            else
                return;
            ManagedMessageBoxResult resul = ManagedMessageBox.ShowQuestionMessage(
                ls["Message_AreYouSureYouWantToClearIconsForSelectedItems"],
                ls["MessageCaption_ClearIcon"] + " " + ls["Word_for"] + " " + element.Name + " " + name);
            if (resul.ClickedButtonIndex == 0)
            {
                element.Icon = null;

                profileManager.Profile.OnElementIconChanged(element);
                treeView1.Invalidate();
            }
        }
        public override void ShowItemProperties()
        {
            if (treeView1.SelectedNode == null) return;

            currentlySelectedNodeParentIndex = -1;
            if (treeView1.SelectedNode.Parent != null)
                currentlySelectedNodeParentIndex = treeView1.SelectedNode.Parent.Index;
            currentlySelectedNodeIndex = treeView1.SelectedNode.Index;

            if (treeView1.SelectedNode is TreeNodePlaylist)
            {
                PlaylistProperties frm = new PlaylistProperties(((TreeNodePlaylist)treeView1.SelectedNode).ID);
                frm.ShowDialog();
            }
            else if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
            {
                PlaylistsGroupProperties frm = new PlaylistsGroupProperties(((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID);
                frm.ShowDialog();
            }
        }
        public override void LoadControlSettings()
        {
            base.LoadControlSettings();
            trackBar_zoom.Value = (int)settings.GetValue("PlaylistsBrowser:ZoomValue", true, 16);

            trackBar_zoom_Scroll(this, null);
        }
        public void ClonePlaylistSettings()
        {
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Playlist:
                    {
                        if (profileManager.Profile.Playlists.Count == 0)
                        {
                            ManagedMessageBox.ShowErrorMessage("There is no playlist in the profile !!");
                            return;
                        }
                        if (profileManager.Profile.Playlists.Count == 1)
                        {
                            ManagedMessageBox.ShowErrorMessage("There must be more than playlist in the profile !");
                            return;
                        }
                        Form_CloneSettings frm = new Form_CloneSettings(SettingsCopyMode.PLAYLIST);
                        frm.ShowDialog(this);
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        if (profileManager.Profile.PlaylistGroups.Count == 0)
                        {
                            ManagedMessageBox.ShowErrorMessage("There is no playlists group in the profile !!");
                            return;
                        }
                        if (profileManager.Profile.PlaylistGroups.Count == 1)
                        {
                            ManagedMessageBox.ShowErrorMessage("There must be more than one playlists group in the profile !");
                            return;
                        }
                        Form_CloneSettings frm = new Form_CloneSettings(SettingsCopyMode.PLAYLISTS_GROUP);
                        frm.ShowDialog(this);
                        break;
                    }
                case SelectionType.Console:
                case SelectionType.ConsolesGroup:
                case SelectionType.None: ManagedMessageBox.ShowErrorMessage("Please select a playlist or playlists group first."); return;
            }
        }
        public void ExportSelectedPlaylist()
        {
            if (treeView1.SelectedNode == null)
                return;

            if (!(treeView1.SelectedNode is TreeNodePlaylist))
            {
                ManagedMessageBox.ShowErrorMessage("Please select a playlist first.", "Export playlist");
                return;
            }
            // Show options
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = "Export Playlist";
            sav.Filter = "Emulators Organizer Playlist (*.eopl)|*.eopl";
            if (sav.ShowDialog(this) == DialogResult.OK)
            {
                Playlist pl = profileManager.Profile.Playlists[((TreeNodePlaylist)treeView1.SelectedNode).playlist.ID];
                try
                {
                    Trace.WriteLine("Saving playlist ..", "Profile Manager");
                    FileStream fs = new FileStream(sav.FileName, FileMode.Create, FileAccess.Write);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, pl);
                    fs.Close();
                    Trace.WriteLine("Playlist saved successfully", "Profile Manager");
                    ManagedMessageBox.ShowMessage(ls["Status_Done"], "Export playlist");
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to save playlist: " + ex.Message);
                    ManagedMessageBox.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString(), "Export playlist");
                }
            }
        }
        public void ImportPlaylistFile()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Import playlist from file";
            op.Filter = "Emulators Organizer Playlist (*.eopl)|*.eopl";
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    // Deserialize !
                    Trace.WriteLine("Opening playlist file ..", "Profile Manager");
                    FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter formatter = new BinaryFormatter();
                    Playlist pfile = (Playlist)formatter.Deserialize(fs);
                    fs.Close();
                    Trace.WriteLine("Playlist opened successfully", "Profile Manager");
                    // Add playlist, give it and id !
                    EmulatorsOrganizer.Core.Playlist newPlaylist = pfile;
                    newPlaylist.ID = profileManager.Profile.GenerateID();
                    newPlaylist.ParentGroupID = "";// No parent !
                    int i = 1;
                    string name = pfile.Name;
                    while (profileManager.Profile.Playlists.Contains(name))
                    {
                        name = pfile.Name + "_" + i;
                    }
                    pfile.Name = name;
                    // Add the playlist !
                    profileManager.Profile.Playlists.Add(newPlaylist);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to open playlist: " + ex.Message);
                    ManagedMessageBox.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString(), "Import Playlist");
                }
            }
        }

        public override bool CanDelete
        {
            get
            {
                return treeView1.SelectedNode != null;
            }
        }
        public override bool CanRename
        {
            get
            {
                return treeView1.SelectedNode != null;
            }
        }
        public override bool CanChangeIcon
        {
            get
            {
                return treeView1.SelectedNode != null;
            }
        }
        public override bool CanShowProperties
        {
            get
            {
                return (treeView1.SelectedNode != null);
            }
        }
        protected override void OnProfileOpened()
        {
            base.OnProfileOpened(); RefreshPlaylistGroups();
            if (profileManager.Profile.RememberLatestSelectedConsoleOnProfileOpen)
            {
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.PlaylistsGroup:
                        {
                            // Look for the consoles group in the nodes ...
                            foreach (TreeNode node in treeView1.Nodes)
                            {
                                if (node is TreeNodePlaylistsGroup)
                                {
                                    // Search the internal consoles

                                    if (((TreeNodePlaylistsGroup)node).group.ID == profileManager.Profile.SelectedPlaylistsGroupID)
                                    {
                                        treeView1.SelectedNode = node;
                                        node.Expand();
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case SelectionType.Playlist:
                        {
                            // Look for the console in the nodes ...
                            foreach (TreeNode node in treeView1.Nodes)
                            {
                                if (node is TreeNodePlaylist)
                                {
                                    if (((TreeNodePlaylist)node).playlist.ID == profileManager.Profile.SelectedPlaylistID)
                                    {
                                        treeView1.SelectedNode = node;
                                        node.Expand();
                                        break;
                                    }
                                }
                                else if (node is TreeNodePlaylistsGroup)
                                {
                                    // Search the internal consoles
                                    foreach (TreeNode childnode in node.Nodes)
                                    {
                                        if (childnode is TreeNodePlaylist)
                                        {
                                            if (((TreeNodePlaylist)childnode).playlist.ID == profileManager.Profile.SelectedPlaylistID)
                                            {
                                                treeView1.SelectedNode = childnode;
                                                childnode.Expand();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }

                ActiveSelection();
            }
        }
        protected override void OnNewProfileCreated()
        {
            base.OnNewProfileCreated(); RefreshPlaylistGroups();
        }
        protected override void OnEnableDisableButtons()
        {
            base.OnEnableDisableButtons();
            clearIconToolStripMenuItem.Enabled = changeIconToolStripMenuItem.Enabled = CanChangeIcon;
            deleteToolStripMenuItem.Enabled = CanDelete;
            propertiesToolStripMenuItem.Enabled = CanShowProperties;
            renameToolStripMenuItem.Enabled = CanRename;
            toolStripButton_moveDown.Enabled = toolStripButton_moveUp.Enabled = treeView1.SelectedNode != null;
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            // When saving a profile, disable editing and moving
            this.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            this.Enabled = true;
        }
        public string HighlightedPlaylistID
        {
            get
            {
                if (treeView1.SelectedNode != null)
                    if (treeView1.SelectedNode is TreeNodePlaylist)
                        return ((TreeNodePlaylist)treeView1.SelectedNode).ID;
                return "";
            }
        }

        private void PlaylistsBrowser_Leave(object sender, EventArgs e)
        {
            panel1.Visible = toolStrip1.Visible = false;
            if (!Focused)
                canDoDragDrop = false;
        }
        private void PlaylistsBrowser_MouseUp(object sender, MouseEventArgs e)
        {
            canDoDragDrop = true;
        }
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null)
                return;
            if (e.Label == "" || e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }
            if (e.Node is TreeNodePlaylistsGroup)
            {
                if (profileManager.Profile.PlaylistGroups.Contains(e.Label))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherPlaylistsGroup"],
                        ls["MessageCaption_Rename"]);
                    e.CancelEdit = true;
                    return;
                }
                profileManager.Profile.RenamePlaylistsGroup(((TreeNodePlaylistsGroup)e.Node).ID, e.Label);
            }
            else if (e.Node is TreeNodePlaylist)
            {
                string parentID = "";
                if (e.Node.Parent != null)
                    parentID = ((TreeNodePlaylistsGroup)e.Node.Parent).ID;
                if (profileManager.Profile.Playlists.Contains(e.Label, parentID, ((TreeNodePlaylist)e.Node).ID))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherPlaylist"],
                        ls["MessageCaption_Rename"]);
                    e.CancelEdit = true;
                    return;
                }
                profileManager.Profile.RenamePlaylist(((TreeNodePlaylist)e.Node).ID, e.Label);
            }
            ((EOTreeNode)e.Node).RefreshText();
        }
        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            if (!canRename) e.CancelEdit = true;
        }
        private void playlistsGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profileManager.Profile.AddPlaylistsGroup();
        }
        /*Draw*/
        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            int h = trackBar_zoom.Value;
            int w = trackBar_zoom.Value;
            Size CharsSize = TextRenderer.MeasureText(e.Node.Text, treeView1.Font);

            Image im = null;
            int XP = 16;
            if (e.Node.GetType() == typeof(TreeNodePlaylistsGroup))
            {
                if (treeView1.SelectedNode == e.Node)
                {
                    if (treeView1.Focused)
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(treeView1.ForeColor), 2),
                           e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                    else
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LightGray), 2),
                          e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                }
                if (e.Node.Nodes.Count > 0)
                {
                    if (!e.Node.IsExpanded)
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_black,
                            e.Bounds.X + 1, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_arrow_down,
                            e.Bounds.X + 1, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                }
                im = profileManager.Profile.PlaylistGroups[((TreeNodePlaylistsGroup)e.Node).ID].Icon;
                if (im == null)
                    im = Properties.Resources.folder;

                e.Graphics.DrawImage(im, e.Bounds.X + XP, e.Bounds.Y + 2, w, h);
                e.Graphics.DrawString(e.Node.Text, treeView1.Font, new SolidBrush(Color.Black),
                    new PointF(e.Bounds.X + w + 20, e.Bounds.Y + ((e.Bounds.Height / 2) - 5)));
            }

            if (e.Node.GetType() == typeof(TreeNodePlaylist))
            {
                if (profileManager.Profile.Playlists[((TreeNodePlaylist)e.Node).ID].ParentGroupID != "")
                    XP += 16;
                if (treeView1.SelectedNode == e.Node)
                {
                    if (treeView1.Focused)
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(treeView1.ForeColor), 2),
                           e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                    else
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LightGray), 2),
                          e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                }
                if (e.Node.Nodes.Count > 0)
                {
                    if (!e.Node.IsExpanded)
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_black,
                            e.Bounds.X + 16, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_arrow_down,
                            e.Bounds.X + 16, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                }
                im = profileManager.Profile.Playlists[((TreeNodePlaylist)e.Node).ID].Icon;
                if (im == null)
                    im = Properties.Resources.Favorites;

                e.Graphics.DrawImage(im, e.Bounds.X + XP, e.Bounds.Y + 2, w, h);
                e.Graphics.DrawString(e.Node.Text, treeView1.Font, new SolidBrush(Color.Black),
                    new PointF(e.Bounds.X + w + XP, e.Bounds.Y + ((e.Bounds.Height / 2) - 5)));
            }
        }
        private void trackBar_zoom_Scroll(object sender, EventArgs e)
        {
            imageList1.ImageSize = new Size(trackBar_zoom.Value, trackBar_zoom.Value);
            treeView1.Refresh();

            // Save
            settings.AddValue(new SettingsValue("PlaylistsBrowser:ZoomValue", trackBar_zoom.Value));
        }
        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.Location);

                if (treeView1.SelectedNode == null)
                { OnEnableDisableButtons(); return; }

                if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
                {
                    if (e.Location.X < 25)
                    {
                        canRename = false;
                        if (treeView1.SelectedNode.IsExpanded)
                            treeView1.SelectedNode.Collapse();
                        else
                            treeView1.SelectedNode.Expand();
                    }
                    else
                    {
                        canRename = true;
                    }
                }
                else if (treeView1.SelectedNode is TreeNodePlaylist)
                {
                    if (e.Location.X < 25 && e.Location.X > 10)
                    {
                        canRename = false;
                        if (treeView1.SelectedNode.IsExpanded)
                            treeView1.SelectedNode.Collapse();
                        else
                            treeView1.SelectedNode.Expand();
                    }
                    else
                    {
                        canRename = true;
                    }
                }
                OnEnableDisableButtons();
            }
            treeView1.Invalidate();
        }
        /*Drag and drop*/
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (canDoDragDrop)
                DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Link);
        }
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {

        }
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodePlaylistsGroup)))
            {
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);
                if (target == null)
                    e.Effect = DragDropEffects.Move;
                else if (target is TreeNodePlaylistsGroup)
                    e.Effect = DragDropEffects.Move;
                else if (target is TreeNodePlaylist)
                {
                    // get parent
                    string parentID = "";
                    if (target.Parent != null)
                        parentID = "1";
                    if (parentID == "")
                        e.Effect = DragDropEffects.Move;
                    else
                        e.Effect = DragDropEffects.None;
                }
            }
            else if (e.Data.GetDataPresent(typeof(TreeNodePlaylist)))
            {
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);

                if (target is TreeNodePlaylistsGroup)
                    e.Effect = DragDropEffects.Link;
                else
                    e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodePlaylistsGroup)))
            {
                TreeNodePlaylistsGroup original = (TreeNodePlaylistsGroup)e.Data.GetData(typeof(TreeNodePlaylistsGroup));
                PlaylistsGroup originalGR = profileManager.Profile.PlaylistGroups[original.ID];
                int originalIndex = profileManager.Profile.PlaylistGroups.IndexOf(originalGR);
                // Get target
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);

                if (target == null)
                {
                    currentlySelectedNodeIndex = profileManager.Profile.PlaylistGroups.Count - 1;
                    profileManager.Profile.PlaylistGroups.Move(originalIndex, -1);
                }
                else if (target is TreeNodePlaylist)
                {
                    if (target.Parent == null)
                    {
                        currentlySelectedNodeIndex = profileManager.Profile.PlaylistGroups.Count - 1;
                        profileManager.Profile.PlaylistGroups.Move(originalIndex, -1);
                    }
                }
                else if (target is TreeNodePlaylistsGroup)
                {
                    int targetIndex = treeView1.Nodes.IndexOf(target);
                    // Then move the group into last position in the list
                    currentlySelectedNodeIndex = targetIndex;
                    profileManager.Profile.PlaylistGroups.Move(originalIndex, targetIndex);
                }
            }
            else if (e.Data.GetDataPresent(typeof(TreeNodePlaylist)))
            {
                TreeNodePlaylist original = (TreeNodePlaylist)e.Data.GetData(typeof(TreeNodePlaylist));
                Core.Playlist originalCon = profileManager.Profile.Playlists[original.ID];
                int originalIndex = profileManager.Profile.Playlists.IndexOf(originalCon);
                // Get target
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);

                if (target == null)
                {
                    currentlySelectedNodeParentIndex = -1;
                    currentlySelectedNodeIndex = treeView1.Nodes.Count - 1;
                    originalCon.ParentGroupID = "";
                    profileManager.Profile.Playlists.Move(originalIndex, -1);
                }
                else if (target is TreeNodePlaylist)
                {
                    if (target.Parent == null)
                    {
                        currentlySelectedNodeParentIndex = -1;

                        int targetIndex = profileManager.Profile.Playlists.IndexOf
                            (profileManager.Profile.Playlists[((TreeNodePlaylist)target).ID]);
                        currentlySelectedNodeIndex = target.Index;
                        originalCon.ParentGroupID = "";
                        profileManager.Profile.Playlists.Move(originalIndex, targetIndex);
                    }
                    else
                    {
                        // Get parent id
                        currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(target.Parent);
                        currentlySelectedNodeIndex = target.Index;
                        string parentID = ((TreeNodePlaylistsGroup)target.Parent).ID;
                        int targetIndex = profileManager.Profile.Playlists.IndexOf
                           (profileManager.Profile.Playlists[((TreeNodePlaylist)target).ID]);
                        profileManager.Profile.Playlists[originalIndex].ParentGroupID = parentID;
                        profileManager.Profile.Playlists.Move(originalIndex, targetIndex);
                    }
                }
                else if (target is TreeNodePlaylistsGroup)
                {
                    string parentID = ((TreeNodePlaylistsGroup)target).ID;
                    profileManager.Profile.Playlists[originalIndex].ParentGroupID = parentID;
                    profileManager.Profile.OnPlaylistMoved(profileManager.Profile.Playlists[originalIndex].Name);
                }
            }
            OnEnableDisableButtons();
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            canDoDragDrop = true;
        }
        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ActiveSelection();
        }
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
                ActiveSelection();
        }

        private void Profile_PlaylistsGroupsCleared(object sender, EventArgs e)
        {
            RefreshPlaylistGroups(); OnEnableDisableButtons();
        }
        private void Profile_PlaylistsGroupRemoved(object sender, EventArgs e)
        {
            RefreshPlaylistGroups(); OnEnableDisableButtons();
        }
        private void Profile_PlaylistsGroupAdded(object sender, EventArgs e)
        {
            RefreshPlaylistGroups(); OnEnableDisableButtons();
            //  if (treeView1.Nodes.Count > 0)
            //       treeView1.Nodes[treeView1.Nodes.Count - 1].BeginEdit();
        }
        private void Profile_PlaylistsCleared(object sender, EventArgs e)
        {
            RefreshPlaylists(); OnEnableDisableButtons();
        }
        private void Profile_PlaylistRemoved(object sender, EventArgs e)
        {
            RefreshPlaylists(); OnEnableDisableButtons();
        }
        private void Profile_PlaylistAdded(object sender, EventArgs e)
        {
            RefreshPlaylists(); OnEnableDisableButtons();
        }
        private void Profile_PlaylistsGroupsMoved(object sender, EventArgs e)
        {
            RefreshPlaylistGroups();
            try
            {
                treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeIndex];
            }
            catch { }
            OnEnableDisableButtons();
        }
        private void Profile_PlaylistMoved(object sender, EventArgs e)
        {
            RefreshPlaylistGroups();
            try
            {
                if (currentlySelectedNodeParentIndex >= 0)
                {
                    treeView1.Nodes[currentlySelectedNodeParentIndex].Expand();
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeParentIndex].Nodes[currentlySelectedNodeIndex];
                }
                else
                {
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeIndex];
                }
            }
            catch { }
            OnEnableDisableButtons();
        }
        private void Profile_PlaylistsGroupsSorted(object sender, EventArgs e)
        {
            RefreshPlaylistGroups(); OnEnableDisableButtons();
        }
        private void Profile_PlaylistsSorted(object sender, EventArgs e)
        {
            RefreshPlaylists();
            try
            {
                if (currentlySelectedNodeParentIndex > 0)
                {
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeParentIndex];
                    treeView1.Nodes[currentlySelectedNodeParentIndex].Expand();
                }
            }
            catch { }
            OnEnableDisableButtons();
        }
        private void Profile_PlaylistPropertiesChanged(object sender, EventArgs e)
        {
            RefreshPlaylists();
            try
            {
                if (currentlySelectedNodeParentIndex >= 0)
                {
                    treeView1.Nodes[currentlySelectedNodeParentIndex].Expand();
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeParentIndex].Nodes[currentlySelectedNodeIndex];
                }
                else
                {
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeIndex];
                }
            }
            catch { }
            OnEnableDisableButtons();

            if (treeView1.SelectedNode == null)
            {
                SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
                object _defaultStyle = settings.GetValue("Default Style", true, new EOStyle());
                if (_defaultStyle != null)
                    treeView1.BackgroundImage = ((EOStyle)_defaultStyle).image_PlaylistsBrowser;
                else
                    treeView1.BackgroundImage = null;
                treeView1.Invalidate();
                return;
            }
            if (treeView1.SelectedNode is TreeNodePlaylistsGroup)
            {
                treeView1.BackgroundImage = profileManager.Profile.PlaylistGroups[((TreeNodePlaylistsGroup)treeView1.SelectedNode).ID].Style.image_PlaylistsBrowser;
            }
            else if (treeView1.SelectedNode is TreeNodePlaylist)
            {
                treeView1.BackgroundImage = profileManager.Profile.Playlists[((TreeNodePlaylist)treeView1.SelectedNode).ID].Style.image_PlaylistsBrowser;
            }
            treeView1.Invalidate();
        }
        private void Profile_ProfileCleanUpFinished(object sender, EventArgs e)
        {
            RefreshPlaylists();
        }
        private void rootPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRootPlaylist();
        }
        private void playlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPlaylist();
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
        private void clearIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearIcon();
        }
        private void playlistGroupsByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortPlaylistsGroupByName();
        }
        private void rootPlaylistsByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortRootPlaylistsByName();
        }
        private void playlistsByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortPlaylistsByName();
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowItemProperties();
        }
        private void toolStripButton_moveUp_Click(object sender, EventArgs e)
        {
            MoveSelectedItemUp();
        }
        private void toolStripButton_moveDown_Click(object sender, EventArgs e)
        {
            MoveSelectedItemDown();
        }
        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            SortRootPlaylistsByName();
        }
        private void PlaylistsBrowser_Enter(object sender, EventArgs e)
        {
            panel1.Visible = toolStrip1.Visible = true;
        }
        private void toolStripSplitButton2_ButtonClick_1(object sender, EventArgs e)
        {
            SortPlaylistsGroupByName();
            SortRootPlaylistsByName();
        }
        private void cloneSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClonePlaylistSettings();
        }
        private void importPlaylistFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportPlaylistFile();
        }
        private void exportPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportSelectedPlaylist();
        }
    }
}
