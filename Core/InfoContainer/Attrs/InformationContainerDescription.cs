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
    public class InformationContainerDescription : Attribute
    {
        /// <summary>
        /// Indicate information container basic information.
        /// </summary>
        /// <param name="name">The name of this container</param>
        /// <param name="columnable">Indicates if this information container can be showed as column in roms list</param>
        /// <param name="dectable">Indicates if this information container can be detect</param>
        public InformationContainerDescription(string name, bool columnable, bool dectable)
        {
            this.name = name;
            this.columnable = columnable;
            this.dectable = dectable;
        }
        private string name;
        private bool columnable;
        private bool dectable;

        /// <summary>
        /// Get the name of this container
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// Get if this information container can be showed as column in roms list
        /// </summary>
        public bool Columnable { get { return columnable; } }
        /// <summary>
        /// Indicates if this information container can be detect
        /// </summary>
        public bool Dectable { get { return dectable; } }
    }
}
