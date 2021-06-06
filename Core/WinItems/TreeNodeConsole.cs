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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Tree node holds console
    /// </summary>
    public class TreeNodeConsole : EOTreeNode
    {
        /// <summary>
        /// Tree node holds console
        /// </summary>
        /// <param name="id">The console id</param>
        public TreeNodeConsole(string id)
            : base(id)
        {
            console = profileManager.Profile.Consoles[id];
            RefreshText();
        }
        private Core.Console console;
        public Core.Console Console
        { get { return console; } set { console = value; } }
        /// <summary>
        /// Refresh text of this node
        /// </summary>
        public override void RefreshText()
        {
            this.Text = console.Name;
        }
        /// <summary>
        /// Refresh nodes of this node
        /// </summary>
        public override void RefreshNodes()
        {
            base.RefreshNodes();
        }
    }
}
