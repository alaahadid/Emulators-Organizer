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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesCategories : IRomPropertiesControl
    {
        public RomPropertiesCategories()
        {
            InitializeComponent();
        }

        private string[] oldCats;

        public override string ToString()
        {
            return ls["Title_Categories"];
        }
        public override string Description
        {
            get
            {
                return ls["RomPropertiesDescription_Categories"];
            }
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            oldCats = profileManager.Profile.Roms[romID].Categories.ToArray();
            listBox_categories.Items.Clear();
            foreach (string cat in oldCats)
            {
                listBox_categories.Items.Add(cat);
            }
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            profileManager.Profile.Roms[romID].Categories = new List<string>();
            foreach (string cat in listBox_categories.Items)
            {
                profileManager.Profile.Roms[romID].Categories.Add(cat);
            }
            profileManager.Profile.Roms[romID].Modified = true;
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
            listBox_categories.Items.Clear();
            foreach (string cat in oldCats)
            {
                listBox_categories.Items.Add(cat);
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        // Add new
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
    }
}
