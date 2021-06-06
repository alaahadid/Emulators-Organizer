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
namespace EmulatorsOrganizer.Core
{
    [Serializable, InformationContainerDescription("Review/Score", true, false)]
    public class InformationContainerReviewScore : InformationContainer
    {
        public InformationContainerReviewScore(string id)
            : base(id)
        {
            this.ShowToolstrip = false;
            this.Fields = new List<string>();
            // Add some defaults ...
            this.Fields.Add("Game Play");
            this.Fields.Add("Music");
            this.Fields.Add("Graphics");
            this.Fields.Add("Levels Design");
            this.Fields.Add("Story Line");
        }
        public List<string> Fields { get; set; }
        public bool ShowToolstrip { get; set; }
    }
}
