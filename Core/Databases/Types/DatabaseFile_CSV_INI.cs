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

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("CSV INI", new string[] { ".ini" }, CompareMode.RomFileName | CompareMode.RomName,
        true, true, false)]
    public class DatabaseFile_CSV : DatabaseFile
    {
        public bool _IgnoreEmptyFields = false;
        public bool _AddAllDataItems = false;
        public bool IncludeCategoriesOnExport = false;
        public override List<DBEntry> LoadFile(string filePath)
        {
            OnProgressStarted("Reading CSV INI file ...");
            //clear files
            List<DBEntry> files = new List<DBEntry>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);

            //decode lines
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))//we got a game here
                {
                    DBEntry ROM = DBEntry.Empty;
                    string name = lines[i].Replace("[", "").Replace("]", "");
                    ROM.Properties.Add(new PropertyStruct("Name", name));
                    ROM.FileNames.Add(name);
                    i++;
                    while (true)
                    {
                        if (lines[i].Contains("="))//we have a property here
                        {
                            string[] code = lines[i].Split(new char[] { '=' });
                            string prop = code[0];
                            string val = "";
                            if (code.Length > 1)
                                val = code[1].Replace("<br>", "\n");
                            if (prop == "Goodname" || prop == "NoIntro" || prop == "Tosec")
                                ROM.FileNames.Add(val);
                            if (prop == "Genre")
                                ROM.Categories.Add("Genre/" + val);
                            //else
                            ROM.Properties.Add(new PropertyStruct(prop, val));
                            i++;
                        }
                        else if (lines[i] == "")
                        {
                            files.Add(ROM);
                            break;
                        }
                    }
                }
                int x = (i * 100) / lines.Length;
                base.OnProgress(x, "Reading CSV file ... [" + x + " %]");
            }
            OnProgressFinished("CSV INI file read successfully.");
            return files;
        }
        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            base.OnProgressStarted("Saving CSV database file...");

            List<string> lines = new List<string>();
            int i = 0;
            foreach (DBEntry file in entries)
            {
                lines.Add("[" + file.GetPropertyValue("Name") + "]");
                if (!_AddAllDataItems)
                {
                    if (!_IgnoreEmptyFields)
                    {
                        lines.Add("Publisher=" + file.GetPropertyValue("Publisher"));
                        lines.Add("Developer=" + file.GetPropertyValue("Developer"));
                        lines.Add("Released=" + file.GetPropertyValue("Released"));
                        lines.Add("Systems=" + file.GetPropertyValue("Systems"));
                        if (IncludeCategoriesOnExport)
                        {
                            if (file.Categories != null)
                                if (file.Categories.Count > 0)
                                {
                                    string cats = "";
                                    foreach (string cat in file.Categories)
                                    {
                                        cats += cat + ", ";
                                    }
                                    lines.Add("Genre=" + cats);
                                }
                        }
                        else
                        {
                            lines.Add("Genre=" + file.GetPropertyValue("Genre"));
                        }
                        // lines.Add("Genre=" + (IncludeCategoriesOnExport ? file.Category : file.GetPropertyValue("Genre")));


                        lines.Add("Perspective=" + file.GetPropertyValue("Perspective"));
                        lines.Add("Score=" + file.GetPropertyValue("Score"));
                        lines.Add("Controls=" + file.GetPropertyValue("Controls"));
                        lines.Add("Players=" + file.GetPropertyValue("Players"));
                        lines.Add("Esrb=" + file.GetPropertyValue("Esrb"));
                        lines.Add("Url=" + file.GetPropertyValue("Url"));
                        lines.Add("Description=" + file.GetPropertyValue("Description").Replace("\n", "<br>"));
                        lines.Add("Goodname=" + file.GetPropertyValue("Goodname"));
                        lines.Add("NoIntro=" + file.GetPropertyValue("NoIntro"));
                        lines.Add("Tosec=" + file.GetPropertyValue("Tosec"));
                    }
                    else
                    {
                        if (file.GetPropertyValue("Publisher") != "")
                            lines.Add("Publisher=" + file.GetPropertyValue("Publisher"));
                        if (file.GetPropertyValue("Developer") != "")
                            lines.Add("Developer=" + file.GetPropertyValue("Developer"));
                        if (file.GetPropertyValue("Released") != "")
                            lines.Add("Released=" + file.GetPropertyValue("Released"));
                        if (file.GetPropertyValue("Systems") != "")
                            lines.Add("Systems=" + file.GetPropertyValue("Systems"));
                        if (file.GetPropertyValue("Genre") != "")
                            lines.Add("Genre=" + file.GetPropertyValue("Genre"));
                        if (file.GetPropertyValue("Perspective") != "")
                            lines.Add("Perspective=" + file.GetPropertyValue("Perspective"));
                        if (file.GetPropertyValue("Score") != "")
                            lines.Add("Score=" + file.GetPropertyValue("Score"));
                        if (file.GetPropertyValue("Controls") != "")
                            lines.Add("Controls=" + file.GetPropertyValue("Controls"));
                        if (file.GetPropertyValue("Players") != "")
                            lines.Add("Players=" + file.GetPropertyValue("Players"));
                        if (file.GetPropertyValue("Esrb") != "")
                            lines.Add("Esrb=" + file.GetPropertyValue("Esrb"));
                        if (file.GetPropertyValue("Url") != "")
                            lines.Add("Url=" + file.GetPropertyValue("Url"));
                        if (file.GetPropertyValue("Description") != "")
                            lines.Add("Description=" + file.GetPropertyValue("Description").Replace("\n", "<br>"));
                        if (file.GetPropertyValue("Goodname") != "")
                            lines.Add("Goodname=" + file.GetPropertyValue("Goodname"));
                        if (file.GetPropertyValue("NoIntro") != "")
                            lines.Add("NoIntro=" + file.GetPropertyValue("NoIntro"));
                        if (file.GetPropertyValue("Tosec") != "")
                            lines.Add("Tosec=" + file.GetPropertyValue("Tosec"));
                    }
                    lines.Add("");
                }
                else//add all of rom
                {
                    foreach (PropertyStruct pro in file.Properties)
                    {
                        if (pro.Property == "Name")
                            continue;
                        if (!_IgnoreEmptyFields)
                        {
                            lines.Add(pro.Property + "=" + pro.Value);
                        }
                        else
                        {
                            if (pro.Value != null)
                                if (pro.Value != "")
                                    lines.Add(pro.Property + "=" + pro.Value);
                        }
                    }
                    lines.Add("");
                }
                int x = (i * 100) / entries.Count;
                OnProgress(x, "Saving database file ... [" + x + " %]");
                i++;
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
            OnProgressFinished("CSV file saved successfully.");

        }
        public override SeparateItem[] GetSeparate(string filePath)
        {
            List<SeparateItem> items = new List<SeparateItem>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);
            OnProgressStarted("Saving CSV database file ...");
            //decode lines
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))//we got a game here
                {
                    SeparateItem item = new SeparateItem();
                    item.Data = lines[i] + "\n";
                    item.FileName = lines[i].Replace("[", "").Replace("]", "") + ".ini";
                    i++;
                    while (true)
                    {
                        item.Data += lines[i] + "\n";
                        if (lines[i].Contains("="))//we have a property here
                        {
                            i++;
                        }
                        else if (lines[i] == "")
                        {
                            items.Add(item);
                            break;
                        }
                    }
                }
                int x = (i * 100) / lines.Length;
                OnProgress(x, "Saving CSV database file ... [" + x + " %]");
            }
            OnProgressFinished("CSV database file saved successfully.");
            return items.ToArray();
        }
        public override void ShowExportOptions()
        {
            Frm_CsvExportOptions frm = new Frm_CsvExportOptions(this);
            frm.ShowDialog();
            _IgnoreEmptyFields = frm._IgnoreEmptyFields;
            _AddAllDataItems = frm._AddAllDataItems;
            IncludeCategoriesOnExport = frm.IncludeCategoriesOnExport;
        }
    }
}
