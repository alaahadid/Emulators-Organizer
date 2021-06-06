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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// A class represents emulator
    /// </summary>
    [Serializable()]
    public class Emulator : IEOElement
    {
        /// <summary>
        /// A class represents emulator
        /// </summary>
        /// <param name="name">The emulator name</param>
        /// <param name="id">The emulator id</param>
        public Emulator(string name, string id)
            : base(name, id)
        {
        }
        /// <summary>
        /// A class represents emulator
        /// </summary>
        /// <param name="id">The emulator id</param>
        public Emulator(string id)
            : base("", id)
        {
        }

        private string excutablePath = "";
        private bool batMod = false;
        private string batScript = "";
        private List<EmulatorParentConsole> parentConsoles = new List<EmulatorParentConsole>();
        private List<ProgramProperties> programsToLaunchBefore = new List<ProgramProperties>();
        private List<ProgramProperties> programsToLaunchAfter = new List<ProgramProperties>();
        /*This collection should be updated for each console associated*/
        /// <summary>
        /// Get or set the parent consoles collection
        /// </summary>
        public List<EmulatorParentConsole> ParentConsoles
        { get { return parentConsoles; } set { parentConsoles = value; } }
        public List<ProgramProperties> ProgramsToLaucnhBefore
        { get { return programsToLaunchBefore; } set { programsToLaunchBefore = value; } }
        public List<ProgramProperties> ProgramsToLaucnhAfter
        { get { return programsToLaunchAfter; } set { programsToLaunchAfter = value; } }

        public bool BatMode { get { return batMod; } set { batMod = value; } }
        public string BatScript { get { return batScript; } set { batScript = value; } }
        /// <summary>
        /// Get or set the emulator's excutable file path
        /// </summary>
        public string ExcutablePath
        { get { return excutablePath; } set { excutablePath = value; } }
        /// <summary>
        /// Get if this emulator is supported for given console
        /// </summary>
        /// <param name="consoleID">The console id to check</param>
        /// <returns>True if given console is supported by this emulator</returns>
        public bool IsConsoleSupported(string consoleID)
        {
            if (parentConsoles == null)
                parentConsoles = new List<EmulatorParentConsole>();
            foreach (EmulatorParentConsole p in parentConsoles)
            {
                if (p.ConsoleID == consoleID) { return true; }
            }
            return false;
        }
        /// <summary>
        /// Get command-line groups for given console if supported.
        /// </summary>
        /// <param name="consoleID">The console id</param>
        /// <returns>List of command-line groups for given console if supported otherwise null</returns>
        public List<CommandlinesGroup> GetCommandlinesGroupsForConsole(string consoleID)
        {
            if (parentConsoles == null)
                parentConsoles = new List<EmulatorParentConsole>();
            foreach (EmulatorParentConsole p in parentConsoles)
            {
                if (p.ConsoleID == consoleID) { return p.CommandlineGroups; }
            }
            return null;
        }
        /// <summary>
        /// Get parent index
        /// </summary>
        /// <param name="consoleID">The console id to use</param>
        /// <returns></returns>
        public int GetParentIndex(string consoleID)
        {
            if (parentConsoles == null)
                parentConsoles = new List<EmulatorParentConsole>();
            for (int i = 0; i < parentConsoles.Count; i++)
            {
                if (parentConsoles[i].ConsoleID == consoleID) { return i; }
            }
            return -1;
        }
        public void RemoveParent(string consoleID)
        {
            if (parentConsoles == null)
                parentConsoles = new List<EmulatorParentConsole>();
            for (int i = 0; i < parentConsoles.Count; i++)
            {
                if (parentConsoles[i].ConsoleID == consoleID)
                {
                    parentConsoles.RemoveAt(i); i--;
                }
            }
        }
    }
}
