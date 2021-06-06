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
using System;
using System.ComponentModel;
using System.IO;

namespace EmulatorsOrganizer.GUI
{
    public partial class ICPDF : ICControl
    {
        public ICPDF(string icid, string parentConsoleID)
        {
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;
            base.canAcceptDraggedFiles = true;
            base.canSelectionReverse = true;
            console = profileManager.Profile.Consoles[base.parentConsoleID];
            InitializeComponent();
            base.ApplyStyleOnRomSelection();

            // Load settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    toolStrip1.Visible = ((InformationContainerPDF)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip;
            }
        }
        private EmulatorsOrganizer.Core.Console console;
        private string currentFile;

        public override void InitializeEvents()
        {
            base.InitializeEvents();
            // Get some important events
            if (console != null)
            {
                console.InformationContainersMap.TabReordered += InformationContainersMap_TabReordered;
                console.InformationContainersMap.TabDragged += InformationContainersMap_TabReordered;
                console.InformationContainersMap.TabReordered += InformationContainersMap_TabReordered;
            }
        }
        public override void DisposeEvents()
        {
            base.DisposeEvents();
            if (console != null)
            {
                try
                {
                    console.InformationContainersMap.TabReordered -= InformationContainersMap_TabReordered;
                    console.InformationContainersMap.TabDragged -= InformationContainersMap_TabReordered;
                    console.InformationContainersMap.TabReordered -= InformationContainersMap_TabReordered;
                }
                catch { }
            }
        }
        private void ClearBrowser()
        {
            if (!webBrowser1.IsDisposed)
                webBrowser1.Url = null;
        }
        protected override void ShowFile()
        {
            ClearBrowser();
            if (console == null) return;
            if (base.IsValidFileIndex() && console.InformationContainersMap.IsContainerVisibleAndSelected(ICID))
            {
                string filePath = HelperTools.GetFullPath(files[fileIndex]);
                if (currentFile == filePath) return;
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
                else
                {
                    if (currentFile != "")
                    {
                        currentFile = "";
                    }
                }
            }
            else if (currentFile != "")
            {
                currentFile = "";
            }
        }
        public override void OnPriorityActive()
        {
            ShowFile();// make sure it's shown !
        }
        protected override void UpdateStatus()
        {
            StatusLabel1.Text = base.GetStatusString();
            toolStripButton6.Enabled = toolStripButton7.Enabled = files.Count > 1;
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            toolStrip1.BackColor = style.bkgColor_InformationContainerTabs;
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            toolStripButton1.Enabled = toolStripButton2.Enabled = toolStripButton3.Enabled = false;
            addMoreFilesToolStripMenuItem.Enabled = removeSelectedToolStripMenuItem.Enabled = editListToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            toolStripButton1.Enabled = toolStripButton2.Enabled = toolStripButton3.Enabled = true;
            addMoreFilesToolStripMenuItem.Enabled = removeSelectedToolStripMenuItem.Enabled = editListToolStripMenuItem.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            base.AddFilesToList();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            base.RemoveSelectedFileFromList();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            base.EditList();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            base.OpenSelectedFile();
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            base.OpenSelectedFileLocation();
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            base.PreviousFile();
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            base.NextFile();
        }
        private void InformationContainersMap_TabReordered(object sender, EventArgs e)
        {
            ShowFile();
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            ShowFile();
        }
        private void showToolstripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
            // Save settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    ((InformationContainerPDF)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip = toolStrip1.Visible;
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            nextToolStripMenuItem.Enabled = previousToolStripMenuItem.Enabled = files.Count > 1;
            showToolstripToolStripMenuItem.Checked = toolStrip1.Visible;
        }
    }
}
