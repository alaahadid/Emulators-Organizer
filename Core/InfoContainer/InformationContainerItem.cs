﻿// This file is part of Emulators Organizer
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
/*Rom should contains a collection of this ....*/
namespace EmulatorsOrganizer.Core
{
    [Serializable]
    public abstract class InformationContainerItem
    {
        public InformationContainerItem(string id, string parentID)
        {
            this.id = id;
            this.parentID = parentID;
        }
        protected string id;
        protected string parentID;

        // Properties
        /// <summary>
        /// Get or set the id of this item
        /// </summary>
        public string ID
        { get { return id; } set { id = value; } }
        /// <summary>
        /// Get or set the parent id of this item
        /// </summary>
        public string ParentID
        { get { return parentID; } set { parentID = value; } }
    }
}
