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
namespace MTC
{
    /// <summary>
    /// Managed Tab Control draw objects
    /// </summary>
    public struct MTCDrawPageObjects
    {
        /// <summary>
        /// Managed Tab Control draw objects
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="image">The image</param>
        public MTCDrawPageObjects(string text, Image image)
        {
            this.text = text;
            this.image = image;
        }
        private string text;
        private Image image;
        /// <summary>
        /// Get or set the text
        /// </summary>
        public string Text
        { get { return text; } set { text = value; } }
        /// <summary>
        /// Get or set the image
        /// </summary>
        public Image Image
        { get { return image; } set { image = value; } }
    }
}
