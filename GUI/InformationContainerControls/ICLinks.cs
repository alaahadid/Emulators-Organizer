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
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class ICLinks : ICControl
    {
        /*
         * In this control, we need to implement all methods instead of overriding
         */
        public ICLinks(string icid, string parentConsoleID)
        {
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;
            InitializeComponent();
            base.canAcceptDraggedFiles = false;
            base.canSelectionReverse = true;
            base.ApplyStyleOnRomSelection();
            // Load settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    toolStrip1.Visible = ((InformationContainerLinks)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip;
            }
        }
        private int linkIndex = 0;
        private Dictionary<string, string> links = new Dictionary<string, string>();
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
            ClearBrowser();
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
        }
        private void ShowLink()
        {
            ClearBrowser();
            if (linkIndex >= 0 && linkIndex < links.Count)
            {
                // Get page
                try
                {
                    // show browser
                    toolStripButton_showList.Checked = false;
                    // load page within browser
                    webBrowser1.Url = new Uri(links[listView1.Items[linkIndex].Text]);
                }
                catch
                {

                }
            }
        }
        protected override void UpdateStatus()
        {
            StatusLabel.Text = string.Format("{0} / {1}", (linkIndex + 1), links.Count);
            toolStripButton3.Enabled = toolStripButton4.Enabled = links.Count > 1;
        }
        private void ClearBrowser()
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
            toolStripButton1.Enabled =
                toolStripButton2.Enabled = false;
            addLinkToolStripMenuItem.Enabled =
                removeSelectedToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            toolStripButton1.Enabled =
    toolStripButton2.Enabled = true;
            addLinkToolStripMenuItem.Enabled =
                removeSelectedToolStripMenuItem.Enabled = true;
        }

        // Add link
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form_AddLink frm = new Form_AddLink(links);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainerLinks cont = (InformationContainerLinks)profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);

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
        // next link
        private void toolStripButton4_Click(object sender, EventArgs e)
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
        // previous link
        private void toolStripButton3_Click(object sender, EventArgs e)
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
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                linkIndex = listView1.SelectedItems[0].Index;
                UpdateStatus();
                ShowLink();
            }
        }
        private void toolStripButton_showList_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Visible = !toolStripButton_showList.Checked;
            listView1.Visible = toolStripButton_showList.Checked;
        }
        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            toolStripLabel_browserStatus.Text = ls["Status_LoadingPage"];
        }
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            toolStripLabel_browserStatus.Text = e.Url.OriginalString;
        }
        // show selected with default browser
        private void toolStripButton5_Click(object sender, EventArgs e)
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
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            nextToolStripMenuItem.Enabled = previousToolStripMenuItem.Enabled = links.Count > 1;
            showToolstripToolStripMenuItem.Checked = toolStrip1.Visible;
            editListToolStripMenuItem.Checked = toolStripButton_showList.Checked;
        }
        private void editListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_showList.Checked = !toolStripButton_showList.Checked;
        }
        private void showToolstripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    ((InformationContainerLinks)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip = toolStrip1.Visible;
            }
        }
    }
}
