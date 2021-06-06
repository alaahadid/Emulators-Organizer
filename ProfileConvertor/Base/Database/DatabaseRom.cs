/* This file is part of Emulators Organizer
   A program that can organize roms and emulators

   Copyright © Ali Hadid and Ala Hadid 2009 - 2013

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHD.EO.Base
{
    public struct DatabaseRom
    {
        public DatabaseRom(string name, string description, string path, List<string> fileNames,
            string sha1, string crc, string crcNoSkip, string md5, List<string> additions, List<DBProperty> properties, string data)
        {
            this.name = name;
            this.description = description;
            this.sha1 = sha1;
            this.crc = crc;
            this.crcNoSkip = crcNoSkip;
            this.md5 = md5;
            this.path = path;
            this.fileNames = fileNames;
            this.additions = additions;
            this.properties = properties;
            this.data = data;
        }
        private string name;
        private string description;
        private string sha1;
        private string crc;
        private string crcNoSkip;
        private string md5;
        private string path;
        private string data;
        private List<string> fileNames;
        private List<string> additions;
        private List<DBProperty> properties;

        /// <summary>
        /// Get or set the Name of this rom
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the rom file name, useed for "file name" compare feature
        /// </summary>
        public List<string> FileNames
        { get { return fileNames; } set { fileNames = value; } }
        /// <summary>
        /// Get or set the Description of this rom
        /// </summary>
        public string Description
        { get { return description; } set { description = value; } }
        /// <summary>
        /// Get or set the SHA1 of this rom
        /// </summary>
        public string SHA1
        { get { return sha1; } set { sha1 = value; } }
        /// <summary>
        /// Get or set the SHA1 of this rom
        /// </summary>
        public string CRC
        { get { return crc; } set { crc = value; } }
        public string CRCNoSkip
        { get { return crcNoSkip; } set { crcNoSkip = value; } }
        /// <summary>
        /// Get or set the SHA1 of this rom
        /// </summary>
        public string MD5
        { get { return md5; } set { md5 = value; } }
        /// <summary>
        /// Get or set the path of rom, this used only for save
        /// </summary>
        public string Path
        { get { return path; } set { path = value; } }
        /// <summary>
        /// Get or set the list of addidtion strings
        /// </summary>
        public List<string> Additions
        { get { return additions; } set { additions = value; } }
        /// <summary>
        /// Get the properties of this rom loaded from database
        /// </summary>
        public List<DBProperty> Properties
        { get { return properties; } set { properties = value; } }
        /// <summary>
        /// Get or set the data of this rom as read from database.
        /// </summary>
        public string Data
        { get { return data; } set { data = value; } }
        public static DatabaseRom Empty = new DatabaseRom("", "", "", new List<string>(),
            "", "", "", "", new List<string>(), new List<DBProperty>(), "");
    }
}
