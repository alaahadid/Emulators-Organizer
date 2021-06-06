﻿/* This file is part of Emulators Organizer
   A program that can organize roms and emulators

   Copyright © Ali Hadid and Ala Hadid 2009 - 2013

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
namespace AHD.EO.Base
{
    [Serializable()]
    public class InformationContainerYoutubeVideoItem : InformationContainerItem
    {
        public InformationContainerYoutubeVideoItem()
            : base()
        { }
        public InformationContainerYoutubeVideoItem(string id)
            : base(id)
        { }

        private List<YoutubeLink> youtubeVideos = new List<YoutubeLink>();

        /// <summary>
        /// Get or set the youtube videos collection
        /// </summary>
        public List<YoutubeLink> Videos
        { get { return youtubeVideos; } set { youtubeVideos = value; } }
    }
    [Serializable()]
    public class YoutubeLink
    {
        private string name = "";
        private string link = "";

        /// <summary>
        /// Get or set the name of this video
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the original link of this video
        /// </summary>
        public string Link
        { get { return link; } set { link = value; } }
    }
}
