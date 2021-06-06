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
using System.IO;
namespace AHD.EO.Base
{
    public class DB_CSV : IDatabaseFile
    {
        List<DatabaseRom> files = new List<DatabaseRom>();
        public string Name
        {
            get { return "Csv system ini files (*.ini)"; }
        }
        public List<DatabaseRom> Files
        {
            get { return files; }
            set { files = value; }
        }
        public bool _IgnoreEmptyFields = false;
        public bool _AddAllDataItems = false;

        public bool OpenFile(string fileName)
        {
            //clear files
            files = new List<DatabaseRom>();
            //read lines
            string[] lines = File.ReadAllLines(fileName);

            //decode lines
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))//we got a game here
                {
                    DatabaseRom ROM = new DatabaseRom();
                    ROM.Name = lines[i].Replace("[", "").Replace("]", "");
                    ROM.FileNames = new List<string>();
                    ROM.FileNames.Add(ROM.Name);
                    ROM.Properties = new List<DBProperty>();
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
                            //else
                            ROM.Properties.Add(new DBProperty(prop, val));
                            i++;
                        }
                        else if (lines[i] == "")
                        {
                            files.Add(ROM);
                            break;
                        }
                    }
                }
                if (Progress != null)
                    Progress(this, new ProgressArg((i * 100) / lines.Length, "Reading ini file ..."));
            }
            return files.Count > 0;
        }
        public bool SaveFile(string fileName)
        {
            if (Progress != null)
                Progress(this, new ProgressArg(0, "Saving database file..."));
            Frm_CsvExportOptions frm = new Frm_CsvExportOptions(this);
            frm.ShowDialog();
            List<string> lines = new List<string>();
            int i = 0;
            foreach (DatabaseRom file in files)
            {
                lines.Add("[" + file.Name + "]");
                if (!_AddAllDataItems)
                {
                    if (!_IgnoreEmptyFields)
                    {
                        lines.Add("Publisher=" + GetValue("Publisher", file));
                        lines.Add("Developer=" + GetValue("Developer", file));
                        lines.Add("Released=" + GetValue("Released", file));
                        lines.Add("Systems=" + GetValue("Systems", file));
                        lines.Add("Genre=" + GetValue("Genre", file));
                        lines.Add("Perspective=" + GetValue("Perspective", file));
                        lines.Add("Score=" + GetValue("Score", file));
                        lines.Add("Controls=" + GetValue("Controls", file));
                        lines.Add("Players=" + GetValue("Players", file));
                        lines.Add("Esrb=" + GetValue("Esrb", file));
                        lines.Add("Url=" + GetValue("Url", file));
                        lines.Add("Description=" + GetValue("Description", file).Replace("\n", "<br>"));
                        lines.Add("Goodname=" + GetValue("Goodname", file));
                        lines.Add("NoIntro=" + GetValue("NoIntro", file));
                        lines.Add("Tosec=" + GetValue("Tosec", file));
                    }
                    else
                    {
                        if (GetValue("Publisher", file) != "")
                            lines.Add("Publisher=" + GetValue("Publisher", file));
                        if (GetValue("Developer", file) != "")
                            lines.Add("Developer=" + GetValue("Developer", file));
                        if (GetValue("Released", file) != "")
                            lines.Add("Released=" + GetValue("Released", file));
                        if (GetValue("Systems", file) != "")
                            lines.Add("Systems=" + GetValue("Systems", file));
                        if (GetValue("Genre", file) != "")
                            lines.Add("Genre=" + GetValue("Genre", file));
                        if (GetValue("Perspective", file) != "")
                            lines.Add("Perspective=" + GetValue("Perspective", file));
                        if (GetValue("Score", file) != "")
                            lines.Add("Score=" + GetValue("Score", file));
                        if (GetValue("Controls", file) != "")
                            lines.Add("Controls=" + GetValue("Controls", file));
                        if (GetValue("Players", file) != "")
                            lines.Add("Players=" + GetValue("Players", file));
                        if (GetValue("Esrb", file) != "")
                            lines.Add("Esrb=" + GetValue("Esrb", file));
                        if (GetValue("Url", file) != "")
                            lines.Add("Url=" + GetValue("Url", file));
                        if (GetValue("Description", file) != "")
                            lines.Add("Description=" + GetValue("Description", file).Replace("\n", "<br>"));
                        if (GetValue("Goodname", file) != "")
                            lines.Add("Goodname=" + GetValue("Goodname", file));
                        if (GetValue("NoIntro", file) != "")
                            lines.Add("NoIntro=" + GetValue("NoIntro", file));
                        if (GetValue("Tosec", file) != "")
                            lines.Add("Tosec=" + GetValue("Tosec", file));
                    }
                    lines.Add("");
                }
                else//add all of rom
                {
                    foreach (DBProperty pro in file.Properties)
                    {
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
                Progress(this, new ProgressArg((i * 100) / files.Count, "Saving database file..."));
                i++;
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
            if (Progress != null)
                Progress(this, new ProgressArg(100, "Done"));
            return true;
        }
        private string GetValue(string id, DatabaseRom file)
        {
            foreach (DBProperty pro in file.Properties)
            {
                if (pro.Property.ToLower() == id.ToLower())
                    return pro.Value;
            }
            return "";
        }
        public string Extension
        {
            get { return "ini"; }
        }

        public event EventHandler<ProgressArg> Progress;

        public bool IsSupportCRC
        {
            get { return false; }
        }
        public bool IsSupportSHA1
        {
            get { return false; }
        }
        public bool IsSupportMD5
        {
            get { return false; }
        }
        public bool IsSupportFileNameCompare
        {
            get { return true; }
        }
        public bool IsSupportSave
        {
            get { return true; }
        }
        public string CalculateSHA1(string filePath)
        {
            return "";
        }
        public string CalculateMD5(string filePath)
        {
            return "";
        }
        public string CalculateCRC(string filePath)
        {
            return "";
        }
        public string CalculateCRCWithoutSkip(string filePath)
        {
            return "";
        }

        public bool IsSupportSeparate
        {
            get { return true; }
        }

        public SeparateItem[] GetSeparate(string filePath)
        {
            List<SeparateItem> items = new List<SeparateItem>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);

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
                if (Progress != null)
                    Progress(this, new ProgressArg((i * 100) / lines.Length, "Reading ini file ..."));
            }
            return items.ToArray();
        }
    }
}
