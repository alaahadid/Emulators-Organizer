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
using System.Windows.Forms;
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_RecentProfiles : Form
    {
        public Form_RecentProfiles()
        {
            InitializeComponent();
            // Load recents
            string[] recentProfiles = (string[])settings.GetValue("Recent profiles", true, new string[0]);

            foreach (string file in recentProfiles)
            {
                ListViewItem item = new ListViewItem();
                item.Text = Path.GetFileNameWithoutExtension(file);
                item.SubItems.Add(file);
                item.Tag = file;
                listView1.Items.Add(item);
            }
            if (listView1.Items.Count > 0)
                listView1.Items[0].Selected = true;
            // Load options
            checkBox_showAtAppStart.Checked = (bool)settings.GetValue("Show startup window", true, true);
            checkBox_loadRecentProfile.Checked = (bool)settings.GetValue("Load recent profile", true, false);

        }
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");

        private void AddRecent(string filePath)
        {
            // Load old collection
            string[] recentProfiles = (string[])settings.GetValue("Recent profiles", true, new string[0]);
            List<string> rec = new List<string>(recentProfiles);
            // If the path already exist, remove it first
            if (rec.Contains(filePath))
                rec.Remove(filePath);
            // Insert new one at the beginning 
            rec.Insert(0, filePath);
            // Limit to 10
            if (rec.Count == 11)
                rec.RemoveAt(10);
            // Save back
            settings.AddValue("Recent profiles", rec.ToArray());
        }
        private void RemoveRecent(string filePath)
        {
            // Load old collection
            string[] recentProfiles = (string[])settings.GetValue("Recent profiles", true, new string[0]);
            List<string> rec = new List<string>(recentProfiles);
            // If the path already exist, remove it first
            if (rec.Contains(filePath))
                rec.Remove(filePath);

            // Save back
            settings.AddValue("Recent profiles", rec.ToArray());
        }
        // Open on disk
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = ls["MessageCaption_OpenProfile"];
            openDialog.Filter = ls["Filter_EOP"];
            if (openDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ProfileSaveLoadStatus status = profileManager.LoadProfile(openDialog.FileName);
                if (status.Type == ProfileSaveLaodType.Success)
                {
                    AddRecent(openDialog.FileName);
                    Close();
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantOpenProfile"] + ":\n" + status.Message,
                        ls["MessageCaption_OpenProfile"]);
                }
            }
        }
        // Open selected from recent
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectaProfileFromTheListFirst"]);
                return;
            }
            string path = listView1.SelectedItems[0].Tag.ToString();
            ProfileSaveLoadStatus status = profileManager.LoadProfile(path);
            if (status.Type == ProfileSaveLaodType.Success)
            {
                AddRecent(path);
                Close();
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantOpenProfile"] + ":\n" + status.Message,
                    ls["MessageCaption_OpenProfile"]);
            }
        }
        // Getting started
        private void button4_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, HelperTools.StartUpPath + "\\Help.chm", HelpNavigator.KeywordIndex,
            //    "Work with Emulators Organizer");
            try
            {
                System.Diagnostics.Process.Start(HelperTools.StartUpPath + "\\Help\\index.htm");
            }
            catch (Exception ex)
            { ManagedMessageBox.ShowErrorMessage(ex.Message); }
        }
        // Close
        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void checkBox_showAtAppStart_CheckedChanged(object sender, EventArgs e)
        {
            settings.AddValue("Show startup window", checkBox_showAtAppStart.Checked);
        }
        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2_Click(this, null);
        }
        private void checkBox_loadRecentProfile_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_showAtAppStart.Enabled = !checkBox_loadRecentProfile.Checked;
            settings.AddValue("Load recent profile", checkBox_loadRecentProfile.Checked);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseSelectaProfileFromTheListFirst"]);
                return;
            }
            ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(ls["Message_ThisWillRemoveProfileFromRecentList"]);
            if (res.ClickedButtonIndex == 0)
            {
                RemoveRecent(listView1.SelectedItems[0].Tag.ToString());
                listView1.Items.RemoveAt(listView1.SelectedItems[0].Index);
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(ls["Message_ThisWillRemoveAllProfilesInTheRecentList"]);
            if (res.ClickedButtonIndex == 0)
            {
                listView1.Items.Clear();
                settings.AddValue("Recent profiles", new string[0]);
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = (listView1.SelectedItems.Count == 1);
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2_Click(this, new EventArgs());
        }
        // start and add console
        private void button3_Click(object sender, EventArgs e)
        {
            // 1 Add the console, save the new id.
            string newConsoleID = profileManager.Profile.AddConsole("");
            // 2 Show console properties !
            ConsoleProperties conProperties = new ConsoleProperties(newConsoleID, ls["Title_General"]);
            if (conProperties.ShowDialog(this) == DialogResult.OK)
            {
                // 3 Add roms
                Form_AddRomsFolderScan frmAddRoms = new Form_AddRomsFolderScan(newConsoleID);
                if (frmAddRoms.ShowDialog(this) == DialogResult.OK)
                {
                    // 4 Add emulator
                    Form_AddEmulator frmAddEmulator = new Form_AddEmulator(false, newConsoleID);
                    frmAddEmulator.ShowDialog(this);

                    // 5 Detect tabs !
                    Form_DetectICSWizard frmDetect = new Form_DetectICSWizard(newConsoleID);
                    frmDetect.ShowDialog(this);

                    // 6 Show done message
                    ManagedMessageBox.ShowMessage(ls["Status_Done"]);
                    Close();
                }
            }
            else
            {
                // Remove the added console :(
                profileManager.Profile.Consoles.Remove(newConsoleID);
            }
        }
        // Start and add mame console.
        private void button6_Click(object sender, EventArgs e)
        {
            Form_AddMameConsole frm = new Form_AddMameConsole();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // 2 Show console properties !
                ConsoleProperties conProperties = new ConsoleProperties(frm.NewConsoleID, ls["Title_General"]);
                if (conProperties.ShowDialog(this) == DialogResult.OK)
                {
                    // 3 Add emulator
                    Form_AddEmulator frmAddEmulator = new Form_AddEmulator(false, frm.NewConsoleID);
                    frmAddEmulator.ShowDialog(this);

                    // 4 Detect tabs !
                    Form_DetectICSWizard frmDetect = new Form_DetectICSWizard(frm.NewConsoleID);
                    frmDetect.ShowDialog(this);

                    // 5 Show done message
                    ManagedMessageBox.ShowMessage(ls["Status_Done"]);
                    Close();
                }
            }
        }
    }
}
