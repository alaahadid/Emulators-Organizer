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

namespace EmulatorsOrganizer.Core
{
    [Serializable, InformationContainerDescription("Media", true, true)]
    public class InformationContainerMedia : InformationContainerFiles
    {
        public InformationContainerMedia(string id)
            : base(id)
        {
            this.AutoStart = true;
            this.ShowToolstrip = true;
            this.AutoHideToolstrip = true;
            this.ShowMediaControls = true;
            this.RepeatList = true;
        }

        public override string[] DefaultExtensions
        {
            get
            {
                return new string[]
            {   ".mp3", ".ogg", ".wav", ".midi", ".flc", ".ape", ".wma", ".vox", ".tta", 
                ".raw", ".mpc", ".m4p", ".iklax", ".gsm",".flac",
                ".dct", ".awb", ".Au", ".atrac", ".amr", ".alac", ".aac", ".aiff", ".act",".avi",
                ".mpeg", ".mpg", ".mpeg", ".mp4", ".3gp", ".mov", ".wmv", ".flv"  };
            }
        }
        public bool AutoStart { get; set; }
        public bool AutoHideToolstrip { get; set; }
        public bool ShowMediaControls { get; set; }
        public bool ShowToolstrip { get; set; }
        public bool RepeatList { get; set; }
        public System.Collections.Generic.List<int> ColumnWidths { get; set; }
    }
}
