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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using SevenZip;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_AddRoms : Form
    {
        public Form_AddRoms(string consoleID)
        {
            InitializeComponent();
            console = profileManager.Profile.Consoles[consoleID];

            button1_Click(this, null);
            textBox_archive_extensions.Text = ".7z;.rar;.zip;.gzip;.tar;.bzip2;.xz";
        }
        public Form_AddRoms(string consoleID, string[] filesToAdd)
        {
            InitializeComponent();
            console = profileManager.Profile.Consoles[consoleID];

            foreach (string file in filesToAdd)
            {
                if (console.Extensions.Contains(Path.GetExtension(file)))
                {
                    ListViewItem newItem = new ListViewItem();
                    newItem.Text = Path.GetFileName(file);
                    newItem.SubItems.Add(file);
                    newItem.Tag = file;
                    listView1.Items.Add(newItem);
                }
            }
            string ex = "";
            foreach (string ext in console.ArchiveExtensions)
                ex += ext + ";";
            textBox_archive_extensions.Text = ex;
        }
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private TextWriterTraceListener listner;
        private string logPath;
        private Core.Console console;
        private string status = "";
        private int progressValue = 0;
        private Thread mainThread;
        private bool finished = false;
        private bool _clearAllRoms = false;
        private bool _replaceRom = false;
        private bool _useAIPath = true;
        private bool _useParent = true;
        private bool _alwaysPlayChild = false;
        private List<string> _archiveExtensions;
        private delegate void SetProgressBarValue(int val);
        private delegate void SetStatus(string val);
        private List<string> files = new List<string>();
        private int added_roms_count;

        private void PROGRESS()
        {
            // Add listener
            logPath = ".\\Logs\\" + DateTime.Now.ToLocalTime().ToString() + "-add roms.txt";
            logPath = logPath.Replace(":", "");
            logPath = logPath.Replace("/", "-");
            listner = new TextWriterTraceListener(HelperTools.GetFullPath(logPath));
            Trace.Listeners.Add(listner);
            // Start
            Trace.WriteLine("Add roms to console '" + console.Name + "' started at " +
                DateTime.Now.ToLocalTime(), "Add roms");

            Trace.WriteLine("Total files found= " + files.Count + " file(s)", "Add roms");
            bool needToSerachForMatchedRom = true;
            added_roms_count = 0;
            // clear old collection if requested by user
            if (_clearAllRoms)
            {
                Trace.WriteLine("Clearing roms that belong to console '" + console.Name + "' by the request of the user.", "Add roms");
                profileManager.Profile.Roms.RemoveThatBelongToConsole(console.ID);
                Trace.WriteLine("Roms removed from console '" + console.Name + "'", "Add roms");
            }
            // if the console is clear, no need to search for matched roms inside it. Fast things up
            needToSerachForMatchedRom = profileManager.Profile.Roms[console.ID, false].Length > 0;
            // adding roms
            Trace.WriteLine("Adding roms ....", "Add roms");
            int i = 0;
            foreach (string file in files)
            {
                if (_useAIPath)
                {
                    // Check if that file is archive
                    if (_archiveExtensions.Contains(Path.GetExtension(file).ToLower()))
                    {
                        #region FILE IS ARCHIVE
                        string parentRomID = "";
                        if (_useParent)
                        {
                            Trace.WriteLine("File is archive ! adding file as parent !", "Add roms (folders scan)");
                            parentRomID = AddRomNotArchiveProcess(file, needToSerachForMatchedRom);
                            Trace.WriteLine("---->Rom assigned as a parent rom !!", "Add roms (folders scan)");
                        }
                        Trace.WriteLine("File is archive ! adding file content as AI roms...", "Add roms (folders scan)");
                        // This is it !!
                        SevenZipExtractor extractor = new SevenZipExtractor(file);
                        // Loop through files of that archive, add them one by one using AI path
                        foreach (ArchiveFileInfo inf in extractor.ArchiveFileData)
                        {
                            // AI path formula is: AI(<index>)<ArchiveFilePath>
                            string romFile = string.Format("AI({0}){1}", inf.Index, file);
                            string ex = Path.GetExtension(HelperTools.GetPathFromAIPath(romFile)).ToLower();

                            // Check if we can add this rom
                            if (!console.Extensions.Contains(ex))
                            {
                                Trace.WriteLine("Can't add file: " + romFile, "Add roms (folders scan)");
                                Trace.WriteLine("Extension " + Path.GetExtension(romFile) + " no supported by given console.", "Add roms (folders scan)");
                                goto SHOWPROGRESS;
                            }
                            // Look for this rom in the roms collection
                            if (needToSerachForMatchedRom)
                            {
                                string romID = "";
                                if (profileManager.Profile.Roms.ContainsByPath(romFile, out romID, console.ID))
                                {
                                    Trace.WriteLine("File already exist in database: " + romFile, "Add roms (folders scan)");
                                    if (_replaceRom)
                                    {
                                        Trace.WriteLine("Replacing the rom..", "Add roms (folders scan)");
                                        profileManager.Profile.Roms.Remove(romID, false);
                                        // create new rom with new id
                                        if (_useParent && parentRomID != "")
                                        {
                                            string newRomID = AddNewRom(inf.FileName, (long)inf.Size, romFile, parentRomID);
                                            profileManager.Profile.Roms[parentRomID].IsParentRom = true;
                                            profileManager.Profile.Roms[parentRomID].AlwaysChooseChildWhenPlay = checkBox_alwaysChooseChild.Checked;
                                            profileManager.Profile.Roms[parentRomID].ChildrenRoms.Add(newRomID);
                                            Trace.WriteLine("---->Rom assigned as a child of parent [" + parentRomID + "]", "Add roms (folders scan)");
                                        }
                                        else
                                            AddNewRom(inf.FileName, (long)inf.Size, romFile);
                                        Trace.WriteLine(">Rom replaced.", "Add roms (folders scan)");
                                    }
                                    else
                                    {
                                        Trace.WriteLine("Rom ignored by the request of the user.", "Add roms (folders scan)");
                                    }
                                }
                                else// Just add the rom !
                                {
                                    if (_useParent && parentRomID != "")
                                    {
                                        string newRomID = AddNewRom(inf.FileName, (long)inf.Size, romFile, parentRomID);
                                        profileManager.Profile.Roms[parentRomID].IsParentRom = true;
                                        profileManager.Profile.Roms[parentRomID].AlwaysChooseChildWhenPlay = checkBox_alwaysChooseChild.Checked;
                                        profileManager.Profile.Roms[parentRomID].ChildrenRoms.Add(newRomID);
                                        Trace.WriteLine("---->Rom assigned as a child of parent [" + parentRomID + "]", "Add roms (folders scan)");
                                    }
                                    else
                                        AddNewRom(inf.FileName, (long)inf.Size, romFile);
                                }
                            }
                            else// Just add the rom !
                            {
                                if (_useParent && parentRomID != "")
                                {
                                    string newRomID = AddNewRom(inf.FileName, (long)inf.Size, romFile, parentRomID);
                                    profileManager.Profile.Roms[parentRomID].IsParentRom = true;
                                    profileManager.Profile.Roms[parentRomID].AlwaysChooseChildWhenPlay = checkBox_alwaysChooseChild.Checked;
                                    profileManager.Profile.Roms[parentRomID].ChildrenRoms.Add(newRomID);
                                    Trace.WriteLine("---->Rom assigned as a child of parent [" + parentRomID + "]", "Add roms (folders scan)");
                                }
                                else
                                    AddNewRom(inf.FileName, (long)inf.Size, romFile);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Trace.WriteLine("File is not an archive ! adding file normally.", "Add roms");
                        AddRomNotArchiveProcess(file, needToSerachForMatchedRom);
                    }
                }
                else
                {
                    AddRomNotArchiveProcess(file, needToSerachForMatchedRom);
                }

            SHOWPROGRESS:
                int x = (i * 100) / files.Count;
                if (x != progressValue)
                {
                    progressValue = x;
                    status = ls["Status_AddingRoms"] + " " + i + " / " + files.Count + " [" + progressValue + " %]";
                    UpdateValues();
                }
                i++;
            }
            ServicesManager.OnEnableWindowListner();
            Trace.WriteLine("Add roms finished at " + DateTime.Now.ToLocalTime(), "Add roms");
            listner.Flush();
            Trace.Listeners.Remove(listner);
            CloseAfterFinish();
        }
        private string AddRomNotArchiveProcess(string file, bool needToSerachForMatchedRom)
        {
            // File is not archive, add it normally.
            string romFile = HelperTools.GetDotPath(file);
            string ex = Path.GetExtension(romFile).ToLower();

            // Check if we can add this rom
            if (!console.Extensions.Contains(ex))
            {
                Trace.WriteLine("Can't add file: " + romFile, "Add roms (folders scan)");
                Trace.WriteLine("Extension " + Path.GetExtension(romFile) + " no supported by given console.", "Add roms (folders scan)");
                return "";
            }
            // Look for this rom in the roms collection
            if (needToSerachForMatchedRom)
            {
                string romID = "";
                if (profileManager.Profile.Roms.ContainsByPath(romFile, out romID, console.ID))
                {
                    Trace.WriteLine("File already exist in database: " + romFile, "Add roms (folders scan)");
                    if (_replaceRom)
                    {
                        Trace.WriteLine("Replacing the rom..", "Add roms (folders scan)");
                        profileManager.Profile.Roms.Remove(romID, false);
                        // create new rom with new id
                        AddNewRom(Path.GetFileNameWithoutExtension(romFile), HelperTools.GetSizeAsBytes(romFile), romFile);
                        Trace.WriteLine(">Rom replaced.", "Add roms (folders scan)");
                    }
                    else
                    {
                        Trace.WriteLine("Rom ignored by the request of the user.", "Add roms (folders scan)");
                    }
                }
                else// Just add the rom !
                {
                    return AddNewRom(Path.GetFileNameWithoutExtension(romFile), HelperTools.GetSizeAsBytes(romFile), romFile);
                }
            }
            else// Just add the rom !
            {
                return AddNewRom(Path.GetFileNameWithoutExtension(romFile), HelperTools.GetSizeAsBytes(romFile), romFile);
            }
            return "";
        }
        private string AddNewRom(string name, long size, string romFile)
        {
            // create new rom with new id
            Rom newRom = new Rom(profileManager.Profile.GenerateID());
            newRom.Path = romFile;
            if (HelperTools.IsAIPath(romFile))
                newRom.IgnorePathNotExist = true;

            newRom.FileSize = size;
            newRom.Name = name;
            newRom.ParentConsoleID = console.ID;
            newRom.ChildrenRoms = new List<string>();
            newRom.IndexWithinConsole = added_roms_count;
            profileManager.Profile.Roms.Add(newRom, false);
            Trace.WriteLine(">Rom added: [" + newRom.ID + "] " + newRom.Name, "Add roms (folders scan)");

            added_roms_count++;
            return newRom.ID;
        }
        private string AddNewRom(string name, long size, string romFile, string parentRomID)
        {
            // create new rom with new id
            Rom newRom = new Rom(profileManager.Profile.GenerateID());
            newRom.Path = romFile;
            if (HelperTools.IsAIPath(romFile))
                newRom.IgnorePathNotExist = true;

            newRom.FileSize = size;
            newRom.Name = name;
            newRom.ParentConsoleID = console.ID;
            newRom.ChildrenRoms = new List<string>();
            newRom.IsParentRom = false;
            newRom.IsChildRom = true;
            newRom.ParentRomID = parentRomID;
            newRom.IndexWithinConsole = added_roms_count;
            profileManager.Profile.Roms.Add(newRom, false);
            Trace.WriteLine(">Rom added: [" + newRom.ID + "] " + newRom.Name, "Add roms (folders scan)");

            added_roms_count++;
            return newRom.ID;
        }
        private void SetBarMax(int max)
        {
            if (!this.InvokeRequired)
                SetBarMax1(max);
            else
                this.Invoke(new SetProgressBarValue(SetBarMax1), max);
        }
        private void SetBarMax1(int max)
        { progressBar1.Maximum = max; }
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
            listView1.Enabled = button1.Enabled = button2.Enabled = checkBox_clearRomsCollectionFirst.Enabled =
              checkBox_replaceExistedRoms.Enabled = button_start.Enabled = true;
            progressBar1.Visible = label2.Visible = false;
            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(ls["Message_Done"] + ", " + ls["Message_LogFileSavedTo"] + @" '" + logPath + "'",
          ls["MessageCaption_AddRomsFoldersScan"], new string[] { ls["Button_Ok"], ls["Button_OpenLog"] },
          0, null, ManagedMessageBoxIcon.Info); profileManager.Profile.OnRomsAdd();
            if (res.ClickedButtonIndex == 1) { try { Process.Start(HelperTools.GetFullPath(logPath)); } catch { } }

            // Select the console
            profileManager.Profile.RecentSelectedType = SelectionType.Console;
            profileManager.Profile.SelectedConsoleID = console.ID;
            profileManager.Profile.OnRomsAdd();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
 
            this.Close();
        }
        private void UpdateValues()
        {
            if (!this.InvokeRequired)
                UpdateValues1();
            else
                this.Invoke(new Action(UpdateValues1));
        }
        private void UpdateValues1()
        {
            progressBar1.Value = progressValue;
            label2.Text = status;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Add
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = ls["Title_AddRoms"];
            string filter = console.Name + " " + ls["Word_Roms"] + " (";
            if (console.Extensions.Count == 1)
            { filter += "*." + console.Extensions[0]; }
            else
            {
                foreach (string ext in console.Extensions)
                {

                    filter += (ext.Contains(".") ? "*" : "*.") + ext;
                    if (console.Extensions[console.Extensions.Count - 1] != ext)
                    { filter += ","; }
                }
            }
            filter += ")|";
            foreach (string ext in console.Extensions)
            {
                filter += (ext.Contains(".") ? "*" : "*.") + ext + ";";
            }
            op.Filter = filter;
            op.Multiselect = true;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in op.FileNames)
                {
                    bool found = false;
                    // Make sure this file is not in the list
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Tag.ToString() == file)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        ListViewItem newItem = new ListViewItem();
                        newItem.Text = Path.GetFileName(file);
                        newItem.SubItems.Add(file);
                        newItem.Tag = file;
                        listView1.Items.Add(newItem);
                    }
                }
            }
        }
        // Remove
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                listView1.Items.RemoveAt(listView1.SelectedItems[0].Index); i = -1;
            }
        }
        // Start
        private void button_start_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseAddRomsToTheListFirst"], ls["MessageCaption_AddRoms"]);
                return;
            }
            // disable things
            ServicesManager.OnDisableWindowListner();
            listView1.Enabled = button1.Enabled = button2.Enabled = checkBox_clearRomsCollectionFirst.Enabled =
                checkBox_use_ai_path.Enabled = textBox_archive_extensions.Enabled = linkLabel1.Enabled =
                checkBox_replaceExistedRoms.Enabled = button_start.Enabled = checkBox_alwaysChooseChild.Enabled =
                checkBox_useParent.Enabled = false;
            // get options
            _clearAllRoms = checkBox_clearRomsCollectionFirst.Checked;
            _replaceRom = checkBox_replaceExistedRoms.Checked;
            _useAIPath = checkBox_use_ai_path.Checked;
            _alwaysPlayChild = checkBox_alwaysChooseChild.Checked;
            _archiveExtensions = new List<string>(textBox_archive_extensions.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            _useParent = checkBox_useParent.Checked;
            // show bar and status
            progressBar1.Visible = label2.Visible = true;
            // add roms to the list
            files = new List<string>();
            foreach (ListViewItem item in listView1.Items)
            {
                files.Add(item.Tag.ToString());
            }
            // start !
            finished = false;
            timer1.Start();
            mainThread = new Thread(new ThreadStart(PROGRESS));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }

        private void Form_AddRoms_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_AddRoms"]);
                        if (result.ClickedButtonIndex == 0)
                        {
                            mainThread.Abort();
                            mainThread = null;
                            ServicesManager.OnEnableWindowListner();
                            Trace.WriteLine("Add roms finished at " + DateTime.Now.ToLocalTime(), "Add roms");
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

        private void checkBox_use_ai_path_CheckedChanged(object sender, EventArgs e)
        {
            textBox_archive_extensions.Enabled = linkLabel1.Enabled = checkBox_useParent.Enabled =
                checkBox_alwaysChooseChild.Enabled = checkBox_use_ai_path.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string ex = "";
            foreach (string ext in console.ArchiveExtensions)
                ex += ext + ";";
            textBox_archive_extensions.Text = ex;
        }
    }
}
