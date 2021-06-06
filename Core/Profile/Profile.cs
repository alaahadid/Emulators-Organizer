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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Diagnostics;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// The class that holds everything in Emulators Organizer
    /// </summary>
    [Serializable]
    public sealed class Profile
    {
        /// <summary>
        /// The class that holds everything in Emulators Organizer
        /// </summary>
        public Profile()
        {
            consoleGroups = new ConsoleGroupsCollection(this);
            consoles = new ConsolesCollection(this);
            emulators = new EmulatorsCollection(this);
            roms = new RomsCollection(this, true);
            playlists = new PlaylistsCollection(this);
            playlistGroups = new PlaylistGroupsCollection(this);
        }

        private string name = "";
        private long baseID = 0;
        private string ROMPATH;
        private ConsoleGroupsCollection consoleGroups;
        private ConsolesCollection consoles;
        private EmulatorsCollection emulators;
        private RomsCollection roms;
        private PlaylistsCollection playlists;
        private PlaylistGroupsCollection playlistGroups;
        [NonSerialized()]
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        [NonSerialized()]
        private List<string> marked_to_delete = new List<string>();
        [NonSerialized()]
        ProfileManager pManagerService;
        private string selectedConsolesGroupID = "";
        private string selectedConsoleID = "";
        private string selectedPlaylistsGroupID = "";
        private string selectedPlaylistID = "";
        private string selectedEmulatorID = "";
        private bool rememberLatestSelectedConsoleOnProfileOpen = true;
        private bool rememberLatestSelectedRomOnProfileOpen = true;
        private List<string> selectedRomIDS = new List<string>();
        private List<string> activeCategories = new List<string>();
        private List<Filter> selectedFilters = new List<Filter>();
        private SelectionType recenltySelected = SelectionType.None;
        /*Rom launch elements*/
        [NonSerialized()]
        private Process currentProccess;
        [NonSerialized()]
        private Timer timer;
        [NonSerialized()]
        private long lastPlayedRomTime;
        [NonSerialized()]
        private Rom lastPlayedRom;
        [NonSerialized()]
        private bool autoMinimizeEnable = true;
        [NonSerialized()]
        private CommandlinesUsageMode commandlinesUsageMode = CommandlinesUsageMode.Emulator;
        [NonSerialized()]
        private string tempFolder;
        [NonSerialized()]
        private bool isAllFilterMatch;

        #region Properties
        /// <summary>
        /// Get or set the profile name.
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the console groups collection
        /// </summary>
        public ConsoleGroupsCollection ConsoleGroups
        { get { return consoleGroups; } set { consoleGroups = value; } }
        /// <summary>
        /// Get or set the consoles collection
        /// </summary>
        public ConsolesCollection Consoles
        { get { return consoles; } set { consoles = value; } }
        /// <summary>
        /// Get or set the emulators collection
        /// </summary>
        public EmulatorsCollection Emulators
        { get { return emulators; } set { emulators = value; } }
        /// <summary>
        /// Get or set the roms collection
        /// </summary>
        public RomsCollection Roms
        { get { return roms; } set { roms = value; } }
        /// <summary>
        /// Get or set playlists collection.
        /// </summary>
        public PlaylistsCollection Playlists
        { get { return playlists; } set { playlists = value; } }
        /// <summary>
        /// Get or set playlist groups
        /// </summary>
        public PlaylistGroupsCollection PlaylistGroups
        { get { return playlistGroups; } set { playlistGroups = value; } }
        /// <summary>
        /// Get current base id.
        /// </summary>
        public long BaseID
        { get { return baseID; } }
        /// <summary>
        /// Get or set selected consoles group id
        /// </summary>
        public string SelectedConsolesGroupID
        {
            get { return selectedConsolesGroupID; }
            set
            {
                selectedConsolesGroupID = value;
                recenltySelected = SelectionType.ConsolesGroup;
                OnConsolesGroupSelected(consoleGroups[value].Name);
            }
        }
        /// <summary>
        /// Get or set selected consoles group id
        /// </summary>
        public string SelectedConsoleID
        {
            get { return selectedConsoleID; }
            set
            {
                selectedConsoleID = value;
                if (value != "")
                {
                    recenltySelected = SelectionType.Console;
                    OnConsoleSelected(consoles[value].Name);
                }
                else
                {
                    recenltySelected = SelectionType.None;
                    OnConsoleSelected("");
                }
            }
        }
        /// <summary>
        /// Get or set selected consoles group id
        /// </summary>
        public string SelectedPlaylistsGroupID
        {
            get { return selectedPlaylistsGroupID; }
            set
            {
                selectedPlaylistsGroupID = value;
                recenltySelected = SelectionType.PlaylistsGroup;
                OnPlaylistsGroupSelected(playlistGroups[value].Name);
            }
        }
        /// <summary>
        /// Get or set selected consoles group id
        /// </summary>
        public string SelectedPlaylistID
        {
            get { return selectedPlaylistID; }
            set
            {
                selectedPlaylistID = value;
                recenltySelected = SelectionType.Playlist;
                OnPlaylistSelected(playlists[value].Name);
            }
        }
        /// <summary>
        /// Get or set the selected emulator id
        /// </summary>
        public string SelectedEmulatorID
        { get { return selectedEmulatorID; } set { selectedEmulatorID = value; } }
        /// <summary>
        /// Get or set selected rom ids collection
        /// </summary>
        public List<string> SelectedRomIDS
        { get { return selectedRomIDS; } set { selectedRomIDS = value; } }
        /// <summary>
        /// Get or set the recently selected item
        /// </summary>
        public SelectionType RecentSelectedType
        { get { return recenltySelected; } set { recenltySelected = value; } }
        /// <summary>
        /// Get the active categories collection
        /// </summary>
        public List<string> ActiveCategories
        { get { return activeCategories; } set { activeCategories = value; } }
        /// <summary>
        /// Get or set active filters for selected element
        /// </summary>
        public List<Filter> ActiveFilters
        { get { return selectedFilters; } set { selectedFilters = value; } }
        /// <summary>
        /// Get or set if the main window should minimized when the rom launched
        /// </summary>
        public bool AutoMinimizeEnable
        { get { return autoMinimizeEnable; } set { autoMinimizeEnable = value; } }
        /// <summary>
        /// Get or set the commandlines usage mode.
        /// </summary>
        public CommandlinesUsageMode CommandlinesUsage
        { get { return commandlinesUsageMode; } set { commandlinesUsageMode = value; } }
        public List<string> MarkedToBeDeleted
        {
            get
            {
                if (marked_to_delete == null)
                    marked_to_delete = new List<string>();
                return marked_to_delete;
            }
            set
            {
                if (value == null)
                    value = new List<string>();
                marked_to_delete = value;
            }
        }
        /// <summary>
        /// Get or set the temp folder
        /// </summary>
        public string TempFolder
        { get { return tempFolder; } set { tempFolder = value; } }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAllFilterMatch
        { get { return isAllFilterMatch; } set { isAllFilterMatch = value; } }
        public bool RememberLatestSelectedConsoleOnProfileOpen
        {
            get { return rememberLatestSelectedConsoleOnProfileOpen; }
            set { rememberLatestSelectedConsoleOnProfileOpen = value; }
        }
        public bool RememberLatestSelectedRomOnProfileOpen
        {
            get { return rememberLatestSelectedRomOnProfileOpen; }
            set { rememberLatestSelectedRomOnProfileOpen = value; }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            ConoslesGroupAdded = null;
            ConoslesGroupRemoved = null;
            ConoslesGroupsCleared = null;
            ConoslesGroupsRenamed = null;
            ConoslesGroupsMoved = null;
            ConoslesGroupsSorted = null;
            ConsolesGroupPropertiesChanged = null;
            ConsolesGroupColumnVisibleChanged = null;
            ConsolesGroupColumnResized = null;
            ConsolesGroupColumnReorder = null;
            ConsolesGroupSelectionChanged = null;
            ConsoleAdded = null;
            ConsoleRemoved = null;
            ConoslesCleared = null;
            ConosleRenamed = null;
            ConsoleMoved = null;
            ConsolesSorted = null;
            ConsoleColumnResized = null;
            ConsoleColumnReorder = null;
            ConsoleColumnVisibleChanged = null;
            ConsolePropertiesChanged = null;
            ConsoleSelectionChanged = null;
            PlaylistsGroupSelectionChanged = null;
            PlaylistsGroupAdded = null;
            PlaylistsGroupRemoved = null;
            PlaylistGroupsCleared = null;
            PlaylistGroupsRenamed = null;
            PlaylistsGroupMoved = null;
            PlaylistGroupsSorted = null;
            PlaylistsGroupPropertiesChanged = null;
            PlaylistsGroupColumnVisibleChanged = null;
            PlaylistsGroupColumnResized = null;
            PlaylistsGroupColumnReorder = null;
            PlaylistSelectionChanged = null;
            PlaylistAdded = null;
            PlaylistRemoved = null;
            PlaylistsCleared = null;
            PlaylistRenamed = null;
            PlaylistMoved = null;
            PlaylistRomsReorder = null;
            PlaylistsSorted = null;
            PlaylistColumnResized = null;
            PlaylistColumnReorder = null;
            PlaylistColumnVisibleChanged = null;
            PlaylistPropertiesChanged = null;
            RomAdded = null;
            RomShowed = null;
            RomsAdded = null;
            RomRemoved = null;
            RomFinishedPlayed = null;
            RomsRemoved = null;
            RomRenamed = null;
            RomIconsChanged = null;
            RomsCleared = null;
            RomMoved = null;
            RomPropertiesChanged = null;
            RomMultiplePropertiesChanged = null;
            RomsSorted = null;
            RomRatingChanged = null;
            RomsRefreshRequest = null;
            RomsAddedToPlaylist = null;
            RomsRemovedFromPlaylist = null;
            RomSelectionChanged = null;
            EmulatorAdded = null;
            EmulatorRemoved = null;
            EmulatorRenamed = null;
            EmulatorsCleared = null;
            EmulatorMoved = null;
            EmulatorsSorted = null;
            EmulatorPropertiesChanged = null;
            EmulatorsRefreshRequest = null;
            EmulatorSelectionChanged = null;
            ElementIconChanged = null;
            MainWindowMinimize = null;
            MainWindowReturnToNormal = null;
            RequestCategoriesListClear = null;
            RequestSearch = null;
            FilterAdded = null;
            FilterRemoved = null;
            FilterEdit = null;
            InformationContainerAdded = null;
            InformationContainerRemoved = null;
            InformationContainerMoved = null;
            InformationContainerVisibiltyChanged = null;
            InformationContainerItemsDetected = null;
            InformationContainerItemsModified = null;
            RefreshPriorityRequest = null;
            ProfileCleanUpFinished = null;
            ExtensionChange = null;
            BeforeRomLaunch = null;
            DatabaseImported = null;
            GamePlayStart = null;
        }
        /// <summary>
        /// Generate an id for object. ID incremented first.
        /// </summary>
        /// <returns>The id generated by clinic base</returns>
        public string GenerateID()
        {
            baseID++;
            return baseID.ToString("X8");
        }
        /// <summary>
        /// Add new consoles group
        /// </summary>
        public void AddConsolesGroup()
        {
            ConsolesGroup group = new ConsolesGroup(GenerateID());
            int i = 1;
            string name = ls["Name_ConsolesGroup"] + "1";
            while (ConsoleGroups.Contains(name))
            {
                i++;
                name = ls["Name_ConsolesGroup"] + i;
            }
            group.Name = name;
            ConsoleGroups.Add(group);
            Trace.WriteLine("Consoles group added: " + name, "Profile");
        }
        /// <summary>
        /// Rename cosnoles group
        /// </summary>
        /// <param name="id">The consoles group to rename</param>
        /// <param name="newName">The new name to use</param>
        public void RenameConsolesGroup(string id, string newName)
        {
            consoleGroups[id].Name = newName;
            if (ConoslesGroupsRenamed != null)
                ConoslesGroupsRenamed(this, new EventArgs());
            Trace.WriteLine("Consoles group renamed: ID=" + id + ", new name=" + newName, "Profile");
        }
        /// <summary>
        /// Add new console to the consoles collection
        /// </summary>
        /// <param name="parentGroupID">The parent consoles group id. Set to "" to disable group</param>
        /// <returns>Console id</returns>
        public string AddConsole(string parentGroupID)
        {
            Console console = new Console(GenerateID(), parentGroupID);
            int i = 1;
            string name = ls["Name_Console"] + "1";
            while (consoles.Contains(name, parentGroupID, ""))
            {
                i++;
                name = ls["Name_Console"] + i;
            }
            console.Name = name;
            consoles.Add(console);
            Trace.WriteLine("Console added: " + name, "Profile");

            return console.ID;
        }
        /// <summary>
        /// Rename cosnole
        /// </summary>
        /// <param name="id">The console to rename</param>
        /// <param name="newName">The new name to use</param>
        public void RenameConsole(string id, string newName)
        {
            consoles[id].Name = newName;
            if (ConosleRenamed != null)
                ConosleRenamed(this, new EventArgs());
            Trace.WriteLine("Console renamed: ID=" + id + ", new name=" + newName, "Profile");
        }
        public void AddPlaylistsGroup()
        {
            PlaylistsGroup group = new PlaylistsGroup(GenerateID());
            int i = 1;
            string name = ls["Name_PlaylistsGroup"] + "1";
            while (playlistGroups.Contains(name))
            {
                i++;
                name = ls["Name_PlaylistsGroup"] + i;
            }
            group.Name = name;
            playlistGroups.Add(group);
            Trace.WriteLine("Playlists group added: " + name, "Profile");
        }
        /// <summary>
        /// Rename playlists group
        /// </summary>
        /// <param name="id">The playlists group to rename</param>
        /// <param name="newName">The new name to use</param>
        public void RenamePlaylistsGroup(string id, string newName)
        {
            playlistGroups[id].Name = newName;
            if (PlaylistGroupsRenamed != null)
                PlaylistGroupsRenamed(this, new EventArgs());
            Trace.WriteLine("Playlists group renamed: ID=" + id + ", new name=" + newName, "Profile");
        }
        /// <summary>
        /// Add new playlist to the consoles collection
        /// </summary>
        /// <param name="parentGroupID">The parent playlists group id. Set to "" to disable group</param>
        public void AddPlaylist(string parentGroupID)
        {
            Playlist playlist = new Playlist(GenerateID(), parentGroupID);
            int i = 1;
            string name = ls["Name_Playlist"] + "1";
            while (playlists.Contains(name, parentGroupID, ""))
            {
                i++;
                name = ls["Name_Playlist"] + i;
            }
            playlist.Name = name;
            playlists.Add(playlist);
            Trace.WriteLine("Playlist added: " + name, "Profile");
        }
        /// <summary>
        /// Rename playlist
        /// </summary>
        /// <param name="id">The playlist to rename</param>
        /// <param name="newName">The new name to use</param>
        public void RenamePlaylist(string id, string newName)
        {
            playlists[id].Name = newName;
            if (PlaylistRenamed != null)
                PlaylistRenamed(this, new EventArgs());
            Trace.WriteLine("Playlist renamed: ID=" + id + ", new name=" + newName, "Profile");
        }
        /// <summary>
        /// Assign playlist to roms
        /// </summary>
        /// <param name="roms">The roms to assign playlist for</param>
        /// <param name="playlistID">The playlist id to assign</param>
        public void AddRomsToPlaylist(Rom[] roms, string playlistID)
        {
            Playlist pl = playlists[playlistID];
            if (pl != null)
            {
                foreach (Rom rom in roms)
                {
                    if (!pl.RomIDS.Contains(rom.ID))
                    {
                        pl.RomIDS.Add(rom.ID);
                    }
                }
                OnRomsAddedToPlaylist(pl.Name, roms.Length);
            }
        }
        /// <summary>
        /// Assign playlist to roms
        /// </summary>
        /// <param name="romIDS">The rom ids to assign playlist for</param>
        /// <param name="playlistID">The playlist id to assign</param>
        public void AddRomsToPlaylist(string[] romIDS, string playlistID)
        {
            Playlist pl = playlists[playlistID];
            if (pl != null)
            {
                foreach (string romID in romIDS)
                {
                    if (!pl.RomIDS.Contains(romID))
                    {
                        pl.RomIDS.Add(romID);
                    }
                }
                OnRomsAddedToPlaylist(pl.Name, romIDS.Length);
            }
        }
        /// <summary>
        /// Remove rom(s) from playlist
        /// </summary>
        /// <param name="roms">The roms to remove</param>
        /// <param name="playlistID">The playlist id</param>
        public void RemoveRomsFromPlaylist(Rom[] roms, string playlistID)
        {
            Playlist pl = playlists[playlistID];
            if (pl != null)
            {
                foreach (Rom rom in roms)
                {
                    if (pl.RomIDS.Contains(rom.ID))
                    {
                        pl.RomIDS.Remove(rom.ID);
                    }
                }
                OnRomsRemovedFromPlaylist(pl.Name, roms.Length);
            }
        }
        /// <summary>
        /// Play selected rom using selected emulator.
        /// </summary>
        public void PlayRom()
        {
            if (pManagerService == null)
                pManagerService = (ProfileManager)ServicesManager.GetService("Profile Manager");
            // Check for errors ...
            Trace.WriteLine("Doing before launch errors check..", "Profile");
            if (currentProccess != null)
            {
                Trace.TraceWarning("The process is still running !!");
                //throw new Exception("The process is still running !!");
                // Trace.WriteLine("Terminating old process ...");
                // try
                // {
                //     currentProccess.Kill();
                // }
                // catch(Exception ex) { Trace.TraceWarning(ex.Message); }
                currentProccess = null;
                //Trace.WriteLine("Process terminated successfully.");
            }
            if (selectedRomIDS.Count != 1)
            {
                Trace.TraceError("Only one rom can be launched at a time.");
                throw new Exception("Only one rom can be launched at a time.");
            }
            Trace.WriteLine("Getting rom path..", "Profile");

            lastPlayedRom = roms[selectedRomIDS[0]];
            string ROM_PARENTCONSOLE = lastPlayedRom.ParentConsoleID;
            bool ShowParentAndChildrenChoosingDialog =
                 consoles[lastPlayedRom.ParentConsoleID].ParentAndChildrenMode &&
                 lastPlayedRom.AlwaysChooseChildWhenPlay &&
                 lastPlayedRom.IsParentRom;

            #region Child Choosing
            if (ShowParentAndChildrenChoosingDialog)
            {
                Trace.TraceWarning("User picking up a child for this parent rom !!");
                FormChildPick frm = new FormChildPick(this.Roms.GetChildrenOf(lastPlayedRom.ID), false);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Get the selected rom ...
                    lastPlayedRom = roms[frm.SelectedRomID];
                    ROM_PARENTCONSOLE = lastPlayedRom.ParentConsoleID;
                    Trace.TraceWarning("A child rom picked up [" + frm.SelectedRomID + "]");
                }
                else
                {
                    if (!frm.PlayParentInstead)
                    {
                        Trace.TraceWarning("Rom play canceled by user, no child selected.");
                        return;
                    }
                    else
                    {
                        Trace.WriteLine("User changed his/her mind and want to play the parent rom this time.", "Profile");
                    }
                }
            }
            #endregion

            ROMPATH = HelperTools.GetFullPath(lastPlayedRom.Path);
            if (!lastPlayedRom.IgnorePathNotExist)
                if (!File.Exists(HelperTools.GetPathFromAIPath(ROMPATH)))
                {
                    Trace.TraceError("Rom file is not exist at " + ROMPATH);
                    throw new Exception("Rom file is not exist at " + ROMPATH);
                }
            // Load settings
            Trace.WriteLine("Loading settings...", "Profile");
            SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");

            autoMinimizeEnable = (bool)settings.GetValue(DefaultProfileSettings.AutoMinimize_Key,
                true, DefaultProfileSettings.AutoMinimize);
            tempFolder = HelperTools.GetFullPath((string)settings.GetValue(DefaultProfileSettings.TempFolder_Key,
                true, DefaultProfileSettings.TempFolder));

            // Raise event
            if (BeforeRomLaunch != null)
                BeforeRomLaunch(this, new EventArgs());

            Trace.WriteLine("Settings loaded successfully", "Profile");
            #region Launch condition
            Trace.WriteLine("Determining launch conditions ..", "Profile");
            bool EMUENABLED = false;
            bool COMMANDLINESENABLED = false;
            bool ExtractRomIfArchive = consoles[lastPlayedRom.ParentConsoleID].ExtractRomIfArchive;

            switch (recenltySelected)
            {
                case SelectionType.Console:
                    {
                        EMUENABLED = consoles[selectedConsoleID].EnableEmulator;
                        COMMANDLINESENABLED = consoles[selectedConsoleID].EnableCommandlines;
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        EMUENABLED = consoleGroups[selectedConsolesGroupID].EnableEmulator;
                        COMMANDLINESENABLED = consoleGroups[selectedConsolesGroupID].EnableCommandlines;
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        EMUENABLED = playlists[selectedPlaylistID].EnableEmulator;
                        COMMANDLINESENABLED = playlists[selectedPlaylistID].EnableCommandlines;
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        EMUENABLED = playlistGroups[selectedPlaylistsGroupID].EnableEmulator;
                        COMMANDLINESENABLED = playlistGroups[selectedPlaylistsGroupID].EnableCommandlines;
                        break;
                    }
            }
            if (EMUENABLED && selectedEmulatorID == "")
            {
                Trace.TraceError("No emulator selected !");
                throw new Exception("No emulator selected ! please select an emulator first");
            }
            #endregion

            #region Rom is archive ?
            if (HelperTools.IsAIPath(ROMPATH))
            {
                // If the rom path is AI path, no need to use Archive checks.
                bool extractAll = Consoles[ROM_PARENTCONSOLE].ExtractAllFilesOfArchive;
                Trace.WriteLine("Rom is archive. Extracting ...", "Profile");
                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }
                // This is archive. Do the archive open seq.
                Form_ExtractArchive frm = new Form_ExtractArchive(ROMPATH, tempFolder, extractAll);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ROMPATH = frm.OutFilePath;
                }
                else
                {
                    Trace.WriteLine("Archive extraction canceled by user.", "Profile");
                    return;
                }
            }
            else if (ExtractRomIfArchive)
            {
                if (Consoles[ROM_PARENTCONSOLE].ArchiveExtensions == null)
                    Consoles[ROM_PARENTCONSOLE].ArchiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
                if (Consoles[ROM_PARENTCONSOLE].ArchiveAllowedExtractionExtensions == null)
                    Consoles[ROM_PARENTCONSOLE].ArchiveAllowedExtractionExtensions = new List<string>();
                List<string> archiveExtensions = Consoles[ROM_PARENTCONSOLE].ArchiveExtensions;
                List<string> archiveAllowedExtensions = Consoles[ROM_PARENTCONSOLE].ArchiveAllowedExtractionExtensions;
                bool extractFirstOne = Consoles[ROM_PARENTCONSOLE].ExtractFirstFileIfArchiveIncludeMoreThanOne;
                bool extractAll = Consoles[ROM_PARENTCONSOLE].ExtractAllFilesOfArchive;

                if (archiveExtensions.Contains(Path.GetExtension(ROMPATH).ToLower()))
                {
                    Trace.WriteLine("Rom is archive. Extracting ...", "Profile");
                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }
                    // This is archive. Do the archive open seq.
                    Form_ExtractArchive frm = new Form_ExtractArchive(ROMPATH, tempFolder, archiveAllowedExtensions.ToArray(),
                        extractFirstOne, extractAll);
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ROMPATH = frm.OutFilePath;
                    }
                    else
                    {
                        Trace.WriteLine("Archive extraction canceled by user.", "Profile");
                        return;
                    }
                }
            }
            #endregion

            #region Copying and renaming
            bool copyRomFirst = consoles[lastPlayedRom.ParentConsoleID].CopyRomBeforeLaunch;
            string copyRomFolder = consoles[lastPlayedRom.ParentConsoleID].FolderWhereToCopyRomWhenLaunch;
            bool renameRomWhenCopy = consoles[lastPlayedRom.ParentConsoleID].RenameRomBeforeLaunch;
            string romNameWhenCopy = consoles[lastPlayedRom.ParentConsoleID].RomNameBeforeLaunch;
            bool useEmufolderIfAvailable = consoles[lastPlayedRom.ParentConsoleID].UseSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch;
            bool includeEx = consoles[lastPlayedRom.ParentConsoleID].IncludeExtensionWhenRenaming;
            if (copyRomFirst)
            {
                Trace.WriteLine("Rom should be copied to another folder first ...", "Profile");
                // Set the folder
                if (useEmufolderIfAvailable)
                {
                    if (selectedEmulatorID != "")
                    {
                        Emulator emu = emulators[selectedEmulatorID];
                        if (emu != null)
                        {
                            if (emu.ExcutablePath != "")
                            {
                                try
                                {
                                    string folder = Path.GetDirectoryName(emu.ExcutablePath);
                                    if (folder == "")
                                        folder = Path.GetPathRoot(emu.ExcutablePath);
                                    if (folder != "")
                                    {
                                        copyRomFolder = folder;

                                        Trace.WriteLine("The folder to copy into set to '" + folder + "', the selected emu folder.", "Profile");
                                    }
                                }
                                catch
                                { }
                            }
                        }
                    }
                }
                if (Directory.Exists(copyRomFolder))
                {
                    string destination = Path.Combine(HelperTools.GetFullPath(copyRomFolder), Path.GetFileName(ROMPATH));
                    if (renameRomWhenCopy)
                    {
                        string romNamingName = romNameWhenCopy.Replace("<romname>", lastPlayedRom.Name);
                        romNamingName = romNamingName.Replace("<romnamewithoutextension>", Path.GetFileNameWithoutExtension(ROMPATH));
                        if (includeEx)
                            romNamingName += Path.GetExtension(ROMPATH);
                        destination = Path.Combine(HelperTools.GetFullPath(copyRomFolder), romNamingName);

                        Trace.WriteLine("Rom destination file renamed to '" + romNamingName + "'", "Profile");
                    }

                    try
                    {
                        if (File.Exists(destination))
                            File.Delete(destination);
                        File.Copy(ROMPATH, destination);
                        Trace.WriteLine("Rom file copied to '" + destination + "'", "Profile");
                        ROMPATH = destination;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceWarning("Can't copy the rom into '" + destination + "' , " + ex.Message);
                    }
                }
                else
                {
                    Trace.TraceWarning("Folder is not exist at '" + copyRomFolder + "', can't copy the rom.");
                }
            }
            #endregion
            Trace.WriteLine("Emulator enabled = " + EMUENABLED, "Profile");
            Trace.WriteLine("Commandlines enabled = " + COMMANDLINESENABLED, "Profile");
            if (EMUENABLED)
            {
                if (!emulators[selectedEmulatorID].BatMode)
                {
                    Trace.WriteLine("Launching emulator...", "Profile");
                    // GET EMU PATH
                    string EMUPATH = HelperTools.GetFullPath(emulators[selectedEmulatorID].ExcutablePath);
                    if (!File.Exists(EMUPATH))
                    {
                        Trace.TraceError("Emulator executable file can't be found at " + EMUPATH);
                        throw new Exception("Emulator executable file can't be found at " + EMUPATH);
                    }
                    // COMMANDLINES
                    string COMMANDLINES = "\"" + ROMPATH + "\"";
                    if (COMMANDLINESENABLED)
                    {
                        Trace.WriteLine("Building commandlines ...", "Profile");
                        List<CommandlinesGroup> commandlineGroups = null;
                        switch (commandlinesUsageMode)
                        {
                            case CommandlinesUsageMode.Emulator:
                                {
                                    commandlineGroups = emulators[selectedEmulatorID].GetCommandlinesGroupsForConsole(ROM_PARENTCONSOLE);
                                    if (commandlineGroups == null)
                                    {
                                        Trace.TraceError("Emulator commandlines are enabled but this emulator has no commandline to use !");
                                        Trace.TraceWarning("The commandlines set to default (rom path)");
                                    }
                                    else if (commandlineGroups.Count == 0)
                                    {
                                        Trace.TraceError("Emulator commandlines are enabled but this emulator has no commandline to use !");
                                        Trace.TraceWarning("The commandlines set to default (rom path)");
                                        //throw new Exception("Emulator commandlines are enabled but this emulator has no commandline to use !");
                                    }
                                    else
                                    {
                                        COMMANDLINES = CommandlinesEncoder.ToCommandlinesString(commandlineGroups.ToArray(),
                                            ROMPATH, new List<string>());
                                    }
                                    break;
                                }
                            case CommandlinesUsageMode.Rom:
                                {
                                    commandlineGroups = roms[selectedRomIDS[0]].GetCommandlinesGroupsForEmulator(selectedEmulatorID);
                                    if (commandlineGroups == null)
                                    {
                                        Trace.TraceError("Rom commandlines are enabled but this rom has no commandline to use !");
                                        Trace.TraceWarning("The commandlines set to default (rom path)");
                                    }
                                    else if (commandlineGroups.Count == 0)
                                    {
                                        Trace.TraceError("Rom commandlines are enabled but this rom has no commandline to use !");
                                        Trace.TraceWarning("The commandlines set to default (rom path)");
                                        //throw new Exception("Emulator commandlines are enabled but this emulator has no commandline to use !");
                                    }
                                    else
                                    {
                                        COMMANDLINES = CommandlinesEncoder.ToCommandlinesString(commandlineGroups.ToArray(),
                                            ROMPATH, new List<string>());
                                    }
                                    break;
                                }
                        }
                    }
                    // LAUNCH !
                    #region LAUNCH BEFORE
                    Trace.WriteLine("Running programs before emulator process...", "Profile");
                    List<ProgramProperties> progBefore = emulators[selectedEmulatorID].ProgramsToLaucnhBefore;
                    if (lastPlayedRom.ProgramsUsageMode == ProgramUsageMode.Rom)
                        progBefore = lastPlayedRom.ProgramsToLaucnhBefore;
                    if (progBefore != null)
                    {
                        foreach (ProgramProperties prog in progBefore)
                        {
                            if (prog.BatMode)
                            {
                                string progBatpath = Path.Combine(tempFolder, "progbat.bat");

                                string[] progscriptLines = prog.BatScript.Split(new string[] { "\n" }, StringSplitOptions.None);
                                List<string> proglinesToWrite = new List<string>();
                                for (int i = 0; i < progscriptLines.Length; i++)
                                {
                                    proglinesToWrite.Add(CommandlinesEncoder.DecodeCommand(progscriptLines[i], ROMPATH, new List<string>()));
                                }
                                File.WriteAllLines(progBatpath, proglinesToWrite.ToArray());

                                Process progProcess = new Process();
                                progProcess.StartInfo.WorkingDirectory = tempFolder;
                                progProcess.StartInfo.FileName = progBatpath;
                                progProcess.Start();

                                switch (prog.StartMode)
                                {
                                    case ProgramStartMode.INSTANT: break;// Do nothing, just launch the program.
                                    case ProgramStartMode.WAIT_SECONDS:
                                        {
                                            // WAIT THE SECONDS !!
                                            Thread.Sleep(prog.WaitSeconds * 1000);
                                            break;
                                        }
                                    case ProgramStartMode.WAIT_TO_FINISH:
                                        {
                                            // While the program is alive, EO will wait.
                                            progProcess.WaitForExit();
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                Process progProcess = new Process();
                                string progFolder = Path.GetDirectoryName(prog.ProgramPath);
                                if (progFolder == "")
                                    progFolder = Path.GetPathRoot(progFolder);
                                progFolder += "\\";
                                progProcess.StartInfo.WorkingDirectory = progFolder;
                                progProcess.StartInfo.FileName = prog.ProgramPath;
                                progProcess.StartInfo.Arguments =
                                    CommandlinesEncoder.DecodeCommand(prog.Arguments, ROMPATH, new List<string>());
                                progProcess.Start();

                                switch (prog.StartMode)
                                {
                                    case ProgramStartMode.INSTANT: break;// Do nothing, just launch the program.
                                    case ProgramStartMode.WAIT_SECONDS:
                                        {
                                            // WAIT THE SECONDS !!
                                            Thread.Sleep(prog.WaitSeconds * 1000);
                                            break;
                                        }
                                    case ProgramStartMode.WAIT_TO_FINISH:
                                        {
                                            // While the program is alive, EO will wait.
                                            progProcess.WaitForExit();
                                            break;
                                        }
                                }
                            }
                        }
                    }
                    #endregion
                    Trace.WriteLine("Initializing emulator process...", "Profile");
                    currentProccess = new Process();
                    currentProccess.StartInfo.FileName = EMUPATH;
                    EmulatorsOrganizer.Core.Console parConsole = consoles[ROM_PARENTCONSOLE];
                    Trace.WriteLine("Making working directories ...", "Profile");
                    if (parConsole.UseRomWorkingDirectory)
                    {
                        string romFolder = HelperTools.GetDirectory(ROMPATH);
                        Trace.WriteLine("USING ROM WORKING DIRECTORY [" + romFolder + "]");
                        currentProccess.StartInfo.WorkingDirectory = romFolder;
                    }
                    else
                    {
                        string emuFolder = HelperTools.GetDirectory(EMUPATH);
                        Trace.WriteLine("USING EMU WORKING DIRECTORY [" + emuFolder + "]");
                        currentProccess.StartInfo.WorkingDirectory = emuFolder;
                    }
                    currentProccess.StartInfo.Arguments = COMMANDLINES;
                    currentProccess.Start();
                    Trace.WriteLine("Emulator process is running !", "Profile");
                    // Run the process watcher
                    Trace.WriteLine("Initializing process watcher...", "Profile");
                    if (autoMinimizeEnable)
                        OnMainWindowMinimize();
                    AutoResetEvent autoEvent = new AutoResetEvent(false);
                    lastPlayedRomTime = 0;
                    timer = new Timer(TimerTick, autoEvent, 0, 100);
                    Trace.WriteLine("Process watcher is running !", "Profile");
                    if (GamePlayStart != null)
                        GamePlayStart(this, new EventArgs());
                }
                else// BAT MODE !!
                {
                    // In bat mode we have no path or command lines ...
                    Trace.WriteLine("Launching emulator...", "Profile");

                    // LAUNCH !
                    #region LAUNCH BEFORE
                    Trace.WriteLine("Running programs before emulator process...", "Profile");
                    List<ProgramProperties> progBefore = emulators[selectedEmulatorID].ProgramsToLaucnhBefore;
                    if (lastPlayedRom.ProgramsUsageMode == ProgramUsageMode.Rom)
                        progBefore = lastPlayedRom.ProgramsToLaucnhBefore;
                    if (progBefore != null)
                    {
                        foreach (ProgramProperties prog in progBefore)
                        {
                            if (prog.BatMode)
                            {
                                string progBatpath = Path.Combine(tempFolder, "progbat.bat");

                                string[] progscriptLines = prog.BatScript.Split(new string[] { "\n" }, StringSplitOptions.None);
                                List<string> proglinesToWrite = new List<string>();
                                for (int i = 0; i < progscriptLines.Length; i++)
                                {
                                    proglinesToWrite.Add(CommandlinesEncoder.DecodeCommand(progscriptLines[i], ROMPATH, new List<string>()));
                                }
                                File.WriteAllLines(progBatpath, proglinesToWrite.ToArray());

                                Process progProcess = new Process();
                                progProcess.StartInfo.WorkingDirectory = tempFolder;
                                progProcess.StartInfo.FileName = progBatpath;
                                progProcess.Start();

                                switch (prog.StartMode)
                                {
                                    case ProgramStartMode.INSTANT: break;// Do nothing, just launch the program.
                                    case ProgramStartMode.WAIT_SECONDS:
                                        {
                                            // WAIT THE SECONDS !!
                                            Thread.Sleep(prog.WaitSeconds * 1000);
                                            break;
                                        }
                                    case ProgramStartMode.WAIT_TO_FINISH:
                                        {
                                            // While the program is alive, EO will wait.
                                            progProcess.WaitForExit();
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                Process progProcess = new Process();
                                string progFolder = Path.GetDirectoryName(prog.ProgramPath);
                                if (progFolder == "")
                                    progFolder = Path.GetPathRoot(progFolder);
                                progFolder += "\\";
                                progProcess.StartInfo.WorkingDirectory = progFolder;
                                progProcess.StartInfo.FileName = prog.ProgramPath;
                                progProcess.StartInfo.Arguments =
                                    CommandlinesEncoder.DecodeCommand(prog.Arguments, ROMPATH, new List<string>());
                                progProcess.Start();

                                switch (prog.StartMode)
                                {
                                    case ProgramStartMode.INSTANT: break;// Do nothing, just launch the program.
                                    case ProgramStartMode.WAIT_SECONDS:
                                        {
                                            // WAIT THE SECONDS !!
                                            Thread.Sleep(prog.WaitSeconds * 1000);
                                            break;
                                        }
                                    case ProgramStartMode.WAIT_TO_FINISH:
                                        {
                                            // While the program is alive, EO will wait.
                                            progProcess.WaitForExit();
                                            break;
                                        }
                                }
                            }
                        }
                    }
                    #endregion
                    Trace.WriteLine("Initializing emulator process...", "Profile");

                    Trace.WriteLine("Creating emu bat file ...", "Profile");
                    string batBath = Path.Combine(tempFolder, "batemu.bat");

                    string[] scriptLines = emulators[selectedEmulatorID].BatScript.Split(new string[] { "\n" }, StringSplitOptions.None);
                    List<string> linesToWrite = new List<string>();
                    for (int i = 0; i < scriptLines.Length; i++)
                    {
                        linesToWrite.Add(CommandlinesEncoder.DecodeCommand(scriptLines[i], ROMPATH, new List<string>()));
                    }
                    File.WriteAllLines(batBath, linesToWrite.ToArray());

                    currentProccess = new Process();
                    currentProccess.StartInfo.FileName = batBath;
                    EmulatorsOrganizer.Core.Console parConsole = consoles[ROM_PARENTCONSOLE];
                    Trace.WriteLine("Making working directories ...", "Profile");
                    if (parConsole.UseRomWorkingDirectory)
                    {
                        string romFolder = HelperTools.GetDirectory(ROMPATH);
                        Trace.WriteLine("USING ROM WORKING DIRECTORY [" + romFolder + "]");
                        currentProccess.StartInfo.WorkingDirectory = romFolder;
                    }
                    else
                    {
                        Trace.WriteLine("USING EMU WORKING DIRECTORY [" + tempFolder + "]");
                        currentProccess.StartInfo.WorkingDirectory = tempFolder;
                    }
                    currentProccess.Start();
                    Trace.WriteLine("Emulator process is running !", "Profile");
                    // Run the process watcher
                    Trace.WriteLine("Initializing process watcher...", "Profile");
                    if (autoMinimizeEnable)
                        OnMainWindowMinimize();
                    AutoResetEvent autoEvent = new AutoResetEvent(false);
                    lastPlayedRomTime = 0;
                    timer = new Timer(TimerTick, autoEvent, 0, 100);
                    Trace.WriteLine("Process watcher is running !", "Profile");
                    if (GamePlayStart != null)
                        GamePlayStart(this, new EventArgs());
                }
            }
            else// No emulator enabled, launch the rom normally.
            {
                Trace.WriteLine("Launching rom without emulator ...", "Profile");
                Trace.WriteLine("Initializing rom program process...", "Profile");
                currentProccess = new Process();
                currentProccess.StartInfo.FileName = ROMPATH;
                currentProccess.StartInfo.WorkingDirectory = HelperTools.GetDirectory(ROMPATH);
                // Let's see if this rom uses command-lines ...
                List<CommandlinesGroup> commandlineGroups = roms[selectedRomIDS[0]].CommandlineGroupsWhenExecutingWithoutEmulator;
                if (commandlineGroups == null)
                {
                    Trace.TraceError("This rom have no commandlines.");
                }
                else if (commandlineGroups.Count == 0)
                {
                    Trace.TraceError("This rom have no commandlines.");
                }
                else
                {
                    Trace.TraceError("This rom have commandlines !!");
                    currentProccess.StartInfo.Arguments = CommandlinesEncoder.ToCommandlinesString(commandlineGroups.ToArray(),
                        ROMPATH, new List<string>());
                }

                currentProccess.Start();
                Trace.WriteLine("Rom process is running !", "Profile");
                // Run the process watcher
                Trace.WriteLine("Initializing process watcher...", "Profile");
                if (autoMinimizeEnable)
                    OnMainWindowMinimize();
                AutoResetEvent autoEvent = new AutoResetEvent(false);
                lastPlayedRomTime = 0;
                timer = new Timer(TimerTick, autoEvent, 0, 100);
                Trace.WriteLine("Process watcher is running !", "Profile");
                if (GamePlayStart != null)
                    GamePlayStart(this, new EventArgs());
            }
        }
        private void TimerTick(object state)
        {
            lastPlayedRomTime += 100;
            AutoResetEvent autoEvent = (AutoResetEvent)state;
            if (pManagerService.IsSaving)
                return;
            if (currentProccess != null)
            {
                if (!currentProccess.HasExited)
                {
                    return;
                }
            }
            // Timer thread is not the same of the EO thread so culture info may changes
            // in the timer call back ...
            // We need to reset language selection !
            SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
            ls.Language = (string)settings.GetValue("Language", true, "English (United States)");

            timer.Dispose();
            autoEvent.Close();
            currentProccess = null;
            Trace.WriteLine("The process watcher determined that emulator process has existed.", "Profile");
            Trace.WriteLine("Stopping watcher ...", "Profile");
            Trace.WriteLine("Complete. Returning the main window to old state.", "Profile");
            if (autoMinimizeEnable)
                OnMainWindowReturnToNormal();
            // Update rom info

            lastPlayedRom.LastPlayed = DateTime.Now;
            lastPlayedRom.PlayedTimes++;
            lastPlayedRom.PlayedTimeLength += lastPlayedRomTime;
            OnRomFinishedPlayed(lastPlayedRom.Name, lastPlayedRom.ID);
            if (selectedEmulatorID != "")
            {
                List<ProgramProperties> progAfter = emulators[selectedEmulatorID].ProgramsToLaucnhAfter;
                if (lastPlayedRom.ProgramsUsageMode == ProgramUsageMode.Rom)
                    progAfter = lastPlayedRom.ProgramsToLaucnhAfter;
                if (progAfter != null)
                {
                    foreach (ProgramProperties prog in progAfter)
                    {
                        if (prog.BatMode)
                        {
                            switch (prog.StartMode)
                            {
                                case ProgramStartMode.INSTANT: break;// Do nothing, just launch the program.
                                case ProgramStartMode.WAIT_SECONDS:
                                    {
                                        // WAIT THE SECONDS !!
                                        Thread.Sleep(prog.WaitSeconds * 1000);
                                        break;
                                    }
                            }

                            string progBatpath = Path.Combine(tempFolder, "progbat.bat");

                            string[] progscriptLines = prog.BatScript.Split(new string[] { "\n" }, StringSplitOptions.None);
                            List<string> proglinesToWrite = new List<string>();
                            for (int i = 0; i < progscriptLines.Length; i++)
                            {
                                proglinesToWrite.Add(CommandlinesEncoder.DecodeCommand(progscriptLines[i], ROMPATH, new List<string>()));
                            }
                            File.WriteAllLines(progBatpath, proglinesToWrite.ToArray());

                            Process progProcess = new Process();
                            progProcess.StartInfo.WorkingDirectory = tempFolder;
                            progProcess.StartInfo.FileName = progBatpath;
                            progProcess.Start();
                            switch (prog.StartMode)
                            {
                                case ProgramStartMode.WAIT_TO_FINISH:
                                    {
                                        // While the program is alive, EO will wait.
                                        progProcess.WaitForExit();
                                        break;
                                    }
                            }

                        }
                        else
                        {
                            switch (prog.StartMode)
                            {
                                case ProgramStartMode.INSTANT: break;// Do nothing, just launch the program.
                                case ProgramStartMode.WAIT_SECONDS:
                                    {
                                        // WAIT THE SECONDS !!
                                        Thread.Sleep(prog.WaitSeconds * 1000);
                                        break;
                                    }
                            }
                            Process progProcess = new Process();
                            string progFolder = Path.GetDirectoryName(prog.ProgramPath);
                            if (progFolder == "")
                                progFolder = Path.GetPathRoot(progFolder);
                            progFolder += "\\";
                            progProcess.StartInfo.WorkingDirectory = progFolder;
                            progProcess.StartInfo.FileName = prog.ProgramPath;
                            progProcess.StartInfo.Arguments =
                                CommandlinesEncoder.DecodeCommand(prog.Arguments, ROMPATH, new List<string>());
                            progProcess.Start();
                            switch (prog.StartMode)
                            {
                                case ProgramStartMode.WAIT_TO_FINISH:
                                    {
                                        // While the program is alive, EO will wait.
                                        progProcess.WaitForExit();
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region  Events
        #region Consoles Group Events
        /// <summary>
        /// Raised when new consoles group added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConoslesGroupAdded;
        /// <summary>
        /// Raised when new consoles group removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConoslesGroupRemoved;
        /// <summary>
        /// Raised when new console groups collection cleared
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConoslesGroupsCleared;
        /// <summary>
        /// Raised when new consoles group get renamed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConoslesGroupsRenamed;
        /// <summary>
        /// Raised when new consoles group get moved
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConoslesGroupsMoved;
        /// <summary>
        /// Raised when new console groups collection sorted
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConoslesGroupsSorted;
        /// <summary>
        /// Raised when consoles group properties get changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsolesGroupPropertiesChanged;
        /// <summary>
        /// Raised when a column visible changed of consoles group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsolesGroupColumnVisibleChanged;
        /// <summary>
        /// Raised when a column resized of consoles group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsolesGroupColumnResized;
        /// <summary>
        /// Raised when a column reorder of consoles group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsolesGroupColumnReorder;
        /// <summary>
        /// Raised when the user selects consoles group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsolesGroupSelectionChanged;
        #endregion
        #region Console Events
        /// <summary>
        /// Raised when new console added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsoleAdded;
        /// <summary>
        /// Raised when new console removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsoleRemoved;
        /// <summary>
        /// Raised when new consoles collection cleared
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConoslesCleared;
        /// <summary>
        /// Raised when new console get renamed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConosleRenamed;
        /// <summary>
        /// Raised when new console get moved
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsoleMoved;
        /// <summary>
        /// Raised when new consoles get sorted
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsolesSorted;
        /// <summary>
        /// Raised when a column resized of console
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsoleColumnResized;
        /// <summary>
        /// Raised when a column reorded of console
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsoleColumnReorder;
        /// <summary>
        /// Raised when a column visbile changed of console
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsoleColumnVisibleChanged;
        /// <summary>
        /// Raised when new console properties get changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsolePropertiesChanged;
        /// <summary>
        /// Raised when the user selects console
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ConsoleSelectionChanged;
        #endregion
        #region Playlists Group Events
        /// <summary>
        /// Raised when the user selects playlists group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupSelectionChanged;
        /// <summary>
        /// Raised when new playlists group added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupAdded;
        /// <summary>
        /// Raised when new playlists group removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupRemoved;
        /// <summary>
        /// Raised when new playlists groups collection cleared
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistGroupsCleared;
        /// <summary>
        /// Raised when new playlists group get renamed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistGroupsRenamed;
        /// <summary>
        /// Raised when new playlists group get moved
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupMoved;
        /// <summary>
        /// Raised when new playlist groups collection sorted
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistGroupsSorted;
        /// <summary>
        /// Raised when playlists group properties get changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupPropertiesChanged;
        /// <summary>
        /// Raised when a column visible changed of playlists group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupColumnVisibleChanged;
        /// <summary>
        /// Raised when a column resized of playlists group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupColumnResized;
        /// <summary>
        /// Raised when a column reorder of playlists group
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsGroupColumnReorder;
        #endregion
        #region Playlist Events
        /// <summary>
        /// Raised when the user selects playlist
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistSelectionChanged;
        /// <summary>
        /// Raised when new playlist added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistAdded;
        /// <summary>
        /// Raised when new playlist removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistRemoved;
        /// <summary>
        /// Raised when new playlists collection cleared
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsCleared;
        /// <summary>
        /// Raised when new playlist get renamed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistRenamed;
        /// <summary>
        /// Raised when new playlist get moved
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistMoved;
        /// <summary>
        /// Raised when playlist roms get reordered
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistRomsReorder;
        /// <summary>
        /// Raised when new playlists get sorted
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistsSorted;
        /// <summary>
        /// Raised when a column resized of playlist
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistColumnResized;
        /// <summary>
        /// Raised when a column re-orded of playlist
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistColumnReorder;
        /// <summary>
        /// Raised when a column visible changed of playlist
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistColumnVisibleChanged;
        /// <summary>
        /// Raised when new playlist properties get changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler PlaylistPropertiesChanged;
        #endregion
        #region Rom Events
        /// <summary>
        /// Raised when new Rom added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomAdded;
        /// <summary>
        /// Raised when a Rom showed to user.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<RomShowedArgs> RomShowed;
        /// <summary>
        /// Raised when new Roms added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsAdded;
        /// <summary>
        /// Raised when rom removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomRemoved;
        /// <summary>
        /// Raised when the user finished playing a rom
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<RomFinishedPlayArgs> RomFinishedPlayed;
        /// <summary>
        /// Raised when roms removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsRemoved;
        /// <summary>
        /// Raised when a rom renamed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomRenamed;
        /// <summary>
        /// Raised when roms icon changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomIconsChanged;
        /// <summary>
        /// Raised when roms collection cleared
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsCleared;
        /// <summary>
        /// Raised when roms get moved
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomMoved;
        /// <summary>
        /// Raised when rom properties get changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<RomPropertiesChangedArgs> RomPropertiesChanged;
        /// <summary>
        /// Raised when roms properties get changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomMultiplePropertiesChanged;
        /// <summary>
        /// Raised when roms collection sorted
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsSorted;
        /// <summary>
        /// Raised when a rom's rating value changed 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomRatingChanged;
        /// <summary>
        /// Raised when roms need to be refreshed from selection
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsRefreshRequest;
        /// <summary>
        /// Raised when roms added to playlist
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsAddedToPlaylist;
        /// <summary>
        /// Raised when roms removed from playlist
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsRemovedFromPlaylist;
        /// <summary>
        /// Raised when rom selection changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomSelectionChanged;
        #endregion
        #region Emulators Events
        /// <summary>
        /// Raised when new Emulator added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorAdded;
        /// <summary>
        /// Raised when Emulator removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorRemoved;
        /// <summary>
        /// Raised when a Emulator renamed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorRenamed;
        /// <summary>
        /// Raised when Emulators collection cleared
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorsCleared;
        /// <summary>
        /// Raised when Emulator get moved
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorMoved;
        /// <summary>
        /// Raised when Emulators collection sorted
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorsSorted;
        /// <summary>
        /// Raised when an emulator properties changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorPropertiesChanged;
        /// <summary>
        /// Raised when Emulators need to be refreshed from selection
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<RefreshEmulatorsArgs> EmulatorsRefreshRequest;
        /// <summary>
        /// Raised when emulator selection changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EmulatorSelectionChanged;
        #endregion
        /// <summary>
        /// Raised when an element of EO changed it's icon
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ElementIconChanged;
        /// <summary>
        /// Raised to order the main window to minimize
        /// </summary>
        [field: NonSerialized]
        public event EventHandler MainWindowMinimize;
        /// <summary>
        /// Raised to order the main window to return to original state
        /// </summary>
        [field: NonSerialized]
        public event EventHandler MainWindowReturnToNormal;
        /// <summary>
        /// Raised to request a categories list clear
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RequestCategoriesListClear;
        /// <summary>
        /// Raised to request a roms search
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<SearchRequestArgs> RequestSearch;
        /// <summary>
        /// Raised when a filter added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler FilterAdded;
        /// <summary>
        /// Raised when a filter removed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler FilterRemoved;
        /// <summary>
        /// Raised when a filter edit
        /// </summary>
        [field: NonSerialized]
        public event EventHandler FilterEdit;
        /// <summary>
        /// Raised when an information container added to a console.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler InformationContainerAdded;
        /// <summary>
        /// Raised when an information container removed from console.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler InformationContainerRemoved;
        /// <summary>
        /// Raised when an information container changed it's index.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler InformationContainerMoved;
        /// <summary>
        /// Raised when an information container shown/hiden
        /// </summary>
        [field: NonSerialized]
        public event EventHandler InformationContainerVisibiltyChanged;
        /// <summary>
        /// Raised when information container items get detected
        /// </summary>
        [field: NonSerialized]
        public event EventHandler InformationContainerItemsDetected;
        /// <summary>
        /// Raised when information container items get modified
        /// </summary>
        [field: NonSerialized]
        public event EventHandler InformationContainerItemsModified;
        /// <summary>
        /// Raised to request a priority update.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RefreshPriorityRequest;
        /// <summary>
        /// Raised when the clean up finished
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ProfileCleanUpFinished;
        /// <summary>
        /// Raised when rom extensions changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ExtensionChange;
        /// <summary>
        /// Raised when the core is about to launch a rom
        /// </summary>
        [field: NonSerialized]
        public event EventHandler BeforeRomLaunch;
        /// <summary>
        /// Raised when a database imported
        /// </summary>
        [field: NonSerialized]
        public event EventHandler DatabaseImported;
        /// <summary>
        /// Raised when EO start launching a game
        /// </summary>
        [field: NonSerialized]
        public event EventHandler GamePlayStart;
        /// <summary>
        /// Raised when the rom loading process starts
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsLoadingStarted;
        /// <summary>
        /// Raised when the roms loading process finishes
        /// </summary>
        [field: NonSerialized]
        public event EventHandler RomsLoadingFinished;
        #endregion

        #region Event handlers
        /// <summary>
        /// Call this after opening profile
        /// </summary>
        public void OnProfileOpened()
        {
            ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        }
        /*Console Groups*/
        /// <summary>
        /// Called when a consoles group added to the collection
        /// </summary>
        public void OnConsolesGroupAdd()
        {
            if (ConoslesGroupAdded != null)
                ConoslesGroupAdded(this, new EventArgs());
        }
        /// <summary>
        /// Called when a consoles group removed from the collection
        /// </summary>
        /// <param name="consolesGroupID">The consoles group id that removed</param>
        public void OnConsolesGroupRemoved(string consolesGroupID)
        {
            if (ConoslesGroupRemoved != null)
                ConoslesGroupRemoved(this, new EventArgs());
            // Delete consoles of this consoles group
            for (int i = 0; i < consoles.Count; i++)
            {
                if (consoles[i].ParentGroupID == consolesGroupID)
                {
                    // Delete roms of this console

                    // Get the roms for this console
                    ProfileManager profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");

                    profileManager.OpenCacheStream();
                    ProfileManager.CachedConsoleInfo inf = profileManager.GetConsoleInfo(consoles[i].ID);
                    Rom[] roms_of_console = new Rom[0];
                    if (inf.ID != "")
                    {
                        for (int j = 0; j < inf.RomsCount; j++)
                        {
                            // Get the rom !
                            ProfileManager.CachedRomData romInf = profileManager.GetNextRom();
                            if (romInf.ID == "")
                                continue;// Curropted rom !?

                            // Remove it if it is exist in memory !
                            if (roms.Contains(romInf.ID))
                                roms.Remove(romInf.ID, false);
                            // Mark it for deletion
                            if (marked_to_delete != null)
                                if (!marked_to_delete.Contains(romInf.ID))
                                    marked_to_delete.Add(romInf.ID);
                        }
                        roms_of_console = roms.GetRomsForConsoleNotCahced(consoles[i].ID);
                    }
                    else
                    {
                        roms_of_console = roms[consoles[i].ID, false];
                    }
                    // Make sure there is nothing left ...
                    roms.RemoveThatBelongToConsole(consoles[i].ID);

                    profileManager.CloseCacheStream();

                    consoles.RemoveAt(i);
                    i--;
                }
            }
            Trace.WriteLine("Consoles group removed. ID=" + consolesGroupID, "Profile");
        }
        /// <summary>
        /// Raises the ConsolesGroupPropertiesChanged event
        /// </summary>
        /// <param name="name">The consoles group name</param>
        public void OnConsolesGroupPropertiesChanged(string name)
        {
            if (ConsolesGroupPropertiesChanged != null)
                ConsolesGroupPropertiesChanged(this, new EventArgs());
            Trace.WriteLine("Console group properties changed. Name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the ConsolesGroupColumnVisibleChanged event
        /// </summary>
        /// <param name="consolesGroupName">The consoles group name</param>
        public void OnConsolesGroupColumnVisibleChanged(string consolesGroupName)
        {
            if (ConsolesGroupColumnVisibleChanged != null)
                ConsolesGroupColumnVisibleChanged(this, new EventArgs());
            Trace.WriteLine("Consoles group column visible changed. Consoles group name=" + consolesGroupName, "Profile");
        }
        /// <summary>
        /// Raises the ConsolesGroupColumnResized event
        /// </summary>
        /// <param name="consolesGroupName">The consoles group name that contains the column</param>
        public void OnConsolesGroupColumnResized(string consolesGroupName)
        {
            if (ConsolesGroupColumnResized != null)
                ConsolesGroupColumnResized(this, new EventArgs());
            Trace.WriteLine("Consoles group column resized. Consoles group name=" + consolesGroupName, "Profile");
        }
        /// <summary>
        /// Raises the ConsolesGroupColumnResized event
        /// </summary>
        /// <param name="consolesGroupName">The consoles group name that contains the column</param>
        public void OnConsolesGroupColumnReorder(string consolesGroupName)
        {
            if (ConsolesGroupColumnReorder != null)
                ConsolesGroupColumnReorder(this, new EventArgs());
            Trace.WriteLine("Consoles group column reordered. Consoles group name=" + consolesGroupName, "Profile");
        }
        /// <summary>
        /// Raises the ConsolesGroupSelectionChanged event
        /// </summary>
        /// <param name="name">The consoles group that selected.</param>
        public void OnConsolesGroupSelected(string name)
        {
            if (ConsolesGroupSelectionChanged != null)
                ConsolesGroupSelectionChanged(this, null);
            Trace.WriteLine("Consoles group selected. Name=" + name, "Profile");
        }
        /// <summary>
        /// Called when consoles group collection cleared
        /// </summary>
        public void OnConsoleGroupsClear()
        {
            if (ConoslesGroupsCleared != null)
                ConoslesGroupsCleared(this, new EventArgs());
            Trace.WriteLine("Console groups collection cleared", "Profile");
        }
        /// <summary>
        /// Called when consoles group collection cleared
        /// </summary>
        public void OnConsoleGroupsSort()
        {
            if (ConoslesGroupsSorted != null)
                ConoslesGroupsSorted(this, new EventArgs());
            Trace.WriteLine("Console groups collection sorted", "Profile");
        }
        /// <summary>
        /// Called when consoles group moved 
        /// </summary>
        /// <param name="name">The name of the conosles group that moved</param>
        public void OnConsolesGroupMoved(string name)
        {
            if (ConoslesGroupsMoved != null)
                ConoslesGroupsMoved(this, new EventArgs());
            Trace.WriteLine("Consoles group moved: name=" + name, "Profile");
        }
        /*Playlist Groups*/
        /// <summary>
        /// Called when a playlists group added to the collection
        /// </summary>
        public void OnPlaylistsGroupAdd()
        {
            if (PlaylistsGroupAdded != null)
                PlaylistsGroupAdded(this, new EventArgs());
        }
        /// <summary>
        /// Called when a playlists group removed from the collection
        /// </summary>
        /// <param name="playlistsGroupID">The playlists group id that removed</param>
        public void OnPlaylistsGroupRemoved(string playlistsGroupID)
        {
            if (PlaylistsGroupRemoved != null)
                PlaylistsGroupRemoved(this, new EventArgs());
            // Delete playlists of this consoles group
            for (int i = 0; i < playlists.Count; i++)
            {
                if (playlists[i].ParentGroupID == playlistsGroupID)
                {
                    playlists.RemoveAt(i);
                    i--;
                }
            }
            Trace.WriteLine("Playlists group removed. ID=" + playlistsGroupID, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistsGroupPropertiesChanged event
        /// </summary>
        /// <param name="name">The playlists group name</param>
        public void OnPlaylistsGroupPropertiesChanged(string name)
        {
            if (PlaylistsGroupPropertiesChanged != null)
                PlaylistsGroupPropertiesChanged(this, new EventArgs());
            Trace.WriteLine("Playlists group properties changed. Name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistsGroupColumnVisibleChanged event
        /// </summary>
        /// <param name="playlistsGroupName">The playlists group name</param>
        public void OnPlaylistsGroupColumnVisibleChanged(string playlistsGroupName)
        {
            if (PlaylistsGroupColumnVisibleChanged != null)
                PlaylistsGroupColumnVisibleChanged(this, new EventArgs());
            Trace.WriteLine("Playlists group column visible changed. Playlists group name=" + playlistsGroupName, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistsGroupColumnResized event
        /// </summary>
        /// <param name="playlistsGroupName">The playlists group name that contains the column</param>
        public void OnPlaylistsGroupColumnResized(string playlistsGroupName)
        {
            if (PlaylistsGroupColumnResized != null)
                PlaylistsGroupColumnResized(this, new EventArgs());
            Trace.WriteLine("Playlists group column resized. Playlists group name=" + playlistsGroupName, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistsGroupColumnReorder event
        /// </summary>
        /// <param name="playlistsGroupName">The playlists group name that contains the column</param>
        public void OnPlaylistsGroupColumnReorder(string playlistsGroupName)
        {
            if (PlaylistsGroupColumnReorder != null)
                PlaylistsGroupColumnReorder(this, new EventArgs());
            Trace.WriteLine("Playlists group column reordered. Playlists group name=" + playlistsGroupName, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistsGroupSelectionChanged event
        /// </summary>
        /// <param name="name">The playlists group that selected.</param>
        public void OnPlaylistsGroupSelected(string name)
        {
            if (PlaylistsGroupSelectionChanged != null)
                PlaylistsGroupSelectionChanged(this, null);
            Trace.WriteLine("Playlists group selected. Name=" + name, "Profile");
        }
        /// <summary>
        /// Called when playlist groups collection cleared
        /// </summary>
        public void OnPlaylistGroupsClear()
        {
            if (PlaylistGroupsCleared != null)
                PlaylistGroupsCleared(this, new EventArgs());
            Trace.WriteLine("Playlist groups collection cleared", "Profile");
        }
        /// <summary>
        /// Called when playlist groups collection sorted
        /// </summary>
        public void OnPlaylistGroupsSort()
        {
            if (PlaylistGroupsSorted != null)
                PlaylistGroupsSorted(this, new EventArgs());
            Trace.WriteLine("Playlist groups collection sorted", "Profile");
        }
        /// <summary>
        /// Called when playlists group moved 
        /// </summary>
        /// <param name="name">The name of the playlists group that moved</param>
        public void OnPlaylistsGroupMoved(string name)
        {
            if (PlaylistsGroupMoved != null)
                PlaylistsGroupMoved(this, new EventArgs());
            Trace.WriteLine("Consoles group moved: name=" + name, "Profile");
        }
        /*Consoles*/
        /// <summary>
        /// Called when a console added to the collection
        /// </summary>
        public void OnConsoleAdd()
        {
            if (ConsoleAdded != null)
                ConsoleAdded(this, new EventArgs());
        }
        /// <summary>
        /// Called when a console removed from the collection
        /// </summary>
        /// <param name="consoleID">The console id that removed</param>
        public void OnConsoleRemoved(string consoleID)
        {
            if (ConsoleRemoved != null)
                ConsoleRemoved(this, new EventArgs());
            // If the selection is this console, select something else
            if (recenltySelected == SelectionType.Console && selectedConsoleID == consoleID)
            {
                recenltySelected = SelectionType.None;
                selectedConsoleID = "";
            }
            // Remove all roms related to this console...
            Rom[] console_roms = roms.GetRomsForConsoleNotCahced(consoleID);

            if (console_roms.Length == 0)
            {
                if (pManagerService == null)
                    pManagerService = (ProfileManager)ServicesManager.GetService("Profile Manager");
                // Use cache method ...
                pManagerService.OpenCacheStream();
                // Get the console
                ProfileManager.CachedConsoleInfo consoleINF = pManagerService.GetConsoleInfo(consoleID);
                if (consoleINF.ID != "")
                {
                    // Get all roms and mark them to be remove !
                    for (int i = 0; i < consoleINF.RomsCount; i++)
                    {
                        // Get the rom
                        ProfileManager.CachedRomData romINF = pManagerService.GetNextRom();
                        if (romINF.ID != "")
                        {
                            if (!marked_to_delete.Contains(romINF.ID))
                                marked_to_delete.Add(romINF.ID);
                        }
                    }
                }
            }
            else
            {
                // Just remove them !
                for (int i = 0; i < console_roms.Length; i++)
                {
                    roms.Remove(console_roms[i], false);
                }
            }

            // Remove from emulators (if they consider this console as a parent)
            for (int i = 0; i < emulators.Count; i++)
            {
                emulators[i].RemoveParent(consoleID);
            }
            Trace.WriteLine("Console removed. ID=" + consoleID, "Profile");
        }
        /// <summary>
        /// Called when the consoles collection cleared
        /// </summary>
        public void OnConsolesClear()
        {
            if (ConoslesCleared != null)
                ConoslesCleared(this, new EventArgs());
            Trace.WriteLine("Consoles collection cleared", "Profile");
        }
        /// <summary>
        /// Called when console moved 
        /// </summary>
        /// <param name="name">The name of the console that moved</param>
        public void OnConsoleMoved(string name)
        {
            if (ConsoleMoved != null)
                ConsoleMoved(this, new EventArgs());
            Trace.WriteLine("Console moved: name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the ConsolesSorted event
        /// </summary>
        public void OnConsolesSort()
        {
            if (ConsolesSorted != null)
                ConsolesSorted(this, new EventArgs());
            Trace.WriteLine("Consoles sorted", "Profile");
        }
        /// <summary>
        /// Raises the ConsolePropertiesChanged event
        /// </summary>
        /// <param name="name"></param>
        public void OnConsolePropertiesChanged(string name)
        {
            if (ConsolePropertiesChanged != null)
                ConsolePropertiesChanged(this, new EventArgs());
            Trace.WriteLine("Console properties changed. Name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the ConsoleSelectionChanged event
        /// </summary>
        /// <param name="name">The console that selected.</param>
        public void OnConsoleSelected(string name)
        {
            if (ConsoleSelectionChanged != null)
                ConsoleSelectionChanged(this, null);
            Trace.WriteLine("Console selected. Name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the ConsoleColumnResized event
        /// </summary>
        /// <param name="consoleName">The console name that contains the column</param>
        public void OnConsoleColumnResized(string consoleName)
        {
            if (ConsoleColumnResized != null)
                ConsoleColumnResized(this, new EventArgs());
            Trace.WriteLine("Console column resized. Console name=" + consoleName, "Profile");
        }
        /// <summary>
        /// Raises the ConsoleColumnReorder event
        /// </summary>
        /// <param name="consoleName">The console name that contains the column</param>
        public void OnConsoleColumnReorder(string consoleName)
        {
            if (ConsoleColumnReorder != null)
                ConsoleColumnReorder(this, new EventArgs());
            Trace.WriteLine("Console column reordered. Console name=" + consoleName, "Profile");
        }
        /// <summary>
        /// Raises the ConsoleColumnVisibleChanged event
        /// </summary>
        /// <param name="consoleName">The console name that contains the column</param>
        public void OnConsoleColumnVisibleChanged(string consoleName)
        {
            if (ConsoleColumnVisibleChanged != null)
                ConsoleColumnVisibleChanged(this, new EventArgs());
            Trace.WriteLine("Console column visible changed. Console name=" + consoleName, "Profile");
        }
        /*Playlists*/
        /// <summary>
        /// Raises the PlaylistSelectionChanged event
        /// </summary>
        /// <param name="name">The playlist that selected.</param>
        public void OnPlaylistSelected(string name)
        {
            if (PlaylistSelectionChanged != null)
                PlaylistSelectionChanged(this, null);
            Trace.WriteLine("Playlist selected. Name=" + name, "Profile");
        }
        /// <summary>
        /// Called when a playlist added to the collection
        /// </summary>
        public void OnPlaylistAdd()
        {
            if (PlaylistAdded != null)
                PlaylistAdded(this, new EventArgs());
        }
        /// <summary>
        /// Called when a playlist removed from the collection
        /// </summary>
        /// <param name="playlistID">The playlist id that removed</param>
        public void OnPlaylistRemoved(string playlistID)
        {
            if (PlaylistRemoved != null)
                PlaylistRemoved(this, new EventArgs());
            // Mark all roms that belong to this playlist for deletion
            string[] romIDS = playlists[playlistID].RomIDS.ToArray();
            for (int i = 0; i < romIDS.Length; i++)
            {
                if (!MarkedToBeDeleted.Contains(romIDS[i]))
                    MarkedToBeDeleted.Add(romIDS[i]);
                // Remove it from the roms collection if found
                if (roms.Contains(romIDS[i]))
                    roms.Remove(romIDS[i], false);
            }
            Trace.WriteLine("Playlist removed. ID=" + playlistID, "Profile");
        }
        /// <summary>
        /// Called when the playlists collection cleared
        /// </summary>
        public void OnPlaylistsClear()
        {
            if (PlaylistsCleared != null)
                PlaylistsCleared(this, new EventArgs());
            Trace.WriteLine("Playlists collection cleared", "Profile");
        }
        /// <summary>
        /// Called when playlist moved 
        /// </summary>
        /// <param name="name">The name of the playlist that moved</param>
        public void OnPlaylistMoved(string name)
        {
            if (PlaylistMoved != null)
                PlaylistMoved(this, new EventArgs());
            Trace.WriteLine("Playlist moved: name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistsSorted event
        /// </summary>
        public void OnPlaylistsSort()
        {
            if (PlaylistsSorted != null)
                PlaylistsSorted(this, new EventArgs());
            Trace.WriteLine("Playlists sorted", "Profile");
        }
        /// <summary>
        /// Raises the PlaylistPropertiesChanged event
        /// </summary>
        /// <param name="name"></param>
        public void OnPlaylistPropertiesChanged(string name)
        {
            if (PlaylistPropertiesChanged != null)
                PlaylistPropertiesChanged(this, new EventArgs());
            Trace.WriteLine("Playlist properties changed. Name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistColumnResized event
        /// </summary>
        /// <param name="playlistName">The playlist name that contains the column</param>
        public void OnPlaylistColumnResized(string playlistName)
        {
            if (PlaylistColumnResized != null)
                PlaylistColumnResized(this, new EventArgs());
            Trace.WriteLine("Playlist column resized. Playlist name=" + playlistName, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistColumnReorder event
        /// </summary>
        /// <param name="playlistName">The playlist name that contains the column</param>
        public void OnPlaylistColumnReorder(string playlistName)
        {
            if (PlaylistColumnReorder != null)
                PlaylistColumnReorder(this, new EventArgs());
            Trace.WriteLine("Console column reordered. Playlist name=" + playlistName, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistColumnVisibleChanged event
        /// </summary>
        /// <param name="playlistName">The playlist name that contains the column</param>
        public void OnPlaylistColumnVisibleChanged(string playlistName)
        {
            if (PlaylistColumnVisibleChanged != null)
                PlaylistColumnVisibleChanged(this, new EventArgs());
            Trace.WriteLine("Playlist column visible changed. Playlist name=" + playlistName, "Profile");
        }
        /// <summary>
        /// Raises the PlaylistRomsReorder event
        /// </summary>
        /// <param name="playlistName">The playlist name that contain the roms</param>
        public void OnPlaylistRomsReorder(string playlistName)
        {
            if (PlaylistRomsReorder != null)
                PlaylistRomsReorder(this, new EventArgs());
            Trace.WriteLine("Playlist rom(s) reordered. Playlist name=" + playlistName, "Profile");
        }
        /*Roms*/
        /// <summary>
        /// Called when a rom added to the collection
        /// </summary>
        public void OnRomAdd()
        {
            if (RomAdded != null)
                RomAdded(this, new EventArgs());
        }
        /// <summary>
        /// Called when roms added to the collection
        /// </summary>
        public void OnRomsAdd()
        {
            if (RomsAdded != null)
                RomsAdded(this, new EventArgs());
            Trace.WriteLine("New roms added to the collection.", "Profile");
        }
        /// <summary>
        /// Called when a rom removed from the collection
        /// </summary>
        /// <param name="ID">The rom id that removed</param>
        public void OnRomRemoved(string ID)
        {
            if (RomRemoved != null)
                RomRemoved(this, new EventArgs());
            if (marked_to_delete == null)
                marked_to_delete = new List<string>();
            if (!marked_to_delete.Contains(ID))
                marked_to_delete.Add(ID);
            Trace.WriteLine("Rom removed. ID=" + ID, "Profile");
        }
        /// <summary>
        /// Raises the RomFinishedPlayed event
        /// </summary>
        /// <param name="romName">The rom name</param>
        /// <param name="id">The rom id</param>
        public void OnRomFinishedPlayed(string romName, string id)
        {
            if (RomFinishedPlayed != null)
                RomFinishedPlayed(this, new RomFinishedPlayArgs(id));
            Trace.WriteLine("Rom played: name=" + romName, "Profile");
        }
        /// <summary>
        /// Called when roms collection cleared
        /// </summary>
        public void OnRomsClear()
        {
            if (RomsCleared != null)
                RomsCleared(this, new EventArgs());
            Trace.WriteLine("Roms collection cleared", "Profile");
        }
        /// <summary>
        /// Called when roms collection cleared
        /// </summary>
        public void OnRomsSort()
        {
            if (RomsSorted != null)
                RomsSorted(this, new EventArgs());
            Trace.WriteLine("Roms collection sorted", "Profile");
        }
        /// <summary>
        /// Called when a rom moved 
        /// </summary>
        /// <param name="name">The name of the rom that moved</param>
        public void OnRomMoved(string name)
        {
            if (RomMoved != null)
                RomMoved(this, new EventArgs());
            Trace.WriteLine("Consoles group moved: name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the RomsRemoved event
        /// </summary>
        /// <param name="romsRemovedCount">The count of the roms that removed</param>
        public void OnRomsRemoved(int romsRemovedCount)
        {
            if (RomsRemoved != null)
                RomsRemoved(this, new EventArgs());
            Trace.WriteLine(romsRemovedCount + " Rom(s) removed.", "Profile");
        }
        /// <summary>
        /// Raises the RomsRemoved event
        /// </summary>
        /// <param name="romsCount">The count of the roms that changed icon</param>
        public void OnRomIconsChanged(int romsCount)
        {
            if (RomIconsChanged != null)
                RomIconsChanged(this, new EventArgs());
            Trace.WriteLine(romsCount + " Rom(s) changed icon.", "Profile");
        }
        /// <summary>
        /// Raises the RomRatingChanged event
        /// </summary>
        /// <param name="romName">The rom name</param>
        public void OnRomRatingChanged(string romName)
        {
            if (RomRatingChanged != null)
                RomRatingChanged(this, new EventArgs());
            Trace.WriteLine("Rom rating changed: name=" + name, "Profile");
        }
        /// <summary>
        /// Raise the RomsRefreshRequest event
        /// </summary>
        public void OnRomsRefreshRequest()
        {
            if (RomsRefreshRequest != null)
                RomsRefreshRequest(this, new EventArgs());
        }
        /// <summary>
        /// Raises the RomRenamed event
        /// </summary>
        /// <param name="romOldName">The rom old name before renaming</param>
        /// <param name="romNewName">The rom new name after renaming</param>
        public void OnRomRenamed(string romOldName, string romNewName)
        {
            if (RomRenamed != null)
                RomRenamed(this, new EventArgs());
            Trace.WriteLine("Rom '" + romOldName + "' renamed to '" + romNewName + "'", "Profile");
        }
        /// <summary>
        /// Raises the RomsAddedToPlaylist event
        /// </summary>
        /// <param name="playlistName">The playlist name</param>
        /// <param name="romsCount">The roms count that added</param>
        public void OnRomsAddedToPlaylist(string playlistName, int romsCount)
        {
            if (RomsAddedToPlaylist != null)
                RomsAddedToPlaylist(this, new EventArgs());
            Trace.WriteLine(romsCount + " Rom(s) added to playlist '" + playlistName + "'", "Profile");
        }
        /// <summary>
        /// Raises the RomsRemovedFromPlaylist event
        /// </summary>
        /// <param name="playlistName">The playlist name</param>
        /// <param name="romsCount">The roms count that added</param>
        public void OnRomsRemovedFromPlaylist(string playlistName, int romsCount)
        {
            if (RomsRemovedFromPlaylist != null)
                RomsRemovedFromPlaylist(this, new EventArgs());
            Trace.WriteLine(romsCount + " Rom(s) removed from playlist '" + playlistName + "'", "Profile");
        }
        /// <summary>
        /// Raises the RomPropertiesChanged event
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="requireRomsRefresh"></param>
        public void OnRomPropertiesChanged(string name, string id, bool requireRomsRefresh)
        {
            if (RomPropertiesChanged != null)
                RomPropertiesChanged(this, new RomPropertiesChangedArgs(id, requireRomsRefresh));
            Trace.WriteLine("Rom properties changed. Name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the RomMultiblePropertiesChanged event
        /// </summary>
        public void OnRomMultiplePropertiesChanged()
        {
            if (RomMultiplePropertiesChanged != null)
                RomMultiplePropertiesChanged(this, new EventArgs());
            Trace.WriteLine("Rom multiple properties changed.", "Profile");
        }
        /// <summary>
        /// Raises the RomShowed event
        /// </summary>
        /// <param name="rom">The rom that showed</param>
        public void OnRomShowed(Rom rom)
        {
            if (RomShowed != null) RomShowed(this, new RomShowedArgs(rom));
        }
        /// <summary>
        /// Raises the RomSelectionChanged event
        /// </summary>
        public void OnRomSelectionChanged()
        {
            if (RomSelectionChanged != null)
                RomSelectionChanged(this, new EventArgs());
        }
        /*Emulators*/
        /// <summary>
        /// Raises the EmulatorAdded event
        /// </summary>
        /// <param name="name">The emualtor's name</param>
        public void OnEmulatorAdded(string name)
        {
            if (EmulatorAdded != null)
                EmulatorAdded(this, new EventArgs());
            Trace.WriteLine("Emulator added '" + name + "'", "Profile");
        }
        /// <summary>
        /// Raises the EmulatorRemoved event
        /// </summary>
        /// <param name="name">The emualtor's name</param>
        public void OnEmulatorRemoved(string name)
        {
            if (EmulatorRemoved != null)
                EmulatorRemoved(this, new EventArgs());
            Trace.WriteLine("Emulator removed '" + name + "'", "Profile");
        }
        /// <summary>
        /// Raises the EmulatorRenamed event
        /// </summary>
        /// <param name="oldName">The emualtor's od name</param>
        /// <param name="newName">The emualtor's new name</param>
        public void OnEmulatorRenamed(string oldName, string newName)
        {
            if (EmulatorRenamed != null)
                EmulatorRenamed(this, new EventArgs());
            Trace.WriteLine("Emulator renamed from '" + oldName + "' to '" + newName + "'", "Profile");
        }
        /// <summary>
        /// Raises the EmulatorsCleared event
        /// </summary>
        public void OnEmulatorsCleared()
        {
            if (EmulatorRenamed != null)
                EmulatorRenamed(this, new EventArgs());
            Trace.WriteLine("Emulators collection cleared", "Profile");
        }
        /// <summary>
        /// Raises the EmulatorMoved event
        /// </summary>
        /// <param name="name">The emualtor's name</param>
        public void OnEmulatorMoved(string name)
        {
            if (EmulatorMoved != null)
                EmulatorMoved(this, new EventArgs());
            Trace.WriteLine("Emulator moved '" + name + "'", "Profile");
        }
        /// <summary>
        /// Raises the EmulatorsSorted event
        /// </summary>
        public void OnEmulatorsSorted()
        {
            if (EmulatorsSorted != null)
                EmulatorsSorted(this, new EventArgs());
            Trace.WriteLine("Emulators collection sorted", "Profile");
        }
        /// <summary>
        /// Raises the EmulatorsRefreshRequest event
        /// </summary>
        /// <param name="emuIDs">The emulators to refresh (ids of emulators)</param>
        public void OnEmulatorsRefreshRequest(string[] emuIDs)
        {
            if (EmulatorsRefreshRequest != null)
                EmulatorsRefreshRequest(this, new RefreshEmulatorsArgs(emuIDs));
        }
        /// <summary>
        /// Raises the EmulatorPropertiesChanged event
        /// </summary>
        /// <param name="name"></param>
        public void OnEmulatorPropertiesChanged(string name)
        {
            if (EmulatorPropertiesChanged != null)
                EmulatorPropertiesChanged(this, new EventArgs());
            Trace.WriteLine("Emulator properties changed. Name=" + name, "Profile");
        }
        /// <summary>
        /// Raises the EmulatorSelectionChanged event
        /// </summary>
        public void OnEmulatorSelectionChanged()
        {
            if (EmulatorSelectionChanged != null)
                EmulatorSelectionChanged(this, new EventArgs());
        }
        /*Others*/
        /// <summary>
        /// Raise the ElementIconChanged event
        /// </summary>
        /// <param name="element">The element that changed it's icon</param>
        public void OnElementIconChanged(IEOElement element)
        {
            if (ElementIconChanged != null)
                ElementIconChanged(this, new EventArgs());
            Trace.WriteLine("Element icon changed. Element type=" + element.GetType().Name + ", name=" + element.Name, "Profile");
        }
        /// <summary>
        /// Raises the MainWindowMinimize event
        /// </summary>
        public void OnMainWindowMinimize()
        {
            if (MainWindowMinimize != null)
                MainWindowMinimize(this, new EventArgs());
        }
        /// <summary>
        /// Raises the MainWindowReturnToNormal event
        /// </summary>
        public void OnMainWindowReturnToNormal()
        {
            if (MainWindowReturnToNormal != null)
                MainWindowReturnToNormal(this, new EventArgs());
        }
        /// <summary>
        /// Raises the RequestCategoriesListClear event
        /// </summary>
        public void OnRequestCategoriesListClear()
        {
            if (RequestCategoriesListClear != null)
                RequestCategoriesListClear(this, new EventArgs());
        }
        /// <summary>
        /// Raises the RequestSearch event
        /// </summary>
        /// <param name="e">The search args</param>
        public void OnRequestSearch(SearchRequestArgs e)
        {
            if (RequestSearch != null)
                RequestSearch(this, e);
        }
        /// <summary>
        /// Raises the FilterAdded event
        /// </summary>
        public void OnFilterAdded()
        {
            if (FilterAdded != null)
                FilterAdded(this, new EventArgs());
        }
        /// <summary>
        /// Raises the FilterRemoved event
        /// </summary>
        public void OnFilterRemoved()
        {
            if (FilterRemoved != null)
                FilterRemoved(this, new EventArgs());
        }
        /// <summary>
        /// Raises the FilterEdit event
        /// </summary>
        public void OnFilterEdit()
        {
            if (FilterEdit != null)
                FilterEdit(this, new EventArgs());
        }
        /// <summary>
        /// Raises the InformationContainerAdded event
        /// </summary>
        /// <param name="icName">The information container name</param>
        /// <param name="consoleName">The parent console name</param>
        public void OnInformationContainerAdded(string icName, string consoleName)
        {
            if (InformationContainerAdded != null)
                InformationContainerAdded(this, new EventArgs());
            Trace.WriteLine(string.Format("New information conrainer added: '{0}' to console '{1}'", icName, consoleName), "Profile");
        }
        /// <summary>
        /// Raises the InformationContainerRemoved event
        /// </summary>
        /// <param name="consoleName">The parent console name</param>
        public void OnInformationContainerRemoved(string consoleName)
        {
            if (InformationContainerRemoved != null)
                InformationContainerRemoved(this, new EventArgs());
            Trace.WriteLine(string.Format("Information conrainer removed from console '{0}'", consoleName), "Profile");
        }
        /// <summary>
        /// Raises the InformationContainerVisibiltyChanged event
        /// </summary>
        public void OnInformationContainerVisibiltyChanged()
        {
            if (InformationContainerVisibiltyChanged != null)
                InformationContainerVisibiltyChanged(this, new EventArgs());
        }
        /// <summary>
        /// Raises the InformationContainerItemsDetected event
        /// </summary>
        public void OnInformationContainerItemsDetected()
        {
            if (InformationContainerItemsDetected != null)
                InformationContainerItemsDetected(this, new EventArgs());
            Trace.WriteLine("Information conrainer items detected.", "Profile");
        }
        /// <summary>
        /// Raises the InformationContainerItemsModifeed event
        /// </summary>
        /// <param name="icName">The information conrainer item name</param>
        public void OnInformationContainerItemsModified(string icName)
        {
            if (InformationContainerItemsModified != null)
                InformationContainerItemsModified(this, new EventArgs());
            Trace.WriteLine(string.Format("Items modified for information conrainer '{0}'", icName), "Profile");
        }
        /// <summary>
        /// Raises the InformationContainerMoved event
        /// </summary>
        /// <param name="icName">The container name</param>
        public void OnInformationContainerMoved(string icName)
        {
            if (InformationContainerMoved != null)
                InformationContainerMoved(this, new EventArgs());
            Trace.WriteLine(string.Format("Information Conrainer '{0}' moved", icName), "Profile");
        }
        /// <summary>
        /// Raises the RefreshPriorityRequest event.
        /// </summary>
        public void OnRefreshPriorityRequest()
        {
            if (RefreshPriorityRequest != null)
                RefreshPriorityRequest(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ProfileCleanUpFinished event
        /// </summary>
        public void OnProfileCleanUpFinished()
        {
            if (ProfileCleanUpFinished != null)
                ProfileCleanUpFinished(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ExtensionChange event
        /// </summary>
        public void OnExtensionChange()
        {
            if (ExtensionChange != null)
                ExtensionChange(this, new EventArgs());
        }
        /// <summary>
        /// Raises the DatabaseImported event
        /// </summary>
        public void OnDatabaseImported()
        {
            if (DatabaseImported != null)
                DatabaseImported(this, new EventArgs());
        }
        /// <summary>
        /// Raises the RomsLoadingStarted event
        /// </summary>
        public void OnRomsLoadingStarted()
        {
            RomsLoadingStarted?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// Raises the RomsLoadingFinished event
        /// </summary>
        public void OnRomsLoadingFinished()
        {
            RomsLoadingFinished?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
