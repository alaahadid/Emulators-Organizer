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
    /// Console groups collection
    /// </summary>
    [Serializable()]
    public class ConsoleGroupsCollection : IList<ConsolesGroup>
    {
        /// <summary>
        ///  Console groups collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        public ConsoleGroupsCollection(Profile owner)
            : base()
        {
            this.owner = owner;
            items = new List<ConsolesGroup>();
        }
        private Profile owner;
        private List<ConsolesGroup> items;

        /// <summary>
        /// Get or set consoles group using desired consoles group id.
        /// </summary>
        /// <param name="id">Desired consoles group id.</param>
        /// <returns>Consoles group for given id if found otherwise null.</returns>
        public ConsolesGroup this[string id]
        {
            get
            {
                return items.Find(
                      delegate(ConsolesGroup gr)
                      {
                          return gr.ID == id;
                      }
                  );
            }
            set
            {
                ConsolesGroup group = items.Find(
                      delegate(ConsolesGroup gr)
                      {
                          return gr.ID == id;
                      }
                  );
                if (group != null)
                {
                    int index = items.IndexOf(group);
                    if (index < items.Count)
                        this[index] = value;
                }
            }
        }
        /// <summary>
        /// Get or set consoles group
        /// </summary>
        /// <param name="index">The consoles group index</param>
        /// <returns>Consoles group at given index otherwise null</returns>
        public ConsolesGroup this[int index]
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
        /// Get ondex of consoles group
        /// </summary>
        /// <param name="item">The consoles group to get index for</param>
        /// <returns>The given consoles group index if found otherwise null</returns>
        public int IndexOf(ConsolesGroup item)
        {
            return items.IndexOf(item);
        }
        /// <summary>
        /// Get index of consoles group
        /// </summary>
        /// <param name="itemID">The consoles group id to get index for</param>
        /// <returns>The given consoles group index if found otherwise null</returns>
        public int IndexOf(string itemID)
        {
            return items.IndexOf(this[itemID]);
        }
        /// <summary>
        /// Insert consoels group at index
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="item">The consoles group to insert</param>
        public void Insert(int index, ConsolesGroup item)
        {
            items.Insert(index, item);
            owner.OnConsolesGroupAdd();
        }
        /// <summary>
        /// Move consoles group from location to another
        /// </summary>
        /// <param name="index">The consoles group index to move</param>
        /// <param name="newIndex">The new index to move consoles group into</param>
        public void Move(int index, int newIndex)
        {
            string name = this[index].Name;

            if (items.Count < 2) goto RaiseEvent;
            if (index == newIndex) return;
            // Get console
            ConsolesGroup con = this[index];
            items.RemoveAt(index);
            if (newIndex >= 0)
                items.Insert(newIndex, con);
            else
                items.Add(con);

        RaiseEvent:
            if (owner != null)
                owner.OnConsolesGroupMoved(name);
        }
        /// <summary>
        /// Remove consoles group at index
        /// </summary>
        /// <param name="index">The index of consoles group to remove</param>
        public void RemoveAt(int index)
        {
            string id = this[index].ID;
            items.RemoveAt(index);
            owner.OnConsolesGroupRemoved(id);
        }
        /// <summary>
        /// Remove consoles group at index
        /// </summary>
        /// <param name="index">The index of consoles group to remove</param>
        /// <param name="raiseEvent">Set to true to raise the ConsolesGroupRemoved event</param>
        public void RemoveAt(int index, bool raiseEvent)
        {
            string id = this[index].ID;
            items.RemoveAt(index);
            if (raiseEvent)
                owner.OnConsolesGroupRemoved(id);
        }
        /// <summary>
        /// Add consoels group
        /// </summary>
        /// <param name="item">The consoels group to add</param>
        public void Add(ConsolesGroup item)
        {
            items.Add(item);
            owner.OnConsolesGroupAdd();
        }
        /// <summary>
        /// Cleare consoels group collection
        /// </summary>
        public void Clear()
        {
            items.Clear();
            owner.OnConsoleGroupsClear();
        }
        /// <summary>
        /// Check if this collection include consoles group using name
        /// </summary>
        /// <param name="name">The name of consoles group. Not case sensitive</param>
        /// <returns>True if given consoles group name exists otherwise false</returns>
        public bool Contains(string name)
        {
            foreach (ConsolesGroup gr in items)
            {
                if (gr.Name.ToLower() == name.ToLower())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Check if this collection include consoles group using name
        /// </summary>
        /// <param name="name">The name of consoles group. Not case sensitive</param>
        /// <param name="idToIgnore">The console group id to ignore in search</param>
        /// <returns>True if given consoles group name exists otherwise false</returns>
        public bool Contains(string name, string idToIgnore)
        {
            foreach (ConsolesGroup gr in items)
            {
                if (gr.Name.ToLower() == name.ToLower() && gr.ID != idToIgnore)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Check if this collection include consoles group
        /// </summary>
        /// <param name="item">The consoles group to check</param>
        /// <returns>True if given consoles group exists otherwise false</returns>
        public bool Contains(ConsolesGroup item)
        {
            return items.Contains(item);
        }
        /// <summary>
        /// Copy consoles group collection to array
        /// </summary>
        /// <param name="array">The console groups array to copy into</param>
        /// <param name="arrayIndex">The array index to start from</param>
        public void CopyTo(ConsolesGroup[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// The console groups count
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
        /// Remove consoles group from the collection
        /// </summary>
        /// <param name="item">The consoles group to remove</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(ConsolesGroup item)
        {
            bool val = items.Remove(item);
            owner.OnConsolesGroupRemoved(item.ID);
            return val;
        }
        /// <summary>
        /// Remove consoles group from the collection
        /// </summary>
        /// <param name="itemID">The consoels group id to remove</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(string itemID)
        {
            bool val = items.Remove(this[itemID]);
            owner.OnConsolesGroupRemoved(itemID);
            return val;
        }
        /// <summary>
        /// Sort the collection
        /// </summary>
        /// <param name="comparer">The comparer to use</param>
        public void Sort(ConsoleGroupsComparer comparer)
        {
            items.Sort(comparer);
            if (owner != null)
                owner.OnConsoleGroupsSort();
        }
        /// <summary>
        /// This.GetEnumerator
        /// </summary>
        /// <returns>This.GetEnumerator</returns>
        public IEnumerator<ConsolesGroup> GetEnumerator()
        {
            return items.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
