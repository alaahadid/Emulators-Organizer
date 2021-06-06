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
namespace EmulatorsOrganizer.Core
{
    [Serializable]
    public class RomEmulatorParentCommandlines
    {
        public RomEmulatorParentCommandlines(string parentEmulatorID)
        {
            this.parentEmulatorID = parentEmulatorID;
            BuildDefaultCommandlines();
        }
        private List<CommandlinesGroup> commandlineGroups = new List<CommandlinesGroup>();
        private string parentEmulatorID;

        /// <summary>
        /// Get or set the emulator id associated
        /// </summary>
        public string EmulatorID { get { return parentEmulatorID; } set { parentEmulatorID = value; } }
        /// <summary>
        /// Get or set the commandline groups for assigned emulator
        /// </summary>
        public List<CommandlinesGroup> CommandlineGroups
        { get { return commandlineGroups; } set { commandlineGroups = value; } }
        /// <summary>
        /// Build default commandlines for this rom
        /// </summary>
        public void BuildDefaultCommandlines()
        {
            if (!IsDefaultGroupExist())
            {
                CommandlinesGroup gr = new CommandlinesGroup();
                gr.Name = "<default>";
                gr.IsReadOnly = true;
                gr.Enabled = true;

                Commandline cm = new Commandline();
                cm.Name = "<default>";
                cm.Code = "<rompath>";
                cm.Enabled = true;
                cm.IsReadOnly = true;
                gr.Commandlines.Add(cm);

                commandlineGroups.Add(gr);
            }
        }
        private bool IsDefaultGroupExist()
        {
            foreach (CommandlinesGroup gr in commandlineGroups)
            {
                if (gr.IsReadOnly && gr.Name == "<default>")
                    return true;
            }
            return false;
        }
    }
}
