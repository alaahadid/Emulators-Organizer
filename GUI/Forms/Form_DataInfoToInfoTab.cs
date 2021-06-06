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
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_DataInfoToInfoTab : Form
    {
        public Form_DataInfoToInfoTab(string consoleID)
        {
            InitializeComponent();
            // Refresh data info items
            console = profileManager.Profile.Consoles[consoleID];

            foreach (RomData item in console.RomDataInfoElements)
            {
                checkedListBox1.Items.Add(item);
            }
            if (checkedListBox1.Items.Count > 0)
                checkedListBox1.SetItemChecked(0, true);
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_TheresNoDataInfoForThisConsole"]);
                Close();
                return;
            }
            foreach (InformationContainer con in console.InformationContainers)
            {
                if (con is InformationContainerInfoText)
                    listBox_infoTab.Items.Add(con);
            }
            if (listBox_infoTab.Items.Count > 0)
                listBox_infoTab.SelectedIndex = 0;
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_TheresNoInfoTabForThisConsole"]);
                Close();
                return;
            }
        }

        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private EmulatorsOrganizer.Core.Console console;

        private List<string> selectedDataInfoIDs;
        private string selectedTabID;
        private string selectedFolder;
        private bool clearFolder;
        private bool removeDataInfoElements;
        private Thread mainThread;
        private bool done = false;
        private string status;
        private int progress = 0;

        private void Progress()
        {
            int i = 0;
            if (clearFolder)
            {
                status = ls["Status_ClearingInfoFolder"];

                string[] files = Directory.GetFiles(HelperTools.GetFullPath(selectedFolder));
                foreach (string file in files)
                {
                    try
                    { File.Delete(file); }
                    catch
                    {

                    }
                    progress = (i * 100) / files.Length;
                    status = ls["Status_ClearingInfoFolder"] + " (" + progress + " %)";
                    i++;
                }
            }
            status = "";
            Rom[] roms = profileManager.Profile.Roms[console.ID, false];
            i = 0;
            foreach (Rom rom in roms)
            {
                // 1 Get the info
                string value = "";
                // Make the text value to save !
                if (selectedDataInfoIDs.Count == 1)
                {
                    object v = rom.GetDataItemValue(selectedDataInfoIDs[0]);
                    if (v != null)
                    {
                        value = v.ToString();
                    }
                }
                else
                {
                    // Add one by one
                    for (int j = 0; j < selectedDataInfoIDs.Count; j++)
                    {
                        object v = rom.GetDataItemValue(selectedDataInfoIDs[j]);
                        if (v != null)
                        {
                            value += console.GetDataInfo(selectedDataInfoIDs[j]).Name + ":\n";
                            value += v.ToString() + "\n\n";
                        }
                    }
                }
                if (removeDataInfoElements)
                {
                    for (int j = 0; j < selectedDataInfoIDs.Count; j++)
                    {
                        rom.DeleteDataInfoItemItems(selectedDataInfoIDs[j]);
                    }
                }
                if (value != null && value != "")
                {
                    // 2 Save the file !
                    string romFileName = Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)));
                    string pathOfInfFile = selectedFolder + "\\" + romFileName + ".txt";
                    int j = 0;
                    while (File.Exists(pathOfInfFile))
                    {
                        j++;
                        pathOfInfFile = selectedFolder + "\\" + romFileName + "_(" + j + ").txt";
                    }
                    File.WriteAllText(pathOfInfFile, value, Encoding.UTF8);
                    // 3 Add it to the Info tab !
                    InformationContainerItemFiles item;
                    if (rom.IsInformationContainerItemExist(selectedTabID))
                    {
                        item = ((InformationContainerItemFiles)rom.GetInformationContainerItem(selectedTabID));
                    }
                    else
                    {
                        item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), selectedTabID);
                        rom.RomInfoItems.Add(item);
                    }
                    if (item.Files == null)
                        item.Files = new List<string>();
                    if (!item.Files.Contains(pathOfInfFile))
                        item.Files.Add(pathOfInfFile);
                }
                progress = (i * 100) / roms.Length;
                status = ls["Status_CreatingInfoFiles"] + " (" + progress + " %)";
                i++;
            }
            if (removeDataInfoElements)
            {
                for (int j = 0; j < selectedDataInfoIDs.Count; j++)
                {
                    console.DeleteRomDataInfo(selectedDataInfoIDs[j]);
                }
                console.FixColumnsForRomDataInfo();
            }
            if (!this.InvokeRequired)
                Done();
            else
                this.Invoke(new Action(Done));
        }
        private void Done()
        {
            done = true;
            profileManager.Profile.OnInformationContainerItemsDetected();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        // Change folder
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = "Browse for folder where to save info files";
            fol.SelectedPath = textBox_filesFolder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                textBox_filesFolder.Text = fol.SelectedPath;
        }
        // Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Start
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectTheDataInfoElementToSave"]);
                return;
            }
            if (listBox_infoTab.SelectedIndex < 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectTheInfoTabYouWantToSaveInto"]);
                return;
            }
            if (!Directory.Exists(textBox_filesFolder.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForTheFolderWhereToSaveFilesFirst"]);
                return;
            }
            selectedDataInfoIDs = new List<string>();
            //selectedDataInfoID = ((RomData)listBox_dataInfo.SelectedItem).ID;
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                selectedDataInfoIDs.Add(((RomData)checkedListBox1.CheckedItems[i]).ID);
            }
            selectedTabID = ((InformationContainer)listBox_infoTab.SelectedItem).ID;
            selectedFolder = textBox_filesFolder.Text;
            clearFolder = checkBox_clearFolder.Checked;
            removeDataInfoElements = checkBox_deleteDataInfo.Checked;
            // Disable things
            checkedListBox1.Enabled = listBox_infoTab.Enabled = button1.Enabled = button2.Enabled
                = textBox_filesFolder.Enabled = false;
            //show things
            label_status.Visible = progressBar1.Visible = true;
            timer1.Start();

            mainThread = new Thread(new ThreadStart(Progress));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status.Text = status;
            progressBar1.Value = progress;
        }
        private void Form_DataInfoToInfoTab_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!done)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"]);
                        if (result.ClickedButtonIndex == 0)
                        {
                            mainThread.Abort();
                            mainThread = null;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }
        // Move up
        private void button4_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex < 0) return;
            int index = checkedListBox1.SelectedIndex;
            if (index - 1 >= 0)
            {
                // Get the item at selected
                object selected = checkedListBox1.Items[checkedListBox1.SelectedIndex];
                bool chec = checkedListBox1.GetItemChecked(checkedListBox1.SelectedIndex);
                // Remove it !
                checkedListBox1.Items.RemoveAt(index);
                // Dec
                index--;
                // Insert it !
                checkedListBox1.Items.Insert(index, selected);
                checkedListBox1.SetItemChecked(index, chec);
                // Select it !
                checkedListBox1.SelectedIndex = index;
            }
        }
        // Move down
        private void button5_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex < 0) return;
            int index = checkedListBox1.SelectedIndex;
            if (index + 1 < checkedListBox1.Items.Count)
            {
                // Get the item at selected
                object selected = checkedListBox1.Items[checkedListBox1.SelectedIndex];
                bool chec = checkedListBox1.GetItemChecked(checkedListBox1.SelectedIndex);
                // Remove it !
                checkedListBox1.Items.RemoveAt(index);
                // Dec
                index--;
                // Insert it !
                checkedListBox1.Items.Insert(index, selected);
                checkedListBox1.SetItemChecked(index, chec);
                // Select it !
                checkedListBox1.SelectedIndex = index;
            }
        }
        // Reset
        private void button6_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            foreach (RomData item in console.RomDataInfoElements)
            {
                checkedListBox1.Items.Add(item);
            }
        }
    }
}
