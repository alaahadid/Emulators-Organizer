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
using System.Drawing;
using System.Xml.Serialization;
namespace AHD.EO.Base
{
    /// <summary>
    /// Class represents conole
    /// </summary>
    [Serializable()]
    [XmlInclude(typeof(Bitmap))]
    [XmlInclude(typeof(ICConsoleItem))]
    [XmlInclude(typeof(ICFilesInFolderConsoleItem))]
    public class Console
    {
        public Console()
        { 
        }
        public Console(string name)
        {
            this.Name = name;
        }
        private string name = "New Console";
        private string id = "new console";
        private string downloadedRomsFolder = "";
        private bool copyRomToDownloadsFolder = false;
        private bool extractRomFirstIfArchive = true;
        private List<Rom> roms = new List<Rom>();
        private List<Emulator> emulators = new List<Emulator>();
        private List<ConsoleCategory> categories = new List<ConsoleCategory>();
        private List<string> extensions = new List<string>();
        private Image icon;
        private List<string> memory_RomFolders = new List<string>();
        private List<InformationContainerConsole> icItems = new List<InformationContainerConsole>();
        private List<string> thunmpnailsViewPriority = new List<string>();

        public string Name
        { get { return name; } set { name = value; id = name.ToLower(); UpdateRomsID(); } }
        public string ID
        { get { return id; } set { id = value; UpdateRomsID(); } }
        public override string ToString()
        {
            return name;
        }
        public List<Rom> Roms
        { get { return roms; } set { roms = value; } }
        public List<Emulator> Emulators
        {
            get { return emulators; }
            set { emulators = value; }
        }
        public List<ConsoleCategory> Categories
        { get { return categories; } set { categories = value; } }
        public List<string> Extensions
        { get { return extensions; } set { extensions = value; } }
        [XmlIgnore()]
        public Image Icon
        { get { return icon; } set { icon = value; } }
        public List<string> Memory_RomFolders
        { get { return memory_RomFolders; } set { memory_RomFolders = value; } }
        public string DownloadedRomsFolder
        { get { return downloadedRomsFolder; } set { downloadedRomsFolder = value; } }
        public bool CopyRomToDownloadsFolder
        { get { return copyRomToDownloadsFolder; } set { copyRomToDownloadsFolder = value; } }
        public List<InformationContainerConsole> ICItems
        { get { return icItems; } set { icItems = value; } }
        public bool ExtractRomFirstIfArchive
        { get { return extractRomFirstIfArchive; } set { extractRomFirstIfArchive = value; } }
        public List<string> ThunmpnailsViewPriority
        { get { return thunmpnailsViewPriority; } set { thunmpnailsViewPriority = value; } }
        public bool IsCategoryExist(string name)
        {
            foreach (ConsoleCategory category in categories)
                if (category.Name.ToLower() == name.ToLower())
                    return true;
            return false;
        }
        public void UpdateRomsID()
        {
            foreach (Rom rom in this.roms)
                rom.ConsoleID = id;
        }
        public bool IsEmulatorExist(string name)
        {
            foreach (Emulator emu in emulators)
                if (emu.Name.ToLower() == name.ToLower())
                    return true;
            return false;
        }
        public InformationContainerConsole GetICElementByID(string id)
        {
            foreach (InformationContainerConsole item in icItems)
            {
                if (item.ParentContainer.ID == id)
                    return item;
            }
            return null;
        }
        public void UpdateInformationContainerID(string oldID, string newID)
        {
            foreach (Rom rom in roms)
            {
                foreach (InformationContainerItem item in rom.ICItems)
                {
                    if (item.ContainerID == oldID)
                        item.ContainerID = newID;
                }
            }
        }
        /// <summary>
        /// Check if this playlist ahas a rom
        /// </summary>
        /// <param name="rom">The rom to check</param>
        /// <returns>True if this rom existed, otherwiser false</returns>
        public bool IsRomExist(Rom rom)
        {
            foreach (Rom trom in roms)
            {
                if (trom.Name == rom.Name && trom.Path == rom.Path)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Create a clone copy of this console except roms and emulators.
        /// </summary>
        /// <returns></returns>
        public AHD.EO.Base.Console Clone()
        {
            AHD.EO.Base.Console newConsole = new AHD.EO.Base.Console();
            newConsole.categories = new List<ConsoleCategory>();
            foreach (ConsoleCategory cat in this.categories)
            {
                ConsoleCategory newcat = new ConsoleCategory();
                newcat.Icon = cat.Icon;
                newcat.Name = cat.Name;
                newConsole.categories.Add(newcat);
            }
            newConsole.copyRomToDownloadsFolder = this.copyRomToDownloadsFolder;
            newConsole.extensions = new List<string>();
            foreach (string ex in this.extensions)
            {
                newConsole.extensions.Add(ex);
            }
            newConsole.extractRomFirstIfArchive = this.extractRomFirstIfArchive;
            newConsole.icItems = new List<InformationContainerConsole>();
            foreach (InformationContainerConsole item in this.icItems)
            {
                newConsole.icItems.Add(item);
            }
            newConsole.icon = this.icon;
            newConsole.id = this.id;
            newConsole.memory_RomFolders = this.memory_RomFolders;
            newConsole.name = this.name;
            newConsole.thunmpnailsViewPriority = this.thunmpnailsViewPriority;
            return newConsole;
        }
    }
}
