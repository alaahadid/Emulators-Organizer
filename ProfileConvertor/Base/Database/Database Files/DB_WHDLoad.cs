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
    class DB_WHDLoad : IDatabaseFile
    {
        private List<DatabaseRom> files = new List<DatabaseRom>();
        public string Name
        {
            get { return "WHDLoad (*.map)"; }
        }
        public List<DatabaseRom> Files
        {
            get { return files; }
            set { files = value; }
        }

        public bool OpenFile(string fileName)
        {
            //clear files
            files = new List<DatabaseRom>();
            //read lines
            string[] lines = File.ReadAllLines(fileName);

              //decode lines
            for (int i = 0; i < lines.Length; i++)
            {
                string[] code = lines[i].Split(new char[] { '|' });
                if (code.Length > 1)
                {
                    DatabaseRom ROM = new DatabaseRom();
                    ROM.Name = code[1];
                    ROM.FileNames = new List<string>();
                    ROM.FileNames.Add(code[0]);
                    ROM.Properties = new List<DBProperty>();

                    files.Add(ROM);
                }
                if (Progress != null)
                    Progress(this, new ProgressArg((i * 100) / lines.Length, "Reading map file ..."));
            } 
            return files.Count > 0;
        }

        public bool SaveFile(string fileName)
        {
           if (Progress != null)
                Progress(this, new ProgressArg(0, "Saving database file..."));
            List<string> lines = new List<string>();
            int i = 0;
            foreach (DatabaseRom file in files)
            {
                lines.Add(Path.GetFileNameWithoutExtension(file.Path) + "|" + file.Name);
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
            get { return "map"; }
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
            get { return false; }
        }

        public SeparateItem[] GetSeparate(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
