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
using System.Drawing;

namespace MLV
{
    /// <summary>
    /// Arguments for mouse over subitem events.
    /// </summary>
    public class ManagedListViewMouseOverSubItemArgs : EventArgs
    {
        /// <summary>
        /// Arguments for mouse over subitem events.
        /// </summary>
        /// <param name="itemIndex">The target item index.</param>
        /// <param name="columnID">The column id which the subitem belong to.</param>
        /// <param name="mouseX">The mouse x coordinate value in the panel (not the view port).</param>
        /// <param name="subitemRectangle">The rectangle that represent the subitem</param>
        /// <param name="text">The subitem text</param>
        /// <param name="isTextFullyVisible">Indicates if the subitem's text is fully visible.</param>
        public ManagedListViewMouseOverSubItemArgs(int itemIndex, string columnID, int mouseX, Rectangle subitemRectangle,
            string text, bool isTextFullyVisible)
        {
            this.itemIndex = itemIndex;
            this.columnID = columnID;
            this.mouseX = mouseX;
            this.text = text;
            this.isTextFullyVisible = isTextFullyVisible;
            this.subitemRectangle = subitemRectangle;
        }

        private int itemIndex = -1;
        private string columnID = "";
        private int mouseX = 0;
        private Rectangle subitemRectangle;
        private string text;
        private bool isTextFullyVisible;

        /// <summary>
        /// Get the column id which the subitem belong to.
        /// </summary>
        public string ColumnID
        { get { return columnID; } }
        /// <summary>
        /// Get the parent item index.
        /// </summary>
        public int ItemIndex
        { get { return itemIndex; } }
        /// <summary>
        /// The mouse x coordinate value in the panel (not the view port).
        /// </summary>
        public int MouseX
        { get { return mouseX; } }
        /// <summary>
        /// The subitem text
        /// </summary>
        public string SubitemText
        { get { return text; } }
        /// <summary>
        /// Indicates if the subitem's text is fully visible.
        /// </summary>
        public bool IsTextFullyVisible
        { get { return isTextFullyVisible; } }
        /// <summary>
        /// Get the subitem Rectangle
        /// </summary>
        public Rectangle SubitemRectangle { get { return subitemRectangle; } }
    }
}
