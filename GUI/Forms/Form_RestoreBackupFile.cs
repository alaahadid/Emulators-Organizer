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
using System.Diagnostics;
using System.IO;
using System.Threading;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
using SevenZip;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_RestoreBackupFile : Form
    {
        public Form_RestoreBackupFile(string consoleID, string icID)
        {
            Trace.WriteLine("Loading detect form...", "Detect Files");
            console = profileManager.Profile.Consoles[consoleID];
            container = (InformationContainerFiles)console.GetInformationContainer(icID);// Must be files type
            InitializeComponent();
            this.Text = ls["Title_DetectWindow"] + " " + container.DisplayName + " [" + console.Name + "]";
            Trace.WriteLine("Loading extenions ...", "Detect Files");
            // Load extensions
            textBox_extensions.Text = container.GetExtensionsJoined();
            // Load folders
            Trace.WriteLine("Loading folders ...", "Detect Files");
            foreach (string fol in container.FoldersMemory)
            {
                if (Directory.Exists(fol))
                    listView1.Items.Add(fol, 0);
                else
                    Trace.WriteLine("Folder can't be added; it's not exist. [" + fol + "]", "Detect Files");
            }
        }
        private EmulatorsOrganizer.Core.Console console;
        private InformationContainerFiles container;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private Thread mainThread;
        private string status;
        private int process;
        // Options
        private List<string> foldersToSearch;
        private List<string> extensions;
        private string backupFile;
        private bool includeSubFolders;
        private bool clearOldRomDetectedFiles;
        private bool finished;

        private void SEARCH()
        {
            Trace.WriteLine("Detect process started at " + DateTime.Now.ToLocalTime().ToString(), "Detect Files");
            Trace.WriteLine(status = "Loading roms collection ...", "Detect Files");
            Rom[] roms = profileManager.Profile.Roms[console.ID, false];
            Trace.WriteLine(status = "Loading files collection ...", "Detect Files");
            List<string> files = new List<string>();
            foreach (string folder in foldersToSearch)
            {
                files.AddRange(Directory.GetFiles(folder, "*",
                    includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            }
            Trace.WriteLine(status = "Reading backup file lines ...", "Detect Files");
            //now let's read the backup file
            string[] lines = File.ReadAllLines(backupFile);
            Dictionary<string, string[]> romSHAromFiles = new Dictionary<string, string[]>();
            Dictionary<string, string> imageSHAimagePATH = new Dictionary<string, string>();
            //read the buffer
            int j = 0;
            foreach (string imfile in files)
            {
                string sha = HelperTools.CalculateSHA1(imfile);
                if (!imageSHAimagePATH.Keys.Contains(sha))
                    imageSHAimagePATH.Add(sha, imfile);
                process = (j * 100) / files.Count;
                status = ls["Status_Buffering"] + " " + (j + 1).ToString() + "/" +
                    files.Count + " (" + process + "%)";
                j++;
            }
            //read the file
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "$Rom")
                {
                    i++;
                    string sha1 = "";
                    List<string> dbfiles = new List<string>();
                    while (true)
                    {
                        string[] codes = lines[i].Split(new char[] { '=' });
                        if (codes[0] == "Sha1")
                        {
                            sha1 = codes[1];
                        }
                        if (lines[i] == "$Files")
                        {
                            while (lines[i] != "")
                            {
                                codes = lines[i].Split(new char[] { '=' });
                                if (codes[0] == "Sha1")
                                {
                                    dbfiles.Add(codes[1]);
                                }
                                i++;
                            }
                            //add 
                            romSHAromFiles.Add(sha1, dbfiles.ToArray());
                            break;
                        }
                        i++;
                    }
                }
                process = (i * 100) / lines.Length;
                status = ls["Status_ReadingDatabaseFile"] + " " + (i + 1).ToString() + "/" +
              lines.Length + " (" + process + "%)";
            }

            // Start the operation, loop through roms
            for (int i = 0; i < roms.Length; i++)
            {
                string rompath = roms[i].Path;

                Trace.WriteLine("Detecting for rom: " + Path.GetFileName(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(roms[i].Path))), "Detect Files");
                if (HelperTools.IsAIPath(roms[i].Path))
                {
                    Trace.WriteLine("Rom is compressed, checking inside of it for a match", "Import Nes Cart database");
                    // Extract the content of the archive first
                    SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(roms[i].Path)));
                    int index = HelperTools.GetIndexFromAIPath(roms[i].Path);
                    Stream mstream = null;

                    // Try to extract and get data
                    try
                    {
                        mstream = new FileStream(rompath = Path.GetTempPath() + "\\test.tst", FileMode.Create, FileAccess.Write);
                        extractor.ExtractFile(index, mstream);
                        mstream.Close();
                        mstream.Dispose();
                    }
                    catch { }
                }
                else
                {
                    rompath = HelperTools.GetFullPath(roms[i].Path);
                }
                // Clear detected files first ?
                if (clearOldRomDetectedFiles)
                {
                    Trace.WriteLine("Clearing information container items for rom: " + Path.GetFileName(rompath), "Detect Files");
                    roms[i].DeleteInformationContainerItems(container.ID);
                }
                // See if match the backup.
                if (romSHAromFiles.Keys.Contains(HelperTools.CalculateSHA1(rompath)))
                {
                    string[] icfiles = romSHAromFiles[HelperTools.CalculateSHA1(rompath)];
                    InformationContainerItemFiles iccitem =
                        (InformationContainerItemFiles)roms[i].GetInformationContainerItem(container.ID);
                    if (iccitem == null)
                    {
                        iccitem = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), container.ID);
                        iccitem.Files = new List<string>();
                        roms[i].RomInfoItems.Add(iccitem);
                        roms[i].Modified = true;
                    }
                    if (iccitem.Files == null)
                        iccitem.Files = new List<string>();
                    //search the image files for the matched
                    foreach (string icfile in icfiles)
                    {
                        if (imageSHAimagePATH.Keys.Contains(icfile))
                        {
                            iccitem.Files.Add(HelperTools.GetDotPath(imageSHAimagePATH[icfile]));
                            break;
                        }
                    }
                }
                // Update progress
                process = (i * 100) / roms.Length;
                status = string.Format(ls["Status_DetectingRoms"] + " {0} / {1} [{2} %]", (i + 1), roms.Length, process);
            }
            // Done !
            Trace.WriteLine("Detect process finished at " + DateTime.Now.ToLocalTime().ToString(), "Detect Files");
            finished = true;
            CloseWin();
        }
        private void CloseWin()
        {
            if (!this.InvokeRequired)
                CloseWin1();
            else
                this.Invoke(new Action(CloseWin1));
        }
        private void CloseWin1()
        {
            ServicesManager.OnEnableWindowListner();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        // Open backup file
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = ls["Filter_BackupFile"];
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_backupFile.Text = op.FileName;
                //add the folders
                string[] lines = File.ReadAllLines(op.FileName);
                string log = "";
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == "$Folders")
                    {
                        while (i < lines.Length)
                        {
                            string[] codes = lines[i].Split(new char[] { '=' });
                            if (codes.Length > 1)
                            {
                                if (Directory.Exists(codes[1]))
                                {
                                    // Make sure the folder is not already exist in the list
                                    bool found = false;
                                    foreach (ListViewItem item in listView1.Items)
                                    {
                                        if (item.Text == codes[1])
                                        {
                                            found = true; break;
                                        }
                                    }
                                    if (!found)
                                        listView1.Items.Add(codes[1], 0);
                                }
                                else
                                {
                                    log += codes[1] + "\n";
                                }
                            }
                            i++;
                        }
                    }
                }
                if (log != "")
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_FoldersNotExist"] + ":\n" + log,
                      ls["MessageCaption_DeleteInformationContainer"]);
                }
            }
        }
        // Add folder
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            // Set selected directory
            if (listView1.Items.Count > 0)
                fol.SelectedPath = listView1.Items[listView1.Items.Count - 1].Text;
            else if (console.Memory_RomFolders != null)
            {
                if (console.Memory_RomFolders.Count > 0)
                {
                    fol.SelectedPath = console.Memory_RomFolders[console.Memory_RomFolders.Count - 1];
                }
            }
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Make sure the folder is not already exist in the list
                bool found = false;
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Text == fol.SelectedPath)
                    {
                        found = true; break;
                    }
                }
                if (!found)
                    listView1.Items.Add(fol.SelectedPath, 0);
            }
        }
        // Remove folder
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(
                ls["Message_AreYouSureToDeleteSelectedFolders"],
                ls["Title_Detect"],
                new string[] { ls["Button_Yes"], ls["Button_No"] }, 1, ManagedMessageBoxIcon.Question, false, false, "");
            if (res.ClickedButtonIndex == 0)// Yes
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Selected)
                    {
                        listView1.Items.RemoveAt(i);
                        i = -1;
                    }
                }
            }
        }
        // Reset extensions
        private void button5_Click(object sender, EventArgs e)
        {
            textBox_extensions.Text = container.GetDefaultExtensionsJoined();
        }
        // Cancel
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // Start
        private void button3_Click(object sender, EventArgs e)
        {
            // Make a check
            if (listView1.Items.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_NoFolderToSearchPleaseAddAtLeastOneFolder"], ls["Title_Detect"]);
                return;
            }
            if (textBox_extensions.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_NoExtensionAdded"], ls["Title_Detect"]);
                return;
            }
            if (!File.Exists(textBox_backupFile.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForBackupFile"], ls["Title_Detect"]);
                return;
            }
            // Get options
            foldersToSearch = new List<string>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (Directory.Exists(item.Text))
                    foldersToSearch.Add(item.Text);
            }
            backupFile = textBox_backupFile.Text;
            extensions = new List<string>(textBox_extensions.Text.ToLower().Split(new char[] { ';' }));
            includeSubFolders = checkBox_includeSubFolders.Checked;
            clearOldRomDetectedFiles = checkBox_deleteOldDetected.Checked;
            finished = false;
            // Disable things
            listView1.Enabled = button1.Enabled = button2.Enabled = textBox_extensions.Enabled
            = checkBox_deleteOldDetected.Enabled = checkBox_includeSubFolders.Enabled =
           button3.Enabled = false;
            progressBar1.Visible = label_status.Visible = true;
            timer1.Start();
            button4.Text = ls["Button_Stop"];
            // Start the thread !
            ServicesManager.OnDisableWindowListner();
            mainThread = new Thread(new ThreadStart(SEARCH));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status.Text = status;
            progressBar1.Value = process;
        }
        private void Form_RestoreBackupFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainThread != null && !finished)
            {
                if (mainThread.IsAlive)
                {
                    ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(
                                  ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                                  ls["Title_Detect"],
                                  new string[] { ls["Button_Yes"], ls["Button_No"] }, 1, ManagedMessageBoxIcon.Question, false, false, "");
                    if (res.ClickedButtonIndex == 0)// Yes
                    {
                        mainThread.Abort();
                    }
                    else
                    { e.Cancel = true; return; }
                }
            }
            // Save ....
            // Folders memory
            container.FoldersMemory = new List<string>();
            foreach (ListViewItem item in listView1.Items)
                if (Directory.Exists(item.Text))
                    container.FoldersMemory.Add(item.Text);
            // Extensions
            container.Extenstions = new List<string>(textBox_extensions.Text.ToLower().Split(new char[] { ';' }));
        }
    }
}
