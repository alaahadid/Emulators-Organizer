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
using System.IO;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class ConsoleManualViewer : UserControl
    {
        public ConsoleManualViewer()
        {
            InitializeComponent();
        }
        private EmulatorsOrganizer.Core.Console console;
        protected ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        protected LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        protected SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private string currentFilePath;
        private string[] DefaultExtensions = { ".pdf" };

        private string GetExtensionDialogFilter()
        {
            string filter = ls["FilterName_Manual"] + " |";
            foreach (string ex in DefaultExtensions)
            {
                filter += "*" + ex + ";";
            }
            return filter.Substring(0, filter.Length - 1);
        }
        public void ClearInformation()
        {
            currentFilePath = "";
        }
        public void LoadInformation(string consoleID)
        {
            console = profileManager.Profile.Consoles[consoleID];
            RefreshInformation();
        }
        public void RefreshInformation()
        {
            webBrowser1.Url = null;
            currentFilePath = "";
            if (console == null) return;
            if (console != null)
            {
                string filePath = HelperTools.GetFullPath(console.PDFPath);
                if (File.Exists(filePath))
                {
                    try
                    {
                        webBrowser1.Url = new Uri(filePath);
                    }
                    catch (Exception ex)
                    {
                        //Trace.TraceError("Unable to open pdf file: " + ex.Message);
                    }
                }
            }
        }

        private void toolStripButton_openAssigne_Click(object sender, EventArgs e)
        {
            if (console == null)
                return;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = string.Format("{0} '{1}' {2}", ls["Title_OpenFile"], console.Name, ls["Title_ConsoleManual"]);
            op.Filter = GetExtensionDialogFilter();
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                // Assign it
                currentFilePath = op.FileName;
                console.PDFPath = HelperTools.GetDotPath(op.FileName);
                // Raise event 
                profileManager.Profile.OnConsolePropertiesChanged(console.Name);
                // Refresh it (open it)
                RefreshInformation();
            }
        }
        private void toolStripButton_remove_Click(object sender, EventArgs e)
        {
            if (console == null)
                return;
            ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
           ls["Message_AreYouSureYouWantToRemoveThePdfAssignmentForThisConsole"], "", true, false,
           ls["CheckBox_DeleteFileFromDiskToo"]);
            if (res.ClickedButtonIndex == 0)
            {
                console.PDFPath = "";
                RefreshInformation();
                profileManager.Profile.OnConsolePropertiesChanged(console.Name);
                if (res.Checked)
                {
                    string pdfPath = HelperTools.GetFullPath(console.PDFPath);
                    if (File.Exists(pdfPath))
                    {
                        try
                        {
                            File.Delete(pdfPath);
                        }
                        catch (Exception ex)
                        {
                            ManagedMessageBox.ShowErrorMessage(ls["Message_UnableToDeleteFile"] + ": " + ex.Message);
                        }
                    }
                }
            }
        }
        private void toolStripButton_viewUsingWindowsDefaultViewer_Click(object sender, EventArgs e)
        {
            if (console == null)
                return;
            string pdfPath = HelperTools.GetFullPath(console.PDFPath);
            if (File.Exists(pdfPath))
            {
                System.Diagnostics.Process.Start(pdfPath);
            }
        }
        private void toolStripButton_openFileLocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (console == null)
                    return;
                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + HelperTools.GetFullPath(console.PDFPath));
            }
            catch { }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshInformation();
        }
    }
}
