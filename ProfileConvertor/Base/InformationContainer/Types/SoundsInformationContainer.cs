/* This file is part of Emulators Organizer
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
namespace AHD.EO.Base
{
    [Serializable(), Detectable(true), MultiDetect(), AttrHasControl(true)]
    public class SoundsInformationContainer : FilesInFolderInformationContainer
    {
        public SoundsInformationContainer()
            : base()
        { }
        public SoundsInformationContainer(string name)
            : base(name)
        { }
        public override string[] Extensions
        {
            get
            {
                return new string[]
            { ".mp3", ".ogg", ".wav", ".midi", ".flc", ".ape", ".wma", ".vox", ".tta", 
                ".raw", ".mpc", ".m4p", ".iklax", ".gsm",
                ".dct", ".awb", ".Au", ".atrac", ".amr", ".alac", ".aac", ".aiff", ".act" };
            }
        }
        public override string ToString()
        {
            return "Sounds";
        }
    }
}
