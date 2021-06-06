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
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace EmulatorsOrganizer.GUI
{
    class ListViewItemComparer : IComparer
    {
        private int col;
        private bool az = false;
        public ListViewItemComparer(int column, bool az)
        {
            this.col = column;
            this.az = az;
        }
        public int Compare(object x, object y)
        {
            ListViewItem item1 = ((ListViewItem)x);
            ListViewItem item2 = ((ListViewItem)y);
            int val = -1;
            switch (col)
            {
                case 0://name
                    val = String.Compare(item1.Text, item2.Text); break;
                case 1://type
                    val = String.Compare(item1.SubItems[1].Text, item2.SubItems[1].Text); break;
                case 2://location
                    val = String.Compare(item1.SubItems[2].Text, item2.SubItems[2].Text); break;
            }
            return az ? val : val * -1;
        }
    }
}
