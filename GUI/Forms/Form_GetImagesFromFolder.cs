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
    public partial class Form_GetImagesFromFolder : Form
    {
        public Form_GetImagesFromFolder(string SearchFor, string searchFolder, string saveToFolder, string extensions)
        {
            InitializeComponent();
            GoogleImageSearcher.ResultsCount = 20;
            textBox_searchFor.Text = SearchFor;
            _SearchFolder = textBox_searchFolder.Text = HelperTools.GetFullPath(searchFolder);
            textBox2.Text = HelperTools.GetFullPath(saveToFolder);

            comboBox_condition.Items.Clear();

            comboBox_condition.Items.Add(ls["SearchCondition_Contains"]);
            comboBox_condition.Items.Add(ls["SearchCondition_DoesNotContain"]);
            comboBox_condition.Items.Add(ls["SearchCondition_Is"]);
            comboBox_condition.Items.Add(ls["SearchCondition_IsNot"]);
            comboBox_condition.Items.Add(ls["SearchCondition_StartWith"]);
            comboBox_condition.Items.Add(ls["SearchCondition_DoesNotStartWith"]);
            comboBox_condition.Items.Add(ls["SearchCondition_EndWith"]);
            comboBox_condition.Items.Add(ls["SearchCondition_DoesNotEndWith"]);

            comboBox_condition.SelectedIndex = 0;

            textBox_extensions.Text = original_extensions = extensions;
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private delegate void AddListViewItem(string item);
        private delegate void AddImageToList(Image item);
        private delegate void ClearListView();
        private bool isSearching;
        private Thread mainThread;
        private string _SearchName;
        private string _SearchFolder;
        private bool _subfodlers;
        private bool _matchCase;
        private bool _matchWord;
        private string original_extensions;
        private TextSearchCondition condition;
        private List<string> files = new List<string>();

        void AddItem(string item)
        {
            if (!this.InvokeRequired)
                AddItem1(item);
            else
                this.Invoke(new AddListViewItem(AddItem1), new object[] { item });
        }
        void AddItem1(string item)
        {
            ListViewItem it = new ListViewItem();
            it.Text = Path.GetFileName(item) + " [" + item + "]";
            it.Tag = item;
            listView1.Items.Add(it);
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
        public List<string> SelectedImageFiles { get; set; }
        public string FolderSelected
        {
            get { return textBox2.Text; }
        }

        private void SearchImages()
        {
            try
            {
                isSearching = true;
                ClearTheList();
                ClearTheImageList();
                // Start search
                files = new List<string>(Directory.GetFiles(_SearchFolder, "*",
                    _subfodlers ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
                List<string> results = new List<string>();
                List<string> extensions = new List<string>(textBox_extensions.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

                foreach (string file in files)
                {
                    if (extensions.Contains(Path.GetExtension(file).ToLower()))
                        if (FilterSearch(_SearchName, file))
                        {
                            AddImage(getImage(file));
                            AddItem(file);
                        }
                }
            }
            catch
            {
            }
            if (this.InvokeRequired)
            { this.Invoke(new Action(Done)); }
            else
            {
                Done();
            }
        }
        private bool FilterSearch(string searchWhat, string filePath)
        {
            if (searchWhat == "") return false;
            // Let's see what's the mode
            string searchWord = _matchCase ? searchWhat : searchWhat.ToLower();
            string searchTargetText = Path.GetFileNameWithoutExtension(filePath);
            searchTargetText = _matchCase ? searchTargetText : searchTargetText.ToLower();

            // Decode user code
            string[] searchCodes = searchWord.Split(new char[] { '+' });
            // Do the search
            switch (condition)
            {
                case TextSearchCondition.Contains:// The target contains the search word
                    {
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText.Contains(s))
                                return true;
                        }
                        return false;
                    }
                case TextSearchCondition.DoesNotContain:// The target doesn't contain the search word
                    {
                        bool add = true;
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText.Contains(s))
                            {
                                add = false; break;
                            }
                        }
                        return add;
                    }
                case TextSearchCondition.Is:// Match the word !
                    {
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText == s)
                                return true;
                        }
                        return false;
                    }
                case TextSearchCondition.IsNot:// Don't match the word !
                    {
                        bool add = true;
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText == s)
                            {
                                add = false; break;
                            }
                        }
                        return add;
                    }
                case TextSearchCondition.StartWith:// The target starts the search word
                    {
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText.StartsWith(s))
                                return true;
                        }
                        return false;
                    }
                case TextSearchCondition.DoesNotStartWith:// The target doesn't start the search word
                    {
                        bool add = true;
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText.StartsWith(s))
                            {
                                add = false; break;
                            }
                        }
                        return add;
                    }
                case TextSearchCondition.EndWith:// The target ends the search word
                    {
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText.EndsWith(s))
                                return true;
                        }
                        return false;
                    }
                case TextSearchCondition.DoesNotEndWith:// The target doesn't end with the search word
                    {
                        bool add = true;
                        foreach (string s in searchCodes)
                        {
                            if (searchTargetText.EndsWith(s))
                            {
                                add = false; break;
                            }
                        }
                        return add;
                    }
            }

            return false;
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
                e.Graphics.DrawString(imageList1.Images[e.ItemIndex].Width + " x " + imageList1.Images[e.ItemIndex].Height, new Font("Tahoma", 8, FontStyle.Regular),
                    new SolidBrush(Color.White), new PointF(e.Bounds.X + 1, e.Bounds.Y + 1));
            }
            // catch { }
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis);

        }
        private Image getImage(string url)
        {
            Image im = null;
            try
            {
                //Stream str = new FileStream(url, FileMode.Open, FileAccess.Read);
                //byte[] buff = new byte[str.Length];
                //str.Read(buff, 0, (int)str.Length);
                //str.Dispose();
                //str.Close();

                //im = Image.FromStream(new MemoryStream(buff));
                using (var bmpTemp = new Bitmap(url))
                {
                    im = new Bitmap(bmpTemp);
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
        // OK
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
                    ls["MessageCaption_GetImagesFromFolder"],
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(HelperTools.GetFullPath(textBox2.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ls["MessageCaption_GetImagesFromFolder"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (res == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else if (res == System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show(ls["Message_PleaseBrowseForTheTargetFolderFirst"]);
                    textBox2.SelectAll();
                    return;
                }
            }
            if (mainThread != null)
            {
                if (mainThread.IsAlive)
                {
                    if (MessageBox.Show(ls["Message_ItStillSearching"],
                           ls["MessageCaption_GetImagesFromFolder"],
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == System.Windows.Forms.DialogResult.Yes)
                    {
                        mainThread.Abort();
                    }
                    else
                        return;
                }
            }
            SelectedImageFiles = new List<string>();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                SelectedImageFiles.Add(item.Tag.ToString());
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
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
        private void Frm_GetImagesFromGoogleImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainThread != null)
            {
                if (mainThread.IsAlive)
                {

                    if (MessageBox.Show(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                      ls["MessageCaption_GetImagesFromFolder"],
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
        // Search
        private void button1_Click_1(object sender, EventArgs e)
        {
            _SearchFolder = textBox_searchFolder.Text;
            _SearchName = textBox_searchFor.Text;
            _subfodlers = checkBox_subfolders.Checked;
            _matchCase = checkBox_matchCase.Checked;
            _matchWord = checkBox_matchWord.Checked;
            switch (comboBox_condition.SelectedIndex)
            {
                case 0: condition = TextSearchCondition.Contains; break;
                case 1: condition = TextSearchCondition.DoesNotContain; break;
                case 2: condition = TextSearchCondition.Is; break;
                case 3: condition = TextSearchCondition.IsNot; break;
                case 4: condition = TextSearchCondition.StartWith; break;
                case 5: condition = TextSearchCondition.DoesNotStartWith; break;
                case 6: condition = TextSearchCondition.EndWith; break;
                case 7: condition = TextSearchCondition.DoesNotEndWith; break;
            }
            label_status.Visible = true;
            label_status.Text = ls["Status_SearchingForImages"] + " ...";
            mainThread = new Thread(new ThreadStart(SearchImages));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }
        // Change folder
        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = ls["Title_SelectTheFolderYouWantToSearch"];
            folderBrowser.SelectedPath = textBox_searchFolder.Text;
            if (folderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                textBox_searchFolder.Text = folderBrowser.SelectedPath;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox_extensions.Text = original_extensions;
        }
    }
}
