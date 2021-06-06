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
using System.Xml;
using System.Diagnostics;
using System.Security.Cryptography;
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("Nes Cart", new string[] { ".xml" }, CompareMode.CRC | CompareMode.SHA1)]
    [ForceNesDatabase]
    public class DatabaseFile_NesCart : DatabaseFile
    {
        public override List<DBEntry> LoadFile(string filePath)
        {
            LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
            //1 clear the database
            List<DBEntry> files = new List<DBEntry>();
            //2 read the xml file
            Stream databaseStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            XmlReaderSettings sett = new XmlReaderSettings();
            sett.DtdProcessing = DtdProcessing.Ignore;
            sett.IgnoreWhitespace = true;
            XmlReader XMLread = XmlReader.Create(databaseStream, sett);
            OnProgressStarted(ls["Status_ReadingDatabaseFile"] + " ....");
            this.BytesToSkip = 16;
            int i = 0;
            DBEntry crt = DBEntry.Empty;
            while (XMLread.Read())
            {
                //Is it a game ?
                if (XMLread.Name == "game" & XMLread.IsStartElement())
                {
                    crt = DBEntry.Empty;
                    crt.Properties = new List<PropertyStruct>();
                    if (XMLread.MoveToAttribute("name"))
                        //     crt.Name = XMLread.Value;
                        crt.Properties.Add(new PropertyStruct("Name", XMLread.Value));
                    if (XMLread.MoveToAttribute("altname"))
                        //crt.AlternativeName = XMLread.Value.ToString();
                        crt.Properties.Add(new PropertyStruct("Alternative Name", XMLread.Value));
                    if (XMLread.MoveToAttribute("class"))
                        crt.Properties.Add(new PropertyStruct("Class", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("subclass"))
                        crt.Properties.Add(new PropertyStruct("Sub Class", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("catalog"))
                        crt.Properties.Add(new PropertyStruct("Catalog", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("publisher"))
                        crt.Properties.Add(new PropertyStruct("Publisher", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("developer"))
                        crt.Properties.Add(new PropertyStruct("Developer", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("region"))
                        crt.Properties.Add(new PropertyStruct("Region", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("players"))
                        crt.Properties.Add(new PropertyStruct("Players", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("date"))
                        crt.Properties.Add(new PropertyStruct("Released", XMLread.Value.ToString()));
                }
                //cartridge info
                if (XMLread.Name == "cartridge" & XMLread.IsStartElement())
                {
                    if (XMLread.MoveToAttribute("system"))
                        crt.Properties.Add(new PropertyStruct("System", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("crc"))
                        crt.CRC = XMLread.Value;
                    if (XMLread.MoveToAttribute("sha1"))
                        crt.SHA1 = XMLread.Value;
                }
                if (XMLread.Name == "board" & XMLread.IsStartElement())
                {
                    if (XMLread.MoveToAttribute("type"))
                        crt.Properties.Add(new PropertyStruct("Board", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("pcb"))
                        crt.Properties.Add(new PropertyStruct("Board PCB", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("mapper"))
                        crt.Properties.Add(new PropertyStruct("Board Mapper", XMLread.Value.ToString()));
                    files.Add(crt);
                }
                i++;
                if (i < databaseStream.Length)
                {
                    int x = (i * 100) / (int)databaseStream.Length;
                    OnProgress(x, ls["Status_ReadingDatabaseFile"] + " .... [" + x + " %]");
                }
            }
            XMLread.Close();
            databaseStream.Close();
            OnProgressFinished(ls["Status_Done"]);
            return files;
        }
    }
}
