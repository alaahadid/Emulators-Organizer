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
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
using System.Net;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_AssignDataFromTheGamesDB : Form
    {
        public Form_AssignDataFromTheGamesDB(string romID, int gameDBID)
        {
            InitializeComponent();
            this.romID = romID;
            this.gameDBID = gameDBID;
            rom = profileManager.Profile.Roms[romID];
            selectedConsole = profileManager.Profile.Consoles[consoleID = rom.ParentConsoleID];

            /* game = GamesDB.GetGame(gameDBID);

             listView1.Items.Clear();

             listView1.Items.Add("Title");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Title);
             listView1.Items.Add("Platform");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Platform);
             listView1.Items.Add("ReleaseDate");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.ReleaseDate);
             listView1.Items.Add("Overview");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Overview);
             listView1.Items.Add("ESRB");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.ESRB);
             listView1.Items.Add("Players");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Players);
             listView1.Items.Add("Publisher");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Publisher);
             listView1.Items.Add("Developer");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Developer);
             listView1.Items.Add("Rating");
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Rating);
             listView1.Items.Add("AlternateTitles");
             string titles = "";
             foreach (string t in game.AlternateTitles)
                 titles += t + ", ";
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(titles);
             listView1.Items.Add("Genres");
             string genres = "";
             foreach (string t in game.Genres)
                 genres += t + ", ";
             listView1.Items[listView1.Items.Count - 1].SubItems.Add(genres);*/

            List<string> folders = new List<string>();
            // Load the tabs 
            foreach (InformationContainer container in selectedConsole.InformationContainers)
            {
                if (container is InformationContainerInfoText)
                {
                    comboBox_overview_tab.Items.Add(container);
                    if (container.Name == "Overview")
                    {
                        if (((InformationContainerInfoText)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerInfoText)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_overview_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_overview_tab.SelectedItem = container;
                        checkBox_create_new_tab_overview.Checked = false;
                    }
                }
                if (container is InformationContainerImage)
                {
                    comboBox_Banners_tabs.Items.Add(container);
                    if (container.Name == "Banners")
                    {
                        comboBox_Banners_tabs.SelectedItem = container;
                        checkBox_create_tab_Banners.Checked = false;
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Banners_folder.Text = folders[folders.Count - 1];
                        }
                    }
                    comboBox_boxart_bakc_tabs.Items.Add(container);
                    if (container.Name == "Boxart Back")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_boxart_back_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_boxart_bakc_tabs.SelectedItem = container;
                        checkBox_create_new_tab_boxat_back.Checked = false;
                    }
                    comboBox_boxart_front_tab.Items.Add(container);
                    if (container.Name == "Boxart Front")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_boxart_front_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_boxart_front_tab.SelectedItem = container;
                        checkBox_create_new_tab_boxart_front.Checked = false;
                    }
                    comboBox_Fanart_tabs.Items.Add(container);
                    if (container.Name == "Fanart")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Fanart_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_Fanart_tabs.SelectedItem = container;
                        checkBox_create_tab_for_Fanart.Checked = false;
                    }
                    comboBox_Screenshots_tabs.Items.Add(container);
                    if (container.Name == "Screenshots")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Screenshots_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_Screenshots_tabs.SelectedItem = container;
                        checkBox_create_tab_Screenshots.Checked = false;
                    }
                    comboBox_clearlogo_tabs.Items.Add(container);
                    if (container.Name == "Clear Logo")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_clearlogo_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_clearlogo_tabs.SelectedItem = container;
                        checkBox_clearlogo_create_new_tab.Checked = false;
                    }
                }
            }
            if (comboBox_overview_tab.Items.Count > 0 && comboBox_overview_tab.SelectedIndex < 0)
                comboBox_overview_tab.SelectedIndex = 0;
            if (comboBox_Banners_tabs.Items.Count > 0 && comboBox_Banners_tabs.SelectedIndex < 0)
                comboBox_Banners_tabs.SelectedIndex = 0;
            if (comboBox_boxart_bakc_tabs.Items.Count > 0 && comboBox_boxart_bakc_tabs.SelectedIndex < 0)
                comboBox_boxart_bakc_tabs.SelectedIndex = 0;
            if (comboBox_boxart_front_tab.Items.Count > 0 && comboBox_boxart_front_tab.SelectedIndex < 0)
                comboBox_boxart_front_tab.SelectedIndex = 0;
            if (comboBox_Fanart_tabs.Items.Count > 0 && comboBox_Fanart_tabs.SelectedIndex < 0)
                comboBox_Fanart_tabs.SelectedIndex = 0;
            if (comboBox_Screenshots_tabs.Items.Count > 0 && comboBox_Screenshots_tabs.SelectedIndex < 0)
                comboBox_Screenshots_tabs.SelectedIndex = 0;
            if (comboBox_clearlogo_tabs.Items.Count > 0 && comboBox_clearlogo_tabs.SelectedIndex < 0)
                comboBox_clearlogo_tabs.SelectedIndex = 0;
        }
        public Form_AssignDataFromTheGamesDB(string romID, int gameDBID, bool setRenameOption, bool setAddInfoAsRomData, bool setAddOverviewAsTab)
        {
            InitializeComponent();
            this.romID = romID;
            this.gameDBID = gameDBID;
            rom = profileManager.Profile.Roms[romID];
            selectedConsole = profileManager.Profile.Consoles[consoleID = rom.ParentConsoleID];

           /* game = GamesDB.GetGame(gameDBID);

            listView1.Items.Clear();

            listView1.Items.Add("Title");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Title);
            listView1.Items.Add("Platform");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Platform);
            listView1.Items.Add("ReleaseDate");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.ReleaseDate);
            listView1.Items.Add("Overview");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Overview);
            listView1.Items.Add("ESRB");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.ESRB);
            listView1.Items.Add("Players");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Players);
            listView1.Items.Add("Publisher");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Publisher);
            listView1.Items.Add("Developer");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Developer);
            listView1.Items.Add("Rating");
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(game.Rating);
            listView1.Items.Add("AlternateTitles");
            string titles = "";
            foreach (string t in game.AlternateTitles)
                titles += t + ", ";
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(titles);
            listView1.Items.Add("Genres");
            string genres = "";
            foreach (string t in game.Genres)
                genres += t + ", ";
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(genres);*/

            List<string> folders = new List<string>();
            // Load the tabs 
            foreach (InformationContainer container in selectedConsole.InformationContainers)
            {
                if (container is InformationContainerInfoText)
                {
                    comboBox_overview_tab.Items.Add(container);
                    if (container.Name == "Overview")
                    {
                        if (((InformationContainerInfoText)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerInfoText)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_overview_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_overview_tab.SelectedItem = container;
                        checkBox_create_new_tab_overview.Checked = false;
                    }
                }
                if (container is InformationContainerImage)
                {
                    comboBox_Banners_tabs.Items.Add(container);
                    if (container.Name == "Banners")
                    {
                        comboBox_Banners_tabs.SelectedItem = container;
                        checkBox_create_tab_Banners.Checked = false;
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Banners_folder.Text = folders[folders.Count - 1];
                        }
                    }
                    comboBox_boxart_bakc_tabs.Items.Add(container);
                    if (container.Name == "Boxart Back")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_boxart_back_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_boxart_bakc_tabs.SelectedItem = container;
                        checkBox_create_new_tab_boxat_back.Checked = false;
                    }
                    comboBox_boxart_front_tab.Items.Add(container);
                    if (container.Name == "Boxart Front")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_boxart_front_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_boxart_front_tab.SelectedItem = container;
                        checkBox_create_new_tab_boxart_front.Checked = false;
                    }
                    comboBox_Fanart_tabs.Items.Add(container);
                    if (container.Name == "Fanart")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Fanart_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_Fanart_tabs.SelectedItem = container;
                        checkBox_create_tab_for_Fanart.Checked = false;
                    }
                    comboBox_Screenshots_tabs.Items.Add(container);
                    if (container.Name == "Screenshots")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_Screenshots_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_Screenshots_tabs.SelectedItem = container;
                        checkBox_create_tab_Screenshots.Checked = false;
                    }
                    comboBox_clearlogo_tabs.Items.Add(container);
                    if (container.Name == "Clear Logo")
                    {
                        if (((InformationContainerImage)container).FoldersMemory != null)
                        {
                            folders = ((InformationContainerImage)container).FoldersMemory;
                            if (folders.Count > 0)
                                textBox_clearlogo_folder.Text = folders[folders.Count - 1];
                        }
                        comboBox_clearlogo_tabs.SelectedItem = container;
                        checkBox_clearlogo_create_new_tab.Checked = false;
                    }
                }
            }
            if (comboBox_overview_tab.Items.Count > 0 && comboBox_overview_tab.SelectedIndex < 0)
                comboBox_overview_tab.SelectedIndex = 0;
            if (comboBox_Banners_tabs.Items.Count > 0 && comboBox_Banners_tabs.SelectedIndex < 0)
                comboBox_Banners_tabs.SelectedIndex = 0;
            if (comboBox_boxart_bakc_tabs.Items.Count > 0 && comboBox_boxart_bakc_tabs.SelectedIndex < 0)
                comboBox_boxart_bakc_tabs.SelectedIndex = 0;
            if (comboBox_boxart_front_tab.Items.Count > 0 && comboBox_boxart_front_tab.SelectedIndex < 0)
                comboBox_boxart_front_tab.SelectedIndex = 0;
            if (comboBox_Fanart_tabs.Items.Count > 0 && comboBox_Fanart_tabs.SelectedIndex < 0)
                comboBox_Fanart_tabs.SelectedIndex = 0;
            if (comboBox_Screenshots_tabs.Items.Count > 0 && comboBox_Screenshots_tabs.SelectedIndex < 0)
                comboBox_Screenshots_tabs.SelectedIndex = 0;
            if (comboBox_clearlogo_tabs.Items.Count > 0 && comboBox_clearlogo_tabs.SelectedIndex < 0)
                comboBox_clearlogo_tabs.SelectedIndex = 0;
        }
        //private Game game;
        private string romID;
        private Rom rom;
        private EmulatorsOrganizer.Core.Console selectedConsole;
        private string consoleID;
        private int gameDBID;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");

        // DB options
        private bool _db_add_overview_as_tab;
        private bool _db_add_banners_as_tab;
        private bool _db_add_boxart_front_as_tab;
        private bool _db_add_boxart_back_as_tab;
        private bool _db_add_fanart_as_tab;
        private bool _db_add_screenshots_as_tab;
        private bool _db_add_clearlogo_as_tab;

        private bool _db_create_new_tab_for_overview;
        private bool _db_create_new_tab_for_banners;
        private bool _db_create_new_tab_for_boxart_front;
        private bool _db_create_new_tab_for_boxart_back;
        private bool _db_create_new_tab_for_fanart;
        private bool _db_create_new_tab_for_screenshots;
        private bool _db_create_new_tab_for_clearlogo;

        private string _db_overview_folder;
        private string _db_banners_folder;
        private string _db_boxart_front_folder;
        private string _db_boxart_back_folder;
        private string _db_fanart_folder;
        private string _db_screenshots_folder;
        private string _db_clearlogo_folder;

        private string _db_overview_ic_id;
        private string _db_banners_ic_id;
        private string _db_boxart_front_ic_id;
        private string _db_boxart_back_ic_id;
        private string _db_fanart_ic_id;
        private string _db_screenshots_ic_id;
        private string _db_clearlogo_ic_id;

        private bool _db_overview_ic_clearlist;
        private bool _db_banners_ic_clearlist;
        private bool _db_boxart_front_ic_clearlist;
        private bool _db_boxart_back_ic_clearlist;
        private bool _db_fanart_ic_clearlist;
        private bool _db_screenshots_ic_clearlist;
        private bool _db_clearlogo_ic_clearlist;

        private bool _db_fanart_ic_limitdownload;
        private bool _db_screenshots_ic_limitdownload;
        private bool _db_banners_ic_limitdownload;

        private InformationContainerInfoText AddNewInfoTAB(string newTabName, string folder)
        {
            //search the profile for information container that match the same name
            bool found = false;
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.Name.ToLower() == newTabName.ToLower() && ic is InformationContainerInfoText)
                {
                    found = true; ShowTab(ic.ID);
                    return (InformationContainerInfoText)ic;
                }
            }
            if (!found)
            {
                //add new ic with this project
                InformationContainerInfoText newIC = new InformationContainerInfoText(
                    profileManager.Profile.GenerateID());
                newIC.Name = newTabName;
                newIC.DisplayName = newTabName;
                if (newIC.FoldersMemory == null)
                    newIC.FoldersMemory = new List<string>();
                if (!newIC.FoldersMemory.Contains(folder))
                    newIC.FoldersMemory.Add(folder);
                profileManager.Profile.Consoles[consoleID].InformationContainers.Add(newIC);
                ShowTab(newIC.ID);
                return newIC;
            }
            return null;
        }
        private void ShowTab(string tabID)
        {
            bool visible = selectedConsole.InformationContainersMap.IsContainerVisible(tabID);
            if (visible)
            {
                // Already visible
                return;
            }
            else
            {
                // Add it to map if not exist
                if (!selectedConsole.InformationContainersMap.AddNewContainerID(tabID))
                {
                    if (selectedConsole.InformationContainersMap.ContainerIDS == null)
                        selectedConsole.InformationContainersMap.ContainerIDS = new List<string>();
                    selectedConsole.InformationContainersMap.ContainerIDS.Add(tabID);
                }

            }
            // Event for refresh request
            profileManager.Profile.OnInformationContainerVisibiltyChanged();
        }
        private void AddNewImageTAB(string newTabName, string folder)
        {
            //search the profile for information container that match the same name
            bool found = false;
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.Name.ToLower() == newTabName.ToLower())
                {
                    found = true; ShowTab(ic.ID); break;
                }
            }
            if (!found)
            {
                //add new ic with this project
                InformationContainerImage newIC = new InformationContainerImage(
                    profileManager.Profile.GenerateID());
                newIC.Name = newTabName;
                newIC.DisplayName = newTabName;
                if (newIC.FoldersMemory == null)
                    newIC.FoldersMemory = new List<string>();
                if (!newIC.FoldersMemory.Contains(folder))
                    newIC.FoldersMemory.Add(folder);
                profileManager.Profile.Consoles[consoleID].InformationContainers.Add(newIC);
                ShowTab(newIC.ID);
            }

        }
        private void AddTabConentFilesToRom(string tabName, string downloads_folder, List<string> links, Rom rom, bool clearOldList)
        {
            if (!Directory.Exists(downloads_folder))
                Directory.CreateDirectory(downloads_folder);
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.DisplayName.ToLower() == tabName.ToLower())
                {

                    // Download the files
                    string NameOfSavedFiles = tabName + "-" + Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)))
                       + ".txt";

                    int c = 1;
                    List<string> filesToAdd = new List<string>();
                    foreach (string link in links)
                    {
                        // Try downloading
                        try
                        {
                            Uri uri = new Uri(link);
                            string[] splited = link.Split(new char[] { '/' });
                            string ext = Path.GetExtension(splited[splited.Length - 1]);
                            int j = 0;
                            while (File.Exists(Path.GetFullPath(downloads_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext)))
                                j++;
                            WebClient client = new WebClient();
                            client.DownloadFile(uri, Path.GetFullPath(downloads_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext));

                            filesToAdd.Add(downloads_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext);

                        }
                        catch (Exception ex)
                        {
                        }
                        c++;
                    }
                    if (rom.RomInfoItems != null)
                    {
                        if (!rom.IsInformationContainerItemExist(ic.ID))
                        {
                            // Create new
                            InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                            if (clearOldList)
                                item.Files = new List<string>();
                            item.Files.AddRange(filesToAdd);
                            rom.RomInfoItems.Add(item);
                            rom.Modified = true;
                        }
                        else
                        {
                            // Update
                            foreach (InformationContainerItem item in rom.RomInfoItems)
                            {
                                if (item.ParentID == ic.ID)
                                {
                                    InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                                    if (ictem.Files == null)
                                        ictem.Files = new List<string>();
                                    if (clearOldList)
                                        ictem.Files = new List<string>();
                                    foreach (string f in filesToAdd)
                                    {
                                        if (!ictem.Files.Contains(f))
                                            ictem.Files.Add(f);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        rom.RomInfoItems = new List<InformationContainerItem>();
                        // Create new
                        InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                        item.Files.AddRange(filesToAdd);
                        rom.RomInfoItems.Add(item);
                        rom.Modified = true;
                    }
                    break;
                }
            }

        }
        private void AddTabConentFilesToRom(string tabName, string folder, string file_content, Rom rom, bool clearOldList)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (InformationContainer ic in profileManager.Profile.Consoles[consoleID].InformationContainers)
            {
                if (ic.DisplayName.ToLower() == tabName.ToLower())
                {
                    if (((InformationContainerImage)ic).FoldersMemory == null)
                        ((InformationContainerImage)ic).FoldersMemory = new List<string>();
                    if (!((InformationContainerImage)ic).FoldersMemory.Contains(folder))
                        ((InformationContainerImage)ic).FoldersMemory.Add(folder);
                    // Get the files
                    string fileToAdd = tabName + "-" + Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)))
                        + ".txt";
                    fileToAdd = Path.Combine(folder, fileToAdd);
                    int i = 1;
                    while (File.Exists(fileToAdd))
                    {
                        i++;
                        fileToAdd = tabName + "-" + Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path)))
                          + "_" + i + ".txt";
                        fileToAdd = Path.Combine(folder, fileToAdd);
                    }
                    try
                    {
                        File.WriteAllText(fileToAdd, file_content);

                        if (rom.RomInfoItems != null)
                        {
                            if (!rom.IsInformationContainerItemExist(ic.ID))
                            {
                                // Create new
                                InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                                if (clearOldList)
                                    item.Files = new List<string>();
                                item.Files.Add(fileToAdd);
                                rom.RomInfoItems.Add(item);
                                rom.Modified = true;
                            }
                            else
                            {
                                // Update
                                foreach (InformationContainerItem item in rom.RomInfoItems)
                                {
                                    if (item.ParentID == ic.ID)
                                    {
                                        InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                                        if (ictem.Files == null)
                                            ictem.Files = new List<string>();
                                        if (clearOldList)
                                            ictem.Files = new List<string>();
                                        if (!ictem.Files.Contains(fileToAdd))
                                            ictem.Files.Add(fileToAdd);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            rom.RomInfoItems = new List<InformationContainerItem>();
                            // Create new
                            InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), ic.ID);
                            item.Files.Add(fileToAdd);
                            rom.RomInfoItems.Add(item);
                            rom.Modified = true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                }
            }

        }
        private void comboBox_info_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        protected virtual void AddNewIC(string newic)
        {
            //search the profile for information container that match the same name
            bool found = false;
            foreach (RomData ic in selectedConsole.RomDataInfoElements)
            {
                if (ic.Name.ToLower() == newic.ToLower())
                {
                    found = true; break;
                }
            }
            if (!found)
            {
                //add new ic with this project
                RomData newIC = new RomData(profileManager.Profile.GenerateID(), newic, RomDataType.Text);
                selectedConsole.RomDataInfoElements.Add(newIC);
            }
        }
        protected virtual void AddDataToRom(string icName, string data, Rom rom)
        {
            foreach (RomData ic in selectedConsole.RomDataInfoElements)
            {
                if (ic.Name.ToLower() == icName.ToLower())
                {
                    rom.UpdateDataInfoItemValue(ic.ID, data);
                    break;
                }
            }
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            /*// Make checks
            if (checkBox_add_Banners.Checked)
            {
                if (!Directory.Exists(textBox_Banners_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Banners' is not exist.");
                    button7_Click(null, null);
                    return;
                }
                if (!checkBox_create_tab_Banners.Checked)
                {
                    if (comboBox_Banners_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Banners first.");
                        return;
                    }
                }
            }
            if (checkBox_add_boxart_back.Checked)
            {
                if (!Directory.Exists(textBox_boxart_back_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Boxart Back' is not exist.");
                    button4_Click(null, null);
                    return;
                }
                if (!checkBox_create_new_tab_boxat_back.Checked)
                {
                    if (comboBox_boxart_bakc_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Boxart Back first.");
                        return;
                    }
                }
            }
            if (checkBox_add_boxart_front.Checked)
            {
                if (!Directory.Exists(textBox_boxart_front_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Boxart Front' is not exist.");
                    button5_Click(null, null);
                    return;
                }
                if (!checkBox_create_new_tab_boxart_front.Checked)
                {
                    if (comboBox_boxart_front_tab.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for boxart Front first.");
                        return;
                    }
                }
            }
            if (checkBox_add_Fanart.Checked)
            {
                if (!Directory.Exists(textBox_Fanart_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Fanart' is not exist.");
                    button6_Click(null, null);
                    return;
                }
                if (!checkBox_create_tab_for_Fanart.Checked)
                {
                    if (comboBox_Fanart_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Fanart first.");
                        return;
                    }
                }
            }
            if (checkBox_add_overview.Checked)
            {
                if (!Directory.Exists(textBox_overview_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Overview' is not exist.");
                    button3_Click_1(null, null);
                    return;
                }
                if (!checkBox_create_new_tab_overview.Checked)
                {
                    if (comboBox_overview_tab.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Overview first.");
                        return;
                    }
                }
            }
            if (checkBox_add_Screenshots.Checked)
            {
                if (!Directory.Exists(textBox_Screenshots_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Screenshots' is not exist.");
                    button8_Click(null, null);
                    return;
                }
                if (!checkBox_create_tab_Screenshots.Checked)
                {
                    if (comboBox_Screenshots_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Screenshots first.");
                        return;
                    }
                }
            }
            if (checkBox_clearlogo_addastab.Checked)
            {
                if (!Directory.Exists(textBox_clearlogo_folder.Text))
                {
                    ManagedMessageBox.ShowErrorMessage("The folder for 'Clear Logo' is not exist.");
                    button16_Click(null, null);
                    return;
                }
                if (!checkBox_clearlogo_create_new_tab.Checked)
                {
                    if (comboBox_clearlogo_tabs.SelectedIndex < 0)
                    {
                        ManagedMessageBox.ShowErrorMessage("Please select an Information Container (tab) to use for Clear Logo first.");
                        return;
                    }
                }
            }
            // Poke options ...
            _db_add_overview_as_tab = checkBox_add_overview.Checked;
            _db_add_banners_as_tab = checkBox_add_Banners.Checked;
            _db_add_boxart_front_as_tab = checkBox_add_boxart_front.Checked;
            _db_add_boxart_back_as_tab = checkBox_add_boxart_back.Checked;
            _db_add_fanart_as_tab = checkBox_add_Fanart.Checked;
            _db_add_screenshots_as_tab = checkBox_add_Screenshots.Checked;
            _db_add_clearlogo_as_tab = checkBox_clearlogo_addastab.Checked;

            _db_create_new_tab_for_overview = checkBox_create_new_tab_overview.Checked;
            _db_create_new_tab_for_banners = checkBox_create_tab_Banners.Checked;
            _db_create_new_tab_for_boxart_front = checkBox_create_new_tab_boxart_front.Checked;
            _db_create_new_tab_for_boxart_back = checkBox_create_new_tab_boxat_back.Checked;
            _db_create_new_tab_for_fanart = checkBox_create_tab_for_Fanart.Checked;
            _db_create_new_tab_for_screenshots = checkBox_create_tab_Screenshots.Checked;
            _db_create_new_tab_for_clearlogo = checkBox_clearlogo_create_new_tab.Checked;

            _db_overview_folder = textBox_overview_folder.Text;
            _db_banners_folder = textBox_Banners_folder.Text;
            _db_boxart_front_folder = textBox_boxart_front_folder.Text;
            _db_boxart_back_folder = textBox_boxart_back_folder.Text;
            _db_fanart_folder = textBox_Fanart_folder.Text;
            _db_screenshots_folder = textBox_Screenshots_folder.Text;
            _db_clearlogo_folder = textBox_clearlogo_folder.Text;

            if (!_db_create_new_tab_for_overview)
                _db_overview_ic_id = ((InformationContainer)comboBox_overview_tab.SelectedItem).ID;
            if (!_db_create_new_tab_for_banners)
                _db_banners_ic_id = ((InformationContainer)comboBox_Banners_tabs.SelectedItem).ID;
            if (!_db_create_new_tab_for_boxart_front)
                _db_boxart_front_ic_id = ((InformationContainer)comboBox_boxart_front_tab.SelectedItem).ID;
            if (!_db_create_new_tab_for_boxart_back)
                _db_boxart_back_ic_id = ((InformationContainer)comboBox_boxart_bakc_tabs.SelectedItem).ID;
            if (!_db_create_new_tab_for_fanart)
                _db_fanart_ic_id = ((InformationContainer)comboBox_Fanart_tabs.SelectedItem).ID;
            if (!_db_create_new_tab_for_screenshots)
                _db_screenshots_ic_id = ((InformationContainer)comboBox_Screenshots_tabs.SelectedItem).ID;
            if (!_db_create_new_tab_for_clearlogo)
                _db_clearlogo_ic_id = ((InformationContainer)comboBox_clearlogo_tabs.SelectedItem).ID;

            _db_overview_ic_clearlist = checkBox_overview_clearlist.Checked;
            _db_banners_ic_clearlist = checkBox_banners_clear_list.Checked;
            _db_boxart_front_ic_clearlist = checkBox_boxart_front_clearlist.Checked;
            _db_boxart_back_ic_clearlist = checkBox_boxart_back_clearlist.Checked;
            _db_fanart_ic_clearlist = checkBox_fanart_clearlist.Checked;
            _db_screenshots_ic_clearlist = checkBox_screenshots_clearlist.Checked;
            _db_clearlogo_ic_clearlist = checkBox_clearlogo_clear_files_first.Checked;

            _db_fanart_ic_limitdownload = checkBox_limit_download_fanart.Checked;
            _db_screenshots_ic_limitdownload = checkBox_limit_download_screenshots.Checked;
            _db_banners_ic_limitdownload = checkBox_limit_download_banners.Checked;
            // Do it !
            Form_PleaseWait frm = new Form_PleaseWait();
            frm.Show(); frm.BringToFront();
            if (checkBox_renameUsingTitle.Checked)
                profileManager.Profile.Roms[romID].Name = game.Title;
            if (checkBox_add_elemnts.Checked)
            {
                AddNewIC("Platform");
                AddDataToRom("Platform", game.Platform, profileManager.Profile.Roms[romID]);
                AddNewIC("ReleaseDate");
                AddDataToRom("ReleaseDate", game.ReleaseDate, profileManager.Profile.Roms[romID]);
                if (!checkBox_add_overview.Checked)
                {
                    AddNewIC("Overview");
                    AddDataToRom("Overview", game.Overview, profileManager.Profile.Roms[romID]);
                }
                AddNewIC("ESRB");
                AddDataToRom("ESRB", game.ESRB, profileManager.Profile.Roms[romID]);
                AddNewIC("Players");
                AddDataToRom("Players", game.Players, profileManager.Profile.Roms[romID]);
                AddNewIC("Publisher");
                AddDataToRom("Publisher", game.Publisher, profileManager.Profile.Roms[romID]);
                AddNewIC("Developer");
                AddDataToRom("Developer", game.Developer, profileManager.Profile.Roms[romID]);
                AddNewIC("Rating");
                AddDataToRom("Rating", game.Rating, profileManager.Profile.Roms[romID]);

                AddNewIC("AlternateTitles");
                string titles = "";
                foreach (string t in game.AlternateTitles)
                    titles += t + ", ";
                AddDataToRom("AlternateTitles", titles, profileManager.Profile.Roms[romID]);

                AddNewIC("Genres");
                string genres = "";
                foreach (string t in game.Genres)
                    genres += t + ", ";
                AddDataToRom("Genres", genres, profileManager.Profile.Roms[romID]);
                profileManager.Profile.OnRomPropertiesChanged(rom.Name, rom.ID, true);
            }
            #region Overview
            if (checkBox_add_overview.Checked)
            {
                // Save the overview file !
                if (!Directory.Exists(_db_overview_folder))
                    Directory.CreateDirectory(_db_overview_folder);

                int i = 1;
                string rname = rom.Name + "_" + i + ".txt";
                rname = rname.Replace(":", "");
                rname = rname.Replace("/", "-");
                string filePath = Path.Combine(_db_overview_folder, rname);
                while (File.Exists(filePath))
                {
                    i++;
                    rname = rom.Name + "_" + i + ".txt";
                    rname = rname.Replace(":", "");
                    rname = rname.Replace("/", "-");
                    filePath = Path.Combine(_db_overview_folder, rname);
                }

                RichTextBox richTextBox1 = new RichTextBox();
                richTextBox1.Text = game.Overview;
                File.WriteAllLines(filePath, richTextBox1.Lines);

                // Get the info item id
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_overview_tab.SelectedItem;
                if (_db_create_new_tab_for_overview)
                {
                    cont = AddNewInfoTAB("Overview", _db_overview_folder);
                }
                else
                {
                    cont = (InformationContainerFiles)comboBox_overview_tab.SelectedItem;
                    if (cont.FoldersMemory == null)
                        cont.FoldersMemory = new List<string>();
                    if (!cont.FoldersMemory.Contains(_db_overview_folder))
                        cont.FoldersMemory.Add(_db_overview_folder);
                }

                // Add the element to the rom
                if (!rom.IsInformationContainerItemExist(cont.ID))
                {
                    // Create new
                    InformationContainerItemFiles item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), cont.ID);
                    item.Files.Add(filePath);
                    rom.RomInfoItems.Add(item);
                    rom.Modified = true;
                }
                else
                {
                    // Update
                    foreach (InformationContainerItem item in rom.RomInfoItems)
                    {
                        if (item.ParentID == cont.ID)
                        {
                            InformationContainerItemFiles ictem = (InformationContainerItemFiles)item;
                            if (ictem.Files == null)
                                ictem.Files = new List<string>();
                            if (!ictem.Files.Contains(filePath))
                                ictem.Files.Add(filePath);
                            break;
                        }
                    }
                }
                profileManager.Profile.OnInformationContainerItemsModified(cont.Name);
            }
            #endregion
            #region Banners
            if (_db_add_banners_as_tab)
            {
                List<string> links = new List<string>();
                if (game.Images.Banners != null)
                {
                    if (game.Images.Banners.Count > 0)
                    {
                        // Download the tabs for it !
                        for (int i = 0; i < game.Images.Banners.Count; i++)
                        {
                            links.Add(GamesDB.BaseImgURL + game.Images.Banners[i].Path);
                            if (_db_banners_ic_limitdownload)
                                break;// one link added so far.
                        }
                    }
                }
                if (_db_create_new_tab_for_banners)
                {
                    // Create new tab for it !!
                    AddNewImageTAB("Banners", _db_banners_folder);

                    if (links.Count > 0)
                        AddTabConentFilesToRom("Banners", _db_banners_folder, links, rom, _db_banners_ic_clearlist);
                }
                else if (links.Count > 0)
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_banners_ic_id).DisplayName;
                    // Download the tabs for it !
                    AddTabConentFilesToRom(tabName, _db_banners_folder, links, rom, _db_banners_ic_clearlist);
                }
            }
            #endregion
            #region Screenshots
            if (_db_add_screenshots_as_tab)
            {
                List<string> links = new List<string>();
                if (game.Images.Screenshots != null)
                {
                    if (game.Images.Screenshots.Count > 0)
                    {
                        // Download the tabs for it !

                        for (int i = 0; i < game.Images.Screenshots.Count; i++)
                        {
                            links.Add(GamesDB.BaseImgURL + game.Images.Screenshots[i].Path);
                            if (_db_screenshots_ic_limitdownload)
                                break;// one link added so far.
                        }
                    }
                }
                if (_db_create_new_tab_for_screenshots)
                {
                    // Create new tab for it !!
                    AddNewImageTAB("Screenshots", _db_screenshots_folder);
                    if (links.Count > 0)
                        AddTabConentFilesToRom("Screenshots", _db_screenshots_folder, links, rom, _db_screenshots_ic_clearlist);
                }
                else if (links.Count > 0)
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_screenshots_ic_id).DisplayName;
                    // Download the tabs for it !
                    AddTabConentFilesToRom(tabName, _db_screenshots_folder, links, rom, _db_screenshots_ic_clearlist);
                }
            }
            #endregion
            #region Fanart
            if (_db_add_fanart_as_tab)
            {
                List<string> links = new List<string>();
                if (game.Images.Fanart != null)
                {
                    if (game.Images.Fanart.Count > 0)
                    {
                        // Download the tabs for it !

                        for (int i = 0; i < game.Images.Fanart.Count; i++)
                        {
                            links.Add(GamesDB.BaseImgURL + game.Images.Fanart[i].Path);
                            if (_db_fanart_ic_limitdownload)
                                break;// one link added so far.
                        }
                    }
                }
                if (_db_create_new_tab_for_fanart)
                {
                    // Create new tab for it !!
                    AddNewImageTAB("Fanart", _db_fanart_folder);
                    if (links.Count > 0)
                        AddTabConentFilesToRom("Fanart", _db_fanart_folder, links, rom, _db_fanart_ic_clearlist);
                }
                else if (links.Count > 0)
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_fanart_ic_id).DisplayName;
                    // Download the tabs for it !
                    AddTabConentFilesToRom(tabName, _db_fanart_folder, links, rom, _db_fanart_ic_clearlist);
                }
            }
            #endregion
            #region Boxart back
            if (_db_add_boxart_back_as_tab)
            {
                List<string> links = new List<string>();
                if (game.Images.BoxartBack != null)
                {
                    // Download the tabs for it !
                    links.Add(GamesDB.BaseImgURL + game.Images.BoxartBack.Path);
                }
                if (_db_create_new_tab_for_boxart_back)
                {
                    // Create new tab for it !!
                    AddNewImageTAB("Boxart Back", _db_boxart_back_folder);
                    if (links.Count > 0)
                        AddTabConentFilesToRom("Boxart Back", _db_boxart_back_folder, links, rom, _db_boxart_back_ic_clearlist);
                }
                else if (links.Count > 0)
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_boxart_back_ic_id).DisplayName;
                    // Download the tabs for it !

                    AddTabConentFilesToRom(tabName, _db_boxart_back_folder, links, rom, _db_boxart_back_ic_clearlist);
                }
            }
            #endregion
            #region Boxart Front
            if (_db_add_boxart_front_as_tab)
            {
                List<string> links = new List<string>();
                if (game.Images.BoxartFront != null)
                {
                    // Download the tabs for it !

                    links.Add(GamesDB.BaseImgURL + game.Images.BoxartFront.Path);
                }
                if (_db_create_new_tab_for_boxart_front)
                {
                    // Create new tab for it !!
                    AddNewImageTAB("Boxart Front", _db_boxart_front_folder);
                    if (links.Count > 0)
                        AddTabConentFilesToRom("Boxart Front", _db_boxart_front_folder, links, rom, _db_boxart_front_ic_clearlist);
                }
                else if (links.Count > 0)
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_boxart_front_ic_id).DisplayName;
                    // Download the tabs for it !
                    AddTabConentFilesToRom(tabName, _db_boxart_front_folder, links, rom, _db_boxart_front_ic_clearlist);
                }
            }
            #endregion
            #region Clear Logo
            if (_db_add_clearlogo_as_tab)
            {
                List<string> links = new List<string>();
                if (game.Images.ClearLogo != null)
                {
                    // Download the tabs for it !
                    links.Add(GamesDB.BaseImgURL + game.Images.ClearLogo.Path);
                }
                if (_db_create_new_tab_for_clearlogo)
                {
                    // Create new tab for it !!
                    AddNewImageTAB("Clear Logo", _db_clearlogo_folder);
                    if (links.Count > 0)
                        AddTabConentFilesToRom("Clear Logo", _db_clearlogo_folder, links, rom, _db_clearlogo_ic_clearlist);
                }
                else if (links.Count > 0)
                {
                    // Assign to a tab !
                    string tabName = profileManager.Profile.Consoles[consoleID].GetInformationContainer(_db_clearlogo_ic_id).DisplayName;
                    // Download the tabs for it !

                    AddTabConentFilesToRom(tabName, _db_clearlogo_folder, links, rom, _db_clearlogo_ic_clearlist);
                }
            }
            #endregion
            if (comboBox_overview_tab.SelectedIndex > 0)
            {
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_overview_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (!cont.FoldersMemory.Contains(_db_overview_folder))
                    cont.FoldersMemory.Add(_db_overview_folder);
            }
            if (comboBox_Banners_tabs.SelectedIndex > 0)
            {
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_Banners_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (!cont.FoldersMemory.Contains(_db_banners_folder))
                    cont.FoldersMemory.Add(_db_banners_folder);
            }
            if (comboBox_boxart_bakc_tabs.SelectedIndex > 0)
            {
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_boxart_bakc_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (!cont.FoldersMemory.Contains(_db_boxart_back_folder))
                    cont.FoldersMemory.Add(_db_boxart_back_folder);
            }
            if (comboBox_boxart_front_tab.SelectedIndex > 0)
            {
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_boxart_front_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (!cont.FoldersMemory.Contains(_db_boxart_front_folder))
                    cont.FoldersMemory.Add(_db_boxart_front_folder);
            }
            if (comboBox_Fanart_tabs.SelectedIndex > 0)
            {
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_Fanart_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (!cont.FoldersMemory.Contains(_db_fanart_folder))
                    cont.FoldersMemory.Add(_db_fanart_folder);
            }
            if (comboBox_Screenshots_tabs.SelectedIndex > 0)
            {
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_Screenshots_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (!cont.FoldersMemory.Contains(_db_screenshots_folder))
                    cont.FoldersMemory.Add(_db_screenshots_folder);
            }
            if (comboBox_clearlogo_tabs.SelectedIndex > 0)
            {
                InformationContainerFiles cont = (InformationContainerFiles)comboBox_clearlogo_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (!cont.FoldersMemory.Contains(_db_clearlogo_folder))
                    cont.FoldersMemory.Add(_db_clearlogo_folder);
            }
            this.DialogResult = DialogResult.OK;
            Close();
            frm.Close();*/
        }
        private void button_set_master_folder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.Description = "Please select a master folder where to save the downloaded files (empty folder is recommended)";

            if (selectedConsole.Memory_RomFolders != null)
            {
                if (selectedConsole.Memory_RomFolders.Count > 0)
                {
                    fol.SelectedPath = selectedConsole.Memory_RomFolders[selectedConsole.Memory_RomFolders.Count - 1];
                }
            }
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage("These folders will be created :"
                   + "\n" + fol.SelectedPath + "\\Overview\\"
                    + "\n" + fol.SelectedPath + "\\Banners\\"
                    + "\n" + fol.SelectedPath + "\\BoxartBack\\"
                    + "\n" + fol.SelectedPath + "\\BoxartFront\\"
                    + "\n" + fol.SelectedPath + "\\Fanart\\"
                    + "\n" + fol.SelectedPath + "\\Screenshots\\"
                    + "\n" + fol.SelectedPath + "\\Clear Logo\\"
                    + "\n" + "\nConinue ?");
                if (res.ClickedButtonIndex == 0)
                {
                    textBox_overview_folder.Text = fol.SelectedPath + "\\Overview\\";
                    textBox_Banners_folder.Text = fol.SelectedPath + "\\Banners\\";
                    textBox_boxart_back_folder.Text = fol.SelectedPath + "\\BoxartBack\\";
                    textBox_boxart_front_folder.Text = fol.SelectedPath + "\\BoxartFront\\";
                    textBox_Fanart_folder.Text = fol.SelectedPath + "\\Fanart\\";
                    textBox_Screenshots_folder.Text = fol.SelectedPath + "\\Screenshots\\";
                    textBox_clearlogo_folder.Text = fol.SelectedPath + "\\Clear Logo\\";
                    Directory.CreateDirectory(textBox_overview_folder.Text);
                    Directory.CreateDirectory(textBox_Banners_folder.Text);
                    Directory.CreateDirectory(textBox_boxart_back_folder.Text);
                    Directory.CreateDirectory(textBox_boxart_front_folder.Text);
                    Directory.CreateDirectory(textBox_Fanart_folder.Text);
                    Directory.CreateDirectory(textBox_Screenshots_folder.Text);
                    Directory.CreateDirectory(textBox_clearlogo_folder.Text);
                }
            }
        }
        private void button_set_all_Click(object sender, EventArgs e)
        {
            checkBox_add_Banners.Checked =
      checkBox_add_boxart_back.Checked =
      checkBox_add_boxart_front.Checked =
      //checkBox_add_datainfor_elemnts.Checked =
      checkBox_add_Fanart.Checked =
      checkBox_add_overview.Checked =
      checkBox_add_Screenshots.Checked =
      checkBox_clearlogo_addastab.Checked =

      checkBox_create_new_tab_boxart_front.Checked =
      checkBox_create_new_tab_boxat_back.Checked =
      checkBox_create_new_tab_overview.Checked =
      checkBox_create_tab_Banners.Checked =
      checkBox_create_tab_for_Fanart.Checked =
      checkBox_create_tab_Screenshots.Checked =
      checkBox_clearlogo_create_new_tab.Checked =
      true;
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_overview_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_overview_folder.Text = fol.SelectedPath;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_boxart_back_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_boxart_back_folder.Text = fol.SelectedPath;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_boxart_front_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_boxart_front_folder.Text = fol.SelectedPath;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_Fanart_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Fanart_folder.Text = fol.SelectedPath;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_Banners_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Banners_folder.Text = fol.SelectedPath;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_Screenshots_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Screenshots_folder.Text = fol.SelectedPath;
            }
        }
        private void comboBox_overview_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_overview_tab.SelectedIndex > 0 && textBox_overview_folder.Text == "")
            {
                InformationContainerInfoText cont = (InformationContainerInfoText)comboBox_overview_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_overview_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void comboBox_boxart_bakc_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_boxart_bakc_tabs.SelectedIndex > 0 && textBox_boxart_back_folder.Text == "")
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_boxart_bakc_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_boxart_back_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void comboBox_boxart_front_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_boxart_front_tab.SelectedIndex > 0 && textBox_boxart_front_folder.Text == "")
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_boxart_front_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_boxart_front_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void comboBox_Fanart_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Fanart_tabs.SelectedIndex > 0 && textBox_Fanart_folder.Text == "")
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Fanart_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Fanart_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void comboBox_Banners_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Banners_tabs.SelectedIndex > 0 && textBox_Banners_folder.Text == "")
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Banners_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Banners_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void comboBox_Screenshots_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Screenshots_tabs.SelectedIndex > 0 && textBox_Screenshots_folder.Text == "")
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Screenshots_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Screenshots_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox_overview_tab.SelectedIndex > 0)
            {
                InformationContainerInfoText cont = (InformationContainerInfoText)comboBox_overview_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_overview_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox_boxart_bakc_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_boxart_bakc_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_boxart_back_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (comboBox_boxart_front_tab.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_boxart_front_tab.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_boxart_front_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox_Fanart_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Fanart_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Fanart_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox_Banners_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Banners_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Banners_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (comboBox_Screenshots_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_Screenshots_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_Screenshots_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_clearlogo_folder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_clearlogo_folder.Text = fol.SelectedPath;
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            if (comboBox_clearlogo_tabs.SelectedIndex > 0)
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_clearlogo_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_clearlogo_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }

        private void comboBox_clearlogo_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_clearlogo_tabs.SelectedIndex > 0 && textBox_clearlogo_folder.Text == "")
            {
                InformationContainerImage cont = (InformationContainerImage)comboBox_clearlogo_tabs.SelectedItem;
                if (cont.FoldersMemory == null)
                    cont.FoldersMemory = new List<string>();
                if (cont.FoldersMemory.Count > 0)
                {
                    textBox_clearlogo_folder.Text = cont.FoldersMemory[cont.FoldersMemory.Count - 1];
                }
            }
        }
    }
}
