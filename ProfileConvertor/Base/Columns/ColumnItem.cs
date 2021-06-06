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
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
namespace AHD.EO.Base
{
    [Serializable()]
    public class ColumnItem
    {
        public string ColumnID = "";
        public string ColumnName = "";
        public bool Visible = true;
        public int Width = 60;
        private static ResourceManager resources =
            new ResourceManager("AHD.EO.Base.LanguageResources.Resource", Assembly.GetExecutingAssembly());

        public static string[,] DEFAULTCOLUMNS
        {
            get
            {
                return new string[,]  {
          { resources.GetString("Name"),       "name" } ,
          { resources.GetString("Size"),       "size" } ,
          { resources.GetString("FileType"),   "file type" } ,
          { resources.GetString("PlayedTimes"),"played times" } ,
          { resources.GetString("PlayTime"),   "play time" } ,
          { resources.GetString("Rating"),     "rating" } ,
          { resources.GetString("Path"),       "path" }                        
                                      };
            }
        }

        public static bool IsDefaultColumn(string id)
        {
            for (int i = 0; i < DEFAULTCOLUMNS.Length / 2; i++)
            {
                if (DEFAULTCOLUMNS[i, 1] == id)
                    return true;
            }
            return false;
        }
    }
}
