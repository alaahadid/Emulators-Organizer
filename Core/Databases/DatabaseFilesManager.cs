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
using System.Reflection;

namespace EmulatorsOrganizer.Core
{
    public class DatabaseFilesManager
    {
        public static DatabaseFile[] AvailableFormats { get; set; }
        /// <summary>
        /// Detect all database file supported formats
        /// </summary>
        public static void DetectSupportedFormats()
        {
            List<DatabaseFile> availableFormats = new List<DatabaseFile>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type tp in types)
            {
                if (tp.IsSubclassOf(typeof(DatabaseFile)))
                {
                    DatabaseFile format = Activator.CreateInstance(tp) as DatabaseFile;
                    availableFormats.Add(format);
                }
            }
            AvailableFormats = availableFormats.ToArray();
        }
        /// <summary>
        /// Get a filter that can be used in open/save dialog
        /// </summary>
        /// <returns></returns>
        public static string GetFilter()
        {
            string _Filter = "";
            foreach (DatabaseFile formm in AvailableFormats)
            {
                if (!formm.IsVisible)
                    continue;

                _Filter += formm.Name + "|";
                foreach (string ex in formm.Extensions)
                {
                    if (ex == formm.Extensions[formm.Extensions.Length - 1])
                    { _Filter += "*" + ex; }
                    else { _Filter += "*" + ex + ";"; }
                }
                if (formm != AvailableFormats[AvailableFormats.Length - 1])
                { _Filter += "|"; }
            }
            return _Filter;
        }
        public static string GetFilter(DatabaseFile db)
        {
            string _Filter = db.Name + "|";
            foreach (string ex in db.Extensions)
            {
                if (ex == db.Extensions[db.Extensions.Length - 1])
                { _Filter += "*" + ex; }
                else { _Filter += "*" + ex + ";"; }
            }

            return _Filter;
        }
    }
}
