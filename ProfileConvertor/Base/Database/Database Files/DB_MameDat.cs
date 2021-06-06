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
using System.Xml;
using System.Security.Cryptography;

namespace AHD.EO.Base
{
    public class DB_MameDat : IDatabaseFile
    {
        List<DatabaseRom> files = new List<DatabaseRom>();
        public string Name
        {
            get { return "CLRMamePro (Mame dat)"; }
        }

        public List<DatabaseRom> Files
        {
            get { return files; }
            set { files = value; }
        }

        public bool OpenFile(string fileName)
        {
           // try
            {
                //1 clear the database
                files = new List<DatabaseRom>();
                //2 read the xml file
                Stream databaseStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.DtdProcessing = DtdProcessing.Ignore;
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(databaseStream, sett);

                if (Progress != null)
                    Progress(this, new ProgressArg(0, "Reading database file..."));
                int i = 0;
                string name = "";
                DatabaseRom crt = new DatabaseRom();
                crt.FileNames = new List<string>();
                crt.Additions = new List<string>();
                while (XMLread.Read())
                {
                    //Is it a game ?
                    if (XMLread.Name == "game" & XMLread.IsStartElement())
                    {
                        crt = new DatabaseRom();
                        crt.FileNames = new List<string>();
                        crt.Additions = new List<string>();
                        if (XMLread.MoveToAttribute("name"))
                            name = XMLread.Value;
                        if (XMLread.MoveToAttribute("cloneof"))
                        {
                            crt.Additions.Add("true");
                            crt.Additions.Add(XMLread.Value);
                        }
                        else
                        {
                            crt.Additions.Add("false");
                            crt.Additions.Add("");
                        }
                        if (XMLread.MoveToAttribute("romof"))
                        {
                            crt.Additions.Add("true");
                            crt.Additions.Add(XMLread.Value);
                        }
                        else
                        {
                            crt.Additions.Add("false");
                            crt.Additions.Add("");
                        }
                        XMLread.Read();//advance to <description>
                    }
                    if (XMLread.Name == "description" & XMLread.IsStartElement())
                    {
                        //if (crt.FileNames == null)
                        //    crt.FileNames = new List<string>();
                        crt.FileNames.Add(name);
                        crt.Name = XMLread.ReadString();
                    }
                    if (XMLread.Name == "rom" & XMLread.IsStartElement())
                    {
                        if (XMLread.MoveToAttribute("crc"))
                            crt.Additions.Add(XMLread.Value);
                    }
                    if (XMLread.Name == "game" & !XMLread.IsStartElement())
                    {
                        files.Add(crt);
                    }
                    i++;
                    if (i < databaseStream.Length)
                        Progress(this, new ProgressArg((i * 100) / (int)databaseStream.Length, "Reading database file..."));
                }
                XMLread.Close();
                databaseStream.Close();
                if (Progress != null)
                    Progress(this, new ProgressArg(100, "Done"));
                return true;
            }
            //catch { }
            return false;
        }

        public bool SaveFile(string fileName)
        {
            return false;
        }

        public string Extension
        {
            get { return "dat"; }
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
            get { return false; }
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
            get { return false; }
        }

        public SeparateItem[] GetSeparate(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
