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
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("Emulation Station", new string[] { ".xml" }, CompareMode.RomFileName | CompareMode.RomName, true, false, false)]
    public class DatabaseFile_EmulationStationXML : DatabaseFile
    {
        // Import
        public bool SkipBytesInImport;
        public int BytesToSkipInImport;
        public int ReleaseDateImportOption = 0;

        public string _db_header_system = "";
        public string _db_header_software = "Emulators Organizer";
        public string _db_header_database = "Emulators Organizer";
        public string _db_header_web = "https://sourceforge.net/projects/emusorganizer/";
        public bool _db_export_use_year_as_release_date = false;
        public bool _db_export_release_date_format = true;
        public bool IncludeCategoriesOnExport = false;

        public override List<DBEntry> LoadFile(string filePath)
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

            OnProgressStarted("Reading Emulation Station XML file...");

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

                    /*if (XMLread.MoveToAttribute("name"))
                    {   //crt.Name = crt.AlternativeName = XMLread.Value; 
                        string nn = XMLread.Value;
                        crt.FileNames.Add(nn);
                        crt.Properties.Add(new PropertyStruct("Name", nn));
                    }*/

                    if (XMLread.MoveToAttribute("id"))
                    {
                        string nn = XMLread.Value;
                        crt.Properties.Add(new PropertyStruct("ID", nn));
                    }
                    if (XMLread.MoveToAttribute("source"))
                    {
                        string nn = XMLread.Value;
                        crt.Properties.Add(new PropertyStruct("Source", nn));
                    }
                }
                else if (XMLread.Name == "path" & XMLread.IsStartElement())
                {
                    // Do we need to add the path as property ?
                    //crt.Properties.Add(new PropertyStruct("Description", XMLread.ReadString()));
                    string ff = XMLread.ReadString();
                    crt.FileNames.Add(Path.GetFileNameWithoutExtension(ff));
                }
                else if (XMLread.Name == "name" & XMLread.IsStartElement())
                {
                    string nn = XMLread.ReadString();
                    crt.Properties.Add(new PropertyStruct("Name", nn));
                    // now we can add the entry
                    files.Add(crt);
                    // Just in case
                    if (crt.FileNames.Count == 0)
                    {
                        crt.FileNames.Add(nn);
                    }
                }
                else if (XMLread.Name == "desc" & XMLread.IsStartElement())
                {
                    crt.Properties.Add(new PropertyStruct("Description", XMLread.ReadString()));
                }
                else if (XMLread.Name == "releasedate" & XMLread.IsStartElement())
                {
                    // crt.CRC = XMLread.ReadString();
                    string date = XMLread.ReadString();
                    if (date.Length > 0)
                    {
                        string year = date.Substring(0, 4);
                        string month = date.Substring(4, 2);
                        string day = date.Substring(6, 2);
                        string time = date.Substring(9, date.Length - 9);
                        switch (ReleaseDateImportOption)
                        {
                            case 0:// Use full release date format
                                {
                                    string ff = string.Format("{0}.{1}.{2} {3}", day, month, year, time);

                                    crt.Properties.Add(new PropertyStruct("Release Date", ff));
                                    crt.AddCategory(ff, "Release Date");

                                    break;
                                }
                            case 1:// Use date-only release date format
                                {
                                    string ff = string.Format("{0}.{1}.{2}", day, month, year);

                                    crt.Properties.Add(new PropertyStruct("Release Date", ff));
                                    crt.AddCategory(ff, "Release Date");

                                    break;
                                }
                            case 2:// Use year release date format
                                {
                                    crt.Properties.Add(new PropertyStruct("Release Date", year));
                                    crt.AddCategory(year, "Release Date");

                                    break;
                                }
                            case 3:// Use year release date format and name the tab year
                                {
                                    crt.Properties.Add(new PropertyStruct("Year", year));
                                    crt.AddCategory(year, "Year");

                                    break;
                                }
                        }
                    }

                }
                else if (XMLread.Name == "publisher" & XMLread.IsStartElement())
                {
                    string y = XMLread.ReadString();
                    crt.Properties.Add(new PropertyStruct("Publisher", y));
                    crt.AddCategory(y, "Publisher");
                }
                else if (XMLread.Name == "genre" & XMLread.IsStartElement())
                {
                    string cat = XMLread.ReadString();
                    crt.Properties.Add(new PropertyStruct("Genre", cat));
                    crt.AddCategory(cat, "Genre");
                }
                else if (XMLread.Name == "rating" & XMLread.IsStartElement())
                {
                    string y = XMLread.ReadString();
                    crt.Properties.Add(new PropertyStruct("Rating", y));
                    crt.AddCategory(y, "Rating");
                }
                else if (XMLread.Name == "players" & XMLread.IsStartElement())
                {
                    string y = XMLread.ReadString();
                    crt.Properties.Add(new PropertyStruct("Players", y));
                    crt.AddCategory(y, "Players");
                }
                else if (XMLread.Name == "developer" & XMLread.IsStartElement())
                {
                    string y = XMLread.ReadString();
                    crt.Properties.Add(new PropertyStruct("Developer", y));
                    crt.AddCategory(y, "Developer");
                }
                i++;
                if (i < databaseStream.Length)
                {
                    int x = (i * 100) / (int)databaseStream.Length;
                    OnProgress(x, "Reading Emulation Station XML file... [" + x + " %]");
                }
            }
            XMLread.Close();
            databaseStream.Close();
            OnProgressFinished("Emulation Station XML read done successfully.");
            return files;
        }
        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });

            OnProgressStarted("Saving Emulation Station XML database file ...");

            Stream databaseStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            XmlWriter XMLwrite = XmlWriter.Create(databaseStream, sett);

            XMLwrite.WriteStartElement("gamelist");
            /*HEADER (Provider)*/
            XMLwrite.WriteStartElement("provider");
            // NAME
            XMLwrite.WriteStartElement("System");
            XMLwrite.WriteString(_db_header_system);
            XMLwrite.WriteEndElement();// System
            // Version
            XMLwrite.WriteStartElement("software");
            XMLwrite.WriteString(_db_header_software);
            XMLwrite.WriteEndElement();// software
            // Date
            XMLwrite.WriteStartElement("database");
            XMLwrite.WriteString(_db_header_database);
            XMLwrite.WriteEndElement();// database
            // Author
            XMLwrite.WriteStartElement("web");
            XMLwrite.WriteString(_db_header_web);
            XMLwrite.WriteEndElement();// web

            XMLwrite.WriteEndElement();// HEADER (Provider)

            // GAMES !!
            int i = 0;
            foreach (DBEntry entry in entries)
            {
                // Game info
                XMLwrite.WriteStartElement("game");
                XMLwrite.WriteAttributeString("id", entry.GetPropertyValue("ID"));
                XMLwrite.WriteAttributeString("source", entry.GetPropertyValue("Source"));

                // path
                XMLwrite.WriteStartElement("path");
                //XMLwrite.WriteString(HelperTools.GetIndexFromAIPath(entry.));
                if (HelperTools.IsAIPath(entry.TemporaryPathForExport))
                {
                    XMLwrite.WriteString("./" + Path.GetFileName(HelperTools.GetPathFromAIPath(entry.TemporaryPathForExport)));
                }
                else
                {
                    XMLwrite.WriteString("./" + Path.GetFileName(entry.TemporaryPathForExport));
                }
                XMLwrite.WriteEndElement();// path

                // name
                XMLwrite.WriteStartElement("name");
                XMLwrite.WriteString(entry.GetPropertyValue("Name"));
                XMLwrite.WriteEndElement();// name

                // desc
                XMLwrite.WriteStartElement("desc");
                XMLwrite.WriteString(entry.GetPropertyValue("Description"));
                XMLwrite.WriteEndElement();// desc

                // Rating
                XMLwrite.WriteStartElement("rating");
                XMLwrite.WriteString(entry.GetPropertyValue("Rating"));
                XMLwrite.WriteEndElement();// rating

                // Year
                string yy = "";
                if (_db_export_use_year_as_release_date)
                {
                    yy = entry.GetPropertyValue("Year");
                }
                else
                {
                    yy = entry.GetPropertyValue("Release Date");
                    if (yy.Length == 0)// Try again
                        yy = entry.GetPropertyValue("Release date");
                }
                if (yy.Length > 0)
                {
                    XMLwrite.WriteStartElement("releasedate");
                    if (_db_export_release_date_format)
                    {
                        //XMLwrite.WriteString(yy.Replace(".", "").Replace(" ", "T"));
                        if (yy.Length == 17)
                        {
                            string day = yy.Substring(0, 2);
                            string month = yy.Substring(3, 2);
                            string year = yy.Substring(6, 4);
                            // 01.01.1991 000000
                            string time = yy.Substring(11, 6);

                            XMLwrite.WriteString(string.Format("{0}{1}{2}T{3}", year, month, day, time));
                        }
                        else if (yy.Length == 4)
                        {
                            // Normal year
                            XMLwrite.WriteString(yy);
                        }
                        else
                        {
                            XMLwrite.WriteString(yy.Replace(".", "").Replace(" ", "T"));
                        }
                    }
                    else
                    {
                        XMLwrite.WriteString(yy);
                    }
                    XMLwrite.WriteEndElement();// releasedate
                }

                // Developer
                XMLwrite.WriteStartElement("developer");
                XMLwrite.WriteString(entry.GetPropertyValue("Developer"));
                XMLwrite.WriteEndElement();// developer

                // publisher
                XMLwrite.WriteStartElement("publisher");
                XMLwrite.WriteString(entry.GetPropertyValue("Publisher"));
                XMLwrite.WriteEndElement();// developer

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

                // Players
                XMLwrite.WriteStartElement("players");
                XMLwrite.WriteString(entry.GetPropertyValue("Players"));
                XMLwrite.WriteEndElement();// players

                XMLwrite.WriteEndElement();// game

                // Progress
                int p = (i * 100) / entries.Count;
                OnProgress(p, "Saving Emulation Station XML database file [" + p + " %]");
                i++;
            }

            XMLwrite.WriteEndElement();// gamelist
            // Flush
            XMLwrite.Flush();
            XMLwrite.Close();

            OnProgressFinished("Emulation Station XML database saved successfully.");
        }

        public override Control OptionsControl
        {
            get { return new DatabaseFileControl_EmulationStation(this); }
        }
        public override void ShowExportOptions()
        {
            Frm_EmulationStationExportOptions frm = new Frm_EmulationStationExportOptions(this);
            frm.ShowDialog();
        }
    }
}
