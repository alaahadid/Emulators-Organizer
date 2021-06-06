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

namespace MTC
{
    /// <summary>
    /// Args can be used in show tool tip args
    /// </summary>
    public class MTCTabPageShowToolTipArgs : EventArgs
    {
        /// <summary>
        /// Args can be used in show tool tip args
        /// </summary>
        /// <param name="pageID">The tab page id</param>
        /// <param name="pageIndex">The tab page index</param>
        /// <param name="toolTipText">The tool tip string that gonna show</param>
        public MTCTabPageShowToolTipArgs(string pageID, int pageIndex, string toolTipText)
        {
            this.pageID = pageID;
            this.pageIndex = pageIndex;
            this.toolTipText = toolTipText;
        }
        private string pageID;
        private int pageIndex;
        private string toolTipText;
        /// <summary>
        /// Get the target tab page id
        /// </summary>
        public string TabPageID
        { get { return pageID; } }
        /// <summary>
        /// Get the target tab page index within collection
        /// </summary>
        public int TabPageIndex
        { get { return pageIndex; } }
        /// <summary>
        /// Get or set the tool tip text that will be shown
        /// </summary>
        public string ToolTipText
        { get { return toolTipText; } set { toolTipText = value; } }
    }
}
