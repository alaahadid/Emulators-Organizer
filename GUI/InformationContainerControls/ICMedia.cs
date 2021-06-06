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
using AxWMPLib;
using EmulatorsOrganizer.Core;
using MLV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EmulatorsOrganizer.GUI
{
    // TODO: media player is pain in the ass
    public partial class ICMedia : ICControl
    {
        public ICMedia(string icid, string parentConsoleID)
        {
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;
            base.canAcceptDraggedFiles = true;
            base.canSelectionReverse = true;
            InitializeComponent();
            base.ApplyStyleOnRomSelection();
            if (parentConsoleID != null && parentConsoleID != "")
            {
                console = profileManager.Profile.Consoles[base.parentConsoleID];
                InformationContainerMedia cont = (InformationContainerMedia)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
                if (cont == null) return;
                // Load/Fix settings
                toolStripButton_autoStart.Checked = cont.AutoStart;
                toolStripButton_autoHideToolstrip.Checked = cont.AutoHideToolstrip;
                toolStrip1.Visible = cont.ShowToolstrip;
                toolStripButton_repeat_list.Checked = cont.RepeatList;

                console.InformationContainersMap.TabSelectionChanged += InformationContainersMap_TabSelectionChanged;
                if (cont.ColumnWidths == null)
                    cont.ColumnWidths = new List<int>();
                if (cont.ColumnWidths.Count < 4)
                {
                    cont.ColumnWidths.Add(60);
                    cont.ColumnWidths.Add(60);
                    cont.ColumnWidths.Add(60);
                    cont.ColumnWidths.Add(100);
                }
                // Refresh columns
                ManagedListViewColumn column = new ManagedListViewColumn();
                column.HeaderText = ls["Column_Name"];
                column.ID = "name";
                column.Width = cont.ColumnWidths[0];
                managedListView1.Columns.Add(column);
                column = new ManagedListViewColumn();
                column.HeaderText = ls["Column_Duration"];
                column.ID = "duration";
                column.Width = cont.ColumnWidths[1];
                managedListView1.Columns.Add(column);
                column = new ManagedListViewColumn();
                column.HeaderText = ls["Column_Type"];
                column.ID = "type";
                column.Width = cont.ColumnWidths[2];
                managedListView1.Columns.Add(column);
                column = new ManagedListViewColumn();
                column.HeaderText = ls["Column_Path"];
                column.ID = "path";
                column.Width = cont.ColumnWidths[3];
                managedListView1.Columns.Add(column);
                InitializeMediaPlayer(cont);
            }
        }

        private EmulatorsOrganizer.Core.Console console;
        private AxWindowsMediaPlayer mediaPlayer;
        private bool mediaPlayerInitialized;
        private bool isDisposingMediaPlayer = false;
        private bool isInitializingMediaPlayer = false;

        public override void InitializeEvents()
        {
            base.InitializeEvents();
            profileManager.Profile.GamePlayStart += Profile_GamePlayStart;
        }
        public override void DisposeEvents()
        {
            base.DisposeEvents();
            profileManager.Profile.GamePlayStart -= Profile_GamePlayStart;
        }
        private void InitializeMediaPlayer(InformationContainerMedia cont)
        {
            if (parentConsoleID == null)
                return;
            if (parentConsoleID == "")
                return;
            if (mediaPlayer == null && !isInitializingMediaPlayer && !isDisposingMediaPlayer && !mediaPlayerInitialized)
            {
                isInitializingMediaPlayer = true;
                mediaPlayer = new AxWindowsMediaPlayer();
                ((System.ComponentModel.ISupportInitialize)(mediaPlayer)).BeginInit();

                // Player
                mediaPlayer.Parent = this;
                mediaPlayer.Enabled = true;
                mediaPlayer.Dock = DockStyle.Fill;
                mediaPlayer.BringToFront();
                mediaPlayer.Name = "Player";
                mediaPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.axWindowsMediaPlayer1_PlayStateChange);
                mediaPlayer.MouseDownEvent += mediaPlayer_MouseDownEvent;
                ((System.ComponentModel.ISupportInitialize)(mediaPlayer)).EndInit();

                mediaPlayer.enableContextMenu = false;
                mediaPlayer.ContextMenuStrip = contextMenuStrip1;
                mediaPlayer.uiMode = cont.ShowMediaControls ? "full" : "none";


                isInitializingMediaPlayer = false;
                mediaPlayerInitialized = true;
            }
        }
        private void DisposeMediaPlayer()
        {
            Trace.WriteLine("!!!! disposing media player !!!");
            if (mediaPlayer != null && !isInitializingMediaPlayer && !isDisposingMediaPlayer && mediaPlayerInitialized)
            {
                isDisposingMediaPlayer = true;
                try
                {
                    mediaPlayer.Ctlcontrols.stop();
                    mediaPlayer.close();
                    mediaPlayer.PlayStateChange -= new AxWMPLib.
                    _WMPOCXEvents_PlayStateChangeEventHandler(this.axWindowsMediaPlayer1_PlayStateChange);
                    mediaPlayer.MouseDownEvent -= mediaPlayer_MouseDownEvent;
                    if (mediaPlayer.currentMedia != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.currentMedia);
                    if (mediaPlayer.mediaCollection != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.mediaCollection);
                    if (mediaPlayer.playlistCollection != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.playlistCollection);
                    if (mediaPlayer.Ctlcontrols != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.Ctlcontrols);
                    if (mediaPlayer.currentPlaylist != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.currentPlaylist);
                    this.Controls.Remove(mediaPlayer);
                    mediaPlayer.Dispose();
                    while (!mediaPlayer.IsDisposed)
                    {
                        // Wait until it get disposed
                    }
                }
                catch { }
                mediaPlayer = null;
                isDisposingMediaPlayer = false;
                mediaPlayerInitialized = false;
            }
            GC.Collect();
        }
        protected override void ShowFile()
        {
            if (parentConsoleID == null)
                return;
            if (parentConsoleID == "")
                return;
            if (mediaPlayer == null)
                return;
            if (base.IsValidFileIndex() && console.InformationContainersMap.IsContainerVisibleAndSelected(ICID))
            {
                mediaPlayer.Ctlcontrols.playItem(mediaPlayer.currentPlaylist.get_Item(fileIndex));
                if (!toolStripButton_autoStart.Checked)
                    mediaPlayer.Ctlcontrols.stop();
                toolStripButton_showList.Checked = false;
            }
        }
        protected override void UpdateStatus()
        {
            base.UpdateStatus();
            StatusLabel.Text = base.GetStatusString();
            toolStripButton6.Enabled = toolStripButton7.Enabled = files.Count > 1;
        }
        // Handle internal events
        protected override void OnFilesRefresh()
        {
            ClearMediaList();
            if (parentConsoleID == null)
                return;
            if (parentConsoleID == "")
                return;

            // if (files.Count > 0 && mediaPlayer == null && !mediaPlayerInitialized)
            // {
            //      Trace.WriteLine("*** initialize media player; files are empty" );
            //      InitializeMediaPlayer();
            // }
            if (mediaPlayer == null)
                return;
            // Refresh playlist
            foreach (string file in files)
            {
                WMPLib.IWMPMedia m1 = mediaPlayer.newMedia(HelperTools.GetFullPath(file));
                mediaPlayer.currentPlaylist.appendItem(m1);

                ManagedListViewItem item = new ManagedListViewItem();
                ManagedListViewSubItem subitem = new ManagedListViewSubItem();
                subitem.ColumnID = "name";
                subitem.DrawMode = ManagedListViewItemDrawMode.UserDraw;
                if (m1.name != "" && m1.name != null)
                    item.Text = subitem.Text = m1.name;
                else
                    item.Text = subitem.Text = Path.GetFileName(file);
                item.SubItems.Add(subitem);

                subitem = new ManagedListViewSubItem();
                subitem.ColumnID = "duration";
                subitem.DrawMode = ManagedListViewItemDrawMode.Text;
                subitem.Text = m1.durationString;
                item.SubItems.Add(subitem);

                subitem = new ManagedListViewSubItem();
                subitem.ColumnID = "type";
                subitem.DrawMode = ManagedListViewItemDrawMode.Text;
                subitem.Text = Path.GetExtension(file).Replace(".", "");
                item.SubItems.Add(subitem);

                subitem = new ManagedListViewSubItem();
                subitem.ColumnID = "path";
                subitem.DrawMode = ManagedListViewItemDrawMode.Text;
                subitem.Text = HelperTools.GetFullPath(file);
                item.SubItems.Add(subitem);

                managedListView1.Items.Add(item);
            }
            mediaPlayer.settings.setMode("loop", toolStripButton_repeat_list.Checked);
        }
        public override void OnPriorityActive()
        {
            ShowFile();// make sure it's shown !
        }
        private void ClearMediaList()
        {
            // Only stop media
            if (mediaPlayer != null)
            {
                mediaPlayer.Ctlcontrols.stop();
                mediaPlayer.currentPlaylist.clear();
                mediaPlayer.URL = null;
            }
            managedListView1.Items.Clear();// clear list
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            toolStrip1.BackColor = style.bkgColor_InformationContainerTabs;
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            toolStripButton1.Enabled = toolStripButton2.Enabled = toolStripButton3.Enabled =
                toolStripButton_autoHideToolstrip.Enabled = toolStripButton_autoStart.Enabled =
                toolStripButton_repeat_list.Enabled = false;
            addMoreFilesToolStripMenuItem.Enabled = deleteSelectedToolStripMenuItem.Enabled =
                editListToolStripMenuItem.Enabled = autoStartToolStripMenuItem.Enabled =
                autoHideToolstripToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            if (this.InvokeRequired) OnProfileSavingFinished1();
            else
                this.Invoke(new Action(OnProfileSavingFinished1));
        }
        private void OnProfileSavingFinished1()
        {
            base.OnProfileSavingFinished();
            toolStripButton1.Enabled = toolStripButton2.Enabled = toolStripButton3.Enabled =
              toolStripButton_autoHideToolstrip.Enabled = toolStripButton_autoStart.Enabled =
              toolStripButton_repeat_list.Enabled = true;
            addMoreFilesToolStripMenuItem.Enabled = deleteSelectedToolStripMenuItem.Enabled =
          editListToolStripMenuItem.Enabled = autoStartToolStripMenuItem.Enabled =
          autoHideToolstripToolStripMenuItem.Enabled = true;
        }
        // Add files
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            base.AddFilesToList();
        }
        // Remove selected file
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            base.RemoveSelectedFileFromList();
        }
        // Edit list
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            base.EditList();
        }
        // Open selected file
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            base.OpenSelectedFile();
        }
        // Open file location
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            base.OpenSelectedFileLocation();
        }
        // Previous
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            base.PreviousFile();
        }
        // Next
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            base.NextFile();
        }
        // Draw stuff
        private void managedListView1_DrawSubItem(object sender, MLV.ManagedListViewSubItemDrawArgs e)
        {
            if (e.ColumnID == "name")
            {
                if (e.ItemIndex == fileIndex)
                {
                    if (mediaPlayer != null)
                    {
                        if (!mediaPlayer.IsDisposed)
                        {
                            if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                            {
                                e.ImageToDraw = Properties.Resources.control_play;
                            }
                        }
                    }
                }
                e.TextToDraw = managedListView1.Items[e.ItemIndex].Text;
            }
        }
        // On tab changes !
        private void InformationContainersMap_TabSelectionChanged(object sender, EventArgs e)
        {
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[parentConsoleID];
            if (console == null) return;
            bool stopMediaOnTabChange = (bool)settings.GetValue("MediaTab:StopPlayerOnTabChange", true, true);
            if (!stopMediaOnTabChange)
                return;

            if (console.InformationContainersMap.IsContainerVisibleAndSelected(ICID))
            {
                // The user is selecting this page !!
                // Make auto play if it can
                ShowFile();
            }
            else
            {
                // The user is selecting other page, stop media
                if (mediaPlayer != null)
                {
                    try
                    {
                        mediaPlayer.Ctlcontrols.stop();
                    }
                    catch { }
                }
            }

        }
        // On list show/hide
        private void toolStripButton_showList_CheckedChanged(object sender, EventArgs e)
        {
            managedListView1.Visible = toolStripButton_showList.Checked;
            if (mediaPlayer != null)
            {
                if (!mediaPlayer.IsDisposed)
                {
                    mediaPlayer.Visible = !toolStripButton_showList.Checked;
                }
            }
        }
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (mediaPlayer != null)
            {
                if (!mediaPlayer.IsDisposed)
                {
                    if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
                    {
                        toolStrip1.Visible = true;
                        toolStripButton_showList.Checked = true;
                    }
                    else if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
                    {
                        toolStrip1.Visible = true;
                    }
                    else if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        if (toolStripButton_autoHideToolstrip.Checked)
                            toolStrip1.Visible = false;
                    }
                }
            }
        }
        private void managedListView1_ItemDoubleClick(object sender, ManagedListViewItemDoubleClickArgs e)
        {
            fileIndex = e.ClickedItemIndex;
            ShowFile();
        }
        private void managedListView1_AfterColumnResize(object sender, EventArgs e)
        {
            // Save column widths
            InformationContainerMedia cont = (InformationContainerMedia)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
            if (cont == null) return;
            cont.ColumnWidths = new List<int>();
            cont.ColumnWidths.Add(managedListView1.Columns[0].Width);
            cont.ColumnWidths.Add(managedListView1.Columns[1].Width);
            cont.ColumnWidths.Add(managedListView1.Columns[2].Width);
            cont.ColumnWidths.Add(managedListView1.Columns[3].Width);
        }
        private void managedListView1_DragDrop(object sender, DragEventArgs e)
        {
            base.OnDragDrop(e);
        }
        private void managedListView1_DragEnter(object sender, DragEventArgs e)
        {
            base.OnDragEnter(e);
        }
        private void managedListView1_DragLeave(object sender, EventArgs e)
        {
            base.OnDragLeave(e);
        }
        private void managedListView1_DragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);
        }
        private void Profile_GamePlayStart(object sender, EventArgs e)
        {
            // Stop it !
            if (mediaPlayer != null)
            {
                try
                {
                    mediaPlayer.Ctlcontrols.stop();
                }
                catch { }
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            nextToolStripMenuItem.Enabled = previousToolStripMenuItem.Enabled =
               files.Count > 1;
            deleteSelectedToolStripMenuItem.Enabled = base.IsValidFileIndex();
            showToolsbarToolStripMenuItem.Checked = toolStrip1.Visible;
            showListToolStripMenuItem.Checked = toolStripButton_showList.Checked;
            showMediaControlsToolStripMenuItem.Checked = (mediaPlayer.uiMode == "full");
            autoHideToolstripToolStripMenuItem.Checked = toolStripButton_autoHideToolstrip.Checked;
            autoStartToolStripMenuItem.Checked = toolStripButton_autoStart.Checked;
        }
        private void showListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_showList.Checked = !toolStripButton_showList.Checked;
        }
        private void showMediaControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer.uiMode = (mediaPlayer.uiMode == "none") ? "full" : "none";
            mediaPlayer.Invalidate();
            if (parentConsoleID != null && parentConsoleID != "")
            {
                console = profileManager.Profile.Consoles[base.parentConsoleID];
                InformationContainerMedia cont = (InformationContainerMedia)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
                if (cont != null)
                {
                    cont.ShowMediaControls = mediaPlayer.uiMode == "full";
                    profileManager.Profile.OnInformationContainerItemsModified(cont.DisplayName);
                }
            }
        }
        private void showToolsbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
            if (parentConsoleID != null && parentConsoleID != "")
            {
                console = profileManager.Profile.Consoles[base.parentConsoleID];
                InformationContainerMedia cont = (InformationContainerMedia)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
                if (cont != null)
                {
                    cont.ShowToolstrip = toolStrip1.Visible;
                    profileManager.Profile.OnInformationContainerItemsModified(cont.DisplayName);
                }
            }
        }
        private void autoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_autoStart.Checked = !toolStripButton_autoStart.Checked;
        }
        private void autoHideToolstripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_autoHideToolstrip.Checked = !toolStripButton_autoHideToolstrip.Checked;
        }
        private void mediaPlayer_MouseDownEvent(object sender, _WMPOCXEvents_MouseDownEvent e)
        {
            if (e.nButton == 2)
                this.ContextMenuStrip.Show(mediaPlayer.PointToScreen(new Point(e.fX, e.fY)));
        }
        private void toolStripButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.settings.setMode("loop", toolStripButton_repeat_list.Checked);
                InformationContainerMedia cont = (InformationContainerMedia)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
                if (cont != null)
                {
                    cont.RepeatList = toolStripButton_repeat_list.Checked;
                }
            }
        }
    }
}
