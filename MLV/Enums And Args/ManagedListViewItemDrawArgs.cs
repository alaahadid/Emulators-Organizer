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
using System.Drawing;
namespace MLV
{
    /// <summary>
    /// Arguments for item draw vents
    /// </summary>
    public class ManagedListViewItemDrawArgs : EventArgs
    {
        /// <summary>
        /// Arguments for item draw vents
        /// </summary>
        /// <param name="itemIndex">The target item index</param>
        public ManagedListViewItemDrawArgs(int itemIndex)
        {
            this.itemIndex = itemIndex;
        }

        private int itemIndex;
        private Image image;
        private string text;

        /// <summary>
        /// Get or set the text to draw 
        /// </summary>
        public string TextToDraw
        { get { return text; } set { text = value; } }
        /// <summary>
        /// Get or set the image to draw
        /// </summary>
        public Image ImageToDraw
        { get { return image; } set { image = value; } }
        /// <summary>
        /// Get the target item index
        /// </summary>
        public int ItemIndex
        { get { return itemIndex; } }
    }
}
