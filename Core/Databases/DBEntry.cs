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

namespace EmulatorsOrganizer.Core
{
    public class DBEntry
    {
        public DBEntry()
        {

        }
        /// <summary>
        /// IMPORTANT: this will set the category to null.
        /// </summary>
        /// <param name="crc"></param>
        /// <param name="sha1"></param>
        /// <param name="md5"></param>
        /// <param name="exportTempPath"></param>
        /// <param name="fileNames"></param>
        /// <param name="properties"></param>
        /// <param name="perfectMatchCRCS"></param>
        public DBEntry(string crc, string sha1, string md5, string exportTempPath, string[] categories, string[] fileNames,
            PropertyStruct[] properties, PerfectMatchFile[] perfectMatchCRCS)
        {
            if (fileNames != null)
                this.FileNames = new List<string>(fileNames);
            else
                this.FileNames = new List<string>();

            if (properties != null)
                this.Properties = new List<PropertyStruct>(properties);
            else
                this.Properties = new List<PropertyStruct>();

            this.CRC = crc;
            this.SHA1 = sha1;
            this.MD5 = md5;
            this.TemporaryPathForExport = exportTempPath;

            if (perfectMatchCRCS != null)
                this.PerfectMatchCRCS = new List<PerfectMatchFile>(perfectMatchCRCS);
            else
                this.PerfectMatchCRCS = new List<PerfectMatchFile>();

            if (categories != null)
                Categories = new List<string>(categories);
            else
                Categories = new List<string>();
        }

        public List<string> FileNames;
        public List<PropertyStruct> Properties;
        public List<PerfectMatchFile> PerfectMatchCRCS;
        public string CRC;
        public string SHA1;
        public string MD5;
        public string TemporaryPathForExport;
        public List<string> Categories;

        public string GetPropertyValue(string property)
        {
            string val = "";
            if (this.Properties == null)
                return "";
            foreach (PropertyStruct pr in Properties)
            {
                if (pr.Property == property)
                    return pr.Value;
            }
            return val;
        }
        public bool IsPRopertyExist(string property)
        {
            if (this.Properties == null)
                return false;
            foreach (PropertyStruct pr in Properties)
            {
                if (pr.Property == property)
                    return true;
            }
            return false;
        }
        public void UpdateProperty(string property, string newValue)
        {
            if (this.Properties == null)
                return;
            foreach (PropertyStruct pr in Properties)
            {
                if (pr.Property == property)
                {
                    pr.Value = newValue;
                    break;
                }
            }
        }

        public void AddCategory(string cat, string infoName)
        {
            if (cat != "" && infoName != "")
            {
                string n = infoName + "/" + cat;
                if (!Categories.Contains(n))
                    Categories.Add(n);
            }
        }
        public static DBEntry Empty
        {
            get { return new DBEntry("", "", "", "", null, null, null, null); }
        }
        public DBEntry Clone()
        {
            DBEntry dd = new DBEntry();

            dd.CRC = CRC;
            dd.MD5 = MD5;
            dd.SHA1 = SHA1;
            dd.TemporaryPathForExport = TemporaryPathForExport;

            dd.Categories = new List<string>();
            foreach (string cc in Categories)
                dd.Categories.Add(cc);

            dd.FileNames = new List<string>();
            foreach (string ff in FileNames)
                dd.FileNames.Add(ff);

            dd.Properties = new List<PropertyStruct>();
            foreach (PropertyStruct pp in Properties)
                dd.Properties.Add(pp.Clone());

            dd.PerfectMatchCRCS = new List<PerfectMatchFile>();
            foreach (PerfectMatchFile pp in PerfectMatchCRCS)
                dd.PerfectMatchCRCS.Add(pp.Clone());

            return dd;
        }
    }
}
