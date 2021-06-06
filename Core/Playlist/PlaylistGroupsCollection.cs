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
    /// Playlist groups collection
    /// </summary>
    [Serializable()]
    public class PlaylistGroupsCollection : IList<PlaylistsGroup>
    {
        /// <summary>
        /// Playlist groups collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        public PlaylistGroupsCollection(Profile owner)
            : base()
        {
            this.owner = owner;
            this.items = new List<PlaylistsGroup>();
        }
        private Profile owner;
        private List<PlaylistsGroup> items;

        /// <summary>
        /// Get or set playlist group using desired playlist group id.
        /// </summary>
        /// <param name="id">Desired playlist group id.</param>
        /// <returns>Playlist group for given id if found otherwise null.</returns>
        public PlaylistsGroup this[string id]
        {
            get
            {
                return this.items.Find(
                      delegate(PlaylistsGroup pl)
                      {
                          return pl.ID == id;
                      }
                  );
            }
            set
            {
                PlaylistsGroup tpl = this.items.Find(
                      delegate(PlaylistsGroup pl)
                      {
                          return pl.ID == id;
                      }
                  );
                if (tpl != null)
                {
                    int index = this.items.IndexOf(tpl);
                    if (index < this.items.Count)
                        this[index] = value;
                }
            }
        }
        /// <summary>
        /// Get or set playlists group
        /// </summary>
        /// <param name="index">The playlists group index</param>
        /// <returns>Playlists group at given index otherwise null</returns>
        public PlaylistsGroup this[int index]
        {
            get
            {
                if (index < items.Count)
                    return items[index];
                return null;
            }
            set
            {
                if (index < items.Count)
                    items[index] = value;
            }
        }
        /// <summary>
        /// Get the index of element within the collection
        /// </summary>
        /// <param name="item">The item to get the index of</param>
        /// <returns>The index of the element if found otherwise -1</returns>
        public int IndexOf(PlaylistsGroup item)
        {
            return items.IndexOf(item);
        }
        /// <summary>
        /// Get the index of element within the collection
        /// </summary>
        /// <param name="itemID">The item id to get the index of</param>
        /// <returns>The given item index if found otherwise null</returns>
        public int IndexOf(string itemID)
        {
            return items.IndexOf(this[itemID]);
        }

        /// <summary>
        /// Insert playlists group at index
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="item">The playlists group to insert</param>
        public void Insert(int index, PlaylistsGroup item)
        {
            items.Insert(index, item);
            owner.OnPlaylistsGroupAdd();
        }
        /// <summary>
        /// Move playlists group from location to another
        /// </summary>
        /// <param name="index">The playlists group index to move</param>
        /// <param name="newIndex">The new index to move playlists group into</param>
        public void Move(int index, int newIndex)
        {
            string name = this[index].Name;

            if (items.Count < 2) goto RaiseEvent;
            if (index == newIndex) return;
            // Get console
            PlaylistsGroup con = this[index];
            items.RemoveAt(index);
            if (newIndex >= 0)
                items.Insert(newIndex, con);
            else
                items.Add(con);

        RaiseEvent:
            if (owner != null)
                owner.OnPlaylistsGroupMoved(name);
        }
        /// <summary>
        /// Remove playlists group at index
        /// </summary>
        /// <param name="index">The index of playlists group to remove</param>
        public void RemoveAt(int index)
        {
            string id = this[index].ID;
            items.RemoveAt(index);
            owner.OnPlaylistsGroupRemoved(id);
        }
        /// <summary>
        /// Remove playlists group at index
        /// </summary>
        /// <param name="index">The index of playlists group to remove</param>
        /// <param name="raiseEvent">Set to true to raise the PlaylistsGroupRemoved event</param>
        public void RemoveAt(int index, bool raiseEvent)
        {
            string id = this[index].ID;
            items.RemoveAt(index);
            if (raiseEvent)
                owner.OnPlaylistsGroupRemoved(id);
        }
        /// <summary>
        /// Add playlists group
        /// </summary>
        /// <param name="item">The playlists group to add</param>
        public void Add(PlaylistsGroup item)
        {
            items.Add(item);
            owner.OnPlaylistsGroupAdd();
        }
        /// <summary>
        /// Clear playlists group collection
        /// </summary>
        public void Clear()
        {
            items.Clear();
            owner.OnPlaylistGroupsClear();
        }
        /// <summary>
        /// Check if this collection include playlists group using name
        /// </summary>
        /// <param name="name">The name of playlists group. Not case sensitive</param>
        /// <returns>True if given playlists group name exists otherwise false</returns>
        public bool Contains(string name)
        {
            foreach (PlaylistsGroup gr in items)
            {
                if (gr.Name.ToLower() == name.ToLower())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Check if this collection include playlists group using name
        /// </summary>
        /// <param name="name">The name of playlists group. Not case sensitive</param>
        /// <param name="idToIgnore">The playlist group id to ignore in search</param>
        /// <returns>True if given playlists group name exists otherwise false</returns>
        public bool Contains(string name, string idToIgnore)
        {
            foreach (PlaylistsGroup gr in items)
            {
                if (gr.Name.ToLower() == name.ToLower() && gr.ID != idToIgnore)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Check if this collection include playlists group
        /// </summary>
        /// <param name="item">The playlists group to check</param>
        /// <returns>True if given playlists group exists otherwise false</returns>
        public bool Contains(PlaylistsGroup item)
        {
            return items.Contains(item);
        }
        /// <summary>
        /// Copy playlists group collection to array
        /// </summary>
        /// <param name="array">The playlist groups array to copy into</param>
        /// <param name="arrayIndex">The array index to start from</param>
        public void CopyTo(PlaylistsGroup[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// The playlist groups count
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }
        /// <summary>
        /// Is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Remove playlists group from the collection
        /// </summary>
        /// <param name="item">The playlists group to remove</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(PlaylistsGroup item)
        {
            bool val = items.Remove(item);
            owner.OnPlaylistsGroupRemoved(item.ID);
            return val;
        }
        /// <summary>
        /// Remove playlists group from the collection
        /// </summary>
        /// <param name="itemID">The playlists group id to remove</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(string itemID)
        {
            bool val = items.Remove(this[itemID]);
            owner.OnPlaylistsGroupRemoved(itemID);
            return val;
        }
        /// <summary>
        /// Sort the collection
        /// </summary>
        /// <param name="comparer">The comparer to use</param>
        public void Sort(PlaylistGroupsComparer comparer)
        {
            items.Sort(comparer);
            if (owner != null)
                owner.OnPlaylistGroupsSort();
        }
        /// <summary>
        /// This.GetEnumerator
        /// </summary>
        /// <returns>This.GetEnumerator</returns>
        public IEnumerator<PlaylistsGroup> GetEnumerator()
        {
            return items.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
