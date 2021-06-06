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
    /// <summary>
    /// Database file info attributes
    /// </summary>
    public class DatabaseInfoAttribute : Attribute
    {
        /// <summary>
        /// Database file info attributes; This database will not be Exportable, Separable nor SupportPerfectMatch
        /// </summary>
        /// <param name="name">The database file name</param>
        /// <param name="extensions">The database file extensions</param>
        /// <param name="mode">The database file comparing modes</param>
        public DatabaseInfoAttribute(string name, string[] extensions, CompareMode mode)
        {
            this.Name = name;
            this.Extensions = extensions;
            this.ComparingMode = mode;
            this.Exportable = false;
            this.SupportPerfectMatch = false;
            this.Separable = false;
        }
        /// <summary>
        /// Database file info attributes
        /// </summary>
        /// <param name="name">The database file name</param>
        /// <param name="extensions">The database file extensions</param>
        /// <param name="mode">The database file comparing modes</param>
        /// <param name="exprotable">Indicates if this database file support exporting</param>
        /// <param name="separable">Indicates if this format can be separated</param>
        /// <param name="supportPerfectMatch">Indicates if this format support perfect match option</param>
        public DatabaseInfoAttribute(string name, string[] extensions, CompareMode mode, bool exprotable,
            bool separable, bool supportPerfectMatch)
        {
            this.Name = name;
            this.Extensions = extensions;
            this.ComparingMode = mode;
            this.Exportable = exprotable;
            this.SupportPerfectMatch = supportPerfectMatch;
            this.Separable = separable;
        }
        /// <summary>
        /// Get the database file name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Get the database file extensions
        /// </summary>
        public string[] Extensions { get; private set; }
        /// <summary>
        /// Get the database file comparing modes
        /// </summary>
        public CompareMode ComparingMode { get; private set; }
        /// <summary>
        /// Indicates if this database file support exporting
        /// </summary>
        public bool Exportable { get; private set; }
        /// <summary>
        /// Indicates if this format support perfect match option
        /// </summary>
        public bool SupportPerfectMatch { get; private set; }
        /// <summary>
        /// Indicates if this database can be separated. Separable
        /// </summary>
        public bool Separable { get; private set; }
    }
}
