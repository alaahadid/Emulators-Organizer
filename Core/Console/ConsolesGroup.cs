﻿// This file is part of Emulators Organizer
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

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Represents group of consoles.
    /// </summary>
    [Serializable]
    public class ConsolesGroup : IEOElement
    {
        /// <summary>
        /// Represents group of consoles.
        /// </summary>
        /// <param name="name">The consoles group name</param>
        /// <param name="id">The consoles group id</param>
        public ConsolesGroup(string name, string id)
            : base(name, id)
        {
            base.BuildDefaultColumns(null);
            enableCommandlines = enableEmulator = true;
        }
        /// <summary>
        /// Represents group of consoles.
        /// </summary>
        /// <param name="id">The consoles group id</param>
        public ConsolesGroup(string id)
            : base("", id)
        {
            base.BuildDefaultColumns(null);
            enableCommandlines = enableEmulator = true;
        }
    }
}
