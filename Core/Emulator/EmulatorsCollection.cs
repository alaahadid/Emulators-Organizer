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
    /// Emulators collection
    /// </summary>
    [Serializable()]
    public class EmulatorsCollection : IList<Emulator>
    {
        /// <summary>
        /// Emulators collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        public EmulatorsCollection(Profile owner)
            :base()
        {
            this.owner = owner;
            this.items = new List<Emulator>();
        }
        /// <summary>
        /// Emulators collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        /// <param name="emulators">A list of emulators to start with</param>
        public EmulatorsCollection(Profile owner, Emulator [] emulators)
            : base()
        {
            this.owner = owner;
            this.items = new List<Emulator>(emulators);
        }
        private Profile owner;
        private List<Emulator> items;

        /// <summary>
        /// Emulator
        /// </summary>
        /// <param name="index">Index of desired emulator</param>
        /// <returns>Emulator at given index if found otherwise null</returns>
        public Emulator this[int index]
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
        /// Get or set emulator using id.
        /// </summary>
        /// <param name="id">Desired emulator id.</param>
        /// <returns>Emulator for given id if found otherwise null.</returns>
        public Emulator this[string id]
        {
            get
            {
                return items.Find(
                      delegate(Emulator emu)
                      {
                          return emu.ID == id;
                      }
                  );
            }
            set
            {
                Emulator temu = items.Find(
                      delegate(Emulator emu)
                      {
                          return emu.ID == id;
                      }
                  );
                if (temu != null)
                {
                    int index = items.IndexOf(temu);
                    if (index < items.Count)
                        this[index] = value;
                }
            }
        }
        /// <summary>
        /// Get emulators that belong to given console.
        /// </summary>
        /// <param name="consoleID">Desired console id.</param>
        /// <param name="sort">Indicate whether to sort results by name.</param>
        /// <returns>Emulators that belong to given console.</returns>
        public Emulator[] this[string consoleID, bool sort]
        {
            get
            {
                List<Emulator> emus = items.FindAll(
                      delegate(Emulator emu)
                      {
                          return emu.IsConsoleSupported(consoleID);
                      }
                  );
                if (emus != null)
                    if (sort)
                        emus.Sort(new EmulatorsComparer(true, EmulatorCompareType.Name));
                return emus.ToArray();
            }
        }
        /// <summary>
        /// Get index of emulator
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(Emulator item)
        {
            return items.IndexOf(item);
        }
        /// <summary>
        /// Get index of emulator
        /// </summary>
        /// <param name="itemID">The emulator id to get index for</param>
        /// <returns>The given emulator index if found otherwise null</returns>
        public int IndexOf(string itemID)
        {
            return items.IndexOf(this[itemID]);
        }
        /// <summary>
        /// Move emulator from location to another
        /// </summary>
        /// <param name="index">The emulator index to move</param>
        /// <param name="newIndex">The new index to move emulator into</param>
        public void Move(int index, int newIndex)
        {
            if (index < 0)
                return;
            if (newIndex < 0)
                return;
            if (index >= this.Count)
                return;
            if (newIndex >= this.Count)
                return;

            string name = this[index].Name;

            if (items.Count < 2) goto RaiseEvent;
            if (index == newIndex) return;
            // Get emulator
            Emulator emu = this[index];
            items.RemoveAt(index);
            if (newIndex >= 0)
                items.Insert(newIndex, emu);
            else
                items.Add(emu);

        RaiseEvent:
            if (owner != null)
                owner.OnEmulatorMoved(name);
        }
        /// <summary>
        /// Insert new emulator
        /// </summary>
        /// <param name="index">The index within collection where to insert emulator at</param>
        /// <param name="item">The emulator to insert</param>
        public void Insert(int index, Emulator item)
        {
            items.Insert(index, item);
            if (owner != null)
                owner.OnEmulatorAdded(item.Name);
        }
        /// <summary>
        /// Remove emulator at index
        /// </summary>
        /// <param name="index">The emulator's index</param>
        public void RemoveAt(int index)
        {
            string name = items[index].Name;
            items.RemoveAt(index);
            if (owner != null)
                owner.OnEmulatorRemoved(name);
        }
        /// <summary>
        /// Add emulator
        /// </summary>
        /// <param name="item">The emulator object to add</param>
        public void Add(Emulator item)
        {
            items.Add(item);
            if (owner != null)
                owner.OnEmulatorAdded(item.Name);
        }
        /// <summary>
        /// Add emulators to the collection. THIS WILL NOT RAISE ANY EVENT !
        /// </summary>
        /// <param name="itemsToAdd">The emulators to add</param>
        public void AddRange(Emulator[] itemsToAdd)
        {
            items.AddRange(itemsToAdd);
        }
        /// <summary>
        /// Add emulators to the collection. THIS WILL NOT RAISE ANY EVENT !
        /// </summary>
        /// <param name="itemsToAdd">The emulators to add</param>
        public void AddRange(EmulatorsCollection itemsToAdd)
        {
            items.AddRange(itemsToAdd);
        }
        /// <summary>
        /// Clear the emulators collection
        /// </summary>
        public void Clear()
        {
            items.Clear();
            if (owner != null)
                owner.OnEmulatorsCleared();
        }
        /// <summary>
        /// Get if emulator already exist in the collection
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>True if given emulator exist otherwise false</returns>
        public bool Contains(Emulator item)
        {
            return items.Contains(item);
        }
        /// <summary>
        /// Check if this collection include emulator using name
        /// </summary>
        /// <param name="name">The name of emulator. Not case sensitive</param>
        /// <param name="idToIgnore">The emulator id to ignore in search</param>
        /// <returns>True if given emulator name exists otherwise false</returns>
        public bool Contains(string name, string idToIgnore)
        {
            foreach (Emulator emu in items)
            {
                if (emu.Name.ToLower() == name.ToLower() && emu.ID != idToIgnore)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Copy the emulators collection to an array
        /// </summary>
        /// <param name="array">The array to copy into</param>
        /// <param name="arrayIndex">The start index within given array to start with</param>
        public void CopyTo(Emulator[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Get the emulators count
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }
        /// <summary>
        /// Get if this collection is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Remove emulator from the collection
        /// </summary>
        /// <param name="item">The emulator to remove</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(Emulator item)
        {
            string name = item.Name;
            bool val = items.Remove(item);
            if (owner != null)
                owner.OnEmulatorRemoved(name);
            return val;
        }
        /// <summary>
        /// Remove emulator from the collection
        /// </summary>
        /// <param name="itemID">The emulator id to remove</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(string itemID)
        {
            Emulator item = this[itemID];
            string name = item.Name;
            bool val = items.Remove(item);
            if (owner != null)
                owner.OnEmulatorRemoved(name);
            return val;
        }
        /// <summary>
        /// Remove emulators from the collection
        /// </summary>
        /// <param name="itemsToRemove">The emulators to remove</param>
        /// <returns>True if emulators removed otherwise false</returns>
        public void RemoveItems(EmulatorsCollection itemsToRemove)
        {
            foreach (Emulator it in itemsToRemove)
                items.Remove(this[it.ID]);
        }
        /// <summary>
        /// Remove emulators from the collection
        /// </summary>
        /// <param name="itemsToRemove">The emulators to remove</param>
        /// <returns>True if emulators removed otherwise false</returns>
        public void RemoveItems(Emulator[] itemsToRemove)
        {
            foreach (Emulator it in itemsToRemove)
                items.Remove(this[it.ID]);
        }
        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Emulator> GetEnumerator()
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
        public void Sort(EmulatorsComparer comparer)
        {
            items.Sort(comparer);
            if (owner != null)
                owner.OnEmulatorsSorted();
        }
    }
}
