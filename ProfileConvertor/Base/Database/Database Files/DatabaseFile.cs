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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHD.EO.Base
{
    public interface IDatabaseFile
    {
        /// <summary>
        /// Get the name of this database file
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Get the files (or roms) of this database file
        /// </summary>
        List<DatabaseRom> Files { get; set; }
        /// <summary>
        /// Open a database file
        /// </summary>
        /// <param name="fileName">The full path of the database</param>
        /// <returns>True if open success, false if not</returns>
        bool OpenFile(string fileName);
        /// <summary>
        /// Save database file, Files property must be determined first
        /// </summary>
        /// <param name="fileName">The full path of the database</param>
        /// <returns>True if done, false if not</returns>
        bool SaveFile(string fileName);

        /// <summary>
        /// Get the supported extension for this database file
        /// </summary>
        string Extension { get; }
        /// <summary>
        /// Rised up when a progress happens
        /// </summary>
        event EventHandler<ProgressArg> Progress;

        bool IsSupportCRC { get; }
        bool IsSupportSHA1 { get; }
        bool IsSupportMD5 { get; }
        bool IsSupportFileNameCompare { get; }
        bool IsSupportSave { get; }
        bool IsSupportSeparate { get; }

        string CalculateSHA1(string filePath);
        string CalculateMD5(string filePath);
        string CalculateCRC(string filePath);
        string CalculateCRCWithoutSkip(string filePath);
        SeparateItem[] GetSeparate(string filePath);
    }
}