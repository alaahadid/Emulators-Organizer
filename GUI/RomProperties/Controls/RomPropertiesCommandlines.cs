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

namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesCommandlines : IRomPropertiesControl
    {
        public RomPropertiesCommandlines()
        {
            InitializeComponent();
        }
        private bool isLoading;
        public override string ToString()
        {
            return ls["Title_Commandlines"];
        }
        public override string Description
        {
            get
            {
                return ls["RomPropertiesDescription_Commandlines"];
            }
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            isLoading = true;
            // Load all emulator related to the parent console of this rom
            string parentConsole = profileManager.Profile.Roms[romID].ParentConsoleID;
            Emulator[] emulators = profileManager.Profile.Emulators[parentConsole, false];
            if (emulators != null)
            {
                foreach (Emulator e in emulators)
                {
                    comboBox_emulators.Items.Add(e);
                }
                if (comboBox_emulators.Items.Count > 0)
                    comboBox_emulators.SelectedIndex = 0;
            }
            isLoading = false;
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            SaveCurrent();
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        private void SaveCurrent()
        {
            if (isLoading) return;
            if (comboBox_emulators.SelectedIndex >= 0)
            {
                profileManager.Profile.Roms[romID].UpdateEmulatorCommandlines(((Emulator)comboBox_emulators.SelectedItem).ID,
                    commandlinesEditor1.CommandlineGroups);
            }
        }
        private void comboBox_emulators_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrent();
            // Load new
            commandlinesEditor1.CommandlineGroups.Clear();
            commandlinesEditor1.Enabled = comboBox_emulators.SelectedIndex >= 0;
            if (comboBox_emulators.SelectedIndex >= 0)
            {
                commandlinesEditor1.CommandlineGroups =
                    profileManager.Profile.Roms[romID].GetCommandlinesGroupsForEmulator(((Emulator)comboBox_emulators.SelectedItem).ID);
            }
        }
    }
}
