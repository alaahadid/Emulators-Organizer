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
    [Serializable()]
    [XmlInclude(typeof(Bitmap))]
    [XmlInclude(typeof(InformationContainerFilesInFolderItem))]
    [XmlInclude(typeof(InformationContainerLinksItem))]
    [XmlInclude(typeof(InformationContainerYoutubeVideoItem))]
    [XmlInclude(typeof(RomDataICItem))]
    [XmlInclude(typeof(YoutubeLink))]
    public class Rom
    {
        public Rom()
        { }
        public Rom(string consoleID)
        {
            this.consoleID = consoleID;
        }
        private string name = "";
        private string consoleID = "";
        private string path = "";
        private string size = "0 KB";
        private int rating = 0;
        private bool isURL = false;
        private string url = "";
        private string downloadFileName = "";
        private int playedTimes = 0;
        private double playedTimeLength = 0;
        private bool hasSpecialCommandlines = false;
        private List<SpecialCommandlinesGroup> cmgroups = new List<SpecialCommandlinesGroup>();
        private List<string> categories = new List<string>();
        private List<InformationContainerItem> icItems = new List<InformationContainerItem>();
        private List<string> attachedFiles = new List<string>();
        private Image icon;

        /// <summary>
        /// Get or set the name of this rom
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the file path of this rom
        /// </summary>
        public string Path
        { get { return path; } set { path = value; } }
        /// <summary>
        /// Get or set the size of this rom
        /// </summary>
        public string Size
        { get { return size; } set { size = value; } }
        /// <summary>
        /// Get or set the rating of this rom. 0=none, 1-5 stars
        /// </summary>
        public int Rating
        { get { return rating; } set { rating = value; } }
        /// <summary>
        /// Get or set the console id of this rom.
        /// </summary>
        public string ConsoleID
        { get { return consoleID; } set { consoleID = value; } }
        [XmlIgnore()]
        public Image IconImage
        { get { return icon; } set { icon = value; } }
        public bool IsURL
        { get { return isURL; } set { isURL = value; } }
        public string URL
        { get { return url; } set { url = value; } }
        public string DownloadFileName
        { get { return downloadFileName; } set { downloadFileName = value; } }
        public int PlayedTimes { get { return playedTimes; } set { playedTimes = value; } }
        public double PlayedTimeLength { get { return playedTimeLength; } set { playedTimeLength = value; } }
        /// <summary>
        /// Get or set if this rom has special commandlines
        /// </summary>
        public bool HasSpecialCommandlines
        {
            get
            {
                return hasSpecialCommandlines;
            }
            set { hasSpecialCommandlines = value; }
        }
        public List<string> AttachedFiles
        { get { return attachedFiles; } set { attachedFiles = value; } }
        public List<SpecialCommandlinesGroup> SpecialCommandlineGroups
        { get { return cmgroups; } set { cmgroups = value; } }
        public bool HasCommandlinesForEmulator(Emulator emu)
        {
            if (cmgroups == null)
                cmgroups = new List<SpecialCommandlinesGroup>();
            foreach (SpecialCommandlinesGroup gr in cmgroups)
            {
                if (gr.Emulator == emu)
                    return true;
            }
            return false;
        }
        public List<CommandlinesGroup> GetCommandlinesGroupForEmulator(Emulator emu)
        {
            if (cmgroups == null)
                cmgroups = new List<SpecialCommandlinesGroup>();
            foreach (SpecialCommandlinesGroup gr in cmgroups)
            {
                if (gr.Emulator == emu)
                    return gr.CommandlinesGroups;
            }
            return null;
        }
        public List<string> Categories
        { get { return categories; } set { categories = value; } }
        public List<InformationContainerItem> ICItems
        {
            get { return icItems; }
            set { icItems = value; }
        }
        public bool IsICItemExist(string containerID)
        {
            foreach (InformationContainerItem item in icItems)
            {
                if (item.ContainerID == containerID)
                    return true;
            }
            return false;
        }
        public InformationContainerItem GetICItem(string containerID)
        {
            foreach (InformationContainerItem item in icItems)
            {
                if (item.ContainerID == containerID)
                    return item;
            }
            return null;
        }
        /// <summary>
        /// Get a clone of this rom
        /// </summary>
        /// <returns>The clone rom</returns>
        public Rom Clone()
        {
            Rom newrom = new Rom(this.consoleID);
            newrom.downloadFileName = this.downloadFileName;
            newrom.hasSpecialCommandlines = this.hasSpecialCommandlines;
            newrom.isURL = this.isURL;
            newrom.name = this.name;
            newrom.path = this.path;
            newrom.rating = this.rating;
            newrom.size = this.size;
            newrom.url = this.url;
            newrom.cmgroups = new List<SpecialCommandlinesGroup>();
            if (this.icon != null)
                newrom.IconImage = (Bitmap)this.icon.Clone();
            foreach (SpecialCommandlinesGroup gr in this.cmgroups)
            {
                newrom.cmgroups.Add(gr.Clone());
            }
            newrom.categories = new List<string>();
            foreach (string cat in this.categories)
                newrom.categories.Add(cat);
            newrom.icItems = new List<InformationContainerItem>();
            foreach (InformationContainerItem item in this.icItems)
            {
                if (item is InformationContainerFilesInFolderItem)
                {
                    InformationContainerFilesInFolderItem newic = new InformationContainerFilesInFolderItem(item.ContainerID);
                    if (((InformationContainerFilesInFolderItem)item).Files != null)
                    {
                        newic.Files = new List<string>();
                        foreach (string file in ((InformationContainerFilesInFolderItem)item).Files)
                        {
                            newic.Files.Add(file);
                        }
                    }
                    newrom.ICItems.Add(newic);
                }
                else if (item is InformationContainerLinksItem)
                {
                    InformationContainerLinksItem newic = new InformationContainerLinksItem(item.ContainerID);
                    if (((InformationContainerLinksItem)item).Links != null)
                    {
                        newic.Links = new List<string>();
                        foreach (string link in ((InformationContainerLinksItem)item).Links)
                        {
                            newic.Links.Add(link);
                        }
                    }
                    newrom.ICItems.Add(newic);
                }
                else if (item is InformationContainerYoutubeVideoItem)
                {
                    InformationContainerYoutubeVideoItem newic = new InformationContainerYoutubeVideoItem(item.ContainerID);
                    if (((InformationContainerYoutubeVideoItem)item).Videos != null)
                    {
                        newic.Videos = new List<YoutubeLink>();
                        foreach (YoutubeLink link in ((InformationContainerYoutubeVideoItem)item).Videos)
                        {
                            YoutubeLink newv = new YoutubeLink();
                            newv.Link = link.Link;
                            newv.Name = link.Name;
                            newic.Videos.Add(newv);
                        }
                    }
                    newrom.ICItems.Add(newic);
                }
                else if (item is RomDataICItem)
                {
                    RomDataICItem newic = new RomDataICItem(item.ContainerID);
                    newic.Text = ((RomDataICItem)item).Text;
                    newrom.ICItems.Add(newic);
                }
                else
                {
                    // should not be an embty item
                }
            }
            return newrom;
        }
    }
}
