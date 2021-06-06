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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public class ICControl : UserControl, IStylable
    {
        protected ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        protected LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        protected SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        public string ICID;
        public string parentConsoleID;

        protected List<string> files = new List<string>();
        protected int fileIndex;
        /// <summary>
        /// If true, the user can switch file from 0 to max
        /// </summary>
        protected bool canSelectionReverse = true;
        protected bool canAcceptDraggedFiles = true;

        /*Methods*/
        protected virtual void ApplyStyleOnRomSelection()
        {
            if (parentConsoleID == null)
            {
                return;
            }
            // make console theme
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[parentConsoleID];
            if (console != null)
            {
                ApplyStyle(console.Style);
            }
        }
        public virtual void ApplyStyle(EOStyle style)
        {
            this.BackColor = style.bkgColor_InformationContainerTabs;
            this.BackgroundImage = style.image_InformationContainerTabs;
        }
        public virtual void InitializeEvents()
        {
            profileManager.NewProfileCreated += profileManager_NewProfileCreated;
            profileManager.ProfileOpened += profileManager_ProfileOpened;
            profileManager.Profile.RomSelectionChanged += Profile_RomSelectionChanged;
            profileManager.Profile.ConsoleSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.ProfileSavingStarted += profileManager_ProfileSavingStarted;
            profileManager.ProfileSavingFinished += profileManager_ProfileSavingFinished;
        }
        public virtual void DisposeEvents()
        {
            profileManager.NewProfileCreated -= profileManager_NewProfileCreated;
            profileManager.ProfileOpened -= profileManager_ProfileOpened;
            profileManager.Profile.RomSelectionChanged -= Profile_RomSelectionChanged;
            profileManager.Profile.ConsoleSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.ProfileSavingStarted -= profileManager_ProfileSavingStarted;
            profileManager.ProfileSavingFinished -= profileManager_ProfileSavingFinished;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DisposeEvents();
        }

        public virtual void OnPriorityActive() { }
        protected virtual void RefreshFiles()
        {
            // Clear
            fileIndex = -1;
            files.Clear();
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                // Load files if found
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                if (rom != null)
                {
                    InformationContainerItem item = rom.GetInformationContainerItem(ICID);

                    if (item != null && item is InformationContainerItemFiles)
                    {
                        files = new List<string>(((InformationContainerItemFiles)item).Files);
                        fileIndex = files.Count > 0 ? 0 : -1;
                    }
                }
            }
            OnFilesRefresh();
            ShowFile();
            UpdateStatus();
        }
        protected virtual void ShowFile()
        {
        }
        protected virtual void UpdateStatus()
        {
        }
        protected virtual void EditList()
        {
            if (profileManager.IsSaving)
                return;
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);

                if (item != null && item is InformationContainerItemFiles)
                {
                    InformationContainerFiles cont = (InformationContainerFiles)profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);

                    List<string> filesList = new List<string>(((InformationContainerItemFiles)item).Files);
                    if (filesList.Count == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_CantEditFilesListNoFileFound"], ls["MessageCaption_InformationContainerControl"]);
                    }
                    else
                    {
                        FormFileListEdit frm = new FormFileListEdit(filesList.ToArray(), cont.GetExtensionDialogFilter());
                        frm.Text = ls["Title_EditFilesListFor"] + " '" + cont.DisplayName + "'";
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {
                            ((InformationContainerItemFiles)item).Files = new List<string>(frm.FilesAfterEdit);
                            RefreshFiles();
                            profileManager.Profile.OnInformationContainerItemsModified(cont.DisplayName);
                            this.OnAfterListEdit();
                        }
                    }
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantEditFilesListNoFileFound"], ls["MessageCaption_InformationContainerControl"]);
                }
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantEditFilesListNoRomSelected"], ls["MessageCaption_InformationContainerControl"]);
            }
        }
        protected virtual void AddFilesToList()
        {
            if (profileManager.IsSaving)
                return;
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainer cont = profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);

                OpenFileDialog op = new OpenFileDialog();
                op.Title = string.Format(ls["Title_AddFilesFor"] + " '{0}'", cont.DisplayName);
                op.Filter = ((InformationContainerFiles)cont).GetExtensionDialogFilter();
                op.Multiselect = true;
                if (op.ShowDialog(this) == DialogResult.OK)
                {
                    // Add the files
                    InformationContainerItemFiles infItem = null;
                    if (item != null && item is InformationContainerItemFiles)
                    {
                        infItem = (InformationContainerItemFiles)item;
                        if (infItem.Files != null)
                            if (infItem.Files.Count > 0)
                                if (MessageBox.Show(ls["Message_ClearOldImagesCollectionFirst"],
                                  string.Format(ls["Title_AddFilesFor"] + " '{0}'", cont.DisplayName),
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    infItem.Files = new List<string>();
                                }
                    }
                    else
                    {
                        // Create new
                        infItem = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ICID);
                        infItem.Files = new List<string>();
                        // Add it to the rom
                        rom.RomInfoItems.Add(infItem);
                        rom.Modified = true;
                    }
                    if (infItem.Files == null)
                        infItem.Files = new List<string>();
                    // Add the files
                    foreach (string file in op.FileNames)
                        infItem.Files.Add(HelperTools.GetDotPath(file));

                    RefreshFiles();
                    profileManager.Profile.OnInformationContainerItemsModified(cont.Name);
                    this.OnFilesAdded();
                }

            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantAddFilesListNoRomSelected"], ls["MessageCaption_InformationContainerControl"]);
            }
        }
        protected virtual void AddFilesToList(string[] filesToAdd, bool askToClearCollection)
        {
            if (profileManager.IsSaving)
                return;
            if (filesToAdd == null) return;
            if (filesToAdd.Length == 0) return;
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainerFiles cont = (InformationContainerFiles)profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID);

                // Add the files
                InformationContainerItemFiles infItem = null;
                if (item != null && item is InformationContainerItemFiles)
                {
                    infItem = (InformationContainerItemFiles)item;
                    if (infItem.Files != null)
                        if (infItem.Files.Count > 0 && askToClearCollection)
                            if (MessageBox.Show(ls["Message_ClearOldImagesCollectionFirst"],
                              string.Format(ls["Title_AddFilesFor"] + " '{0}'", cont.DisplayName),
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                infItem.Files = new List<string>();
                            }
                }
                else
                {
                    // Create new
                    infItem = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ICID);
                    infItem.Files = new List<string>();
                    // Add it to the rom
                    rom.RomInfoItems.Add(infItem);
                    rom.Modified = true;
                }
                if (infItem.Files == null)
                    infItem.Files = new List<string>();

                // Add only files with default extensions
                foreach (string file in filesToAdd)
                {
                    if (cont.DefaultExtensions.Contains(Path.GetExtension(file).ToLower()))
                        infItem.Files.Add(HelperTools.GetDotPath(file));
                }

                RefreshFiles();
                profileManager.Profile.OnInformationContainerItemsModified(cont.Name);
                this.OnFilesAdded();
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantAddFilesListNoRomSelected"], ls["MessageCaption_InformationContainerControl"]);
            }
        }
        protected virtual void RemoveSelectedFileFromList()
        {
            if (profileManager.IsSaving)
                return;
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                string ICName = profileManager.Profile.Consoles[rom.ParentConsoleID].GetInformationContainer(ICID).DisplayName;

                if (item != null && item is InformationContainerItemFiles)
                {
                    List<string> filesList = new List<string>(((InformationContainerItemFiles)item).Files);
                    if (filesList.Count == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteFileNoFileFound"], ls["MessageCaption_InformationContainerControl"]);
                    }
                    else
                    {
                        if (IsValidFileIndex())
                        {
                            // This is it
                            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(
                                ls["Message_AreYouSureYouWantToDeleteThisFileFormTheList"],
                                ls["MessageCaption_InformationContainerControl"],
                                new string[] { ls["Button_Yes"], ls["Button_No"] }, 1, ManagedMessageBoxIcon.Question,
                                true, false, ls["CheckBox_DeleteFileFromDiskToo"]);
                            if (res.ClickedButtonIndex == 0)
                            {
                                if (res.Checked)
                                {
                                    try
                                    {
                                        File.Delete(HelperTools.GetFullPath(((InformationContainerItemFiles)item).Files[fileIndex]));
                                    }
                                    catch (Exception ex)
                                    {
                                        ManagedMessageBox.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString(),
                                            ls["MessageCaption_InformationContainerControl"]);
                                    }
                                }
                                ((InformationContainerItemFiles)item).Files.RemoveAt(fileIndex);
                                RefreshFiles();
                                profileManager.Profile.OnInformationContainerItemsModified(ICName);
                                this.OnFilesRemoved();
                            }
                        }
                        else
                        {
                            ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteFileNotValidIndex"], ls["MessageCaption_InformationContainerControl"]);
                        }
                    }
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteFileNoFileFound"], ls["MessageCaption_InformationContainerControl"]);
                }
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantDeleteFileNoRomSelected"], ls["MessageCaption_InformationContainerControl"]);
            }
        }
        protected virtual void NextFile()
        {
            if (!canSelectionReverse)
            {
                if (CanMoveNext())
                {
                    fileIndex++;
                }
            }
            else
            {
                if (files.Count > 0)
                {
                    fileIndex = (fileIndex + 1) % files.Count;
                }
                else
                {
                    fileIndex = -1;
                }
            }
            UpdateStatus();
            ShowFile();
            OnFileSelectionChanged();
        }
        protected virtual void PreviousFile()
        {
            if (!canSelectionReverse)
            {
                if (CanMovePrevious())
                {
                    fileIndex--;
                }
            }
            else
            {
                if (files.Count > 0)
                {
                    fileIndex--;
                    if (fileIndex < 0)
                        fileIndex = files.Count - 1;
                }
                else
                {
                    fileIndex = -1;
                }
            }
            UpdateStatus();
            ShowFile();
            OnFileSelectionChanged();
        }
        protected virtual void SelectLastFile()
        {
            if (files.Count > 0)
            {
                fileIndex = files.Count - 1;
            }
            UpdateStatus();
            ShowFile();
            OnFileSelectionChanged();
        }
        protected virtual void SelectFirstFile()
        {
            if (files.Count > 0)
            {
                fileIndex = 0;
            }
            UpdateStatus();
            ShowFile();
            OnFileSelectionChanged();
        }
        protected virtual void OpenSelectedFile()
        {
            if (IsValidFileIndex())
                try
                {
                    System.Diagnostics.Process.Start(files[fileIndex]);
                    OnFileOpenedByWindows();
                }
                catch { }
        }
        protected virtual void OpenSelectedFileLocation()
        {
            if (IsValidFileIndex())
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", @"/select, " + HelperTools.GetFullPath(files[fileIndex]));
                }
                catch { }
        }
        protected virtual string GetStatusString()
        { return string.Format("{0} / {1}", (fileIndex + 1), files.Count); }
        protected virtual bool CanMoveNext()
        {
            return fileIndex < files.Count - 1;
        }
        protected virtual bool CanMovePrevious()
        {
            return fileIndex > 0;
        }
        protected virtual bool IsValidFileIndex()
        {
            return fileIndex >= 0 && fileIndex < files.Count;
        }
        /*Events handle*/
        private void profileManager_NewProfileCreated(object sender, EventArgs e)
        {
            OnNewProfileCreated();
        }
        private void profileManager_ProfileOpened(object sender, EventArgs e)
        {
            OnProfileOpened();
        }
        private void Profile_RomSelectionChanged(object sender, EventArgs e)
        {
            this.RefreshFiles();
            this.ApplyStyleOnRomSelection();
        }
        private void Profile_ConsoleSelectionChanged(object sender, EventArgs e)
        {
            this.ApplyStyleOnRomSelection();
        }
        private void profileManager_ProfileSavingFinished(object sender, EventArgs e)
        {
            OnProfileSavingFinished();
        }
        private void profileManager_ProfileSavingStarted(object sender, EventArgs e)
        {
            OnProfileSavingStarted();
        }
        // Drag and drop
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            if (profileManager.IsSaving)
                return;
            if (!canAcceptDraggedFiles)
                return;
            if (drgevent.Data.GetDataPresent(typeof(MTC.MTCTabPage)))
            {
                // Do nothing; the IC panel should handle this.
                return;
            }
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if ((drgevent.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy &&
                    (profileManager.Profile.SelectedRomIDS.Count == 1))
                {
                    // Do it !
                    this.AddFilesToList((string[])drgevent.Data.GetData(DataFormats.FileDrop), true);
                }
                else
                {
                    // Can't do it
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantAcceptDraggedFilesOneRomMustSelected"],
                        ls["MessageCaption_InformationContainerControl"]);
                }
            }
        }
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if (profileManager.IsSaving)
                return;
            base.OnDragOver(drgevent);

            if (drgevent.Data.GetDataPresent(typeof(MTC.MTCTabPage)))
            {
                // Do nothing; the IC panel should handle this.
                return;
            }
            if (!canAcceptDraggedFiles)
                return;
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if ((drgevent.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy &&
                    (profileManager.Profile.SelectedRomIDS.Count == 1))
                {
                    drgevent.Effect = DragDropEffects.Copy;
                }
                else
                {
                    drgevent.Effect = DragDropEffects.None;
                }
            }
        }
        // Handler to override
        protected virtual void OnFilesAdded()
        {
        }
        protected virtual void OnFilesRemoved()
        {
        }
        protected virtual void OnAfterListEdit()
        {
        }
        protected virtual void OnFileSelectionChanged()
        {
        }
        protected virtual void OnFileOpenedByWindows()
        {
        }
        protected virtual void OnFilesRefresh()
        {
        }

        /// <summary>
        /// Called when a new profile created. This method calls InitializeEvents() by default.
        /// </summary>
        protected virtual void OnNewProfileCreated()
        { InitializeEvents(); }
        /// <summary>
        /// Called when a profile opened. This method calls InitializeEvents() by default.
        /// </summary>
        protected virtual void OnProfileOpened()
        { InitializeEvents(); }
        /// <summary>
        /// Called when profile starting to save.
        /// </summary>
        protected virtual void OnProfileSavingStarted()
        {
        }
        /// <summary>
        /// Called when profile finished saving.
        /// </summary>
        protected virtual void OnProfileSavingFinished()
        {
        }
        // Properties
        /// <summary>
        /// Get or set if this control is showing files now.
        /// </summary>
        public virtual bool GotFilesToShow
        { get { return files.Count > 0; } }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ICControl
            // 
            this.Name = "ICControl";
            this.ResumeLayout(false);

        }
    }
}
