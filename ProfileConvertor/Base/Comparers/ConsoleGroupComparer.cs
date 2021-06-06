/* This file is part of Emulators Organizer
   A program that can organize roms and emulators

   Copyright © Ali Hadid and Ala Hadid 2009 - 2013

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHD.EO.Base
{
    public class ConsoleGroupComparer : IComparer<ConsolesGroup>
    {
        bool AtoZ = true;
        public ConsoleGroupComparer(bool AtoZ)
        {
            this.AtoZ = AtoZ;
        }
        public int Compare(ConsolesGroup x, ConsolesGroup y)
        {
            if (AtoZ)
                return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name);
            else
                return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name));
        }
    }
}
