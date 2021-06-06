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
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Core;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class ConsoleLinksViewer : UserControl
    {
        public ConsoleLinksViewer()
        {
            InitializeComponent();
        }
        protected ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        protected LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private Dictionary<string, string> links = new Dictionary<string, string>();
        private int linkIndex = 0;
        private string consoleID;

        public void LoadInformation(string consoleID)
        {
            this.consoleID = consoleID;
            Core.Console console = profileManager.Profile.Consoles[consoleID];
            // Clear
            linkIndex = -1;
            links.Clear();
            ClearBrowser();

            // Load links if found
            if (console.Links == null)
                console.Links = new Dictionary<string, string>();
            links = new Dictionary<string, string>(console.Links);
            linkIndex = links.Count > 0 ? 0 : -1;

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
        public void ClearInformation()
        {
            listView1.Items.Clear();
            ClearBrowser();
        }
        private void UpdateStatus()
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

        // Add link
        private void AddLink_Click(object sender, EventArgs e)
        {
            Form_AddLink frm = new Form_AddLink(links);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                Core.Console console = profileManager.Profile.Consoles[consoleID];
                // Add the link
                if (!console.Links.ContainsKey(frm.LinkName))// make sure !
                    profileManager.Profile.Consoles[consoleID].Links.Add(frm.LinkName, frm.LinkURL);
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_EnteredNameAlreadyTakenForrom"], ls["MessageCaption_AddLink"]);
                    return;
                }

                LoadInformation(consoleID);
               // profileManager.Profile.OnConsolePropertiesChanged(console.Name);
            }
        }
        // Next link
        private void Next_Click(object sender, EventArgs e)
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
        // Previous link
        private void Previous_Click(object sender, EventArgs e)
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
        private void ShowSelectedInBrowser_Click(object sender, EventArgs e)
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
        private void Remove_Click(object sender, EventArgs e)
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
                    Core.Console console = profileManager.Profile.Consoles[consoleID];
                    console.Links.Remove(listView1.Items[linkIndex].Text);
                    LoadInformation(consoleID);
                   // profileManager.Profile.OnConsolePropertiesChanged(console.Name);
                }
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteLinkNotValidIndex"], ls["MessageCaption_InformationContainerControl"]);
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
        }
    }
}
