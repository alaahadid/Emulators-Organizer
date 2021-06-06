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
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_GetImagesFromGoogleImage : Form
    {
        public Form_GetImagesFromGoogleImage(string SearchFor, string folder)
        {
            InitializeComponent();
            GoogleImageSearcher.ResultsCount = 20;
            textBox1.Text = SearchFor;
            textBox2.Text = HelperTools.GetFullPath(folder);
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private List<string> SelectedLinks = new List<string>();
        public event EventHandler OptionRequest;
        private delegate void AddListViewItem(string item);
        private delegate void AddImageToList(Image item);
        private delegate void ClearListView();
        private bool isSearching;
        private Thread mainThread;

        private string _SearchName;
        private string _SearchSize;

        void AddItem(string item)
        {
            if (!this.InvokeRequired)
                AddItem1(item);
            else
                this.Invoke(new AddListViewItem(AddItem1), new object[] { item });
        }
        void AddItem1(string item)
        {
            listView1.Items.Add(item);
        }
        void AddImage(Image item)
        {
            if (!this.InvokeRequired)
                AddImage1(item);
            else
                this.Invoke(new AddImageToList(AddImage1), new object[] { item });
        }
        void AddImage1(Image item)
        {
            if (item != null)
                imageList1.Images.Add(item);
        }
        void ClearTheList()
        {
            if (!this.InvokeRequired)
                ClearTheList1();
            else
                this.Invoke(new ClearListView(ClearTheList1));
        }
        void ClearTheList1()
        {
            listView1.Items.Clear();
        }
        void ClearTheImageList()
        {
            if (!this.InvokeRequired)
                ClearTheImageList1();
            else
                this.Invoke(new ClearListView(ClearTheImageList1));
        }
        void ClearTheImageList1()
        {
            imageList1.Images.Clear();
        }
        public List<string> SelectedImageLinks
        {
            get { return SelectedLinks; }
        }
        public string FolderSelected
        {
            get { return textBox2.Text; }
        }

        void SearchImages()
        {
            try
            {
                isSearching = true;
                GoogleImageSearcher.GetImages(_SearchName, _SearchSize);
                ClearTheList();
                ClearTheImageList();
                for (int i = 0; i < GoogleImageSearcher.ImageThunmbnails.Length; i++)
                {
                    AddImage(getImage(GoogleImageSearcher.ImageThunmbnails[i]));
                    AddItem(GoogleImageSearcher.ImageLinks[i]);
                }
            }
            catch
            {
                isSearching = false;
                MessageBox.Show(ls["Message_UnableToConnect"]);
            }
            if (this.InvokeRequired)
            { this.Invoke(new Action(Done)); }
            else
            {
                Done();
            }
        }
        private void Done()
        {
            isSearching = false;
            label_status.Text = ls["Status_Done"];
        }
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            int h = imageList1.ImageSize.Height;
            int w = imageList1.ImageSize.Width;
            if (e.Item.Selected)
            {
                if (listView1.Focused)
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight),
                      e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.ButtonFace),
                e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
            //try
            {
                e.Graphics.DrawImage(imageList1.Images[e.ItemIndex], e.Bounds.X + ((e.Bounds.Width / 2) - (w / 2)), e.Bounds.Y + 2, w, h);
                Size ss = TextRenderer.MeasureText(GoogleImageSearcher.ImageSizes[e.ItemIndex], new Font("Tahoma", 8, FontStyle.Regular));
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, ss.Width + 3, ss.Height));
                e.Graphics.DrawString(GoogleImageSearcher.ImageSizes[e.ItemIndex], new Font("Tahoma", 8, FontStyle.Regular),
                    new SolidBrush(Color.White), new PointF(e.Bounds.X + 1, e.Bounds.Y + 1));
            }
            // catch { }
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GoogleImageSearcher.ResultsCount = (int)numericUpDown1.Value;
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show(ls["Message_PleaseEnterTheRomNameToSearchFor"]);
                return;
            }
            _SearchName = textBox1.Text;
            if (!checkBox1.Checked)
            {
                _SearchSize = "";
            }
            else
            {
                if (numericUpDown_h.Value > 0 & numericUpDown_width.Value > 0)
                {
                    _SearchSize = numericUpDown_width.Value + "x" + numericUpDown_h.Value;
                }
                else
                {
                    MessageBox.Show(ls["Message_TheImageSizeMUSTNotIncludeZeros"]);
                    return;
                }
            }
            label_status.Visible = true;
            label_status.Text = ls["Status_SearchingForImages"] + " ...";
            mainThread = new Thread(new ThreadStart(SearchImages));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }

        private Image getImage(string url)
        {
            Image im = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 15000;
                request.ProtocolVersion = HttpVersion.Version11;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        im = Image.FromStream(responseStream);
                    }
                }
            }
            catch
            {
            }
            return im;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(listView1.SelectedItems[0].Text);
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(ls["Message_SelectImageYouWantToDownloadFirst"]);
                return;
            }
            if (!Directory.Exists(HelperTools.GetFullPath(textBox2.Text)))
            {
                DialogResult res = MessageBox.Show(ls["Message_ThisFolderIsntExistDoYouWantToCreateIt"] + "\n" + textBox2.Text,
                    ls["MessageCaption_GetImagesFromGoogleImage"],
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(HelperTools.GetFullPath(textBox2.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ls["MessageCaption_GetImagesFromGoogleImage"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (res == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else if (res == System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show(ls["Message_PleaseBrowseForADownloadFolderFirst"]);
                    textBox2.SelectAll();
                    return;
                }
            }
            if (mainThread != null)
            {
                if (mainThread.IsAlive)
                {
                    if (MessageBox.Show(ls["Message_ItStillSearching"],
                           ls["MessageCaption_GetImagesFromGoogleImage"],
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == System.Windows.Forms.DialogResult.Yes)
                    {
                        mainThread.Abort();
                    }
                    else
                        return;
                }
            }
            SelectedLinks.Clear();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                SelectedLinks.Add(item.Text);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://images.google.com/");
            }
            catch { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = ls["Message_SelectFolderWhereToSaveFiles"];
            fol.SelectedPath = textBox2.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = fol.SelectedPath;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (OptionRequest != null)
                OptionRequest(sender, e);
        }

        private void Frm_GetImagesFromGoogleImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainThread != null)
            {
                if (mainThread.IsAlive)
                {

                    if (MessageBox.Show(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                      ls["MessageCaption_GetImagesFromGoogleImage"],
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == System.Windows.Forms.DialogResult.Yes)
                    {
                        mainThread.Abort();
                        label_status.Text = ls["Status_Canceled"];
                    }
                    else
                        e.Cancel = true;
                }
            }
        }
    }
}
