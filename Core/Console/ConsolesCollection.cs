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
    /// Consoles collection
    /// </summary>
    [Serializable()]
    public class ConsolesCollection : IList<Console>
    {
        /// <summary>
        /// Consoles collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        public ConsolesCollection(Profile owner)
            : base()
        {
            this.owner = owner;
            items = new List<Console>();
        }
        /// <summary>
        /// Consoles collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        /// <param name="consoles">The consoles to start with</param>
        public ConsolesCollection(Profile owner, Console[] consoles)
            : base()
        {
            this.owner = owner;
            items = new List<Console>(consoles);
        }
        private List<Console> items;
        private Profile owner;
        /// <summary>
        /// Get or set console using desired console id.
        /// </summary>
        /// <param name="id">Desired console id.</param>
        /// <returns>Console for given id if found otherwise null.</returns>
        public Console this[string id]
        {
            get
            {
                return items.Find(
                      delegate(Console con)
                      {
                          return con.ID == id;
                      }
                  );
            }
            set
            {
                Console tcon = items.Find(
                      delegate(Console con)
                      {
                          return con.ID == id;
                      }
                  );
                if (tcon != null)
                {
                    int index = items.IndexOf(tcon);
                    if (index < items.Count)
                        this[index] = value;
                }
            }
        }
        /// <summary>
        /// Console from conosoles collection
        /// </summary>
        /// <param name="index">The console index</param>
        /// <returns>The console at given index if found otherwise null</returns>
        public Console this[int index]
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
        /// Get consoles that belong to console group id.
        /// </summary>
        /// <param name="groupID">Desired console group id.</param>
        /// <param name="sort">Indicate whether to sort results by name</param>
        /// <returns>Consoles that belong to console group.</returns>
        public Console[] this[string groupID, bool sort]
        {
            get
            {
                List<Console> cons = items.FindAll(
                      delegate(Console con)
                      {
                          return con.ParentGroupID == groupID;
                      }
                  );
                if (cons != null)
                {
                    if (sort)
                        cons.Sort(new ConsolesComparer(true, ConsoleCompareType.Name));
                }
                return cons.ToArray();
            }
        }

        /// <summary>
        /// Return item's index
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>Index of item otherwise -1</returns>
        public int IndexOf(Console item)
        {
            return items.IndexOf(item);
        }
        /// <summary>
        /// Get ondex of console
        /// </summary>
        /// <param name="itemID">The console id to get index for</param>
        /// <returns>The given console index if found otherwise null</returns>
        public int IndexOf(string itemID)
        {
            return items.IndexOf(this[itemID]);
        }
        /// <summary>
        /// Insert item to the collection
        /// </summary>
        /// <param name="index">The index where to insert at</param>
        /// <param name="item">The console to insert</param>
        public void Insert(int index, Console item)
        {
            items.Insert(index, item);
            if (owner != null)
                owner.OnConsoleAdd();
        }
        /// <summary>
        /// Move console from location to another
        /// </summary>
        /// <param name="index">The index of the console to move</param>
        /// <param name="newIndex">The new index to move the console to</param>
        public void Move(int index, int newIndex)
        {
            string name = this[index].Name;

            if (items.Count < 2) goto RaiseEvent;
            if (index == newIndex) return;
            // Get console
            Console con = this[index];
            items.RemoveAt(index);
            if (newIndex >= 0)
                items.Insert(newIndex, con);
            else
                items.Add(con);

        RaiseEvent:
            if (owner != null)
                owner.OnConsoleMoved(name);
        }
        /// <summary>
        /// Remove console at index
        /// </summary>
        /// <param name="index">The console index to remove</param>
        public void RemoveAt(int index)
        {
            RemoveAt(index, true);
        }
        /// <summary>
        /// Remove console at index
        /// </summary>
        /// <param name="index">The console index to remove</param>
        /// <param name="raiseEvent">Set to raise the ConsoleRemoved event</param>
        public void RemoveAt(int index, bool raiseEvent)
        {
            string id = this[index].ID;
            items.RemoveAt(index);
            if (owner != null && raiseEvent)
                owner.OnConsoleRemoved(id);
        }
        /// <summary>
        /// Add console to the collection
        /// </summary>
        /// <param name="item">The console to add</param>
        public void Add(Console item)
        {
            items.Add(item);
            if (owner != null)
                owner.OnConsoleAdd();
        }
        /// <summary>
        /// Add console to the collection without raising the event
        /// </summary>
        /// <param name="item">The console to add</param>
        public void AddNoEvent(Console item)
        {
            items.Add(item);
        }
        /// <summary>
        /// Add consoles to the collection. THIS WILL NOT RAISE ANY EVENT !
        /// </summary>
        /// <param name="itemsToAdd">The consoles to add</param>
        public void AddRange(Console[] itemsToAdd)
        {
            items.AddRange(itemsToAdd);
        }
        /// <summary>
        /// Add consoles to the collection. THIS WILL NOT RAISE ANY EVENT !
        /// </summary>
        /// <param name="itemsToAdd">The consoles to add</param>
        public void AddRange(ConsolesCollection itemsToAdd)
        {
            items.AddRange(itemsToAdd);
        }
        /// <summary>
        /// Clear consoles collecion
        /// </summary>
        public void Clear()
        {
            items.Clear();
            if (owner != null)
                owner.OnConsolesClear();
        }
        /// <summary>
        /// Indicate whether a console exist in this collection
        /// </summary>
        /// <param name="item">The console to check</param>
        /// <returns>True if console exists otherwise false</returns>
        public bool Contains(Console item)
        {
            return items.Contains(item);
        }
        /// <summary>
        /// Indicate whether a console exist in this collection
        /// </summary>
        /// <param name="name">The console name to check</param>
        /// <returns>True if console exists otherwise false</returns>
        public bool Contains(string name)
        {
            foreach (Console con in items)
            {
                if (con.Name.ToLower() == name.ToLower())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Indicate whether a console exist in this collection
        /// </summary>
        /// <param name="name">The console name to check</param>
        /// <param name="parentGroupID">The parent console id to use for filtering</param>
        /// <param name="ignoreConsoleID">The console id to ignore. Set to "" to ignore nothing.</param>
        /// <returns>True if console exists otherwise false</returns>
        public bool Contains(string name, string parentGroupID, string ignoreConsoleID)
        {
            foreach (Console con in items)
            {
                if (con.ID != ignoreConsoleID)
                    if (con.ParentGroupID == parentGroupID)
                    {
                        if (con.Name.ToLower() == name.ToLower())
                            return true;
                    }
            }
            return false;
        }
        /// <summary>
        /// Indicate whether a console exist in this collection
        /// </summary>
        /// <param name="name">The console name to check</param>
        /// <returns>True if console exists otherwise false</returns>
        public bool ContainsID(string id)
        {
            foreach (Console con in items)
            {
                if (con.ID == id)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Copy collection to consoles array
        /// </summary>
        /// <param name="array">The consoles array to copy into</param>
        /// <param name="arrayIndex">The array start index which will copy from</param>
        public void CopyTo(Console[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// The collection consoles count
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
        /// Remove console from the collection
        /// </summary>
        /// <param name="item">The console to remove</param>
        /// <returns>True if console removed otherwise false</returns>
        public bool Remove(Console item)
        {
            bool val = items.Remove(item);
            if (owner != null)
                owner.OnConsoleRemoved(item.ID);
            return val;
        }
        /// <summary>
        /// Remove console from the collection
        /// </summary>
        /// <param name="itemsToRemove">The consoles to remove</param>
        /// <returns>True if console removed otherwise false</returns>
        public void RemoveItems(ConsolesCollection itemsToRemove)
        {
            foreach (Console it in itemsToRemove)
                items.Remove(this[it.ID]);
        }
        /// <summary>
        /// Remove console from the collection
        /// </summary>
        /// <param name="itemsToRemove">The consoles to remove</param>
        /// <returns>True if console removed otherwise false</returns>
        public void RemoveItems(Console[] itemsToRemove)
        {
            foreach (Console it in itemsToRemove)
                items.Remove(this[it.ID]);
        }
        /// <summary>
        /// Remove console from the collection using console id
        /// </summary>
        /// <param name="itemID">The console id to remove</param>
        /// <returns>True if console removed otherwise false</returns>
        public bool Remove(string itemID)
        {
            bool val = items.Remove(this[itemID]);
            if (owner != null)
                owner.OnConsoleRemoved(itemID);
            return val;
        }
        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns>This.GetEnumerator</returns>
        public IEnumerator<Console> GetEnumerator()
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
        public void Sort(ConsolesComparer comparer)
        {
            items.Sort(comparer);
            if (owner != null)
                owner.OnConsolesSort();
        }
    }
}
