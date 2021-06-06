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
    public class ManagedListViewShowThumbnailTooltipArgs : EventArgs
    {
        public ManagedListViewShowThumbnailTooltipArgs(int index, string text, Rectangle rect)
        {
            this.ItemsIndex = index;
            this.TextToShow = text;
            this.TextThumbnailRectangle = rect;
            this.Rating = -1;
        }
        public ManagedListViewShowThumbnailTooltipArgs(int index, string text, int rating, Rectangle rect)
        {
            this.ItemsIndex = index;
            this.TextToShow = text;
            this.TextThumbnailRectangle = rect;
            this.Rating = rating;
        }
        public int ItemsIndex { get; private set; }
        public Rectangle TextThumbnailRectangle { get; private set; }
        public string TextToShow { get; set; }
        public int Rating { get; set; }
    }
}
