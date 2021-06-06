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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLV
{
    /// <summary>
    /// ManagedListView Columns Collection
    /// </summary>
    public class ManagedListViewColumnsCollection : List<ManagedListViewColumn>
    {
        /// <summary>
        /// ManagedListView Columns Collection. (This constructor will declare the parent control as null so no event raise)
        /// </summary>
        public ManagedListViewColumnsCollection()
            : base()
        {
            this.owner = null;
        }
        /// <summary>
        /// ManagedListView Columns Collection. (This constructor will declare the parent control as null so no event raise)
        /// </summary>
        /// <param name="newColumns">The new columns to be added to the collection</param>
        public ManagedListViewColumnsCollection(IEnumerable<ManagedListViewColumn> newColumns)
            : base(newColumns)
        {
            this.owner = null;
        }
        /// <summary>
        /// ManagedListView Columns Collection.
        /// </summary>
        /// <param name="owner">The parent <see cref="MLV.ManagedListView"/> control</param>
        public ManagedListViewColumnsCollection(ManagedListView owner)
            : base()
        {
            this.owner = owner;
        }
        /// <summary>
        /// ManagedListView Columns Collection.
        /// </summary>
        /// <param name="owner">The parent <see cref="MLV.ManagedListView"/> control</param>
        /// <param name="newColumns">The new columns to be added to the collection</param>
        public ManagedListViewColumnsCollection(ManagedListView owner, IEnumerable<ManagedListViewColumn> newColumns)
            : base(newColumns)
        {
            this.owner = owner;
        }

        private ManagedListView owner;

        /// <summary>
        /// Add column to this collection
        /// </summary>
        /// <param name="item"><see cref="ManagedListViewColumn"/></param>
        public new void Add(ManagedListViewColumn item)
        {
            base.Add(item);
            if (owner != null)
                owner.OnColumnAdded();
        }
        /// <summary>
        /// Insert column to this collection
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="item"><see cref="ManagedListViewColumn"/></param>
        public new void Insert(int index, ManagedListViewColumn item)
        {
            base.Insert(index, item);
            if (owner != null)
                owner.OnColumnAdded();
        }
        /// <summary>
        /// Clear this collection
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            if (owner != null)
                owner.OnColumnsCollectionCleared();
        }
        /// <summary>
        /// Remove a column from this collection
        /// </summary>
        /// <param name="item">The <see cref="ManagedListViewColumn"/> to remove</param>
        /// <returns>True if column removed successfully otherwise false.</returns>
        public new bool Remove(ManagedListViewColumn item)
        {
            bool val = base.Remove(item);
            if (owner != null)
                owner.OnItemRemoved();
            return val;
        }
        /// <summary>
        /// Remove column at given index
        /// </summary>
        /// <param name="index">The index of the column to remove</param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            if (owner != null)
                owner.OnItemRemoved();
        }
        /// <summary>
        /// Get column using given id
        /// </summary>
        /// <param name="id">The target column id</param>
        /// <returns>The column if found otherwise null.</returns>
        public ManagedListViewColumn GetColumnByID(string id)
        {
            foreach (ManagedListViewColumn column in this)
            {
                if (column.ID == id) return column;
            }
            return null;
        }
        /// <summary>
        /// Sort the columns collection
        /// </summary>
        public new void Sort()
        {
            base.Sort();
            if (owner != null)
                owner.OnItemsCollectionSorted();
        }
        /// <summary>
        /// Sort the columns collection using comparer
        /// </summary>
        /// <param name="comparer">The comparer to use in compare operation</param>
        public new void Sort(IComparer<ManagedListViewColumn> comparer)
        {
            base.Sort(comparer);
            if (owner != null)
                owner.OnItemsCollectionSorted();
        }
        /// <summary>
        /// Sort the columns collection using comparer
        /// </summary>
        /// <param name="index">The start index to start with</param>
        /// <param name="count">The count of columns</param>
        /// <param name="comparer">The comparer to use in compare operation</param>
        public new void Sort(int index, int count, IComparer<ManagedListViewColumn> comparer)
        {
            base.Sort(index, count, comparer);
            if (owner != null)
                owner.OnItemsCollectionSorted();
        }
    }
}
