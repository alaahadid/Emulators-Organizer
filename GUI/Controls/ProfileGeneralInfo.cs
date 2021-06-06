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
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;

namespace EmulatorsOrganizer.GUI
{
    public partial class ProfileGeneralInfo : UserControl
    {
        public ProfileGeneralInfo()
        {
            InitializeComponent();
            // TODO: add more general profile info ..
            // Load info
            ListViewItem item = new ListViewItem();
            item.Text = ls["Property_ProfileName"];
            item.SubItems.Add(profileManager.Profile.Name);
            listView1.Items.Add(item);

            item = new ListViewItem();
            item.Text = ls["Property_TotalConsoleGroups"];
            item.SubItems.Add(profileManager.Profile.ConsoleGroups.Count.ToString() + " group");
            listView1.Items.Add(item);

            item = new ListViewItem();
            item.Text = ls["Property_TotalConsoles"];
            item.SubItems.Add(profileManager.Profile.Consoles.Count.ToString() + " console");
            listView1.Items.Add(item);

            item = new ListViewItem();
            item.Text = ls["Property_TotalPlaylistGroups"];
            item.SubItems.Add(profileManager.Profile.PlaylistGroups.Count.ToString() + " groups");
            listView1.Items.Add(item);

            item = new ListViewItem();
            item.Text = ls["Property_TotalPlaylists"];
            item.SubItems.Add(profileManager.Profile.Playlists.Count.ToString() + " playlist");
            listView1.Items.Add(item);

            item = new ListViewItem();
            item.Text = ls["Property_TotalEmulators"];
            item.SubItems.Add(profileManager.Profile.Emulators.Count.ToString() + " emulator");
            listView1.Items.Add(item);

            item = new ListViewItem();
            item.Text = ls["Property_TotalRoms"];
            // Get the count
            item.SubItems.Add(profileManager.Profile.Roms.TotalRomsCount.ToString() + " rom");
            listView1.Items.Add(item);
        }
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
    }
}
