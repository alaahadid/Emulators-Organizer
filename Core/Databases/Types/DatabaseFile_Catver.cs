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
using System.Diagnostics;
namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("Catveren", new string[] { ".ini" }, CompareMode.RomFileName | CompareMode.RomName)]
    public class DatabaseFile_Catver : DatabaseFile
    {
        public bool _removeOldCategoriesList = true;
        public override System.Windows.Forms.Control OptionsControl
        {
            get
            {
                return new DatabaseFileControl_Catveren(this);
            }
        }
        public override List<DBEntry> LoadFile(string filePath)
        {
            //clear files
            List<DBEntry> files = new List<DBEntry>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);
            OnProgressStarted("Reading Cateveren file ...");
            //decode lines
            for (int i = 0; i < lines.Length; i++)
            {
                string[] codes = lines[i].Split('=');
                if (codes == null)
                    goto progress;
                if (codes.Length != 2)
                    goto progress;
                // we have a game here

                DBEntry ROM = DBEntry.Empty;
                ROM.Properties.Add(new PropertyStruct("Category", codes[1]));
                ROM.FileNames = new List<string>();
                ROM.FileNames.Add(codes[0]);
                files.Add(ROM);

                progress:
                int x = (i * 100) / lines.Length;
                OnProgress(x, "Reading Cateveren file ... [" + x + " %]");
            }
            OnProgressFinished("Cateveren file read successfully.");
            return files;
        }
        public override void ApplyName(Rom rom, DBEntry entry, bool applyName, bool applyDataInfo)
        {
            if (_removeOldCategoriesList)
            {
                rom.Categories = new List<string>();

                Trace.WriteLine(">Rom old categories list cleared.", "Catveren database");
            }

            rom.Categories.Add(entry.GetPropertyValue("Category"));
            rom.Modified = true;
            Trace.WriteLine(">Rom categories updated.", "Catveren database");
        }
    }
}
