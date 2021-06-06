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
using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("No Intro", new string[] { ".dat" },
     CompareMode.CRC | CompareMode.MD5 | CompareMode.SHA1, true, false, false)]
    public class DatabaseFile_NoIntro : DatabaseFile
    {
        public string dbHeaderRuleValue = "";
        public string dbName = "DataBase";
        public string dbDescription = "Created By Emulators Organizer 6";
        public string dbVersion = "";
        public string dbHeader = "";
        public string dbComment = "no-intro | www.no-intro.org";
        public bool IgnoreSkipForCompressed;

        public bool opRenameRomUsingName = true;
        public bool opAddDescriptionAsDataItemToRom = true;
        private void AddFilter(string name, string search_key, string search_data_item)
        {
            base.AddFilters = true;
            foreach (Filter f in Filters)
            {
                if (f.Name == name)
                    return;// Already exists
            }

            Filter ff = new Filter();
            ff.Name = name;
            ff.Parameters = new SearchRequestArgs(search_key, search_data_item, TextSearchCondition.Contains, NumberSearchCondition.Equal, false);

            Filters.Add(ff);
        }
        public override List<DBEntry> LoadFile(string filePath)
        {
            List<DBEntry> entries = new List<DBEntry>();
            string[] lines = File.ReadAllLines(filePath);


            OnProgressStarted("Reading database file ...");

            if (base.IsNesConsole)
                base.BytesToSkip = 16;
            if (Filters == null)
                Filters = new List<Filter>();
            // If the file is new xml version of no intro dat file
            if (lines[0].StartsWith("<?xml"))
            {
                Trace.WriteLine("!! This is the new XML version of the database, using XML reader ...", "No-Intro database");
                Stream databaseStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.DtdProcessing = DtdProcessing.Ignore;
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(databaseStream, sett);

                while (XMLread.Read())
                {
                    if (XMLread.IsStartElement())
                    {
                        if (XMLread.Name == "header")
                        {
                            while (XMLread.Read())
                            {
                                if (XMLread.Name == "clrmamepro")
                                {
                                    XMLread.MoveToAttribute("name");
                                    string header_file_name = XMLread.Value;
                                    ReadHeaderFile(Path.GetDirectoryName(filePath) + "\\" + header_file_name);
                                    break;
                                }
                                else if (!XMLread.IsStartElement())
                                {
                                    break;
                                }
                            }
                        }
                        else if (XMLread.Name == "game")
                        {
                            DBEntry rom = DBEntry.Empty;

                            XMLread.MoveToAttribute("name");
                            rom.Properties.Add(new PropertyStruct("Name", XMLread.Value));

                            while (XMLread.Read())
                            {
                                if (XMLread.Name == "description" && XMLread.IsStartElement())
                                {
                                    rom.Properties.Add(new PropertyStruct("Description", XMLread.ReadString()));
                                }
                                if (XMLread.Name == "rom" && XMLread.IsStartElement())
                                {
                                    XMLread.MoveToAttribute("name");
                                    rom.FileNames.Add(XMLread.Value);

                                    XMLread.MoveToAttribute("crc");
                                    rom.CRC = XMLread.Value;
                                    XMLread.MoveToAttribute("md5");
                                    rom.MD5 = XMLread.Value;
                                    XMLread.MoveToAttribute("sha1");
                                    rom.SHA1 = XMLread.Value;
                                }
                                if (XMLread.Name == "game" && !XMLread.IsStartElement())
                                {
                                    entries.Add(rom);
                                    break;
                                }
                                if (XMLread.Name == "release" & XMLread.IsStartElement())
                                {
                                    if (!rom.IsPRopertyExist("Release Name"))
                                    {
                                        if (XMLread.MoveToAttribute("name"))
                                            rom.Properties.Add(new PropertyStruct("Release Name", XMLread.Value));
                                    }
                                    else
                                    {
                                        if (XMLread.MoveToAttribute("name"))
                                        {
                                            string val = rom.GetPropertyValue("Release Name");
                                            if (XMLread.MoveToAttribute("name"))
                                            {
                                                val += ", " + XMLread.Value;
                                                rom.UpdateProperty("Release Name", val);
                                            }
                                        }
                                    }
                                    if (!rom.IsPRopertyExist("Region"))
                                    {
                                        if (XMLread.MoveToAttribute("region"))
                                        {
                                            rom.Properties.Add(new PropertyStruct("Region", XMLread.Value));
                                            AddFilter(XMLread.Value, XMLread.Value, "Region");
                                        }
                                    }
                                    else
                                    {
                                        if (XMLread.MoveToAttribute("region"))
                                        {
                                            string val = rom.GetPropertyValue("Region");
                                            if (XMLread.MoveToAttribute("region"))
                                            {
                                                AddFilter(XMLread.Value, XMLread.Value, "Region");
                                                val += ", " + XMLread.Value;
                                                rom.UpdateProperty("Region", val);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        if (XMLread.Name == "datafile")
                        {
                            // This is the end element
                            XMLread.Close();
                            databaseStream.Close();
                            break;
                        }
                    }
                }
            }
            else
            {
                // OLD VERSIONS
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length >= 4)
                    {
                        // If header is presented, it will override bytes to skip option.
                        if (lines[i].Length >= "clrmamepro".Length)
                        {
                            if (lines[i].Substring(0, "clrmamepro".Length) == "clrmamepro")
                            {
                                i += 3;
                                if (lines[i].Contains("header"))
                                {
                                    string[] texts = lines[i].Split(new char[] { '"' });
                                    string headerFileName = texts[1];
                                    if (File.Exists(Path.GetDirectoryName(filePath) + "\\" + headerFileName))
                                    {
                                        Stream databaseStream = new FileStream(Path.GetDirectoryName(filePath) + "\\" + headerFileName, FileMode.Open, FileAccess.Read);
                                        XmlReaderSettings sett = new XmlReaderSettings();
                                        sett.DtdProcessing = DtdProcessing.Ignore;
                                        sett.IgnoreWhitespace = true;
                                        XmlReader XMLread = XmlReader.Create(databaseStream, sett);
                                        while (XMLread.Read())
                                        {
                                            if (XMLread.Name == "name" & XMLread.IsStartElement())
                                            {
                                                if (XMLread.ReadString().Contains("iNES"))
                                                    base.BytesToSkip = 16;
                                            }
                                            if (XMLread.Name == "data" & XMLread.IsStartElement())
                                            {
                                                if (XMLread.MoveToAttribute("value"))
                                                    dbHeaderRuleValue = XMLread.Value;
                                            }
                                        }
                                        XMLread.Close();
                                        databaseStream.Close();
                                    }
                                }
                            }
                        }
                        if (lines[i].Substring(0, 4) == "game")
                        {
                            try
                            {
                                DBEntry rom = DBEntry.Empty;
                                i++;
                                string[] texts = lines[i].Split(new char[] { '"' });
                                //rom.Name = texts[1];
                                // Add name property
                                rom.Properties.Add(new PropertyStruct("Name", texts[1]));
                                i++;
                                texts = lines[i].Split(new char[] { '"' });
                                //rom.Description = texts[1];
                                // Add description property
                                rom.Properties.Add(new PropertyStruct("Description", texts[1]));
                                i++;
                                texts = lines[i].Split(new char[] { '"' });
                                texts = texts[2].Split(new char[] { ' ' });
                                rom.CRC = texts[4];
                                rom.MD5 = texts[6];
                                rom.SHA1 = texts[8];
                                entries.Add(rom);
                            }
                            catch { }
                        }
                    }
                    OnProgress((i * 100) / lines.Length, "Reading database file...");
                }
            }

            OnProgressFinished("Database file read successful.");

            return entries;
        }
        private void ReadHeaderFile(string file_name)
        {
            if (File.Exists(file_name))
            {
                Stream databaseStream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.DtdProcessing = DtdProcessing.Ignore;
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(databaseStream, sett);
                while (XMLread.Read())
                {
                    if (XMLread.Name == "name" & XMLread.IsStartElement())
                    {
                        if (XMLread.ReadString().Contains("iNES"))
                            base.BytesToSkip = 16;
                    }
                    if (XMLread.Name == "data" & XMLread.IsStartElement())
                    {
                        if (XMLread.MoveToAttribute("value"))
                            dbHeaderRuleValue = XMLread.Value;
                    }
                }
                XMLread.Close();
                databaseStream.Close();
            }
        }

        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });


            OnProgressStarted("Saving No-Intro database file ...");
            dbVersion = (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") +
                DateTime.Now.Day.ToString("D2")).ToString();

            List<string> lines = new List<string>();
            //Write header
            lines.Add("clrmamepro (");
            lines.Add("\t" + @"name """ + dbName + @"""");
            lines.Add("\t" + @"description """ + dbDescription + @"""");
            if (dbHeader != "")
                lines.Add("\t" + @"header """ + dbHeader + @"""");
            lines.Add("\t" + @"version """ + dbVersion + @"""");
            lines.Add("\t" + @"comment """ + dbComment + @"""");
            lines.Add(")");
            lines.Add("");
            int i = 0;
            foreach (DBEntry rom in entries)
            {
                lines.Add("game (");
                lines.Add("\t" + @"name """ + rom.GetPropertyValue("Name") + @"""");
                lines.Add("\t" + @"description """ + rom.GetPropertyValue("Description") + @"""");
                string filePath = rom.TemporaryPathForExport;
                bool skipBytes = true;
                if (HelperTools.IsAIPath(rom.TemporaryPathForExport))
                {
                    SevenZipExtractor extractor =
                        new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.TemporaryPathForExport)));
                    int index = HelperTools.GetIndexFromAIPath(rom.TemporaryPathForExport);
                    // Try to extract and get data
                    try
                    {
                        FileStream mstream = new FileStream(Path.GetTempPath() + "\\test.tst", FileMode.Create, FileAccess.Write);
                        extractor.ExtractFile(index, mstream);
                        mstream.Close();
                        mstream.Dispose();
                        filePath = Path.GetTempPath() + "\\test.tst";
                    }
                    catch { }
                }
                else if (IgnoreSkipForCompressed)
                {
                    if (archiveExtensions.Contains(Path.GetExtension(filePath).ToLower()))
                        skipBytes = false;
                }
                string line = "\t" + "rom ( name " + @"""" + rom.GetPropertyValue("Name") + @""""
                    + " size " + HelperTools.GetSizeAsBytes(filePath)
                    + " crc " + CalculateCRC(filePath, skipBytes).ToUpper()
                    + " md5 " + CalculateMD5(filePath, skipBytes).ToUpper()
                    + " sha1 " + CalculateSHA1(filePath, skipBytes).ToUpper() + " )";
                lines.Add(line);
                lines.Add(")");
                lines.Add("");
                int p = (i * 100) / entries.Count;
                OnProgress(p, "Saving No-Intro database file [" + p + " %]");
                i++;
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
            OnProgressFinished("No-Intro database saved successfully.");

        }
        public override System.Windows.Forms.Control OptionsControl
        {
            get
            {
                return new DatabaseFileControl_NoIntro(this);
            }
        }
        public override void ShowExportOptions()
        {
            Frm_NoIntroDat frm = new Frm_NoIntroDat(this);
            frm.ShowDialog();
        }
        public override void ApplyName(Rom rom, DBEntry entry, bool applyName, bool applyDataInfo)
        {
            // Renaming
            if (opRenameRomUsingName && applyName)
            {
                rom.Name = entry.GetPropertyValue("Name");

                Trace.WriteLine("->Rom renamed to: " + rom.Name, "No-Intro database");
            }
            // Add description ?
            if (applyDataInfo)
            {
                if (opAddDescriptionAsDataItemToRom)
                {
                    AddNewIC("Description", rom.ParentConsoleID);
                    AddDataToRom("Description", entry.GetPropertyValue("Description"), rom);
                }
                // Region and Release Name
                foreach (PropertyStruct p in entry.Properties)
                {
                    if (p.Property == "Region" || p.Property == "Release Name")
                    {
                        AddNewIC(p.Property, rom.ParentConsoleID);
                        AddDataToRom(p.Property, p.Value, rom);
                    }
                }
            }
            Trace.WriteLine("->Rom data updated.", "No-Intro database");
        }
    }
}
