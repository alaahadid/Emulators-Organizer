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
    [Serializable, InformationContainerDescription("Info Text", true, true)]
    public class InformationContainerInfoText : InformationContainerFiles
    {
        public InformationContainerInfoText(string id)
            : base(id)
        {
            this.ShowToolstrip = true;
        }
        public override string[] DefaultExtensions
        {
            get { return new string[] { ".txt", ".rtf", ".doc", ".ini" }; }
        }
        public override string GetExtensionDialogFilter()
        {
            string filter = base.DisplayName + "|";
            foreach (string ex in DefaultExtensions)
            {
                filter += "*" + ex + ";";
            }
            // Add one by one !
            filter += "|Text File (*.txt)|*.txt;|RTF (*.rtf)|*.rtf;|DOC (*.doc)|*.doc;|INI (*.ini)|*.ini;";
            return filter.Substring(0, filter.Length - 1);
        }
        public bool ShowToolstrip { get; set; }
    }
}
