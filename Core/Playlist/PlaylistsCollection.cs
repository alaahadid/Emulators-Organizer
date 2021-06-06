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
    /// Playlists collection
    /// </summary>
    [Serializable()]
    public class PlaylistsCollection : IList<Playlist>
    {
        /// <summary>
        /// The playlists collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        public PlaylistsCollection(Profile owner)
            : base()
        {
            this.owner = owner;
            this.items = new List<Playlist>();
        }
        /// <summary>
        /// playlists collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        /// <param name="playlists">The playlists to start with</param>
        public PlaylistsCollection(Profile owner, Playlist[] playlists)
            : base()
        {
            this.owner = owner;
            this.items = new List<Playlist>(playlists);
        }

        private Profile owner;
        private List<Playlist> items;

        /// <summary>
        /// Playlist from playlists collection
        /// </summary>
        /// <param name="index">The playlist index</param>
        /// <returns>The playlist at given index if found otherwise null</returns>
        public Playlist this[int index]
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
        /// Get or set playlist using desired playlist id.
        /// </summary>
        /// <param name="id">Desired playlist id.</param>
        /// <returns>Playlist for given id if found otherwise null.</returns>
        public Playlist this[string id]
        {
            get
            {
                return items.Find(
                      delegate(Playlist pl)
                      {
                          return pl.ID == id;
                      }
                  );
            }
            set
            {
                Playlist tpl = items.Find(
                      delegate(Playlist pl)
                      {
                          return pl.ID == id;
                      }
                  );
                if (tpl != null)
                {
                    int index = items.IndexOf(tpl);
                    if (index < items.Count)
                        this[index] = value;
                }
            }
        }
        /// <summary>
        /// Get playlists that belong to playlists group id.
        /// </summary>
        /// <param name="groupID">Desired playlists group id.</param>
        /// <param name="sort">Indecate whether to sort results by name</param>
        /// <returns>Playlists that belong to playlists group.</returns>
        public Playlist[] this[string groupID, bool sort]
        {
            get
            {
                List<Playlist> cons = items.FindAll(
                      delegate(Playlist pl)
                      {
                          return pl.ParentGroupID == groupID;
                      }
                  );
                if (cons != null)
                {
                    if (sort)
                        cons.Sort(new PlaylistsComparer(true, PlaylistCompareType.Name));
                }
                return cons.ToArray();
            }
        }

        /// <summary>
        /// Return item's index
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>Index of item otherwise -1</returns>
        public int IndexOf(Playlist item)
        {
            return items.IndexOf(item);
        }
        /// <summary>
        /// Get index of playlist
        /// </summary>
        /// <param name="itemID">The playlist id to get index for</param>
        /// <returns>The given playlist index if found otherwise null</returns>
        public int IndexOf(string itemID)
        {
            return items.IndexOf(this[itemID]);
        }
        /// <summary>
        /// Insert item to the collection
        /// </summary>
        /// <param name="index">The index where to insert at</param>
        /// <param name="item">The playlist to insert</param>
        public void Insert(int index, Playlist item)
        {
            items.Insert(index, item);
            if (owner != null)
                owner.OnPlaylistAdd();
        }
        /// <summary>
        /// Move playlist from location to another
        /// </summary>
        /// <param name="index">The index of the playlist to move</param>
        /// <param name="newIndex">The new index to move the playlist to</param>
        public void Move(int index, int newIndex)
        {
            string name = this[index].Name;

            if (items.Count < 2) goto RaiseEvent;
            if (index == newIndex) return;
            // Get playlist
            Playlist con = this[index];
            items.RemoveAt(index);
            if (newIndex >= 0)
                items.Insert(newIndex, con);
            else
                items.Add(con);

        RaiseEvent:
            if (owner != null)
                owner.OnPlaylistMoved(name);
        }
        /// <summary>
        /// Remove playlist at index
        /// </summary>
        /// <param name="index">The playlist index to remove</param>
        public void RemoveAt(int index)
        {
            string id = this[index].ID;
            items.RemoveAt(index);
            if (owner != null)
                owner.OnPlaylistRemoved(id);
        }
        /// <summary>
        /// Add playlist to the collection
        /// </summary>
        /// <param name="item">The playlist to add</param>
        public void Add(Playlist item)
        {
            items.Add(item);
            if (owner != null)
                owner.OnPlaylistAdd();
        }
        /// <summary>
        /// Add playlists to the collection. THIS WILL NOT RAISE ANY EVENT !
        /// </summary>
        /// <param name="itemsToAdd">The playlists to add</param>
        public void AddRange(Playlist[] itemsToAdd)
        {
            items.AddRange(itemsToAdd);
        }
        /// <summary>
        /// Add playlists to the collection. THIS WILL NOT RAISE ANY EVENT !
        /// </summary>
        /// <param name="itemsToAdd">The playlists to add</param>
        public void AddRange(PlaylistsCollection itemsToAdd)
        {
            items.AddRange(itemsToAdd);
        }
        /// <summary>
        /// Clear playlists collecion
        /// </summary>
        public void Clear()
        {
            items.Clear();
            if (owner != null)
                owner.OnPlaylistsClear();
        }
        /// <summary>
        /// Indicate whether a playlist exist in this collection
        /// </summary>
        /// <param name="item">The playlist to check</param>
        /// <returns>True if playlist exists otherwise false</returns>
        public bool Contains(Playlist item)
        {
            return items.Contains(item);
        }
        /// <summary>
        /// Indicate whether a playlist exist in this collection
        /// </summary>
        /// <param name="name">The playlist name to check</param>
        /// <returns>True if playlist exists otherwise false</returns>
        public bool Contains(string name)
        {
            foreach (Playlist playlist in items)
            {
                if (playlist.Name.ToLower() == name.ToLower())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Indicate whether a playlist exist in this collection
        /// </summary>
        /// <param name="name">The playlist name to check</param>
        /// <param name="parentGroupID">The parent playlist id to use for filtering</param>
        /// <param name="ignorePlaylistID">The playlist id to ignore. Set to "" to ignore nothing.</param>
        /// <returns>True if playlist exists otherwise false</returns>
        public bool Contains(string name, string parentGroupID, string ignorePlaylistID)
        {
            foreach (Playlist playlist in items)
            {
                if (playlist.ID != ignorePlaylistID)
                    if (playlist.ParentGroupID == parentGroupID)
                    {
                        if (playlist.Name.ToLower() == name.ToLower())
                            return true;
                    }
            }
            return false;
        }
        /// <summary>
        /// Check if playlist exist using playlist id
        /// </summary>
        /// <param name="id">The playlist id to check</param>
        /// <returns>True if playlist exists otherwise false</returns>
        public bool ContainsID(string id)
        {
            foreach (Playlist playlist in items)
            {
                if (playlist.ID == id) return true;
            }
            return false;
        }
        /// <summary>
        /// Copy collection to playlists array
        /// </summary>
        /// <param name="array">The playlists array to copy into</param>
        /// <param name="arrayIndex">The array start index which will copy from</param>
        public void CopyTo(Playlist[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// The collection playlists count
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }
        /// <summary>
        /// Is ready only ?
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Remove playlist from the collection
        /// </summary>
        /// <param name="item">The playlist to remove</param>
        /// <returns>True if playlist removed otherwise false</returns>
        public bool Remove(Playlist item)
        {
            bool val = items.Remove(item);
            if (owner != null)
                owner.OnPlaylistRemoved(item.ID);
            return val;
        }
        /// <summary>
        /// Remove playlist from the collection
        /// </summary>
        /// <param name="itemsToRemove">The playlists to remove</param>
        /// <returns>True if playlist removed otherwise false</returns>
        public void RemoveItems(PlaylistsCollection itemsToRemove)
        {
            foreach (Playlist it in itemsToRemove)
                items.Remove(this[it.ID]);
        }
        /// <summary>
        /// Remove playlist from the collection
        /// </summary>
        /// <param name="itemsToRemove">The playlists to remove</param>
        /// <returns>True if playlist removed otherwise false</returns>
        public void RemoveItems(Playlist[] itemsToRemove)
        {
            foreach (Playlist it in itemsToRemove)
                items.Remove(this[it.ID]);
        }
        /// <summary>
        /// Remove playlist from the collection using playlist id
        /// </summary>
        /// <param name="itemID">The playlist id to remove</param>
        /// <returns>True if playlist removed otherwise false</returns>
        public bool Remove(string itemID)
        {
            bool val = items.Remove(this[itemID]);
            if (owner != null)
                owner.OnPlaylistRemoved(itemID);
            return val;
        }
        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns>This.GetEnumerator</returns>
        public IEnumerator<Playlist> GetEnumerator()
        {
            return items.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
        /// <summary>
        /// Sort the collection
        /// </summary>
        /// <param name="comparer">The comparer to use</param>
        public void Sort(PlaylistsComparer comparer)
        {
            items.Sort(comparer);
            if (owner != null)
                owner.OnPlaylistsSort();
        }
    }
}
