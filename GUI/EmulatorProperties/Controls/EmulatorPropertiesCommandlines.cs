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
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class EmulatorPropertiesCommandlines : IEmulatorPropertiesControl
    {
        public EmulatorPropertiesCommandlines()
        {
            InitializeComponent();
            commandlinesEditor1.AllowEdit = true;
        }

        private string currentConsoleID;

        public override string ToString()
        {
            return ls["Title_Commandlines"];
        }
        public override string Description
        {
            get
            {
                return ls["EmulatorPropertiesDescription_Commandlines"];
            }
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            // List consoles
            foreach (EmulatorParentConsole p in profileManager.Profile.Emulators[emulatorID].ParentConsoles)
            {
                comboBox_consoles.Items.Add(profileManager.Profile.Consoles[p.ConsoleID].Name);
            }
            if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
            {
                comboBox_consoles.SelectedItem = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Name;
            }
            else
                comboBox_consoles.SelectedIndex = 0;
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            SaveCurrentConsoleCommandlines();
        }
        public override void CancelSettings()
        {
            base.CancelSettings();
            SaveCurrentConsoleCommandlines();
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
            //commandlinesEditor1.Reset();
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        // when the user changes the console
        private void comboBox_consoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save for current console if found
            SaveCurrentConsoleCommandlines();
            // Get commandlines group for selected console
            currentConsoleID = profileManager.Profile.Emulators[emulatorID].ParentConsoles[comboBox_consoles.SelectedIndex].ConsoleID;
            commandlinesEditor1.CommandlineGroups = profileManager.Profile.Emulators[emulatorID].GetCommandlinesGroupsForConsole(currentConsoleID);
        }
        private void SaveCurrentConsoleCommandlines()
        {
            if (profileManager.Profile.Emulators[emulatorID].IsConsoleSupported(currentConsoleID)) // make sure
            {
                int index = profileManager.Profile.Emulators[emulatorID].GetParentIndex(currentConsoleID);
                profileManager.Profile.Emulators[emulatorID].ParentConsoles[index].CommandlineGroups =
                    new List<Core.CommandlinesGroup>();

                foreach (CommandlinesGroup gr in commandlinesEditor1.CommandlineGroups)
                    profileManager.Profile.Emulators[emulatorID].ParentConsoles[index].CommandlineGroups.Add(gr.Clone());
            }
        }
    }
}
