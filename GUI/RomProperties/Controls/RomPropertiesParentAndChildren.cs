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
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesChildren : IRomPropertiesControl
    {
        public RomPropertiesChildren()
        {
            InitializeComponent();
        }
        private bool oldParentStatus;
        public override string ToString()
        {
            return ls["Title_ParentAndChildren"];
        }
        public override string Description
        {
            get
            {
                return ls["RomPropertiesDescription_ParentAndChildren"];
            }
        }

        private void OnClosing()
        {
            if (profileManager.Profile.Roms[romID].IsParentRom && profileManager.Profile.Roms[romID].ChildrenRoms.Count == 0)
            {
                // Canceling without setting up parent, cancel parent status
                profileManager.Profile.Roms[romID].IsParentRom = false;
                profileManager.Profile.Roms[romID].AlwaysChooseChildWhenPlay = false;
            }
        }
        private void RefreshChildren()
        {
            listView1.Items.Clear();
            if (profileManager.Profile.Roms[romID].IsParentRom || checkBox_isParent.Checked)
            {
                Rom[] roms = profileManager.Profile.Roms.GetChildrenOf(romID);

                for (int i = 0; i < roms.Length; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = roms[i].Name;
                    item.SubItems.Add(roms[i].Path);
                    item.Tag = roms[i].ID;
                    listView1.Items.Add(item);
                }
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                if (checkBox_isParent.Checked && profileManager.Profile.Roms[romID].ChildrenRoms.Count == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ParentRomMustHaveCHILD"]);
                    return false;
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            oldParentStatus = checkBox_isParent.Checked = profileManager.Profile.Roms[romID].IsParentRom;
            checkBox_isChild.Checked = profileManager.Profile.Roms[romID].IsChildRom;
            checkBox_alwaysPickChild.Checked = profileManager.Profile.Roms[romID].AlwaysChooseChildWhenPlay;
            RefreshChildren();
            if (checkBox_isChild.Checked)
            {
                checkBox_isParent.Enabled = false;
                checkBox_alwaysPickChild.Enabled = false;
                groupBox1.Enabled = false;
            }
            else
            {
                checkBox_isParent.Enabled = true;
                checkBox_alwaysPickChild.Enabled = true;
                groupBox1.Enabled = true;
            }
        }
        public override void SaveSettings()
        {
            if (!checkBox_isChild.Checked)
            {
                profileManager.Profile.Roms[romID].IsParentRom = checkBox_isParent.Checked;
                profileManager.Profile.Roms[romID].AlwaysChooseChildWhenPlay = checkBox_alwaysPickChild.Checked;
                // Do important clear
                if (oldParentStatus && !checkBox_isParent.Checked)
                {
                    // No longer a parent ? remove all childish relationships.
                    foreach (string child in profileManager.Profile.Roms[romID].ChildrenRoms)
                    {
                        profileManager.Profile.Roms[child].IsChildRom = false;
                        profileManager.Profile.Roms[child].ParentRomID = "";
                    }
                    profileManager.Profile.Roms[romID].ChildrenRoms = new List<string>();
                    base.RomsRefreshRequired = true;
                }
            }
        }
        public override void DefaultSettings()
        {
            if (!checkBox_isChild.Checked)
            {
                checkBox_isParent.Checked = false;
                checkBox_alwaysPickChild.Checked = false;
            }
        }
        // Remove children
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ManagedMessageBoxResult res =
                    ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureyouWantToRemoveChildren"],
                   ls["MessageCaption_RemoveChildren"]);

                if (res.ClickedButtonIndex == 0)// Yes
                {
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        // Remove it from rom children list;
                        profileManager.Profile.Roms[romID].ChildrenRoms.Remove(item.Tag.ToString());
                        // Tell that rom that it is no longer a child !
                        profileManager.Profile.Roms[item.Tag.ToString()].IsChildRom = false;
                        profileManager.Profile.Roms[item.Tag.ToString()].ParentRomID = "";
                    }
                    // Refresh the list !
                    RefreshChildren();
                    base.RomsRefreshRequired = true;
                }
            }
        }
        // Add roms as children
        private void button1_Click(object sender, EventArgs e)
        {
            string parentConsoleID = profileManager.Profile.Roms[romID].ParentConsoleID;
            List<Rom> romsToCheck = new List<Rom>(profileManager.Profile.Roms.GetChildrenRoms(parentConsoleID));
            romsToCheck.AddRange(profileManager.Profile.Roms.GetSingleRoms(parentConsoleID));
            FormChildPick frm = new FormChildPick(romsToCheck.ToArray(), true);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(ls["Message_AddChildrenWarning"]);
                if (res.ClickedButtonIndex == 0)
                {
                    // 1 Get the new children list
                    string[] newChildrenList = frm.SelectedRomIDS;
                    // 2 Do the loop
                    foreach (string newChild in newChildrenList)
                    {
                        // 3 Check if this child is already belong to this rom
                        if (profileManager.Profile.Roms[romID].ChildrenRoms.Contains(newChild))
                        {
                            continue;// Nothing to do ...
                        }
                        // 4 This rom is not a part of the family, Check if it is already belong to another family
                        Rom pRom = profileManager.Profile.Roms[newChild];

                        if (pRom.IsChildRom)
                        {
                            // Remove the relationship!
                            profileManager.Profile.Roms[pRom.ParentRomID].ChildrenRoms.Remove(newChild);
                            profileManager.Profile.Roms[pRom.ParentRomID].Modified = true;
                            // Check out the parent situation
                            if (profileManager.Profile.Roms[pRom.ParentRomID].ChildrenRoms.Count == 0)
                            {
                                // no children, no parent anymore :(
                                profileManager.Profile.Roms[pRom.ParentRomID].IsParentRom = false;
                                profileManager.Profile.Roms[pRom.ParentRomID].AlwaysChooseChildWhenPlay = false;
                            }
                        }

                        // 5 Add the child to current rom
                        profileManager.Profile.Roms[romID].ChildrenRoms.Add(newChild);
                        // 6 Mark the new parent in the child's head
                        profileManager.Profile.Roms[newChild].IsChildRom = true;
                        profileManager.Profile.Roms[newChild].ParentRomID = romID;
                    }
                    // 7 Refresh
                    RefreshChildren();
                    base.RomsRefreshRequired = true;
                }
            }
        }
        // Move up
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            // Get selected item index
            int selectedIndex = listView1.SelectedItems[0].Index;
            // Check
            if (selectedIndex - 1 < 0)
                return;
            // Do it !
            // Remove the item from the list
            ListViewItem item = listView1.SelectedItems[0];
            listView1.Items.RemoveAt(selectedIndex);
            // Insert it !
            listView1.Items.Insert(selectedIndex - 1, item);
            // Select none
            foreach (ListViewItem it in listView1.Items)
                it.Selected = false;
            // Select ours
            listView1.Items[selectedIndex - 1].Selected = true;
            listView1.Items[selectedIndex - 1].EnsureVisible();
            // Apply to rom list
            profileManager.Profile.Roms[romID].ChildrenRoms = new List<string>();
            foreach (ListViewItem newItem in listView1.Items)
                profileManager.Profile.Roms[romID].ChildrenRoms.Add(newItem.Tag.ToString());
            profileManager.Profile.Roms[romID].Modified = true;
            base.RomsRefreshRequired = true;
        }
        // Move down
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            // Get selected item index
            int selectedIndex = listView1.SelectedItems[0].Index;
            // Check
            if (selectedIndex + 1 >= listView1.Items.Count)
                return;
            // Do it !
            // Remove the item from the list
            ListViewItem item = listView1.SelectedItems[0];
            listView1.Items.RemoveAt(selectedIndex);
            // Insert it !
            listView1.Items.Insert(selectedIndex + 1, item);
            // Select none
            foreach (ListViewItem it in listView1.Items)
                it.Selected = false;
            // Select ours
            listView1.Items[selectedIndex + 1].Selected = true;
            listView1.Items[selectedIndex + 1].EnsureVisible();
            // Apply to rom list
            profileManager.Profile.Roms[romID].ChildrenRoms = new List<string>();
            foreach (ListViewItem newItem in listView1.Items)
                profileManager.Profile.Roms[romID].ChildrenRoms.Add(newItem.Tag.ToString());
            base.RomsRefreshRequired = true;
        }
    }
}
