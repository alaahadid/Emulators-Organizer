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
    public class PlaylistsGroup
    {
        public PlaylistsGroup()
        { }
        public PlaylistsGroup(string name)
        { this.name = name; }
        private Image icon;
        private string name = "";
        private List<Playlist> playlists = new List<Playlist>();
        /// <summary>
        /// Get or set the playlists collection
        /// </summary>
        public List<Playlist> PlayLists
        { get { return playlists; } set { playlists = value; } }
        /// <summary>
        /// Get or set the name of this group
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the playlists group icon
        /// </summary>
        [XmlIgnore()]
        public Image Icon
        { get { return icon; } set { icon = value; } }

        public bool IsPlaylistExist(string name)
        {
            foreach (Playlist pl in playlists)
                if (pl.Name.ToLower() == name.ToLower())
                    return true;
            return false;
        }
    }
}
