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

namespace MLV
{
    /// <summary>
    /// The items comparer class for comparing Managed ListView items via column id.
    /// </summary>
    public class ManagedListViewItemsComparer : IComparer<ManagedListViewItem>
    {
        /// <summary>
        /// The items comparer class for comparing Managed ListView items via column id.
        /// </summary>
        /// <param name="AtoZ">True= A to Z sort, False=Z to A sort</param>
        /// <param name="subitemId">The column id of the subitem.</param>
        public ManagedListViewItemsComparer(bool AtoZ, string subitemId)
        {
            this.AtoZ = AtoZ;
            this.subitemId = subitemId;
        }

        private bool AtoZ = true;
        private string subitemId = "";

        /// <summary>
        /// Compare 2 items depending on subitem
        /// </summary>
        /// <param name="x">The first item</param>
        /// <param name="y">The second item</param>
        /// <returns>Compare result.</returns>
        public int Compare(ManagedListViewItem x, ManagedListViewItem y)
        {
            if (x.GetSubItemByID(subitemId) != null && y.GetSubItemByID(subitemId) != null)
            {
                if (AtoZ)
                    return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.GetSubItemByID(subitemId).Text, y.GetSubItemByID(subitemId).Text);
                else
                    return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.GetSubItemByID(subitemId).Text, y.GetSubItemByID(subitemId).Text));
            }
            return -1;
        }
    }
}
