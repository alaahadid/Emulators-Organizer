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
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
using SevenZip;
using System.Net;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_DetectAndDownloadFromTheGameDBRoms : Form
    {
        public Form_DetectAndDownloadFromTheGameDBRoms(string consoleID, Rom[] romsCollection)
        {
            InitializeComponent();
            this.consoleID = consoleID;
            roms = new RomsCollection(null, false, romsCollection);
            selectedConsole = profileManager.Profile.Consoles[consoleID];
            // Fill up archive extensions
            string exs = "";
            foreach (string ex in selectedConsole.ArchiveExtensions)
                exs += ex + ";";
            if (exs.Length > 1)
                textBox_archive_extesnions.Text = exs.Substring(0, exs.Length - 1);
           /* // Fill up platforms
            ICollection<PlatformSearchResult> results = GamesDB.GetPlatforms();
            foreach (PlatformSearchResult res in results)
            {
                comboBox_platforms.Items.Add(res);
            }
            if (comboBox_platforms.Items.Count > 0)
                comboBox_platforms.SelectedIndex = 0;*/

            List<string> folders = new List<string>();
            // Load the tabs 
            foreach (InformationContainer container in selectedConsole.InformationContainers)
            {
                if (container is InformationContainerInfoText)
                {
                    comboBox_overview_tab.Items.Add(container);
                    if (container.Name == "Overview")
                    {
                        if (((InformationContainerInfoText)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerInfoText)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_overview_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_overview_tab.SelectedItem = container;
                        checkBox_create_new_tab_overview.Checked = false;
                    }
                }
                if (container is InformationContainerImage)
                {
                    comboBox_Banners_tabs.Items.Add(container);
                    if (container.Name == "Banners")
                    {
                        comboBox_Banners_tabs.SelectedItem = container;
                        checkBox_create_tab_Banners.Checked = false;
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Banners_folder.Text = folders[folders.Count - 1];
                        }
                    }
                    comboBox_boxart_bakc_tabs.Items.Add(container);
                    if (container.Name == "Boxart Back")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_boxart_back_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_boxart_bakc_tabs.SelectedItem = container;
                        checkBox_create_new_tab_boxat_back.Checked = false;
                    }
                    comboBox_boxart_front_tab.Items.Add(container);
                    if (container.Name == "Boxart Front")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_boxart_front_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_boxart_front_tab.SelectedItem = container;
                        checkBox_create_new_tab_boxart_front.Checked = false;
                    }
                    comboBox_Fanart_tabs.Items.Add(container);
                    if (container.Name == "Fanart")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Fanart_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_Fanart_tabs.SelectedItem = container;
                        checkBox_create_tab_for_Fanart.Checked = false;
                    }
                    comboBox_Screenshots_tabs.Items.Add(container);
                    if (container.Name == "Screenshots")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Screenshots_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_Screenshots_tabs.SelectedItem = container;
                        checkBox_create_tab_Screenshots.Checked = false;
                    }
                }
            }
            if (comboBox_overview_tab.Items.Count > 0 && comboBox_overview_tab.SelectedIndex < 0)
                comboBox_overview_tab.SelectedIndex = 0;
            if (comboBox_Banners_tabs.Items.Count > 0 && comboBox_Banners_tabs.SelectedIndex < 0)
                comboBox_Banners_tabs.SelectedIndex = 0;
            if (comboBox_boxart_bakc_tabs.Items.Count > 0 && comboBox_boxart_bakc_tabs.SelectedIndex < 0)
                comboBox_boxart_bakc_tabs.SelectedIndex = 0;
            if (comboBox_boxart_front_tab.Items.Count > 0 && comboBox_boxart_front_tab.SelectedIndex < 0)
                comboBox_boxart_front_tab.SelectedIndex = 0;
            if (comboBox_Fanart_tabs.Items.Count > 0 && comboBox_Fanart_tabs.SelectedIndex < 0)
                comboBox_Fanart_tabs.SelectedIndex = 0;
            if (comboBox_Screenshots_tabs.Items.Count > 0 && comboBox_Screenshots_tabs.SelectedIndex < 0)
                comboBox_Screenshots_tabs.SelectedIndex = 0;
            // Force user to select tab manually for other tabs.
        }
        private Core.Console selectedConsole;
        private string consoleID;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private TextWriterTraceListener listner;
        private WebClient client = new WebClient();
        private RomsCollection roms;
        private string logPath;
        private bool _compare_name_name;
        private bool _compare_fileName_name;
        private bool _turbo_speed;
        private bool _check_inside_archive;
        private List<string> _archive_extesnions = new List<string>();
        private bool _delete_rom_not_found;
        private bool _delete_rom_not_found_file;
        private bool _delete_rom_not_found_related_files;
        // Parent and children
        private bool _rename_parent;
        private bool _parent_match_keep_all_children;
        private bool _parent_match_keep_child_match;
        private bool _parent_not_match_keep_on_one_child_match;
        private bool _parent_not_match_keep_on_one_child_match_dont_keep_not_mathced_children;
        private bool _parent_not_match_make_matched_children_singles;
        // Status
        private string status_master;
        private string status_sub;
        private string status_sub_sub;
        private int progress_master;
        private int progress_sub;
        // Thread
        private Thread mainThread;
        private bool finished;
        // DB options
        private int _db_selected_platform_id;
        private bool _db_rename_console;
        private bool _db_rename_rom_using_title;
        private bool _db_add_elements_as_romdata;

        private bool _db_add_overview_as_tab;
        private bool _db_add_banners_as_tab;
        private bool _db_add_boxart_front_as_tab;
        private bool _db_add_boxart_back_as_tab;
        private bool _db_add_fanart_as_tab;
        private bool _db_add_screenshots_as_tab;
        private bool _db_add_clearlogo_as_tab;

        private bool _db_create_new_tab_for_overview;
        private bool _db_create_new_tab_for_banners;
        private bool _db_create_new_tab_for_boxart_front;
        private bool _db_create_new_tab_for_boxart_back;
        private bool _db_create_new_tab_for_fanart;
        private bool _db_create_new_tab_for_screenshots;
        private bool _db_create_new_tab_for_clearlogo;

        private string _db_overview_folder;
        private string _db_banners_folder;
        private string _db_boxart_front_folder;
        private string _db_boxart_back_folder;
        private string _db_fanart_folder;
        private string _db_screenshots_folder;
        private string _db_clearlogo_folder;

        private string _db_overview_ic_id;
        private string _db_banners_ic_id;
        private string _db_boxart_front_ic_id;
        private string _db_boxart_back_ic_id;
        private string _db_fanart_ic_id;
        private string _db_screenshots_ic_id;
        private string _db_clearlogo_ic_id;

        private bool _db_overview_ic_clearlist;
        private bool _db_banners_ic_clearlist;
        private bool _db_boxart_front_ic_clearlist;
        private bool _db_boxart_back_ic_clearlist;
        private bool _db_fanart_ic_clearlist;
        private bool _db_screenshots_ic_clearlist;
        private bool _db_clearlogo_ic_clearlist;

        private bool _db_fanart_ic_limitdownload;
        private bool _db_screenshots_ic_limitdownload;
        private bool _db_banners_ic_limitdownload;

        private void PROCESS()
        {
           /* // Add listener
            string logFileName = string.Format("{0}-detect and download from the-game-db.txt",
                DateTime.Now.ToLocalTime().ToString());
            logFileName = logFileName.Replace(":", "");
            logFileName = logFileName.Replace("/", "-");
            logPath = Path.Combine("Logs", logFileName);
            listner = new TextWriterTraceListener(HelperTools.GetFullPath(logPath));
            Trace.Listeners.Add(listner);
            Services.ServicesManager.OnDisableWindowListner();

            // Start
            Trace.WriteLine(string.Format("Detect and download from the-game-db.for console '{0}' started at {1}",
             selectedConsole.Name, DateTime.Now.ToLocalTime()), "Detect And Download From TheGamesDB.net");
            int step_index = 0;
            int steps_count = 5;
            #region 1 Getting all platform entries from the internet
            Trace.WriteLine("Getting entries for selected platform ...", "Detect And Download From TheGamesDB.net");
            status_master = "Getting entries for selected platform ...";
            progress_master = 100 / (steps_count - step_index);
            // Get database content
            Platform selectedPlatform = GamesDB.GetPlatform(_db_selected_platform_id);
            List<GameSearchResult> databaseEntries = new List<GameSearchResult>(GamesDB.GetPlatformGames(_db_selected_platform_id));
            Trace.WriteLine("Platform entries done, total of " + databaseEntries.Count + " entries found.", "Detect And Download From TheGamesDB.net");
            #endregion
            #region 2 Get the roms
            step_index++;
            Trace.WriteLine("Collecting the roms ...", "Detect And Download From TheGamesDB.net");
            status_master = "Collecting the roms ...";
            progress_master = 100 / (steps_count - step_index);
            // Add children finally.
            // roms.AddRange(profileManager.Profile.Roms.GetChildrenRoms(selectedConsole.ID));
            Trace.WriteLine("Roms collected, total of " + roms.Count + " rom(s) [SINGLES AND PARENTS ONLY, children roms get checked later]", "Detect And Download From TheGamesDB.net");
            #endregion
            #region 3 Renaming Console
            step_index++;
            Trace.WriteLine("Renaming console", "Detect And Download From TheGamesDB.net");
            status_master = "Renaming console ...";
            progress_master = 100 / (steps_count - step_index);

            if (_db_rename_console)
            {
                string old_console_name = selectedConsole.Name;
                profileManager.Profile.Consoles[selectedConsole.ID].Name = selectedPlatform.Name;
                Trace.WriteLine(string.Format("Console renamed from {0} to {1}", old_console_name, selectedPlatform.Name), "Detect And Download From TheGamesDB.net");
            }
            #endregion
            #region 4 Compare and apply stuff
            step_index++;
            Trace.WriteLine("Comparing and applying naming", "Detect And Download From TheGamesDB.net");
            status_master = "Comparing ...";
            progress_master = 100 / (steps_count - step_index);

            int matchedCount = 0;
            List<string> matchedRomNames = new List<string>();
            List<string> notMatchedRomNames = new List<string>();
            for (int rom_index = roms.Count - 1; rom_index >= 0; rom_index--)
            {
                status_sub_sub = "";
                if (roms[rom_index].IsSingle)
                {
                    bool matched = false;
                    COMPARE_ROM(roms[rom_index], true, ref databaseEntries, ref matchedRomNames, ref notMatchedRomNames,
                        ref matchedCount, out matched, true);
                }
                else if (roms[rom_index].IsParentRom)
                {
                    // Compare the parent
                    bool parent_match = false;
                    COMPARE_ROM(roms[rom_index], _rename_parent, ref databaseEntries, ref matchedRomNames, ref notMatchedRomNames,
                        ref matchedCount, out parent_match, false);
                    List<string> children_ids = roms[rom_index].ChildrenRoms;
                    List<string> children_matched_ids = new List<string>();
                    // Compare children ...
                    foreach (string childID in children_ids)
                    {
                        bool c_match = false;
                        COMPARE_ROM(profileManager.Profile.Roms[childID], true, ref databaseEntries, ref matchedRomNames, ref notMatchedRomNames,
                        ref matchedCount, out c_match, false);

                        if (c_match)
                            children_matched_ids.Add(childID);
                    }
                    if (parent_match)
                        Trace.WriteLine(" --> PARENT ROM MATCHED [" + roms[rom_index].ID + "] (" + roms[rom_index].Name + ")", "Detect And Download From TheGamesDB.net");

                    // Do the delete stuff
                    if (_delete_rom_not_found)
                    {
                        if (parent_match)
                        {
                            // The parent match, see what should we do with the children
                            if (_parent_match_keep_all_children)
                            {
                                // Do nothing, keep parent with its all children untouched.
                                Trace.WriteLine(" +--> All children kept along with parent", "Detect And Download From TheGamesDB.net");
                            }
                            else if (_parent_match_keep_child_match)
                            {
                                Trace.WriteLine(" +--> Removing not matched children ..", "Detect And Download From TheGamesDB.net");
                                // Remove the children that not match from parent
                                for (int c = 0; c < children_ids.Count; c++)
                                {
                                    if (!children_matched_ids.Contains(children_ids[c]))
                                    {
                                        // Delete the child from database
                                        string cName = profileManager.Profile.Roms[children_ids[c]].Name;
                                        DeleteRomRoutin(profileManager.Profile.Roms[children_ids[c]], ref notMatchedRomNames);
                                        Trace.WriteLine(" +---> CHILD REMOVED [" + children_ids[c] + "] (" + cName + ")", "Detect And Download From TheGamesDB.net");
                                        // Remove it from the parent
                                        roms[rom_index].Modified = true;
                                        roms[rom_index].ChildrenRoms.Remove(children_ids[c]);
                                    }
                                }
                                // Check the parent situation
                                if (roms[rom_index].ChildrenRoms.Count == 0)
                                {
                                    // no parent any more ...
                                    roms[rom_index].IsParentRom = false;
                                    roms[rom_index].AlwaysChooseChildWhenPlay = false;
                                }
                            }
                        }
                        else  // Parent not match !
                        {
                            Trace.WriteLine(" --> PARENT ROM DOES NOT MATCH [" + roms[rom_index].ID + "] (" + roms[rom_index].Name + ")", "Detect And Download From TheGamesDB.net");
                            if (_parent_not_match_keep_on_one_child_match)
                            {
                                if (children_matched_ids.Count > 0)
                                {
                                    Trace.WriteLine(" +--> keeping the parent, one of children match.", "Detect And Download From TheGamesDB.net");
                                    // Keep the parent on one child match.
                                    // Do we have to keep unmatched children ?
                                    if (_parent_not_match_keep_on_one_child_match_dont_keep_not_mathced_children)
                                    {
                                        // Remove the children that not match from parent
                                        for (int c = 0; c < children_ids.Count; c++)
                                        {
                                            if (!children_matched_ids.Contains(children_ids[c]))
                                            {
                                                string cName = profileManager.Profile.Roms[children_ids[c]].Name;
                                                // Delete the child from database
                                                DeleteRomRoutin(profileManager.Profile.Roms[children_ids[c]], ref notMatchedRomNames);
                                                Trace.WriteLine(" +---> CHILD REMOVED [" + children_ids[c] + "] (" + cName + ")", "Detect And Download From TheGamesDB.net");
                                                // Remove it from the parent
                                                roms[rom_index].Modified = true;
                                                roms[rom_index].ChildrenRoms.Remove(children_ids[c]);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Trace.WriteLine(" +--> REMOVING PARENT; NO CHILD MATCH.", "Detect And Download From TheGamesDB.net");
                                    // No child match, remove the parent.
                                    DeleteRomRoutin(roms[rom_index], ref notMatchedRomNames);
                                }
                            }
                            else if (_parent_not_match_make_matched_children_singles)
                            {
                                Trace.WriteLine(" +--> REMOVING PARENT; MAKING MATCHED CHILDREN SINGLES ...", "Detect And Download From TheGamesDB.net");
                                // Remove the parent.
                                DeleteRomRoutin(roms[rom_index], ref notMatchedRomNames);
                                // Make the matched children free
                                for (int c = 0; c < children_ids.Count; c++)
                                {
                                    // Remove the children that not match from database
                                    if (!children_matched_ids.Contains(children_ids[c]))
                                    {
                                        string cName = profileManager.Profile.Roms[children_ids[c]].Name;
                                        // Delete the child from database
                                        DeleteRomRoutin(profileManager.Profile.Roms[children_ids[c]], ref notMatchedRomNames);
                                        Trace.WriteLine(" +---> CHILD REMOVED [" + children_ids[c] + "] (" + cName + ")", "Detect And Download From TheGamesDB.net");
                                    }
                                    else
                                    {
                                        string cName = profileManager.Profile.Roms[children_ids[c]].Name;
                                        // free it !
                                        profileManager.Profile.Roms[children_ids[c]].IsChildRom = false;
                                        profileManager.Profile.Roms[children_ids[c]].IsParentRom = false;
                                        profileManager.Profile.Roms[children_ids[c]].ParentRomID = "";
                                        Trace.WriteLine(" +---> CHILD BECOME SIGNLE !! [" + children_ids[c] + "] (" + cName + ")", "Detect And Download From TheGamesDB.net");
                                    }
                                }
                            }
                        }
                    }
                }

                // Progress
                progress_sub = ((roms.Count - rom_index) * 100) / roms.Count;
                status_sub = string.Format("{0} {1} / {2} ({3} MATCHED) ... {4} %",
                    ls["Status_ApplyingDatabase"], ((roms.Count - rom_index) + 1).ToString(), roms.Count,
                    matchedCount.ToString(), progress_sub);
            }
            #endregion
            #region 5 Update log with matched and not found roms
            step_index++;
            Trace.WriteLine("Finishing", "Detect And Download From TheGamesDB.net");
            status_master = "Finishing ...";
            progress_master = 100 / (steps_count - step_index);

            Trace.WriteLine("----------------------------");
            Trace.WriteLine("MATCHED ROMS ( total of " + matchedRomNames.Count + " rom(s) )");
            Trace.WriteLine("------------");
            for (int i = 0; i < matchedRomNames.Count; i++)
                Trace.WriteLine((i + 1).ToString("D8") + "." + matchedRomNames[i]);

            Trace.WriteLine("----------------------------");
            Trace.WriteLine("ROMS NOT FOUND ( total of " + notMatchedRomNames.Count + " rom(s) )");
            Trace.WriteLine("--------------");
            for (int i = 0; i < notMatchedRomNames.Count; i++)
                Trace.WriteLine((i + 1).ToString("D8") + "." + notMatchedRomNames[i]);

            Trace.WriteLine("----------------------------");

            ServicesManager.OnEnableWindowListner();
            Trace.WriteLine(string.Format("Detect And Download From TheGamesDB.net finished at {0}.", DateTime.Now.ToLocalTime()), "Detect And Download From TheGamesDB.net");
            listner.Flush();
            Trace.Listeners.Remove(listner);
            CloseAfterFinish();
            #endregion
     */
           }

        #region Comparing Methods
        /// <summary>
        /// Compare rom name with entry file names. WORKS WITH SINGLE ROMS ONLY
        /// </summary>
        /// <param name="romName"></param>
        /// <param name="entryFileNames"></param>
        /// <returns></returns>
        private bool Compare_RomName_EntryName(string romName, string entryFileNames)
        {
            bool ROMMATCH = false;
            string temp_searchWord_ROM = "";
            string temp_searchTargetText_FILE = "";
            for (int i = 0; i < romName.Length; i++)
            {
                if (romName[i] != '(' && romName[i] != '[')
                    temp_searchWord_ROM += romName[i];
                else
                    break;
            }
            for (int i = temp_searchWord_ROM.Length - 1; i >= 0; i--)
            {
                if (temp_searchWord_ROM[i] != ' ')
                {
                    if (i == temp_searchWord_ROM.Length - 1) break;//nothing to skip
                    temp_searchWord_ROM = temp_searchWord_ROM.Substring(0, i + 1);
                    break;
                }
            }
            for (int i = 0; i < entryFileNames.Length; i++)
            {
                if (entryFileNames[i] != '(' && entryFileNames[i] != '[')
                    temp_searchTargetText_FILE += entryFileNames[i];
                else
                    break;
            }
            for (int i = temp_searchTargetText_FILE.Length - 1; i >= 0; i--)
            {
                if (temp_searchTargetText_FILE[i] != ' ')
                {
                    if (i == temp_searchTargetText_FILE.Length - 1) break;//nothing to skip
                    temp_searchTargetText_FILE = temp_searchTargetText_FILE.Substring(0, i + 1);
                    break;
                }
            }
            romName = temp_searchWord_ROM;
            entryFileNames = temp_searchTargetText_FILE;

            romName = romName.Replace(":", " ");
            romName = romName.Replace("|", "");
            romName = romName.Replace(@"\", "");
            romName = romName.Replace("/", "");
            romName = romName.Replace("*", "");
            romName = romName.Replace("?", "");
            romName = romName.Replace("<", "");
            romName = romName.Replace(">", "");
            romName = romName.Replace("_", "");
            romName = romName.Replace("!", "");
            romName = romName.Replace("&", "");
            romName = romName.Replace("-", "");
            romName = romName.Replace("'", "");
            romName = romName.Replace(".", "");
            romName = romName.Replace(@"""", "");
            romName = romName.Replace(" ", "");
            string fileOfDatabase = "";
            // Check rom

            // Remove forbidden values
            fileOfDatabase = entryFileNames.Replace(":", " ");
            fileOfDatabase = fileOfDatabase.Replace("|", "");
            fileOfDatabase = fileOfDatabase.Replace(@"\", "");
            fileOfDatabase = fileOfDatabase.Replace("/", "");
            fileOfDatabase = fileOfDatabase.Replace("*", "");
            fileOfDatabase = fileOfDatabase.Replace("?", "");
            fileOfDatabase = fileOfDatabase.Replace("<", "");
            fileOfDatabase = fileOfDatabase.Replace(">", "");
            fileOfDatabase = fileOfDatabase.Replace("_", "");
            fileOfDatabase = fileOfDatabase.Replace("!", "");
            fileOfDatabase = fileOfDatabase.Replace("&", "");
            fileOfDatabase = fileOfDatabase.Replace("-", "");
            fileOfDatabase = fileOfDatabase.Replace("'", "");
            fileOfDatabase = fileOfDatabase.Replace(".", "");
            fileOfDatabase = fileOfDatabase.Replace(@"""", "");
            fileOfDatabase = fileOfDatabase.Replace(" ", "");

            if (romName == "" || fileOfDatabase == "")
                ROMMATCH = false;
            else if (romName.Length == 0 || fileOfDatabase.Length == 0)
                ROMMATCH = false;
            else if (romName.Length == fileOfDatabase.Length)
                ROMMATCH = fileOfDatabase.ToLower() == romName.ToLower();
            /*else
            {
                //  ROMMATCH = romName.ToLower().StartsWith(fileOfDatabase.ToLower());
                if (romName.Length > fileOfDatabase.Length)
                {
                    ROMMATCH = romName.ToLower().Contains(fileOfDatabase.ToLower());
                }
                else
                {
                    ROMMATCH = fileOfDatabase.ToLower().Contains(romName.ToLower());
                }
            }*/

            return ROMMATCH;
        }
        /// <summary>
        /// Compare rom FILE NAME with entry file names. If the file is compressed and "check inside archive" 
        /// is enabled, will consider the rom MATCH if only ONE file inside the archive matches IF "perfect match" 
        /// IS NOT SET, otherwise if perfect match is set, all files MUST match. 
        /// WORKS WITH SINGLE ROMS ONLY
        /// </summary>
        /// <param name="romFilePath"></param>
        /// <param name="entryFileNames"></param>
        /// <returns></returns>
        private bool Compare_RomPath_EntryName(string romFilePath, string entryFileNames)
        {
            List<string> romFiles = new List<string>();
            // Is it AI path ?
            if (HelperTools.IsAIPath(romFilePath))
            {
                SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(romFilePath)));
                int index = HelperTools.GetIndexFromAIPath(romFilePath);

                if (index < extractor.ArchiveFileNames.Count)
                {
                    romFiles.Add(Path.GetFileNameWithoutExtension(extractor.ArchiveFileNames[index]));
                }
            }
            else if (_check_inside_archive && _archive_extesnions.Contains(Path.GetExtension(romFilePath).ToLower()))
            {
                SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(romFilePath));

                foreach (string f in extractor.ArchiveFileNames)
                {
                    if (selectedConsole.Extensions.Contains(Path.GetExtension(f).ToLower()))
                        romFiles.Add(Path.GetFileNameWithoutExtension(f));
                }
            }
            else
            {
                // Normal rom, archive or not.
                romFiles.Add(Path.GetFileNameWithoutExtension(romFilePath));
            }
            int matchedNumber = 0;
            for (int i = 0; i < romFiles.Count; i++)
            {
                string fileOfRom = romFiles[i].Replace(":", " ");
                fileOfRom = fileOfRom.Replace("|", "");
                fileOfRom = fileOfRom.Replace(@"\", "");
                fileOfRom = fileOfRom.Replace("/", "");
                fileOfRom = fileOfRom.Replace("*", "");
                fileOfRom = fileOfRom.Replace("?", "");
                fileOfRom = fileOfRom.Replace("<", "");
                fileOfRom = fileOfRom.Replace(">", "");
                fileOfRom = fileOfRom.Replace("_", "");
                fileOfRom = fileOfRom.Replace("!", "");
                fileOfRom = fileOfRom.Replace("&", "");
                fileOfRom = fileOfRom.Replace("-", "");
                fileOfRom = fileOfRom.Replace("'", "");
                fileOfRom = fileOfRom.Replace(".", "");
                fileOfRom = fileOfRom.Replace(@"""", "");
                fileOfRom = fileOfRom.Replace(" ", "");
                string fileOfDatabase = "";
                bool dbEntryMatch = false;
                // Check rom

                // Remove forbidden values
                fileOfDatabase = entryFileNames.Replace(":", " ");
                fileOfDatabase = fileOfDatabase.Replace("|", "");
                fileOfDatabase = fileOfDatabase.Replace(@"\", "");
                fileOfDatabase = fileOfDatabase.Replace("/", "");
                fileOfDatabase = fileOfDatabase.Replace("*", "");
                fileOfDatabase = fileOfDatabase.Replace("?", "");
                fileOfDatabase = fileOfDatabase.Replace("<", "");
                fileOfDatabase = fileOfDatabase.Replace(">", "");
                fileOfDatabase = fileOfDatabase.Replace("_", "");
                fileOfDatabase = fileOfDatabase.Replace("!", "");
                fileOfDatabase = fileOfDatabase.Replace("&", "");
                fileOfDatabase = fileOfDatabase.Replace("-", "");
                fileOfDatabase = fileOfDatabase.Replace("'", "");
                fileOfDatabase = fileOfDatabase.Replace(".", "");
                fileOfDatabase = fileOfDatabase.Replace(@"""", "");
                fileOfDatabase = fileOfDatabase.Replace(" ", "");

                if (fileOfRom == "" || fileOfDatabase == "")
                {
                    // Do nothing, we needed this check anyway ...
                }
                else if (fileOfRom.Length == 0 || fileOfDatabase.Length == 0)
                {
                    // Do nothing, we needed this check anyway ...
                }
                else if (fileOfRom.Length == fileOfDatabase.Length)
                {
                    if (fileOfDatabase.ToLower() == fileOfRom.ToLower())
                    { matchedNumber++; dbEntryMatch = true; }
                }
                /*else
                {
                    if (fileOfRom.Length > fileOfDatabase.Length)
                    {
                        if (fileOfRom.ToLower().Contains(fileOfDatabase.ToLower()))
                        { matchedNumber++; dbEntryMatch = true; }
                    }
                    else
                    {
                        if (fileOfDatabase.ToLower().Contains(fileOfRom.ToLower()))
                        { matchedNumber++; dbEntryMatch = true; }
                    }
                }*/
            }
            //   if (_perfect_match)
            //       return matchedNumber == romFiles.Count && matchedNumber > 0;
            //   else
            return matchedNumber > 0;
        }

        /*private void COMPARE_ROM(Rom rom, bool applyName, ref List<GameSearchResult> databaseEntries,
            ref List<string> matchedRomNames, ref List<string> notMatchedRomNames,
            ref int matchedCount, out bool rom_matched, bool doDelete)
        {
            bool ROMMATCH = false;
            Game matchedEntry = null;

            // Loop through database entries looking for a match
            for (int entry_index = databaseEntries.Count - 1; entry_index >= 0; entry_index--)
            {
                if (_compare_name_name)
                {
                    ROMMATCH = Compare_RomName_EntryName(rom.Name,
                         databaseEntries[entry_index].Title);
                }
                else if (_compare_fileName_name)
                {
                    ROMMATCH = Compare_RomPath_EntryName(rom.Path,
                        databaseEntries[entry_index].Title);
                }

                if (ROMMATCH)
                {
                    matchedEntry = GamesDB.GetGame(databaseEntries[entry_index].ID);
                    if (_turbo_speed)
                        databaseEntries.RemoveAt(entry_index);
                    break;
                }
            }

            rom_matched = ROMMATCH;
            if (ROMMATCH)
            {
                Trace.WriteLine("ROM MATCHED [" + rom.ID + "] (" + rom.Name + ")", "Detect And Download From TheGamesDB.net");
                matchedRomNames.Add(rom.Name);
                if (applyName)
                {
                    // Database info items
                    ApplyRom(rom, matchedEntry);
                    Trace.WriteLine("ROM DATA UPDATED.", "Detect And Download From TheGamesDB.net");
                }
                matchedCount++;
            }
            else
            {
                if (doDelete)
                    DeleteRomRoutin(rom, ref notMatchedRomNames);
            }
        }*/
        private void AddNewIC(string newic)
        {
            //search the profile for information container that match the same name
            bool found = false;
            foreach (RomData ic in profileManager.Profile.Consoles[consoleID].RomDataInfoElements)
            {
                if (ic.Name.ToLower() == newic.ToLower())
                {
                    found = true; break;
                }
            }
            if (!found)
            {
                //add new ic with this project
                RomData newIC = new RomData(profileManager.Profile.GenerateID(), newic, RomDataType.Text);
                profileManager.Profile.Consoles[consoleID].RomDataInfoElements.Add(newIC);
                Trace.WriteLine(@"/!\ Rom data info element added to console: " + newic);
            }
        }
        private void AddNewInfoTextTAB(string newTabName)
        {
            //search the profile for information container that match the same name
            bool found = false;
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.Name.ToLower() == newTabName.ToLower())
                {
                    found = true; ShowTab(ic.ID); break;
                }
            }
            if (!found)
            {
                //add new ic with this project
                InformationContainerInfoText newIC = new InformationContainerInfoText(
                    profileManager.Profile.GenerateID());
                newIC.Name = newTabName;
                newIC.DisplayName = newTabName;
                profileManager.Profile.Consoles[consoleID].InformationContainers.Add(newIC);
                ShowTab(newIC.ID);
                Trace.WriteLine(@"/!\ Information Container (Tab) added to console: " + newTabName);
            }
        }
        private void AddNewImageTAB(string newTabName, string folder)
        {
            //search the profile for information container that match the same name
            bool found = false;
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.Name.ToLower() == newTabName.ToLower())
                {
                    found = true; ShowTab(ic.ID); break;
                }
            }
            if (!found)
            {
                //add new ic with this project
                InformationContainerImage newIC = new InformationContainerImage(
                    profileManager.Profile.GenerateID());
                newIC.Name = newTabName;
                newIC.DisplayName = newTabName;
                if (newIC.FoldersMemory == null)
                    newIC.FoldersMemory = new List<string>();
                if (!newIC.FoldersMemory.Contains(folder))
                    newIC.FoldersMemory.Add(folder);
                profileManager.Profile.Consoles[consoleID].InformationContainers.Add(newIC);
                ShowTab(newIC.ID);
                Trace.WriteLine(@"/!\ Information Container (Tab) added to console: " + newTabName);
            }
        }
        private void AddDataToRom(string icName, string data, Rom rom)
        {
            foreach (RomData ic in profileManager.Profile.Consoles[consoleID].RomDataInfoElements)
            {
                if (ic.Name.ToLower() == icName.ToLower())
                {
                    rom.UpdateDataInfoItemValue(ic.ID, data);
                    Trace.WriteLine("--> Rom data info '" + icName + "' updated with -> " + data);
                    break;
                }
            }
        }
        private void AddTabConentFilesToRom(string tabName, string downloads_folder, List<string> links, Rom rom, bool clearOldList)
        {
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.DisplayName.ToLower() == tabName.ToLower())
                {
                    // Download the files
                    Trace.WriteLine(string.Format("Downloading files for '{0}'", tabName), "Detect And Download From TheGamesDB.net");
                    string NameOfSavedFiles = tabName + "-" + Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)));

                    int c = 1;
                    List<string> filesToAdd = new List<string>();
                    foreach (string link in links)
                    {
                        // Try downloading
                        try
                        {
                            Trace.WriteLine(string.Format("Downloading file from '{0}'", link), "Detect And Download From TheGamesDB.net");

                            Uri uri = new Uri(link);
                            string[] splited = link.Split(new char[] { '/' });
                            string ext = Path.GetExtension(splited[splited.Length - 1]);
                            int j = 0;
                            while (File.Exists(Path.GetFullPath(downloads_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext)))
                                j++;

                            client.DownloadFile(uri, Path.GetFullPath(downloads_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext));

                            filesToAdd.Add(downloads_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext);

                            status_sub_sub = string.Format("[Downloading file {0} of {1} from {2}]", c, links.Count, link);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(string.Format("XXX Unable to download file from '{0}'; {1}", link, ex.Message), "Detect And Download From TheGamesDB.net");
                        }
                        c++;
                    }
                    if (rom.RomInfoItems != null)
                    {
                        if (!rom.IsInformationContainerItemExist(ic.ID))
                        {
                            // Create new
                            InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                            if (clearOldList)
                                item.Files = new List<string>();
                            item.Files.AddRange(filesToAdd);
                            rom.RomInfoItems.Add(item);
                            rom.Modified = true;
                        }
                        else
                        {
                            // Update
                            foreach (InformationContainerItem item in rom.RomInfoItems)
                            {
                                if (item.ParentID == ic.ID)
                                {
                                    InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                                    if (ictem.Files == null)
                                        ictem.Files = new List<string>();
                                    if (clearOldList)
                                        ictem.Files = new List<string>();
                                    foreach (string f in filesToAdd)
                                    {
                                        if (!ictem.Files.Contains(f))
                                            ictem.Files.Add(f);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        rom.RomInfoItems = new List<InformationContainerItem>();
                        // Create new
                        InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                        item.Files.AddRange(filesToAdd);
                        rom.RomInfoItems.Add(item);
                        rom.Modified = true;
                    }
                    break;
                }
            }

        }
        private void AddTabConentFilesToRom(string tabName, string folder, string file_content, Rom rom, bool clearOldList)
        {
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.DisplayName.ToLower() == tabName.ToLower())
                {
                    // Get the files
                    string fileToAdd = tabName + "-" + Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)))
                        + ".txt";
                    fileToAdd = Path.Combine(folder, fileToAdd);
                    int i = 1;
                    while (File.Exists(fileToAdd))
                    {
                        i++;
                        fileToAdd = tabName + "-" + Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)))
                          + "_" + i + ".txt";
                        fileToAdd = Path.Combine(folder, fileToAdd);
                    }
                    try
                    {
                        status_sub_sub = string.Format("[Saving file at {0} for {1}]", fileToAdd, tabName);
                        File.WriteAllText(fileToAdd, file_content);
                        Trace.WriteLine(string.Format("->File saved for '{0}' at '{1}'", tabName, fileToAdd), "Detect And Download From TheGamesDB.net");

                        if (rom.RomInfoItems != null)
                        {
                            if (!rom.IsInformationContainerItemExist(ic.ID))
                            {
                                // Create new
                                InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                                if (clearOldList)
                                    item.Files = new List<string>();
                                item.Files.Add(fileToAdd);
                                rom.RomInfoItems.Add(item);
                                rom.Modified = true;
                            }
                            else
                            {
                                // Update
                                foreach (InformationContainerItem item in rom.RomInfoItems)
                                {
                                    if (item.ParentID == ic.ID)
                                    {
                                        InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                                        if (ictem.Files == null)
                                            ictem.Files = new List<string>();
                                        if (clearOldList)
                                            ictem.Files = new List<string>();
                                        if (!ictem.Files.Contains(fileToAdd))
                                            ictem.Files.Add(fileToAdd);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            rom.RomInfoItems = new List<InformationContainerItem>();
                            // Create new
                            InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                            item.Files.Add(fileToAdd);
                            rom.RomInfoItems.Add(item);
                            rom.Modified = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(string.Format("XXX Unable to save file for '{0}' at '{1}'; " + ex.Message, tabName, fileToAdd));
                    }
                    break;
                }
            }

        }
        /*private void ApplyRom(Rom rom, Game game)
        {
            if (_db_rename_rom_using_title)
            {
                string old_rom_name = rom.Name;
                profileManager.Profile.Roms[rom.ID].Name = game.Title;
                Trace.WriteLine(string.Format("->Rom renamed from {0} to {1}", old_rom_name, game.Title), "Detect And Download From TheGamesDB.net");
            }
            if (_db_add_elements_as_romdata)
            {
                AddNewIC("Platform");
                AddDataToRom("Platform", game.Platform, profileManager.Profile.Roms[rom.ID]);
                AddNewIC("ReleaseDate");
                AddDataToRom("ReleaseDate", game.ReleaseDate, profileManager.Profile.Roms[rom.ID]);
                if (!_db_add_overview_as_tab)
                {
                    AddNewIC("Overview");
                    AddDataToRom("Overview", game.Overview, profileManager.Profile.Roms[rom.ID]);
                }
                AddNewIC("ESRB");
                AddDataToRom("ESRB", game.ESRB, profileManager.Profile.Roms[rom.ID]);
                AddNewIC("Players");
                AddDataToRom("Players", game.Players, profileManager.Profile.Roms[rom.ID]);
                AddNewIC("Publisher");
                AddDataToRom("Publisher", game.Publisher, profileManager.Profile.Roms[rom.ID]);
                AddNewIC("Developer");
                AddDataToRom("Developer", game.Developer, profileManager.Profile.Roms[rom.ID]);
                AddNewIC("Rating");
                AddDataToRom("Rating", game.Rating, profileManager.Profile.Roms[rom.ID]);

                AddNewIC("AlternateTitles");
                string titles = "";
                foreach (string t in game.AlternateTitles)
                    titles += t + ", ";
                AddDataToRom("AlternateTitles", titles, profileManager.Profile.Roms[rom.ID]);

                AddNewIC("Genres");
                string genres = "";
                foreach (string t in game.Genres)
                    genres += t + ", ";
                AddDataToRom("Genres", genres, profileManager.Profile.Roms[rom.ID]);
            }
            #region Overview
            if (_db_add_overview_as_tab)
            {
                if (_db_create_new_tab_for_overview)
                {
                    // Create new tab for it !!
                    Trace.WriteLine("Adding 'Overview' tab ....", "Detect And Download From TheGamesDB.net");
                    AddNewInfoTextTAB("Overview");
                    // Download the tabs for it !
                    AddTabConentFilesToRom("Overview", _db_overview_folder, game.Overview, rom, _db_overview_ic_clearlist);
                }
                else
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_overview_ic_id).DisplayName;
                    // Download the tabs for it !
                    AddTabConentFilesToRom(tabName, _db_overview_folder, game.Overview, rom, _db_overview_ic_clearlist);
                }
            }
            #endregion
            #region Banners
            if (_db_add_banners_as_tab)
            {
                if (game.Images.Banners != null)
                {
                    if (game.Images.Banners.Count > 0)
                    {
                        // Download the tabs for it !
                        List<string> links = new List<string>();
                        for (int i = 0; i < game.Images.Banners.Count; i++)
                        {
                            links.Add(GamesDB.BaseImgURL + game.Images.Banners[i].Path);
                            if (_db_banners_ic_limitdownload)
                                break;// one link added so far.
                        }
                        if (_db_create_new_tab_for_banners)
                        {
                            // Create new tab for it !!
                            AddNewImageTAB("Banners", _db_banners_folder);

                            AddTabConentFilesToRom("Banners", _db_banners_folder, links, rom, _db_banners_ic_clearlist);
                        }
                        else
                        {
                            // Assign to a tab !
                            string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_banners_ic_id).DisplayName;
                            // Download the tabs for it !
                            AddTabConentFilesToRom(tabName, _db_banners_folder, links, rom, _db_banners_ic_clearlist);
                        }
                    }
                }
            }
            #endregion
            #region Screenshots
            if (_db_add_screenshots_as_tab)
            {
                if (game.Images.Screenshots != null)
                {
                    if (game.Images.Screenshots.Count > 0)
                    {
                        // Download the tabs for it !
                        List<string> links = new List<string>();
                        for (int i = 0; i < game.Images.Screenshots.Count; i++)
                        {
                            links.Add(GamesDB.BaseImgURL + game.Images.Screenshots[i].Path);
                            if (_db_screenshots_ic_limitdownload)
                                break;// one link added so far.
                        }
                        if (_db_create_new_tab_for_screenshots)
                        {
                            // Create new tab for it !!
                            AddNewImageTAB("Screenshots", _db_screenshots_folder);

                            AddTabConentFilesToRom("Screenshots", _db_screenshots_folder, links, rom, _db_screenshots_ic_clearlist);
                        }
                        else
                        {
                            // Assign to a tab !
                            string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_screenshots_ic_id).DisplayName;
                            // Download the tabs for it !
                            AddTabConentFilesToRom(tabName, _db_screenshots_folder, links, rom, _db_screenshots_ic_clearlist);
                        }
                    }
                }
            }
            #endregion
            #region Fanart
            if (_db_add_fanart_as_tab)
            {
                if (game.Images.Fanart != null)
                {
                    if (game.Images.Fanart.Count > 0)
                    {
                        // Download the tabs for it !
                        List<string> links = new List<string>();
                        for (int i = 0; i < game.Images.Fanart.Count; i++)
                        {
                            links.Add(GamesDB.BaseImgURL + game.Images.Fanart[i].Path);
                            if (_db_fanart_ic_limitdownload)
                                break;// one link added so far.
                        }
                        if (_db_create_new_tab_for_fanart)
                        {
                            // Create new tab for it !!
                            AddNewImageTAB("Fanart", _db_fanart_folder);

                            AddTabConentFilesToRom("Fanart", _db_fanart_folder, links, rom, _db_fanart_ic_clearlist);
                        }
                        else
                        {
                            // Assign to a tab !
                            string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_fanart_ic_id).DisplayName;
                            // Download the tabs for it !
                            AddTabConentFilesToRom(tabName, _db_fanart_folder, links, rom, _db_fanart_ic_clearlist);
                        }
                    }
                }
            }
            #endregion
            #region Boxart back
            if (_db_add_boxart_back_as_tab)
            {
                if (game.Images.BoxartBack != null)
                {
                    // Download the tabs for it !
                    List<string> links = new List<string>();
                    links.Add(GamesDB.BaseImgURL + game.Images.BoxartBack.Path);

                    if (_db_create_new_tab_for_boxart_back)
                    {
                        // Create new tab for it !!
                        AddNewImageTAB("Boxart Back", _db_boxart_back_folder);

                        AddTabConentFilesToRom("Boxart Back", _db_boxart_back_folder, links, rom, _db_boxart_back_ic_clearlist);
                    }
                    else
                    {
                        // Assign to a tab !
                        string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_boxart_back_ic_id).DisplayName;
                        // Download the tabs for it !
                        AddTabConentFilesToRom(tabName, _db_boxart_back_folder, links, rom, _db_boxart_back_ic_clearlist);
                    }

                }
            }
            #endregion
            #region Boxart Front
            if (_db_add_boxart_front_as_tab)
            {
                if (game.Images.BoxartFront != null)
                {
                    // Download the tabs for it !
                    List<string> links = new List<string>();
                    links.Add(GamesDB.BaseImgURL + game.Images.BoxartFront.Path);

                    if (_db_create_new_tab_for_boxart_front)
                    {
                        // Create new tab for it !!
                        AddNewImageTAB("Boxart Front", _db_boxart_front_folder);

                        AddTabConentFilesToRom("Boxart Front", _db_boxart_front_folder, links, rom, _db_boxart_front_ic_clearlist);
                    }
                    else
                    {
                        // Assign to a tab !
                        string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_boxart_front_ic_id).DisplayName;
                        // Download the tabs for it !
                        AddTabConentFilesToRom(tabName, _db_boxart_front_folder, links, rom, _db_boxart_front_ic_clearlist);
                    }

                }
            }
            #endregion
            #region Clear Logo
            if (_db_add_clearlogo_as_tab)
            {
                List<string> links = new List<string>();
                if (game.Images.ClearLogo != null)
                {
                    // Download the tabs for it !
                    links.Add(GamesDB.BaseImgURL + game.Images.ClearLogo.Path);
                }
                if (_db_create_new_tab_for_clearlogo)
                {
                    // Create new tab for it !!
                    AddNewImageTAB("Clear Logo", _db_clearlogo_folder);
                    if (links.Count > 0)
                        AddTabConentFilesToRom("Clear Logo", _db_clearlogo_folder, links, rom, _db_clearlogo_ic_clearlist);
                }
                else if (links.Count > 0)
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_clearlogo_ic_id).DisplayName;
                    // Download the tabs for it !

                    AddTabConentFilesToRom(tabName, _db_clearlogo_folder, links, rom, _db_clearlogo_ic_clearlist);
                }
            }
            #endregion
        }*/
        private void DeleteRomRoutin(Rom rom, ref List<string> notMatchedRomNames)
        {
            notMatchedRomNames.Add(rom.Name);
            if (_delete_rom_not_found)
            {
                Trace.WriteLine(" !--> Deleting rom from database", "Detect And Download From TheGamesDB.net");
                Trace.WriteLine(" +---> Removing rom: [" + rom.ID + "] " + rom.Name, "Detect And Download From TheGamesDB.net");
                profileManager.Profile.Roms.Remove(rom.ID, false);
                // Delete from disk
                if (_delete_rom_not_found_file && !HelperTools.IsAIPath(rom.Path))
                {
                    Trace.WriteLine(" +----> Removing rom file: " + rom.Path, "Detect And Download From TheGamesDB.net");
                    try
                    {
                        File.Delete(HelperTools.GetFullPath(rom.Path));
                        Trace.WriteLine(" +----> File removed: " + rom.Path, "Detect And Download From TheGamesDB.net");
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(" +---->> X UNABLE to remove rom file: " + rom.Path, "Detect And Download From TheGamesDB.net");
                        Trace.WriteLine(" +---->> X " + ex.Message, "Detect And Download From TheGamesDB.net");
                    }
                }
                // Delete related files !
                if (_delete_rom_not_found_related_files)
                {
                    Trace.WriteLine(" +---> Removing rom related files ... ", "Detect And Download From TheGamesDB.net");
                    if (rom.RomInfoItems != null)
                    {
                        foreach (InformationContainerItem rinf in rom.RomInfoItems)
                        {
                            if (rinf is InformationContainerItemFiles)
                            {
                                if (((InformationContainerItemFiles)rinf).Files != null)
                                {
                                    foreach (string rr in ((InformationContainerItemFiles)rinf).Files)
                                    {
                                        try
                                        {
                                            File.Delete(HelperTools.GetFullPath(rr));
                                            Trace.WriteLine(" +----> File removed: " + rr, "Detect And Download From TheGamesDB.net");
                                        }
                                        catch (Exception ex)
                                        {
                                            Trace.WriteLine(" +---->> X UNABLE to remove file: " + rr, "Detect And Download From TheGamesDB.net");
                                            Trace.WriteLine(" +---->>" + ex.Message, "Detect And Download From TheGamesDB.net");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Trace.WriteLine(">Rom has no related file.", "Import CSV database");
                    }
                }
            }
        }
        private void ShowTab(string tabID)
        {
            bool visible = selectedConsole.InformationContainersMap.IsContainerVisible(tabID);
            if (visible)
            {
                // Already visible
                return;
            }
            else
            {
                // Add it to map if not exist
                if (!selectedConsole.InformationContainersMap.AddNewContainerID(tabID))
                {
                    if (selectedConsole.InformationContainersMap.ContainerIDS == null)
                        selectedConsole.InformationContainersMap.ContainerIDS = new List<string>();
                    selectedConsole.InformationContainersMap.ContainerIDS.Add(tabID);
                }

            }
            // Event for refresh request
            profileManager.Profile.OnInformationContainerVisibiltyChanged();
        }
        #endregion
        private void CloseAfterFinish()
        {
            if (!this.InvokeRequired)
                CloseAfterFinish1();
            else
                this.Invoke(new Action(CloseAfterFinish1));
        }
        private void CloseAfterFinish1()
        {
            finished = true;
            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(ls["Message_Done"] + ", " + ls["Message_LogFileSavedTo"] + " '" + logPath + "'",
          ls["MessageCaption_DetectAndDownloadFromTheGamesDB"], new string[] { ls["Button_Ok"], ls["Button_OpenLog"] },
          0, null, ManagedMessageBoxIcon.Info);
            if (res.ClickedButtonIndex == 1)
            {
                try { Process.Start(HelperTools.GetFullPath(logPath)); }
                catch (Exception ex)
                { ManagedMessageBox.ShowErrorMessage(ex.Message); }
            }
            profileManager.Profile.OnDatabaseImported();

            this.Close();
            Services.ServicesManager.OnEnableWindowListner();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_overview_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_overview_folder.Text = fol.SelectedPath;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_boxart_back_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_boxart_back_folder.Text = fol.SelectedPath;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_boxart_front_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_boxart_front_folder.Text = fol.SelectedPath;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_Fanart_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Fanart_folder.Text = fol.SelectedPath;
            }
        }
        private void button_banners_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_Banners_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Banners_folder.Text = fol.SelectedPath;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_Screenshots_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Screenshots_folder.Text = fol.SelectedPath;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            checkBox_add_Banners.Checked =
           checkBox_add_boxart_back.Checked =
           checkBox_add_boxart_front.Checked =
           //checkBox_add_datainfor_elemnts.Checked =
           checkBox_add_Fanart.Checked =
           checkBox_add_overview.Checked =
           checkBox_add_Screenshots.Checked =
           checkBox_clearlogo_addastab.Checked =

           checkBox_create_new_tab_boxart_front.Checked =
           checkBox_create_new_tab_boxat_back.Checked =
           checkBox_create_new_tab_overview.Checked =
           checkBox_create_tab_Banners.Checked =
           checkBox_create_tab_for_Fanart.Checked =
           checkBox_create_tab_Screenshots.Checked =
           checkBox_clearlogo_create_new_tab.Checked =
           true;
            checkBox_rename_console_with_platform.Checked = false;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.Description = "Please select a master folder where to save the downloaded files (empty folder is recommended)";

            if (selectedConsole.Memory_RomFolders != null)
            {
                if (selectedConsole.Memory_RomFolders.Count > 0)
                {
                    fol.SelectedPath = selectedConsole.Memory_RomFolders[selectedConsole.Memory_RomFolders.Count - 1];
                }
            }
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage("These folders will be created :"
                   + "\n" + fol.SelectedPath + "\\Overview\\"
                    + "\n" + fol.SelectedPath + "\\Banners\\"
                    + "\n" + fol.SelectedPath + "\\BoxartBack\\"
                    + "\n" + fol.SelectedPath + "\\BoxartFront\\"
                    + "\n" + fol.SelectedPath + "\\Fanart\\"
                    + "\n" + fol.SelectedPath + "\\Screenshots\\"
                    + "\n" + fol.SelectedPath + "\\Clear Logo\\"
                    + "\n" + "\nConinue ?");
                if (res.ClickedButtonIndex == 0)
                {
                    textBox_overview_folder.Text = fol.SelectedPath + "\\Overview\\";
                    textBox_Banners_folder.Text = fol.SelectedPath + "\\Banners\\";
                    textBox_boxart_back_folder.Text = fol.SelectedPath + "\\BoxartBack\\";
                    textBox_boxart_front_folder.Text = fol.SelectedPath + "\\BoxartFront\\";
                    textBox_Fanart_folder.Text = fol.SelectedPath + "\\Fanart\\";
                    textBox_Screenshots_folder.Text = fol.SelectedPath + "\\Screenshots\\";
                    textBox_clearlogo_folder.Text = fol.SelectedPath + "\\Clear Logo\\";
                    Directory.CreateDirectory(textBox_overview_folder.Text);
                    Directory.CreateDirectory(textBox_Banners_folder.Text);
                    Directory.CreateDirectory(textBox_boxart_back_folder.Text);
                    Directory.CreateDirectory(textBox_boxart_front_folder.Text);
                    Directory.CreateDirectory(textBox_Fanart_folder.Text);
                    Directory.CreateDirectory(textBox_Screenshots_folder.Text);
                    Directory.CreateDirectory(textBox_clearlogo_folder.Text);
                }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.thegamesdb.net/");
            }
            catch { }
        }

        // Reset archive extensions
        private void reset_archive_extensions_Click(object sender, EventArgs e)
        {
            // Fill up archive extensions
            string exs = "";
            foreach (string ex in profileManager.Profile.Consoles[consoleID].ArchiveExtensions)
                exs += ex + ";";
            if (exs.Length > 1)
                textBox_archive_extesnions.Text = exs.Substring(0, exs.Length - 1);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status_master.Text = status_master;
            label_status_sub.Text = status_sub + " " + status_sub_sub;
            progressBar_master.Value = progress_master;
            progressBar_slave.Value = progress_sub;
        }
        private void checkBox_delete_roms_nott_found_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_delete_not_matched_files.Enabled =
        checkBox_delete_not_matched_related.Enabled =
        groupBox_parent_match.Enabled =
        groupBox_parent_not_match.Enabled =
        checkBox_delete_roms_nott_found.Checked;
        }
        private void Form_DetectAndDownloadFromTheGamesDB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_DetectAndDownloadFromTheGamesDB"]);
                        if (result.ClickedButtonIndex == 0)
                        {
                            client.CancelAsync();
                            mainThread.Abort();
                            mainThread = null;
                            ServicesManager.OnEnableWindowListner();
                            Trace.WriteLine("Database import operation finished at " + DateTime.Now.ToLocalTime(), "Detect And Download From TheGamesDB.net");
                            listner.Flush();
                            Trace.Listeners.Remove(listner);
                            CloseAfterFinish();
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }
        // START !!
        private void button2_Click(object sender, EventArgs e)
        {
            // Make checks
            if (comboBox_platforms.SelectedIndex < 0)
            {
                ManagedMessageBox.ShowErrorMessage("Please select a platform first.");
                return;
            }
            if (checkBox_add_Banners.Checked)
            {
                if (!Directory.Exists(textBox_Banners_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Banners' is not exist.");
                    button_banners_Click(null, null);
                    return;
                }
                if (!checkBox_create_tab_Banners.Checked)
                {
                    if (comboBox_Banners_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Banners first.");
                        return;
                    }
                }
            }
            if (checkBox_add_boxart_back.Checked)
            {
                if (!Directory.Exists(textBox_boxart_back_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Boxart Back' is not exist.");
                    button4_Click(null, null);
                    return;
                }
                if (!checkBox_create_new_tab_boxat_back.Checked)
                {
                    if (comboBox_boxart_bakc_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Boxart Back first.");
                        return;
                    }
                }
            }
            if (checkBox_add_boxart_front.Checked)
            {
                if (!Directory.Exists(textBox_boxart_front_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Boxart Front' is not exist.");
                    button5_Click(null, null);
                    return;
                }
                if (!checkBox_create_new_tab_boxart_front.Checked)
                {
                    if (comboBox_boxart_front_tab.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for boxart Front first.");
                        return;
                    }
                }
            }
            if (checkBox_add_Fanart.Checked)
            {
                if (!Directory.Exists(textBox_Fanart_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Fanart' is not exist.");
                    button6_Click(null, null);
                    return;
                }
                if (!checkBox_create_tab_for_Fanart.Checked)
                {
                    if (comboBox_Fanart_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Fanart first.");
                        return;
                    }
                }
            }
            if (checkBox_add_overview.Checked)
            {
                if (!Directory.Exists(textBox_overview_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Overview' is not exist.");
                    button1_Click_1(null, null);
                    return;
                }
                if (!checkBox_create_new_tab_overview.Checked)
                {
                    if (comboBox_overview_tab.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Overview first.");
                        return;
                    }
                }
            }
            if (checkBox_add_Screenshots.Checked)
            {
                if (!Directory.Exists(textBox_overview_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Screenshots' is not exist.");
                    button8_Click(null, null);
                    return;
                }
                if (!checkBox_create_tab_Screenshots.Checked)
                {
                    if (comboBox_Screenshots_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Screenshots first.");
                        return;
                    }
                }
            }
            if (checkBox_clearlogo_addastab.Checked)
            {
                if (!Directory.Exists(textBox_clearlogo_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Clear Logo' is not exist.");
                    button16_Click(null, null);
                    return;
                }
                if (!checkBox_clearlogo_create_new_tab.Checked)
                {
                    if (comboBox_clearlogo_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Clear Logo first.");
                        return;
                    }
                }
            }
            // Poke options ...
            _compare_name_name = checkBox_compare_using_rom_name.Checked;
            _compare_fileName_name = !checkBox_compare_using_rom_name.Checked;

            _turbo_speed = checkBox_turboe.Checked;
            _check_inside_archive = checkBox_check_inside_archive.Checked;
            _archive_extesnions = new List<string>(textBox_archive_extesnions.Text.Split(';'));
            _delete_rom_not_found = checkBox_delete_roms_nott_found.Checked;
            _delete_rom_not_found_file = checkBox_delete_not_matched_files.Checked;
            _delete_rom_not_found_related_files = checkBox_delete_not_matched_related.Checked;
            // Apply format stuff
            //_db_selected_platform_id = ((PlatformSearchResult)comboBox_platforms.SelectedItem).ID;
            _rename_parent = checkBox_rename_parent.Checked;
            _parent_match_keep_all_children = radioButton_parent_match_keep_all_children.Checked;
            _parent_match_keep_child_match = radioButton_parent_matche_keep_matched_only.Checked;
            _parent_not_match_keep_on_one_child_match = radioButton_parent_not_match_keep_parent_if_one_child_match.Checked;
            _parent_not_match_make_matched_children_singles = radioButton_parent_not_mathc_free_matched_children.Checked;
            _parent_not_match_keep_on_one_child_match_dont_keep_not_mathced_children = checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.Checked;
            _db_rename_console = checkBox_rename_console_with_platform.Checked;
            _db_rename_rom_using_title = checkBox_rename_rom.Checked;
            _db_add_elements_as_romdata = checkBox_add_datainfor_elemnts.Checked;

            _db_add_overview_as_tab = checkBox_add_overview.Checked;
            _db_add_banners_as_tab = checkBox_add_Banners.Checked;
            _db_add_boxart_front_as_tab = checkBox_add_boxart_front.Checked;
            _db_add_boxart_back_as_tab = checkBox_add_boxart_back.Checked;
            _db_add_fanart_as_tab = checkBox_add_Fanart.Checked;
            _db_add_screenshots_as_tab = checkBox_add_Screenshots.Checked;
            _db_add_clearlogo_as_tab = checkBox_clearlogo_addastab.Checked;

            _db_create_new_tab_for_overview = checkBox_create_new_tab_overview.Checked;
            _db_create_new_tab_for_banners = checkBox_create_tab_Banners.Checked;
            _db_create_new_tab_for_boxart_front = checkBox_create_new_tab_boxart_front.Checked;
            _db_create_new_tab_for_boxart_back = checkBox_create_new_tab_boxat_back.Checked;
            _db_create_new_tab_for_fanart = checkBox_create_tab_for_Fanart.Checked;
            _db_create_new_tab_for_screenshots = checkBox_create_tab_Screenshots.Checked;
            _db_create_new_tab_for_clearlogo = checkBox_clearlogo_create_new_tab.Checked;

            _db_overview_folder = textBox_overview_folder.Text;
            _db_banners_folder = textBox_Banners_folder.Text;
            _db_boxart_front_folder = textBox_boxart_front_folder.Text;
            _db_boxart_back_folder = textBox_boxart_back_folder.Text;
            _db_fanart_folder = textBox_Fanart_folder.Text;
            _db_screenshots_folder = textBox_Screenshots_folder.Text;
            _db_clearlogo_folder = textBox_clearlogo_folder.Text;

            if (checkBox_add_overview.Checked && !_db_create_new_tab_for_overview)
                _db_overview_ic_id = ((InformationContainer)comboBox_overview_tab.SelectedItem).ID;
            if (checkBox_add_Banners.Checked && !_db_create_new_tab_for_banners)
                _db_banners_ic_id = ((InformationContainer)comboBox_Banners_tabs.SelectedItem).ID;
            if (checkBox_add_boxart_front.Checked && !_db_create_new_tab_for_boxart_front)
                _db_boxart_front_ic_id = ((InformationContainer)comboBox_boxart_front_tab.SelectedItem).ID;
            if (checkBox_add_boxart_back.Checked && !_db_create_new_tab_for_boxart_back)
                _db_boxart_back_ic_id = ((InformationContainer)comboBox_boxart_bakc_tabs.SelectedItem).ID;
            if (checkBox_add_Fanart.Checked && !_db_create_new_tab_for_fanart)
                _db_fanart_ic_id = ((InformationContainer)comboBox_Fanart_tabs.SelectedItem).ID;
            if (checkBox_add_Screenshots.Checked && !_db_create_new_tab_for_screenshots)
                _db_screenshots_ic_id = ((InformationContainer)comboBox_Screenshots_tabs.SelectedItem).ID;
            if (!_db_create_new_tab_for_clearlogo)
                _db_clearlogo_ic_id = ((InformationContainer)comboBox_clearlogo_tabs.SelectedItem).ID;

            _db_overview_ic_clearlist = checkBox_overview_clearlist.Checked;
            _db_banners_ic_clearlist = checkBox_banners_clear_list.Checked;
            _db_boxart_front_ic_clearlist = checkBox_boxart_front_clearlist.Checked;
            _db_boxart_back_ic_clearlist = checkBox_boxart_back_clearlist.Checked;
            _db_fanart_ic_clearlist = checkBox_fanart_clearlist.Checked;
            _db_screenshots_ic_clearlist = checkBox_screenshots_clearlist.Checked;
            _db_clearlogo_ic_clearlist = checkBox_clearlogo_clear_files_first.Checked;

            _db_fanart_ic_limitdownload = checkBox_limit_download_fanart.Checked;
            _db_screenshots_ic_limitdownload = checkBox_limit_download_screenshots.Checked;
            _db_banners_ic_limitdownload = checkBox_limit_download_banners.Checked;
            // Disable everything
            groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = tabControl1.Enabled =
            groupBox_general.Enabled = button_set_all.Enabled = button_set_master_folder.Enabled = false;
            button_start.Enabled = groupBox6.Enabled = false;
            // Start timer
            timer1.Start();
            finished = false;
            // Start thread !
            mainThread = new Thread(new ThreadStart(PROCESS));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox_boxart_bakc_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected id
            if (comboBox_boxart_bakc_tabs.SelectedIndex >= 0)
            {
                string id = ((InformationContainer)comboBox_boxart_bakc_tabs.SelectedItem).ID;

                // Checkout others
                if (comboBox_Banners_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Banners_tabs.SelectedItem).ID == id)
                        comboBox_Banners_tabs.SelectedIndex = -1;
                if (comboBox_boxart_front_tab.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_front_tab.SelectedItem).ID == id)
                        comboBox_boxart_front_tab.SelectedIndex = -1;
                if (comboBox_Fanart_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Fanart_tabs.SelectedItem).ID == id)
                        comboBox_Fanart_tabs.SelectedIndex = -1;
                if (comboBox_Screenshots_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Screenshots_tabs.SelectedItem).ID == id)
                        comboBox_Screenshots_tabs.SelectedIndex = -1;

                // Make folder
                //  if (textBox_boxart_back_folder.Text.Length == 0)
                {
                    InformationContainerImage imgCont = (InformationContainerImage)comboBox_boxart_bakc_tabs.SelectedItem;
                    if (imgCont.FoldersMemory != null)
                        if (imgCont.FoldersMemory.Count > 0)
                            textBox_boxart_back_folder.Text = HelperTools.GetFullPath(imgCont.FoldersMemory[0]);
                }
            }
        }
        private void comboBox_boxart_front_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected id
            if (comboBox_boxart_front_tab.SelectedIndex >= 0)
            {
                string id = ((InformationContainer)comboBox_boxart_front_tab.SelectedItem).ID;
                // Checkout others
                if (comboBox_Banners_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Banners_tabs.SelectedItem).ID == id)
                        comboBox_Banners_tabs.SelectedIndex = -1;
                if (comboBox_boxart_bakc_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_bakc_tabs.SelectedItem).ID == id)
                        comboBox_boxart_bakc_tabs.SelectedIndex = -1;
                if (comboBox_Fanart_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Fanart_tabs.SelectedItem).ID == id)
                        comboBox_Fanart_tabs.SelectedIndex = -1;
                if (comboBox_Screenshots_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Screenshots_tabs.SelectedItem).ID == id)
                        comboBox_Screenshots_tabs.SelectedIndex = -1;
                // Make folder
                //     if (textBox_boxart_front_folder.Text.Length == 0)
                {
                    InformationContainerImage imgCont = (InformationContainerImage)comboBox_boxart_front_tab.SelectedItem;
                    if (imgCont.FoldersMemory != null)
                        if (imgCont.FoldersMemory.Count > 0)
                            textBox_boxart_front_folder.Text = HelperTools.GetFullPath(imgCont.FoldersMemory[0]);
                }
            }
        }
        private void comboBox_Fanart_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected id
            if (comboBox_Fanart_tabs.SelectedIndex >= 0)
            {
                string id = ((InformationContainer)comboBox_Fanart_tabs.SelectedItem).ID;
                // Checkout others
                if (comboBox_Banners_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Banners_tabs.SelectedItem).ID == id)
                        comboBox_Banners_tabs.SelectedIndex = -1;
                if (comboBox_boxart_bakc_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_bakc_tabs.SelectedItem).ID == id)
                        comboBox_boxart_bakc_tabs.SelectedIndex = -1;
                if (comboBox_boxart_front_tab.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_front_tab.SelectedItem).ID == id)
                        comboBox_boxart_front_tab.SelectedIndex = -1;
                if (comboBox_Screenshots_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Screenshots_tabs.SelectedItem).ID == id)
                        comboBox_Screenshots_tabs.SelectedIndex = -1;
                // Make folder
                //    if (textBox_Fanart_folder.Text.Length == 0)
                {
                    InformationContainerImage imgCont = (InformationContainerImage)comboBox_Fanart_tabs.SelectedItem;
                    if (imgCont.FoldersMemory != null)
                        if (imgCont.FoldersMemory.Count > 0)
                            textBox_Fanart_folder.Text = HelperTools.GetFullPath(imgCont.FoldersMemory[0]);
                }
            }
        }
        private void comboBox_Banners_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected id
            if (comboBox_Banners_tabs.SelectedIndex >= 0)
            {
                string id = ((InformationContainer)comboBox_Banners_tabs.SelectedItem).ID;
                // Checkout others
                if (comboBox_Fanart_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Fanart_tabs.SelectedItem).ID == id)
                        comboBox_Fanart_tabs.SelectedIndex = -1;
                if (comboBox_boxart_bakc_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_bakc_tabs.SelectedItem).ID == id)
                        comboBox_boxart_bakc_tabs.SelectedIndex = -1;
                if (comboBox_boxart_front_tab.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_front_tab.SelectedItem).ID == id)
                        comboBox_boxart_front_tab.SelectedIndex = -1;
                if (comboBox_Screenshots_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Screenshots_tabs.SelectedItem).ID == id)
                        comboBox_Screenshots_tabs.SelectedIndex = -1;
                // Make folder
                //    if (textBox_Banners_folder.Text.Length == 0)
                {
                    InformationContainerImage imgCont = (InformationContainerImage)comboBox_Banners_tabs.SelectedItem;
                    if (imgCont.FoldersMemory != null)
                        if (imgCont.FoldersMemory.Count > 0)
                            textBox_Banners_folder.Text = HelperTools.GetFullPath(imgCont.FoldersMemory[0]);
                }
            }
        }
        private void comboBox_Screenshots_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected id
            //   if (comboBox_Screenshots_tabs.SelectedIndex >= 0)
            {
                string id = ((InformationContainer)comboBox_Screenshots_tabs.SelectedItem).ID;
                // Checkout others
                if (comboBox_Fanart_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Fanart_tabs.SelectedItem).ID == id)
                        comboBox_Fanart_tabs.SelectedIndex = -1;
                if (comboBox_boxart_bakc_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_bakc_tabs.SelectedItem).ID == id)
                        comboBox_boxart_bakc_tabs.SelectedIndex = -1;
                if (comboBox_boxart_front_tab.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_boxart_front_tab.SelectedItem).ID == id)
                        comboBox_boxart_front_tab.SelectedIndex = -1;
                if (comboBox_Banners_tabs.SelectedIndex >= 0)
                    if (((InformationContainer)comboBox_Banners_tabs.SelectedItem).ID == id)
                        comboBox_Banners_tabs.SelectedIndex = -1;
                // Make folder
                if (textBox_Screenshots_folder.Text.Length == 0)
                {
                    InformationContainerImage imgCont = (InformationContainerImage)comboBox_Screenshots_tabs.SelectedItem;
                    if (imgCont.FoldersMemory != null)
                        if (imgCont.FoldersMemory.Count > 0)
                            textBox_Screenshots_folder.Text = HelperTools.GetFullPath(imgCont.FoldersMemory[0]);
                }
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (comboBox_overview_tab.SelectedIndex > 0)
            {
                InformationContainerInfoText cont = (InformationContainerInfoText)comboBox_overview_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_overview_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox_boxart_bakc_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_boxart_bakc_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_boxart_back_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox_boxart_front_tab.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_boxart_front_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_boxart_front_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button10_Click_1(object sender, EventArgs e)
        {
            if (comboBox_Fanart_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Fanart_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Fanart_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button9_Click_1(object sender, EventArgs e)
        {
            if (comboBox_Banners_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Banners_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Banners_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (comboBox_Screenshots_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Screenshots_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Screenshots_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            if (comboBox_clearlogo_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_clearlogo_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_clearlogo_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_clearlogo_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_clearlogo_folder.Text = fol.SelectedPath;
            }
        }
        private void comboBox_clearlogo_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_clearlogo_tabs.SelectedIndex > 0 && textBox_clearlogo_folder.Text == "")
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_clearlogo_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_clearlogo_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
    }
}
