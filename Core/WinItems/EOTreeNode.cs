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
using System.Windows.Forms;
using EmulatorsOrganizer.Services;
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Emulators Organizer Tree Node
    /// </summary>
    public abstract class EOTreeNode : TreeNode
    {
        /// <summary>
        /// Emulators Organizer Tree Node
        /// </summary>
        /// <param name="id">The IEOElemnt id</param>
        public EOTreeNode(string id)
            : base()
        {
            this.id = id;
            profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        }
        /// <summary>
        /// The profile manager service
        /// </summary>
        protected ProfileManager profileManager;
        /// <summary>
        /// The IEOElemnt id
        /// </summary>
        protected string id = "";
        /// <summary>
        /// The IEOElemnt id
        /// </summary>
        public virtual string ID { get { return id; } set { id = value; } }
        /// <summary>
        /// Refresh text of this node
        /// </summary>
        public abstract void RefreshText();
        /// <summary>
        /// Refresh nodes of this node
        /// </summary>
        public virtual void RefreshNodes() { }
    }
}
