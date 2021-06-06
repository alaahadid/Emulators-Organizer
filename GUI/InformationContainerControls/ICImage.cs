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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace EmulatorsOrganizer.GUI
{
    public partial class ICImage : ICControl
    {
        public ICImage(string icid, string parentConsoleID)
        {
            InitializeComponent();
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;

            base.ApplyStyleOnRomSelection();

            // TODO: don't forget the download games from db
            searchTheGamesDBnetForImagesToolStripMenuItem.Visible = false;
            toolStripButton_search_gmdb.Visible = false;
        }
        private bool canSwitchOnClick;
        private Point mouseDownPoint;
        private bool isMouseDown = false;
        private ImageViewMode preferedImageView;

        public override void ApplyStyle(Core.EOStyle style)
        {
            base.ApplyStyle(style);
            trackBar_zoom.BackColor = toolStrip1.BackColor = toolStrip2.BackColor = this.BackColor = style.bkgColor_InformationContainerTabs;
            this.imagePanel1.DefaultImage = style.image_InformationContainerTabs;
            // Load settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                InformationContainerImage cont = (InformationContainerImage)profileManager.Profile.Consoles[parentConsoleID].GetInformationContainer(ICID);
                if (cont != null)
                {
                    preferedImageView = (ImageViewMode)cont.PreferedImageMode;
                    toolStrip1.Visible = cont.ShowToolBar;
                    toolStrip2.Visible = cont.ShowStatusBar;
                    toolStripButton_pixelate.Checked = cont.UseNearestNighborDraw;
                    toolStripButton_pixelate_CheckedChanged(this, new EventArgs());
                }
            }
        }
        protected override void ShowFile()
        {
            if (base.IsValidFileIndex())
            {
                // Get image
                try
                {
                    string filePath = HelperTools.GetFullPath(files[fileIndex]);
                    //Stream str = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    //byte[] buff = new byte[str.Length];
                    //str.Read(buff, 0, (int)str.Length);
                    //str.Dispose();
                    //str.Close();

                    imagePanel1.zoom = -1;
                    //imagePanel1.ImageToView = (Bitmap)Image.FromStream(new MemoryStream(buff));

                    using (var bmpTemp = new Bitmap(filePath))
                    {
                        imagePanel1.ImageToView = new Bitmap(bmpTemp);
                    }

                    // Return to preferred image view mode.
                    imagePanel1.ImageViewMode = preferedImageView;

                    toolTip1.SetToolTip(imagePanel1, filePath);
                    // Reached here means the load is success.
                    trackBar_zoom.Enabled = true;
                    toolStripButton_setOriginal.Enabled = true;
                    canSwitchOnClick = true;
                    imagePanel1.Text = "";
                }
                catch (Exception ex)
                {
                    ClearDisplay();
                    imagePanel1.ForeColor = Color.Red;
                    imagePanel1.Text = ls["Message_CantShowImage"] + "\n\n" + ex.Message;
                }
            }
            else
            {
                ClearDisplay();
                if (profileManager.Profile.SelectedRomIDS.Count == 1)
                {
                    //imagePanel1.ForeColor = Color.Black;
                    //imagePanel1.Text = ls["Message_PleaseSelectFile"];
                    toolTip1.SetToolTip(imagePanel1, ls["Message_PleaseSelectFile"]);
                }
            }
            imagePanel1.CalculateImageValues();
            imagePanel1.Invalidate();
        }
        protected override void UpdateStatus()
        {
            StatusLabel.Text = base.GetStatusString();
            //toolStripButton_next.Enabled = base.CanMoveNext();
            //toolStripButton_previous.Enabled = base.CanMovePrevious();
            toolStripButton_previous.Enabled = toolStripButton_next.Enabled = files.Count > 1;
        }
        private void ClearDisplay()
        {
            imagePanel1.ImageToView = null;
            trackBar_zoom.Enabled = false; imagePanel1.zoom = -1;
            toolStripButton_setOriginal.Enabled = false;
            imagePanel1.Text = "";
        }
        private void SaveSettings()
        {
            //try
            //{
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                ((InformationContainerImage)profileManager.Profile.Consoles[parentConsoleID].InformationContainers[index]).PreferedImageMode = (int)preferedImageView;
                ((InformationContainerImage)profileManager.Profile.Consoles[parentConsoleID].InformationContainers[index]).ShowToolBar = toolStrip1.Visible;
                ((InformationContainerImage)profileManager.Profile.Consoles[parentConsoleID].InformationContainers[index]).ShowStatusBar = toolStrip2.Visible;
                ((InformationContainerImage)profileManager.Profile.Consoles[parentConsoleID].InformationContainers[index]).UseNearestNighborDraw = toolStripButton_pixelate.Checked;
                profileManager.Profile.OnInformationContainerItemsModified(profileManager.Profile.Consoles[parentConsoleID].InformationContainers[index].DisplayName);
            }
            //}
            //catch { }
        }
        protected override void OnProfileSavingStarted()
        {
            if (!this.InvokeRequired)
                OnProfileSavingStartedThreaded();
            else
                Invoke(new Action(OnProfileSavingStartedThreaded));
        }
        private void OnProfileSavingStartedThreaded()
        {
            toolStripButton1.Enabled =
    toolStripButton2.Enabled =
    toolStripButton3.Enabled =
    toolStripButton6.Enabled =
    toolStripButton7.Enabled =
    toolStripButton_search_gmdb.Enabled = false;
            addImagesToolStripMenuItem.Enabled =
    removeImageToolStripMenuItem.Enabled =
    editListToolStripMenuItem.Enabled =
    searchGoogleForMoreImagesToolStripMenuItem.Enabled =
    searchAFolderForMoreImagesToolStripMenuItem.Enabled =
    searchTheGamesDBnetForImagesToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            if (!this.InvokeRequired)
                OnProfileSavingFinishedThreaded();
            else
                Invoke(new Action(OnProfileSavingFinishedThreaded));
        }
        private void OnProfileSavingFinishedThreaded()
        {
            toolStripButton1.Enabled =
toolStripButton2.Enabled =
toolStripButton3.Enabled =
toolStripButton6.Enabled =
toolStripButton7.Enabled =
toolStripButton_search_gmdb.Enabled = true;

            addImagesToolStripMenuItem.Enabled =
    removeImageToolStripMenuItem.Enabled =
    editListToolStripMenuItem.Enabled =
    searchGoogleForMoreImagesToolStripMenuItem.Enabled =
    searchAFolderForMoreImagesToolStripMenuItem.Enabled =
    searchTheGamesDBnetForImagesToolStripMenuItem.Enabled = true;
        }
        // Next
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            base.NextFile();
        }
        // Previous
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            base.PreviousFile();
        }
        // Edit list
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            base.EditList();
        }
        // Add files to list
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            base.AddFilesToList();
        }
        // Remove selected file from list
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            base.RemoveSelectedFileFromList();
        }
        // Open image
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            base.OpenSelectedFile();
        }
        // Locate
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            base.OpenSelectedFileLocation();
        }
        private void toolStripSplitButton1_DropDownOpening(object sender, EventArgs e)
        {
            for (int i = 0; i < toolStripSplitButton1.DropDownItems.Count; i++)
            {
                ((ToolStripMenuItem)toolStripSplitButton1.DropDownItems[i]).Checked = false;
            }
            switch (preferedImageView)
            {
                case ImageViewMode.StretchIfLarger: stretchToFitToolStripMenuItem.Checked = true; break;
                case ImageViewMode.StretchToFit: alwaysStretchToolStripMenuItem.Checked = true; break;
                case ImageViewMode.StretchNoAspectRatio: stretchnoAspectRatioToolStripMenuItem.Checked = true; break;
            }
        }
        private void alwaysStretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imagePanel1.ImageViewMode = ImageViewMode.StretchToFit;
            preferedImageView = ImageViewMode.StretchToFit;
            imagePanel1.Invalidate(); SaveSettings();
        }
        private void stretchToFitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imagePanel1.ImageViewMode = ImageViewMode.StretchIfLarger;
            preferedImageView = ImageViewMode.StretchIfLarger;
            SaveSettings();
            imagePanel1.Invalidate();
        }
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            switch (preferedImageView)
            {
                case ImageViewMode.StretchIfLarger:
                    {
                        imagePanel1.ImageViewMode = ImageViewMode.StretchToFit;
                        preferedImageView = ImageViewMode.StretchToFit;
                        imagePanel1.Invalidate();
                        break;
                    }
                case ImageViewMode.StretchToFit:
                    {
                        imagePanel1.ImageViewMode = ImageViewMode.StretchNoAspectRatio;
                        preferedImageView = ImageViewMode.StretchNoAspectRatio;
                        imagePanel1.Invalidate();
                        break;
                    }
                case ImageViewMode.StretchNoAspectRatio:
                    {
                        imagePanel1.ImageViewMode = ImageViewMode.StretchIfLarger;
                        preferedImageView = ImageViewMode.StretchIfLarger;
                        imagePanel1.Invalidate();
                        break;
                    }
            }
            SaveSettings();
        }
        // Zoom
        private void trackBar_zoom_Scroll(object sender, EventArgs e)
        {
            // switch to normal mode
            imagePanel1.ImageViewMode = ImageViewMode.Normal;
            float val = (float)trackBar_zoom.Value / 10;
            imagePanel1.zoom = val;
            toolTip1.SetToolTip(trackBar_zoom, "X " + val.ToString());
            imagePanel1.CalculateImageValues();
            imagePanel1.Invalidate();
        }
        private void imagePanel1_ImageViewModeChanged(object sender, EventArgs e)
        {
            vScrollBar1.Visible = hScrollBar1.Visible = imagePanel1.ImageViewMode == ImageViewMode.Normal;
        }
        private void imagePanel1_DisableScrollBars(object sender, EventArgs e)
        {
            vScrollBar1.Enabled = hScrollBar1.Enabled = false;
        }
        private void imagePanel1_EnableScrollBars(object sender, EventArgs e)
        {
            vScrollBar1.Enabled = hScrollBar1.Enabled = true;
        }
        private void imagePanel1_CalculateScrollValues(object sender, EventArgs e)
        {
            if (imagePanel1.viewImageWidth > this.Width)
            {
                hScrollBar1.Enabled = true;
                hScrollBar1.Maximum = imagePanel1.viewImageWidth - this.Width + 40;
                hScrollBar1.Value = imagePanel1.drawX * -1;
            }
            else
            {
                hScrollBar1.Enabled = false;
            }

            if (imagePanel1.viewImageHeight > this.Height)
            {
                vScrollBar1.Enabled = true;
                vScrollBar1.Maximum = imagePanel1.viewImageHeight - this.Height + 60;
                vScrollBar1.Value = imagePanel1.drawY * -1;
            }
            else
            {
                vScrollBar1.Enabled = false;
            }
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            imagePanel1.drawY = vScrollBar1.Value * -1;
            imagePanel1.Invalidate();
        }
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            imagePanel1.drawX = hScrollBar1.Value * -1;
            imagePanel1.Invalidate();
        }
        // Reset
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            // switch to normal mode
            imagePanel1.ImageViewMode = ImageViewMode.Normal;
            imagePanel1.zoom = -1;
            imagePanel1.CalculateImageValues();
            imagePanel1.Invalidate();
        }
        private void imagePanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.OpenSelectedFile();
        }
        private void imagePanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            if (canSwitchOnClick)
                base.NextFile();
        }
        private void imagePanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
                if (((e.X - mouseDownPoint.X) > 3 || (e.X - mouseDownPoint.X) < -3)
                    || ((e.Y - mouseDownPoint.Y) > 3 || (e.Y - mouseDownPoint.Y) < -3))
                {
                    canSwitchOnClick = false;
                }
        }
        private void imagePanel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            canSwitchOnClick = true;
            mouseDownPoint = new Point(e.X, e.Y);
            System.Threading.Thread.Sleep(100);
        }
        private void imagePanel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            System.Threading.Thread.Sleep(100);
        }
        // Search google
        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (profileManager.Profile.SelectedRomIDS.Count != 1)
            {
                MessageBox.Show(ls["Message_PleaseSelectOneRomFirst"]);
                return;
            }
            int GoogleNameIndex = (int)settings.GetValue("GoogleImage:SearchNameMethod", true, 0);
            string GoogleCustomName = (string)settings.GetValue("GoogleImage:CustomName", true, "");
            /*
           Rom name
Rom name + console name
console name + Rom name
Rom name + Information Container name
Information Container name + Rom name
Rom name + console name + Information Container name
             */
            string searchFor = "";
            Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
            InformationContainer cont = console.GetInformationContainer(ICID);
            switch (GoogleNameIndex)
            {
                case 0: searchFor = rom.Name; break;
                case 1: searchFor = rom.Name + " " + console.Name; break;
                case 2: searchFor = console.Name + " " + rom.Name; break;
                case 3: searchFor = rom.Name + " " + cont.DisplayName; break;
                case 4: searchFor = cont.DisplayName + " " + rom.Name; break;
                case 5: searchFor = rom.Name + " " + console.Name + " " + cont.DisplayName; break;
            }
            Form_GetImagesFromGoogleImage searcher = new Form_GetImagesFromGoogleImage(searchFor + " " + GoogleCustomName,
                ((InformationContainerImage)cont).GoogleImageSearchFolder);
            if (searcher.ShowDialog(this) == DialogResult.OK)
            {
                string selectedPath = searcher.FolderSelected;
                Form_DownloadFiles down = new Form_DownloadFiles(searcher.FolderSelected, searcher.SelectedImageLinks.ToArray(),
                    rom.Name);
                if (down.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    base.AddFilesToList(down.DownloadedPaths.ToArray(), true);
                    // Update folder
                    ((InformationContainerImage)console.GetInformationContainer(ICID)).GoogleImageSearchFolder
                        = searcher.FolderSelected;
                }
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            showToolbarToolStripMenuItem.Checked = toolStrip1.Visible;
            showStatusbarToolStripMenuItem.Checked = toolStrip2.Visible;
            nextImageToolStripMenuItem.Enabled = previousImageToolStripMenuItem.Enabled = files.Count > 1;
            pixilatedModeToolStripMenuItem.Checked = toolStripButton_pixelate.Checked;
        }
        private void showToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
            showToolbarToolStripMenuItem.Checked = toolStrip1.Visible; SaveSettings();
        }
        private void showStatusbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip2.Visible = !toolStrip2.Visible;
            showStatusbarToolStripMenuItem.Checked = toolStrip2.Visible;
            SaveSettings();
        }
        private void imageModeToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            for (int i = 0; i < imageModeToolStripMenuItem.DropDownItems.Count; i++)
            {
                ((ToolStripMenuItem)imageModeToolStripMenuItem.DropDownItems[i]).Checked = false;
            }
            switch (preferedImageView)
            {
                case ImageViewMode.StretchIfLarger: normalstretchToFitToolStripMenuItem.Checked = true; break;
                case ImageViewMode.StretchToFit: alwaysStretchToolStripMenuItem1.Checked = true; break;
                case ImageViewMode.StretchNoAspectRatio: stretchnoAspectRatioToolStripMenuItem1.Checked = true; break;
            }
        }
        private void stretchnoAspectRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imagePanel1.ImageViewMode = ImageViewMode.StretchNoAspectRatio;
            preferedImageView = ImageViewMode.StretchNoAspectRatio;
            imagePanel1.Invalidate(); SaveSettings();
        }
        // Search folder for images
        private void toolStripButton7_Click_1(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            if (profileManager.Profile.SelectedRomIDS.Count != 1)
            {
                MessageBox.Show(ls["Message_PleaseSelectOneRomFirst"]);
                return;
            }
            Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
            InformationContainer cont = console.GetInformationContainer(ICID);

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = ls["Title_SelectTheFolderYouWantToSearch"];
            foreach (string fol in ((InformationContainerFiles)cont).FoldersMemory)
            {
                if (Directory.Exists(fol))
                {
                    folderBrowser.SelectedPath = HelperTools.GetFullPath(fol);
                    break;
                }
            }
            if (!Directory.Exists(folderBrowser.SelectedPath))
                if (folderBrowser.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
            int NameIndex = (int)settings.GetValue("GetImagesFromFolder:SearchNameMethod", true, 0);
            string CustomName = (string)settings.GetValue("GetImagesFromFolder:CustomName", true, "");
            string SaveToFolder = (string)settings.GetValue("GetImagesFromFolder:SaveToFolder", true, folderBrowser.SelectedPath);
            string searchFor = "";

            switch (NameIndex)
            {
                case 0: searchFor = rom.Name; break;
                case 1: searchFor = rom.Name + " " + console.Name; break;
                case 2: searchFor = console.Name + " " + rom.Name; break;
                case 3: searchFor = rom.Name + " " + cont.DisplayName; break;
                case 4: searchFor = cont.DisplayName + " " + rom.Name; break;
                case 5: searchFor = rom.Name + " " + console.Name + " " + cont.DisplayName; break;
            }
            Form_GetImagesFromFolder searcher = new Form_GetImagesFromFolder(searchFor + " " + CustomName,
                folderBrowser.SelectedPath, SaveToFolder, ((InformationContainerFiles)cont).GetDefaultExtensionsJoined());
            if (searcher.ShowDialog(this) == DialogResult.OK)
            {
                string selectedPath = searcher.FolderSelected;
                settings.AddValue("GetImagesFromFolder:SaveToFolder", selectedPath);
                foreach (string file in searcher.SelectedImageFiles)
                {
                    try
                    {
                        File.Copy(file, selectedPath + "\\" + Path.GetFileName(file));
                    }
                    catch
                    {

                    }
                }
                base.AddFilesToList(searcher.SelectedImageFiles.ToArray(), true);
            }
        }
        private void toolStripButton_pixelate_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripButton_pixelate.Checked)
                imagePanel1.DrawInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            else
                imagePanel1.DrawInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
        }
        private void pixilatedModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_pixelate.Checked = !toolStripButton_pixelate.Checked;
            SaveSettings();
        }
        private void toolStripButton_search_gmdb_Click(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
          /*  Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
            InformationContainer cont = console.GetInformationContainer(ICID);
            Form_SearchTheGamesDB frm = new Form_SearchTheGamesDB(rom.ID);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                TheGamesDBAPI.Game gm = TheGamesDBAPI.GamesDB.GetGame(frm.SelectedResultID);
                Form_TheGamesDBImageMode frm2 = new Form_TheGamesDBImageMode(gm.Images);
                if (frm2.ShowDialog(this) == DialogResult.OK)
                {
                    // Browse for a folder
                    FolderBrowserDialog fol = new FolderBrowserDialog();
                    string selectedPath = ((InformationContainerImage)cont).GoogleImageSearchFolder;
                    if (((InformationContainerImage)cont).FoldersMemory != null)
                        if (((InformationContainerImage)cont).FoldersMemory.Count > 0)
                            selectedPath = ((InformationContainerImage)cont).FoldersMemory[0];
                    fol.SelectedPath = selectedPath;
                    fol.Description = "Choose where to download the files.";
                    if (fol.ShowDialog(this) == DialogResult.OK)
                    {
                        List<string> links = new List<string>();
                        if (frm2.SelectedBanners)
                        {
                            for (int i = 0; i < gm.Images.Banners.Count; i++)
                                links.Add(TheGamesDBAPI.GamesDB.BaseImgURL + gm.Images.Banners[i].Path);
                        }
                        else if (frm2.SelectedBoxArtBack)
                        {
                            links.Add(TheGamesDBAPI.GamesDB.BaseImgURL + gm.Images.BoxartBack.Path);
                        }
                        else if (frm2.SelectedBoxArtFront)
                        {
                            links.Add(TheGamesDBAPI.GamesDB.BaseImgURL + gm.Images.BoxartFront.Path);
                        }
                        else if (frm2.SelectedFanart)
                        {
                            for (int i = 0; i < gm.Images.Fanart.Count; i++)
                                links.Add(TheGamesDBAPI.GamesDB.BaseImgURL + gm.Images.Fanart[i].Path);
                        }
                        else if (frm2.SelectedScreenshots)
                        {
                            for (int i = 0; i < gm.Images.Screenshots.Count; i++)
                                links.Add(TheGamesDBAPI.GamesDB.BaseImgURL + gm.Images.Screenshots[i].Path);
                        }
                        Form_DownloadFiles down = new Form_DownloadFiles(fol.SelectedPath, links.ToArray(),
                                rom.Name);
                        if (down.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            base.AddFilesToList(down.DownloadedPaths.ToArray(), true);
                            if (((InformationContainerImage)cont).FoldersMemory != null)
                                if (!((InformationContainerImage)cont).FoldersMemory.Contains(fol.SelectedPath))
                                    ((InformationContainerImage)cont).FoldersMemory.Insert(0, fol.SelectedPath);
                        }
                    }
                }
            }*/
        }
    }
}
