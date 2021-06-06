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
using System.ComponentModel;
namespace MLV
{
    /// <summary>
    /// The Advanced ListView Subitem that can be used in Advanced ListView items.
    /// </summary>
    [System.Serializable]
    public class ManagedListViewSubItem : IManagedListViewItem
    {
        string columnID = "";

        /// <summary>
        /// Get or set the column id that this subitem belongs to. This value is important and this subitem will 
        /// NOT GET DROWN until this value is set correctly. Use the same value of ManagedListViewColumn.ID
        /// </summary>
        [Description("The column id. You must set this value to a Column.ID in order to display it under that column."), Category("Style")]
        public string ColumnID
        { get { return columnID; } set { columnID = value; } }

        /// <summary>
        /// Get an exact clone of this sub item
        /// </summary>
        /// <returns>An exact clone of this sub item</returns>
        public ManagedListViewSubItem Clone()
        {
            ManagedListViewSubItem newItem = new ManagedListViewSubItem();
            newItem.Color = this.Color;
            newItem.ColumnID = this.ColumnID;
            newItem.CustomFont = this.CustomFont;
            newItem.CustomFontEnabled = this.CustomFontEnabled;
            newItem.DrawMode = this.DrawMode;
            newItem.ImageIndex = this.ImageIndex;
            newItem.Tag = this.Tag;
            newItem.Text = this.Text;
            return newItem;
        }
    }
}
