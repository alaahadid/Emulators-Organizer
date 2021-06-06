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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class ConsoleInformationViewer : UserControl
    {
        public ConsoleInformationViewer()
        {
            InitializeComponent();
        }
        protected ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        protected LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        protected SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private string consoleID;
        private string currentFilePath = "";
        private string[] DefaultExtensions = { ".txt", ".rtf", ".doc" };

        private string GetExtensionDialogFilter()
        {
            string filter = ls["FilterName_InformationFile"] + " |";
            foreach (string ex in DefaultExtensions)
            {
                filter += "*" + ex + ";";
            }
            return filter.Substring(0, filter.Length - 1);
        }
        public void ClearInformation()
        {
            consoleID = "";
            richTextBox1.Text = "";
            currentFilePath = "";
        }
        public void RefreshInformation()
        {
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console == null)
                return;
            // Load the file if exist

            if (console.RTFPath != null)
            {
                string rtfPath = HelperTools.GetFullPath(console.RTFPath);
                if (File.Exists(rtfPath))
                {
                    currentFilePath = rtfPath;
                    switch (Path.GetExtension(rtfPath).ToLower())
                    {
                        case ".doc":
                        case ".rtf":
                            {
                                richTextBox1.LoadFile(rtfPath, RichTextBoxStreamType.RichText); break;
                            }
                        default:
                            {
                                richTextBox1.Lines = File.ReadAllLines(rtfPath); break;
                            }
                    }
                }
            }
        }
        public void LoadInformation(string consoleID)
        {
            this.consoleID = consoleID;
            RefreshInformation();
        }
        // Create new file
        private void toolStripButton_new_Click(object sender, EventArgs e)
        {
            if (consoleID == "")
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console == null)
                return;

            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = string.Format("{0} '{1}' {2}", ls["Title_NewFile"], console.Name, ls["Title_ConsoleInformation"]);
            sav.Filter = GetExtensionDialogFilter();
            sav.FileName = console.Name + ".txt";
            if (sav.ShowDialog(this) == DialogResult.OK)
            {
                // Assignee it
                currentFilePath = sav.FileName;
                console.RTFPath = HelperTools.GetDotPath(sav.FileName);
                // Save it
                File.WriteAllLines(currentFilePath, richTextBox1.Lines);
                // Refresh it !
                RefreshInformation();
                // Raise event 
                profileManager.Profile.OnConsolePropertiesChanged(console.Name);
            }
        }
        // save current file if exist, open save dialog if not exist
        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            if (consoleID == "")
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console == null)
                return;

            string rtfPath = HelperTools.GetFullPath(console.RTFPath);
            if (File.Exists(rtfPath))
            {
                // Save it
                File.WriteAllLines(rtfPath, richTextBox1.Lines);
            }
            else
            {
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = string.Format("{0} '{1}' {2}", ls["Title_NewFile"], console.Name, ls["Title_ConsoleInformation"]);
                sav.Filter = GetExtensionDialogFilter();
                sav.FileName = console.Name + ".txt";
                if (sav.ShowDialog(this) == DialogResult.OK)
                {
                    // Assignee it
                    currentFilePath = sav.FileName;
                    console.RTFPath = HelperTools.GetFullPath(sav.FileName);
                    // Save it
                    File.WriteAllLines(currentFilePath, richTextBox1.Lines);
                    // Refresh it !
                    RefreshInformation();
                    // Raise event 
                    profileManager.Profile.OnConsolePropertiesChanged(console.Name);
                }
            }
        }
        // Open then add
        private void toolStripButton_open_Click(object sender, EventArgs e)
        {
            if (consoleID == "")
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console == null)
                return;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = string.Format("{0} '{1}' {2}", ls["Title_OpenFile"], console.Name, ls["Title_ConsoleInformation"]);
            op.Filter = GetExtensionDialogFilter();
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                // Assigne it
                currentFilePath = op.FileName;
                console.RTFPath = HelperTools.GetFullPath(op.FileName);
                // Refresh it (open it)
                RefreshInformation();
                // Raise event 
                profileManager.Profile.OnConsolePropertiesChanged(console.Name);
            }
        }
        // Delete
        private void toolStripButton_remove_Click(object sender, EventArgs e)
        {
            if (consoleID == "")
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console == null)
                return;

            ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                ls["Message_AreYouSureYouWantToRemoveTheRtfAssignmentForThisConsole"], "", true, false,
                ls["CheckBox_DeleteFileFromDiskToo"]);
            if (res.ClickedButtonIndex == 0)
            {
                console.RTFPath = "";
                RefreshInformation();
                profileManager.Profile.OnConsolePropertiesChanged(console.Name);
                if (res.Checked)
                {
                    string rtfPath = HelperTools.GetFullPath(console.RTFPath);
                    if (File.Exists(rtfPath))
                    {
                        try
                        {
                            File.Delete(rtfPath);
                        }
                        catch (Exception ex)
                        {
                            ManagedMessageBox.ShowErrorMessage(ls["Message_UnableToDeleteFile"] + ": " + ex.Message);
                        }
                    }
                }
            }

        }
        // Edit
        private void toolStripButton_edit_Click(object sender, EventArgs e)
        {
            if (consoleID == "")
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console == null)
                return;
            string rtfPath = HelperTools.GetFullPath(console.RTFPath);
            if (File.Exists(rtfPath))
            {
                System.Diagnostics.Process.Start(rtfPath);
            }
        }
        // Open file location
        private void toolStripButton_openFileLocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (consoleID == "")
                    return;
                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
                if (console == null)
                    return;
                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + HelperTools.GetFullPath(console.RTFPath));
            }
            catch { }
        }
    }
}
