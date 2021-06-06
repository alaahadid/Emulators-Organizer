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
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("Mame XML", new string[] { ".xml" }, CompareMode.RomFileName, false, false, true)]
    [DatabaseVisibilty(false)]
    public class DatabaseFile_MameXML : DatabaseFile
    {
        public bool _rename_using_description_instead_of_name = true;
        public bool _add_rom_data_items = true;
        public override List<DBEntry> LoadFile(string filePath)
        {
            List<DBEntry> entries = new List<DBEntry>();

            OnProgressStarted("Reading Mame Dat file ...");

            Stream databaseStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            XmlReaderSettings sett = new XmlReaderSettings();
            sett.DtdProcessing = DtdProcessing.Ignore;
            sett.IgnoreWhitespace = true;
            XmlReader XMLread = XmlReader.Create(databaseStream, sett);

            int i = 0;
            string name = "";
            DBEntry crt = DBEntry.Empty;
            crt.FileNames = new List<string>();

            while (XMLread.Read())
            {
                //Is it a game ?
                if (XMLread.Name == "machine" & XMLread.IsStartElement())
                {
                    crt = DBEntry.Empty;

                    if (XMLread.MoveToAttribute("name"))
                    {
                        //crt.AlternativeName = name = XMLread.Value; 
                        name = XMLread.Value;
                        crt.FileNames.Add(name);
                        crt.Properties.Add(new PropertyStruct("Name", name));
                    }
                    if (XMLread.MoveToAttribute("cloneof"))
                    {
                        // crt.Additions.Add("true");
                        //crt.Additions.Add(XMLread.Value);
                        crt.Properties.Add(new PropertyStruct("Clone Of", XMLread.Value));
                    }
                    else
                    {
                        crt.Properties.Add(new PropertyStruct("Clone Of", ""));
                    }
                    if (XMLread.MoveToAttribute("romof"))
                    {
                        crt.Properties.Add(new PropertyStruct("Rom Of", XMLread.Value));
                    }
                    else
                    {
                        crt.Properties.Add(new PropertyStruct("Rom Of", ""));
                    }
                    XMLread.Read();//advance to <description>
                }
                if (XMLread.Name == "description" & XMLread.IsStartElement())
                {
                    string descr = XMLread.ReadString();
                    crt.Properties.Add(new PropertyStruct("Description", descr));
                    crt.FileNames.Add(descr);
                }
                if (XMLread.Name == "year" & XMLread.IsStartElement())
                {
                    crt.Properties.Add(new PropertyStruct("Year", XMLread.ReadString()));
                }
                if (XMLread.Name == "manufacturer" & XMLread.IsStartElement())
                {
                    crt.Properties.Add(new PropertyStruct("Manufacturer", XMLread.ReadString()));
                }
                if (XMLread.Name == "rom" & XMLread.IsStartElement())
                {
                    //if (XMLread.MoveToAttribute("crc"))
                    //     crt.CRCs.Add(XMLread.Value);
                    string r_name = "";
                    string r_merge = "";
                    string r_crc = "";
                    string r_sha1 = "";

                    if (XMLread.MoveToAttribute("name"))
                        r_name = XMLread.Value;
                    if (XMLread.MoveToAttribute("merge"))
                        r_merge = XMLread.Value;
                    if (XMLread.MoveToAttribute("crc"))
                        r_crc = XMLread.Value;
                    if (XMLread.MoveToAttribute("sha1"))
                        r_sha1 = XMLread.Value;

                    crt.PerfectMatchCRCS.Add(new PerfectMatchFile(r_name, r_merge, r_crc, r_sha1));
                }
                if (XMLread.Name == "machine" & !XMLread.IsStartElement())
                {
                    entries.Add(crt);
                }
                i++;
                if (i < databaseStream.Length)
                {
                    int x = (i * 100) / (int)databaseStream.Length;
                    OnProgress(x, "Reading database file... [" + x + " %]");
                }
            }
            XMLread.Close();
            databaseStream.Close();
            OnProgressFinished("Mame Dat Database file read successful.");

            return entries;
        }
        public override System.Windows.Forms.Control OptionsControl
        {
            get
            {
                return new DatabaseFileControl_MameDat(this);
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

                    Trace.WriteLine("->Rom renamed to: " + rom.Name, "MAME Dat database");
                }
                else
                {
                    rom.Name = entry.GetPropertyValue("Name");

                    Trace.WriteLine("->Rom renamed to: " + rom.Name, "MAME Dat database");
                }
            }
            // Add description ?
            if (_add_rom_data_items && applyDataInfo)
            {
                // add description
                if (!_rename_using_description_instead_of_name)
                {
                    AddNewIC("Description", rom.ParentConsoleID);
                    AddDataToRom("Description", entry.GetPropertyValue("Description"), rom);
                }

                AddNewIC("Year", rom.ParentConsoleID);
                AddDataToRom("Year", entry.GetPropertyValue("Year"), rom);
                AddNewIC("Manufacturer", rom.ParentConsoleID);
                AddDataToRom("Manufacturer", entry.GetPropertyValue("Manufacturer"), rom);
            }
            Trace.WriteLine("->Rom data updated.", "MAME Dat database");
        }
    }
}
