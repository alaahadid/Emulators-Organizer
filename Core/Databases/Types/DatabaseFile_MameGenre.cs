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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("Mame Genre / Category", new string[] { ".ini" }, CompareMode.RomFileName | CompareMode.RomName)]
    public class DatabaseFile_MameGenre : DatabaseFile
    {
        public override List<DBEntry> LoadFile(string filePath)
        {
            //clear files
            List<DBEntry> files = new List<DBEntry>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);
            OnProgressStarted("Reading Mame Genre / Category file ...");
            //decode lines
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(";"))
                    goto progress;
                if (lines[i].Length == 0)
                    goto progress;
                if (lines[i].StartsWith("["))
                {
                    if (lines[i] == "[FOLDER_SETTINGS]")
                        goto progress;
                    if (lines[i] == "[ROOT_FOLDER]")
                        goto progress;
                    // Decode category
                    string cat = lines[i].Replace("[","").Replace("]", "");
                    i++;
                    while (i < lines.Length)
                    {
                        if (lines[i] == "")
                            break;
                        DBEntry ROM = DBEntry.Empty;
                        ROM.Properties.Add(new PropertyStruct("Name", lines[i]));
                        ROM.Properties.Add(new PropertyStruct("Category", cat));
                        //ROM.Category = cat;
                        ROM.Categories.Add("Category/" + cat);
                        ROM.FileNames = new List<string>();
                        ROM.FileNames.Add(lines[i]);
                        files.Add(ROM);
                        i++;
                        if (i >= lines.Length)
                            break;
                    }
                }

            progress:
                int x = (i * 100) / lines.Length;
                OnProgress(x, "Reading Mame Genre / Category file ... [" + x + " %]");
            }
            OnProgressFinished("Mame Genre / Category file read successfully.");
            return files;
        }
        public override Control OptionsControl
        {
            get { return new DatabaseFileControl_MameGenre(this); }
        }
    }
}
