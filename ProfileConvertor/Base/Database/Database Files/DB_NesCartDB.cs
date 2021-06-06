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
    class DB_NesCartDB : IDatabaseFile
    {
        List<DatabaseRom> files = new List<DatabaseRom>();
        int bytesToSkip = 16;
        public string Name
        {
            get { return "Nes Cart DB http://bootgod.dyndns.org:7777/home.php (*.xml)"; }
        }
        public List<DatabaseRom> Files
        {
            get { return files; }
            set { files = value; }
        }
        public bool OpenFile(string fileName)
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
            DatabaseRom crt = new DatabaseRom();
            while (XMLread.Read())
            {
                //Is it a game ?
                if (XMLread.Name == "game" & XMLread.IsStartElement())
                {
                    crt = new DatabaseRom();
                    crt.Properties = new List<DBProperty>();
                    if (XMLread.MoveToAttribute("name"))
                        crt.Name = XMLread.Value;
                    if (XMLread.MoveToAttribute("altname"))
                        crt.Properties.Add(new DBProperty("Alternative Name", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("class"))
                        crt.Properties.Add(new DBProperty("Class", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("subclass"))
                        crt.Properties.Add(new DBProperty("Sub Class", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("catalog"))
                        crt.Properties.Add(new DBProperty("Catalog", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("publisher"))
                        crt.Properties.Add(new DBProperty("Publisher", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("developer"))
                        crt.Properties.Add(new DBProperty("Developer", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("region"))
                        crt.Properties.Add(new DBProperty("Region", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("players"))
                        crt.Properties.Add(new DBProperty("Players", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("date"))
                        crt.Properties.Add(new DBProperty("Released", XMLread.Value.ToString()));
                }
                //cartridge info
                if (XMLread.Name == "cartridge" & XMLread.IsStartElement())
                {
                    if (XMLread.MoveToAttribute("system"))
                        crt.Properties.Add(new DBProperty("System", XMLread.Value.ToString()));
                    if (XMLread.MoveToAttribute("crc"))
                        crt.CRC = XMLread.Value;
                    if (XMLread.MoveToAttribute("sha1"))
                        crt.SHA1 = XMLread.Value;
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
        public bool SaveFile(string fileName)
        {
            return false;
        }
        public string Extension
        {
            get { return "xml"; }
        }
        public event EventHandler<ProgressArg> Progress;

        public bool IsSupportCRC
        {
            get { return true; }
        }
        public bool IsSupportSHA1
        {
            get { return true; }
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
            get { return false; }
        }

        public string CalculateSHA1(string filePath)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                byte[] fileBuffer = new byte[fileStream.Length - bytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                fileStream.Close();

                string Sha1 = "";
                SHA1Managed managedSHA1 = new SHA1Managed();
                byte[] shaBuffer = managedSHA1.ComputeHash(fileBuffer);

                foreach (byte b in shaBuffer)
                    Sha1 += b.ToString("x2").ToLower();

                return Sha1;
            }
            return "";
        }
        public string CalculateMD5(string filePath)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                byte[] fileBuffer = new byte[fileStream.Length - bytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                fileStream.Close();

                string md5 = "";
                MD5 m = MD5.Create();
                byte[] md5Buffer = m.ComputeHash(fileBuffer);

                foreach (byte b in md5Buffer)
                    md5 += b.ToString("x2").ToLower();

                return md5;
            }
            return "";
        }
        public string CalculateCRC(string filePath)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                byte[] fileBuffer = new byte[fileStream.Length - bytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
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
            get { return false; }
        }

        public SeparateItem[] GetSeparate(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
