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
    /// Args that can be used in tab page drag events
    /// </summary>
    public class MTCTabPageDragArgs : EventArgs
    {   
        /// <summary>
        /// Args can be used in show tool tip args
        /// </summary>
        /// <param name="pageID">The tab page id</param>
        /// <param name="pageIndex">The tab page index</param>
        public MTCTabPageDragArgs(string pageID, int pageIndex)
        {
            this.pageID = pageID;
            this.pageIndex = pageIndex;
        }
        private string pageID;
        private int pageIndex;
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
    }
}
