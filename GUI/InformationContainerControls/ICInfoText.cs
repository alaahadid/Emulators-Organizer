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
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class ICInfoText : ICControl
    {
        public ICInfoText(string icid, string parentConsoleID)
        {
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;
            base.canAcceptDraggedFiles = true;
            base.canSelectionReverse = true;
            InitializeComponent();
            base.ApplyStyleOnRomSelection();
            // Load settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    toolStrip1.Visible = ((InformationContainerInfoText)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip;
            }
            // TODO: don't forget the download games from db
            getOverviewFromTheGamesDBnetToolStripMenuItem.Visible = false;
            toolStripButton11.Visible = false;
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            toolStrip1.BackColor = style.bkgColor_InformationContainerTabs;
            richTextBox1.BackColor = style.bkgColor_InformationContainerTabs;
        }
        protected override void ShowFile()
        {
            if (base.IsValidFileIndex())
            {
                richTextBox1.Enabled = true;
                richTextBox1.Text = "";
                try
                {
                    string filePath = HelperTools.GetFullPath(files[fileIndex]);
                    if (File.Exists(filePath))
                    {
                        string ex = Path.GetExtension(filePath).ToLower();
                        switch (ex)
                        {
                            case ".doc":
                            case ".rtf":
                                {
                                    richTextBox1.LoadFile(filePath, RichTextBoxStreamType.RichText);
                                    break;
                                }
                            default:
                                {
                                    richTextBox1.Lines = File.ReadAllLines(filePath);
                                    break;
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Unable to show file: " + ex.Message);
                }
            }
            else
                ClearDisplay();
        }
        private void ClearDisplay()
        {
            richTextBox1.Text = "";
            richTextBox1.Enabled = false;
            StripLabel_path.Text = "";
        }
        protected override void UpdateStatus()
        {
            StatusLabel.Text = base.GetStatusString();
            if (base.IsValidFileIndex())
                StripLabel_path.Text = files[fileIndex];
            else
                StripLabel_path.Text = "";
            toolStripButton6.Enabled = toolStripButton7.Enabled = files.Count > 1;
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            toolStripButton1.Enabled =
                toolStripButton2.Enabled =
                toolStripButton8.Enabled =
                toolStripButton9.Enabled =
                toolStripButton3.Enabled =
                 toolStripButton10.Enabled =
                 toolStripButton11.Enabled = false;
            newToolStripMenuItem.Enabled =
                addMoreFilesToolStripMenuItem.Enabled =
               deleteToolStripMenuItem.Enabled =
               editListToolStripMenuItem.Enabled =
               getInfoFromMobygamescomToolStripMenuItem.Enabled =
               getOverviewFromTheGamesDBnetToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            toolStripButton1.Enabled =
                toolStripButton2.Enabled =
                toolStripButton8.Enabled =
                toolStripButton9.Enabled =
                toolStripButton3.Enabled =
                 toolStripButton10.Enabled =
                 toolStripButton11.Enabled = true;
            newToolStripMenuItem.Enabled =
                addMoreFilesToolStripMenuItem.Enabled =
               deleteToolStripMenuItem.Enabled =
               editListToolStripMenuItem.Enabled =
               getInfoFromMobygamescomToolStripMenuItem.Enabled =
               getOverviewFromTheGamesDBnetToolStripMenuItem.Enabled = true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            base.AddFilesToList();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            base.RemoveSelectedFileFromList();
        }
        // New file
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 0)
                return;
            Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
            InformationContainerItem item = rom.GetInformationContainerItem(ICID);
            InformationContainer cont = profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = string.Format(ls["Title_NewFile"] + " '{0}'", cont.DisplayName);
            sav.Filter = ((InformationContainerFiles)cont).GetExtensionDialogFilter();
            sav.FileName = Path.GetFileNameWithoutExtension(rom.Path) + ".txt";
            if (sav.ShowDialog(this) == DialogResult.OK)
            {
                base.AddFilesToList(new string[] { sav.FileName }, false);
                base.SelectLastFile();
                // Save
                toolStripButton9_Click(this, null);
            }
        }
        // Save
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (base.IsValidFileIndex())
            {
                try
                {
                    string ex = Path.GetExtension(files[fileIndex]).ToLower();
                    switch (ex)
                    {
                        case ".rtf": richTextBox1.SaveFile(files[fileIndex], RichTextBoxStreamType.RichText); break;
                        case ".doc": richTextBox1.SaveFile(files[fileIndex], RichTextBoxStreamType.RichText); break;
                        default: File.WriteAllLines(files[fileIndex], richTextBox1.Lines); break;
                    }
                }
                catch (Exception ex)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_UnableToSaveTheFile"] + "\n" + ex.Message + "\n\n" + ex.ToString(),
                    ls["MessageCaption_Error"]);
                }
            }
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
        // MobyGames
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (base.IsValidFileIndex())
            {
                if (Path.GetExtension(files[fileIndex]).ToLower() != ".rtf"
                    && Path.GetExtension(files[fileIndex]).ToLower() != ".doc")
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisFileMustBeRtfOrDocToApplyMobyGamesInfo"],
                          ls["MessageCaption_Error"]);
                    return;
                }
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                Form_MobyGames frm = new Form_MobyGames(rom.Name);
                richTextBox1.Clear();
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    richTextBox1.Clear();
                    richTextBox1.Rtf = frm.RTF;
                    //save
                    toolStripButton9_Click(sender, e);
                }
            }
            else
            {
                // Create new then !
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainer cont = profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = string.Format(ls["Title_NewFile"] + " '{0}'", cont.DisplayName);
                sav.Filter = ((InformationContainerFiles)cont).GetExtensionDialogFilter();
                sav.FileName = Path.GetFileNameWithoutExtension(rom.Path) + ".rtf";
                if (sav.ShowDialog(this) == DialogResult.OK)
                {
                    base.AddFilesToList(new string[] { sav.FileName }, false);
                    base.SelectLastFile();
                    toolStripButton10_Click(this, null); // Recall !
                }
            }
        }
        private void showToolstripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
            // Save settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    ((InformationContainerInfoText)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip = toolStrip1.Visible;
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            nextToolStripMenuItem.Enabled = previousToolStripMenuItem.Enabled = files.Count > 1;
            showToolstripToolStripMenuItem.Checked = toolStrip1.Visible;
        }
        // Get from The Game DB
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (base.IsValidFileIndex())
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                Form_SearchTheGamesDB frm = new Form_SearchTheGamesDB(rom.ID);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    richTextBox1.Clear();
                    //Game gm = GamesDB.GetGame(frm.SelectedResultID);
                    //richTextBox1.Text = gm.Overview;
                    //save
                    toolStripButton9_Click(sender, e);
                }
            }
            else
            {
                // Create new then !
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainer cont = profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = string.Format(ls["Title_NewFile"] + " '{0}'", cont.DisplayName);
                sav.Filter = ((InformationContainerFiles)cont).GetExtensionDialogFilter();
                sav.FileName = Path.GetFileNameWithoutExtension(rom.Path) + ".rtf";
                if (sav.ShowDialog(this) == DialogResult.OK)
                {
                    base.AddFilesToList(new string[] { sav.FileName }, false);
                    base.SelectLastFile();
                    toolStripButton11_Click(this, null); // Recall !
                }
            }
        }
    }
}
