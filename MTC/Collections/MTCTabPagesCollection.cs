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

namespace MTC
{
    /// <summary>
    /// The Managed Tab Control Tab Pages Collection. Should be used only with Managed Tab Control controls.
    /// </summary>
    [Serializable]
    public class MTCTabPagesCollection : List<MTCTabPage>
    {
        /// <summary>
        /// The Managed Tab Control Tab Pages Collection. Should be used only with Managed Tab Control controls.
        /// </summary>
        public MTCTabPagesCollection()
        {

        }
        /// <summary>
        /// The Managed Tab Control Tab Pages Collection. Should be used only with Managed Tab Control controls.
        /// </summary>
        /// <param name="owner">The <see cref="ManagedTabControlPanel"/>, the parent of this collection.</param>
        public MTCTabPagesCollection(ManagedTabControlPanel owner)
        {
            this.owner = owner;
        }

        private ManagedTabControlPanel owner;
        /// <summary>
        /// Insert an item to the collection
        /// </summary>
        /// <param name="index">The index where to insert</param>
        /// <param name="item">The item to insert</param>
        public new void Insert(int index, MTCTabPage item)
        {
            base.Insert(index, item);
            owner.OnTabPagesItemAdded();
        }
        /// <summary>
        /// Remove item at index
        /// </summary>
        /// <param name="index">The index to remove at</param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            owner.OnTabPagesItemRemoved();
            if (base.Count == 0)
                owner.OnTabPagesCollectionClear();
        }
        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="item">The item to add</param>
        public new void Add(MTCTabPage item)
        {
            base.Add(item);
            owner.OnTabPagesItemAdded();
        }
        /// <summary>
        /// Clear the collection
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            owner.OnTabPagesCollectionClear();
        }
        /// <summary>
        /// Remove item from the collection
        /// </summary>
        /// <param name="item">The item to remove</param>
        public new void Remove(MTCTabPage item)
        {
            base.Remove(item);
            owner.OnTabPagesItemRemoved();
            if (base.Count == 0)
                owner.OnTabPagesCollectionClear();
        }
        /// <summary>
        /// Get or set the owner control
        /// </summary>
        public ManagedTabControlPanel OWNER
        { get { return owner; } set { owner = value; } }
    }
}
