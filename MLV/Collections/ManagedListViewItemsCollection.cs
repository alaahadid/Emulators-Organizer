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
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLV
{
    /// <summary>
    /// Managed ListView Items Collection
    /// </summary>
    public class ManagedListViewItemsCollection : List<ManagedListViewItem>
    {
        /// <summary>
        /// Managed ListView Items Collection. (This constructor will declare the parent control as null so no event raise)
        /// </summary>
        public ManagedListViewItemsCollection()
            : base()
        {
            this.owner = null;
        }
        /// <summary>
        /// Managed ListView Items Collection. (This constructor will declare the parent control as null so no event raise)
        /// </summary>
        /// <param name="newItems">The new items that will be added to the collection</param>
        public ManagedListViewItemsCollection(IEnumerable<ManagedListViewItem> newItems)
            : base(newItems)
        {
            this.owner = null;
        }
        /// <summary>
        /// Managed ListView Items Collection
        /// </summary>
        /// <param name="owner">The parent <see cref="MLV.ManagedListView"/> control</param>
        public ManagedListViewItemsCollection(ManagedListView owner)
            : base()
        {
            this.owner = owner;
        }
        /// <summary>
        /// Managed ListView Items Collection
        /// </summary>
        /// <param name="owner">The parent <see cref="MLV.ManagedListView"/> control</param>
        /// <param name="newItems">The new items that will be added to the collection</param>
        public ManagedListViewItemsCollection(ManagedListView owner, IEnumerable<ManagedListViewItem> newItems)
            : base(newItems)
        {
            this.owner = owner;
        }

        private ManagedListView owner;

        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="item"><see cref="ManagedListViewItem"/></param>
        public new void Add(ManagedListViewItem item)
        {
            base.Add(item);
            if (owner != null)
                owner.OnItemAdded();
        }
        public new void AddNoEvent(ManagedListViewItem item)
        {
            base.Add(item);
        }
        /// <summary>
        /// Clear this collection
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            if (owner != null)
                owner.OnItemsCollectionCleared();
        }
        /// <summary>
        /// Remove an item from this collection
        /// </summary>
        /// <param name="item"><see cref="ManagedListViewItem"/> to remove</param>
        /// <returns>True if removed successfully otherwise false.</returns>
        public new bool Remove(ManagedListViewItem item)
        {
            bool val = base.Remove(item);
            if (owner != null)
                owner.OnItemRemoved();
            return val;
        }
        /// <summary>
        /// Remove item at index
        /// </summary>
        /// <param name="index">The index to remove item at</param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            if (owner != null)
                owner.OnItemRemoved();
        }
        /// <summary>
        /// Insert item to this collection at given index
        /// </summary>
        /// <param name="index">The index to insert the item at</param>
        /// <param name="item">The item to insert</param>
        public new void Insert(int index, ManagedListViewItem item)
        {
            base.Insert(index, item);
            if (owner != null)
                owner.OnItemAdded();
        }
        /// <summary>
        /// Insert item to this collection at given index without raising the event.
        /// </summary>
        /// <param name="index">The index to insert the item at</param>
        /// <param name="item">The item to insert</param>
        public void InsertNoEvent(int index, ManagedListViewItem item)
        {
            base.Insert(index, item);
        }
        /// <summary>
        /// Sort the items collection
        /// </summary>
        public new void Sort()
        {
            base.Sort();
            if (owner != null)
                owner.OnItemsCollectionSorted();
        }
        /// <summary>
        /// Sort the items collection using comparer
        /// </summary>
        /// <param name="comparer">The comparer to use in compare operation</param>
        public new void Sort(IComparer<ManagedListViewItem> comparer)
        {
            base.Sort(comparer);
            if (owner != null)
                owner.OnItemsCollectionSorted();
        }
        /// <summary>
        /// Sort the items collection using comparer
        /// </summary>
        /// <param name="index">The start index to start with</param>
        /// <param name="count">The count of items</param>
        /// <param name="comparer">The comparer to use in compare operation</param>
        public new void Sort(int index, int count, IComparer<ManagedListViewItem> comparer)
        {
            base.Sort(index, count, comparer);
            if (owner != null)
                owner.OnItemsCollectionSorted();
        }
    }
}
