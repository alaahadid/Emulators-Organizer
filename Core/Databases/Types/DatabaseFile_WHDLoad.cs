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
using System.Linq;
using System.Text;
using System.IO;
using SevenZip;
namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("WHD Load", new string[] { ".map" }, CompareMode.RomFileName | CompareMode.RomName,
        true, false, false)]
    public class DatabaseFile_WHDLoad : DatabaseFile
    {
        public override List<DBEntry> LoadFile(string filePath)
        {
            //clear files
            List<DBEntry> files = new List<DBEntry>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);
            OnProgressStarted("Reading WHD Load file ...");
            //decode lines
            for (int i = 0; i < lines.Length; i++)
            {
                string[] code = lines[i].Split(new char[] { '|' });
                if (code.Length > 1)
                {
                    DBEntry ROM = DBEntry.Empty;
                    //  ROM.Name = code[1];
                    ROM.Properties.Add(new PropertyStruct("Name", code[1]));
                    ROM.FileNames.Add(code[0]);

                    files.Add(ROM);
                }
                int x = (i * 100) / lines.Length;
                OnProgress(x, "Reading WHD Load file ... [" + x + " %]");
            }
            OnProgressFinished("WHD Load file read successfully.");
            return files;
        }
        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            OnProgressStarted("Saving WHD Load database file...");
            List<string> lines = new List<string>();
            int i = 0;
            foreach (DBEntry file in entries)
            {
                if (HelperTools.IsAIPath(file.TemporaryPathForExport))
                {
                    SevenZipExtractor extractor =
                          new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(file.TemporaryPathForExport)));
                    int index = HelperTools.GetIndexFromAIPath(file.TemporaryPathForExport);
                    lines.Add(extractor.ArchiveFileData[index] + "|" + file.GetPropertyValue("Name"));
                }
                else
                {
                    lines.Add(Path.GetFileName(file.TemporaryPathForExport) + "|" + file.GetPropertyValue("Name"));
                }
                int p = (i * 100) / entries.Count;
                OnProgress(p, "Saving WHD Load database file [" + p + " %]");
                i++;
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
            OnProgressFinished("WHD Load database file saved successfully.");

        }
    }
}
