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
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MLV;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class EmulatorsBrowser : IBrowserControl
    {
        public EmulatorsBrowser()
        {
            InitializeComponent();
            base.InitializeEvents();
            // InitializeEvents();
        }

        private List<string> emuIDsTemp;
        private bool emulatorsAtoZ = false;

        public override void InitializeEvents()
        {
            profileManager.Profile.EmulatorsRefreshRequest += Profile_EmulatorsRefreshRequest;
            profileManager.Profile.EmulatorAdded += Profile_EmulatorAdded;
            profileManager.Profile.EmulatorRemoved += Profile_EmulatorRemoved;
            profileManager.Profile.EmulatorsCleared += Profile_EmulatorsCleared;
            profileManager.Profile.EmulatorsSorted += Profile_EmulatorsSorted;
            profileManager.Profile.EmulatorRenamed += Profile_EmulatorRenamed;
            profileManager.Profile.EmulatorMoved += Profile_EmulatorMoved;
            profileManager.Profile.ConsoleRemoved += Profile_ConsoleRemoved;
            profileManager.Profile.ProfileCleanUpFinished += Profile_ProfileCleanUpFinished;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.EmulatorsRefreshRequest -= Profile_EmulatorsRefreshRequest;
            profileManager.Profile.EmulatorAdded -= Profile_EmulatorAdded;
            profileManager.Profile.EmulatorRemoved -= Profile_EmulatorRemoved;
            profileManager.Profile.EmulatorsCleared -= Profile_EmulatorsCleared;
            profileManager.Profile.EmulatorsSorted -= Profile_EmulatorsSorted;
            profileManager.Profile.EmulatorRenamed -= Profile_EmulatorRenamed;
            profileManager.Profile.EmulatorMoved -= Profile_EmulatorMoved;
            profileManager.Profile.ConsoleRemoved -= Profile_ConsoleRemoved;
            profileManager.Profile.ProfileCleanUpFinished -= Profile_ProfileCleanUpFinished;
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                toolStrip1.BackColor = managedListView1.BackColor = base.BackColor = value;
            }
        }
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                managedListView1.BackgroundImage = base.BackgroundImage = value;
            }
        }
        public override bool CanChangeIcon
        {
            get
            {
                return managedListView1.SelectedItems.Count == 1;
            }
        }
        public override bool CanDelete
        {
            get
            {
                return managedListView1.SelectedItems.Count > 0;
            }
        }
        public override bool CanRename
        {
            get
            {
                return managedListView1.SelectedItems.Count == 1;
            }
        }
        public override bool CanShowProperties
        {
            get
            {
                return managedListView1.SelectedItems.Count == 1;
            }
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_EmulatorsBrowser;
            this.BackgroundImage = style.image_EmulatorsBrowser;
            switch (style.imageMode_EmulatorsBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio;
                        break;
                    }
                case BackgroundImageMode.StretchIfLarger:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioIfLarger;
                        break;
                    }
                case BackgroundImageMode.StretchToFit:
                    {
                        managedListView1.BackgroundRenderMode = ManagedListViewBackgroundRenderMode.StretchWithAspectRatioToFit;
                        break;
                    }
            }
            managedListView1.ForeColor = style.txtColor_EmulatorsBrowser;
            foreach (ManagedListViewItem item in managedListView1.Items)
                item.Color = style.txtColor_EmulatorsBrowser;

            // Font
            try
            {
                FontConverter conv = new FontConverter();
                managedListView1.Font = (Font)conv.ConvertFromString(style.font_EmulatorsBrowser);
            }
            catch { }
        }
        public void AddEmulator(string filePath)
        {
            if (profileManager.Profile.Consoles.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_TheresNoConsoleToAddEmulator"], ls["MessageCaption_AddEmulator"]);
                return;
            }
            Form_AddEmulator frm;
            if (filePath != "NULL")
                frm = new Form_AddEmulator(filePath);
            else
                frm = new Form_AddEmulator(true);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Refresh
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.Console:
                        {
                            Emulator[] emulators = profileManager.Profile.Emulators[profileManager.Profile.SelectedConsoleID, false];
                            List<string> emuIDs = new List<string>();
                            foreach (Emulator em in emulators) emuIDs.Add(em.ID);
                            RefreshEmulators(emuIDs.ToArray(), true);
                            break;
                        }
                    case SelectionType.Playlist:
                    case SelectionType.PlaylistsGroup:
                    case SelectionType.ConsolesGroup:
                        {
                            emuIDsTemp.Add(profileManager.Profile.Emulators[profileManager.Profile.Emulators.Count - 1].ID);
                            RefreshEmulators(emuIDsTemp.ToArray(), true);
                            break;
                        }
                }
            }
        }
        public override void DeleteSelected()
        {
            base.DeleteSelected();
            if (managedListView1.SelectedItems.Count == 0)
                return;
            ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeleteEmulators"],
               ls["MessageCaption_DeleteEmulator"]);
            if (result.ClickedButtonIndex == 0)
            {
                foreach (ManagedListViewItem item in managedListView1.SelectedItems)
                {
                    profileManager.Profile.Emulators.Remove((string)item.Tag);
                    emuIDsTemp.Remove((string)item.Tag);
                    managedListView1.Items.Remove(item);
                }
                RefreshEmulators(emuIDsTemp.ToArray(), true);
            }
        }
        public override void RenameSelected()
        {
            base.RenameSelected();
            if (managedListView1.SelectedItems.Count != 1)
                return;

            Form_EnterName frm = new Form_EnterName(ls["Message_EnterEmulatorName"],
                profileManager.Profile.Emulators[(string)managedListView1.SelectedItems[0].Tag].Name, true, false);
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = Cursor.Position;
            frm.OkPressed += frm_OkPressed;
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                string oldName = profileManager.Profile.Emulators[(string)managedListView1.SelectedItems[0].Tag].Name;
                if (oldName != frm.EnteredName)
                {
                    profileManager.Profile.Emulators[(string)managedListView1.SelectedItems[0].Tag].Name = frm.EnteredName;
                    profileManager.Profile.OnEmulatorRenamed(oldName, frm.EnteredName);
                }
            }
        }
        public override void ChangeIcon()
        {
            if (managedListView1.SelectedItems.Count != 1)
                return;
            OpenFileDialog Op = new OpenFileDialog();
            Op.Title = ls["Title_BrowseForAnIcon"];
            Op.Filter = ls["Filter_Icon"]; ;
            Op.Multiselect = false;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                IEOElement element = profileManager.Profile.Emulators[(string)managedListView1.SelectedItems[0].Tag];
                if (Path.GetExtension(Op.FileName).ToLower() == ".exe" | Path.GetExtension(Op.FileName).ToLower() == ".ico")
                { element.Icon = Icon.ExtractAssociatedIcon(Op.FileName).ToBitmap(); }
                else
                {
                    //Stream str = new FileStream(Op.FileName, FileMode.Open, FileAccess.Read);
                    //byte[] buff = new byte[str.Length];
                    //str.Read(buff, 0, (int)str.Length);
                    //str.Dispose();
                    //str.Close();

                    //element.Icon = (Bitmap)Image.FromStream(new MemoryStream(buff));
                    using (var bmpTemp = new Bitmap(Op.FileName))
                    {
                        element.Icon = new Bitmap(bmpTemp);
                    }
                }
                profileManager.Profile.OnElementIconChanged(element);
                managedListView1.Invalidate();
            }
        }
        public override void ClearIcon()
        {
            if (managedListView1.SelectedItems.Count != 1)
                return;
            Emulator element = profileManager.Profile.Emulators[(string)managedListView1.SelectedItems[0].Tag];
            ManagedMessageBoxResult resul = ManagedMessageBox.ShowQuestionMessage(
                ls["Message_AreYouSureYouWantToClearIconsForSelectedItems"],
                ls["MessageCaption_ClearIcon"] + " " + ls["Word_for"] + " " + element.Name + " " + ls["Emulator"]);
            if (resul.ClickedButtonIndex == 0)
            {
                if (File.Exists(element.ExcutablePath))
                {
                    try
                    {
                        element.Icon = Icon.ExtractAssociatedIcon(element.ExcutablePath).ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        element.Icon = null;
                        ManagedMessageBox.ShowErrorMessage(ex.Message);
                    }
                }
                else
                    element.Icon = null;

                profileManager.Profile.OnElementIconChanged(element);
                managedListView1.Invalidate();
            }
        }
        public override void ShowItemProperties()
        {
            if (managedListView1.SelectedItems.Count != 1)
                return;
            EmulatorProperties frm = new EmulatorProperties((string)managedListView1.SelectedItems[0].Tag,
                ls["Title_General"]);
            frm.ShowDialog(this);
        }
        public override void LoadControlSettings()
        {
            // Load basic settings

            managedListView1.StretchThumbnailsToFit = (bool)settings.GetValue("EmulatorsBrowser:StretchImagesToFitThumbnail", true, false);

            trackBar_zoom.Value = (int)settings.GetValue("EmulatorsBrowser:ZoomValue", true, 36);

            trackBar_zoom_Scroll(this, null);
        }

        private void frm_OkPressed(object sender, EnterNameFormOkPressedArgs e)
        {
            if (profileManager.Profile.Emulators.Contains(e.NameEntered, (string)managedListView1.SelectedItems[0].Tag))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherEmulator"],
                    ls["MessageCaption_AddEmulator"]);
                e.Cancel = true;
            }
        }
        public void RefreshEmulators(string[] emuIDs, bool checkIfSameTemp)
        {
            if (emuIDs == null)
                goto REFRESH;
            if (emuIDs.Length == 0)
                goto REFRESH;
            bool needToQuit = true;
            if (emuIDsTemp == null)
                emuIDsTemp = new List<string>();
            if (checkIfSameTemp)
            {
                foreach (string emuID in emuIDs)
                {
                    if (!emuIDsTemp.Contains(emuID))
                    {
                        needToQuit = false;
                        break; ;
                    }
                }
                if (needToQuit)
                    return;
            }

        REFRESH:
            managedListView1.Items.Clear();
            emuIDsTemp = new List<string>(emuIDs);
            profileManager.Profile.SelectedEmulatorID = "";
            foreach (string emuID in emuIDs)
            {
                ManagedListViewItem item = new ManagedListViewItem();
                item.DrawMode = ManagedListViewItemDrawMode.UserDraw;
                item.Tag = emuID;
                item.Color = managedListView1.ForeColor;
                managedListView1.Items.Add(item);
            }
            if (managedListView1.Items.Count > 0)
            {
                managedListView1.Items[0].Selected = true;
                profileManager.Profile.SelectedEmulatorID = (string)managedListView1.Items[0].Tag;
            }
            managedListView1.Invalidate();
        }

        public void SortByName()
        {
            // Get all emulators of temp ids
            List<Emulator> emus = new List<Emulator>();
            foreach (string id in emuIDsTemp)
                emus.Add(profileManager.Profile.Emulators[id]);
            EmulatorsCollection collection = new EmulatorsCollection(null, emus.ToArray());
            // Remove them from original collection
            profileManager.Profile.Emulators.RemoveItems(collection);
            // Sort them
            collection.Sort(new EmulatorsComparer(emulatorsAtoZ, EmulatorCompareType.Name));
            emulatorsAtoZ = !emulatorsAtoZ;
            // Re-add add them
            profileManager.Profile.Emulators.AddRange(collection);
            // Re-sort temp ids
            emuIDsTemp = new List<string>();
            foreach (Emulator emu in collection)
                emuIDsTemp.Add(emu.ID);
            // Raise event
            profileManager.Profile.OnEmulatorsSorted();
            OnEnableDisableButtons();
        }
        public void MoveSelectedItemUp()
        {
            if (managedListView1.SelectedItems.Count != 1) return;
            // Get collection of all emulators we are handling now
            List<Emulator> emus = new List<Emulator>();
            foreach (string id in emuIDsTemp)
                emus.Add(profileManager.Profile.Emulators[id]);
            EmulatorsCollection collection = new EmulatorsCollection(null, emus.ToArray());
            // Get index of current item
            string emu_id = (string)managedListView1.SelectedItems[0].Tag;
            int tempIndex = collection.IndexOf((string)managedListView1.SelectedItems[0].Tag);
            int index = profileManager.Profile.Emulators.IndexOf(collection[(string)managedListView1.SelectedItems[0].Tag]);
            tempIndex = tempIndex - 1;
            if (tempIndex < 0) return;
            int newIndex = profileManager.Profile.Emulators.IndexOf(collection[tempIndex].ID);// index of previous item
            if (newIndex < 0) return;
            if (newIndex >= profileManager.Profile.Emulators.Count)
                return;
            // Move the emulator within collection
            profileManager.Profile.Emulators.Move(index, newIndex);
            // Re-sort temp ids
            List<string> t = new List<string>();
            foreach (Emulator em in profileManager.Profile.Emulators)
            {
                if (emuIDsTemp.Contains(em.ID)) t.Add(em.ID);
            }
            emuIDsTemp = new List<string>(t.ToArray());

            RefreshEmulators(emuIDsTemp.ToArray(), false);
            for (int ii = 0; ii < managedListView1.Items.Count; ii++)
            {
                managedListView1.Items[ii].Selected = emu_id == managedListView1.Items[ii].Tag.ToString();
            }
            OnEnableDisableButtons();
        }
        public void MoveSelectedItemDown()
        {
            if (managedListView1.SelectedItems.Count != 1) return;
            // Get collection of all emulators we are handling now
            List<Emulator> emus = new List<Emulator>();
            foreach (string id in emuIDsTemp)
                emus.Add(profileManager.Profile.Emulators[id]);
            EmulatorsCollection collection = new EmulatorsCollection(null, emus.ToArray());
            // Get index of current item
            string emu_id = (string)managedListView1.SelectedItems[0].Tag;
            int tempIndex = collection.IndexOf((string)managedListView1.SelectedItems[0].Tag);
            int index = profileManager.Profile.Emulators.IndexOf(collection[(string)managedListView1.SelectedItems[0].Tag]);
            tempIndex = tempIndex + 1;
            if (tempIndex > collection.Count - 1) return;
            int newIndex = profileManager.Profile.Emulators.IndexOf(collection[tempIndex].ID);// index of next item
            if (newIndex > profileManager.Profile.Emulators.Count - 1) return;
            if (newIndex < 0) return;
            if (newIndex >= profileManager.Profile.Emulators.Count)
                return;
            // Move the emulator within collection
            profileManager.Profile.Emulators.Move(index, newIndex);
            // Re-sort temp ids
            List<string> t = new List<string>();
            foreach (Emulator em in profileManager.Profile.Emulators)
            {
                if (emuIDsTemp.Contains(em.ID)) t.Add(em.ID);
            }
            emuIDsTemp = new List<string>(t.ToArray());

            RefreshEmulators(emuIDsTemp.ToArray(), false);
            for (int ii = 0; ii < managedListView1.Items.Count; ii++)
            {
                managedListView1.Items[ii].Selected = emu_id == managedListView1.Items[ii].Tag.ToString();
            }
            OnEnableDisableButtons();
        }
        protected override void OnEnableDisableButtons()
        {
            base.OnEnableDisableButtons();
            deleteToolStripMenuItem.Enabled = CanDelete;
            renameToolStripMenuItem.Enabled = CanRename;
            changeIconToolStripMenuItem.Enabled = clearIconToolStripMenuItem.Enabled = CanChangeIcon;
            propertiesToolStripMenuItem.Enabled = CanShowProperties;
            toolStripButton_moveDown.Enabled = toolStripButton_moveUp.Enabled = managedListView1.SelectedItems.Count == 1;
        }
        protected override void OnCreatingNewProfile()
        {
            base.OnCreatingNewProfile();
            managedListView1.Items.Clear();
        }
        protected override void OnOpeningProfile()
        {
            base.OnOpeningProfile();
            managedListView1.Items.Clear();
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            // When saving a profile, disable editing and moving
            toolStrip1.Enabled = false;
            panel1.Enabled = false;
            contextMenuStrip1.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            toolStrip1.Enabled = true;
            panel1.Enabled = true;
            contextMenuStrip1.Enabled = true;
        }

        private void Profile_EmulatorMoved(object sender, EventArgs e)
        {

        }
        private void Profile_EmulatorsSorted(object sender, EventArgs e)
        {
            RefreshEmulators(emuIDsTemp.ToArray(), false);
        }
        private void Profile_EmulatorsCleared(object sender, EventArgs e)
        {
            managedListView1.Items.Clear();
            managedListView1.Invalidate();
        }
        private void Profile_EmulatorRemoved(object sender, EventArgs e)
        {

        }
        private void Profile_EmulatorAdded(object sender, EventArgs e)
        {

        }
        private void Profile_EmulatorRenamed(object sender, EventArgs e)
        {
            managedListView1.Invalidate();
        }
        private void Profile_EmulatorsRefreshRequest(object sender, RefreshEmulatorsArgs e)
        {
            RefreshEmulators(e.EmulatorIDs, true);
        }
        private void Profile_ConsoleRemoved(object sender, EventArgs e)
        {
            managedListView1.Items.Clear();
        }
        private void Profile_ProfileCleanUpFinished(object sender, EventArgs e)
        {
            RefreshEmulators(new string[0], true);
        }
        private void EmulatorsBrowser_Enter(object sender, EventArgs e)
        {
            panel1.Visible = toolStrip1.Visible = true;
        }
        private void managedListView1_Leave(object sender, EventArgs e)
        {
            panel1.Visible = toolStrip1.Visible = false;
        }
        private void AddEmulatorClick(object sender, EventArgs e)
        {
            AddEmulator("NULL");
        }
        // Draw
        private void managedListView1_DrawItem(object sender, ManagedListViewItemDrawArgs e)
        {
            // get emu
            Emulator emu = profileManager.Profile.Emulators[(string)managedListView1.Items[e.ItemIndex].Tag];
            if (emu != null)
            {
                e.ImageToDraw = emu.Icon;
                e.TextToDraw = emu.Name;
            }
        }
        private void trackBar_zoom_Scroll(object sender, EventArgs e)
        {
            managedListView1.ThunmbnailsHeight = managedListView1.ThunmbnailsWidth = trackBar_zoom.Value;
            managedListView1.Invalidate();

            // Save
            settings.AddValue(new SettingsValue("EmulatorsBrowser:ZoomValue", trackBar_zoom.Value));
        }
        private void addEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmulator("NULL");
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameSelected();
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowItemProperties();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SortByName();
        }
        private void changeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeIcon();
        }
        private void clearIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearIcon();
        }
        private void managedListView1_Click(object sender, EventArgs e)
        {
            OnEnableDisableButtons();
        }
        private void managedListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnEnableDisableButtons();
            if (managedListView1.SelectedItems.Count == 1)
            {
                profileManager.Profile.SelectedEmulatorID = (string)managedListView1.SelectedItems[0].Tag;
            }
            else
            { profileManager.Profile.SelectedEmulatorID = ""; }
            profileManager.Profile.OnEmulatorSelectionChanged();
        }
        private void toolStripButton_moveUp_Click(object sender, EventArgs e)
        {
            MoveSelectedItemUp();
        }
        private void toolStripButton_moveDown_Click(object sender, EventArgs e)
        {
            MoveSelectedItemDown();
        }
        // Start emu
        private void managedListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            runToolStripMenuItem_Click(this, null);
        }
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 1)
            {
                try
                {
                    string path = HelperTools.GetFullPath(profileManager.Profile.Emulators[(string)managedListView1.SelectedItems[0].Tag].ExcutablePath);
                    Process emuProcess = new Process();
                    emuProcess.StartInfo.WorkingDirectory = HelperTools.GetDirectory(path);
                    emuProcess.StartInfo.FileName = path;
                    emuProcess.Start();
                }
                catch { }
            }
        }
        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 1)
            {
                try
                {
                    string path = HelperTools.GetFullPath(profileManager.Profile.Emulators[(string)managedListView1.SelectedItems[0].Tag].ExcutablePath);
                    Process.Start("explorer.exe", @"/select, " + path);
                }
                catch { }
            }
        }
        private void managedListView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                List<string> files = new List<string>((string[])e.Data.GetData(DataFormats.FileDrop));
                if (files.Count == 1)
                    e.Effect = DragDropEffects.Copy;
                else
                { e.Effect = DragDropEffects.None; }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void managedListView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                List<string> files = new List<string>((string[])e.Data.GetData(DataFormats.FileDrop));
                if (files.Count == 1)
                {
                    AddEmulator(files[0]);
                }
            }
        }
    }
}
