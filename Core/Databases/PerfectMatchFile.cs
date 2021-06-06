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

namespace EmulatorsOrganizer
{
    public class PerfectMatchFile
    {
        public PerfectMatchFile()
        {
        }
        public PerfectMatchFile(string name, string merge, string crc, string sha1)
        {
            this.Name = name;
            this.Merge = merge;
            this.CRC = crc;
            this.SHA1 = sha1;
        }
        public string Name;
        public string Merge;
        public string CRC;
        public string SHA1;

        public PerfectMatchFile Clone()
        {
            return new PerfectMatchFile(Name, Merge, CRC, SHA1);
        }
    }
}
