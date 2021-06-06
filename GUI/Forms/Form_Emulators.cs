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
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_Emulators : Form
    {
        public Form_Emulators()
        {
            InitializeComponent();
            profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
            // List emulators
            RefreshEmulators();
            // Events
            profileManager.Profile.EmulatorAdded += Profile_EmulatorAdded;
            profileManager.Profile.EmulatorRemoved += Profile_EmulatorAdded;
            profileManager.Profile.EmulatorPropertiesChanged += Profile_EmulatorAdded;
        }

        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");

        private void RefreshEmulators()
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();
            int i = 0;
            foreach (Emulator emu in profileManager.Profile.Emulators)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = emu.ID;
                if (emu.Icon != null)
                    imageList1.Images.Add(emu.Icon);
                else
                    imageList1.Images.Add(Properties.Resources.application);
                item.ImageIndex = i;
                item.Text = emu.Name;
                item.SubItems.Add(emu.ID);

                string consoles = "";
                foreach (EmulatorParentConsole p in emu.ParentConsoles)
                    consoles += profileManager.Profile.Consoles[p.ConsoleID].Name + ", ";
                if (consoles.Length > 2)
                    consoles = consoles.Substring(0, consoles.Length - 2);

                item.SubItems.Add(consoles);
                item.Tag = emu.ID;
                listView1.Items.Add(item);
                i++;
            }
        }
        // close
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Add
        private void button1_Click(object sender, EventArgs e)
        {
            Form_AddEmulator frm = new Form_AddEmulator(true);
            frm.ShowDialog(this);
        }
        private void Profile_EmulatorAdded(object sender, EventArgs e)
        {
            RefreshEmulators();
        }
        // Remove
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeleteEmulators"],
            ls["MessageCaption_DeleteEmulator"]);
            if (result.ClickedButtonIndex == 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    profileManager.Profile.Emulators.Remove((string)item.Tag);
                }
                RefreshEmulators();
            }
        }
        // Properties
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            EmulatorProperties frm = new EmulatorProperties((string)listView1.SelectedItems[0].Tag,
                ls["Title_General"]);
            frm.ShowDialog(this);
        }
        // Assign consoles
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            EmulatorProperties frm = new EmulatorProperties((string)listView1.SelectedItems[0].Tag,
                ls["Title_ParentConsoles"]);
            frm.ShowDialog(this);
        }
    }
}
