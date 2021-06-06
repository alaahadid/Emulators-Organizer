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
using EmulatorsOrganizer.Services;
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Tree node holds consoles group
    /// </summary>
    public class TreeNodePlaylistsGroup : EOTreeNode
    {
        /// <summary>
        /// Tree node holds playlists group
        /// </summary>
        /// <param name="id">The playlists group id</param>
        public TreeNodePlaylistsGroup(string id)
            : base(id)
        {
            group = profileManager.Profile.PlaylistGroups[id]; 
            RefreshText();
        }
        public PlaylistsGroup group;

        /// <summary>
        /// Refresh the text of this node
        /// </summary>
        public override void RefreshText()
        {
            this.Text = group.Name;
        }
        /// <summary>
        /// Refresh the console nodes
        /// </summary>
        public override void RefreshNodes()
        {
            this.Nodes.Clear();
           
            Playlist[] playlists = profileManager.Profile.Playlists[group.ID, false];
            foreach (Playlist playlist in playlists)
            {
                TreeNodePlaylist tr = new TreeNodePlaylist(playlist.ID);
                tr.RefreshNodes();
                this.Nodes.Add(tr);
            }
        }
    }
}
