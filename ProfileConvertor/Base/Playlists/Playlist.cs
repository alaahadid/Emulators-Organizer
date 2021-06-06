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
using System.Drawing;
using System.Xml.Serialization;
namespace AHD.EO.Base
{
    [Serializable()]
    [XmlInclude(typeof(Bitmap))]
    public class Playlist
    {
        public Playlist()
        { }
        public Playlist(string name)
        { this.name = name; }
        private Image icon;
        private string name = "";
        private List<Rom> roms = new List<Rom>();

        /// <summary>
        /// Get or set the playlist roms collection
        /// </summary>
        public List<Rom> Roms
        {
            get { return roms; }
            set { roms = value; }
        }

        /// <summary>
        /// Get or set the name of this group
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Get or set the playlist icon
        /// </summary>
        [XmlIgnore()]
        public Image Icon
        { get { return icon; } set { icon = value; } }

        /// <summary>
        /// Check if this playlist ahas a rom
        /// </summary>
        /// <param name="rom">The rom to check</param>
        /// <returns>True if this rom existed, otherwiser false</returns>
        public bool IsRomExist(Rom rom)
        {
            foreach (Rom trom in roms)
            {
                if (trom.Name == rom.Name && trom.Path == rom.Path && trom.ConsoleID == rom.ConsoleID)
                    return true;
            }
            return false;
        }
    }
}
