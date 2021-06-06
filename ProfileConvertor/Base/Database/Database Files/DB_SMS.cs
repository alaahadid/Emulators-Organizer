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
    public class DB_SMS : IDatabaseFile
    {
        List<DatabaseRom> files = new List<DatabaseRom>();
        public int bytesToSkip = 0;
        public bool IgnoreSkipForCompressed = true;
        string[] archiveExs = new string[] { ".7z", ".rar", ".zip", ".xz", ".gzip", ".bzip2", ".tar", "wim" };
        public string Name
        {
            get { return "SMS (*.txt)"; }
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
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))//we got a game here
                {
                    DatabaseRom ROM = new DatabaseRom();
                    ROM.CRC = lines[i].Replace("[", "").Replace("]", "");
                    i++;
                    ROM.Name = lines[i];
                    ROM.FileNames = new List<string>();
                    ROM.FileNames.Add(ROM.Name);
                    ROM.Properties = new List<DBProperty>();
                    i++;
                    while (i < lines.Length)
                    {
                        if (lines[i].Contains(":"))//we have a property here
                        {
                            string[] code = lines[i].Split(new string[] { ": " }, StringSplitOptions.None);
                            string prop = code[0];
                            string val = "";
                            if (code.Length > 1)
                                val = code[1];
                            //else
                            ROM.Properties.Add(new DBProperty(prop, val));
                            i++;
                        }
                        else if (lines[i] == "_________________________")
                        {
                            i++;
                            while (i < lines.Length)
                            {
                                if (!lines[i].StartsWith("["))
                                {
                                    ROM.Description += lines[i] + "\n";
                                }
                                else
                                {
                                    ROM.Properties.Add(new DBProperty("Descreption", ROM.Description));
                                    break;
                                }
                                i++;
                            }
                            i--;
                            files.Add(ROM);
                            // Read description
                            break;
                        }
                    }
                }
                if (Progress != null)
                    Progress(this, new ProgressArg((i * 100) / lines.Length, "Reading txt file ..."));
            }
            return files.Count > 0;
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
        public bool SaveFile(string fileName)
        {
            if (Progress != null)
                Progress(this, new ProgressArg(0, "Saving database file..."));
            Frm_SMSExportOptions frm = new Frm_SMSExportOptions(this);
            frm.ShowDialog();
            List<string> lines = new List<string>();
            // add header
            lines.Add("[]");
            lines.Add("!missing");
            lines.Add("Platform: " + frm.Platform);
            lines.Add("Gametype: " + frm.Gametype);
            lines.Add("_________________________");
            lines.Add("");
            lines.Add("");
            int i = 0;
            foreach (DatabaseRom file in files)
            {
                if (!frm._CalculateCRCInsideOfArchive)
                    lines.Add("[" + CalculateCRC(file.Path).ToUpper() + "]");
                else
                {
                    if (archiveExs.Contains(Path.GetExtension(file.Path).ToLower()))
                    {
                        try
                        {
                            SevenZip.SevenZipExtractor extractor = new SevenZip.SevenZipExtractor(file.Path);
                            FileStream mstream = new FileStream(Path.GetTempPath() + "\\eo\\test.tst", FileMode.Create, FileAccess.Write);
                            extractor.ExtractFile(0, mstream);
                            lines.Add("[" + CalculateCRC(Path.GetTempPath() + "\\eo\\test.tst") + "]");
                        }
                        catch//error, just save crc of compressed file 
                        {
                            lines.Add("[" + CalculateCRC(file.Path).ToUpper() + "]");
                        }
                    }
                    else
                    {
                        lines.Add("[" + CalculateCRC(file.Path).ToUpper() + "]");
                    }
                }
                lines.Add(file.Name);
                if (!_AddAllDataItems)
                {
                    if (!_IgnoreEmptyFields)
                    {
                        lines.Add("Platform: " + GetValue("Platform", file));
                        lines.Add("Region: " + GetValue("Region", file));
                        lines.Add("Media: " + GetValue("Media", file));
                        lines.Add("Controller: " + GetValue("Controller", file));
                        lines.Add("Genre: " + GetValue("Genre", file));
                        lines.Add("Gametype: " + GetValue("Gametype", file));
                        lines.Add("Release Year: " + GetValue("Release Year", file));
                        lines.Add("Developer: " + GetValue("Developer", file));
                        lines.Add("Publisher: " + GetValue("Publisher", file));
                        lines.Add("Players: " + GetValue("Players", file));
                    }
                    else
                    {
                        if (GetValue("Platform", file) != "")
                            lines.Add("Platform: " + GetValue("Platform", file));
                        if (GetValue("Region", file) != "")
                            lines.Add("Region: " + GetValue("Region", file));
                        if (GetValue("Media", file) != "")
                            lines.Add("Media: " + GetValue("Media", file));
                        if (GetValue("Controller", file) != "")
                            lines.Add("Controller: " + GetValue("Controller", file));
                        if (GetValue("Genre", file) != "")
                            lines.Add("Genre: " + GetValue("Genre", file));
                        if (GetValue("Gametype", file) != "")
                            lines.Add("Gametype: " + GetValue("Gametype", file));
                        if (GetValue("Release Year", file) != "")
                            lines.Add("Release Year: " + GetValue("Release Year", file));
                        if (GetValue("Developer", file) != "")
                            lines.Add("Developer: " + GetValue("Developer", file));
                        if (GetValue("Publisher", file) != "")
                            lines.Add("Publisher: " + GetValue("Publisher", file));
                        if (GetValue("Players", file) != "")
                            lines.Add("Players: " + GetValue("Players", file));
                    }

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
                }
                lines.Add("_________________________");
                // Add rom description
                string[] descLines = file.Description.Split(new char[] { '\n' });
                lines.AddRange(descLines);
                lines.Add("");
                Progress(this, new ProgressArg((i * 100) / files.Count, "Saving database file..."));
                i++;
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
            if (Progress != null)
                Progress(this, new ProgressArg(100, "Done"));
            return true;
        }

        public string Extension
        {
            get { return "txt"; }
        }

        public event EventHandler<ProgressArg> Progress;

        public bool IsSupportCRC
        {
            get { return true; }
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
            get { return false; }
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
            return CalculateCRCWithoutSkip(filePath);
        }
        public string CalculateCRCWithoutSkip(string filePath)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] fileBuffer = new byte[fileStream.Length];
                fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
                fileStream.Close();
                string crc = "";
                Crc32 crc32 = new Crc32();
                byte[] crc32Buffer = crc32.ComputeHash(fileBuffer);

                foreach (byte b in crc32Buffer)
                    crc += b.ToString("x2").ToLower();

                return crc;
            }
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
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))//we got a game here
                {
                    SeparateItem item = new SeparateItem();
                    item.Data = lines[i] + "\n";
                    i++;
                    item.FileName = lines[i] + ".txt";
                    item.Data += lines[i] + "\n";
                    i++;
                    while (true)
                    {
                        item.Data += lines[i] + "\n";
                        if (lines[i].Contains(":"))//we have a property here
                        {
                            i++;
                        }
                        else if (lines[i] == "_________________________")
                        {
                            i++;
                            while (i < lines.Length)
                            {
                                if (!lines[i].StartsWith("["))
                                {
                                    item.Data += lines[i] + "\n";
                                }
                                else
                                {
                                    break;
                                }
                                i++;
                            }
                            i--;
                            items.Add(item);
                            break;
                        }
                    }
                }
                if (Progress != null)
                    Progress(this, new ProgressArg((i * 100) / lines.Length, "Reading txt file ..."));
            }
            return items.ToArray();
        }
    }
}
