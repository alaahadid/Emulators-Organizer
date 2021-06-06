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
using MMB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_MobyGames : Form
    {
        private MobyGamesGame[] games;
        public string i_Name;
        public string i_PublishedBy;
        public string i_DevelopedBy;
        public string i_Released;
        public string i_Platform;
        public string i_Genre;
        public string i_Description;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager pr = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private string _rom_name;
        public Form_MobyGames(string romName)
        {
            InitializeComponent();
            _rom_name = textBox1.Text = romName;
            Search();

            Core.Console selectedConsole = pr.Profile.Consoles[pr.Profile.SelectedConsoleID];
            bool CreateNew = true;
            if (selectedConsole != null)
            {
                foreach (InformationContainer cont in selectedConsole.InformationContainers)
                {
                    if (cont is InformationContainerInfoText)
                    {
                        comboBox_infoTabs.Items.Add(cont);
                        if (cont.DisplayName == "Description")
                            CreateNew = false;

                        if (((InformationContainerInfoText)cont).FoldersMemory != null)
                        {
                            List<string> folders = ((InformationContainerInfoText)cont).FoldersMemory;

                            if (folders.Count > 0)
                            {
                                string filePath = Path.Combine(folders[folders.Count - 1], romName) + ".txt";
                                int i = 1;
                                /*while (File.Exists(filePath))
                                {
                                    i++;
                                    filePath = Path.Combine(folders[folders.Count - 1], romName + "_" + i) + ".txt";
                                }*/
                                textBox_file.Text = filePath;
                            }
                        }
                    }
                }
                // The file isn't set yet :::
                if (textBox_file.Text.Length == 0)
                {
                    if (selectedConsole.Memory_RomFolders != null)
                    {
                        if (selectedConsole.Memory_RomFolders.Count > 0)
                        {
                            string filePath = Path.Combine(selectedConsole.Memory_RomFolders[selectedConsole.Memory_RomFolders.Count - 1], romName) + ".txt";
                            int i = 1;
                            /*while (File.Exists(filePath))
                            {
                                i++;
                                filePath = Path.Combine(selectedConsole.Memory_RomFolders[selectedConsole.Memory_RomFolders.Count - 1], romName + "_" + i) + ".txt";
                            }*/
                            textBox_file.Text = filePath;
                        }
                    }
                }
                if (textBox_file.Text.Length == 0)
                {
                    // Reached here means the file isn't set yet, set the rom name...
                    textBox_file.Text = romName + ".txt";
                }
            }

            if (comboBox_infoTabs.Items.Count == 0)
            {
                comboBox_infoTabs.Items.Add("Descripion (Create New)");
            }
            else
            {
                if (CreateNew)
                    comboBox_infoTabs.Items.Add("Descripion (Create New)");
            }
            comboBox_infoTabs.SelectedIndex = 0;
        }
        void Search()
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show(ls["Message_PleaseEnterRomNameToSearch"]);
                return;
            }
            listView1.Items.Clear();
            games = MobyGamesSearcher.GetLinks(textBox1.Text);
            foreach (MobyGamesGame game in games)
            {
                listView1.Items.Add(game.Link.Replace("http://www.mobygames.com/game", ""));
            }
        }
        public string RTF
        {
            get
            {
                return richTextBox1.Rtf;
            }
        }
        public bool AddDescriptionAsTabInfo
        { get { return checkBox1.Checked; } }
        public string InfoTabID
        {
            get
            {
                if (comboBox_infoTabs.SelectedItem.ToString() == "Descripion (Create New)")
                    return "Descripion (Create New)";
                else
                    return ((InformationContainerInfoText)comboBox_infoTabs.SelectedItem).ID;
            }
        }
        public string InfoTabFilePath { get { return textBox_file.Text; } }
        public bool IncludeCategories { get { return checkBox_add_as_categories.Checked; } }

        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = richTextBox1.Text.Length > 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked && textBox_file.Text == "")
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseEnterTheFilePathFirstWhereToSaveTheDescriptionFile"]);
                return;
            }

            if (File.Exists(textBox_file.Text))
            {
                if (!checkBox_replace.Checked)
                {
                    int i = 1;
                    string filename = Path.GetFileNameWithoutExtension(textBox_file.Text);
                    string folder = Path.GetDirectoryName(textBox_file.Text);
                    string filePath = textBox_file.Text;
                    while (File.Exists(filePath))
                    {
                        i++;
                        filePath = Path.Combine(folder, filename + "_" + i + ".txt");
                    }
                    textBox_file.Text = filePath;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                richTextBox1.Clear();
                richTextBox1.SelectionStart = 0;
                //insert picture
                WebClient cl = new WebClient();
                string cover = MobyGamesSearcher.FetchCoverLink(games[listView1.SelectedItems[0].Index].Link);
                if (cover != "")
                {
                    try
                    {
                        byte[] data = cl.DownloadData(cover);
                        Stream str = new MemoryStream(data);
                        Clipboard.SetImage(Image.FromStream(new MemoryStream(data)));
                        str.Dispose();
                        str.Close();
                        richTextBox1.Paste();
                    }
                    catch { }
                }
                //get game name
                richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                string[] textLines = MobyGamesSearcher.FetchInfo(games[listView1.SelectedItems[0].Index].Link).Split(new char[] { '\n' });
                richTextBox1.SelectedText = "\n" + textLines[0] + "\n";//name
                i_Name = textLines[0];
                for (int i = 1; i < textLines.Length; i++)
                {
                    if (textLines[i].Length == 0 && textLines[i] == "  ")
                        continue;
                    if (textLines[i].Contains("Published by"))
                    {
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                        richTextBox1.SelectedText = "Published by\n";
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                        richTextBox1.SelectedText = textLines[i].Replace("Published by", "") + "\n";
                        i_PublishedBy = textLines[i].Replace("Published by", "");
                    }
                    else if (textLines[i].Contains("Developed by"))
                    {
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                        richTextBox1.SelectedText = "Developed by\n";
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                        richTextBox1.SelectedText = textLines[i].Replace("Developed by", "") + "\n";
                        i_DevelopedBy = textLines[i].Replace("Developed by", "");
                    }
                    else if (textLines[i].Contains("Released"))
                    {
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                        richTextBox1.SelectedText = "Released\n";
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                        richTextBox1.SelectedText = textLines[i].Replace("Released", "") + "\n";
                        i_Released = textLines[i].Replace("Released", "");
                    }
                    else if (textLines[i].Contains("Platform"))
                    {
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                        richTextBox1.SelectedText = "Platform\n";
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                        string platforms = "";
                        while (textLines[i] != "")
                        {
                            if (textLines[i] != "Platform")
                                platforms += textLines[i] + ", ";
                            i++;
                        }
                        i_Platform = platforms;
                        richTextBox1.SelectedText = platforms + "\n";
                    }
                    else if (textLines[i].Contains("Genre"))
                    {
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                        richTextBox1.SelectedText = "Genre\n";
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                        string genres = "";
                        while (textLines[i] != "")
                        {
                            if (textLines[i] != "Genre")
                                genres += textLines[i] + ", ";
                            i++;
                        }
                        i_Genre = genres;
                        richTextBox1.SelectedText = genres + "\n";
                    }
                    else if (textLines[i].Contains("Description"))
                    {
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                        richTextBox1.SelectedText = "Description\n";
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                        string desc = "";
                        while (textLines[i] != "")
                        {
                            if (textLines[i] != "Description")
                                desc += textLines[i] + "\n";
                            i++;
                        }
                        i_Description = richTextBox1.SelectedText = desc;
                    }
                    else
                    {
                        richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                        richTextBox1.SelectedText = textLines[i] + "\n";
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (comboBox_infoTabs.SelectedIndex < 0)
                return;
            if (comboBox_infoTabs.SelectedItem is InformationContainerInfoText)
            {
                InformationContainerInfoText cont = (InformationContainerInfoText)comboBox_infoTabs.SelectedItem;
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = string.Format(ls["Title_NewFile"] + " '{0}'", cont.DisplayName);
                sav.Filter = cont.GetExtensionDialogFilter();
                sav.FileName = textBox_file.Text;
                if (sav.ShowDialog(this) == DialogResult.OK)
                {
                    textBox_file.Text = sav.FileName;
                }
            }
            else
            {
                InformationContainerInfoText cont = new InformationContainerInfoText("");
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = string.Format(ls["Title_NewFile"] + " '{0}'", "Description");
                sav.Filter = cont.GetExtensionDialogFilter();
                sav.FileName = textBox_file.Text;
                if (sav.ShowDialog(this) == DialogResult.OK)
                {
                    textBox_file.Text = sav.FileName;
                }
            }
        }
    }
}