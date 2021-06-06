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
using System.Threading;
using System.IO;

namespace AHD.EO.Base
{
    public class RomsComparer : IComparer<Rom>
    {
        public RomsComparer(bool az, RomSortType type)
        {
            this.az = az;
            this.type = type;
            this.icID = "";
        }
        public RomsComparer(bool az, RomSortType type, string icID, Profile profile)
        {
            this.az = az;
            this.type = type;
            this.icID = icID;
            this.profile = profile;
        }
        bool az;
        RomSortType type;
        string icID = "";
        Profile profile;

        public int Compare(Rom x, Rom y)
        {
            switch (this.type)
            {
                case RomSortType.Name:
                    if (az)
                        return (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name);
                    else
                        return (-1 * (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name));
                case RomSortType.FileType:
                    if (az)
                        return (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(Path.GetExtension(x.Path).Replace(".", ""), Path.GetExtension(y.Path).Replace(".", ""));
                    else
                        return (-1 * (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(Path.GetExtension(x.Path).Replace(".", ""), Path.GetExtension(y.Path).Replace(".", "")));
                case RomSortType.Path:
                    if (az)
                        return (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.IsURL ? x.URL : x.Path, y.IsURL ? y.URL : y.Path);
                    else
                        return (-1 * (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.IsURL ? x.URL : x.Path, y.IsURL ? y.URL : y.Path));
                case RomSortType.Rating:
                    if (az)
                        return (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.Rating, y.Rating);
                    else
                        return (-1 * (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.Rating, y.Rating));
                case RomSortType.Size:
                    {
                        if (az)
                            return CompareSize(x, y);
                        else
                            return -1 * CompareSize(x, y);
                    }
                case RomSortType.PlayedTimes:
                    if (az)
                        return (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.PlayedTimes, y.PlayedTimes);
                    else
                        return (-1 * (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.PlayedTimes, y.PlayedTimes));
                case RomSortType.PlayTime:
                    if (az)
                        return (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.PlayedTimeLength, y.PlayedTimeLength);
                    else
                        return (-1 * (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(x.PlayedTimeLength, y.PlayedTimeLength));
                case RomSortType.ICDebend:
                    if (az)
                        return CompareIC(x, y);
                    else
                        return -1 * CompareIC(x, y);
            }
            return -1;
        }
        private int CompareIC(Rom x, Rom y)
        {
            int romx = 0;
            int romy = 0;
            InformationContainer container = profile.GetInformationContainerByID(icID);
            if (container != null)
            {
                if (container.GetType().IsSubclassOf(typeof(FilesInFolderInformationContainer)))
                {
                    //x
                    if (x.IsICItemExist(icID))
                    {
                        romx++;
                        List<string> files = ((InformationContainerFilesInFolderItem)x.GetICItem(icID)).Files;
                        if (files != null)
                        {
                            if (files.Count > 0)
                            {
                                romx += files.Count;
                            }
                        }
                    }
                    //y
                    if (y.IsICItemExist(icID))
                    {
                        romy++;
                        List<string> files = ((InformationContainerFilesInFolderItem)y.GetICItem(icID)).Files;
                        if (files != null)
                        {
                            if (files.Count > 0)
                            {
                                romy += files.Count;
                            }
                        }
                    }
                }
                else if (container.GetType() == typeof(LinksInformationContainer))
                {
                    //x
                    if (x.IsICItemExist(icID))
                    {
                        romx++;
                        List<string> links = ((InformationContainerLinksItem)x.GetICItem(icID)).Links;
                        if (links != null)
                        {
                            if (links.Count > 0)
                            {
                                romx += links.Count;
                            }
                        }
                    }
                    //y
                    if (y.IsICItemExist(icID))
                    {
                        romy++;
                        List<string> links = ((InformationContainerLinksItem)y.GetICItem(icID)).Links;
                        if (links != null)
                        {
                            if (links.Count > 0)
                            {
                                romy += links.Count;
                            }
                        }
                    }
                }
                else if (container is RomDataInformationContainer)
                {
                    RomDataICItem itemx = (RomDataICItem)x.GetICItem(icID);
                    RomDataICItem itemy = (RomDataICItem)y.GetICItem(icID);
                    string romXText = "";
                    string romYText = "";
                    if (itemx != null)
                        romXText = itemx.Text;
                    if (itemy != null)
                        romYText = itemy.Text;

                    return (StringComparer.Create(Thread.CurrentThread.CurrentCulture, false)).Compare(romXText, romYText);
                }
            }
            return romx - romy;
        }
        private int CompareSize(Rom x, Rom y)
        {
            double xSize = 0;
            double ySize = 0;
            if (x.Size != "")
            {
                string[] split = x.Size.Split(new char[] {' ' });
                if (double.TryParse(split[0], out xSize))
                {
                    switch (split[1].ToLower())
                    {
                        case "byte": break;
                        case "kb": xSize *= 1024; break;
                        case "mb": xSize *= (1024 * 1024); break;
                        case "gb": xSize *= (1024 * 1024 * 1024); break;
                    }
                }
            }
            if (y.Size != "")
            {
                string[] split = y.Size.Split(new char[] { ' ' });
                if (double.TryParse(split[0], out ySize))
                {
                    switch (split[1].ToLower())
                    {
                        case "byte": break;
                        case "kb": ySize *= 1024; break;
                        case "mb": ySize *= (1024 * 1024); break;
                        case "gb": ySize *= (1024 * 1024 * 1024); break;
                    }
                }
            }
            return (int)(xSize - ySize);
        }
    }
    public enum RomSortType
    {
        Name, Size, Path, FileType, Rating, ICDebend, PlayedTimes, PlayTime
    }
}
