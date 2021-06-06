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
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Emulators Organizer console.
    /// </summary>
    [Serializable()]
    public class Console : IEOElement
    {
        /// <summary>
        /// A class represents console
        /// </summary>
        /// <param name="name">The console name</param>
        /// <param name="id">The console id</param>
        /// <param name="parentID">The parent consoles groups id. Set to "" to add the console to no group</param>
        public Console(string name, string id, string parentID)
            : base(name, id)
        {
            this.parentID = parentID;
            extensions.AddRange(new string[] { ".rar", ".7z", ".zip" });
            // Columns
            base.BuildDefaultColumns(new string[] { "console" });
            // Data and info
            BuildDefaultRomDataInfoList();
            BuildDefaultInformationContainers();
            FixColumnsForRomDataInfo();

            enableCommandlines = enableEmulator = true;
            useRomWorkingDirectory = false;
            // Archive
            extractRomIfArchive = true;
            extractFirstFileIfArchiveIncludeMoreThanOne = false;
            extractAllFilesOfArchive = false;
            archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
        }
        /// <summary>
        /// A class represents console
        /// </summary>
        /// <param name="id">The console id</param>
        /// <param name="parentID">The parent consoles groups id. Set to "" to add the console to no group</param>
        public Console(string id, string parentID)
            : base("", id)
        {
            this.parentID = parentID;
            extensions.AddRange(new string[] { ".rar", ".7z", ".zip" });
            // Columns
            base.BuildDefaultColumns(new string[] { "console" });
            // Data and info
            BuildDefaultRomDataInfoList();
            BuildDefaultInformationContainers();
            FixColumnsForRomDataInfo();

            enableCommandlines = enableEmulator = true;
            useRomWorkingDirectory = false;
            // Archive
            extractRomIfArchive = true;
            extractFirstFileIfArchiveIncludeMoreThanOne = false;
            extractAllFilesOfArchive = false;
            archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
        }

        private string parentID = "";
        private string shortDescription = "";
        private string rtfPath = "";
        private string pdfPath = "";
        private bool copyRomBeforeLaunch = false;
        private string folderWhereToCopyRomWhenLaunch = "";
        private bool renameRomBeforeLaunch = false;
        private string romNameBeforeLaunch = "";
        private bool useSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch;
        private bool includeExtensionWhenrenaming = true;
        private bool parentAndChildrenMode = false;
        private List<string> extensions = new List<string>();
        private List<string> memory_RomFolders = new List<string>();
        private List<RomData> romDataInfo = new List<RomData>();
        private Dictionary<string, string> links = new Dictionary<string, string>();
        // Archive
        private bool extractRomIfArchive = true;
        private bool extractFirstFileIfArchiveIncludeMoreThanOne = false;
        private bool extractAllFilesOfArchive = false;
        private List<string> archiveExtensions = new List<string>();
        private List<string> archiveAllowedExtractionExtensions = new List<string>();

        private List<InformationContainer> informationContainers = new List<InformationContainer>();
        private InformationContainerTabsPanel informationContainersMap;
        private string thumbModeSelectionID;
        private bool autoSwitchTabPriorityDepend;
        private bool useRomWorkingDirectory;

        /// <summary>
        /// Build the default list of rom data info elements for this console
        /// </summary>
        public void BuildDefaultRomDataInfoList()
        {
            ProfileManager pr = (ProfileManager)ServicesManager.GetService("Profile Manager");
            LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
            romDataInfo = new List<RomData>();
            romDataInfo.Add(new RomData(pr.Profile.GenerateID(), ls["DefaultDataInfo_ReleaseDate"], RomDataType.Number));
            romDataInfo.Add(new RomData(pr.Profile.GenerateID(), ls["DefaultDataInfo_Publisher"], RomDataType.Text));
            romDataInfo.Add(new RomData(pr.Profile.GenerateID(), ls["DefaultDataInfo_DevelopedBy"], RomDataType.Text));
            romDataInfo.Add(new RomData(pr.Profile.GenerateID(), ls["DefaultDataInfo_Genre"], RomDataType.Text));
        }
        public void FixColumnsForRomDataInfo()
        {
            // Remove none default columns
            for (int i = 0; i < columns.Count; i++)
            {
                if (!IsDefaultColumn(columns[i].ColumnID))
                {
                    columns.RemoveAt(i);
                    i--;
                }
            }
            // Add columns for rom data info elements
            foreach (RomData data in romDataInfo)
            {
                ColumnItem newItem = new ColumnItem();
                newItem.ColumnID = data.ID;
                newItem.ColumnName = data.Name;
                newItem.Visible = true;
                newItem.Width = 70;

                columns.Add(newItem);
            }
            // Add columns for information container
            foreach (InformationContainer con in informationContainers)
            {
                if (con.Columnable)
                {
                    ColumnItem newItem = new ColumnItem();
                    newItem.ColumnID = con.ID;
                    newItem.ColumnName = con.DisplayName;
                    newItem.Visible = true;
                    newItem.Width = 70;

                    columns.Add(newItem);
                }
            }
        }
        public void UpdateRomsWithNewDataInfoList(List<RomData> newList)
        {
            ProfileManager pr = (ProfileManager)ServicesManager.GetService("Profile Manager");
            foreach (RomData data in romDataInfo)
            {
                // Let's see if this one deleted...
                bool found = false;
                RomDataType newType = RomDataType.Text;
                foreach (RomData d in newList)
                {
                    if (d.ID == data.ID)
                    {
                        found = true;
                        newType = d.Type;
                        break;
                    }
                }
                if (!found)
                {
                    // It's removed !
                    // Update roms ...
                    Rom[] roms = pr.Profile.Roms[id, false];
                    foreach (Rom r in roms)
                    {
                        foreach (RomDataInfoItem it in r.RomDataInfoItems)
                        {
                            if (it.ID == data.ID)
                            {
                                r.RomDataInfoItems.Remove(it);
                                r.Modified = true;
                                break;
                            }
                        }
                    }
                }
                else if (newType == RomDataType.Number)
                {
                    // Update roms to verify entered numbers
                    Rom[] roms = pr.Profile.Roms[id, false];
                    foreach (Rom r in roms)
                    {
                        foreach (RomDataInfoItem it in r.RomDataInfoItems)
                        {
                            if (it.ID == data.ID)
                            {
                                if (it.Value is string)
                                {
                                    int v = 0;
                                    if (!int.TryParse(it.Value.ToString(), out v))
                                    {
                                        it.Value = 0;// Set to null
                                    }
                                    else
                                    {
                                        it.Value = v;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        public void DeleteRomDataInfo(string id)
        {
            for (int i = 0; i < romDataInfo.Count; i++)
            {
                if (romDataInfo[i].ID == id)
                {
                    romDataInfo.RemoveAt(i);
                    break;
                }
            }
        }
        /// <summary>
        /// Build a default information containers map (this will delete all information containers !)
        /// </summary>
        public void BuildDefaultInformationContainers()
        {
            ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
            informationContainers = new List<InformationContainer>();

            // Generate display map
            informationContainersMap = new InformationContainerTabsPanel();
            informationContainersMap.IsHorizontal = true;
            // Add at the top panel
            informationContainersMap.TopPanel = new InformationContainerTabsPanel();

            InformationContainerRomInfo ic_rom = new InformationContainerRomInfo(profileManager.Profile.GenerateID());
            ic_rom.DisplayName = "Rom Info";
            informationContainers.Add(ic_rom);
            informationContainersMap.TopPanel.ContainerIDS.Add(ic_rom.ID);

            InformationContainerImage ic_snaps = new InformationContainerImage(profileManager.Profile.GenerateID());
            ic_snaps.DisplayName = "Snapshots";
            informationContainers.Add(ic_snaps);
            informationContainersMap.TopPanel.ContainerIDS.Add(ic_snaps.ID);

            InformationContainerImage ic_covers = new InformationContainerImage(profileManager.Profile.GenerateID());
            ic_covers.DisplayName = "Covers";
            informationContainers.Add(ic_covers);
            informationContainersMap.TopPanel.ContainerIDS.Add(ic_covers.ID);

            InformationContainerMedia ic_sound = new InformationContainerMedia(profileManager.Profile.GenerateID());
            ic_sound.DisplayName = "Sound";
            informationContainers.Add(ic_sound);
            informationContainersMap.TopPanel.ContainerIDS.Add(ic_sound.ID);

            InformationContainerMedia ic_videos = new InformationContainerMedia(profileManager.Profile.GenerateID());
            ic_videos.DisplayName = "Video";
            informationContainers.Add(ic_videos);
            informationContainersMap.TopPanel.ContainerIDS.Add(ic_videos.ID);

            InformationContainerYoutubeVideo ic_youtube = new InformationContainerYoutubeVideo(profileManager.Profile.GenerateID());
            ic_youtube.DisplayName = "Youtube videos";
            informationContainers.Add(ic_youtube);
            informationContainersMap.TopPanel.ContainerIDS.Add(ic_youtube.ID);

            // Add this to the bottom
            informationContainersMap.BottomPanel = new InformationContainerTabsPanel();
            informationContainersMap.BottomPanel.TopPanel = new InformationContainerTabsPanel();
            informationContainersMap.BottomPanel.BottomPanel = new InformationContainerTabsPanel();

            InformationContainerReviewScore ic_review = new InformationContainerReviewScore(profileManager.Profile.GenerateID());
            ic_review.DisplayName = "Review/Score";
            informationContainers.Add(ic_review);
            informationContainersMap.BottomPanel.TopPanel.ContainerIDS.Add(ic_review.ID);

            InformationContainerLinks ic_links = new InformationContainerLinks(profileManager.Profile.GenerateID());
            ic_links.DisplayName = "Links";
            informationContainers.Add(ic_links);
            informationContainersMap.BottomPanel.TopPanel.ContainerIDS.Add(ic_links.ID);

            InformationContainerInfoText ic_info = new InformationContainerInfoText(profileManager.Profile.GenerateID());
            ic_info.DisplayName = "Info Text";
            informationContainers.Add(ic_info);
            informationContainersMap.BottomPanel.BottomPanel.ContainerIDS.Add(ic_info.ID);

            InformationContainerInfoText ic_history = new InformationContainerInfoText(profileManager.Profile.GenerateID());
            ic_history.DisplayName = "History";
            informationContainers.Add(ic_history);
            informationContainersMap.BottomPanel.BottomPanel.ContainerIDS.Add(ic_history.ID);

            InformationContainerPDF ic_manuals = new InformationContainerPDF(profileManager.Profile.GenerateID());
            ic_manuals.DisplayName = "Manuals";
            informationContainers.Add(ic_manuals);
            informationContainersMap.BottomPanel.BottomPanel.ContainerIDS.Add(ic_manuals.ID);
        }
        /// <summary>
        /// Get information container element using id
        /// </summary>
        /// <param name="id">Information container element id</param>
        /// <returns></returns>
        public InformationContainer GetInformationContainer(string id)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if (ic.ID == id)
                    return ic;
            }
            return null;
        }
        /// <summary>
        /// Get data info element using id
        /// </summary>
        /// <param name="id">data info  element id</param>
        /// <returns></returns>
        public RomData GetDataInfo(string id)
        {
            foreach (RomData da in romDataInfo)
            {
                if (da.ID == id)
                    return da;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RomData GetDataInfoByName(string name)
        {
            foreach (RomData da in romDataInfo)
            {
                if (da.Name.ToLower() == name.ToLower())
                    return da;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="icid"></param>
        public void DeleteInformationContainer(string icid)
        {
            // Check rom by rom to see if a rom conatin items about this information container.
            ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
            Rom[] roms = profileManager.Profile.Roms[this.id, false];
            foreach (Rom rom in roms)
            {
                rom.DeleteInformationContainerItems(icid);
            }
            // Remove it !!
            for (int i = 0; i < informationContainers.Count; i++)
            {
                if (informationContainers[i].ID == icid)
                {
                    informationContainers.RemoveAt(i); i--;
                }
            }
            // Remove this container from the map !
            informationContainersMap.RemoveID(icid);
            // Raise event
            profileManager.Profile.OnInformationContainerRemoved(this.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="icid"></param>
        /// <returns></returns>
        public int GetInformaitonContainerIndex(string icid)
        {
            for (int i = 0; i < informationContainers.Count; i++)
            {
                if (informationContainers[i].ID == icid)
                {
                    return i;
                }
            }
            return -1;
        }
        public bool ContainRomInfoIC()
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if (ic is InformationContainerRomInfo)
                    return true;
            }
            return false;
        }
        public bool ContainRomDataElement(string name, RomDataType typ)
        {
            foreach (RomData dt in romDataInfo)
            {
                if (dt.Name.ToLower() == name.ToLower() && dt.Type == typ)
                    return true;
            }
            return false;
        }
        public bool ContainsICImages(string display_name)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if ((ic is InformationContainerImage) && ic.DisplayName.ToLower() == display_name.ToLower())
                    return true;
            }
            return false;
        }
        public bool ContainsICInfoText(string display_name)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if ((ic is InformationContainerInfoText) && ic.DisplayName.ToLower() == display_name.ToLower())
                    return true;
            }
            return false;
        }
        public bool ContainsICLinks(string display_name)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if ((ic is InformationContainerLinks) && ic.DisplayName.ToLower() == display_name.ToLower())
                    return true;
            }
            return false;
        }
        public bool ContainsICMedia(string display_name)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if ((ic is InformationContainerMedia) && ic.DisplayName.ToLower() == display_name.ToLower())
                    return true;
            }
            return false;
        }
        public bool ContainsICPDF(string display_name)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if ((ic is InformationContainerPDF) && ic.DisplayName.ToLower() == display_name.ToLower())
                    return true;
            }
            return false;
        }
        public bool ContainsICReviewScore(string display_name)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if ((ic is InformationContainerPDF) && ic.DisplayName.ToLower() == display_name.ToLower())
                    return true;
            }
            return false;
        }
        public bool ContainsICYoutube(string display_name)
        {
            foreach (InformationContainer ic in informationContainers)
            {
                if ((ic is InformationContainerYoutubeVideo) && ic.DisplayName.ToLower() == display_name.ToLower())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Get or set the parent consoles group id.
        /// </summary>
        public string ParentGroupID
        { get { return parentID; } set { parentID = value; } }
        /// <summary>
        /// Get or set if the rom should be extracted first if it's compressed
        /// </summary>
        public bool ExtractRomIfArchive
        { get { return extractRomIfArchive; } set { extractRomIfArchive = value; } }
        public bool ExtractFirstFileIfArchiveIncludeMoreThanOne
        {
            get { return extractFirstFileIfArchiveIncludeMoreThanOne; }
            set { extractFirstFileIfArchiveIncludeMoreThanOne = value; }
        }
        public List<string> ArchiveExtensions
        {
            get { return archiveExtensions; }
            set { archiveExtensions = value; }
        }
        public List<string> ArchiveAllowedExtractionExtensions
        {
            get { return archiveAllowedExtractionExtensions; }
            set { archiveAllowedExtractionExtensions = value; }
        }
        public bool ExtractAllFilesOfArchive
        {
            get { return extractAllFilesOfArchive; }
            set { extractAllFilesOfArchive = value; }
        }
        public string ShortDescription
        {
            get { return shortDescription; }
            set { shortDescription = value; }
        }
        public string RTFPath
        {
            get { return rtfPath; }
            set { rtfPath = value; }
        }
        public string PDFPath
        {
            get { return pdfPath; }
            set { pdfPath = value; }
        }
        /// <summary>
        /// Get links as dictionary in formula [link name, link address]
        /// </summary>
        public Dictionary<string, string> Links
        { get { return links; } set { links = value; } }
        /// <summary>
        /// Get or set if this console should use the parent and children mode.
        /// </summary>
        public bool ParentAndChildrenMode
        {
            get { return parentAndChildrenMode; }
            set { parentAndChildrenMode = value; }
        }
        /// <summary>
        /// Get or set if EO should copy a rom into a specific folder when attempting to run that rom.
        /// </summary>
        public bool CopyRomBeforeLaunch
        {
            get { return copyRomBeforeLaunch; }
            set { copyRomBeforeLaunch = value; }
        }
        /// <summary>
        /// The location where to copy the rom when attempting to run it if CopyRomBeforeLaunch is true
        /// </summary>
        public string FolderWhereToCopyRomWhenLaunch
        {
            get { return folderWhereToCopyRomWhenLaunch; }
            set { folderWhereToCopyRomWhenLaunch = value; }
        }
        public bool UseSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch
        {
            get { return useSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch; }
            set { useSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch = value; }
        }
        /// <summary>
        /// If CopyRomBeforeLaunch is true, rename the rom into specific name before sending it it to the emu
        /// </summary>
        public bool RenameRomBeforeLaunch
        {
            get { return renameRomBeforeLaunch; }
            set { renameRomBeforeLaunch = value; }
        }
        /// <summary>
        /// Get or set the rom naming (script) if RenameRomBeforeLaunch is true.
        /// </summary>
        public string RomNameBeforeLaunch
        {
            get { return romNameBeforeLaunch; }
            set { romNameBeforeLaunch = value; }
        }
        public bool IncludeExtensionWhenRenaming
        {
            get { return includeExtensionWhenrenaming; }
            set { includeExtensionWhenrenaming = value; }
        }
        /// <summary>
        /// Get or set the extensions of this console
        /// </summary>
        public List<string> Extensions
        { get { return extensions; } set { extensions = value; } }
        /// <summary>
        /// Get or set the folders list for memory.
        /// </summary>
        public List<string> Memory_RomFolders
        { get { return memory_RomFolders; } set { memory_RomFolders = value; } }
        /// <summary>
        /// Get or set the information containers collection list for this element
        /// </summary>
        public virtual List<InformationContainer> InformationContainers
        { get { return informationContainers; } set { informationContainers = value; } }
        /// <summary>
        /// Get or set the rom data info container collection
        /// </summary>
        public List<RomData> RomDataInfoElements
        { get { return romDataInfo; } set { romDataInfo = value; } }
        /// <summary>
        /// Get or set the information container tabs map
        /// </summary>
        public InformationContainerTabsPanel InformationContainersMap
        {
            get { return informationContainersMap; }
            set { informationContainersMap = value; }
        }
        public bool UseRomWorkingDirectory
        {
            get { return useRomWorkingDirectory; }
            set { useRomWorkingDirectory = value; }
        }
        /// <summary>
        /// Get or set the latetest selected id of ic that should be used for thumbnails mode.
        /// </summary>
        public string ThumbModeSelectionID
        { get { return thumbModeSelectionID; } set { thumbModeSelectionID = value; } }
        /// <summary>
        /// Get or set if the tabs should switch depending on priority.
        /// </summary>
        public bool AutoSwitchTabPriorityDepend
        { get { return autoSwitchTabPriorityDepend; } set { autoSwitchTabPriorityDepend = value; } }
        /// <summary>
        /// Check if data info exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsDataInfoExist(string name)
        {
            foreach (RomData dat in romDataInfo)
            {
                if (dat.Name.ToLower() == name.ToLower())
                    return true;
            }
            return false;
        }
    }
}
