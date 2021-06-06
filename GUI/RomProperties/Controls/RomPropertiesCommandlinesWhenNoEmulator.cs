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
using System.Collections.Generic;
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesCommandlinesWhenNoEmulator : IRomPropertiesControl
    {
        public RomPropertiesCommandlinesWhenNoEmulator()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return ls["Title_LaunchOptions"];
        }
        public override string Description
        {
            get
            {
                return ls["RomPropertiesDescription_LaunchOptions"];
            }
        }

        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        public override void LoadSettings()
        {
            commandlinesEditor1.CommandlineGroups =
                profileManager.Profile.Roms[romID].CommandlineGroupsWhenExecutingWithoutEmulator;
        }
        public override void SaveSettings()
        {
            Rom rom = profileManager.Profile.Roms[romID];

            rom.CommandlineGroupsWhenExecutingWithoutEmulator = new List<CommandlinesGroup>();
            foreach (CommandlinesGroup gr in commandlinesEditor1.CommandlineGroups)
                rom.CommandlineGroupsWhenExecutingWithoutEmulator.Add(gr.Clone());
        }
        public override void DefaultSettings()
        {
            commandlinesEditor1.CommandlineGroups.Clear();
        }
    }
}
