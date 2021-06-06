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
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_AddMameConsole : Form
    {
        public Form_AddMameConsole()
        {
            InitializeComponent();
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private TextWriterTraceListener listner;
        private DatabaseFile db_format;
        private string logPath;
        private string databaseFilePath;
        private string consoleName;
        private bool includeSubFolders;
        private bool verifyFiles;
        public string NewConsoleID;
        private bool buildDefaultICS;
        private List<string> romFolders = new List<string>();
        private Thread mainThread;
        private string status = "";
        private int progressValue = 0;
        private bool finished;

        private void PROCESS()
        {
            db_format.ProgressStarted += NoIntroDB_Progress;
            db_format.ProgressFinished += NoIntroDB_Progress;
            db_format.Progress += NoIntroDB_Progress;
            List<string> romsAdded = new List<string>();
            List<string> notFoundRoms = new List<string>();
            // Disable main window listener then add listener
            ServicesManager.OnDisableWindowListner();
            // Add listener
            logPath = ".\\Logs\\" + DateTime.Now.ToLocalTime().ToString() + "-add mame console.txt";
            logPath = logPath.Replace(":", "");
            logPath = logPath.Replace("/", "-");
            listner = new TextWriterTraceListener(HelperTools.GetFullPath(logPath));
            Trace.Listeners.Add(listner);
            // Start
            Trace.WriteLine("Add MAME console started at " +
                DateTime.Now.ToLocalTime(), "Add MAME console");

            Trace.WriteLine("Loading database file: " + databaseFilePath, "Add MAME console");
            // Get database content
            List<DBEntry> databaseEntries = db_format.LoadFile(databaseFilePath);
            Trace.WriteLine("Done, total " + databaseEntries.Count + " entry/entries found.", "Add MAME console");
            // Get console roms collection
            Trace.WriteLine("Loading rom files from folders", "Add MAME console");
            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
            List<string> romFiles = new List<string>();
            foreach (string folder in romFolders)
            {
                string[] files = Directory.GetFiles(folder, "*",
                    includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (archiveExtensions.Contains(Path.GetExtension(file).ToLower()))
                        romFiles.Add(file);
                }
            }
            // Loop through roms, do what you have to do !
            Trace.WriteLine("Operation started ...", "Add MAME console");
            // Create the console
            Trace.WriteLine("Creating new console ...", "Add MAME console");
            string parentGroupID = "";
            if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup)
                parentGroupID = profileManager.Profile.SelectedConsolesGroupID;
            EmulatorsOrganizer.Core.Console console =
                new EmulatorsOrganizer.Core.Console(profileManager.Profile.GenerateID(), parentGroupID);
            int g = 1;
            string name = consoleName;
            while (profileManager.Profile.Consoles.Contains(name, parentGroupID, ""))
            {
                g++;
                name = consoleName + g;
            }
            // Console configuration (for mame special)
            console.Name = name;
            console.ExtractRomIfArchive = false;
            #region ICS
            if (buildDefaultICS)
            {
                console.AutoSwitchTabPriorityDepend = true;
                console.InformationContainers = new List<InformationContainer>();

                // Generate display map
                console.InformationContainersMap = new InformationContainerTabsPanel();
                console.InformationContainersMap.IsHorizontal = true;
                // Add at the top panel
                console.InformationContainersMap.TopPanel = new InformationContainerTabsPanel();

                InformationContainerRomInfo ic_rom = new InformationContainerRomInfo(profileManager.Profile.GenerateID());
                ic_rom.DisplayName = "Rom Info";
                console.InformationContainers.Add(ic_rom);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_rom.ID);

                InformationContainerImage ic_snaps = new InformationContainerImage(profileManager.Profile.GenerateID());
                ic_snaps.DisplayName = "Snaps";
                console.InformationContainers.Add(ic_snaps);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_snaps.ID);

                InformationContainerImage ic_flyers = new InformationContainerImage(profileManager.Profile.GenerateID());
                ic_flyers.DisplayName = "Flyers";
                console.InformationContainers.Add(ic_flyers);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_flyers.ID);

                InformationContainerImage ic_cabinets = new InformationContainerImage(profileManager.Profile.GenerateID());
                ic_cabinets.DisplayName = "Cabinets";
                console.InformationContainers.Add(ic_cabinets);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_cabinets.ID);

                InformationContainerImage ic_marquees = new InformationContainerImage(profileManager.Profile.GenerateID());
                ic_marquees.DisplayName = "Marquees";
                console.InformationContainers.Add(ic_marquees);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_marquees.ID);

                InformationContainerImage ic_titles = new InformationContainerImage(profileManager.Profile.GenerateID());
                ic_titles.DisplayName = "Titles";
                console.InformationContainers.Add(ic_titles);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_titles.ID);

                InformationContainerImage ic_cpanel = new InformationContainerImage(profileManager.Profile.GenerateID());
                ic_cpanel.DisplayName = "Cpanel";
                console.InformationContainers.Add(ic_cpanel);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_cpanel.ID);

                InformationContainerImage ic_pcb = new InformationContainerImage(profileManager.Profile.GenerateID());
                ic_pcb.DisplayName = "Pcb";
                console.InformationContainers.Add(ic_pcb);
                console.InformationContainersMap.TopPanel.ContainerIDS.Add(ic_pcb.ID);

                // Add this to the bottom
                console.InformationContainersMap.BottomPanel = new InformationContainerTabsPanel();
                console.InformationContainersMap.BottomPanel.TopPanel = new InformationContainerTabsPanel();
                console.InformationContainersMap.BottomPanel.BottomPanel = new InformationContainerTabsPanel();

                InformationContainerReviewScore ic_review = new InformationContainerReviewScore(profileManager.Profile.GenerateID());
                ic_review.DisplayName = "Review/Score";
                console.InformationContainers.Add(ic_review);
                console.InformationContainersMap.BottomPanel.TopPanel.ContainerIDS.Add(ic_review.ID);

                InformationContainerInfoText ic_info = new InformationContainerInfoText(profileManager.Profile.GenerateID());
                ic_info.DisplayName = "Info";
                console.InformationContainers.Add(ic_info);
                console.InformationContainersMap.BottomPanel.BottomPanel.ContainerIDS.Add(ic_info.ID);

                InformationContainerInfoText ic_history = new InformationContainerInfoText(profileManager.Profile.GenerateID());
                ic_history.DisplayName = "History";
                console.InformationContainers.Add(ic_history);
                console.InformationContainersMap.BottomPanel.BottomPanel.ContainerIDS.Add(ic_history.ID);

                InformationContainerPDF ic_manuals = new InformationContainerPDF(profileManager.Profile.GenerateID());
                ic_manuals.DisplayName = "Manuals";
                console.InformationContainers.Add(ic_manuals);
                console.InformationContainersMap.BottomPanel.BottomPanel.ContainerIDS.Add(ic_manuals.ID);
            }
            #endregion
            console.ParentAndChildrenMode = true;// Set parent and children mode by default.
            profileManager.Profile.Consoles.AddNoEvent(console);
            Trace.WriteLine("Console added: " + name, "Add MAME console");

            int applyed = 0;
            for (int i = 0; i < databaseEntries.Count; i++)
            {
                string theClone = databaseEntries[i].GetPropertyValue("Clone Of");
                bool isCloneOf = theClone != "";
                string theRom = databaseEntries[i].GetPropertyValue("Rom Of");
                bool isRomOf = theRom != "";

                bool romMatch = false;
                string matchedFilePath = "";
                // Search rom files !
                foreach (string file in romFiles)
                {
                    if (!isRomOf)
                    {
                        foreach (string f in databaseEntries[i].FileNames)
                        {
                            romMatch = f.ToLower() == Path.GetFileNameWithoutExtension(file).ToLower();
                            if (romMatch) break;
                        }
                    }
                    else
                    {
                        // A child !
                        romMatch = theRom.ToLower() == Path.GetFileNameWithoutExtension(file).ToLower();
                    }
                    if (romMatch)
                    {
                        Trace.WriteLine("*File located: " + file, "Add MAME console");
                        matchedFilePath = file;
                        break;
                    }
                }
                string entryName = databaseEntries[i].GetPropertyValue("Name");
                // If the rom match, add it to the roms collection
                if (romMatch)
                {
                    #region Verify
                    if (verifyFiles)
                    {
                        Trace.WriteLine("*Verifying file: " + matchedFilePath, "Add MAME console");
                        // Do extra check !
                        SevenZipExtractor extractor = new SevenZipExtractor(matchedFilePath);
                        SevenZipExtractor extractor1 = null;
                        SevenZipExtractor extractor2 = null;
                        bool RomOK = true;
                        if (isCloneOf)
                        {
                            if (File.Exists(Path.GetDirectoryName
                                               (matchedFilePath) +
                                               "//" + theClone + Path.GetExtension(matchedFilePath)))
                            {
                                extractor1 = new SevenZipExtractor(Path.GetDirectoryName
                                               (matchedFilePath) +
                                               "//" + theClone + Path.GetExtension(matchedFilePath));
                            }
                            else
                            {
                                isCloneOf = false;
                            }
                        }
                        if (isRomOf)
                        {
                            if (File.Exists(Path.GetDirectoryName
                                               (matchedFilePath) +
                                               "//" + theRom + Path.GetExtension(matchedFilePath)))
                            {
                                extractor2 = new SevenZipExtractor(Path.GetDirectoryName
                                               (matchedFilePath) +
                                               "//" + theRom + Path.GetExtension(matchedFilePath));
                            }
                            else
                            {
                                isRomOf = false;
                            }
                        }
                        for (int o = 4; o < databaseEntries[i].PerfectMatchCRCS.Count; o++)
                        {
                            bool found = false;
                            try
                            {
                                foreach (ArchiveFileInfo inf in extractor.ArchiveFileData)
                                {
                                    if (databaseEntries[i].PerfectMatchCRCS[o].CRC != "")
                                        if (uint.Parse(databaseEntries[i].PerfectMatchCRCS[o].CRC, System.Globalization.NumberStyles.AllowHexSpecifier) == inf.Crc)
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
                                    if (databaseEntries[i].PerfectMatchCRCS[o].CRC != "")
                                        if (uint.Parse(databaseEntries[i].PerfectMatchCRCS[o].CRC, System.Globalization.NumberStyles.AllowHexSpecifier) == inf.Crc)
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
                                    if (databaseEntries[i].PerfectMatchCRCS[o].CRC != "")
                                        if (uint.Parse(databaseEntries[i].PerfectMatchCRCS[o].CRC, System.Globalization.NumberStyles.AllowHexSpecifier) == inf.Crc)
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
                        romMatch = RomOK;
                    }
                    #endregion
                    if (romMatch)
                    {
                        // Still match !!
                        Trace.WriteLine(">Adding file: " + matchedFilePath, "Add MAME console");
                        Rom newRom = new Rom(profileManager.Profile.GenerateID());
                        newRom.ParentConsoleID = console.ID;
                        newRom.Name = databaseEntries[i].GetPropertyValue("Description");
                        newRom.FileSize = HelperTools.GetSizeAsBytes(matchedFilePath);
                        // Make path
                        if (isRomOf)
                        {
                            newRom.Path = Path.GetDirectoryName(matchedFilePath) + "\\" + entryName;
                            newRom.IgnorePathNotExist = true;
                        }
                        else
                        {
                            newRom.Path = matchedFilePath;
                            newRom.IgnorePathNotExist = false;
                            newRom.ChildrenRoms = new List<string>();
                            // Get children for this rom
                            foreach (DBEntry childEntry in databaseEntries)
                            {
                                string childClone = childEntry.GetPropertyValue("Clone Of");
                                bool childisCloneOf = childClone != "";
                                string childchildRom = childEntry.GetPropertyValue("Rom Of");
                                bool childisRomOf = childchildRom != "";

                                if (childisRomOf || childisCloneOf)
                                {
                                    if (childClone == entryName ||
                                        childchildRom == entryName)
                                    {
                                        newRom.ChildrenRoms.Add(childEntry.GetPropertyValue("Name"));
                                    }
                                }
                            }
                        }
                        // Database info items
                        db_format.ApplyName(newRom, databaseEntries[i], true, true);

                        profileManager.Profile.Roms.Add(newRom, false);
                        Trace.WriteLine("->Rom added: " + newRom.Name, "Add MAME console");
                        applyed++;
                        romsAdded.Add(newRom.Name);
                    }
                    else
                    {
                        notFoundRoms.Add("DB Entry [NOT MATCH AFTER VERIFY]: " + entryName + "; FILE: '" + matchedFilePath + "' ");
                    }
                }
                else
                {
                    notFoundRoms.Add("DB Entry [NO FILE FOUND]: " + entryName + " ");
                }
                // Progress
                progressValue = (i * 100) / databaseEntries.Count;
                status = ls["Status_AddingRoms"] + " " + (i + 1).ToString() + " / " + databaseEntries.Count +
                    " [" + applyed + " OK][" + progressValue + " %]";
            }
            // Linking up roms phase ...
            Trace.WriteLine("Linking up parents and children ...", "Add MAME console");
            Rom[] roms = profileManager.Profile.Roms[console.ID, false];
            for (int r = 0; r < roms.Length; r++)
            {
                if (roms[r].ChildrenRoms.Count > 0)
                {
                    // Set this rom as a parent
                    List<string> childrenList = roms[r].ChildrenRoms;
                    profileManager.Profile.Roms[roms[r].ID].ChildrenRoms = new List<string>();

                    // Translate the children ...
                    foreach (string childName in childrenList)
                    {
                        for (int c = 0; c < roms.Length; c++)
                        {
                            if (roms[c].Path.Contains(childName))
                            {
                                Trace.WriteLine("-->Adding child: " + childName);
                                // Add it's id
                                profileManager.Profile.Roms[roms[r].ID].ChildrenRoms.Add(roms[c].ID);
                                // Make the child rom as child
                                profileManager.Profile.Roms[roms[c].ID].IsChildRom = true;
                                profileManager.Profile.Roms[roms[c].ID].IsParentRom = false;
                                profileManager.Profile.Roms[roms[c].ID].ParentRomID = roms[r].ID;
                            }
                        }
                    }
                    if (profileManager.Profile.Roms[roms[r].ID].ChildrenRoms.Count > 0)
                        profileManager.Profile.Roms[roms[r].ID].IsParentRom = true;
                }
                progressValue = (r * 100) / roms.Length;
                status = ls["Status_LinkingRoms"] + " " + (r + 1).ToString() + " / " + roms.Length +
                    "[" + progressValue + " %]";
            }
            // Update log with matched and not found roms
            Trace.WriteLine("----------------------------");
            Trace.WriteLine("ROMS ADDED");
            Trace.WriteLine("------------");
            for (int i = 0; i < romsAdded.Count; i++)
                Trace.WriteLine((i + 1).ToString("D8") + "." + romsAdded[i]);

            Trace.WriteLine("----------------------------");
            Trace.WriteLine("ROMS NOT FOUND");
            Trace.WriteLine("--------------");
            for (int i = 0; i < notFoundRoms.Count; i++)
                Trace.WriteLine((i + 1).ToString("D8") + "." + notFoundRoms[i]);

            Trace.WriteLine("----------------------------");

            ServicesManager.OnEnableWindowListner();
            Trace.WriteLine("Add MAME console finished at " + DateTime.Now.ToLocalTime(), "Add MAME console");
            listner.Flush();
            Trace.Listeners.Remove(listner);
            // Make EO select this console !
            NewConsoleID = console.ID;
            console.FixColumnsForRomDataInfo();

            CloseAfterFinish();
        }
        private void AddNewIC(string newic, EmulatorsOrganizer.Core.Console console)
        {
            //search the profile for information container that match the same name
            bool found = false;
            foreach (RomData ic in console.RomDataInfoElements)
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
                console.RomDataInfoElements.Add(newIC);
            }
        }
        private void AddDataToRom(string icName, string data, Rom rom, EmulatorsOrganizer.Core.Console console)
        {
            foreach (RomData ic in console.RomDataInfoElements)
            {
                if (ic.Name.ToLower() == icName.ToLower())
                {
                    rom.UpdateDataInfoItemValue(ic.ID, data);
                    break;
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
            timer1.Stop();
            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(ls["Message_Done"] + ", " + ls["Message_LogFileSavedTo"] + @" '" + logPath + "'",
          ls["MessageCaption_AddMameConsole"], new string[] { ls["Button_Ok"], ls["Button_OpenLog"] },
          0, null, ManagedMessageBoxIcon.Info);
            if (res.ClickedButtonIndex == 1)
            {
                try { Process.Start(HelperTools.GetFullPath(logPath)); }
                catch (Exception ex)
                { ManagedMessageBox.ShowErrorMessage(ex.Message); }
            }
            profileManager.Profile.RecentSelectedType = SelectionType.Console;
            profileManager.Profile.SelectedConsoleID = NewConsoleID;
            profileManager.Profile.OnConsoleAdd();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = ls["Title_OpenMameDatDatabaseFile"];
            op.Filter = ls["Filter_Mame"];
            op.FileName = textBox_databaseFile.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_databaseFile.Text = op.FileName;

                if (op.FilterIndex == 1)
                {
                    db_format = new DatabaseFile_MameDat();
                }
                if (op.FilterIndex == 2)
                {
                    db_format = new DatabaseFile_MameXML();
                }
                if (op.FilterIndex == 3)
                {
                    db_format = new DatabaseFile_HyperListXML();
                    ((DatabaseFile_HyperListXML)db_format)._rename_using_description_instead_of_name = true;
                }
            }
        }
        // Start
        private void button5_Click(object sender, EventArgs e)
        {
            // Poke options
            if (!File.Exists(textBox_databaseFile.Text))
            {
                ManagedMessageBox.ShowMessage(ls["Message_DatabaseFileIsNotExist"], ls["MessageCaption_AddMameConsole"]);
                return;
            }
            if (textBox_consoleName.Text.Length == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseEnterConsoleNameFirst"], ls["MessageCaption_AddMameConsole"]);
                return;
            }
            if (listBox_folders.Items.Count == 0)
            {
                ManagedMessageBox.ShowMessage(ls["Message_TheresNoFolder"], ls["MessageCaption_AddMameConsole"]);
                return;
            }
            consoleName = textBox_consoleName.Text;
            buildDefaultICS = checkBox_bulidDefaultICS.Checked;
            includeSubFolders = checkBox_subfolders.Checked;
            databaseFilePath = textBox_databaseFile.Text;
            verifyFiles = checkBox_verifyFiles.Checked;
            romFolders = new List<string>();
            foreach (string item in listBox_folders.Items)
                if (Directory.Exists(item))
                    romFolders.Add(item);

            // Enable/Disable things
            button1.Enabled = button2.Enabled = button3.Enabled = button5.Enabled =
            textBox_consoleName.Enabled = listBox_folders.Enabled =
            checkBox_bulidDefaultICS.Enabled = checkBox_subfolders.Enabled = checkBox_verifyFiles.Enabled = false;
            timer1.Start();
            // Start thread !
            mainThread = new Thread(new ThreadStart(PROCESS));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status.Text = status;
            progressBar1.Value = progressValue;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void NoIntroDB_Progress(object sender, ProgressArgs e)
        {
            status = e.Status;
            progressValue = e.Completed;
        }
        private void Form_MameConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_AddMameConsole"]);
                        if (result.ClickedButtonIndex == 0)
                        {
                            mainThread.Abort();
                            mainThread = null;
                            ServicesManager.OnEnableWindowListner();
                            Trace.WriteLine("Add MAME console finished at " + DateTime.Now.ToLocalTime(), "Add MAME console");
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
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = ls["Title_AddFolder"];
            if (listBox_folders.SelectedIndex >= 0)
                fol.SelectedPath = listBox_folders.SelectedItem.ToString();
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                if (!listBox_folders.Items.Contains(fol.SelectedPath))
                    listBox_folders.Items.Add(fol.SelectedPath);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox_folders.SelectedIndex >= 0)
                listBox_folders.Items.RemoveAt(listBox_folders.SelectedIndex);
        }
    }
}
