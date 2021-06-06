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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_ImportDatabaseFileUniversal : Form
    {
        public Form_ImportDatabaseFileUniversal(string consoleID)
        {
            InitializeComponent();
            // Get the console
            selectedConsole = profileManager.Profile.Consoles[consoleID];
            // Fill up archive extensions
            string exs = "";
            foreach (string ex in selectedConsole.ArchiveExtensions)
                exs += ex + ";";
            if (exs.Length > 1)
                textBox_archive_extesnions.Text = exs.Substring(0, exs.Length - 1);

            // Load all databases !
            DatabaseFilesManager.DetectSupportedFormats();
            // Fill them in the combobox
            comboBox_dbType.Items.Clear();
            foreach (DatabaseFile db in DatabaseFilesManager.AvailableFormats)
            {
                if (db.IsVisible)
                    comboBox_dbType.Items.Add(db);
            }
            // Change the database file
            changeDatabaseFile(this, null);
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private EmulatorsOrganizer.Core.Console selectedConsole;
        private DatabaseFile selectedFormat;
        private TextWriterTraceListener listner;
        private string logPath;
        private string _database_file;
        private bool _apply_rom_name;
        private bool _apply_rom_datainfo;
        private bool _compare_name_name;
        private bool _compare_fileName_name;
        private bool _compare_crc;
        private bool _compare_md5;
        private bool _compare_sha1;
        private bool _cache_on_disk;
        private bool _turbo_speed;
        private bool _check_inside_archive;
        private string _archive_password;
        private bool _perfect_match;
        private bool _add_categories;
        private bool _add_filters;
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
        private int progress_master;
        private int progress_sub;
        // Thread
        private Thread mainThread;
        private bool finished;

        private void PROCESS()
        {
            // Add listener
            string logFileName = string.Format("{0}-import {1} database.txt",
                DateTime.Now.ToLocalTime().ToString(), selectedFormat.Name);
            logFileName = logFileName.Replace(":", "");
            logFileName = logFileName.Replace("/", "-");
            logPath = Path.Combine("Logs", logFileName);
            listner = new TextWriterTraceListener(HelperTools.GetFullPath(logPath));
            Trace.Listeners.Add(listner);
            Services.ServicesManager.OnDisableWindowListner();

            // Start
            Trace.WriteLine(string.Format("Import {0} database file for console '{1}' started at {2}",
                selectedFormat.Name, selectedConsole.Name, DateTime.Now.ToLocalTime()), "Import Database File");
            int step_index = 0;
            #region 1 Read the database file content
            Trace.WriteLine("Reading database file ...", "Import Database File");
            status_master = "Reading database file ...";
            progress_master = 100 / (4 - step_index);
            // Get database content
            List<DBEntry> databaseEntries = selectedFormat.LoadFile(_database_file);
            Trace.WriteLine("Database read done, total " + databaseEntries.Count + " entries found.", "Import Database File");
            #endregion
            #region 2 Get the roms
            step_index++;
            Trace.WriteLine("Collecting the roms ...", "Import Database file");
            status_master = "Collecting the roms ...";
            progress_master = 100 / (4 - step_index);
            // Get single roms first
            RomsCollection roms = new RomsCollection(null, false, profileManager.Profile.Roms.GetSingleRoms(selectedConsole.ID));
            // Add parents ...
            roms.AddRange(profileManager.Profile.Roms.GetParentRoms(selectedConsole.ID));
            // Add children finally.
            // roms.AddRange(profileManager.Profile.Roms.GetChildrenRoms(selectedConsole.ID));
            Trace.WriteLine("Roms collected, total of " + roms.Count + " rom(s) [SINGLES AND PARENTS ONLY, children roms get checked later]", "Import Database file");
            #endregion
            #region 3 Compare and apply stuff
            step_index++;
            Trace.WriteLine("Comparing and applying naming", "Import Database File");
            status_master = "Comparing ...";
            progress_master = 100 / (10 - step_index);

            int matchedCount = 0;
            List<string> matchedRomNames = new List<string>();
            List<string> notMatchedRomNames = new List<string>();
            for (int rom_index = 0; rom_index < roms.Count; rom_index++)
            {
                if (roms[rom_index].IsSingle)
                {
                    Trace.WriteLine(" --------> THIS IS A SINGLE ROM [" + roms[rom_index].ID + "] (" + roms[rom_index].Name + ")", "Import Database File");
                    bool matched = false;
                    COMPARE_ROM(roms[rom_index], _apply_rom_name, _apply_rom_datainfo, ref databaseEntries, ref matchedRomNames, ref notMatchedRomNames,
                        ref matchedCount, out matched, true);
                }
                else if (roms[rom_index].IsParentRom)
                {
                    Trace.WriteLine(" --------> THIS IS A PARENT ROM [" + roms[rom_index].ID + "] (" + roms[rom_index].Name + ")", "Import Database File");
                    // Compare the parent
                    bool parent_match = false;
                    COMPARE_ROM(roms[rom_index], _rename_parent, _rename_parent, ref databaseEntries, ref matchedRomNames, ref notMatchedRomNames,
                        ref matchedCount, out parent_match, false);
                    List<string> children_ids = roms[rom_index].ChildrenRoms;
                    List<string> children_matched_ids = new List<string>();
                    // Compare children ...
                    foreach (string childID in children_ids)
                    {
                        bool c_match = false;
                        COMPARE_ROM(profileManager.Profile.Roms[childID], _apply_rom_name, _apply_rom_datainfo, ref databaseEntries, ref matchedRomNames, ref notMatchedRomNames,
                        ref matchedCount, out c_match, false);

                        if (c_match)
                            children_matched_ids.Add(childID);
                    }
                    if (parent_match)
                        Trace.WriteLine(" --> PARENT ROM MATCHED [" + roms[rom_index].ID + "] (" + roms[rom_index].Name + ")", "Import Database File");

                    // Do the delete stuff
                    if (_delete_rom_not_found)
                    {
                        if (parent_match)
                        {
                            // The parent match, see what should we do with the children
                            if (_parent_match_keep_all_children)
                            {
                                // Do nothing, keep parent with its all children untouched.
                                Trace.WriteLine(" +--> All children kept along with parent", "Import Database File");
                            }
                            else if (_parent_match_keep_child_match)
                            {
                                Trace.WriteLine(" +--> Removing not matched children ..", "Import Database File");
                                // Remove the children that not match from parent
                                for (int c = 0; c < children_ids.Count; c++)
                                {
                                    if (!children_matched_ids.Contains(children_ids[c]))
                                    {
                                        // Delete the child from database
                                        string cName = profileManager.Profile.Roms[children_ids[c]].Name;
                                        DeleteRomRoutin(profileManager.Profile.Roms[children_ids[c]], ref notMatchedRomNames);
                                        Trace.WriteLine(" +---> CHILD REMOVED [" + children_ids[c] + "] (" + cName + ")", "Import Database File");
                                        // Remove it from the parent
                                        roms[rom_index].ChildrenRoms.Remove(children_ids[c]);
                                        roms[rom_index].Modified = true;
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
                            Trace.WriteLine(" --> PARENT ROM DOES NOT MATCH [" + roms[rom_index].ID + "] (" + roms[rom_index].Name + ")", "Import Database File");
                            if (_parent_not_match_keep_on_one_child_match)
                            {
                                if (children_matched_ids.Count > 0)
                                {
                                    Trace.WriteLine(" +--> keeping the parent, one of children match.", "Import Database File");
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
                                                Trace.WriteLine(" +---> CHILD REMOVED [" + children_ids[c] + "] (" + cName + ")", "Import Database File");
                                                // Remove it from the parent
                                                roms[rom_index].ChildrenRoms.Remove(children_ids[c]);
                                                roms[rom_index].Modified = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Trace.WriteLine(" +--> REMOVING PARENT; NO CHILD MATCH.", "Import Database File");
                                    // No child match, remove the parent.
                                    DeleteRomRoutin(roms[rom_index], ref notMatchedRomNames);
                                }
                            }
                            else if (_parent_not_match_make_matched_children_singles)
                            {
                                Trace.WriteLine(" +--> REMOVING PARENT; MAKING MATCHED CHILDREN SINGLES ...", "Import Database File");
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
                                        Trace.WriteLine(" +---> CHILD REMOVED [" + children_ids[c] + "] (" + cName + ")", "Import Database File");
                                    }
                                    else
                                    {
                                        string cName = profileManager.Profile.Roms[children_ids[c]].Name;
                                        // free it !
                                        profileManager.Profile.Roms[children_ids[c]].IsChildRom = false;
                                        profileManager.Profile.Roms[children_ids[c]].IsParentRom = false;
                                        profileManager.Profile.Roms[children_ids[c]].ParentRomID = "";
                                        Trace.WriteLine(" +---> CHILD BECOME SIGNLE !! [" + children_ids[c] + "] (" + cName + ")", "Import Database File");
                                    }
                                }
                            }
                        }
                    }
                }

                // Progress
                progress_sub = (rom_index * 100) / roms.Count;
                status_sub = string.Format("{0} {1} / {2} ({3} MATCHED) ... {4} %",
                    ls["Status_ApplyingDatabase"], (rom_index + 1).ToString(), roms.Count,
                    matchedCount.ToString(), progress_sub);
            }
            #endregion
            #region 4 Update log with matched and not found roms
            step_index++;
            Trace.WriteLine("Finishing", "Import Database File");
            status_master = "Finishing ...";
            progress_master = 100 / (4 - step_index);

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
            Trace.WriteLine(string.Format("Import Database File '{0}' finished at {1}.", selectedFormat.Name, DateTime.Now.ToLocalTime()), "Import Database File");
            listner.Flush();
            Trace.Listeners.Remove(listner);
            CloseAfterFinish();
            #endregion
        }
        #region Comparing Methods
        /// <summary>
        /// Compare rom name with entry file names. WORKS WITH SINGLE ROMS ONLY
        /// </summary>
        /// <param name="romName"></param>
        /// <param name="entryFileNames"></param>
        /// <returns></returns>
        private bool Compare_RomName_EntryName(string romName, string[] entryFileNames)
        {
            bool ROMMATCH = false;
            romName = romName.Replace(":", " ");
            romName = romName.Replace("|", " ");
            romName = romName.Replace(@"\", " ");
            romName = romName.Replace("/", " ");
            romName = romName.Replace("*", " ");
            romName = romName.Replace("?", " ");
            romName = romName.Replace("<", " ");
            romName = romName.Replace(">", " ");
            romName = romName.Replace("_", " ");
            romName = romName.Replace(@"""", " ");
            romName = romName.Replace(" ", "");
            string fileOfDatabase = "";
            // Check rom
            foreach (string f in entryFileNames)
            {
                // Remove forbidden values
                fileOfDatabase = f.Replace(":", " ");
                fileOfDatabase = fileOfDatabase.Replace("|", " ");
                fileOfDatabase = fileOfDatabase.Replace(@"\", " ");
                fileOfDatabase = fileOfDatabase.Replace("/", " ");
                fileOfDatabase = fileOfDatabase.Replace("*", " ");
                fileOfDatabase = fileOfDatabase.Replace("?", " ");
                fileOfDatabase = fileOfDatabase.Replace("<", " ");
                fileOfDatabase = fileOfDatabase.Replace(">", " ");
                fileOfDatabase = fileOfDatabase.Replace("_", " ");
                fileOfDatabase = fileOfDatabase.Replace(@"""", " ");
                fileOfDatabase = fileOfDatabase.Replace(" ", "");

                if (romName == "" || fileOfDatabase == "")
                    ROMMATCH = false;
                else if (romName.Length == 0 || fileOfDatabase.Length == 0)
                    ROMMATCH = false;
                else if (romName.Length == fileOfDatabase.Length)
                    ROMMATCH = fileOfDatabase.ToLower() == romName.ToLower();
                else
                    ROMMATCH = romName.ToLower().StartsWith(fileOfDatabase.ToLower());
                if (ROMMATCH) break;
            }
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
        private bool Compare_RomPath_EntryName(string romFilePath, string[] entryFileNames)
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
                extractor.Dispose();
            }
            else if (_check_inside_archive && _archive_extesnions.Contains(Path.GetExtension(romFilePath).ToLower()))
            {
                SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(romFilePath));

                foreach (string f in extractor.ArchiveFileNames)
                {
                    if (selectedConsole.Extensions.Contains(Path.GetExtension(f).ToLower()))
                        romFiles.Add(Path.GetFileNameWithoutExtension(f));
                }
                extractor.Dispose();
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
                fileOfRom = fileOfRom.Replace("|", " ");
                fileOfRom = fileOfRom.Replace(@"\", " ");
                fileOfRom = fileOfRom.Replace("/", " ");
                fileOfRom = fileOfRom.Replace("*", " ");
                fileOfRom = fileOfRom.Replace("?", " ");
                fileOfRom = fileOfRom.Replace("<", " ");
                fileOfRom = fileOfRom.Replace(">", " ");
                fileOfRom = fileOfRom.Replace("_", " ");
                fileOfRom = fileOfRom.Replace(@"""", " ");
                fileOfRom = fileOfRom.Replace(" ", "");
                string fileOfDatabase = "";
                bool dbEntryMatch = false;
                // Check rom
                foreach (string f in entryFileNames)
                {
                    // Remove forbidden values
                    fileOfDatabase = f.Replace(":", " ");
                    fileOfDatabase = fileOfDatabase.Replace("|", " ");
                    fileOfDatabase = fileOfDatabase.Replace(@"\", " ");
                    fileOfDatabase = fileOfDatabase.Replace("/", " ");
                    fileOfDatabase = fileOfDatabase.Replace("*", " ");
                    fileOfDatabase = fileOfDatabase.Replace("?", " ");
                    fileOfDatabase = fileOfDatabase.Replace("<", " ");
                    fileOfDatabase = fileOfDatabase.Replace(">", " ");
                    fileOfDatabase = fileOfDatabase.Replace("_", " ");
                    fileOfDatabase = fileOfDatabase.Replace(@"""", " ");
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
                    else
                    {
                        if (fileOfRom.ToLower().StartsWith(fileOfDatabase.ToLower()))
                        { matchedNumber++; dbEntryMatch = true; }
                    }
                    if (dbEntryMatch)
                        break;
                }
            }
            //   if (_perfect_match)
            //       return matchedNumber == romFiles.Count && matchedNumber > 0;
            //   else
            return matchedNumber > 0;
        }
        private List<string> GetFile_CRC_MD5_SHA1(string romFilePath, bool useCRC, bool useMD5, bool useSHA1)
        {
            List<string> romCRCs = new List<string>();
            List<string> romSHA1s = new List<string>();
            List<string> romMD5s = new List<string>();
            if (HelperTools.IsAIPath(romFilePath))
            {
                Trace.WriteLine("-- Rom is AI path ...", "Import Database File");
                // Extract the content of the archive first
                SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(romFilePath)));
                int index = HelperTools.GetIndexFromAIPath(romFilePath);
                Stream mstream = null;

                if (selectedFormat.BytesToSkip == 0 && useCRC)
                {
                    // No need to extract, just use already calculated in the archive
                    romCRCs.Add(extractor.ArchiveFileData[index].Crc.ToString("X8").ToLower());
                }
                else if (extractor.ArchiveFileData[index].Encrypted)
                {
                    // Use password
                    extractor.Dispose();
                    extractor = new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(romFilePath)), _archive_password);
                    // Try to extract and get data
                    try
                    {
                        mstream = new FileStream(Path.GetTempPath() + "\\test.tst", FileMode.Create, FileAccess.Write);
                        extractor.ExtractFile(index, mstream);
                        mstream.Close();
                        mstream.Dispose();
                        if (useCRC)
                        {
                            romCRCs.Add(selectedFormat.CalculateCRC(Path.GetTempPath() + "\\test.tst", true).ToLower());
                        }
                        if (useSHA1)
                        {
                            romSHA1s.Add(selectedFormat.CalculateSHA1(Path.GetTempPath() + "\\test.tst", true).ToLower());
                        }
                        if (useMD5)
                        {
                            romMD5s.Add(selectedFormat.CalculateMD5(Path.GetTempPath() + "\\test.tst", true).ToLower());
                        }
                    }
                    catch { }
                }
                else
                {
                    // Try to extract and get data
                    try
                    {
                        mstream = new FileStream(Path.GetTempPath() + "\\test.tst", FileMode.Create, FileAccess.Write);
                        extractor.ExtractFile(index, mstream);
                        mstream.Close();
                        mstream.Dispose();
                        if (useCRC)
                        {
                            romCRCs.Add(selectedFormat.CalculateCRC(Path.GetTempPath() + "\\test.tst", true).ToLower());
                        }
                        if (useSHA1)
                        {
                            romSHA1s.Add(selectedFormat.CalculateSHA1(Path.GetTempPath() + "\\test.tst", true).ToLower());
                        }
                        if (useMD5)
                        {
                            romMD5s.Add(selectedFormat.CalculateMD5(Path.GetTempPath() + "\\test.tst", true).ToLower());
                        }
                    }
                    catch { }
                }
                extractor.Dispose();
            }
            else if (_check_inside_archive && _archive_extesnions.Contains(Path.GetExtension(romFilePath).ToLower()))
            {
                Trace.WriteLine("-- Rom is compressed [archive extension match + 'check inside archive' is enabled] checking inside of it for a match", "Import Database File");
                if (!_cache_on_disk)
                {
                    Trace.WriteLine("----- Collecting archive files information (extracting if necessary)...", "Import Database File");

                    // Extract the content of the archive first
                    SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(romFilePath));
                    int tries = 0;
                    Stream mstream = null;

                    for (int j = 0; j < extractor.ArchiveFileData.Count; j++)
                    {
                        if (selectedFormat.BytesToSkip == 0 && useCRC)
                        {
                            // No need to extract, just use already calculated in the archive
                            romCRCs.Add(extractor.ArchiveFileData[j].Crc.ToString("X8").ToLower());
                            continue;
                        }
                        if (extractor.ArchiveFileData[j].Encrypted && tries == 0)
                        {
                            // Use password
                            extractor.Dispose();
                            extractor = new SevenZipExtractor(HelperTools.GetFullPath(romFilePath), _archive_password);
                            tries++;
                            j = -1;
                            continue;
                        }
                        // Try to extract and get data
                        try
                        {
                            mstream = new FileStream(Path.GetTempPath() + "\\test.tst", FileMode.Create, FileAccess.Write);
                            extractor.ExtractFile(j, mstream);
                            mstream.Close();
                            mstream.Dispose();
                            if (useCRC)
                            {
                                romCRCs.Add(selectedFormat.CalculateCRC(Path.GetTempPath() + "\\test.tst", true).ToLower());
                            }
                            if (useSHA1)
                            {
                                romSHA1s.Add(selectedFormat.CalculateSHA1(Path.GetTempPath() + "\\test.tst", true).ToLower());
                            }
                            if (useMD5)
                            {
                                romMD5s.Add(selectedFormat.CalculateMD5(Path.GetTempPath() + "\\test.tst", true).ToLower());
                            }
                        }
                        catch { }
                    }
                    extractor.Dispose();
                }
                else
                {
                    Trace.WriteLine("----- Caching on disk ...", "Import Database File");

                    // Extract the content of the archive first
                    SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(romFilePath));
                    if (extractor.ArchiveFileData.Count > 0)
                    {
                        if (selectedFormat.BytesToSkip == 0 && useCRC)
                        {
                            for (int j = 0; j < extractor.ArchiveFileData.Count; j++)
                            {
                                // No need to extract, just use already calculated in the archive
                                romCRCs.Add(extractor.ArchiveFileData[j].Crc.ToString("X8").ToLower());
                            }
                        }
                        else
                        {
                            // Extract content of the archive on disk then calculate one by one ...
                            // THis is the "cache on disk".
                            if (extractor.ArchiveFileData[0].Encrypted)
                                extractor = new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(romFilePath)), _archive_password);
                            // Extract
                            Directory.CreateDirectory(Path.GetTempPath() + "\\EOTMP\\");
                            // Make sure it's clear
                            string[] tmpFiles = Directory.GetFiles(Path.GetTempPath() + "\\EOTMP\\");
                            foreach (string f in tmpFiles)
                            {
                                try
                                {
                                    File.Delete(f);
                                }
                                catch { }
                            }
                            extractor.ExtractArchive(Path.GetTempPath() + "\\EOTMP\\");

                            tmpFiles = Directory.GetFiles(Path.GetTempPath() + "\\EOTMP\\");
                            foreach (string f in tmpFiles)
                            {
                                if (useCRC)
                                {
                                    romCRCs.Add(selectedFormat.CalculateCRC(f, true).ToLower());
                                }
                                if (useSHA1)
                                {
                                    romSHA1s.Add(selectedFormat.CalculateSHA1(f, true).ToLower());
                                }
                                if (useMD5)
                                {
                                    romMD5s.Add(selectedFormat.CalculateMD5(f, true).ToLower());
                                }
                            }
                        }
                    }
                    extractor.Dispose();
                }
            }
            else
            {
                Trace.WriteLine("-- Treating rom as normal rom (not archive)", "Import Database File");

                // No check inside archive, calculate whatever file is (archive or not)
                if (useCRC)
                {
                    romCRCs.Add(selectedFormat.CalculateCRC(HelperTools.GetFullPath(romFilePath), !_archive_extesnions.Contains(Path.GetExtension(romFilePath).ToLower())).ToLower());
                }
                if (useSHA1)
                {
                    romSHA1s.Add(selectedFormat.CalculateSHA1(HelperTools.GetFullPath(romFilePath), !_archive_extesnions.Contains(Path.GetExtension(romFilePath).ToLower())).ToLower());
                }
                if (useMD5)
                {
                    romMD5s.Add(selectedFormat.CalculateMD5(HelperTools.GetFullPath(romFilePath), !_archive_extesnions.Contains(Path.GetExtension(romFilePath).ToLower())).ToLower());
                }
            }
            // Compare
            if (useCRC)
                return romCRCs;
            if (useMD5)
                return romMD5s;
            if (useSHA1)
                return romSHA1s;
            return null;
        }
        private void COMPARE_ROM(Rom rom, bool applyName, bool applyDataInfo, ref List<DBEntry> databaseEntries,
            ref List<string> matchedRomNames, ref List<string> notMatchedRomNames,
            ref int matchedCount, out bool rom_matched, bool doDelete)
        {
            bool ROMMATCH = false;
            DBEntry matchedEntry = new DBEntry();
            List<string> romCRCs = new List<string>();
            if (_compare_crc || _compare_md5 || _compare_sha1)
            {
                romCRCs = GetFile_CRC_MD5_SHA1(rom.Path,
                           _compare_crc, _compare_md5, _compare_sha1);
            }
            // Loop through database entries looking for a match
            for (int entry_index = 0; entry_index < databaseEntries.Count; entry_index++)
            {
                if (_compare_name_name)
                {
                    ROMMATCH = Compare_RomName_EntryName(rom.Name,
                         databaseEntries[entry_index].FileNames.ToArray());
                }
                else if (_compare_fileName_name)
                {
                    ROMMATCH = Compare_RomPath_EntryName(rom.Path,
                        databaseEntries[entry_index].FileNames.ToArray());
                }
                else
                {
                    if (_compare_crc)
                    {
                        ROMMATCH = romCRCs.Contains(databaseEntries[entry_index].CRC.ToLower());
                    }
                    if (_compare_md5)
                    {
                        ROMMATCH = romCRCs.Contains(databaseEntries[entry_index].MD5.ToLower());
                    }
                    if (_compare_sha1)
                    {
                        ROMMATCH = romCRCs.Contains(databaseEntries[entry_index].SHA1.ToLower());
                    }
                }
                if (_perfect_match)// MAME perfect match.
                {
                    SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath
                        (rom.Path)));
                    SevenZipExtractor extractor1 = null;
                    SevenZipExtractor extractor2 = null;
                    bool RomOK = true;
                    string theClone = databaseEntries[entry_index].GetPropertyValue("Clone Of");
                    bool isCloneOf = theClone != "";
                    string theRom = databaseEntries[entry_index].GetPropertyValue("Rom Of");
                    bool isRomOf = theRom != "";

                    if (isCloneOf)
                    {
                        if (File.Exists(Path.GetDirectoryName
                                           (HelperTools.GetFullPath(rom.Path)) +
                                           "//" + theClone + Path.GetExtension(rom.Path)))
                        {
                            extractor1 = new SevenZipExtractor(Path.GetDirectoryName
                                           (HelperTools.GetFullPath(rom.Path)) +
                                           "//" + theClone + Path.GetExtension(rom.Path));
                        }
                        else
                        {
                            isCloneOf = false;
                        }
                    }
                    if (isRomOf)
                    {
                        if (File.Exists(Path.GetDirectoryName
                                           (HelperTools.GetFullPath(rom.Path)) +
                                           "//" + theRom + Path.GetExtension(rom.Path)))
                        {
                            extractor2 = new SevenZipExtractor(Path.GetDirectoryName
                                           (HelperTools.GetFullPath(rom.Path)) +
                                           "//" + theRom + Path.GetExtension(rom.Path));
                        }
                        else
                        {
                            isRomOf = false;
                        }
                    }
                    for (int o = 4; o < databaseEntries[entry_index].PerfectMatchCRCS.Count; o++)
                    {
                        bool found = false;
                        try
                        {
                            foreach (ArchiveFileInfo inf in extractor.ArchiveFileData)
                            {
                                if (databaseEntries[entry_index].PerfectMatchCRCS[o].CRC != "")
                                    if (uint.Parse(databaseEntries[entry_index].PerfectMatchCRCS[o].CRC, System.Globalization.NumberStyles.AllowHexSpecifier) == inf.Crc)
                                    {
                                        found = true;
                                        break;
                                    }
                            }
                        }
                        catch { }
                        if (isCloneOf & !found)
                        {
                            foreach (ArchiveFileInfo inf in extractor1.ArchiveFileData)
                            {
                                if (databaseEntries[entry_index].PerfectMatchCRCS[o].CRC != "")
                                    if (uint.Parse(databaseEntries[entry_index].PerfectMatchCRCS[o].CRC, System.Globalization.NumberStyles.AllowHexSpecifier) == inf.Crc)
                                    {
                                        found = true;
                                        break;
                                    }
                            }
                        }
                        if (isRomOf & !found)
                        {

                            foreach (ArchiveFileInfo inf in extractor2.ArchiveFileData)
                            {
                                if (databaseEntries[entry_index].PerfectMatchCRCS[o].CRC != "")
                                    if (uint.Parse(databaseEntries[entry_index].PerfectMatchCRCS[o].CRC, System.Globalization.NumberStyles.AllowHexSpecifier) == inf.Crc)
                                    {
                                        found = true;
                                        break;
                                    }
                            }
                        }
                        if (!found)
                        {
                            RomOK = false; break;
                        }
                    }
                    extractor.Dispose();
                    extractor1.Dispose();
                    extractor2.Dispose();
                    ROMMATCH = RomOK;
                }
                if (ROMMATCH)
                {
                    matchedEntry = databaseEntries[entry_index].Clone();

                    if (_turbo_speed)
                        databaseEntries.RemoveAt(entry_index);
                    break;
                }
            }

            rom_matched = ROMMATCH;
            if (ROMMATCH)
            {
                string ename = matchedEntry.GetPropertyValue("Name");
                Trace.WriteLine("ROM MATCHED [ID=" + rom.ID + "] (Name=" + rom.Name + "), entry name: " + ename, "Import Database File");
                matchedRomNames.Add(rom.Name);
                if (applyName || applyDataInfo)
                {
                    // Database info items
                    selectedFormat.ApplyName(rom, matchedEntry, applyName, applyDataInfo);
                    Trace.WriteLine("ROM DATA UPDATED [ Renamed to: " + rom.Name + " ]", "Import Database File");
                }
                if (_add_categories)
                {
                    if (matchedEntry.Categories != null)
                    {
                        foreach (string categ in matchedEntry.Categories)
                        {
                            if (!rom.Categories.Contains(categ))
                            {
                                rom.Categories.Add(categ);
                                Trace.WriteLine("ROM DATA UPDATED [ CATEGORY ADDED: " + categ + " ]", "Import Database File");
                            }
                        }
                    }
                }
                matchedCount++;
            }
            else
            {
                if (doDelete)
                    DeleteRomRoutin(rom, ref notMatchedRomNames);
            }
        }
        private void DeleteRomRoutin(Rom rom, ref List<string> notMatchedRomNames)
        {
            notMatchedRomNames.Add(rom.Name);
            if (_delete_rom_not_found)
            {
                Trace.WriteLine(" !--> Deleting rom from database", "Import Database File");
                Trace.WriteLine(" +---> Removing rom: [" + rom.ID + "] " + rom.Name, "Import Database File");
                profileManager.Profile.Roms.Remove(rom.ID, false);
                // Delete from disk
                if (_delete_rom_not_found_file && !HelperTools.IsAIPath(rom.Path))
                {
                    Trace.WriteLine(" +----> Removing rom file: " + rom.Path, "Import Database File");
                    try
                    {
                        File.Delete(HelperTools.GetFullPath(rom.Path));
                        Trace.WriteLine(" +----> File removed: " + rom.Path, "Import Database File");
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(" +---->> X UNABLE to remove rom file: " + rom.Path, "Import Database File");
                        Trace.WriteLine(" +---->> X " + ex.Message, "Import Database File");
                    }
                }
                // Delete related files !
                if (_delete_rom_not_found_related_files)
                {
                    Trace.WriteLine(" +---> Removing rom related files ... ", "Import Database File");
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
                                            Trace.WriteLine(" +----> File removed: " + rr, "Import Database File");
                                        }
                                        catch (Exception ex)
                                        {
                                            Trace.WriteLine(" +---->> X UNABLE to remove file: " + rr, "Import Database File");
                                            Trace.WriteLine(" +---->>" + ex.Message, "Import Database File");
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
        #endregion
        private void RefreshComparingOptions()
        {
            if (selectedFormat == null)
            {
                radioButton_compare_crc.Enabled =
                radioButton_compare_md5.Enabled =
                radioButton_compare_name.Enabled =
                radioButton_compare_rom_file_name.Enabled =
                radioButton_compare_sha1.Enabled = false;
            }
            else
            {
                radioButton_compare_crc.Enabled = (selectedFormat.ComparingMode & CompareMode.CRC) == CompareMode.CRC;
                radioButton_compare_md5.Enabled = (selectedFormat.ComparingMode & CompareMode.MD5) == CompareMode.MD5;
                radioButton_compare_name.Enabled = (selectedFormat.ComparingMode & CompareMode.RomName) == CompareMode.RomName;
                radioButton_compare_rom_file_name.Enabled = (selectedFormat.ComparingMode & CompareMode.RomFileName) == CompareMode.RomFileName;
                radioButton_compare_sha1.Enabled = (selectedFormat.ComparingMode & CompareMode.SHA1) == CompareMode.SHA1;
            }
            if (radioButton_compare_name.Enabled)
                radioButton_compare_name.Checked = true;
            else if (radioButton_compare_rom_file_name.Enabled)
                radioButton_compare_rom_file_name.Checked = true;
            else if (radioButton_compare_crc.Enabled)
                radioButton_compare_crc.Checked = true;
            else if (radioButton_compare_md5.Enabled)
                radioButton_compare_md5.Checked = true;
            else if (radioButton_compare_sha1.Enabled)
                radioButton_compare_sha1.Checked = true;
        }
        private void RefreshFormatOptions()
        {
            panel_db_options.Controls.Clear();
            checkBox_archive_perfect_match.Enabled = false;
            if (selectedFormat != null)
            {
                Control optionsControl = selectedFormat.OptionsControl;
                if (optionsControl != null)
                {
                    panel_db_options.Controls.Add(optionsControl);
                    optionsControl.Location = new Point(0, 0);
                }
                checkBox_archive_perfect_match.Enabled = selectedFormat.SupportPerfectMatch;
                if (selectedFormat.ForceNes)
                {
                    checkBox_forNesArchiveTweeks.Checked = true;
                    checkBox_cahceOndisk.Checked = true;
                    checkBox_forNesArchiveTweeks.Enabled = checkBox_cahceOndisk.Enabled = false;
                }
                else
                {
                    checkBox_forNesArchiveTweeks.Checked = false;
                    checkBox_cahceOndisk.Checked = false;
                    checkBox_forNesArchiveTweeks.Enabled = checkBox_cahceOndisk.Enabled = true;
                }
            }
        }
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
          ls["MessageCaption_ImportDatabaseFile"], new string[] { ls["Button_Ok"], ls["Button_OpenLog"] },
          0, null, ManagedMessageBoxIcon.Info);
            if (res.ClickedButtonIndex == 1)
            {
                try { Process.Start(HelperTools.GetFullPath(logPath)); }
                catch (Exception ex)
                { ManagedMessageBox.ShowErrorMessage(ex.Message); }
            }
            if (selectedFormat != null)
            {
                if (_add_filters && selectedFormat.AddFilters)
                {
                    foreach (Filter ff in selectedFormat.Filters)
                    {
                        if (!selectedConsole.IsFilterExist(ff.Name))
                        {
                            profileManager.Profile.Consoles[selectedConsole.ID].Filters.Add(ff);
                            Trace.WriteLine("Filter added: " + ff.Name, "Import Database File");
                        }
                    }
                }
                selectedFormat.Progress -= selectedFormat_Progress;
                selectedFormat.ProgressStarted -= selectedFormat_Progress;
                selectedFormat.ProgressFinished -= selectedFormat_Progress;
                selectedFormat.Dispose();
            }
            profileManager.Profile.OnDatabaseImported();
            this.Close();
            Services.ServicesManager.OnEnableWindowListner();
        }
        // When a format changes
        private void comboBox_dbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_dbType.SelectedIndex >= 0)
                selectedFormat = (DatabaseFile)comboBox_dbType.SelectedItem;
            else
                selectedFormat = null;

            RefreshComparingOptions();
            RefreshFormatOptions();
        }
        // START !
        private void button_start_Click(object sender, EventArgs e)
        {
            // Make checks
            if (textBox_dbFile.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage("No database file selected !");
                changeDatabaseFile(this, null);
                return;
            }
            if (!File.Exists(textBox_dbFile.Text))
            {
                ManagedMessageBox.ShowErrorMessage("Database file is not exist !");
                return;
            }
            if (comboBox_dbType.SelectedIndex < 0 || selectedFormat == null)
            {
                ManagedMessageBox.ShowErrorMessage("Database type is not selected.");
                return;
            }

            // Poke options ...
            _database_file = textBox_dbFile.Text;
            _apply_rom_name = checkBox_rename_rom.Checked;
            _apply_rom_datainfo = checkBox_apply_rom_data_info.Checked;
            _compare_name_name = radioButton_compare_name.Checked;
            _compare_fileName_name = radioButton_compare_rom_file_name.Checked;
            _compare_crc = radioButton_compare_crc.Checked;
            _compare_md5 = radioButton_compare_md5.Checked;
            _compare_sha1 = radioButton_compare_sha1.Checked;
            _turbo_speed = checkBox_turboe.Checked;
            _check_inside_archive = checkBox_check_inside_archive.Checked;
            _archive_password = textBox_archivePassword.Text;
            _perfect_match = checkBox_archive_perfect_match.Checked;
            _cache_on_disk = checkBox_cahceOndisk.Checked;
            _archive_extesnions = new List<string>(textBox_archive_extesnions.Text.Split(';'));
            _delete_rom_not_found = checkBox_delete_roms_nott_found.Checked;
            _delete_rom_not_found_file = checkBox_delete_not_matched_files.Checked;
            _delete_rom_not_found_related_files = checkBox_delete_not_matched_related.Checked;
            _add_categories = checkBox_import_category.Checked;
            _add_filters = checkBox_add_filters.Checked;
            // Apply format stuff
            selectedFormat.Progress += selectedFormat_Progress;
            selectedFormat.ProgressStarted += selectedFormat_Progress;
            selectedFormat.ProgressFinished += selectedFormat_Progress;
            selectedFormat.IsNesConsole = checkBox_forNesArchiveTweeks.Checked;
            _rename_parent = checkBox_rename_parent.Checked;
            _parent_match_keep_all_children = radioButton_parent_match_keep_all_children.Checked;
            _parent_match_keep_child_match = radioButton_parent_matche_keep_matched_only.Checked;
            _parent_not_match_keep_on_one_child_match = radioButton_parent_not_match_keep_parent_if_one_child_match.Checked;
            _parent_not_match_make_matched_children_singles = radioButton_parent_not_mathc_free_matched_children.Checked;
            _parent_not_match_keep_on_one_child_match_dont_keep_not_mathced_children = checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.Checked;
            // Disable everything
            groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = false;
            button_start.Enabled = groupBox6.Enabled = false;
            // Start timer
            timer1.Start();
            finished = false;
            // Start thread !
            mainThread = new Thread(new ThreadStart(PROCESS));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }
        private void selectedFormat_Progress(object sender, ProgressArgs e)
        {
            status_sub = e.Status;
            progress_sub = e.Completed;
        }
        // Change database file
        private void changeDatabaseFile(object sender, EventArgs e)
        {
            // Show open file dialog
            OpenFileDialog openDial = new OpenFileDialog();
            openDial.Title = ls["Title_ImportDatabaseFile"];
            openDial.Filter = DatabaseFilesManager.GetFilter();
            if (openDial.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Set file path
                textBox_dbFile.Text = openDial.FileName;
                // Set type
                if (openDial.FilterIndex - 1 < comboBox_dbType.Items.Count)
                    comboBox_dbType.SelectedIndex = openDial.FilterIndex - 1;
            }
            if (comboBox_dbType.Items.Count > 0 && comboBox_dbType.SelectedIndex < 0)
                comboBox_dbType.SelectedIndex = 0;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status_master.Text = status_master;
            label_status_sub.Text = status_sub;
            progressBar_master.Value = progress_master;
            progressBar_slave.Value = progress_sub;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void FormImportDatabaseFileUniversal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_ImportDatabaseFile"]);
                        if (result.ClickedButtonIndex == 0)
                        {
                            mainThread.Abort();
                            mainThread = null;
                            ServicesManager.OnEnableWindowListner();
                            Trace.WriteLine("Database import operation finished at " + DateTime.Now.ToLocalTime(), "Import Database File");
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
        private void checkBox_delete_roms_nott_found_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_delete_not_matched_files.Enabled =
            checkBox_delete_not_matched_related.Enabled =
            groupBox_parent_match.Enabled =
            groupBox_parent_not_match.Enabled =
            checkBox_delete_roms_nott_found.Checked;
        }
        private void radioButton_parent_not_match_keep_parent_if_one_child_match_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.Enabled =
                radioButton_parent_not_match_keep_parent_if_one_child_match.Checked;
        }
        // Reset archive extensions
        private void button3_Click(object sender, EventArgs e)
        {
            // Fill up archive extensions
            string exs = "";
            foreach (string ex in selectedConsole.ArchiveExtensions)
                exs += ex + ";";
            if (exs.Length > 1)
                textBox_archive_extesnions.Text = exs.Substring(0, exs.Length - 1);
        }
        private void checkBox_forNesArchiveTweeks_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_cahceOndisk.Checked = true;
        }
    }
}
