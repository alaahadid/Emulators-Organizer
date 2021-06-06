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
    /// The playlists comparer.
    /// </summary>
    public class PlaylistsComparer : IComparer<Playlist>
    {
        /// <summary>
        /// The playlists comparer
        /// </summary>
        /// <param name="aToZ">Sort by A to Z or Z to A</param>
        /// <param name="type">The type of the sort</param>
        public PlaylistsComparer(bool aToZ, PlaylistCompareType type)
        {
            this.aToZ = aToZ;
            this.type = type;
        }
        private bool aToZ = true;
        private PlaylistCompareType type = PlaylistCompareType.Name;
        /// <summary>
        /// Compare 2 playlists
        /// </summary>
        /// <param name="x">The first platylist</param>
        /// <param name="y">The second platylist</param>
        /// <returns>Compare result.</returns>
        public int Compare(Playlist x, Playlist y)
        {
            switch (type)
            {
                case PlaylistCompareType.Name:
                    {
                        if (aToZ)
                            return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name);
                        else
                            return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name));
                    }
                default: return -1;
            }
        }
    }
    /// <summary>
    /// Playlist Compare Type
    /// </summary>
    public enum PlaylistCompareType
    {
        /// <summary>
        /// By name
        /// </summary>
        Name
    }
}
