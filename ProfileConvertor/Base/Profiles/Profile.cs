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
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Xml.Serialization;
namespace AHD.EO.Base
{
    [Serializable()]
    [XmlInclude(typeof(FilesInFolderInformationContainer))]
    [XmlInclude(typeof(ImagesInformationContainer))]
    [XmlInclude(typeof(InfoTextInformationContainer))]
    [XmlInclude(typeof(LinksInformationContainer))]
    [XmlInclude(typeof(YoutubeInformationContainer))]
    [XmlInclude(typeof(ManualsInformationContainer))]
    [XmlInclude(typeof(RomDataInformationContainer))]
    [XmlInclude(typeof(RomInfoInformationContainer))]
    [XmlInclude(typeof(SoundsInformationContainer))]
    [XmlInclude(typeof(VideosInformationContainer))]
    [XmlInclude(typeof(RomInfoInformationContainer))]
    public class Profile
    {
        public Profile()
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            resources = new ResourceManager("AHD.EO.Base.LanguageResources.Resource", Assembly.GetExecutingAssembly());
            name = resources.GetString("Untitled");
            //add default containers
            BuildDefaultRomData();

            //snapshots
            InformationContainer container = new RomInfoInformationContainer(resources.GetString("RomInfo"));
            informationContainers.Add(container);
            //snapshots
            container = new ImagesInformationContainer(resources.GetString("Snapshots"));
            informationContainers.Add(container);
            //covers
            container = new ImagesInformationContainer(resources.GetString("Covers"));
            informationContainers.Add(container);
            //sounds
            container = new SoundsInformationContainer(resources.GetString("Sounds"));
            container.Location = InformationContainerLocation.TopLeft;
            informationContainers.Add(container);
            //videos
            container = new VideosInformationContainer(resources.GetString("Videos"));
            container.Location = InformationContainerLocation.TopLeft;
            informationContainers.Add(container);
            //info
            container = new InfoTextInformationContainer(resources.GetString("Info"));
            container.Location = InformationContainerLocation.DownLeft;
            informationContainers.Add(container);
            //history
            container = new InfoTextInformationContainer(resources.GetString("History"));
            container.Location = InformationContainerLocation.DownLeft;
            informationContainers.Add(container);
            //manuals
            container = new ManualsInformationContainer(resources.GetString("Manuals"));
            container.Location = InformationContainerLocation.DownLeft;
            informationContainers.Add(container);
            //links
            container = new LinksInformationContainer(resources.GetString("Links"));
            container.Location = InformationContainerLocation.DownLeft;
            informationContainers.Add(container);
            //youtube video
            container = new YoutubeInformationContainer(resources.GetString("YoutubeVideos"));
            container.Location = InformationContainerLocation.DownLeft;
            informationContainers.Add(container);

            //build columns
            BuildDefaultColumns();
            BuildColumnsForContainers();
            RebuildTabsPriority();
        }
        private ResourceManager resources;
        private string name = "Untitled";
        private List<ConsolesGroup> groups = new List<ConsolesGroup>();
        private List<PlaylistsGroup> playlistGroups = new List<PlaylistsGroup>();
        private List<InformationContainer> informationContainers = new List<InformationContainer>();
        private List<ColumnItem> columns = new List<ColumnItem>();
        private bool enablTabsPriority;
        private List<string> tabsPriority_TopLeft = new List<string>();
        private List<string> tabsPriority_TopRight = new List<string>();
        private List<string> tabsPriority_DownLeft = new List<string>();
        private List<string> tabsPriority_DownRight = new List<string>();
        private List<string> tabsPriority_TopMiddle = new List<string>();
        private List<string> tabsPriority_DownMiddle = new List<string>();

        /// <summary>
        /// Get or set the profile name
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Get or set console groups collection
        /// </summary>
        public List<ConsolesGroup> ConsoleGroups
        {
            get { return groups; }
            set { groups = value; }
        }

        /// <summary>
        /// Get or set the Playlist Groups collection
        /// </summary>
        public List<PlaylistsGroup> PlaylistGroups
        {
            get { return playlistGroups; }
            set { playlistGroups = value; }
        }

        /// <summary>
        /// Get or set theinformation containers collection
        /// </summary>
        public List<InformationContainer> InformationContainers
        { get { return informationContainers; } set { informationContainers = value; } }

        /// <summary>
        /// Check console groups if given name is taken
        /// </summary>
        /// <param name="name">The name of the group to check, not case sensitive</param>
        /// <returns>True if existed otherwise false</returns>
        public bool IsConsolesGroupExist(string name)
        {
            foreach (ConsolesGroup group in groups)
            {
                if (name.ToLower() == group.Name.ToLower())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check console groups if given name is taken
        /// </summary>
        /// <param name="name">The name of the group to check, not case sensitive</param>
        /// <returns>True if existed otherwise false</returns>
        public bool IsPlaylistsGroupExist(string name)
        {
            foreach (PlaylistsGroup group in playlistGroups)
            {
                if (name.ToLower() == group.Name.ToLower())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get a console by id
        /// </summary>
        /// <param name="id">The console id</param>
        /// <returns>The console if found, otherwise null</returns>
        public Console GetConsoleByID(string id)
        {
            foreach (ConsolesGroup group in groups)
            {
                foreach (Console console in group.Consoles)
                {
                    if (console.ID.ToLower() == id.ToLower())
                        return console;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a playlist by name
        /// </summary>
        /// <param name="name">The playlist name</param>
        /// <returns>The playlist if found, otherwise null</returns>
        public Playlist GetPlaylistByName(string name)
        {
            foreach (PlaylistsGroup group in playlistGroups)
            {
                foreach (Playlist playlist in group.PlayLists)
                {
                    if (playlist.Name.ToLower() == name.ToLower())
                        return playlist;
                }
            }
            return null;
        }

        /// <summary>
        /// Call this after adding a console into the collection
        /// </summary>
        /// <param name="console">The console that added to the collection</param>
        public void OnAddConsole(AHD.EO.Base.Console console)
        {
            RefreshInformationContainersForConsole(console);
        }

        public void RefreshInformationContainersForAllConsoles()
        {
            foreach (ConsolesGroup gr in groups)
            {
                foreach (Console console in gr.Consoles)
                {
                    RefreshInformationContainersForConsole(console);
                }
            }
        }

        public void ClearInformationContainerFromConsoles(string id)
        {
            foreach (ConsolesGroup gr in groups)
            {
                foreach (Console console in gr.Consoles)
                {
                    InformationContainerConsole item = console.GetICElementByID(id);
                    console.ICItems.Remove(item);
                }
            }
        }

        public void ChangeIDOfInformationContainer(string oldID, string newID)
        {
            foreach (ConsolesGroup gr in groups)
            {
                foreach (Console console in gr.Consoles)
                {
                    console.UpdateInformationContainerID(oldID, newID);
                }
            }
        }

        public void RefreshInformationContainersForConsole(AHD.EO.Base.Console console)
        {
            console.ICItems = new List<InformationContainerConsole>();
            console.ThunmpnailsViewPriority = new List<string>();
            foreach (InformationContainer container in informationContainers)
            {
                if (container.GetType().IsSubclassOf(typeof(FilesInFolderInformationContainer)))
                {
                    console.ICItems.Add(new ICFilesInFolderConsoleItem(container));
                }
                else if (container.GetType() == typeof(LinksInformationContainer))
                {
                    console.ICItems.Add(new ICConsoleItem(container));
                }
                else if (container.GetType() == typeof(YoutubeInformationContainer))
                {
                    console.ICItems.Add(new ICConsoleItem(container));
                }
                else if (container.GetType() == typeof(RomInfoInformationContainer))
                {
                    console.ICItems.Add(new ICConsoleItem(container));
                }

                if (container is ImagesInformationContainer)
                    console.ThunmpnailsViewPriority.Add(container.Name);
            }
        }

        public int GetInformationContainerIndex(string id)
        {
            for (int i = 0; i < informationContainers.Count; i++)
            {
                if (informationContainers[i].ID == id)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Get an information container element id
        /// </summary>
        /// <param name="name">The container name</param>
        /// <returns>The container ID</returns>
        public string GetInformationContainerID(string name)
        {
            string val = "";
            foreach (InformationContainer container in informationContainers)
            {
                if (container.Name == name)
                    return container.ID;
            }
            return val;
        }

        public InformationContainer GetInformationContainerByID(string id)
        {
            foreach (InformationContainer container in informationContainers)
            {
                if (id == container.ID)
                    return container;
            }
            return null;
        }

        public void BuildDefaultRomData()
        {
            RomDataInformationContainer container = new RomDataInformationContainer(resources.GetString("ReleaseDate"));
            informationContainers.Add(container);

            container = new RomDataInformationContainer(resources.GetString("Publisher"));
            informationContainers.Add(container);

            container = new RomDataInformationContainer(resources.GetString("DevelopedBy"));
            informationContainers.Add(container);

            container = new RomDataInformationContainer(resources.GetString("Genre"));
            informationContainers.Add(container);
        }

        #region Columns
        public List<ColumnItem> Columns
        { get { return columns; } set { columns = value; } }
        public void BuildDefaultColumns()
        {
            columns = new List<ColumnItem>();
            for (int i = 0; i < ColumnItem.DEFAULTCOLUMNS.Length / 2; i++)
            {
                ColumnItem item = new ColumnItem();
                item.ColumnName = ColumnItem.DEFAULTCOLUMNS[i, 0];
                item.ColumnID = ColumnItem.DEFAULTCOLUMNS[i, 1];
                item.Width = 60;
                item.Visible = true;
                columns.Add(item);
            }
        }
        public void BuildColumnsForContainers()
        {
            foreach (InformationContainer container in informationContainers)
            {
                ColumnItem item = new ColumnItem();
                item.ColumnID = container.ID;
                item.ColumnName = container.Name;
                item.Width = 60;
                item.Visible = true;
                columns.Add(item);
            }
        }
        public void RebuildColumnsForContainers()
        {
            //Clear the columns that no longer needed
            for (int i = 0; i < columns.Count; i++)
            {
                if (ColumnItem.IsDefaultColumn(columns[i].ColumnID))
                    continue;
                bool found = false;
                foreach (InformationContainer container in informationContainers)
                {

                    if (container.ID == columns[i].ColumnID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    columns.RemoveAt(i);
                    i = -1;
                }
            }
            //add new column if the container's column not exists
            foreach (InformationContainer container in informationContainers)
            {
                bool found = false;
                foreach (ColumnItem item in columns)
                {
                    if (ColumnItem.IsDefaultColumn(item.ColumnID))
                        continue;
                    if (container.ID == item.ColumnID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ColumnItem item = new ColumnItem();
                    item.ColumnID = container.ID;
                    item.ColumnName = container.Name;
                    item.Width = 60;
                    item.Visible = true;
                    columns.Add(item);
                }
            }
        }
        #endregion
        #region Tabs Priority
        public bool TabsPriorityEnabled
        { get { return enablTabsPriority; } set { enablTabsPriority = value; } }
        public List<string> TabsPriority_TopLeft
        { get { return tabsPriority_TopLeft; } set { tabsPriority_TopLeft = value; } }
        public List<string> TabsPriority_TopRight
        { get { return tabsPriority_TopRight; } set { tabsPriority_TopRight = value; } }
        public List<string> TabsPriority_DownLeft
        { get { return tabsPriority_DownLeft; } set { tabsPriority_DownLeft = value; } }
        public List<string> TabsPriority_DownRight
        { get { return tabsPriority_DownRight; } set { tabsPriority_DownRight = value; } }
        public List<string> TabsPriority_DownMiddle
        { get { return tabsPriority_DownMiddle; } set { tabsPriority_DownMiddle = value; } }
        public List<string> TabsPriority_TopMiddle
        { get { return tabsPriority_TopMiddle; } set { tabsPriority_TopMiddle = value; } }
        public void RebuildTabsPriority()
        {
            tabsPriority_TopLeft = new List<string>();
            tabsPriority_TopRight = new List<string>();
            tabsPriority_DownLeft = new List<string>();
            tabsPriority_DownRight = new List<string>();
            tabsPriority_TopMiddle = new List<string>();
            tabsPriority_DownMiddle = new List<string>();
            foreach (InformationContainer container in informationContainers)
            {
                if (!(container is RomDataInformationContainer))
                {
                    switch (container.Location)
                    {
                        case InformationContainerLocation.DownLeft: tabsPriority_DownLeft.Add(container.Name); break;
                        case InformationContainerLocation.DownRight: tabsPriority_DownRight.Add(container.Name); break;
                        case InformationContainerLocation.TopLeft: tabsPriority_TopLeft.Add(container.Name); break;
                        case InformationContainerLocation.TopRight: tabsPriority_TopRight.Add(container.Name); break;
                        case InformationContainerLocation.TopMiddle: tabsPriority_TopMiddle.Add(container.Name); break;
                        case InformationContainerLocation.DownMiddle: tabsPriority_DownMiddle.Add(container.Name); break;
                    }
                }
            }
        }
        #endregion
    }
}
