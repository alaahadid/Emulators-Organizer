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
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// The currently selection type
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// Nothing selected
        /// </summary>
        None = 0,
        /// <summary>
        /// Selection is consoles group
        /// </summary>
        ConsolesGroup = 1,
        /// <summary>
        /// Selection is console
        /// </summary>
        Console = 2,
        /// <summary>
        /// Selection is playlists group
        /// </summary>
        PlaylistsGroup = 3,
        /// <summary>
        /// Selection is playlist
        /// </summary>
        Playlist = 4
    }
}
