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
using System.Text;
using System.Diagnostics;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("CSV", new string[] { ".csv" }, CompareMode.RomFileName | CompareMode.RomName, true, false, false)]
    public class DatabaseFile_CSV_NEW : DatabaseFile
    {
        public string SPLITTER = ">";
        public bool IncludeCategoriesOnExport;
        public bool UseAlternativeNameForComparing = true;
        public bool UseAlternativeNameForApplyingName = false;

        public override System.Windows.Forms.Control OptionsControl
        {
            get
            {
                return new DatabaseFileControl_CSVNew(this);
            }
        }

        public override List<DBEntry> LoadFile(string filePath)
        {
            OnProgressStarted("Reading CSV file ...");
            //clear files
            List<DBEntry> files = new List<DBEntry>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);

            // DECODE
            for (int i = 0; i < lines.Length; i++)
            {
                DBEntry ROM = DBEntry.Empty;
                string[] line_codes = lines[i].Split(new string[] { SPLITTER }, System.StringSplitOptions.None);

                if (line_codes != null)
                {
                    if (line_codes.Length >= 10)
                    {
                        // NAME
                        ROM.Properties.Add(new PropertyStruct("Name", line_codes[0]));
                        if (!UseAlternativeNameForComparing)
                            ROM.FileNames.Add(line_codes[0]);
                        // ALT NAME
                        ROM.Properties.Add(new PropertyStruct("Alternative Name", line_codes[1]));
                        if (UseAlternativeNameForComparing)
                            ROM.FileNames.Add(line_codes[1]);
                        // Release Year
                        ROM.Properties.Add(new PropertyStruct("Release Year", line_codes[2]));
                        // ESRB Rating
                        ROM.Properties.Add(new PropertyStruct("ESRB Rating", line_codes[3]));
                        // Publisher
                        ROM.Properties.Add(new PropertyStruct("Publisher", line_codes[4]));
                        // Developer
                        ROM.Properties.Add(new PropertyStruct("Developer", line_codes[5]));
                        // Genre
                        ROM.Properties.Add(new PropertyStruct("Genre", line_codes[6]));
                        //ROM.Category = line_codes[6];
                        ROM.Categories.Add("Genre/" + line_codes[6]);
                        // Score
                        ROM.Properties.Add(new PropertyStruct("Score", line_codes[7]));
                        // Score
                        ROM.Properties.Add(new PropertyStruct("Players", line_codes[8]));
                        // Description
                        ROM.Properties.Add(new PropertyStruct("Description", line_codes[9].Replace("****", "\n")));
                        // Add it !!
                        files.Add(ROM);
                    }
                }
                int x = (i * 100) / lines.Length;
                base.OnProgress(x, "Reading CSV file ... [" + x + " %]");
            }

            OnProgressFinished("CSV file read successfully.");
            return files;
        }
        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });

            OnProgressStarted("Saving CSV database file ...");
            // GAMES !!
            int i = 0;
            List<string> lines = new List<string>();
            foreach (DBEntry rom in entries)
            {
                string cats = "";
                if (IncludeCategoriesOnExport)
                {
                    if (rom.Categories != null)
                        if (rom.Categories.Count > 0)
                        {
                            foreach (string cat in rom.Categories)
                            {
                                cats += cat + ", ";
                            }
                        }
                }
                lines.Add(string.Format("{0}" + SPLITTER + "{1}" + SPLITTER +
                    "{2}" + SPLITTER + "{3}" + SPLITTER + "{4}" + SPLITTER + "{5}" + SPLITTER + "{6}" + SPLITTER + "{7}" + SPLITTER + "{8}" + SPLITTER + "{9}",
                    rom.GetPropertyValue("Name"),
                    rom.GetPropertyValue("Alternative Name"),
                    rom.GetPropertyValue("Release Year"),
                    rom.GetPropertyValue("ESRB Rating"),
                    rom.GetPropertyValue("Publisher"),
                    rom.GetPropertyValue("Developer"),
                    IncludeCategoriesOnExport ? cats : rom.GetPropertyValue("Genre"),
                    rom.GetPropertyValue("Score"),
                    rom.GetPropertyValue("Players"),
                    rom.GetPropertyValue("Description").Replace("\n", "****")));
                // Progress
                int p = (i * 100) / entries.Count;
                OnProgress(p, "Saving CSV database file [" + p + " %]");
                i++;
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
            OnProgressFinished("CSV database saved successfully.");
        }
        public override void ShowExportOptions()
        {
            base.ShowExportOptions();
            Frm_CSVNewExport frm = new Frm_CSVNewExport(this);
            frm.ShowDialog();
        }
        public override void ApplyName(Rom rom, DBEntry entry, bool applyName, bool applyDataInfo)
        {
            // Renaming
            if (applyName)
            {
                if (UseAlternativeNameForApplyingName)
                {
                    rom.Name = entry.GetPropertyValue("Alternative Name");

                    Trace.WriteLine("->Rom renamed to: " + rom.Name, "HyperList XML database");

                }
                else
                {
                    rom.Name = entry.GetPropertyValue("Name");

                    Trace.WriteLine("->Rom renamed to: " + rom.Name, "HyperList XML database");
                }
            }
            if (applyDataInfo)
            {
                // Database info items
                foreach (PropertyStruct p in entry.Properties)
                {
                    if (!UseAlternativeNameForApplyingName)
                    {
                        if (p.Property != "Name")
                        {
                            AddNewIC(p.Property, rom.ParentConsoleID);
                            AddDataToRom(p.Property, p.Value, rom);
                        }
                    }
                    else
                    {
                        if (p.Property != "Alternative Name")
                        {
                            AddNewIC(p.Property, rom.ParentConsoleID);
                            AddDataToRom(p.Property, p.Value, rom);

                            Trace.WriteLine("->>> IC ADDED: " + p.Property + " = " + p.Value, "HyperList XML database");
                        }
                    }
                }
            }
        }
    }
}
