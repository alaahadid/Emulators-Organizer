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
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
namespace AHD.EO.Base
{
    public class DatabaseManager
    {
        private static IDatabaseFile[] supportedFormats;

        public static IDatabaseFile[] SupportedFormats
        { get { return supportedFormats; } }

        public static void DetectDataBaseFiles()
        {
            List<IDatabaseFile> formats = new List<IDatabaseFile>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type tp in types)
            {
                Type[] inters = tp.GetInterfaces();
                if (inters.Contains(typeof(IDatabaseFile)))
                {
                    IDatabaseFile format = Activator.CreateInstance(tp) as IDatabaseFile;
                    formats.Add(format);
                }
            }
            supportedFormats = formats.ToArray();
        }
        public static string GetFilter()
        {
            return GetFilter(false);
        }
        public static string GetFilter(bool IgnoreNotSupportExport)
        {
            string _Filter = "";
            foreach (IDatabaseFile formm in supportedFormats)
            {
                if (IgnoreNotSupportExport)
                    if (!formm.IsSupportSave)
                        continue;
                _Filter += formm.Name + "|*." + formm.Extension + "|";
            }
            return _Filter.Substring(0, _Filter.Length - 1);
        }
        public static IDatabaseFile GetFromIndex(int exIndex)
        {
            return GetFromIndex(exIndex, false);
        }
        public static IDatabaseFile GetFromIndex(int exIndex, bool IgnoreNotSupportExport)
        {
            //1 create list
            List<IDatabaseFile> databaseFiles = new List<IDatabaseFile>();
            foreach (IDatabaseFile formm in supportedFormats)
            {
                if (IgnoreNotSupportExport)
                    if (!formm.IsSupportSave)
                        continue;
                databaseFiles.Add(formm);
            }

            return databaseFiles[exIndex];
        }
    }
}
