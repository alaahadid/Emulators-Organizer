﻿/* This file is part of Emulators Organizer
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
    [Serializable()]
    public abstract class ICommandline
    {
        private string name = "";
        private string code = "";
        private bool isReadOnly = false;
        private bool enabled = true;

        public virtual string Name
        { get { return name; } set { name = value; } }
        public virtual string Code
        { get { return code; } set { code = value; } }
        public virtual bool Enabled
        { get { return enabled; } set { enabled = value; } }
        public virtual bool IsReadOnly
        { get { return isReadOnly; } set { isReadOnly = value; } }
    }
}
