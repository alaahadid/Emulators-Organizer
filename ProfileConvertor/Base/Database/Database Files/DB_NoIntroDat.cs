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
using System.Security.Cryptography;
using System.Xml;
using AHD.Utilities;

namespace AHD.EO.Base
{
    public class DB_NoIntroDat : IDatabaseFile
    {
        List<DatabaseRom> files = new List<DatabaseRom>();
        public int bytesToSkip = 0;
        public bool IgnoreSkipForCompressed = true;
        public string dbHeaderRuleValue = "";
        public string dbName = "DataBase";
        public string dbDescription = "Created By Emulators Organizer 5";
        public string dbVersion = "";
        public string dbHeader = "";
        public string dbComment = "no-intro | www.no-intro.org";

        public string Name
        {
            get { return "No-Intro.com Dat database (*.dat)"; }
        }
        public string Extension
        {
            get { return "dat"; }
        }
        public List<DatabaseRom> Files
        {
            get { return files; }
            set { files = value; }
        }

        public bool OpenFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            if (Progress != null)
                Progress(this, new ProgressArg(0, "Reading database file..."));
            files = new List<DatabaseRom>();
            IgnoreSkipForCompressed = true;
            bytesToSkip = 0;
            dbHeaderRuleValue = "";
            bool skipFlag = false;

            Frm_NoIntroDatImport frm = new Frm_NoIntroDatImport();
            frm.ShowDialog();
            skipFlag = frm.IsManualSkip;
            if (skipFlag)
            { bytesToSkip = frm.BytesToSkip; }

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length >= 4)
                {
                    //1 look if we have header, then read and skip...
                    if (!skipFlag)
                    {
                        if (lines[i].Length >= "clrmamepro".Length)
                        {
                            if (lines[i].Substring(0, "clrmamepro".Length) == "clrmamepro")
                            {
                                i += 3;
                                if (lines[i].Contains("header"))
                                {
                                    string[] texts = lines[i].Split(new char[] { '"' });
                                    string headerFileName = texts[1];
                                    if (File.Exists(Path.GetDirectoryName(fileName) + "\\" + headerFileName))
                                    {
                                        Stream databaseStream = new FileStream(Path.GetDirectoryName(fileName) + "\\" + headerFileName, FileMode.Open, FileAccess.Read);
                                        XmlReaderSettings sett = new XmlReaderSettings();
                                        sett.DtdProcessing = DtdProcessing.Ignore;
                                        sett.IgnoreWhitespace = true;
                                        XmlReader XMLread = XmlReader.Create(databaseStream, sett);
                                        while (XMLread.Read())
                                        {
                                            if (XMLread.Name == "name" & XMLread.IsStartElement())
                                            {
                                                if (XMLread.ReadString().Contains("iNES"))
                                                    bytesToSkip = 16;
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
                    }
                    if (lines[i].Substring(0, 4) == "game")
                    {
                        try
                        {
                            DatabaseRom rom = new DatabaseRom();
                            i++;
                            string[] texts = lines[i].Split(new char[] { '"' });
                            rom.Name = texts[1];
                            i++;
                            texts = lines[i].Split(new char[] { '"' });
                            rom.Description = texts[1];
                            i++;
                            texts = lines[i].Split(new char[] { '"' });
                            texts = texts[2].Split(new char[] { ' ' });
                            rom.CRC = texts[4];
                            rom.MD5 = texts[6];
                            rom.SHA1 = texts[8];
                            files.Add(rom);
                        }
                        catch { }
                    }
                }
                Progress(this, new ProgressArg((i * 100) / lines.Length, "Reading database file..."));
            }
            if (Progress != null)
                Progress(this, new ProgressArg(100, "Done"));
            return true;
        }
        public bool SaveFile(string fileName)
        {
            if (Progress != null)
                Progress(this, new ProgressArg(0, "Saving database file..."));
            dbVersion = (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2")).ToString();
            Frm_NoIntroDat frm = new Frm_NoIntroDat(this);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
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
                foreach (DatabaseRom rom in files)
                {
                    lines.Add("game (");
                    lines.Add("\t" + @"name """ + rom.Name + @"""");
                    lines.Add("\t" + @"description """ + rom.Description + @"""");
                    string line = "\t" + "rom ( name " + @"""" + Path.GetFileName(rom.Path) + @""""
                        + " size " + AHD.Utilities.HelperTools.GetSizeAsBytes(rom.Path)
                        + " crc " + this.CalculateCRC(rom.Path).ToUpper()
                        + " md5 " + this.CalculateMD5(rom.Path).ToUpper()
                        + " sha1 " + this.CalculateSHA1(rom.Path).ToUpper() + " )";
                    lines.Add(line);
                    lines.Add(")");
                    lines.Add("");
                    Progress(this, new ProgressArg((i * 100) / files.Count, "Saving database file..."));
                    i++;
                }
                File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
                if (Progress != null)
                    Progress(this, new ProgressArg(100, "Done"));
                return true;
            }
            if (Progress != null)
                Progress(this, new ProgressArg(100, "Canceled By User"));
            return false;
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
            get { return true; }
        }
        public bool IsSupportFileNameCompare
        {
            get { return false; }
        }
        public bool IsSupportSave
        {
            get { return true; }
        }
        byte[] getBuffer(string filePath)
        {
            byte[] fileBuffer;
            if (!IgnoreSkipForCompressed)
            {
                if (dbHeaderRuleValue != "")//if we have a header values, we need to skip by rules...
                {
                    Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    byte[] header = new byte[bytesToSkip];
                    fileStream.Read(header, 0, bytesToSkip);
                    int index = 0;
                    bool skip = true;
                    for (int i = 0; i < dbHeaderRuleValue.Length; i += 2)
                    {
                        if (string.Format("{0:X}", header[index]).ToUpper() != dbHeaderRuleValue.Substring(i, 2))
                        {
                            skip = false; break;//we found a byte that not match
                        }
                        index++;
                    }
                    if (skip)
                    {
                        fileBuffer = new byte[fileStream.Length - bytesToSkip];
                        fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                    }
                    else
                    {
                        fileStream.Position = 0;
                        fileBuffer = new byte[fileStream.Length];
                        fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
                    }
                    fileStream.Close();
                }
                else//no rules, just skip...
                {
                    Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                    fileBuffer = new byte[fileStream.Length - bytesToSkip];
                    fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                    fileStream.Close();
                }
            }//with ignore
            else
            {
                string[] exs = new string[] { ".7z", ".rar", ".zip", ".xz", ".gzip", ".bzip2", ".tar", "wim" };
                bool ignore = false;
                foreach (string ex in exs)
                {
                    if (Path.GetExtension(filePath).ToLower() == "")
                    { ignore = true; break; }
                }
                if (ignore)
                {
                    Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    fileBuffer = new byte[fileStream.Length];
                    fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
                    fileStream.Close();
                }
                else
                {
                    if (dbHeaderRuleValue != "")//if we have a header values, we need to skip by rules...
                    {
                        Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        byte[] header = new byte[bytesToSkip];
                        fileStream.Read(header, 0, bytesToSkip);
                        int index = 0;
                        bool skip = true;
                        for (int i = 0; i < dbHeaderRuleValue.Length; i += 2)
                        {
                            if (string.Format("{0:X}", header[index]).ToUpper() != dbHeaderRuleValue.Substring(i, 2))
                            {
                                skip = false; break;//we found a byte that not match
                            }
                            index++;
                        }
                        if (skip)
                        {
                            fileBuffer = new byte[fileStream.Length - bytesToSkip];
                            fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                        }
                        else
                        {
                            fileStream.Position = 0;
                            fileBuffer = new byte[fileStream.Length];
                            fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
                        }
                        fileStream.Close();
                    }
                    else//no rules, just skip...
                    {
                        Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                        fileBuffer = new byte[fileStream.Length - bytesToSkip];
                        fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                        fileStream.Close();
                    }
                }
            }
            return fileBuffer;
        }

        public string CalculateSHA1(string filePath)
        {
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = getBuffer(filePath);

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
                byte[] fileBuffer = getBuffer(filePath);

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
                byte[] fileBuffer = getBuffer(filePath);

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