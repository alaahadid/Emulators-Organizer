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
using System.IO;
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// The roms comparer
    /// </summary>
    public class RomComparer : IComparer<Rom>
    {
        /// <summary>
        /// The roms comparer
        /// </summary>
        /// <param name="aToZ">Sort by A to Z or Z to A</param>
        /// <param name="type">The type of the sort</param>
        public RomComparer(bool aToZ, RomCompareType type)
        {
            this.aToZ = aToZ;
            this.type = type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aToZ"></param>
        /// <param name="column"></param>
        public RomComparer(bool aToZ, string column)
        {
            this.aToZ = aToZ;
            this.columnID = column;
            switch (column)
            {
                case "console": type = RomCompareType.Console; break;
                case "name": type = RomCompareType.Name; break;
                case "size": type = RomCompareType.Size; break;
                case "file type": type = RomCompareType.FileType; break;
                case "last played": type = RomCompareType.LastPlayed; break;
                case "play time": type = RomCompareType.PlayTime; break;
                case "played times": type = RomCompareType.PlayedTimes; break;
                case "rating": type = RomCompareType.Rating; break;
                case "path": type = RomCompareType.Path; break;
                default: type = RomCompareType.Datainfo; break;
            }
        }
        private bool aToZ = true;
        private RomCompareType type = RomCompareType.Name;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private string columnID;

        /// <summary>
        /// Compare 2 roms
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Rom x, Rom y)
        {
            switch (type)
            {
                case RomCompareType.Console:
                    {
                        string x_console = profileManager.Profile.Consoles[x.ParentConsoleID].Name;
                        string y_console = profileManager.Profile.Consoles[y.ParentConsoleID].Name;
                        if (aToZ)
                            return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x_console, y_console);
                        else
                            return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x_console, y_console));
                    }
                case RomCompareType.Name:
                    {
                        if (aToZ)
                            return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name);
                        else
                            return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name));
                    }
                case RomCompareType.Index:
                    {
                        if (aToZ)
                            return (x.IndexWithinConsole - y.IndexWithinConsole);
                        else
                            return (y.IndexWithinConsole - x.IndexWithinConsole);
                    }
                case RomCompareType.Size:
                    {
                        if (aToZ)
                            return (int)(x.FileSize - y.FileSize);
                        else
                            return (int)(y.FileSize - x.FileSize);
                    }
                case RomCompareType.FileType:
                    {
                        string x_type = Path.GetExtension(HelperTools.GetPathFromAIPath(x.Path)).Replace(".", "");
                        string y_type = Path.GetExtension(HelperTools.GetPathFromAIPath(y.Path)).Replace(".", "");
                        if (aToZ)
                            return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x_type, y_type);
                        else
                            return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x_type, y_type));
                    }
                case RomCompareType.LastPlayed:
                    {
                        if (aToZ)
                            return (int)(x.LastPlayed.Ticks - y.LastPlayed.Ticks);
                        else
                            return (int)(y.LastPlayed.Ticks - x.LastPlayed.Ticks);
                    }
                case RomCompareType.PlayTime:
                    {
                        if (aToZ)
                            return (int)(x.PlayedTimeLength - y.PlayedTimeLength);
                        else
                            return (int)(y.PlayedTimeLength - x.PlayedTimeLength);
                    }
                case RomCompareType.PlayedTimes:
                    {
                        if (aToZ)
                            return (x.PlayedTimes - y.PlayedTimes);
                        else
                            return (y.PlayedTimes - x.PlayedTimes);
                    }
                case RomCompareType.Rating:
                    {
                        if (aToZ)
                            return (x.Rating - y.Rating);
                        else
                            return (y.Rating - x.Rating);
                    }
                case RomCompareType.Path:
                    {
                        if (aToZ)
                            return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(x.Path)), HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(y.Path)));
                        else
                            return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(x.Path)), HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(y.Path))));
                    }
                case RomCompareType.Datainfo:
                    {
                        return CompareOthers(x, y);
                    }
                default: return -1;
            }
        }
        private int CompareOthers(Rom x, Rom y)
        {
            // Data info ?
            if (x.IsDataItemExist(columnID) || y.IsDataItemExist(columnID))
            {
                EmulatorsOrganizer.Core.Console xconsole = profileManager.Profile.Consoles[x.ParentConsoleID];
                switch (xconsole.GetDataInfo(columnID).Type)// one of them can determine type
                {
                    case RomDataType.Text:
                        {
                            string x_inf = "";
                            string y_inf = "";
                            object x_ob = x.GetDataItemValue(columnID);
                            object y_ob = y.GetDataItemValue(columnID);
                            if (x_ob != null)
                                x_inf = x_ob.ToString();
                            else
                                x_inf = "";

                            if (y_ob != null)
                                y_inf = y_ob.ToString();
                            else
                                y_inf = "";

                            if (aToZ)
                                return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x_inf, y_inf);
                            else
                                return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x_inf, y_inf));
                        }
                    case RomDataType.Number:
                        {
                            int x_inf = 0;
                            int y_inf = 0;
                            object x_ob = x.GetDataItemValue(columnID);
                            object y_ob = y.GetDataItemValue(columnID);
                            if (x_ob != null)
                                x_inf = (int)x_ob;
                            else
                                x_inf = 0;

                            if (y_ob != null)
                                y_inf = (int)y_ob;
                            else
                                y_inf = 0;

                            if (aToZ)
                                return (int)(x_inf - y_inf);
                            else
                                return (int)(y_inf - x_inf);
                        }
                }
            }
            // IC
            else if (x.IsInformationContainerItemExist(columnID) || y.IsInformationContainerItemExist(columnID))
            {
                int x_count = 0;
                int y_count = 0;
                InformationContainerItem x_item = x.GetInformationContainerItem(columnID);
                InformationContainerItem y_item = y.GetInformationContainerItem(columnID);
                // X item
                if (x_item != null)
                {
                    if (x_item is InformationContainerItemFiles)
                    {
                        if (((InformationContainerItemFiles)x_item).Files != null)
                            x_count = ((InformationContainerItemFiles)x_item).Files.Count;
                    }
                    else if (x_item is InformationContainerItemLinks)
                    {
                        if (((InformationContainerItemLinks)x_item).Links != null)
                            x_count = ((InformationContainerItemLinks)x_item).Links.Count;
                    }
                    else
                    {
                        // TODO: add more ...
                    }
                }
                if (y_item != null)
                {
                    if (y_item is InformationContainerItemFiles)
                    {
                        if (((InformationContainerItemFiles)y_item).Files != null)
                            y_count = ((InformationContainerItemFiles)y_item).Files.Count;
                    }
                    else if (y_item is InformationContainerItemLinks)
                    {
                        if (((InformationContainerItemLinks)y_item).Links != null)
                            y_count = ((InformationContainerItemLinks)y_item).Links.Count;
                    }
                    else
                    {
                        // TODO: add more compare possiblities...
                    }
                }
                System.Diagnostics.Trace.WriteLine("Result=" + (x_count - y_count));
                if (aToZ)
                    return (x_count - y_count);
                else
                    return (y_count - x_count);
            }
            // Reached here mean nothing to compare ...
            return 0;
        }
    }
    /// <summary>
    /// Rom compare type
    /// </summary>
    public enum RomCompareType
    {
        /// <summary>
        /// Sort by console
        /// </summary>
        Console,
        /// <summary>
        /// Compare by name
        /// </summary>
        Name,
        /// <summary>
        /// Sort by rom size
        /// </summary>
        Size,
        /// <summary>
        /// Sort by file type
        /// </summary>
        FileType,
        /// <summary>
        /// Sort by last played
        /// </summary>
        LastPlayed,
        /// <summary>
        /// Sort by play time
        /// </summary>
        PlayTime,
        /// <summary>
        /// Sort by played times
        /// </summary>
        PlayedTimes,
        /// <summary>
        /// Sort by rating
        /// </summary>
        Rating,
        /// <summary>
        /// Sort by path
        /// </summary>
        Path,
        /// <summary>
        /// Sort by data info or ic
        /// </summary>
        Datainfo,
        /// <summary>
        /// Sort by the index for the console.
        /// </summary>
        Index,
    }
}
