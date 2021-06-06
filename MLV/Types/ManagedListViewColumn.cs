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
using System.Drawing;
using System.ComponentModel;
namespace MLV
{
    /// <summary>
    /// Advanced ListView column.
    /// </summary>
    public class ManagedListViewColumn
    {
        private string text = "";
        private string id = "";
        private ManagedListViewSortMode sortMode = ManagedListViewSortMode.None;
        private int width = 60;
        private Color textColor = Color.Black;

        /// <summary>
        /// Get or set the header text of this column.
        /// </summary>
        [Description("The column text that displayed."), Category("Text")]
        public string HeaderText
        { get { return text; } set { text = value; } }
        /// <summary>
        /// Get or set the header text color of this column.
        /// </summary>
        [Description("The column text color."), Category("Style")]
        public Color HeaderTextColor
        { get { return textColor; } set { textColor = value; } }
        /// <summary>
        /// Get or set the sortmode for this column that will be used to sort items that connected to this column using id.
        /// </summary>
        [Description("The sort mode for this column."), Category("Misc")]
        public ManagedListViewSortMode SortMode
        { get { return sortMode; } set { sortMode = value; } }
        /// <summary>
        /// Get or set the id of this column. Use this to connect subitems to this column.
        /// </summary>
        [Description("The id of this column. Must be used in subitems in order to disblay them under this column."), Category("Misc")]
        public string ID
        { get { return id; } set { id = value; } }
        /// <summary>
        /// Get or set the width of this column.
        /// </summary>
        [Description("The width in pixels"), Category("Style")]
        public int Width
        { get { return width; } set { width = value; } }
        /// <summary>
        /// Ceate an exact clone of this column
        /// </summary>
        /// <returns>An exact clone of this column</returns>
        public ManagedListViewColumn Clone()
        {
            ManagedListViewColumn newItem = new ManagedListViewColumn();
            newItem.HeaderText = this.HeaderText;
            newItem.HeaderTextColor = this.HeaderTextColor;
            newItem.ID = this.ID;
            newItem.SortMode = this.SortMode;
            newItem.Width = this.Width;
            return newItem;
        }
    }
}
