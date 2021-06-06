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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("HyperList XML", new string[] { ".xml" }, CompareMode.RomName | CompareMode.CRC, true, false, false)]
    public class DatabaseFile_HyperListXML : DatabaseFile
    {
        public string _db_header_name;
        public string _db_header_version;
        public string _db_header_date;
        public string _db_header_author;
        public string _db_header_header_file_name;
        public string _db_header_plugin;
        public bool IgnoreSkipForCompressed;
        // Import
        public bool SkipBytesInImport;
        public int BytesToSkipInImport;
        public bool IncludeCategoriesOnExport;
        public bool _rename_using_description_instead_of_name;

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
            // try
            {
                //1 clear the database
                List<DBEntry> files = new List<DBEntry>();
                Filters = new List<Filter>();
                AddFilters = false;
                //2 read the xml file
                Stream databaseStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.DtdProcessing = DtdProcessing.Ignore;
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(databaseStream, sett);

                OnProgressStarted("Reading HyperList XML file...");

                if (SkipBytesInImport)
                    base.BytesToSkip = BytesToSkipInImport;
                else
                    base.BytesToSkip = 0;

                int i = 0;
                DBEntry crt = DBEntry.Empty;
                while (XMLread.Read())
                {
                    //Is it a game ?
                    if (XMLread.Name == "game" & XMLread.IsStartElement())
                    {
                        crt = DBEntry.Empty;

                        if (XMLread.MoveToAttribute("name"))
                        {   //crt.Name = crt.AlternativeName = XMLread.Value; 
                            string nn = XMLread.Value;
                            crt.FileNames.Add(nn);
                            crt.Properties.Add(new PropertyStruct("Name", nn));
                        }
                    }
                    else if (XMLread.Name == "rom" & XMLread.IsStartElement())
                    {
                        if (XMLread.MoveToAttribute("crc"))
                            //crt.Name = crt.AlternativeName = XMLread.Value;
                            crt.CRC = XMLread.Value;
                        if (XMLread.MoveToAttribute("sha1"))
                            //crt.Name = crt.AlternativeName = XMLread.Value;
                            crt.SHA1 = XMLread.Value;
                        if (XMLread.MoveToAttribute("md5"))
                            //crt.Name = crt.AlternativeName = XMLread.Value;
                            crt.MD5 = XMLread.Value;
                    }
                    else if (XMLread.Name == "game" & !XMLread.IsStartElement())
                    {
                        if (!crt.IsPRopertyExist("Clone Of"))
                            crt.Properties.Add(new PropertyStruct("Clone Of", ""));

                        files.Add(crt);
                    }
                    else if (XMLread.Name == "description" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Description", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "cloneof" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Clone Of", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "crc" & XMLread.IsStartElement())
                    {
                        crt.CRC = XMLread.ReadString();
                    }
                    else if (XMLread.Name == "year" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Year", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "manufacturer" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Manufacturer", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "ctrltype" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Control Type", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "buttons" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Buttons", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "joyways" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Joyways", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "genre" & XMLread.IsStartElement())
                    {
                        // TODO: apply categories into all database formats
                        string cat = XMLread.ReadString();
                        crt.Properties.Add(new PropertyStruct("Genre", cat));
                        crt.Categories.Add("Genre/" + cat);
                    }
                    else if (XMLread.Name == "rating" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Rating", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "enabled" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Enabled", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "players" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Players", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "developer" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Developer", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "score" & XMLread.IsStartElement())
                    {
                        crt.Properties.Add(new PropertyStruct("Score", XMLread.ReadString()));
                    }
                    else if (XMLread.Name == "release" & XMLread.IsStartElement())
                    {
                        if (!crt.IsPRopertyExist("Release Name"))
                        {
                            if (XMLread.MoveToAttribute("name"))
                                crt.Properties.Add(new PropertyStruct("Release Name", XMLread.Value));
                        }
                        else
                        {
                            if (XMLread.MoveToAttribute("name"))
                            {
                                string val = crt.GetPropertyValue("Release Name");
                                if (XMLread.MoveToAttribute("name"))
                                {
                                    val += ", " + XMLread.Value;
                                    crt.UpdateProperty("Release Name", val);
                                }
                            }
                        }
                        if (!crt.IsPRopertyExist("Region"))
                        {
                            if (XMLread.MoveToAttribute("region"))
                            {
                                crt.Properties.Add(new PropertyStruct("Region", XMLread.Value));
                                AddFilter(XMLread.Value, XMLread.Value, "Region");
                            }
                        }
                        else
                        {
                            if (XMLread.MoveToAttribute("region"))
                            {
                                string val = crt.GetPropertyValue("Region");
                                if (XMLread.MoveToAttribute("region"))
                                {
                                    AddFilter(XMLread.Value, XMLread.Value, "Region");
                                    val += ", " + XMLread.Value;
                                    crt.UpdateProperty("Region", val);
                                }
                            }
                        }
                    }
                    i++;
                    if (i < databaseStream.Length)
                    {
                        int x = (i * 100) / (int)databaseStream.Length;
                        OnProgress(x, "Reading HyperList XML file... [" + x + " %]");
                    }
                }
                XMLread.Close();
                databaseStream.Close();
                OnProgressFinished("HyperList XML read done successfully.");
                return files;
            }
            //catch { }
        }
        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });

            OnProgressStarted("Saving Hyperlist XML database file ...");

            Stream databaseStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            XmlWriter XMLwrite = XmlWriter.Create(databaseStream, sett);

            XMLwrite.WriteStartElement("menu");
            /*HEADER*/
            XMLwrite.WriteStartElement("header");
            // NAME
            XMLwrite.WriteStartElement("name");
            XMLwrite.WriteString(_db_header_name);
            XMLwrite.WriteEndElement();// name
            // Version
            XMLwrite.WriteStartElement("version");
            XMLwrite.WriteString(_db_header_version);
            XMLwrite.WriteEndElement();// version
            // Date
            XMLwrite.WriteStartElement("date");
            XMLwrite.WriteString(_db_header_date);
            XMLwrite.WriteEndElement();// date
            // Author
            XMLwrite.WriteStartElement("author");
            XMLwrite.WriteString(_db_header_author);
            XMLwrite.WriteEndElement();// author
            // Author
            XMLwrite.WriteStartElement("clrmamepro");
            XMLwrite.WriteAttributeString("header", _db_header_header_file_name);
            XMLwrite.WriteEndElement();// clrmamepro
            // romcenter
            XMLwrite.WriteStartElement("romcenter");
            XMLwrite.WriteAttributeString("plugin", _db_header_plugin);
            XMLwrite.WriteEndElement();// romcenter
            XMLwrite.WriteEndElement();// HEADER

            // GAMES !!
            int i = 0;
            foreach (DBEntry entry in entries)
            {
                string filePath = entry.TemporaryPathForExport;
                string rom_path = entry.TemporaryPathForExport;
                bool skipBytes = true;
                if (HelperTools.IsAIPath(entry.TemporaryPathForExport))
                {
                    rom_path = HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(entry.TemporaryPathForExport));
                    SevenZipExtractor extractor =
                        new SevenZipExtractor(rom_path);
                    int index = HelperTools.GetIndexFromAIPath(entry.TemporaryPathForExport);
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
                // Game info
                XMLwrite.WriteStartElement("game");
                XMLwrite.WriteAttributeString("name", entry.GetPropertyValue("Name"));
                XMLwrite.WriteAttributeString("index", "");
                XMLwrite.WriteAttributeString("image", "");

                // Description
                XMLwrite.WriteStartElement("description");
                XMLwrite.WriteString(entry.GetPropertyValue("Description"));
                XMLwrite.WriteEndElement();// description
                // CLone of
                XMLwrite.WriteStartElement("cloneof");
                // ??
                XMLwrite.WriteEndElement();// cloneof
                // CRC
                XMLwrite.WriteStartElement("crc");
                XMLwrite.WriteString(CalculateCRC(filePath, skipBytes).ToUpper());
                XMLwrite.WriteEndElement();// crc
                // Manufacturer
                XMLwrite.WriteStartElement("manufacturer");
                XMLwrite.WriteString(entry.GetPropertyValue("Manufacturer"));
                XMLwrite.WriteEndElement();// manufacturer
                // Developer
                XMLwrite.WriteStartElement("developer");
                XMLwrite.WriteString(entry.GetPropertyValue("Developer"));
                XMLwrite.WriteEndElement();// developer
                // Year
                XMLwrite.WriteStartElement("year");
                XMLwrite.WriteString(entry.GetPropertyValue("Year"));
                XMLwrite.WriteEndElement();// year
                // Genre
                XMLwrite.WriteStartElement("genre");
                if (IncludeCategoriesOnExport)
                {
                    if (entry.Categories != null)
                        if (entry.Categories.Count > 0)
                        {
                            string cats = "";
                            foreach (string cat in entry.Categories)
                            {
                                cats += cat + ", ";
                            }
                            XMLwrite.WriteString(cats);
                        }
                }
                else
                    XMLwrite.WriteString(entry.GetPropertyValue("Genre"));
                XMLwrite.WriteEndElement();// genre
                // Rating
                XMLwrite.WriteStartElement("rating");
                XMLwrite.WriteString(entry.GetPropertyValue("Rating"));
                XMLwrite.WriteEndElement();// rating
                // Players
                XMLwrite.WriteStartElement("players");
                XMLwrite.WriteString(entry.GetPropertyValue("Players"));
                XMLwrite.WriteEndElement();// players
                // Score
                XMLwrite.WriteStartElement("score");
                XMLwrite.WriteString(entry.GetPropertyValue("Score"));
                XMLwrite.WriteEndElement();// score

                // Rom
                XMLwrite.WriteStartElement("rom");
                XMLwrite.WriteAttributeString("name", Path.GetFileName(rom_path));
                XMLwrite.WriteAttributeString("size", HelperTools.GetSizeAsBytes(rom_path).ToString());
                XMLwrite.WriteAttributeString("crc", CalculateCRC(filePath, skipBytes).ToUpper());
                XMLwrite.WriteAttributeString("md5", CalculateMD5(filePath, skipBytes).ToUpper());
                XMLwrite.WriteAttributeString("sha1", CalculateSHA1(filePath, skipBytes).ToUpper());
                XMLwrite.WriteEndElement();// rom
                // WARNING !! RELEASE INFO IS NOT INCLUDED
                XMLwrite.WriteEndElement();// game

                // Progress
                int p = (i * 100) / entries.Count;
                OnProgress(p, "Saving Hyperlist XML database file [" + p + " %]");
                i++;
            }

            XMLwrite.WriteEndElement();// MENU
            // Flush
            XMLwrite.Flush();
            XMLwrite.Close();

            OnProgressFinished("Hyperlist XML database saved successfully.");
        }
        public override void ShowExportOptions()
        {
            Frm_HyperlistExportOptions frm = new Frm_HyperlistExportOptions(this);
            frm.ShowDialog();
        }
        public override System.Windows.Forms.Control OptionsControl
        {
            get
            {
                return new DatabaseFileControl_Hyperlist(this);
            }
        }
        public override void ApplyName(Rom rom, DBEntry entry, bool applyName, bool applyDataInfo)
        {
            // Renaming
            if (applyName)
            {
                if (_rename_using_description_instead_of_name)
                {
                    rom.Name = entry.GetPropertyValue("Description");

                    Trace.WriteLine("->Rom renamed to: " + rom.Name, "HyperList XML database");
                }
                else
                {
                    rom.Name = entry.GetPropertyValue("Name");

                    Trace.WriteLine("->Rom renamed to: " + rom.Name, "HyperList XML database");
                }
            }

            // Database info items
            if (applyDataInfo)
            {
                foreach (PropertyStruct p in entry.Properties)
                {
                    if (!_rename_using_description_instead_of_name)
                    {
                        if (p.Property != "Name")
                        {
                            AddNewIC(p.Property, rom.ParentConsoleID);
                            AddDataToRom(p.Property, p.Value, rom);
                        }
                    }
                    else
                    {
                        if (p.Property != "Description")
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

