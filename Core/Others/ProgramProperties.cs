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

namespace EmulatorsOrganizer.Core
{
    [Serializable()]
    public class ProgramProperties
    {
        private string programName;
        private string programPath;
        private string arguments;
        private bool batMode;
        private string batScript;
        private ProgramStartMode startMode;
        private int waitSeconds;

        public string ProgramName
        { get { return programName; } set { programName = value; } }
        public string ProgramPath
        { get { return programPath; } set { programPath = value; } }
        public string Arguments
        { get { return arguments; } set { arguments = value; } }
        public string BatScript
        { get { return batScript; } set { batScript = value; } }
        public bool BatMode
        { get { return batMode; } set { batMode = value; } }
        public ProgramStartMode StartMode
        {
            get { return startMode; }
            set { startMode = value; }
        }
        public int WaitSeconds
        { get { return waitSeconds; } set { waitSeconds = value; } }
    }
}
