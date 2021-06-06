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
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Tree node holds playlist
    /// </summary>
    public class TreeNodePlaylist : EOTreeNode
    {
        /// <summary>
        /// Tree node holds playlist
        /// </summary>
        /// <param name="id">The console id</param>
        public TreeNodePlaylist(string id)
            :base(id)
        {
            playlist = profileManager.Profile.Playlists[id];
            RefreshText();
        }
        public Playlist playlist;
        /// <summary>
        /// Refresh text of this node
        /// </summary>
        public override void RefreshText()
        {
            this.Text = playlist.Name;
        }
        /// <summary>
        /// Refresh nodes of this node
        /// </summary>
        public override void RefreshNodes()
        {
            base.RefreshNodes();
        }
    }
}
