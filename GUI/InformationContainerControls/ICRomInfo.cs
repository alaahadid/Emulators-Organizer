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
using System.Drawing;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class ICRomInfo : ICControl
    {
        public ICRomInfo(string icid, string parentConsoleID)
        {
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;
            InitializeComponent();
            base.ApplyStyleOnRomSelection();
            base.canAcceptDraggedFiles = false;
            base.canSelectionReverse = false;
        }
        private RomThumbnailInfo thumbInf = new RomThumbnailInfo();
        private object temp;
        public override void InitializeEvents()
        {
            base.InitializeEvents();

            if (profileManager.Profile != null)
            {
                profileManager.Profile.RomFinishedPlayed += Profile_RomFinishedPlayed;
            }
        }
        public override void DisposeEvents()
        {
            base.DisposeEvents();

            if (profileManager.Profile != null)
            {
                profileManager.Profile.RomFinishedPlayed -= Profile_RomFinishedPlayed;
            }
        }
        protected override void RefreshFiles()
        {
            // Clear all information first
            ClearInfo();
            if (parentConsoleID == null)
                return;
            if (parentConsoleID == "")
                return;
            if (IsDisposed)
                return;
            // Load rom
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                if (rom == null)
                    return;
                label_name.Text = rom.Name;
                toolTip1.SetToolTip(label_name, rom.Name);

                label_path.Text = rom.Path;
                toolTip1.SetToolTip(label_path, rom.Path);

                //label_playCounters.Text = ls["Info_Played"] + " " + rom.PlayedTimes + " " + ls["Info_TimesIn"] + " " +
                //    TimeSpan.FromMilliseconds(rom.PlayedTimeLength) + " " +
                //    ls["Info_LastPlayedAt"] + " " + rom.LastPlayed.ToLocalTime();
                if (rom.PlayedTimes > 0)
                {
                    string playTime = TimeSpan.FromMilliseconds(rom.PlayedTimeLength).ToString();
                    if (playTime.Length > 12)
                        playTime = playTime.Substring(0, 12);

                    label_playCounters.Text = ls["Info_Played"] + " " + rom.PlayedTimes + " " + ls["Info_TimesIn"] + " " +
                       playTime + " [" + ls["Info_LastPlayedAt"] + " " + rom.LastPlayed.ToLocalTime() + "]";
                }
                else
                {
                    label_playCounters.Text = ls["Status_NotPlayed"];
                }
                toolTip1.SetToolTip(label_playCounters, label_playCounters.Text);

                label_size.Text = ls["Info_Size"] + ": " + rom.SizeLable;
                toolTip1.SetToolTip(label_size, label_size.Text);

                // Load thumbnails
                thumbInf = new RomThumbnailInfo();
                EmulatorsOrganizer.Core.Console parentConsole = profileManager.Profile.Consoles[rom.ParentConsoleID];
                List<string> thumbFiles = new List<string>();
                foreach (InformationContainer con in parentConsole.InformationContainers)
                {
                    if (con is InformationContainerImage)
                    {
                        if (rom.IsInformationContainerItemExist(con.ID))
                        {
                            // Add the files
                            InformationContainerItemFiles icitem = (InformationContainerItemFiles)rom.GetInformationContainerItem(con.ID);
                            if (icitem != null)
                                if (icitem.Files != null)
                                    thumbFiles.AddRange(icitem.Files);
                        }
                    }
                }
                thumbInf.ThumbnailFiles = thumbFiles.ToArray();
                thumbInf.ThumbFileIndex = 0;
                if (thumbFiles.Count > 0)
                {
                    timer_thumb_Tick(this, null);
                    timer_thumb.Start();
                }
                // Rating
                rating1.Enabled = true;
                rating1.rating = rom.Rating;
                // Data
                foreach (RomData inf in parentConsole.RomDataInfoElements)
                {
                    dataGridView1.Rows.Add(inf, rom.GetDataItemValue(inf.ID));
                }
            }
        }
        private void RefreshCounter()
        {
            if (parentConsoleID == null)
                return;
            if (parentConsoleID == "")
                return;
            if (IsDisposed)
                return;
            // Load rom
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                if (rom == null)
                    return;

                if (rom.PlayedTimes > 0)
                {
                    string playTime = TimeSpan.FromMilliseconds(rom.PlayedTimeLength).ToString();
                    if (playTime.Length > 12)
                        playTime = playTime.Substring(0, 12);

                    label_playCounters.Text = ls["Info_Played"] + " " + rom.PlayedTimes + " " + ls["Info_TimesIn"] + " " +
                       playTime + " [" + ls["Info_LastPlayedAt"] + " " + rom.LastPlayed.ToLocalTime() + "]";
                }
                else
                {
                    label_playCounters.Text = ls["Status_NotPlayed"];
                }
                toolTip1.SetToolTip(label_playCounters, label_playCounters.Text);
            }
        }
        private void ClearInfo()
        {
            imagePanel1.ImageToView = null;
            imagePanel1.Invalidate();
            label_name.Text = ls["Status_PleaseSelectRom"];
            label_path.Text = "";
            label_playCounters.Text = "";
            label_size.Text = "";
            thumbInf = new RomThumbnailInfo();
            timer_thumb.Stop();
            rating1.rating = 0;
            rating1.Enabled = false;
            dataGridView1.Rows.Clear();
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            dataGridView1.BackgroundColor = style.bkgColor_InformationContainerTabs;

            // if (style.EOBackground != null)
            //     imagePanel1.DefaultImage = style.EOBackground;
            // else
            //     imagePanel1.DefaultImage = Properties.Resources.EmulatorsOrganizer;

            label_name.ForeColor = style.txtColor_InformationContainerTabs;
            label_path.ForeColor = style.txtColor_InformationContainerTabs;
            label_playCounters.ForeColor = style.txtColor_InformationContainerTabs;
            label_size.ForeColor = style.txtColor_InformationContainerTabs;
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            rating1.Enabled = false;
            dataGridView1.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            if (!InvokeRequired)
                AfterSaving();
            else
                Invoke(new Action(AfterSaving));
        }
        private void AfterSaving()
        {
            rating1.Enabled = true;
            dataGridView1.Enabled = true;
        }
        private void GetRatioStretchRectangle(int orgWidth, int orgHeight, int maxWidth, int maxHeight,
                                           ref int out_x, ref int out_y, ref int out_w, ref int out_h)
        {
            float hRatio = orgHeight / maxHeight;
            float wRatio = orgWidth / maxWidth;
            bool touchTargetFromOutside = false;
            if ((wRatio > hRatio) ^ touchTargetFromOutside)
            {
                out_w = maxWidth;
                out_h = (orgHeight * maxWidth) / orgWidth;
            }
            else
            {
                out_h = maxHeight;
                out_w = (orgWidth * maxHeight) / orgHeight;
            }
            out_x = (maxWidth - out_w) / 2;
            out_y = (maxHeight - out_h) / 2;
        }

        // Thumbnails timer
        private void timer_thumb_Tick(object sender, EventArgs e)
        {
            if (thumbInf.ThumbnailFiles.Length > 0)
            {
                if (thumbInf.ThumbFileIndex >= 0 && thumbInf.ThumbFileIndex < thumbInf.ThumbnailFiles.Length)
                {
                    try
                    {
                        if (imagePanel1.ImageToView != null)
                            imagePanel1.ImageToView.Dispose();
                        //Stream str = new FileStream(thumbInf.ThumbnailFiles[thumbInf.ThumbFileIndex], FileMode.Open, FileAccess.Read);
                        //byte[] buff = new byte[str.Length];
                        //str.Read(buff, 0, (int)str.Length);
                        //str.Dispose();
                        //str.Close();

                        int x = 0;
                        int y = 0;
                        int w = 0;
                        int h = 0;
                        //Image img = Image.FromStream(new MemoryStream(buff));
                        Image img = null;
                        using (var bmpTemp = new Bitmap(thumbInf.ThumbnailFiles[thumbInf.ThumbFileIndex]))
                        {
                            img = new Bitmap(bmpTemp);
                        }
                        GetRatioStretchRectangle(img.Width, img.Height, imagePanel1.Width, imagePanel1.Height, ref x, ref y, ref w, ref h);

                        imagePanel1.ImageToView = (Bitmap)img.GetThumbnailImage(w, h, null, IntPtr.Zero);
                        imagePanel1.Invalidate();
                    }
                    catch { }
                }
                thumbInf.ThumbFileIndex = (thumbInf.ThumbFileIndex + 1) % thumbInf.ThumbnailFiles.Length;
            }
            else
            {
                timer_thumb.Stop();
            }
        }
        private void rating1_RatingChanged(object sender, EventArgs e)
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                rom.Rating = rating1.rating;
                profileManager.Profile.OnRomRatingChanged(rom.Name);
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Value changing
            if (e.ColumnIndex == 1)
            {
                RomData d = (RomData)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                switch (d.Type)
                {
                    case RomDataType.Text: break;// it's ok

                    case RomDataType.Number:// must be number
                        {
                            int val = 0;
                            if (!int.TryParse((string)dataGridView1.Rows[e.RowIndex].Cells[1].Value, out val))
                            {
                                // Reset
                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = temp;
                            }
                            else
                            {
                                // Confirm
                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = val;
                            }
                            break;
                        }
                }
                // Save !
                if (profileManager.Profile.SelectedRomIDS.Count == 1)
                {
                    Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                    if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
                    {
                        rom.UpdateDataInfoItemValue(d.ID, dataGridView1.Rows[e.RowIndex].Cells[1].Value);
                    }
                    else
                    {
                        switch (d.Type)
                        {
                            case RomDataType.Text: rom.UpdateDataInfoItemValue(d.ID, ""); break;
                            case RomDataType.Number: rom.UpdateDataInfoItemValue(d.ID, 0); break;
                        }
                    }
                    profileManager.Profile.OnRomPropertiesChanged(rom.Name, rom.ID, false);
                }
            }
        }
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                // Save object
                temp = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }
        private void Profile_RomFinishedPlayed(object sender, RomFinishedPlayArgs e)
        {
            if (!this.InvokeRequired)
                RefreshCounter();
            else
                this.Invoke(new Action(RefreshCounter));
        }
    }
}
