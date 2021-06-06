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
using System.Runtime.InteropServices;
using System.Diagnostics;
using EmulatorsOrganizer.Core;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class ICYoutube : ICControl
    {
        public ICYoutube(string icid, string parentConsoleID)
        {
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;
            InitializeComponent();
            base.canAcceptDraggedFiles = false;
            base.canSelectionReverse = true;
            base.ApplyStyleOnRomSelection();
            // Load settings
            if (parentConsoleID != null && parentConsoleID != "")
            {
                console = profileManager.Profile.Consoles[base.parentConsoleID];
                InformationContainerYoutubeVideo cont = (InformationContainerYoutubeVideo)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
                if (cont == null) return;
                // Load/Fix settings
                toolStripButton_autoPlay.Checked = cont.AutoPlay;
                toolStrip1.Visible = cont.ShowToolstrip;
            }
        }
        private int linkIndex = 0;
        private Dictionary<string, string> links = new Dictionary<string, string>();
        private EmulatorsOrganizer.Core.Console console;

        public override void InitializeEvents()
        {
            base.InitializeEvents();
            profileManager.Profile.GamePlayStart += Profile_GamePlayStart;
            profileManager.Profile.RomFinishedPlayed += Profile_RomFinishedPlayed;

        }
        public override void DisposeEvents()
        {
            base.DisposeEvents();
            profileManager.Profile.GamePlayStart -= Profile_GamePlayStart;
            profileManager.Profile.RomFinishedPlayed -= Profile_RomFinishedPlayed;
        }
        protected override void RefreshFiles()
        {
            RefreshLinks();
        }
        private void RefreshLinks()
        {
            // Instead of normal files refresh, we need to refresh links.
            // Clear
            linkIndex = -1;
            links.Clear();
            ClearFlash();
            if (console == null) return;
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                // Load files if found
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);

                if (item != null && item is InformationContainerItemLinks)
                {
                    links = new Dictionary<string, string>(((InformationContainerItemLinks)item).Links);
                    linkIndex = links.Count > 0 ? 0 : -1;
                }
            }
            // Load links into list view
            listView1.Items.Clear();
            foreach (string k in links.Keys)
            {
                ListViewItem item = new ListViewItem();
                item.Text = k;
                item.SubItems.Add(links[k]);
                listView1.Items.Add(item);
            }
            // Update status
            UpdateStatus();
            // Auto play ?
            if (toolStripButton_autoPlay.Checked && links.Count > 0 && console.InformationContainersMap.IsContainerVisibleAndSelected(ICID))
                ShowLink();
        }
        private void ShowLink()
        {
            ClearFlash();
            if (linkIndex >= 0 && linkIndex < links.Count)
            {
                // Get page
                try
                {
                    // show browser
                    toolStripButton_showList.Checked = false;
                    // Load video
                    webBrowser1.DocumentText = GetEmbededVideo(links[listView1.Items[linkIndex].Text]);
                }
                catch
                {

                }
            }
        }
        public override void OnPriorityActive()
        {
            ShowLink();// make sure it's shown !
        }
        private string GetEmbededVideo(string link)
        {
            // Adjust link
            link = link.Replace("watch?", "");
            link = link.Replace("=", "/");
            bool autoPlay = toolStripButton_autoPlay.Checked && console.InformationContainersMap.IsContainerVisibleAndSelected(ICID);
            // Make code
            return "<embed src=" + link + "?autoplay=" + (autoPlay ? "1" : "0") +
              "&hl=en&fs=1& type=application/x-shockwave-flash allowscriptaccess=always allowfullscreen=true " +
              "width=" + (webBrowser1.Width - 35) +
              " height=" + (webBrowser1.Height - 35) +
              "></embed>";
        }
        protected override void UpdateStatus()
        {
            StatusLabel.Text = string.Format("{0} / {1}", (linkIndex + 1), links.Count);
            toolStripButton4.Enabled = toolStripButton5.Enabled = links.Count > 1;
        }
        private void ClearFlash()
        {
            toolStripButton_showList.Checked = true;
            if (!webBrowser1.IsDisposed)
                webBrowser1.Url = null;
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
                toolStripButton_autoPlay.Enabled = false;
            addToolStripMenuItem.Enabled = removeToolStripMenuItem.Enabled =
                autoPlayToolStripMenuItem.Enabled = openWithDefaultWindowsWebBrowserToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            toolStripButton1.Enabled = toolStripButton2.Enabled = toolStripButton3.Enabled =
                toolStripButton_autoPlay.Enabled = true;
            addToolStripMenuItem.Enabled = removeToolStripMenuItem.Enabled =
                   autoPlayToolStripMenuItem.Enabled = openWithDefaultWindowsWebBrowserToolStripMenuItem.Enabled = true;
        }

        private void toolStripButton_showList_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Visible = !toolStripButton_showList.Checked;
            listView1.Visible = toolStripButton_showList.Checked;
        }
        // Add video
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form_AddLink frm = new Form_AddLink(links);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainerYoutubeVideo cont = (InformationContainerYoutubeVideo)profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);

                // Add the files
                InformationContainerItemLinks infItem = null;
                if (item != null && item is InformationContainerItemLinks)
                {
                    infItem = (InformationContainerItemLinks)item;
                }
                else
                {
                    // Create new
                    infItem = new InformationContainerItemLinks(profileManager.Profile.GenerateID(), ICID);
                    infItem.Links = new Dictionary<string, string>();
                    // Add it to the rom
                    rom.RomInfoItems.Add(infItem);
                    rom.Modified = true;
                }
                if (infItem.Links == null)
                    infItem.Links = new Dictionary<string, string>();
                // Add the link
                if (!infItem.Links.ContainsKey(frm.LinkName))// make sure !
                    infItem.Links.Add(frm.LinkName, frm.LinkURL);
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_EnteredNameAlreadyTakenForrom"], ls["MessageCaption_AddLink"]);
                    return;
                }

                RefreshLinks();
                profileManager.Profile.OnInformationContainerItemsModified(cont.Name);
            }
        }
        // Remove link
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                string ICName = profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID).DisplayName;

                if (item != null && item is InformationContainerItemLinks)
                {
                    Dictionary<string, string> linksList = new Dictionary<string, string>(((InformationContainerItemLinks)item).Links);
                    if (linksList.Count == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteLinkNoLinkFound"],
                            ls["MessageCaption_InformationContainerControl"]);
                    }
                    else
                    {
                        if (linkIndex >= 0 && linkIndex < links.Count)
                        {
                            // This is it
                            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(
                                ls["Message_AreYouSureYouWantToDeleteThisLink"],
                                ls["MessageCaption_InformationContainerControl"],
                                new string[] { ls["Button_Yes"], ls["Button_No"] }, 1, ManagedMessageBoxIcon.Question);
                            if (res.ClickedButtonIndex == 0)
                            {

                                ((InformationContainerItemLinks)item).Links.Remove(listView1.Items[linkIndex].Text);
                                RefreshFiles();
                                profileManager.Profile.OnInformationContainerItemsModified(ICName);
                            }
                        }
                        else
                        {
                            ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteLinkNotValidIndex"], ls["MessageCaption_InformationContainerControl"]);
                        }
                    }
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteLinkNoLinkFound"], ls["MessageCaption_InformationContainerControl"]);
                }
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteLinkNoRomSelected"], ls["MessageCaption_InformationContainerControl"]);
            }
        }
        // next
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (links.Count > 0)
            {
                linkIndex = (linkIndex + 1) % links.Count;
            }
            else
            {
                linkIndex = -1;
            }
            UpdateStatus();
            ShowLink();
        }
        // previous
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (links.Count > 0)
            {
                linkIndex--;
                if (linkIndex < 0)
                    linkIndex = 0;
            }
            else
            {
                linkIndex = -1;
            }
            UpdateStatus();
            ShowLink();
        }
        // play !
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                linkIndex = listView1.SelectedItems[0].Index;
                UpdateStatus();
                ShowLink();
            }
        }
        // show in browser
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (linkIndex >= 0 && linkIndex < links.Count)
            {
                // Get page
                try
                {
                    // load page with windows default browser
                    System.Diagnostics.Process.Start(links[listView1.Items[linkIndex].Text]);
                }
                catch (Exception ex)
                {
                    ManagedMessageBox.ShowErrorMessage(ex.Message);
                }
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                linkIndex = listView1.SelectedItems[0].Index;
            }
            else
                linkIndex = -1;
        }
        private void toolStripButton_autoPlay_CheckedChanged(object sender, EventArgs e)
        {
            InformationContainerYoutubeVideo cont = (InformationContainerYoutubeVideo)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
            if (cont == null) return;
            cont.AutoPlay = toolStripButton_autoPlay.Checked;
        }
        private void ICYoutube_Resize(object sender, EventArgs e)
        {
            // Update video link ?
            ShowLink();
        }
        private void Profile_GamePlayStart(object sender, EventArgs e)
        {
            ClearFlash();
        }
        private void Profile_RomFinishedPlayed(object sender, RomFinishedPlayArgs e)
        {
            ShowLink();
        }
        private void showListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_showList.Checked = !toolStripButton_showList.Checked;
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            autoPlayToolStripMenuItem.Checked = toolStripButton_autoPlay.Checked;
            showListToolStripMenuItem.Checked = toolStripButton_showList.Checked;
            showToolstripToolStripMenuItem.Checked = toolStrip1.Visible;
        }
        private void autoPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_autoPlay.Checked = !toolStripButton_autoPlay.Checked;
        }
        private void showToolstripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
            // Save settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    ((InformationContainerYoutubeVideo)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip = toolStrip1.Visible;
            }
        }
    }
}
