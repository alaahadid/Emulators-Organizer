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

namespace MTC
{
    public class ManagedControlsSpliterPanelsCollection : IList<ManagedControlsSpliterPanel>
    {
        public ManagedControlsSpliterPanelsCollection(ManagedControlsSpliter owner)
        {
            this.owner = owner;
        }

        private ManagedControlsSpliter owner;
        private List<ManagedControlsSpliterPanel> items = new List<ManagedControlsSpliterPanel>();

        public int IndexOf(ManagedControlsSpliterPanel item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, ManagedControlsSpliterPanel item)
        {
            items.Insert(index, item);
            owner.OnPanelAdd(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            owner.OnPanelRemove();
        }

        public ManagedControlsSpliterPanel this[int index]
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

        public void Add(ManagedControlsSpliterPanel item)
        {
            items.Add(item);
            owner.OnPanelAdd(item);
        }

        public void Clear()
        {
            items.Clear();
            owner.OnPanelsClear();
        }

        public bool Contains(ManagedControlsSpliterPanel item)
        {
            return items.Contains(item);
        }

        public void CopyTo(ManagedControlsSpliterPanel[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ManagedControlsSpliterPanel item)
        {
            bool val = items.Remove(item);
            owner.OnPanelRemove();
            return val;
        }

        public IEnumerator<ManagedControlsSpliterPanel> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
