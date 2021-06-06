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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesMultibleEdit : Form
    {
        public RomPropertiesMultibleEdit(string[] romIDs, string[] addCategories)
        {
            InitializeComponent();
            this.romIDs = romIDs;
            // Load categories
            for (int i = 0; i < romIDs.Length; i++)
            {
                string[] cats = profileManager.Profile.Roms[romIDs[i]].Categories.ToArray();
                foreach (string cat in cats)
                {
                    if (!listBox_categories.Items.Contains(cat))
                        listBox_categories.Items.Add(cat);
                }
            }
            if (addCategories != null)
            {
                for (int i = 0; i < addCategories.Length; i++)
                {
                    if (!listBox_categories.Items.Contains(addCategories[i]))
                        listBox_categories.Items.Add(addCategories[i]);

                }
            }
        }
        private string[] romIDs;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        // Add
        private void button2_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName("Enter category", "", true, false);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                if (!listBox_categories.Items.Contains(frm.EnteredName))
                    listBox_categories.Items.Add(frm.EnteredName);
            }
        }
        // Remove
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox_categories.SelectedIndex >= 0)
                listBox_categories.Items.RemoveAt(listBox_categories.SelectedIndex);
        }
        // Close
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Ok
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < romIDs.Length; i++)
            {
                profileManager.Profile.Roms[romIDs[i]].Categories = new List<string>();
                foreach (string cat in listBox_categories.Items)
                {
                    profileManager.Profile.Roms[romIDs[i]].Categories.Add(cat);
                }
                profileManager.Profile.Roms[romIDs[i]].Modified = true;
            }
            profileManager.Profile.OnRomMultiplePropertiesChanged();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox_categories.Items.Clear();
        }
    }
}
