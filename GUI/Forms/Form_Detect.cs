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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_Detect : Form
    {
        public Form_Detect(string consoleID, string icID)
        {
            Trace.WriteLine("Loading detect form...", "Detect Files");
            console = profileManager.Profile.Consoles[consoleID];
            container = (InformationContainerFiles)console.GetInformationContainer(icID);// Must be files type
            InitializeComponent();
            this.Text = ls["Title_DetectWindow"] + " " + container.DisplayName + " [" + console.Name + "]";
            Trace.WriteLine("Loading extensions ...", "Detect Files");
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
            ToggleAdvancedSettings();
        }
        private EmulatorsOrganizer.Core.Console console;
        private InformationContainerFiles container;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private Thread mainThread;
        private string status;
        private int process;
        private bool isAdvacnedSettingsVisible = true;
        // Options
        private List<string> foldersToSearch;
        private List<string> extensions;
        private List<string> symbolsToRemove;
        private bool includeSubFolders;
        private bool clearOldRomDetectedFiles;
        private bool mode_folder_folder;
        private bool useRomNameInsteadRomFileName;
        private bool matchCase;
        private bool matchWord;
        private bool oneFilePerRom;
        private bool reverse;
        private bool searchmode_FileInRom;
        private bool searchmode_RomInFile;
        private bool searchmode_Both;
        private bool startWith;
        private bool contains;
        private bool endWith;
        private bool dontAllowSameFileDetectedByMoreThanOneRom;
        private bool removeSymbols;
        private bool removeSymbolsSpecified;
        private bool finished;

        private void SEARCH()
        {
            ServicesManager.OnDisableWindowListner();

            Trace.WriteLine("Detect process started at " + DateTime.Now.ToLocalTime().ToString(), "Detect Files");
            Trace.WriteLine(status = "Loading roms collection ...", "Detect Files");
            int matchedRoms = 0;
            Rom[] roms = null;
            if (reverse)
                roms = profileManager.Profile.Roms[console.ID, true, false];// Get it sorted Z to A
            else
                roms = profileManager.Profile.Roms[console.ID, true, true];// Get it sorted A to Z
            if (!mode_folder_folder)
            {
                // NORMAL MODE
                Trace.WriteLine(status = "Loading files collection ...", "Detect Files");
                List<string> files = new List<string>();
                foreach (string folder in foldersToSearch)
                {
                    files.AddRange(Directory.GetFiles(folder, "*",
                        includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
                }
                if (reverse)
                    files.Sort(new TextComparer(false));// Sort Z to A
                else
                    files.Sort(new TextComparer(true));// Sort A to Z
                                                       // Start the operation, loop through roms
                for (int i = 0; i < roms.Length; i++)
                {
                    Trace.WriteLine("Detecting for rom: " + roms[i].Name, "Detect Files");
                    // Clear detected files first ?
                    if (clearOldRomDetectedFiles)
                    {
                        Trace.WriteLine("Clearing information container items for rom: " + roms[i].Name, "Detect Files");
                        roms[i].DeleteInformationContainerItems(container.ID);
                    }
                    // Loop through files, look for files for this rom
                    for (int j = 0; j < files.Count; j++)
                    {
                        if (!extensions.Contains(Path.GetExtension(files[j]).ToLower()))
                        {
                            Trace.WriteLine("File ignored (no match for extension): " + files[j], "Detect Files");
                            // Useless file ...
                            files.RemoveAt(j);
                            j--;
                            continue;
                        }
                        if (FilterSearch(roms[i], files[j]))
                        {
                            matchedRoms++;
                            string fileToAdd = HelperTools.GetDotPath(files[j]);
                            Trace.WriteLine("File matches the search: " + fileToAdd, "Detect Files");
                            Trace.WriteLine("Adding file: " + fileToAdd, "Detect Files");
                            if (roms[i].RomInfoItems != null)
                            {
                                if (!roms[i].IsInformationContainerItemExist(container.ID))
                                {
                                    // Create new
                                    InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), container.ID);
                                    item.Files.Add(fileToAdd);
                                    roms[i].RomInfoItems.Add(item);
                                    roms[i].Modified = true;
                                }
                                else
                                {
                                    // Update
                                    foreach (InformationContainerItem item in roms[i].RomInfoItems)
                                    {
                                        if (item.ParentID == container.ID)
                                        {
                                            InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                                            if (ictem.Files == null)
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
                                roms[i].RomInfoItems = new List<InformationContainerItem>();
                                // Create new
                                InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), container.ID);
                                item.Files.Add(fileToAdd);
                                roms[i].RomInfoItems.Add(item);
                                roms[i].Modified = true;
                            }
                            Trace.WriteLine("File added: " + files[j], "Detect Files");
                            // To reduce process, delete detected file
                            if (dontAllowSameFileDetectedByMoreThanOneRom)
                            {
                                files.RemoveAt(j);
                                j--;
                            }

                            if (oneFilePerRom)
                                break;
                        }
                    }
                    // Update progress
                    process = (i * 100) / roms.Length;
                    status = string.Format(ls["Status_DetectingRoms"] + " {0} / {1} [{2} ROM(S) OK][{3} %]", (i + 1),
                        roms.Length, matchedRoms, process);
                }
            }
            else
            {
                // FOLDER-FOLDER MODE
                Trace.WriteLine(status = "Loading files collection ...", "Detect Files");
                bool first = false;
                for (int g = 0; g < foldersToSearch.Count; g++)
                {
                    List<string> files = new List<string>();
                    files.AddRange(Directory.GetFiles(foldersToSearch[g], "*",
                        includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));

                    if (reverse)
                        files.Sort(new TextComparer(false));// Sort Z to A
                    else
                        files.Sort(new TextComparer(true));// Sort A to Z
                                                           // Start the operation, loop through roms
                    for (int i = 0; i < roms.Length; i++)
                    {
                        Trace.WriteLine("Detecting for rom: " + roms[i].Name, "Detect Files");
                        // Clear detected files first ?
                        if (clearOldRomDetectedFiles && !first)
                        {
                            Trace.WriteLine("Clearing information container items for rom: " + roms[i].Name, "Detect Files");
                            roms[i].DeleteInformationContainerItems(container.ID);
                        }
                        // Loop through files, look for files for this rom
                        for (int j = 0; j < files.Count; j++)
                        {
                            if (!extensions.Contains(Path.GetExtension(files[j]).ToLower()))
                            {
                                Trace.WriteLine("File ignored (no match for extension): " + files[j], "Detect Files");
                                // Useless file ...
                                files.RemoveAt(j);
                                j--;
                                continue;
                            }
                            if (FilterSearch(roms[i], files[j]))
                            {
                                matchedRoms++;
                                string fileToAdd = HelperTools.GetDotPath(files[j]);
                                Trace.WriteLine("File matches the search: " + fileToAdd, "Detect Files");
                                Trace.WriteLine("Adding file: " + fileToAdd, "Detect Files");
                                if (roms[i].RomInfoItems != null)
                                {
                                    if (!roms[i].IsInformationContainerItemExist(container.ID))
                                    {
                                        // Create new
                                        InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), container.ID);
                                        item.Files.Add(fileToAdd);
                                        roms[i].RomInfoItems.Add(item);
                                        roms[i].Modified = true;
                                    }
                                    else
                                    {
                                        // Update
                                        foreach (InformationContainerItem item in roms[i].RomInfoItems)
                                        {
                                            if (item.ParentID == container.ID)
                                            {
                                                InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                                                if (ictem.Files == null)
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
                                    roms[i].RomInfoItems = new List<InformationContainerItem>();
                                    // Create new
                                    InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), container.ID);
                                    item.Files.Add(fileToAdd);
                                    roms[i].RomInfoItems.Add(item);
                                    roms[i].Modified = true;
                                }
                                Trace.WriteLine("File added: " + files[j], "Detect Files");
                                // To reduce process, delete detected file
                                if (dontAllowSameFileDetectedByMoreThanOneRom)
                                {
                                    files.RemoveAt(j);
                                    j--;
                                }

                                if (oneFilePerRom)
                                    break;
                            }
                        }

                        // Update progress
                        process = (i * 100) / roms.Length;
                        status = string.Format(ls["Status_DetectingRoms"] + " {0} / {1} [{2} ROM(S) OK][{3} %]", (i + 1),
                            roms.Length, matchedRoms, process);
                    }
                    first = true;
                }
            }
            // Done !
            Trace.WriteLine("Detect process finished at " + DateTime.Now.ToLocalTime().ToString(), "Detect Files");
            finished = true;
            Trace.WriteLine("----------------------------");
            ServicesManager.OnEnableWindowListner();
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
            timer1.Stop();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private bool FilterSearch(Rom rom, string filePath)
        {
            // Let's see what's the mode
            string searchWord_ROM = "";
            string searchTargetText_FILE = "";
            if (!useRomNameInsteadRomFileName)
            {
                searchWord_ROM = matchCase ? Path.GetFileNameWithoutExtension(HelperTools.GetPathFromAIPath(rom.Path)) : Path.GetFileNameWithoutExtension(HelperTools.GetPathFromAIPath(rom.Path)).ToLower();
                searchTargetText_FILE = matchCase ? Path.GetFileNameWithoutExtension(filePath) : Path.GetFileNameWithoutExtension(filePath).ToLower();
            }
            else
            {
                searchWord_ROM = matchCase ? rom.Name : rom.Name.ToLower();
                searchTargetText_FILE = matchCase ? Path.GetFileNameWithoutExtension(filePath) : Path.GetFileNameWithoutExtension(filePath).ToLower();
            }
            if (removeSymbols)
            {
                string temp_searchWord_ROM = "";
                string temp_searchTargetText_FILE = "";
                for (int i = 0; i < searchWord_ROM.Length; i++)
                {
                    if (searchWord_ROM[i] != '(' && searchWord_ROM[i] != '[')
                        temp_searchWord_ROM += searchWord_ROM[i];
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
                for (int i = 0; i < searchTargetText_FILE.Length; i++)
                {
                    if (searchTargetText_FILE[i] != '(' && searchTargetText_FILE[i] != '[')
                        temp_searchTargetText_FILE += searchTargetText_FILE[i];
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
                searchWord_ROM = temp_searchWord_ROM;
                searchTargetText_FILE = temp_searchTargetText_FILE;

                searchWord_ROM = searchWord_ROM.Replace("|", "");
                searchWord_ROM = searchWord_ROM.Replace(@"\", "");
                searchWord_ROM = searchWord_ROM.Replace("/", "");
                searchWord_ROM = searchWord_ROM.Replace("*", "");
                searchWord_ROM = searchWord_ROM.Replace("?", "");
                searchWord_ROM = searchWord_ROM.Replace("<", "");
                searchWord_ROM = searchWord_ROM.Replace(">", "");
                searchWord_ROM = searchWord_ROM.Replace("_", "");
                searchWord_ROM = searchWord_ROM.Replace("!", "");
                searchWord_ROM = searchWord_ROM.Replace("&", "");
                searchWord_ROM = searchWord_ROM.Replace("-", "");
                searchWord_ROM = searchWord_ROM.Replace("'", "");
                searchWord_ROM = searchWord_ROM.Replace(".", "");
                searchWord_ROM = searchWord_ROM.Replace(@"""", "");
                searchWord_ROM = searchWord_ROM.Replace(" ", "");

                searchTargetText_FILE = searchTargetText_FILE.Replace("|", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace(@"\", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("/", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("*", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("?", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("<", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace(">", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("_", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("!", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("&", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("-", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace("'", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace(".", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace(@"""", "");
                searchTargetText_FILE = searchTargetText_FILE.Replace(" ", "");
            }
            if (removeSymbolsSpecified)
            {
                foreach (string ss in symbolsToRemove)
                {
                    if (ss != "" && ss != " ")
                    {
                        searchWord_ROM = searchWord_ROM.Replace(ss, "");
                        searchTargetText_FILE = searchTargetText_FILE.Replace(ss, "");
                    }
                }
            }
            if (!matchWord)// Contain or IS
            {
                if (searchWord_ROM.Length == searchTargetText_FILE.Length)
                {
                    if (searchTargetText_FILE == searchWord_ROM)
                        return true;
                }
                else// Contains
                {
                    if (searchmode_Both)
                    {
                        if (searchWord_ROM.Length > searchTargetText_FILE.Length)
                        {
                            if (contains)
                            {
                                if (searchWord_ROM.Contains(searchTargetText_FILE))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchWord_ROM.StartsWith(searchTargetText_FILE))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchWord_ROM.EndsWith(searchTargetText_FILE))
                                    return true;
                            }
                        }
                        else
                        {
                            if (contains)
                            {
                                if (searchTargetText_FILE.Contains(searchWord_ROM))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchTargetText_FILE.StartsWith(searchWord_ROM))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchTargetText_FILE.EndsWith(searchWord_ROM))
                                    return true;
                            }
                        }
                    }
                    else if (searchmode_FileInRom)
                    {
                        if (searchWord_ROM.Length > searchTargetText_FILE.Length)
                        {
                            if (contains)
                            {
                                if (searchWord_ROM.Contains(searchTargetText_FILE))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchWord_ROM.StartsWith(searchTargetText_FILE))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchWord_ROM.EndsWith(searchTargetText_FILE))
                                    return true;
                            }
                        }
                    }
                    else if (searchmode_RomInFile)
                    {
                        if (searchWord_ROM.Length < searchTargetText_FILE.Length)
                        {
                            if (contains)
                            {
                                if (searchTargetText_FILE.Contains(searchWord_ROM))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchTargetText_FILE.StartsWith(searchWord_ROM))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchTargetText_FILE.EndsWith(searchWord_ROM))
                                    return true;
                            }
                        }
                    }
                }
            }
            else// IS
            {
                if (searchWord_ROM.Length == searchTargetText_FILE.Length)
                {
                    if (searchTargetText_FILE == searchWord_ROM)
                        return true;
                }
            }

            return false;
        }
        private void ToggleAdvancedSettings()
        {
            isAdvacnedSettingsVisible = !isAdvacnedSettingsVisible;
            if (isAdvacnedSettingsVisible)
            {
                linkLabel1.Text = "Hide advanced settings";
                groupBox_advance_settings.Visible = true;
                this.Size = new Size(507, 637);
            }
            else
            {
                linkLabel1.Text = "Show advanced settings";
                groupBox_advance_settings.Visible = false;
                this.Size = new Size(507, 342);
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
            // Get options
            foldersToSearch = new List<string>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (Directory.Exists(item.Text))
                    foldersToSearch.Add(item.Text);
            }
            extensions = new List<string>(textBox_extensions.Text.ToLower().Split(new char[] { ';' }));
            symbolsToRemove = new List<string>(textBox_symbols_to_remove.Text.ToLower().Split(new char[] { ',' }));
            reverse = checkBox_reverse.Checked;
            includeSubFolders = checkBox_includeSubFolders.Checked;
            clearOldRomDetectedFiles = checkBox_deleteOldDetected.Checked;
            matchCase = checkBox_matchCase.Checked;
            matchWord = checkBox_matchWord.Checked;
            dontAllowSameFileDetectedByMoreThanOneRom = checkBox_dontAllowMoreThanOneFile.Checked;
            oneFilePerRom = checkBox_oneFilePerRom.Checked;
            useRomNameInsteadRomFileName = checkBox_useRomNameInstead.Checked;
            searchmode_FileInRom = radioButton_searchmode_fileinrom.Checked;
            searchmode_RomInFile = radioButton_searchmode_rominfile.Checked;
            searchmode_Both = radioButton_searchmode_both.Checked;
            startWith = radioButton_startWith.Checked;
            contains = radioButton_contains.Checked;
            endWith = radioButton_endwith.Checked;
            removeSymbols = checkBox_removeSymbols.Checked;
            removeSymbolsSpecified = checkBox_remove_specified_symbols.Checked;
            mode_folder_folder = radioButton_folder_folder.Checked;
            finished = false;
            // Disable things
            listView1.Enabled = button1.Enabled = button2.Enabled = textBox_extensions.Enabled
            = checkBox_deleteOldDetected.Enabled = checkBox_includeSubFolders.Enabled =
            checkBox_matchCase.Enabled = checkBox_matchWord.Enabled = button3.Enabled =
            checkBox_oneFilePerRom.Enabled = checkBox_dontAllowMoreThanOneFile.Enabled = checkBox_useRomNameInstead.Enabled =
            button5.Enabled = checkBox_reverse.Enabled = radioButton_searchmode_both.Enabled =
            radioButton_searchmode_fileinrom.Enabled = radioButton_searchmode_rominfile.Enabled =
           radioButton_contains.Enabled = radioButton_endwith.Enabled = radioButton_startWith.Enabled =
           checkBox_removeSymbols.Enabled = groupBox4.Enabled = button7.Enabled = button8.Enabled = false;
            progressBar1.Visible = label_status.Visible = groupBox1.Enabled = groupBox2.Enabled = true;
            timer1.Start();
            button4.Text = ls["Button_Stop"];
            // Start the thread !
            ServicesManager.OnDisableWindowListner();
            mainThread = new Thread(new ThreadStart(SEARCH));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }
        // Status timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status.Text = status;
            progressBar1.Value = process;
        }
        private void Form_Detect_FormClosing(object sender, FormClosingEventArgs e)
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
        // Reset extensions
        private void button5_Click(object sender, EventArgs e)
        {
            textBox_extensions.Text = container.GetDefaultExtensionsJoined();
        }
        private void checkBox_matchWord_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = groupBox2.Enabled = !checkBox_matchWord.Checked;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ToggleAdvancedSettings();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            textBox_symbols_to_remove.Text = "(,),[,]";
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        // Move selected folder up
        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowMessage("Please select one folder !!");
                return;
            }
            string folder = listView1.SelectedItems[0].Text;
            int index = listView1.SelectedItems[0].Index;
            if (index == 0)
                return;
            listView1.Items.RemoveAt(index);
            index--;

            listView1.Items.Insert(index, folder, 0);
            listView1.Items[index].Selected = true;
        }
        // Move selected folder down
        private void button8_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowMessage("Please select one folder !!");
                return;
            }
            string folder = listView1.SelectedItems[0].Text;
            int index = listView1.SelectedItems[0].Index;
            listView1.Items.RemoveAt(index);
            index++;

            if (index < listView1.Items.Count)
            {
                listView1.Items.Insert(index, folder, 0);
                listView1.Items[index].Selected = true;
            }
            else
            {
                listView1.Items.Add(folder, 0);
                listView1.Items[listView1.Items.Count - 1].Selected = true;
            }
        }
    }
}
