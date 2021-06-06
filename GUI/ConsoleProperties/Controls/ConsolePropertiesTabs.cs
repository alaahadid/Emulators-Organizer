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
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class ConsolePropertiesTabs : IConsolePropertiesControl
    {
        public ConsolePropertiesTabs()
        {
            InitializeComponent();
        }
        private bool odd;
        public override string ToString()
        {
            return ls["Title_Tabs"];
        }
        public override string Description
        {
            get
            {
                return ls["ConsolePropertiesDescription_Tabs"];
            }
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            listView1.Items.Clear();
            // Reload
            foreach (InformationContainer ic in profileManager.Profile.Consoles[base.consoleID].InformationContainers)
            {
                ListViewItem item = new ListViewItem();
                item.Text = ic.DisplayName;
                item.SubItems.Add(ic.Name);
                item.SubItems.Add(profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.IsContainerVisible(ic.ID) ? ls["Word_Yes"] : ls["Word_No"]);
                item.Tag = ic.ID;
                listView1.Items.Add(item);
            }
            checkBox_tabPriority.Checked = profileManager.Profile.Consoles[base.consoleID].AutoSwitchTabPriorityDepend;
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            profileManager.Profile.Consoles[base.consoleID].AutoSwitchTabPriorityDepend = checkBox_tabPriority.Checked;
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
            checkBox_tabPriority.Checked = true;
        }
        // Add
        private void button1_Click(object sender, EventArgs e)
        {
            Form_AddInformationContainerItem frm = new Form_AddInformationContainerItem(profileManager.Profile.Consoles[base.consoleID]);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Determine type
                InformationContainer inf = null;
                switch (frm.EnteredTypeIndex)
                {
                    case 0: inf = new InformationContainerImage(profileManager.Profile.GenerateID()); break;
                    case 1: inf = new InformationContainerInfoText(profileManager.Profile.GenerateID()); break;
                    case 2: inf = new InformationContainerLinks(profileManager.Profile.GenerateID()); break;
                    case 3: inf = new InformationContainerMedia(profileManager.Profile.GenerateID()); break;
                    case 4: inf = new InformationContainerPDF(profileManager.Profile.GenerateID()); break;
                    case 5:
                        {
                            foreach (InformationContainer ic in profileManager.Profile.Consoles[base.consoleID].InformationContainers)
                            {
                                if (ic is InformationContainerRomInfo)
                                {
                                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouCantAddRomInfoTwice"]);
                                    return;
                                }
                            }
                            inf = new InformationContainerRomInfo(profileManager.Profile.GenerateID());
                            break;
                        }
                    case 6: inf = new InformationContainerYoutubeVideo(profileManager.Profile.GenerateID()); break;
                    case 7: inf = new InformationContainerReviewScore(profileManager.Profile.GenerateID()); break;
                }
                inf.DisplayName = frm.EnteredName;
                // Add it to the collection, since we can't undo this we can add it directly
                profileManager.Profile.Consoles[base.consoleID].InformationContainers.Add(inf);
                // Make it visible
                if (!profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.AddNewContainerID(inf.ID))
                {
                    if (profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.ContainerIDS == null)
                        profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.ContainerIDS = new List<string>();
                    profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.ContainerIDS.Add(inf.ID);
                }
                // Raise event to set save request !
                profileManager.Profile.OnInformationContainerAdded(inf.DisplayName, profileManager.Profile.Consoles[base.consoleID].Name);
                // Refresh ..
                LoadSettings();
                // Fix columns
                profileManager.Profile.Consoles[consoleID].FixColumnsForRomDataInfo();
            }
        }
        // Delete
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeleteInformationContainer"], ls["MessageCaption_DeleteInformationContainer"]);
            if (result.ClickedButtonIndex == 0)// Yes
            {
                profileManager.Profile.Consoles[base.consoleID].DeleteInformationContainer((string)listView1.SelectedItems[0].Tag);
                // Refresh
                LoadSettings();
                // Fix columns
                profileManager.Profile.Consoles[consoleID].FixColumnsForRomDataInfo();
            }
        }
        // Rename
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            InformationContainer ic = profileManager.Profile.Consoles[base.consoleID].GetInformationContainer((string)listView1.SelectedItems[0].Tag);
            Form_EnterName frm = new Form_EnterName(ls["MessageCaption_EnterTheInformationContainerName"], ic.DisplayName, true, false);
            frm.OkPressed += frm_OkPressed;
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ic.DisplayName = listView1.SelectedItems[0].Text = frm.EnteredName;
                // Event for refresh request
                profileManager.Profile.OnInformationContainerVisibiltyChanged();
                // Fix columns
                profileManager.Profile.Consoles[consoleID].UpdateColumnName((string)listView1.SelectedItems[0].Tag, frm.EnteredName);
            }
        }
        private void frm_OkPressed(object sender, EnterNameFormOkPressedArgs e)
        {
            string ic = (string)listView1.SelectedItems[0].Tag;
            foreach (InformationContainer i in profileManager.Profile.Consoles[base.consoleID].InformationContainers)
            {
                if (i.ID != ic && i.DisplayName.ToLower() == e.NameEntered.ToLower())
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTaken"]);
                    e.Cancel = true;
                    break;
                }
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_show.Enabled = listView1.SelectedItems.Count == 1;
            button5.Enabled = button4.Enabled = listView1.SelectedItems.Count == 1;
            if (listView1.SelectedItems.Count == 1)
            {
                bool visible = profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.IsContainerVisible((string)listView1.SelectedItems[0].Tag);
                button_show.Text = visible ? ls["Button_Hide"] : ls["Button_Show"];
            }
            else
            {
                button_show.Text = "...";
            }
        }
        // Hide/Show
        private void button_show_Click(object sender, EventArgs e)
        {
            button_show.Enabled = listView1.SelectedItems.Count == 1;
            if (listView1.SelectedItems.Count == 1)
            {
                string id = (string)listView1.SelectedItems[0].Tag;
                bool visible = profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.IsContainerVisible(id);
                int index = listView1.SelectedItems[0].Index;
                if (visible)
                {
                    // Hide from map
                    profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.CloseContainerID(id);
                }
                else
                {
                    // Add it to map if not exist
                    if (!profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.AddNewContainerID(id))
                    {
                        if (profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.ContainerIDS == null)
                            profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.ContainerIDS = new List<string>();
                        profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.ContainerIDS.Add(id);
                    }

                }
                // Load
                LoadSettings();
                // Update button !
                listView1_SelectedIndexChanged(this, null);
                listView1.Items[index].Selected = true;
                listView1.Items[index].EnsureVisible();
                // Event for refresh request
                profileManager.Profile.OnInformationContainerVisibiltyChanged();
            }
        }
        // Move up !
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1 && listView1.Items.Count > 1)
            {
                // Get selected item
                string id = (string)listView1.SelectedItems[0].Tag;
                int index = profileManager.Profile.Consoles[base.consoleID].GetInformaitonContainerIndex(id);

                if ((index - 1) >= 0)
                {
                    // Get original
                    InformationContainer con = profileManager.Profile.Consoles[base.consoleID].InformationContainers[index];
                    // Remove it !
                    profileManager.Profile.Consoles[base.consoleID].InformationContainers.RemoveAt(index);
                    // Increament index
                    index--;
                    // Insert it again at the new index
                    profileManager.Profile.Consoles[base.consoleID].InformationContainers.Insert(index, con);
                    // Refresh
                    LoadSettings();
                    // Select the new one
                    listView1.Items[index].EnsureVisible();
                    listView1.Items[index].Selected = true;
                    // Raise event
                    profileManager.Profile.OnInformationContainerMoved(con.DisplayName);
                }
            }
        }
        // Move down !
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1 && listView1.Items.Count > 1)
            {
                // Get selected item
                string id = (string)listView1.SelectedItems[0].Tag;
                int index = profileManager.Profile.Consoles[base.consoleID].GetInformaitonContainerIndex(id);

                if ((index + 1) < profileManager.Profile.Consoles[base.consoleID].InformationContainers.Count)
                {
                    // Get original
                    InformationContainer con = profileManager.Profile.Consoles[base.consoleID].InformationContainers[index];
                    // Remove it !
                    profileManager.Profile.Consoles[base.consoleID].InformationContainers.RemoveAt(index);
                    // Increament index
                    index++;
                    // Insert it again at the new index
                    profileManager.Profile.Consoles[base.consoleID].InformationContainers.Insert(index, con);
                    // Refresh
                    LoadSettings();
                    // Select the new one
                    listView1.Items[index].EnsureVisible();
                    listView1.Items[index].Selected = true;
                    // Raise event
                    profileManager.Profile.OnInformationContainerMoved(con.DisplayName);
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (odd = !odd)
            {
                label1.ForeColor = Color.Navy;
            }
            else
            {
                label1.ForeColor = Color.Black;
            }
        }
        // Reset tabs layout
        private void button6_Click(object sender, EventArgs e)
        {
            profileManager.Profile.Consoles[base.consoleID].InformationContainersMap =
                new InformationContainerTabsPanel();
            foreach (InformationContainer ic in profileManager.Profile.Consoles[base.consoleID].InformationContainers)
            {
                profileManager.Profile.Consoles[base.consoleID].InformationContainersMap.ContainerIDS.Add(ic.ID);
            }
            // Event for refresh request
            profileManager.Profile.OnInformationContainerVisibiltyChanged();
        }
    }
}
