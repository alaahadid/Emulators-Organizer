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
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Class represents playlist
    /// </summary>
    [Serializable()]
    public class Playlist : IEOElement
    {
        /// <summary>
        /// A class represents playlist
        /// </summary>
        /// <param name="name">The playlist name</param>
        /// <param name="id">The playlist id</param>
        /// <param name="parentID">The parent playlists groups id. Set to "" to add the playlist to no group</param>
        public Playlist(string name, string id, string parentID)
            : base(name, id)
        {
            this.parentID = parentID;
            base.BuildDefaultColumns(null);
            enableCommandlines = enableEmulator = true;
        }
        /// <summary>
        /// A class represents console
        /// </summary>
        /// <param name="id">The console id</param>
        /// <param name="parentID">The parent consoles groups id. Set to "" to add the playlist to no group</param>
        public Playlist(string id, string parentID)
            : base("", id)
        {
            this.parentID = parentID;
            base.BuildDefaultColumns(null);
            enableCommandlines = enableEmulator = true;
        }

        private string parentID = "";
        private List<string> romIDS = new List<string>();

        /// <summary>
        /// Get or set parent playlists group id
        /// </summary>
        public string ParentGroupID
        { get { return parentID; } set { parentID = value; } }
        /// <summary>
        /// Get or set the roms collection
        /// </summary>
        public List<string> RomIDS
        { get { return romIDS; } set { romIDS = value; } }

        public void MoveRom(int index, int newIndex)
        {
            if (index == newIndex) return;
            string rom = romIDS[index];
            romIDS.RemoveAt(index);
            if (newIndex >= 0)
                romIDS.Insert(newIndex, rom);
            else
                romIDS.Add(rom);
        }
    }
}
