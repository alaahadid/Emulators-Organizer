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
namespace EmulatorsOrganizer.Core
{
    [Serializable]
    public abstract class InformationContainerFiles : InformationContainer
    {
        public InformationContainerFiles(string id)
            : base(id)
        {
            this.Extenstions = new List<string>(DefaultExtensions);
            this.FoldersMemory = new List<string>();
        }
        /// <summary>
        /// Get the extensions supported by this info type !
        /// </summary>
        public virtual List<string> Extenstions { get; set; }
        /// <summary>
        /// Get default extensions list
        /// </summary>
        public abstract string[] DefaultExtensions { get; }
        /// <summary>
        /// Get or set the folders list that can be used as memory.
        /// </summary>
        public virtual List<string> FoldersMemory { get; set; }
        /// <summary>
        /// Get the extensions joined as single line with ;
        /// </summary>
        /// <returns></returns>
        public virtual string GetExtensionsJoined()
        {
            string val = "";
            if (this.Extenstions != null)
                foreach (string ex in Extenstions)
                    val += ex + ";";
            if (val.Length > 0)
                val = val.Substring(0, val.Length - 1);
            return val;
        }
        /// <summary>
        /// Get the default extensions joined as single line with ;
        /// </summary>
        /// <returns></returns>
        public virtual string GetDefaultExtensionsJoined()
        {
            string val = "";
            if (this.DefaultExtensions != null)
                foreach (string ex in DefaultExtensions)
                    val += ex + ";";
            if (val.Length > 0)
                val = val.Substring(0, val.Length - 1);
            return val;
        }
        /// <summary>
        /// Get filter to use in open/save dialogs. The filter involve DefaultExtensions only (not the Extensions)
        /// </summary>
        /// <returns></returns>
        public virtual string GetExtensionDialogFilter()
        {
            string filter = base.DisplayName + "|";
            foreach (string ex in DefaultExtensions)
            {
                filter += "*" + ex + ";";
            }
            return filter.Substring(0, filter.Length - 1);
        }
    }
}
