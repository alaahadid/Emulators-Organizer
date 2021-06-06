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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Class represents rom
    /// </summary>
    [Serializable()]
    public class Rom : IEOElement
    {
        /// <summary>
        /// Class represents rom
        /// </summary>
        /// <param name="name">The rom name</param>
        /// <param name="id">The rom id</param>
        public Rom(string name, string id)
            : base(name, id)
        {
            Modified = false;
        }
        /// <summary>
        /// Class represents rom
        /// </summary>
        /// <param name="id">The rom id</param>
        public Rom(string id)
            : base("", id)
        {
            Modified = false;
        }

        private List<string> categories = new List<string>();
        private List<RomDataInfoItem> romDataInfoItems = new List<RomDataInfoItem>();
        private List<InformationContainerItem> romInfoItems = new List<InformationContainerItem>();
        private List<RomEmulatorParentCommandlines> romEmulatorCommands = new List<RomEmulatorParentCommandlines>();
        private List<CommandlinesGroup> commandlineGroupsWhenExecutingWithoutEmulator = new List<CommandlinesGroup>();
        private CommandlinesUsageMode commandlineUsage = CommandlinesUsageMode.Emulator;
        private ProgramUsageMode programsUsageMode = ProgramUsageMode.Emulator;
        private List<ProgramProperties> programsToLaunchBefore = new List<ProgramProperties>();
        private List<ProgramProperties> programsToLaunchAfter = new List<ProgramProperties>();
        private List<string> childrenRoms = new List<string>();
        private string parentConsoleID = "";
        private long fileSize = 0;
        private string path = "";
        private int rating = 0;
        private int playedTimes = 0;
        private long playedTimeLength = 0;
        private bool ignorePathNotExist;
        private bool isParentRom;
        private bool isChildRom;
        private bool parentChildrenChoose = false;
        private string parentRomID;
        private DateTime lastPlayed;
        private int indexForConsole = -1;

        // Properties
        /// <summary>
        /// Get or set the parent console id
        /// </summary>
        public string ParentConsoleID
        { get { return parentConsoleID; } set { parentConsoleID = value; Modified = true; } }
        /// <summary>
        /// Get or set parent categories
        /// </summary>
        public List<string> Categories
        { get { return categories; } set { categories = value; Modified = true; } }
        /// <summary>
        /// Get or set the rom file size
        /// </summary>
        public long FileSize
        { get { return fileSize; } set { fileSize = value; Modified = true; } }
        /// <summary>
        /// Get the rom file size as lable
        /// </summary>
        public string SizeLable
        { get { return HelperTools.GetSize(fileSize); } }
        /// <summary>
        /// Get or set the rom path (file path or internet link) 
        /// </summary>
        public string Path
        { get { return path; } set { path = value; Modified = true; } }
        /// <summary>
        /// Get or set the rating; 0=no rating, 1-5 =starts count
        /// </summary>
        public int Rating
        { get { return rating; } set { rating = value; Modified = true; } }
        /// <summary>
        /// Get or set how many time this rom played
        /// </summary>
        public int PlayedTimes
        { get { return playedTimes; } set { playedTimes = value; Modified = true; } }
        /// <summary>
        /// Get or set how many milliseconds this rom played
        /// </summary>
        public long PlayedTimeLength
        { get { return playedTimeLength; } set { playedTimeLength = value; Modified = true; } }
        /// <summary>
        /// Get or set a value indicates that this rom is a parent rom which contains roms.
        /// </summary>
        public bool IsParentRom
        {
            get
            {
                return isParentRom;
            }
            set
            {
                isParentRom = value;
                Modified = true;
            }
        }
        /// <summary>
        /// Get or set if this rom is a child rom
        /// </summary>
        public bool IsChildRom
        {
            get { return isChildRom; }
            set { isChildRom = value; Modified = true; }
        }
        /// <summary>
        /// If set, the profile will always permit user to choose children when attempting to play a parent rom.
        /// </summary>
        public bool AlwaysChooseChildWhenPlay
        {
            get { return parentChildrenChoose; }
            set { parentChildrenChoose = value; Modified = true; }
        }
        public string ParentRomID
        {
            get { return parentRomID; }
            set { parentRomID = value; Modified = true; }
        }
        /// <summary>
        /// Get if this rom is single or not (not parent nor child, calculated)
        /// </summary>
        public bool IsSingle
        {
            get { return (!isChildRom && !isParentRom); }
        }
        /// <summary>
        /// Get or set the collection of children roms.
        /// </summary>
        public List<string> ChildrenRoms
        {
            get
            {
                if (this.childrenRoms == null)
                    this.childrenRoms = new List<string>();
                return this.childrenRoms;
            }
            set
            {
                this.childrenRoms = value; Modified = true;
            }
        }
        /// <summary>
        /// Get or set the last played date
        /// </summary>
        public DateTime LastPlayed
        { get { return lastPlayed; } set { lastPlayed = value; Modified = true; } }
        /// <summary>
        /// Get or set a collection of rom data info items
        /// </summary>
        public List<RomDataInfoItem> RomDataInfoItems
        { get { return romDataInfoItems; } set { romDataInfoItems = value; Modified = true; } }
        /// <summary>
        /// Get or set a collection of information container items.
        /// </summary>
        public List<InformationContainerItem> RomInfoItems
        { get { return romInfoItems; } set { romInfoItems = value; Modified = true; } }
        public CommandlinesUsageMode CommandlinesUsageMode
        { get { return commandlineUsage; } set { commandlineUsage = value; Modified = true; } }
        public ProgramUsageMode ProgramsUsageMode
        {
            get { return programsUsageMode; }
            set { programsUsageMode = value; Modified = true; }
        }
        public List<ProgramProperties> ProgramsToLaucnhBefore
        { get { return programsToLaunchBefore; } set { programsToLaunchBefore = value; Modified = true; } }
        public List<ProgramProperties> ProgramsToLaucnhAfter
        { get { return programsToLaunchAfter; } set { programsToLaunchAfter = value; Modified = true; } }
        public List<CommandlinesGroup> CommandlineGroupsWhenExecutingWithoutEmulator
        {
            get
            {
                if (commandlineGroupsWhenExecutingWithoutEmulator == null)
                    commandlineGroupsWhenExecutingWithoutEmulator = new List<CommandlinesGroup>();
                return commandlineGroupsWhenExecutingWithoutEmulator;
            }
            set
            {
                if (value == null)
                    value = new List<CommandlinesGroup>();
                commandlineGroupsWhenExecutingWithoutEmulator = value;
                Modified = true;
            }
        }
        /// <summary>
        /// Get or set if this rom is modified. VERY IMPORTANT FOR SAVING !
        /// </summary>
        public bool Modified { get; set; }
        /// <summary>
        /// Get or set the rom index for the console (for sort)
        /// </summary>
        public int IndexWithinConsole
        {
            get { return indexForConsole; }
            set { indexForConsole = value; }
        }

        // Methods
        /// <summary>
        /// Get value for rom data info
        /// </summary>
        /// <param name="RomDataInfoID">The rom data info element id</param>
        /// <returns>Object value in expected type</returns>
        public object GetDataItemValue(string RomDataInfoID)
        {
            foreach (RomDataInfoItem item in romDataInfoItems)
            {
                if (item.ID == RomDataInfoID)
                    return item.Value;
            }
            return null;
        }
        /// <summary>
        /// Check if a data info item is existed
        /// </summary>
        /// <param name="RomDataInfoID">The rom data info id</param>
        /// <returns>True if found otherwise false</returns>
        public bool IsDataItemExist(string RomDataInfoID)
        {
            foreach (RomDataInfoItem item in romDataInfoItems)
            {
                if (item.ID == RomDataInfoID)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Update data info item value
        /// </summary>
        /// <param name="RomDataInfoID">The rom data info id</param>
        /// <param name="value">The value to use.</param>
        public void UpdateDataInfoItemValue(string RomDataInfoID, object value)
        {
            bool found = false;
            foreach (RomDataInfoItem item in romDataInfoItems)
            {
                if (item.ID == RomDataInfoID)
                {
                    item.Value = value;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                RomDataInfoItem item = new RomDataInfoItem();
                item.ID = RomDataInfoID;
                item.Value = value;
                romDataInfoItems.Add(item);
                Modified = true;
            }
        }
        /// <summary>
        /// Delete associated items for an information container
        /// </summary>
        /// <param name="icid"></param>
        public void DeleteInformationContainerItems(string icid)
        {
            if (romInfoItems != null)
            {
                for (int i = 0; i < romInfoItems.Count; i++)
                {
                    if (romInfoItems[i].ParentID == icid)
                    {
                        romInfoItems.RemoveAt(i);
                        Modified = true;
                        i--;
                    }
                }
            }
        }
        public void DeleteDataInfoItemItems(string icid)
        {
            if (romDataInfoItems != null)
            {
                for (int i = 0; i < romDataInfoItems.Count; i++)
                {
                    if (romDataInfoItems[i].ID == icid)
                    {
                        romDataInfoItems.RemoveAt(i);
                        Modified = true; i--;
                    }
                }
            }
        }
        public bool IsInformationContainerItemExist(string icid)
        {
            if (romInfoItems != null)
            {
                for (int i = 0; i < romInfoItems.Count; i++)
                {
                    if (romInfoItems[i].ParentID == icid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IgnorePathNotExist
        {
            get { return ignorePathNotExist; }
            set { ignorePathNotExist = value; Modified = true; }
        }
        public InformationContainerItem GetInformationContainerItem(string icid)
        {
            if (romInfoItems != null)
            {
                for (int i = 0; i < romInfoItems.Count; i++)
                {
                    if (romInfoItems[i].ParentID == icid)
                    {
                        return romInfoItems[i];
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Get command-line groups for given emulator
        /// </summary>
        /// <param name="emulatorID">The emu id</param>
        /// <returns>List of command-line groups for given emulator</returns>
        public List<CommandlinesGroup> GetCommandlinesGroupsForEmulator(string emulatorID)
        {
            if (romEmulatorCommands == null)
                romEmulatorCommands = new List<RomEmulatorParentCommandlines>();
            foreach (RomEmulatorParentCommandlines p in romEmulatorCommands)
            {
                if (p.EmulatorID == emulatorID) { return p.CommandlineGroups; }
            }
            // Reached here means no group found for given emulator. Add new one
            RomEmulatorParentCommandlines newItem = new RomEmulatorParentCommandlines(emulatorID);
            romEmulatorCommands.Add(newItem);
            return newItem.CommandlineGroups;
        }
        /// <summary>
        /// Update emulator command-lines 
        /// </summary>
        /// <param name="emulatorID"></param>
        /// <param name="commandlineGroups"></param>
        public void UpdateEmulatorCommandlines(string emulatorID, List<CommandlinesGroup> commandlineGroups)
        {
            if (romEmulatorCommands == null)
                romEmulatorCommands = new List<RomEmulatorParentCommandlines>();
            foreach (RomEmulatorParentCommandlines p in romEmulatorCommands)
            {
                if (p.EmulatorID == emulatorID)
                {
                    p.CommandlineGroups.Clear();
                    foreach (CommandlinesGroup gr in commandlineGroups)
                        p.CommandlineGroups.Add(gr.Clone());
                    Modified = true; return;// Done !
                }
            }
            // Reached here means no emulator link found here.
            RomEmulatorParentCommandlines newItem = new RomEmulatorParentCommandlines(emulatorID);
            foreach (CommandlinesGroup gr in commandlineGroups)
                newItem.CommandlineGroups.Add(gr.Clone());
            romEmulatorCommands.Add(newItem);
            Modified = true;
        }
    }
}
