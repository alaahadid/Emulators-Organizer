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
namespace EmulatorsOrganizer.Core
{
    [Serializable, InformationContainerDescription("Image", true, true)]
    public class InformationContainerImage : InformationContainerFiles
    {
        public InformationContainerImage(string id)
            : base(id)
        {
            this.GoogleImageSearchFolder = ".\\GoogleImages\\";
            this.PreferedImageMode = 1;
            this.ShowToolBar = this.ShowStatusBar = true;
            this.UseNearestNighborDraw = true;
        }
        public override string[] DefaultExtensions
        {
            get { return new string[] { ".jpg", ".png", ".bmp", ".gif", ".jpeg", ".tiff", ".tif", ".tga", ".ico" }; }
        }
        // Let's use these to save settings here
        public int PreferedImageMode { get; set; }
        public string GoogleImageSearchFolder { get; set; }
        public bool ShowStatusBar { get; set; }
        public bool ShowToolBar { get; set; }
        public bool UseNearestNighborDraw { get; set; }
    }
}
