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
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_DetectRomIcons : Form
    {
        public Form_DetectRomIcons(string consoleID)
        {
            InitializeComponent();
            Trace.WriteLine("Loading detect form...", "Detect Rom Icons");
            console = profileManager.Profile.Consoles[consoleID];
            textBox_extensions.Text = GetDefaultExtensionsJoined();
        }
        private EmulatorsOrganizer.Core.Console console;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private Thread mainThread;
        private string status;
        private int process;
        private string[] defaultExtensions = new string[] { ".exe", ".ico", ".jpg", ".png", ".bmp", ".gif", ".jpeg", ".tiff", ".tif", ".tga", ".ico" };
        // Options
        private List<string> foldersToSearch;
        private List<string> extensions;
        private bool includeSubFolders;
        private bool useRomNameInsteadRomFileName;
        private bool matchCase;
        private bool matchWord;
        private bool reverse;
        private bool searchmode_FileInRom;
        private bool searchmode_RomInFile;
        private bool searchmode_Both;
        private bool startWith;
        private bool contains;
        private bool endWith;
        private bool dontAllowSameFileDetectedByMoreThanOneRom;
        private bool removeSymbols;
        private bool finished;

        private void SEARCH()
        {
            ServicesManager.OnDisableWindowListner();

            Trace.WriteLine("Detect process started at " + DateTime.Now.ToLocalTime().ToString(), "Detect Rom Icons");
            Trace.WriteLine(status = "Loading roms collection ...", "Detect Rom Icons");
            int matchedRoms = 0;
            Rom[] roms = null;
            if (reverse)
                roms = profileManager.Profile.Roms[console.ID, true, false];// Get it sorted Z to A
            else
                roms = profileManager.Profile.Roms[console.ID, true, true];// Get it sorted A to Z
            Trace.WriteLine(status = "Loading files collection ...", "Detect Rom Icons");
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
                Trace.WriteLine("Detecting for rom: " + roms[i].Name, "Detect Rom Icons");

                // Loop through files, look for files for this rom
                for (int j = 0; j < files.Count; j++)
                {
                    if (!extensions.Contains(Path.GetExtension(files[j]).ToLower()))
                    {
                        Trace.WriteLine("File ignored (no match for extension): " + files[j], "Detect Rom Icons");
                        // Useless file ...
                        files.RemoveAt(j);
                        j--;
                        continue;
                    }
                    if (FilterSearch(roms[i], files[j]))
                    {
                        matchedRoms++;
                        string fileToAdd = HelperTools.GetDotPath(files[j]);
                        Trace.WriteLine("File matches the search: " + fileToAdd, "Detect Rom Icons");
                        Trace.WriteLine(">Setting file as icon: " + fileToAdd, "Detect Rom Icons");
                        if (Path.GetExtension(files[j]).ToLower() == ".exe" | Path.GetExtension(files[j]).ToLower() == ".ico")
                        {
                            roms[i].Icon = Icon.ExtractAssociatedIcon(files[j]).ToBitmap();
                        }
                        else
                        {
                            //Stream str = new FileStream(files[j], FileMode.Open, FileAccess.Read);
                            //byte[] buff = new byte[str.Length];
                            //str.Read(buff, 0, (int)str.Length);
                            //str.Dispose();
                            //str.Close();

                            //roms[i].Icon = (Bitmap)Image.FromStream(new MemoryStream(buff));
                            using (var bmpTemp = new Bitmap(files[j]))
                            {
                                roms[i].Icon = new Bitmap(bmpTemp);
                            }
                            Trace.WriteLine("->Rom[" + roms[i].ID + "] icon updated with: " + fileToAdd, "Detect Rom Icons");
                        }
                        roms[i].Icon = roms[i].Icon.GetThumbnailImage(16, 16, null, IntPtr.Zero);
                        // To reduce process, delete detected file
                        if (dontAllowSameFileDetectedByMoreThanOneRom)
                        {
                            files.RemoveAt(j);
                            j--;
                        }

                        // if (oneFilePerRom) !! only one file can be added as icon
                        break;
                    }
                }
                // Update progress
                process = (i * 100) / roms.Length;
                status = string.Format(ls["Status_DetectingRoms"] + " {0} / {1} [{2} ROM(S) OK][{3} %]", (i + 1),
                    roms.Length, matchedRoms, process);
            }
            // Done !
            Trace.WriteLine("Detect rom icons process finished at " + DateTime.Now.ToLocalTime().ToString(), "Detect Rom Icons");
            finished = true;
            Trace.WriteLine("----------------------------");
            ServicesManager.OnEnableWindowListner();
            CloseWin();
        }
        private string GetDefaultExtensionsJoined()
        {
            string val = "";
            if (this.defaultExtensions != null)
                foreach (string ex in defaultExtensions)
                    val += ex + ";";
            if (val.Length > 0)
                val = val.Substring(0, val.Length - 1);
            return val;
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

                searchWord_ROM = searchWord_ROM.Replace("|", " ");
                searchWord_ROM = searchWord_ROM.Replace(@"\", " ");
                searchWord_ROM = searchWord_ROM.Replace("/", " ");
                searchWord_ROM = searchWord_ROM.Replace("*", " ");
                searchWord_ROM = searchWord_ROM.Replace("?", " ");
                searchWord_ROM = searchWord_ROM.Replace("<", " ");
                searchWord_ROM = searchWord_ROM.Replace(">", " ");
                searchWord_ROM = searchWord_ROM.Replace("_", " ");
                searchWord_ROM = searchWord_ROM.Replace("!", " ");
                searchWord_ROM = searchWord_ROM.Replace("&", " ");
                searchWord_ROM = searchWord_ROM.Replace("-", " ");
                searchWord_ROM = searchWord_ROM.Replace("'", " ");
                searchWord_ROM = searchWord_ROM.Replace(".", " ");
                searchWord_ROM = searchWord_ROM.Replace(@"""", " ");
                searchWord_ROM = searchWord_ROM.Replace(" ", "");

                searchTargetText_FILE = searchTargetText_FILE.Replace("|", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace(@"\", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("/", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("*", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("?", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("<", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace(">", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("_", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("!", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("&", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("-", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace("'", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace(".", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace(@"""", " ");
                searchTargetText_FILE = searchTargetText_FILE.Replace(" ", "");
            }
            if (!matchWord)// Contain or IS
            {
                if (searchWord_ROM.Length == searchTargetText_FILE.Length)
                {
                    if (searchTargetText_FILE == searchWord_ROM)
                        return true;
                }
                // Contains
                else
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
            reverse = checkBox_reverse.Checked;
            includeSubFolders = checkBox_includeSubFolders.Checked;
            matchCase = checkBox_matchCase.Checked;
            matchWord = checkBox_matchWord.Checked;
            dontAllowSameFileDetectedByMoreThanOneRom = checkBox_dontAllowMoreThanOneFile.Checked;

            useRomNameInsteadRomFileName = checkBox_useRomNameInstead.Checked;
            searchmode_FileInRom = radioButton_searchmode_fileinrom.Checked;
            searchmode_RomInFile = radioButton_searchmode_rominfile.Checked;
            searchmode_Both = radioButton_searchmode_both.Checked;
            startWith = radioButton_startWith.Checked;
            contains = radioButton_contains.Checked;
            endWith = radioButton_endwith.Checked;
            removeSymbols = checkBox_removeSymbols.Checked;
            finished = false;
            // Disable things
            listView1.Enabled = button1.Enabled = button2.Enabled = textBox_extensions.Enabled
                = checkBox_includeSubFolders.Enabled = checkBox_matchCase.Enabled = checkBox_matchWord.Enabled = button3.Enabled =
            checkBox_dontAllowMoreThanOneFile.Enabled = checkBox_useRomNameInstead.Enabled =
            button5.Enabled = checkBox_reverse.Enabled = radioButton_searchmode_both.Enabled =
            radioButton_searchmode_fileinrom.Enabled = radioButton_searchmode_rominfile.Enabled =
           radioButton_contains.Enabled = radioButton_endwith.Enabled = radioButton_startWith.Enabled =
           checkBox_removeSymbols.Enabled = false;
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
        }
        // Reset extensions
        private void button5_Click(object sender, EventArgs e)
        {
            textBox_extensions.Text = GetDefaultExtensionsJoined();
        }
        private void checkBox_matchWord_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = groupBox2.Enabled = !checkBox_matchWord.Checked;
        }
    }
}
