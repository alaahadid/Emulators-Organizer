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

namespace EmulatorsOrganizer.Core
{
    [Serializable]
    public class RomData
    {
        public RomData(string id, string name, RomDataType type)
        {
            this.id = id;
            this.name = name;
            this.type = type;
        }

        private string id;
        private string name;
        private RomDataType type;

        /// <summary>
        /// Get or set the id of this data
        /// </summary>
        public string ID
        { get { return id; } set { id = value; } }
        /// <summary>
        /// Get or set the name of this data
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the type of this data
        /// </summary>
        public RomDataType Type
        { get { return type; } set { type = value; } }

        /// <summary>
        /// RomData.ToString()
        /// </summary>
        /// <returns>Name of this data info</returns>
        public override string ToString()
        {
            return name;
        }
    }
}
