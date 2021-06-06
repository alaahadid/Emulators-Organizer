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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_ClearProfile : Form
    {
        public Form_ClearProfile()
        {
            InitializeComponent();
            checkBox_deleteMissingRomFiles_CheckedChanged(this, null);
        }

        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private string logPath;
        private Thread mainThread;
        private int progress;
        private string status;
        private bool _finished;
        private bool _deleteMissingRomFiles;
        private bool _deleteRelatedFilesFirst;
        private bool _dontDeleteRomIfHaveRelatedFiles;
        private bool _removeEmptyConsoleGroups;
        private bool _removeEmptyConsoles;
        private bool _removeEmptyPlaylists;
        private bool _removePlaylistGroups;
        private bool _removeUnneededAssignments;
        private bool _removeUnusedEmulators;
        private bool _clearLogsFolder;

        private void PROGRESS()
        {
            // Add listener
            logPath = ".\\Logs\\" + DateTime.Now.ToLocalTime().ToString() + "-clean up.txt";
            logPath = logPath.Replace(":", "");
            logPath = logPath.Replace("/", "-");
            TextWriterTraceListener listner = new TextWriterTraceListener(logPath);
            Trace.Listeners.Add(listner);
            ServicesManager.OnDisableWindowListner();
            Trace.WriteLine("Clean up profile started at: " + DateTime.Now.ToLocalTime().ToString(), "Clear Profile");
            #region Remove empty console groups
            if (_removeEmptyConsoleGroups)
            {
                Trace.WriteLine("Removing empty console groups ....", "Clear Profile");

                progress = 0;
                for (int i = 0; i < profileManager.Profile.ConsoleGroups.Count; i++)
                {
                    // See if this group has consoles or not
                    string id = profileManager.Profile.ConsoleGroups[i].ID;
                    EmulatorsOrganizer.Core.Console[] consoles = profileManager.Profile.Consoles[id, false];
                    bool performDelete = false;
                    if (consoles == null)
                        performDelete = true;
                    else if (consoles.Length == 0)
                        performDelete = true;
                    if (performDelete)
                    {
                        // Delete it !
                        Trace.WriteLine(">Consoles group removed: " + profileManager.Profile.ConsoleGroups[i].Name, "Clear Profile");
                        profileManager.Profile.ConsoleGroups.RemoveAt(i, false);
                        i--;
                    }
                    // Status and progress
                    status = ls["Status_RemovingEmptyConsoleGroups"] + GetProgressStatus(i, profileManager.Profile.ConsoleGroups.Count);
                    progress = CalculatePrec(i, profileManager.Profile.ConsoleGroups.Count);
                }
            }
            #endregion
            #region Remove empty playlist groups
            if (_removePlaylistGroups)
            {
                Trace.WriteLine("Removing empty playlist groups ....", "Clear Profile");

                progress = 0;
                for (int i = 0; i < profileManager.Profile.PlaylistGroups.Count; i++)
                {
                    // See if this group has consoles or not
                    string id = profileManager.Profile.PlaylistGroups[i].ID;
                    Playlist[] playlists = profileManager.Profile.Playlists[id, false];
                    bool performDelete = false;
                    if (playlists == null)
                        performDelete = true;
                    else if (playlists.Length == 0)
                        performDelete = true;
                    if (performDelete)
                    {
                        // Delete it !
                        Trace.WriteLine(">Playlists group removed: " + profileManager.Profile.PlaylistGroups[i].Name, "Clear Profile");
                        profileManager.Profile.PlaylistGroups.RemoveAt(i, false);
                        i--;
                    }
                    // Status and progress
                    status = ls["Status_RemovingEmptyPlaylistGroups"] + GetProgressStatus(i, profileManager.Profile.PlaylistGroups.Count);
                    progress = CalculatePrec(i, profileManager.Profile.PlaylistGroups.Count);
                }
            }
            #endregion
            #region Remove empty consoles
            // we need to load all roms of all consoles anyway ...
                Trace.WriteLine("Loading all roms and removing empty consoles ....", "Clear Profile");
                progress = 0;
                for (int i = 0; i < profileManager.Profile.Consoles.Count; i++)
                {
                    // See if this group has consoles or not
                    string id = profileManager.Profile.Consoles[i].ID;
                    Rom[] roms = profileManager.Profile.Roms[id, false];
                    bool performDelete = false;
                    if (_removeEmptyConsoles)
                    {
                        if (roms == null)
                            performDelete = true;
                        else if (roms.Length == 0)
                            performDelete = true;
                    }
                    if (performDelete)
                    {
                        // Delete it !
                        Trace.WriteLine(">Console removed: " + profileManager.Profile.Consoles[i].Name, "Clear Profile");
                        profileManager.Profile.Consoles.RemoveAt(i, false);
                        i--;
                    }
                    // Status and progress
                    status = ls["Status_RemovingEmptyConsoles"] + GetProgressStatus(i, profileManager.Profile.Consoles.Count);
                    progress = CalculatePrec(i, profileManager.Profile.Consoles.Count);
                }
            #endregion
            #region Remove empty playlists
            if (_removeEmptyPlaylists)
            {
                Trace.WriteLine("Removing empty playlists ....", "Clear Profile");

                progress = 0;
                for (int i = 0; i < profileManager.Profile.Playlists.Count; i++)
                {
                    // See if this group has consoles or not
                    string id = profileManager.Profile.Playlists[i].ID;

                    Playlist pl = profileManager.Profile.Playlists[i];
                    Rom[] roms = profileManager.Profile.Roms[pl.RomIDS.ToArray()];
                    bool performDelete = false;
                    if (roms == null)
                        performDelete = true;
                    else if (roms.Length == 0)
                        performDelete = true;
                    if (performDelete)
                    {
                        // Delete it !
                        Trace.WriteLine(">Playlist removed: " + profileManager.Profile.Playlists[i].Name, "Clear Profile");
                        profileManager.Profile.Playlists.RemoveAt(i);
                        i--;
                    }
                    // Status and progress
                    status = ls["Status_RemovingEmptyPlaylists"] + GetProgressStatus(i, profileManager.Profile.Playlists.Count);
                    progress = CalculatePrec(i, profileManager.Profile.Playlists.Count);
                }
            }
            #endregion
            #region Remove unused emulators
            if (_removeUnusedEmulators)
            {
                Trace.WriteLine("Removing unused emulators ....", "Clear Profile");

                progress = 0;
                for (int i = 0; i < profileManager.Profile.Emulators.Count; i++)
                {
                    bool performDelete = false;
                    if (profileManager.Profile.Emulators[i].ParentConsoles == null)
                        performDelete = true;
                    else if (profileManager.Profile.Emulators[i].ParentConsoles.Count == 0)
                        performDelete = true;
                    if (performDelete)
                    {
                        // Delete it !
                        Trace.WriteLine(">Emulator removed: " + profileManager.Profile.Emulators[i].Name);
                        profileManager.Profile.Emulators.RemoveAt(i);
                        i--;
                    }
                    // Status and progress
                    status = ls["Status_RemovingUnusedEmulators"] + GetProgressStatus(i, profileManager.Profile.Emulators.Count);
                    progress = CalculatePrec(i, profileManager.Profile.Emulators.Count);
                }
            }
            #endregion
            #region Remove roms with missing files
            if (_deleteMissingRomFiles)
            {
                Trace.WriteLine("Removing roms with missing files ....", "Clear Profile");

                progress = 0;
                for (int i = 0; i < profileManager.Profile.Roms.Count; i++)
                {
                    bool performDelete = false;
                    // Check rom file
                    if (!profileManager.Profile.Roms[i].IgnorePathNotExist)
                        if (!HelperTools.IsAIPath((profileManager.Profile.Roms[i].Path)))
                            if (!File.Exists(HelperTools.GetFullPath(profileManager.Profile.Roms[i].Path)))
                                performDelete = true;
                    // See if rom has releated files and need to cancel remove if it is !
                    if (_dontDeleteRomIfHaveRelatedFiles)
                    {
                        if (profileManager.Profile.Roms[i].RomInfoItems != null)
                        {
                            if (profileManager.Profile.Roms[i].RomInfoItems.Count > 0)
                            {
                                foreach (InformationContainerItem it in profileManager.Profile.Roms[i].RomInfoItems)
                                {
                                    if (it is InformationContainerItemFiles)
                                    {
                                        InformationContainerItemFiles itf = (InformationContainerItemFiles)it;
                                        if (itf.Files != null)
                                            if (itf.Files.Count > 0)
                                            // This is it lol
                                            {
                                                Trace.WriteLine(">Rom remove canceled, rom have related files.", "Clear Profile");
                                                performDelete = false;
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                    if (performDelete)
                    {
                        // Remove related files from disk first !
                        if (_deleteRelatedFilesFirst)
                        {
                            if (profileManager.Profile.Roms[i].RomInfoItems != null)
                            {
                                if (profileManager.Profile.Roms[i].RomInfoItems.Count > 0)
                                {
                                    foreach (InformationContainerItem it in profileManager.Profile.Roms[i].RomInfoItems)
                                    {
                                        if (it is InformationContainerItemFiles)
                                        {
                                            InformationContainerItemFiles itf = (InformationContainerItemFiles)it;
                                            if (itf.Files != null)
                                                foreach (string rr in itf.Files)
                                                {
                                                    try
                                                    {
                                                        File.Delete(HelperTools.GetFullPath(rr));
                                                        Trace.WriteLine(">>Rom related file removed: " + rr, "Clear Profile");
                                                    }
                                                    catch
                                                    {
                                                        Trace.WriteLine(">>! UNABLE to remove related file: " + rr, "Clear Profile");
                                                    }
                                                }
                                        }
                                    }
                                }
                            }
                        }
                        // Delete it !
                        Trace.WriteLine(">Rom removed: " + profileManager.Profile.Roms[i].Name);
                        profileManager.Profile.Roms.RemoveAt(i);
                        i--;

                        // Status and progress
                        status = ls["Status_RemovingRomsWithMissingFiles"] + GetProgressStatus(i, profileManager.Profile.Roms.Count);
                        progress = CalculatePrec(i, profileManager.Profile.Roms.Count);
                    }
                    else if (_removeUnneededAssignments)
                    {
                        // See if rom has releated files and need to cancel remove if it is !
                        if (profileManager.Profile.Roms[i].RomInfoItems != null)
                        {
                            if (profileManager.Profile.Roms[i].RomInfoItems.Count > 0)
                            {
                                for (int j = 0; j < profileManager.Profile.Roms[i].RomInfoItems.Count; j++)
                                {
                                    if (profileManager.Profile.Roms[i].RomInfoItems[j] is InformationContainerItemFiles)
                                    {
                                        InformationContainerItemFiles itf = (InformationContainerItemFiles)profileManager.Profile.Roms[i].RomInfoItems[j];
                                        bool performDeleteUnneeded = false;
                                        if (itf.Files == null)
                                            performDeleteUnneeded = true;
                                        else if (itf.Files.Count == 0)
                                            performDeleteUnneeded = true;
                                        if (performDeleteUnneeded)
                                        {
                                            Trace.WriteLine(">>Rom related assignment: " + itf.ParentID, "Clear Profile");
                                            profileManager.Profile.Roms[i].RomInfoItems.RemoveAt(j);
                                            j--;
                                        }
                                    }
                                    else if (profileManager.Profile.Roms[i].RomInfoItems[j] is InformationContainerItemLinks)
                                    {
                                        InformationContainerItemLinks itl = (InformationContainerItemLinks)profileManager.Profile.Roms[i].RomInfoItems[j];
                                        bool performDeleteUnneeded = false;
                                        if (itl.Links == null)
                                            performDeleteUnneeded = true;
                                        else if (itl.Links.Count == 0)
                                            performDeleteUnneeded = true;
                                        if (performDeleteUnneeded)
                                        {
                                            Trace.WriteLine(">>Rom related assignment: " + itl.ParentID, "Clear Profile");
                                            profileManager.Profile.Roms[i].RomInfoItems.RemoveAt(j);
                                            profileManager.Profile.Roms[i].Modified = true;
                                            j--;
                                        }
                                    }
                                }
                            }
                        }
                        // Status and progress
                        status = ls["Status_RemovingUnneededAssignments"] + GetProgressStatus(i, profileManager.Profile.Roms.Count);
                        progress = CalculatePrec(i, profileManager.Profile.Roms.Count);
                    }
                }
            }
            #endregion
            #region Clear Logs Folder
            if (_clearLogsFolder)
            {
                Trace.WriteLine("Clearing the logs folder. ....", "Clear Profile");
                string[] files = Directory.GetFiles(Path.GetFullPath(HelperTools.StartUpPath + "\\Logs"));
                string[] folders = Directory.GetDirectories(Path.GetFullPath(HelperTools.StartUpPath + "\\Logs"));
                Trace.WriteLine(">Deleting files ...", "Clear Profile");
                int i = 0;
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { Trace.WriteLine(">ERROR: Unable to delete file: " + file, "Clear Profile"); }
                    // Status and progress
                    status = ls["Status_DeletingFilesOfLogsFolder"] + GetProgressStatus(i, files.Length);
                    progress = CalculatePrec(i, files.Length);
                    i++;
                }
                Trace.WriteLine(">Deleting folders ...", "Clear Profile");
                i = 0;
                foreach (string folder in folders)
                {
                    try
                    {
                        Directory.Delete(folder);
                    }
                    catch { Trace.WriteLine(">ERROR: Unable to delete folder: " + folder, "Clear Profile"); }
                    status = ls["Status_DeletingFoldersOfLogsFolder"] + GetProgressStatus(i, folders.Length);
                    progress = CalculatePrec(i, folders.Length);
                    i++;
                }
                Trace.WriteLine("Logs folder cleared.", "Clear Profile");
            }
            #endregion
            status = ls["Status_Done"];
            progress = 100;
            ServicesManager.OnEnableWindowListner();
            Trace.WriteLine("Clean up profile finished at " + DateTime.Now.ToLocalTime(), "Clear Profile");
            listner.Flush();
            Trace.Listeners.Remove(listner);
            CloseAfterFinish();
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
            _finished = true;
            timer1.Stop();
            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(ls["Message_Done"] + ", " + ls["Message_LogFileSavedTo"] + @" '" + logPath + "'",
          ls["MessageCaption_ClearProfile"], new string[] { ls["Button_Ok"], ls["Button_OpenLog"] },
          0, null, ManagedMessageBoxIcon.Info); profileManager.Profile.OnRomsAdd();
            if (res.ClickedButtonIndex == 1) { try { Process.Start(HelperTools.GetFullPath(logPath)); } catch { } }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private int CalculatePrec(int i, int max)
        {
            if (i < 0)
                i = 0;
            if (max == 0)
                return 0;
            return (i * 100) / max;
        }
        private string GetProgressStatus(int i, int max)
        {
            if (i < 0) i = 0;
            return (i + 1) + " / " + max + " [" + CalculatePrec(i, max) + " %]";
        }
        private void checkBox_deleteMissingRomFiles_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_dontDeleteRomIfHaveRelatedFiles.Enabled = checkBox_deleteMissingRomFiles.Checked;
            checkBox_deleteRelatedFilesFirst.Enabled = checkBox_deleteMissingRomFiles.Checked;
            if (checkBox_deleteMissingRomFiles.Checked)
                checkBox_removeUnneededAssignments.Enabled = checkBox_dontDeleteRomIfHaveRelatedFiles.Checked;
            else
                checkBox_removeUnneededAssignments.Enabled = true;
        }
        private void checkBox_dontDeleteRomIfHaveRelatedFiles_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_deleteMissingRomFiles_CheckedChanged(this, null);
        }
        // Total Clear
        private void button1_Click(object sender, EventArgs e)
        {
            checkBox_deleteMissingRomFiles.Checked = true;
            checkBox_deleteRelatedFilesFirst.Checked = false;
            checkBox_dontDeleteRomIfHaveRelatedFiles.Checked = false;
            checkBox_removeEmptyConsoleGroups.Checked = true;
            checkBox_removeEmptyConsoles.Checked = true;
            checkBox_removeEmptyPlaylists.Checked = true;
            checkBox_removeUnneededAssignments.Checked = true;
            checkBox_removeUnusedEmulators.Checked = true;
            checkBox_removePlaylistGroups.Checked = true;
            checkBox_clearLogsFolder.Checked = true;
        }
        // Default
        private void button2_Click(object sender, EventArgs e)
        {
            checkBox_deleteMissingRomFiles.Checked = true;
            checkBox_deleteRelatedFilesFirst.Checked = false;
            checkBox_dontDeleteRomIfHaveRelatedFiles.Checked = true;
            checkBox_removeEmptyConsoleGroups.Checked = false;
            checkBox_removeEmptyConsoles.Checked = false;
            checkBox_removeEmptyPlaylists.Checked = true;
            checkBox_removeUnneededAssignments.Checked = true;
            checkBox_removeUnusedEmulators.Checked = true;
            checkBox_removePlaylistGroups.Checked = false;
            checkBox_clearLogsFolder.Checked = true;
        }
        // Roms only
        private void button3_Click(object sender, EventArgs e)
        {
            checkBox_deleteMissingRomFiles.Checked = true;
            checkBox_deleteRelatedFilesFirst.Checked = false;
            checkBox_dontDeleteRomIfHaveRelatedFiles.Checked = true;
            checkBox_removeEmptyConsoleGroups.Checked = false;
            checkBox_removeEmptyConsoles.Checked = false;
            checkBox_removeEmptyPlaylists.Checked = false;
            checkBox_removeUnneededAssignments.Checked = true;
            checkBox_removeUnusedEmulators.Checked = false;
            checkBox_removePlaylistGroups.Checked = false;
            checkBox_clearLogsFolder.Checked = false;
        }
        // Close
        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Form_ClearProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_finished)
                return;
            if (mainThread != null)
                if (mainThread.IsAlive)
                {
                    ManagedMessageBoxResult result =
                        ManagedMessageBox.ShowMessage(
                       ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                       ls["MessageCaption_ClearProfile"],
                       new string[] {
                           ls["Button_Yes"],
                           ls["Button_No"] },
                           0, ManagedMessageBoxIcon.Question);
                    if (result.ClickedButtonIndex == 1)// No not sure
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        mainThread.Abort();
                    }
                }
        }
        // Begin !
        private void button4_Click(object sender, EventArgs e)
        {
            if (mainThread != null)
                if (mainThread.IsAlive)
                    return;
            // Check
            if (!checkBox_deleteMissingRomFiles.Checked &&
                !checkBox_deleteRelatedFilesFirst.Checked &&
                !checkBox_dontDeleteRomIfHaveRelatedFiles.Checked &&
                !checkBox_removeEmptyConsoleGroups.Checked &&
                !checkBox_removeEmptyConsoles.Checked &&
                !checkBox_removeEmptyPlaylists.Checked &&
                !checkBox_removePlaylistGroups.Checked &&
                !checkBox_removeUnneededAssignments.Checked &&
                !checkBox_removeUnusedEmulators.Checked)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_NothingToDoPleaseSelectAtLeastOneOption"], ls["MessageCaption_ClearProfile"]);
                return;
            }

            // Poke options
            _deleteMissingRomFiles = checkBox_deleteMissingRomFiles.Checked;
            _deleteRelatedFilesFirst = checkBox_deleteRelatedFilesFirst.Checked;
            _dontDeleteRomIfHaveRelatedFiles = checkBox_dontDeleteRomIfHaveRelatedFiles.Checked;
            _removeEmptyConsoleGroups = checkBox_removeEmptyConsoleGroups.Checked;
            _removeEmptyConsoles = checkBox_removeEmptyConsoles.Checked;
            _removeEmptyPlaylists = checkBox_removeEmptyPlaylists.Checked;
            _removePlaylistGroups = checkBox_removePlaylistGroups.Checked;
            _removeUnneededAssignments = checkBox_removeUnneededAssignments.Checked;
            _removeUnusedEmulators = checkBox_removeUnusedEmulators.Checked;
            _clearLogsFolder = checkBox_clearLogsFolder.Checked;
            // Disable things !
            groupBox1.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = false;
            // Launch the thread
            mainThread = new Thread(new ThreadStart(PROGRESS));
            mainThread.CurrentUICulture = ls.CultureInfo;// Necessary for multilingual support
            mainThread.Start();
            // Start timer
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status.Text = status;
            progressBar1.Value = progress;
        }
    }
}
