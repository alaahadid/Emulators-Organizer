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
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_CloneSettings : Form
    {
        public Form_CloneSettings(SettingsCopyMode mode)
        {
            InitializeComponent();
            this.mode = mode;
            // Depending on mode:
            switch (mode)
            {
                case SettingsCopyMode.CONSOLE:
                    {
                        // Load all consoles, except the selected one.
                        foreach (Core.Console con in profileManager.Profile.Consoles)
                        {
                            if (con.ID != profileManager.Profile.SelectedConsoleID)
                            {
                                comboBox_copy_from.Items.Add(con.Name);
                                ids.Add(con.ID);
                            }
                        }
                        label_copy_from.Text = "Copy from this console:";
                        label_copy_to.Text = "To this console:";
                        button_copy_console_only.Enabled = true;
                        textBox_copy_to.Text = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Name;
                        // All settings are enabled, nothing to do
                        break;
                    }
                case SettingsCopyMode.CONSOLES_GROUP:
                    {
                        // Load all consoles, except the selected one.
                        foreach (ConsolesGroup con in profileManager.Profile.ConsoleGroups)
                        {
                            if (con.ID != profileManager.Profile.SelectedConsolesGroupID)
                            {
                                comboBox_copy_from.Items.Add(con.Name);
                                ids.Add(con.ID);
                            }
                        }

                        label_copy_from.Text = "Copy from this consoles group:";
                        label_copy_to.Text = "To this consoles group:";
                        textBox_copy_to.Text = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Name;
                        // We need to disable some settings first
                        checkBox_copy_archive_settings.Enabled = false;
                        checkBox_copy_console_extensions.Enabled = false;
                        checkBox_copy_copy_rom_settings.Enabled = false;
                        checkBox_copy_filters.Enabled = false;
                        checkBox_copy_ics.Enabled = false;
                        checkBox_copy_launch_options.Enabled = false;
                        checkBox_copy_rchive_extensions.Enabled = false;
                        checkBox_copy_rom_data_types.Enabled = false;
                        checkBox_copy_tab_priority_settings.Enabled = false;
                        checkBox__copy_win_size_and_splitters.Enabled = false;
                        checkBox_clear_ics_first.Enabled = false;
                        checkBox_clear_rom_data_first.Enabled = false;
                        checkBox_copy_ics_map.Enabled = false;
                        break;
                    }
                case SettingsCopyMode.PLAYLIST:
                    {
                        // Load all consoles, except the selected one.
                        foreach (Playlist pl in profileManager.Profile.Playlists)
                        {
                            if (pl.ID != profileManager.Profile.SelectedPlaylistID)
                            {
                                comboBox_copy_from.Items.Add(pl.Name);
                                ids.Add(pl.ID);
                            }
                        }

                        label_copy_from.Text = "Copy from this playlist:";
                        label_copy_to.Text = "To this playlist:";
                        textBox_copy_to.Text = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Name;

                        // We need to disable some settings first
                        checkBox_copy_archive_settings.Enabled = false;
                        checkBox_copy_console_extensions.Enabled = false;
                        checkBox_copy_copy_rom_settings.Enabled = false;
                        checkBox_copy_filters.Enabled = false;
                        checkBox_copy_ics.Enabled = false;
                        checkBox_copy_launch_options.Enabled = false;
                        checkBox_copy_rchive_extensions.Enabled = false;
                        checkBox_copy_rom_data_types.Enabled = false;
                        checkBox_copy_tab_priority_settings.Enabled = false;
                        checkBox__copy_win_size_and_splitters.Enabled = false;
                        checkBox_clear_ics_first.Enabled = false;
                        checkBox_clear_rom_data_first.Enabled = false;
                        checkBox_copy_ics_map.Enabled = false;
                        break;
                    }
                case SettingsCopyMode.PLAYLISTS_GROUP:
                    {
                        // Load all consoles, except the selected one.
                        foreach (PlaylistsGroup pl in profileManager.Profile.PlaylistGroups)
                        {
                            if (pl.ID != profileManager.Profile.SelectedPlaylistsGroupID)
                            {
                                comboBox_copy_from.Items.Add(pl.Name);
                                ids.Add(pl.ID);
                            }
                        }

                        label_copy_from.Text = "Copy from this playlists group:";
                        label_copy_to.Text = "To this playlists group:";
                        textBox_copy_to.Text = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Name;

                        // We need to disable some settings first
                        checkBox_copy_archive_settings.Enabled = false;
                        checkBox_copy_console_extensions.Enabled = false;
                        checkBox_copy_copy_rom_settings.Enabled = false;
                        checkBox_copy_filters.Enabled = false;
                        checkBox_copy_ics.Enabled = false;
                        checkBox_copy_launch_options.Enabled = false;
                        checkBox_copy_rchive_extensions.Enabled = false;
                        checkBox_copy_rom_data_types.Enabled = false;
                        checkBox_copy_tab_priority_settings.Enabled = false;
                        checkBox__copy_win_size_and_splitters.Enabled = false;
                        checkBox_clear_ics_first.Enabled = false;
                        checkBox_clear_rom_data_first.Enabled = false;
                        checkBox_copy_ics_map.Enabled = false;
                        break;
                    }
            }
            if (comboBox_copy_from.Items.Count > 0)
                comboBox_copy_from.SelectedIndex = 0;
        }
        private SettingsCopyMode mode;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private List<string> ids = new List<string>();
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox_copy_from.SelectedIndex < 0)
            {
                ManagedMessageBox.ShowErrorMessage("Please select the source (where to copy settings from) first !");
                return;
            }
            switch (mode)
            {
                case SettingsCopyMode.CONSOLE:
                    {
                        // 1 Get the target console (the console to copy settings into)
                        Core.Console target_console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        // 2 Get the source console (the console to get settings from)
                        Core.Console source_console = profileManager.Profile.Consoles[ids[comboBox_copy_from.SelectedIndex]];
                        #region Visual and style
                        if (checkBox__copy_win_size_and_splitters.Checked)
                        {
                            target_console.Style.mainWindowHideLeftPanel = source_console.Style.mainWindowHideLeftPanel;
                            target_console.Style.mainWindowResize = source_console.Style.mainWindowResize;
                            target_console.Style.mainWindowSize = source_console.Style.mainWindowSize;
                            target_console.Style.mainWindowSplitContainer_left = source_console.Style.mainWindowSplitContainer_left;
                            target_console.Style.mainWindowSplitContainer_left_down = source_console.Style.mainWindowSplitContainer_left_down;
                            target_console.Style.mainWindowSplitContainer_main = source_console.Style.mainWindowSplitContainer_main;
                            target_console.Style.mainWindowSplitContainer_right = source_console.Style.mainWindowSplitContainer_right;
                        }
                        if (checkBox_copy_icon.Checked)
                        {
                            target_console.Icon = source_console.Icon;
                            target_console.IconThumbnail = source_console.IconThumbnail;
                        }
                        if (checkBox_copy_style.Checked)
                        {
                            target_console.Style.bkgColor_MainWindow = source_console.Style.bkgColor_MainWindow;
                            target_console.Style.bkgColor_CategoriesBrowser = source_console.Style.bkgColor_CategoriesBrowser;
                            target_console.Style.bkgColor_ConsolesBrowser = source_console.Style.bkgColor_ConsolesBrowser;
                            target_console.Style.bkgColor_EmulatorsBrowser = source_console.Style.bkgColor_EmulatorsBrowser;
                            target_console.Style.bkgColor_FiltersBrowser = source_console.Style.bkgColor_FiltersBrowser;
                            target_console.Style.bkgColor_InformationContainerBrowser = source_console.Style.bkgColor_InformationContainerBrowser;
                            target_console.Style.bkgColor_InformationContainerTabs = source_console.Style.bkgColor_InformationContainerTabs;
                            target_console.Style.bkgColor_PlaylistsBrowser = source_console.Style.bkgColor_PlaylistsBrowser;
                            target_console.Style.bkgColor_RomsBrowser = source_console.Style.bkgColor_RomsBrowser;
                            target_console.Style.bkgColor_StartOptions = source_console.Style.bkgColor_StartOptions;
                            target_console.Style.txtColor_MainWindowMainMenu = source_console.Style.txtColor_MainWindowMainMenu;
                            target_console.Style.txtColor_CategoriesBrowser = source_console.Style.txtColor_CategoriesBrowser;
                            target_console.Style.txtColor_ConsolesBrowser = source_console.Style.txtColor_ConsolesBrowser;
                            target_console.Style.txtColor_EmulatorsBrowser = source_console.Style.txtColor_EmulatorsBrowser;
                            target_console.Style.txtColor_FiltersBrowser = source_console.Style.txtColor_FiltersBrowser;
                            target_console.Style.txtColor_InformationContainerBrowser = source_console.Style.txtColor_InformationContainerBrowser;
                            target_console.Style.txtColor_InformationContainerTabs = source_console.Style.txtColor_InformationContainerTabs;
                            target_console.Style.txtColor_PlaylistsBrowser = source_console.Style.txtColor_PlaylistsBrowser;
                            target_console.Style.txtColor_RomsBrowser = source_console.Style.txtColor_RomsBrowser;
                            target_console.Style.txtColor_StartOptions = source_console.Style.txtColor_StartOptions;
                            target_console.Style.image_CategoriesBrowser = source_console.Style.image_CategoriesBrowser;
                            target_console.Style.image_ConsolesBrowser = source_console.Style.image_ConsolesBrowser;
                            target_console.Style.image_EmulatorsBrowser = source_console.Style.image_EmulatorsBrowser;
                            target_console.Style.image_FiltersBrowser = source_console.Style.image_FiltersBrowser;
                            target_console.Style.image_InformationContainerBrowser = source_console.Style.image_InformationContainerBrowser;
                            target_console.Style.image_InformationContainerTabs = source_console.Style.image_InformationContainerTabs;
                            target_console.Style.image_PlaylistsBrowser = source_console.Style.image_PlaylistsBrowser;
                            target_console.Style.image_RomsBrowser = source_console.Style.image_RomsBrowser;
                            target_console.Style.image_StartOptions = source_console.Style.image_StartOptions;

                            target_console.Style.imageMode_CategoriesBrowser = source_console.Style.imageMode_CategoriesBrowser;
                            target_console.Style.imageMode_ConsolesBrowser = source_console.Style.imageMode_ConsolesBrowser;
                            target_console.Style.imageMode_EmulatorsBrowser = source_console.Style.imageMode_EmulatorsBrowser;
                            target_console.Style.imageMode_FiltersBrowser = source_console.Style.imageMode_FiltersBrowser;
                            target_console.Style.imageMode_InformationContainerBrowser = source_console.Style.imageMode_InformationContainerBrowser;
                            target_console.Style.imageMode_InformationContainerTabs = source_console.Style.imageMode_InformationContainerTabs;
                            target_console.Style.imageMode_PlaylistsBrowser = source_console.Style.imageMode_PlaylistsBrowser;
                            target_console.Style.imageMode_RomsBrowser = source_console.Style.imageMode_RomsBrowser;
                            target_console.Style.imageMode_StartOptions = source_console.Style.imageMode_StartOptions;

                            target_console.Style.font_CategoriesBrowser = source_console.Style.font_CategoriesBrowser;
                            target_console.Style.font_ConsolesBrowser = source_console.Style.font_ConsolesBrowser;
                            target_console.Style.font_EmulatorsBrowser = source_console.Style.font_EmulatorsBrowser;
                            target_console.Style.font_FiltersBrowser = source_console.Style.font_FiltersBrowser;
                            target_console.Style.font_InformationContainerBrowser = source_console.Style.font_InformationContainerBrowser;
                            target_console.Style.font_InformationContainerTabs = source_console.Style.font_InformationContainerTabs;
                            target_console.Style.font_PlaylistsBrowser = source_console.Style.font_PlaylistsBrowser;
                            target_console.Style.font_RomsBrowser = source_console.Style.font_RomsBrowser;
                            target_console.Style.font_StartOptions = source_console.Style.font_StartOptions;

                            target_console.Style.TabPageColor = source_console.Style.TabPageColor;
                            target_console.Style.TabPageHighlightedColor = source_console.Style.TabPageHighlightedColor;
                            target_console.Style.TabPageSelectedColor = source_console.Style.TabPageSelectedColor;
                            target_console.Style.TabPageSplitColor = source_console.Style.TabPageSplitColor;
                            target_console.Style.TabPageTextsColor = source_console.Style.TabPageTextsColor;
                            target_console.Style.listviewColumnClickColor = source_console.Style.listviewColumnClickColor;
                            target_console.Style.listviewColumnColor = source_console.Style.listviewColumnColor;
                            target_console.Style.listviewColumnHighlightColor = source_console.Style.listviewColumnHighlightColor;
                            target_console.Style.listviewColumnTextColor = source_console.Style.listviewColumnTextColor;
                            target_console.Style.listviewDrawHighlight = source_console.Style.listviewDrawHighlight;
                            target_console.Style.listviewHighlightColor = source_console.Style.listviewHighlightColor;
                            target_console.Style.listviewMouseOverColor = source_console.Style.listviewMouseOverColor;
                            target_console.Style.listviewSpecialColor = source_console.Style.listviewSpecialColor;
                            target_console.Style.listviewTextsColor = source_console.Style.listviewTextsColor;
                        }
                        #endregion
                        #region Archive
                        if (checkBox_copy_archive_settings.Checked)
                        {
                            target_console.ArchiveAllowedExtractionExtensions = source_console.ArchiveAllowedExtractionExtensions;
                            target_console.ExtractAllFilesOfArchive = source_console.ExtractAllFilesOfArchive;
                            target_console.ExtractFirstFileIfArchiveIncludeMoreThanOne = source_console.ExtractFirstFileIfArchiveIncludeMoreThanOne;
                            target_console.ExtractRomIfArchive = source_console.ExtractRomIfArchive;
                        }
                        if (checkBox_copy_rchive_extensions.Checked)
                        {
                            target_console.ArchiveExtensions = new List<string>(source_console.ArchiveExtensions);
                        }
                        #endregion
                        #region Rom copying
                        if (checkBox_copy_copy_rom_settings.Checked)
                        {
                            target_console.CopyRomBeforeLaunch = source_console.CopyRomBeforeLaunch;
                            target_console.FolderWhereToCopyRomWhenLaunch = source_console.FolderWhereToCopyRomWhenLaunch;
                        }
                        #endregion
                        #region Console
                        if (checkBox_copy_console_extensions.Checked)
                            target_console.Extensions = new List<string>(source_console.Extensions);

                        if (checkBox_copy_ics.Checked)
                        {
                            // We need to do some things before we can copy the ics
                            if (checkBox_copy_ics_map.Checked)
                            {
                                target_console.InformationContainersMap = source_console.InformationContainersMap.Clone();
                            }
                            if (checkBox_clear_ics_first.Checked)
                            {
                                // Clearing may take a long time depending on roms count.
                                for (int i = 0; i < target_console.InformationContainers.Count; i++)
                                {
                                    target_console.DeleteInformationContainer(target_console.InformationContainers[i].ID);
                                    i--;
                                }
                            }
                            // Now do the copying
                            bool dontShowConflictWindow = false;
                            bool replaceSettings = false;
                            foreach (InformationContainer ic in source_console.InformationContainers)
                            {
                                if (ic is InformationContainerImage)
                                {
                                    if (!target_console.ContainsICImages(ic.DisplayName))
                                    {
                                        InformationContainerImage newIC = new InformationContainerImage(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;
                                        newIC.Extenstions = new List<string>(((InformationContainerImage)ic).Extenstions);
                                        newIC.FoldersMemory = new List<string>(((InformationContainerImage)ic).FoldersMemory);
                                        newIC.GoogleImageSearchFolder = ((InformationContainerImage)ic).GoogleImageSearchFolder;
                                        newIC.PreferedImageMode = ((InformationContainerImage)ic).PreferedImageMode;
                                        newIC.ShowStatusBar = ((InformationContainerImage)ic).ShowStatusBar;
                                        newIC.ShowToolBar = ((InformationContainerImage)ic).ShowToolBar;
                                        newIC.UseNearestNighborDraw = ((InformationContainerImage)ic).UseNearestNighborDraw;

                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        if (!dontShowConflictWindow)
                                        {
                                            FormICCopyConflict ff = new FormICCopyConflict(ic.Name + " from console " + target_console.Name + " => " + ic.Name + " of console " + source_console.Name);
                                            ff.ShowDialog();
                                            dontShowConflictWindow = ff.DoTheSameForOthers;
                                            replaceSettings = ff.CopySettings;
                                        }
                                        if (replaceSettings)
                                        {
                                            InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                            if (target_ic != null)
                                            {
                                                target_ic.Name = ic.Name;
                                                target_ic.DisplayName = ic.DisplayName;
                                                ((InformationContainerImage)target_ic).Extenstions = new List<string>(((InformationContainerImage)ic).Extenstions);
                                                ((InformationContainerImage)target_ic).FoldersMemory = new List<string>(((InformationContainerImage)ic).FoldersMemory);
                                                ((InformationContainerImage)target_ic).GoogleImageSearchFolder = ((InformationContainerImage)ic).GoogleImageSearchFolder;
                                                ((InformationContainerImage)target_ic).PreferedImageMode = ((InformationContainerImage)ic).PreferedImageMode;
                                                ((InformationContainerImage)target_ic).ShowStatusBar = ((InformationContainerImage)ic).ShowStatusBar;
                                                ((InformationContainerImage)target_ic).ShowToolBar = ((InformationContainerImage)ic).ShowToolBar;
                                                ((InformationContainerImage)target_ic).UseNearestNighborDraw = ((InformationContainerImage)ic).UseNearestNighborDraw;
                                                if (checkBox_copy_ics_map.Checked)
                                                    target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                            }
                                        }
                                    }

                                }
                                else if (ic is InformationContainerInfoText)
                                {
                                    if (!target_console.ContainsICInfoText(ic.DisplayName))
                                    {
                                        InformationContainerInfoText newIC = new InformationContainerInfoText(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;
                                        newIC.Extenstions = new List<string>(((InformationContainerInfoText)ic).Extenstions);
                                        newIC.FoldersMemory = new List<string>(((InformationContainerInfoText)ic).FoldersMemory);
                                        newIC.ShowToolstrip = ((InformationContainerInfoText)ic).ShowToolstrip;

                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        if (!dontShowConflictWindow)
                                        {
                                            FormICCopyConflict ff = new FormICCopyConflict(ic.Name + " from console " + target_console.Name + " => " + ic.Name + " of console " + source_console.Name);
                                            ff.ShowDialog();
                                            dontShowConflictWindow = ff.DoTheSameForOthers;
                                            replaceSettings = ff.CopySettings;
                                        }
                                        if (replaceSettings)
                                        {
                                            InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                            if (target_ic != null)
                                            {
                                                ((InformationContainerInfoText)target_ic).Extenstions = new List<string>(((InformationContainerInfoText)ic).Extenstions);
                                                ((InformationContainerInfoText)target_ic).FoldersMemory = new List<string>(((InformationContainerInfoText)ic).FoldersMemory);
                                                ((InformationContainerInfoText)target_ic).ShowToolstrip = ((InformationContainerInfoText)ic).ShowToolstrip;
                                                if (checkBox_copy_ics_map.Checked)
                                                    target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                            }
                                        }
                                    }
                                }
                                else if (ic is InformationContainerLinks)
                                {
                                    if (!target_console.ContainsICLinks(ic.DisplayName))
                                    {
                                        InformationContainerLinks newIC = new InformationContainerLinks(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;
                                        newIC.ShowToolstrip = ((InformationContainerLinks)ic).ShowToolstrip;

                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        if (!dontShowConflictWindow)
                                        {
                                            FormICCopyConflict ff = new FormICCopyConflict(ic.Name + " from console " + target_console.Name + " => " + ic.Name + " of console " + source_console.Name);
                                            ff.ShowDialog();
                                            dontShowConflictWindow = ff.DoTheSameForOthers;
                                            replaceSettings = ff.CopySettings;
                                        }
                                        if (replaceSettings)
                                        {
                                            InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                            if (target_ic != null)
                                            {
                                                ((InformationContainerLinks)target_ic).ShowToolstrip = ((InformationContainerInfoText)ic).ShowToolstrip;
                                                if (checkBox_copy_ics_map.Checked)
                                                    target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                            }
                                        }
                                    }
                                }
                                else if (ic is InformationContainerMedia)
                                {
                                    if (!target_console.ContainsICMedia(ic.DisplayName))
                                    {
                                        InformationContainerMedia newIC = new InformationContainerMedia(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;
                                        newIC.Extenstions = new List<string>(((InformationContainerMedia)ic).Extenstions);
                                        newIC.FoldersMemory = new List<string>(((InformationContainerMedia)ic).FoldersMemory);
                                        newIC.AutoHideToolstrip = ((InformationContainerMedia)ic).AutoHideToolstrip;
                                        newIC.AutoStart = ((InformationContainerMedia)ic).AutoStart;
                                        newIC.ColumnWidths = new List<int>(((InformationContainerMedia)ic).ColumnWidths);
                                        newIC.RepeatList = ((InformationContainerMedia)ic).RepeatList;
                                        newIC.ShowMediaControls = ((InformationContainerMedia)ic).ShowMediaControls;
                                        newIC.ShowToolstrip = ((InformationContainerMedia)ic).ShowToolstrip;

                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        if (!dontShowConflictWindow)
                                        {
                                            FormICCopyConflict ff = new FormICCopyConflict(ic.DisplayName + " from console " + target_console.Name + " => " + ic.DisplayName + " of console " + source_console.Name);
                                            ff.ShowDialog();
                                            dontShowConflictWindow = ff.DoTheSameForOthers;
                                            replaceSettings = ff.CopySettings;
                                        }
                                        if (replaceSettings)
                                        {
                                            InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                            if (target_ic != null)
                                            {
                                                ((InformationContainerMedia)target_ic).Extenstions = new List<string>(((InformationContainerMedia)ic).Extenstions);
                                                ((InformationContainerMedia)target_ic).FoldersMemory = new List<string>(((InformationContainerMedia)ic).FoldersMemory);
                                                ((InformationContainerMedia)target_ic).AutoHideToolstrip = ((InformationContainerMedia)ic).AutoHideToolstrip;
                                                ((InformationContainerMedia)target_ic).AutoStart = ((InformationContainerMedia)ic).AutoStart;
                                                ((InformationContainerMedia)target_ic).ColumnWidths = new List<int>(((InformationContainerMedia)ic).ColumnWidths);
                                                ((InformationContainerMedia)target_ic).RepeatList = ((InformationContainerMedia)ic).RepeatList;
                                                ((InformationContainerMedia)target_ic).ShowMediaControls = ((InformationContainerMedia)ic).ShowMediaControls;
                                                ((InformationContainerMedia)target_ic).ShowToolstrip = ((InformationContainerMedia)ic).ShowToolstrip;
                                                if (checkBox_copy_ics_map.Checked)
                                                    target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                            }
                                        }
                                    }
                                }
                                else if (ic is InformationContainerPDF)
                                {
                                    if (!target_console.ContainsICPDF(ic.DisplayName))
                                    {
                                        InformationContainerPDF newIC = new InformationContainerPDF(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;
                                        newIC.Extenstions = new List<string>(((InformationContainerPDF)ic).Extenstions);
                                        newIC.FoldersMemory = new List<string>(((InformationContainerPDF)ic).FoldersMemory);
                                        newIC.ShowToolstrip = ((InformationContainerPDF)ic).ShowToolstrip;

                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        if (!dontShowConflictWindow)
                                        {
                                            FormICCopyConflict ff = new FormICCopyConflict(ic.Name + " from console " + target_console.Name + " => " + ic.Name + " of console " + source_console.Name);
                                            ff.ShowDialog();
                                            dontShowConflictWindow = ff.DoTheSameForOthers;
                                            replaceSettings = ff.CopySettings;
                                        }
                                        if (replaceSettings)
                                        {
                                            InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                            if (target_ic != null)
                                            {
                                                ((InformationContainerPDF)target_ic).Extenstions = new List<string>(((InformationContainerPDF)ic).Extenstions);
                                                ((InformationContainerPDF)target_ic).FoldersMemory = new List<string>(((InformationContainerPDF)ic).FoldersMemory);
                                                ((InformationContainerPDF)target_ic).ShowToolstrip = ((InformationContainerPDF)ic).ShowToolstrip;
                                                if (checkBox_copy_ics_map.Checked)
                                                    target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                            }
                                        }
                                    }
                                }
                                else if (ic is InformationContainerReviewScore)
                                {
                                    if (!target_console.ContainsICReviewScore(ic.DisplayName))
                                    {
                                        InformationContainerReviewScore newIC = new InformationContainerReviewScore(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;
                                        newIC.ShowToolstrip = ((InformationContainerReviewScore)ic).ShowToolstrip;
                                        newIC.Fields = new List<string>(((InformationContainerReviewScore)ic).Fields);
                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        if (!dontShowConflictWindow)
                                        {
                                            FormICCopyConflict ff = new FormICCopyConflict(ic.Name + " from console " + target_console.Name + " => " + ic.Name + " of console " + source_console.Name);
                                            ff.ShowDialog();
                                            dontShowConflictWindow = ff.DoTheSameForOthers;
                                            replaceSettings = ff.CopySettings;
                                        }
                                        if (replaceSettings)
                                        {
                                            InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                            if (target_ic != null)
                                            {
                                                ((InformationContainerReviewScore)target_ic).ShowToolstrip = ((InformationContainerReviewScore)ic).ShowToolstrip;
                                                ((InformationContainerReviewScore)target_ic).Fields = new List<string>(((InformationContainerReviewScore)ic).Fields);
                                                if (checkBox_copy_ics_map.Checked)
                                                    target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                            }
                                        }
                                    }
                                }
                                else if (ic is InformationContainerYoutubeVideo)
                                {
                                    if (!target_console.ContainsICReviewScore(ic.DisplayName))
                                    {
                                        InformationContainerYoutubeVideo newIC = new InformationContainerYoutubeVideo(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;
                                        newIC.ShowToolstrip = ((InformationContainerYoutubeVideo)ic).ShowToolstrip;
                                        newIC.AutoPlay = ((InformationContainerYoutubeVideo)ic).AutoPlay;
                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        if (!dontShowConflictWindow)
                                        {
                                            FormICCopyConflict ff = new FormICCopyConflict(ic.Name + " from console " + target_console.Name + " => " + ic.Name + " of console " + source_console.Name);
                                            ff.ShowDialog();
                                            dontShowConflictWindow = ff.DoTheSameForOthers;
                                            replaceSettings = ff.CopySettings;
                                        }
                                        if (replaceSettings)
                                        {
                                            InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                            if (target_ic != null)
                                            {
                                                ((InformationContainerYoutubeVideo)target_ic).ShowToolstrip = ((InformationContainerYoutubeVideo)ic).ShowToolstrip;
                                                ((InformationContainerYoutubeVideo)target_ic).AutoPlay = ((InformationContainerYoutubeVideo)ic).AutoPlay;
                                                if (checkBox_copy_ics_map.Checked)
                                                    target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                            }
                                        }
                                    }
                                }
                                else if (ic is InformationContainerRomInfo && !target_console.ContainRomInfoIC())
                                {
                                    if (!target_console.ContainRomInfoIC())
                                    {
                                        InformationContainerRomInfo newIC = new InformationContainerRomInfo(profileManager.Profile.GenerateID());
                                        newIC.Name = ic.Name;
                                        newIC.DisplayName = ic.DisplayName;

                                        target_console.InformationContainers.Add(newIC);
                                        if (checkBox_copy_ics_map.Checked)
                                            target_console.InformationContainersMap.ReplaceID(ic.ID, newIC.ID, true);
                                    }
                                    else
                                    {
                                        InformationContainer target_ic = target_console.GetInformationContainer(ic.ID);
                                        if (target_ic != null)
                                        {
                                            if (checkBox_copy_ics_map.Checked)
                                                target_console.InformationContainersMap.ReplaceID(ic.ID, target_ic.ID, true);
                                        }
                                    }
                                }
                            }
                            target_console.FixColumnsForRomDataInfo();
                        }
                        if (checkBox_copy_rom_data_types.Checked)
                        {
                            foreach (RomData dt in source_console.RomDataInfoElements)
                            {
                                if (!target_console.ContainRomDataElement(dt.Name, dt.Type))
                                {
                                    RomData newDT = new RomData(profileManager.Profile.GenerateID(), dt.Name, dt.Type);
                                    target_console.RomDataInfoElements.Add(newDT);
                                }
                            }
                            target_console.FixColumnsForRomDataInfo();
                        }
                        if (checkBox_copy_launch_options.Checked)
                        {
                            target_console.RenameRomBeforeLaunch = source_console.RenameRomBeforeLaunch;
                            target_console.RomNameBeforeLaunch = source_console.RomNameBeforeLaunch;
                            target_console.UseRomWorkingDirectory = source_console.UseRomWorkingDirectory;
                            target_console.UseSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch = source_console.UseSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch;
                        }
                        if (checkBox_copy_tab_priority_settings.Checked)
                        {
                            target_console.AutoSwitchTabPriorityDepend = source_console.AutoSwitchTabPriorityDepend;
                        }
                        #endregion
                        #region MISC
                        if (checkBox_copy_filters.Checked)
                        {
                            foreach (Filter filt in source_console.Filters)
                            {
                                Filter newFilt = new Filter();
                                newFilt.Name = filt.Name;
                                newFilt.Parameters = filt.Parameters.Clone();

                                target_console.Filters.Add(newFilt);
                            }
                        }
                        if (checkBox_copy_columns_settings.Checked)
                        {
                            foreach (ColumnItem it in source_console.Columns)
                            {
                                foreach (ColumnItem targt_it in target_console.Columns)
                                {
                                    if (it.ColumnID == targt_it.ColumnID)
                                    {
                                        targt_it.Width = it.Width;
                                        targt_it.Visible = it.Visible;
                                    }
                                }
                            }
                        }
                        #endregion
                        profileManager.Profile.OnConsolePropertiesChanged(target_console.Name);
                        break;
                    }
                case SettingsCopyMode.CONSOLES_GROUP:
                    {
                        // 1 Get the target consoles group (the consoles group to copy settings into)
                        Core.ConsolesGroup target_consoles_group = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        // 2 Get the source consoles group (the consoles group to get settings from)
                        Core.ConsolesGroup source_consoles_group = profileManager.Profile.ConsoleGroups[ids[comboBox_copy_from.SelectedIndex]];
                        #region Visual and style
                        if (checkBox__copy_win_size_and_splitters.Checked)
                        {
                            target_consoles_group.Style.mainWindowHideLeftPanel = source_consoles_group.Style.mainWindowHideLeftPanel;
                            target_consoles_group.Style.mainWindowResize = source_consoles_group.Style.mainWindowResize;
                            target_consoles_group.Style.mainWindowSize = source_consoles_group.Style.mainWindowSize;
                            target_consoles_group.Style.mainWindowSplitContainer_left = source_consoles_group.Style.mainWindowSplitContainer_left;
                            target_consoles_group.Style.mainWindowSplitContainer_left_down = source_consoles_group.Style.mainWindowSplitContainer_left_down;
                            target_consoles_group.Style.mainWindowSplitContainer_main = source_consoles_group.Style.mainWindowSplitContainer_main;
                            target_consoles_group.Style.mainWindowSplitContainer_right = source_consoles_group.Style.mainWindowSplitContainer_right;
                        }
                        if (checkBox_copy_icon.Checked)
                        {
                            target_consoles_group.Icon = source_consoles_group.Icon;
                            target_consoles_group.IconThumbnail = source_consoles_group.IconThumbnail;
                        }
                        if (checkBox_copy_style.Checked)
                        {
                            target_consoles_group.Style.bkgColor_MainWindow = source_consoles_group.Style.bkgColor_MainWindow;
                            target_consoles_group.Style.bkgColor_CategoriesBrowser = source_consoles_group.Style.bkgColor_CategoriesBrowser;
                            target_consoles_group.Style.bkgColor_ConsolesBrowser = source_consoles_group.Style.bkgColor_ConsolesBrowser;
                            target_consoles_group.Style.bkgColor_EmulatorsBrowser = source_consoles_group.Style.bkgColor_EmulatorsBrowser;
                            target_consoles_group.Style.bkgColor_FiltersBrowser = source_consoles_group.Style.bkgColor_FiltersBrowser;
                            target_consoles_group.Style.bkgColor_InformationContainerBrowser = source_consoles_group.Style.bkgColor_InformationContainerBrowser;
                            target_consoles_group.Style.bkgColor_InformationContainerTabs = source_consoles_group.Style.bkgColor_InformationContainerTabs;
                            target_consoles_group.Style.bkgColor_PlaylistsBrowser = source_consoles_group.Style.bkgColor_PlaylistsBrowser;
                            target_consoles_group.Style.bkgColor_RomsBrowser = source_consoles_group.Style.bkgColor_RomsBrowser;
                            target_consoles_group.Style.bkgColor_StartOptions = source_consoles_group.Style.bkgColor_StartOptions;
                            target_consoles_group.Style.txtColor_MainWindowMainMenu = source_consoles_group.Style.txtColor_MainWindowMainMenu;
                            target_consoles_group.Style.txtColor_CategoriesBrowser = source_consoles_group.Style.txtColor_CategoriesBrowser;
                            target_consoles_group.Style.txtColor_ConsolesBrowser = source_consoles_group.Style.txtColor_ConsolesBrowser;
                            target_consoles_group.Style.txtColor_EmulatorsBrowser = source_consoles_group.Style.txtColor_EmulatorsBrowser;
                            target_consoles_group.Style.txtColor_FiltersBrowser = source_consoles_group.Style.txtColor_FiltersBrowser;
                            target_consoles_group.Style.txtColor_InformationContainerBrowser = source_consoles_group.Style.txtColor_InformationContainerBrowser;
                            target_consoles_group.Style.txtColor_InformationContainerTabs = source_consoles_group.Style.txtColor_InformationContainerTabs;
                            target_consoles_group.Style.txtColor_PlaylistsBrowser = source_consoles_group.Style.txtColor_PlaylistsBrowser;
                            target_consoles_group.Style.txtColor_RomsBrowser = source_consoles_group.Style.txtColor_RomsBrowser;
                            target_consoles_group.Style.txtColor_StartOptions = source_consoles_group.Style.txtColor_StartOptions;
                            target_consoles_group.Style.image_CategoriesBrowser = source_consoles_group.Style.image_CategoriesBrowser;
                            target_consoles_group.Style.image_ConsolesBrowser = source_consoles_group.Style.image_ConsolesBrowser;
                            target_consoles_group.Style.image_EmulatorsBrowser = source_consoles_group.Style.image_EmulatorsBrowser;
                            target_consoles_group.Style.image_FiltersBrowser = source_consoles_group.Style.image_FiltersBrowser;
                            target_consoles_group.Style.image_InformationContainerBrowser = source_consoles_group.Style.image_InformationContainerBrowser;
                            target_consoles_group.Style.image_InformationContainerTabs = source_consoles_group.Style.image_InformationContainerTabs;
                            target_consoles_group.Style.image_PlaylistsBrowser = source_consoles_group.Style.image_PlaylistsBrowser;
                            target_consoles_group.Style.image_RomsBrowser = source_consoles_group.Style.image_RomsBrowser;
                            target_consoles_group.Style.image_StartOptions = source_consoles_group.Style.image_StartOptions;

                            target_consoles_group.Style.imageMode_CategoriesBrowser = source_consoles_group.Style.imageMode_CategoriesBrowser;
                            target_consoles_group.Style.imageMode_ConsolesBrowser = source_consoles_group.Style.imageMode_ConsolesBrowser;
                            target_consoles_group.Style.imageMode_EmulatorsBrowser = source_consoles_group.Style.imageMode_EmulatorsBrowser;
                            target_consoles_group.Style.imageMode_FiltersBrowser = source_consoles_group.Style.imageMode_FiltersBrowser;
                            target_consoles_group.Style.imageMode_InformationContainerBrowser = source_consoles_group.Style.imageMode_InformationContainerBrowser;
                            target_consoles_group.Style.imageMode_InformationContainerTabs = source_consoles_group.Style.imageMode_InformationContainerTabs;
                            target_consoles_group.Style.imageMode_PlaylistsBrowser = source_consoles_group.Style.imageMode_PlaylistsBrowser;
                            target_consoles_group.Style.imageMode_RomsBrowser = source_consoles_group.Style.imageMode_RomsBrowser;
                            target_consoles_group.Style.imageMode_StartOptions = source_consoles_group.Style.imageMode_StartOptions;

                            target_consoles_group.Style.font_CategoriesBrowser = source_consoles_group.Style.font_CategoriesBrowser;
                            target_consoles_group.Style.font_ConsolesBrowser = source_consoles_group.Style.font_ConsolesBrowser;
                            target_consoles_group.Style.font_EmulatorsBrowser = source_consoles_group.Style.font_EmulatorsBrowser;
                            target_consoles_group.Style.font_FiltersBrowser = source_consoles_group.Style.font_FiltersBrowser;
                            target_consoles_group.Style.font_InformationContainerBrowser = source_consoles_group.Style.font_InformationContainerBrowser;
                            target_consoles_group.Style.font_InformationContainerTabs = source_consoles_group.Style.font_InformationContainerTabs;
                            target_consoles_group.Style.font_PlaylistsBrowser = source_consoles_group.Style.font_PlaylistsBrowser;
                            target_consoles_group.Style.font_RomsBrowser = source_consoles_group.Style.font_RomsBrowser;
                            target_consoles_group.Style.font_StartOptions = source_consoles_group.Style.font_StartOptions;

                            target_consoles_group.Style.TabPageColor = source_consoles_group.Style.TabPageColor;
                            target_consoles_group.Style.TabPageHighlightedColor = source_consoles_group.Style.TabPageHighlightedColor;
                            target_consoles_group.Style.TabPageSelectedColor = source_consoles_group.Style.TabPageSelectedColor;
                            target_consoles_group.Style.TabPageSplitColor = source_consoles_group.Style.TabPageSplitColor;
                            target_consoles_group.Style.TabPageTextsColor = source_consoles_group.Style.TabPageTextsColor;
                            target_consoles_group.Style.listviewColumnClickColor = source_consoles_group.Style.listviewColumnClickColor;
                            target_consoles_group.Style.listviewColumnColor = source_consoles_group.Style.listviewColumnColor;
                            target_consoles_group.Style.listviewColumnHighlightColor = source_consoles_group.Style.listviewColumnHighlightColor;
                            target_consoles_group.Style.listviewColumnTextColor = source_consoles_group.Style.listviewColumnTextColor;
                            target_consoles_group.Style.listviewDrawHighlight = source_consoles_group.Style.listviewDrawHighlight;
                            target_consoles_group.Style.listviewHighlightColor = source_consoles_group.Style.listviewHighlightColor;
                            target_consoles_group.Style.listviewMouseOverColor = source_consoles_group.Style.listviewMouseOverColor;
                            target_consoles_group.Style.listviewSpecialColor = source_consoles_group.Style.listviewSpecialColor;
                            target_consoles_group.Style.listviewTextsColor = source_consoles_group.Style.listviewTextsColor;
                        }
                        #endregion
                        #region MISC
                        if (checkBox_copy_filters.Checked)
                        {
                            foreach (Filter filt in source_consoles_group.Filters)
                            {
                                Filter newFilt = new Filter();
                                newFilt.Name = filt.Name;
                                newFilt.Parameters = filt.Parameters.Clone();

                                target_consoles_group.Filters.Add(newFilt);
                            }
                        }
                        if (checkBox_copy_columns_settings.Checked)
                        {
                            foreach (ColumnItem it in source_consoles_group.Columns)
                            {
                                foreach (ColumnItem targt_it in target_consoles_group.Columns)
                                {
                                    if (it.ColumnID == targt_it.ColumnID)
                                    {
                                        targt_it.Width = it.Width;
                                        targt_it.Visible = it.Visible;
                                    }
                                }
                            }
                        }
                        #endregion
                        profileManager.Profile.OnConsolesGroupPropertiesChanged(target_consoles_group.Name);
                        break;
                    }
                case SettingsCopyMode.PLAYLIST:
                    {
                        // 1 Get the target consoles group (the consoles group to copy settings into)
                        Core.Playlist target_playlist = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        // 2 Get the source consoles group (the consoles group to get settings from)
                        Core.Playlist source_playlist = profileManager.Profile.Playlists[ids[comboBox_copy_from.SelectedIndex]];
                        #region Visual and style
                        if (checkBox__copy_win_size_and_splitters.Checked)
                        {
                            target_playlist.Style.mainWindowHideLeftPanel = source_playlist.Style.mainWindowHideLeftPanel;
                            target_playlist.Style.mainWindowResize = source_playlist.Style.mainWindowResize;
                            target_playlist.Style.mainWindowSize = source_playlist.Style.mainWindowSize;
                            target_playlist.Style.mainWindowSplitContainer_left = source_playlist.Style.mainWindowSplitContainer_left;
                            target_playlist.Style.mainWindowSplitContainer_left_down = source_playlist.Style.mainWindowSplitContainer_left_down;
                            target_playlist.Style.mainWindowSplitContainer_main = source_playlist.Style.mainWindowSplitContainer_main;
                            target_playlist.Style.mainWindowSplitContainer_right = source_playlist.Style.mainWindowSplitContainer_right;
                        }
                        if (checkBox_copy_icon.Checked)
                        {
                            target_playlist.Icon = source_playlist.Icon;
                            target_playlist.IconThumbnail = source_playlist.IconThumbnail;
                        }
                        if (checkBox_copy_style.Checked)
                        {
                            target_playlist.Style.bkgColor_MainWindow = source_playlist.Style.bkgColor_MainWindow;
                            target_playlist.Style.bkgColor_CategoriesBrowser = source_playlist.Style.bkgColor_CategoriesBrowser;
                            target_playlist.Style.bkgColor_ConsolesBrowser = source_playlist.Style.bkgColor_ConsolesBrowser;
                            target_playlist.Style.bkgColor_EmulatorsBrowser = source_playlist.Style.bkgColor_EmulatorsBrowser;
                            target_playlist.Style.bkgColor_FiltersBrowser = source_playlist.Style.bkgColor_FiltersBrowser;
                            target_playlist.Style.bkgColor_InformationContainerBrowser = source_playlist.Style.bkgColor_InformationContainerBrowser;
                            target_playlist.Style.bkgColor_InformationContainerTabs = source_playlist.Style.bkgColor_InformationContainerTabs;
                            target_playlist.Style.bkgColor_PlaylistsBrowser = source_playlist.Style.bkgColor_PlaylistsBrowser;
                            target_playlist.Style.bkgColor_RomsBrowser = source_playlist.Style.bkgColor_RomsBrowser;
                            target_playlist.Style.bkgColor_StartOptions = source_playlist.Style.bkgColor_StartOptions;
                            target_playlist.Style.txtColor_MainWindowMainMenu = source_playlist.Style.txtColor_MainWindowMainMenu;
                            target_playlist.Style.txtColor_CategoriesBrowser = source_playlist.Style.txtColor_CategoriesBrowser;
                            target_playlist.Style.txtColor_ConsolesBrowser = source_playlist.Style.txtColor_ConsolesBrowser;
                            target_playlist.Style.txtColor_EmulatorsBrowser = source_playlist.Style.txtColor_EmulatorsBrowser;
                            target_playlist.Style.txtColor_FiltersBrowser = source_playlist.Style.txtColor_FiltersBrowser;
                            target_playlist.Style.txtColor_InformationContainerBrowser = source_playlist.Style.txtColor_InformationContainerBrowser;
                            target_playlist.Style.txtColor_InformationContainerTabs = source_playlist.Style.txtColor_InformationContainerTabs;
                            target_playlist.Style.txtColor_PlaylistsBrowser = source_playlist.Style.txtColor_PlaylistsBrowser;
                            target_playlist.Style.txtColor_RomsBrowser = source_playlist.Style.txtColor_RomsBrowser;
                            target_playlist.Style.txtColor_StartOptions = source_playlist.Style.txtColor_StartOptions;
                            target_playlist.Style.image_CategoriesBrowser = source_playlist.Style.image_CategoriesBrowser;
                            target_playlist.Style.image_ConsolesBrowser = source_playlist.Style.image_ConsolesBrowser;
                            target_playlist.Style.image_EmulatorsBrowser = source_playlist.Style.image_EmulatorsBrowser;
                            target_playlist.Style.image_FiltersBrowser = source_playlist.Style.image_FiltersBrowser;
                            target_playlist.Style.image_InformationContainerBrowser = source_playlist.Style.image_InformationContainerBrowser;
                            target_playlist.Style.image_InformationContainerTabs = source_playlist.Style.image_InformationContainerTabs;
                            target_playlist.Style.image_PlaylistsBrowser = source_playlist.Style.image_PlaylistsBrowser;
                            target_playlist.Style.image_RomsBrowser = source_playlist.Style.image_RomsBrowser;
                            target_playlist.Style.image_StartOptions = source_playlist.Style.image_StartOptions;

                            target_playlist.Style.imageMode_CategoriesBrowser = source_playlist.Style.imageMode_CategoriesBrowser;
                            target_playlist.Style.imageMode_ConsolesBrowser = source_playlist.Style.imageMode_ConsolesBrowser;
                            target_playlist.Style.imageMode_EmulatorsBrowser = source_playlist.Style.imageMode_EmulatorsBrowser;
                            target_playlist.Style.imageMode_FiltersBrowser = source_playlist.Style.imageMode_FiltersBrowser;
                            target_playlist.Style.imageMode_InformationContainerBrowser = source_playlist.Style.imageMode_InformationContainerBrowser;
                            target_playlist.Style.imageMode_InformationContainerTabs = source_playlist.Style.imageMode_InformationContainerTabs;
                            target_playlist.Style.imageMode_PlaylistsBrowser = source_playlist.Style.imageMode_PlaylistsBrowser;
                            target_playlist.Style.imageMode_RomsBrowser = source_playlist.Style.imageMode_RomsBrowser;
                            target_playlist.Style.imageMode_StartOptions = source_playlist.Style.imageMode_StartOptions;

                            target_playlist.Style.font_CategoriesBrowser = source_playlist.Style.font_CategoriesBrowser;
                            target_playlist.Style.font_ConsolesBrowser = source_playlist.Style.font_ConsolesBrowser;
                            target_playlist.Style.font_EmulatorsBrowser = source_playlist.Style.font_EmulatorsBrowser;
                            target_playlist.Style.font_FiltersBrowser = source_playlist.Style.font_FiltersBrowser;
                            target_playlist.Style.font_InformationContainerBrowser = source_playlist.Style.font_InformationContainerBrowser;
                            target_playlist.Style.font_InformationContainerTabs = source_playlist.Style.font_InformationContainerTabs;
                            target_playlist.Style.font_PlaylistsBrowser = source_playlist.Style.font_PlaylistsBrowser;
                            target_playlist.Style.font_RomsBrowser = source_playlist.Style.font_RomsBrowser;
                            target_playlist.Style.font_StartOptions = source_playlist.Style.font_StartOptions;

                            target_playlist.Style.TabPageColor = source_playlist.Style.TabPageColor;
                            target_playlist.Style.TabPageHighlightedColor = source_playlist.Style.TabPageHighlightedColor;
                            target_playlist.Style.TabPageSelectedColor = source_playlist.Style.TabPageSelectedColor;
                            target_playlist.Style.TabPageSplitColor = source_playlist.Style.TabPageSplitColor;
                            target_playlist.Style.TabPageTextsColor = source_playlist.Style.TabPageTextsColor;
                            target_playlist.Style.listviewColumnClickColor = source_playlist.Style.listviewColumnClickColor;
                            target_playlist.Style.listviewColumnColor = source_playlist.Style.listviewColumnColor;
                            target_playlist.Style.listviewColumnHighlightColor = source_playlist.Style.listviewColumnHighlightColor;
                            target_playlist.Style.listviewColumnTextColor = source_playlist.Style.listviewColumnTextColor;
                            target_playlist.Style.listviewDrawHighlight = source_playlist.Style.listviewDrawHighlight;
                            target_playlist.Style.listviewHighlightColor = source_playlist.Style.listviewHighlightColor;
                            target_playlist.Style.listviewMouseOverColor = source_playlist.Style.listviewMouseOverColor;
                            target_playlist.Style.listviewSpecialColor = source_playlist.Style.listviewSpecialColor;
                            target_playlist.Style.listviewTextsColor = source_playlist.Style.listviewTextsColor;
                        }
                        #endregion
                        #region MISC
                        if (checkBox_copy_filters.Checked)
                        {
                            foreach (Filter filt in source_playlist.Filters)
                            {
                                Filter newFilt = new Filter();
                                newFilt.Name = filt.Name;
                                newFilt.Parameters = filt.Parameters.Clone();

                                target_playlist.Filters.Add(newFilt);
                            }
                        }
                        if (checkBox_copy_columns_settings.Checked)
                        {
                            foreach (ColumnItem it in source_playlist.Columns)
                            {
                                foreach (ColumnItem targt_it in target_playlist.Columns)
                                {
                                    if (it.ColumnID == targt_it.ColumnID)
                                    {
                                        targt_it.Width = it.Width;
                                        targt_it.Visible = it.Visible;
                                    }
                                }
                            }
                        }
                        #endregion
                        profileManager.Profile.OnPlaylistPropertiesChanged(target_playlist.Name);
                        break;
                    }
                case SettingsCopyMode.PLAYLISTS_GROUP:
                    {
                        // 1 Get the target consoles group (the consoles group to copy settings into)
                        Core.PlaylistsGroup target_playlists_group = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                        // 2 Get the source consoles group (the consoles group to get settings from)
                        Core.PlaylistsGroup source_playlists_group = profileManager.Profile.PlaylistGroups[ids[comboBox_copy_from.SelectedIndex]];
                        #region Visual and style
                        if (checkBox__copy_win_size_and_splitters.Checked)
                        {
                            target_playlists_group.Style.mainWindowHideLeftPanel = source_playlists_group.Style.mainWindowHideLeftPanel;
                            target_playlists_group.Style.mainWindowResize = source_playlists_group.Style.mainWindowResize;
                            target_playlists_group.Style.mainWindowSize = source_playlists_group.Style.mainWindowSize;
                            target_playlists_group.Style.mainWindowSplitContainer_left = source_playlists_group.Style.mainWindowSplitContainer_left;
                            target_playlists_group.Style.mainWindowSplitContainer_left_down = source_playlists_group.Style.mainWindowSplitContainer_left_down;
                            target_playlists_group.Style.mainWindowSplitContainer_main = source_playlists_group.Style.mainWindowSplitContainer_main;
                            target_playlists_group.Style.mainWindowSplitContainer_right = source_playlists_group.Style.mainWindowSplitContainer_right;
                        }
                        if (checkBox_copy_icon.Checked)
                        {
                            target_playlists_group.Icon = source_playlists_group.Icon;
                            target_playlists_group.IconThumbnail = source_playlists_group.IconThumbnail;
                        }
                        if (checkBox_copy_style.Checked)
                        {
                            target_playlists_group.Style.bkgColor_MainWindow = source_playlists_group.Style.bkgColor_MainWindow;
                            target_playlists_group.Style.bkgColor_CategoriesBrowser = source_playlists_group.Style.bkgColor_CategoriesBrowser;
                            target_playlists_group.Style.bkgColor_ConsolesBrowser = source_playlists_group.Style.bkgColor_ConsolesBrowser;
                            target_playlists_group.Style.bkgColor_EmulatorsBrowser = source_playlists_group.Style.bkgColor_EmulatorsBrowser;
                            target_playlists_group.Style.bkgColor_FiltersBrowser = source_playlists_group.Style.bkgColor_FiltersBrowser;
                            target_playlists_group.Style.bkgColor_InformationContainerBrowser = source_playlists_group.Style.bkgColor_InformationContainerBrowser;
                            target_playlists_group.Style.bkgColor_InformationContainerTabs = source_playlists_group.Style.bkgColor_InformationContainerTabs;
                            target_playlists_group.Style.bkgColor_PlaylistsBrowser = source_playlists_group.Style.bkgColor_PlaylistsBrowser;
                            target_playlists_group.Style.bkgColor_RomsBrowser = source_playlists_group.Style.bkgColor_RomsBrowser;
                            target_playlists_group.Style.bkgColor_StartOptions = source_playlists_group.Style.bkgColor_StartOptions;
                            target_playlists_group.Style.txtColor_MainWindowMainMenu = source_playlists_group.Style.txtColor_MainWindowMainMenu;
                            target_playlists_group.Style.txtColor_CategoriesBrowser = source_playlists_group.Style.txtColor_CategoriesBrowser;
                            target_playlists_group.Style.txtColor_ConsolesBrowser = source_playlists_group.Style.txtColor_ConsolesBrowser;
                            target_playlists_group.Style.txtColor_EmulatorsBrowser = source_playlists_group.Style.txtColor_EmulatorsBrowser;
                            target_playlists_group.Style.txtColor_FiltersBrowser = source_playlists_group.Style.txtColor_FiltersBrowser;
                            target_playlists_group.Style.txtColor_InformationContainerBrowser = source_playlists_group.Style.txtColor_InformationContainerBrowser;
                            target_playlists_group.Style.txtColor_InformationContainerTabs = source_playlists_group.Style.txtColor_InformationContainerTabs;
                            target_playlists_group.Style.txtColor_PlaylistsBrowser = source_playlists_group.Style.txtColor_PlaylistsBrowser;
                            target_playlists_group.Style.txtColor_RomsBrowser = source_playlists_group.Style.txtColor_RomsBrowser;
                            target_playlists_group.Style.txtColor_StartOptions = source_playlists_group.Style.txtColor_StartOptions;
                            target_playlists_group.Style.image_CategoriesBrowser = source_playlists_group.Style.image_CategoriesBrowser;
                            target_playlists_group.Style.image_ConsolesBrowser = source_playlists_group.Style.image_ConsolesBrowser;
                            target_playlists_group.Style.image_EmulatorsBrowser = source_playlists_group.Style.image_EmulatorsBrowser;
                            target_playlists_group.Style.image_FiltersBrowser = source_playlists_group.Style.image_FiltersBrowser;
                            target_playlists_group.Style.image_InformationContainerBrowser = source_playlists_group.Style.image_InformationContainerBrowser;
                            target_playlists_group.Style.image_InformationContainerTabs = source_playlists_group.Style.image_InformationContainerTabs;
                            target_playlists_group.Style.image_PlaylistsBrowser = source_playlists_group.Style.image_PlaylistsBrowser;
                            target_playlists_group.Style.image_RomsBrowser = source_playlists_group.Style.image_RomsBrowser;
                            target_playlists_group.Style.image_StartOptions = source_playlists_group.Style.image_StartOptions;

                            target_playlists_group.Style.imageMode_CategoriesBrowser = source_playlists_group.Style.imageMode_CategoriesBrowser;
                            target_playlists_group.Style.imageMode_ConsolesBrowser = source_playlists_group.Style.imageMode_ConsolesBrowser;
                            target_playlists_group.Style.imageMode_EmulatorsBrowser = source_playlists_group.Style.imageMode_EmulatorsBrowser;
                            target_playlists_group.Style.imageMode_FiltersBrowser = source_playlists_group.Style.imageMode_FiltersBrowser;
                            target_playlists_group.Style.imageMode_InformationContainerBrowser = source_playlists_group.Style.imageMode_InformationContainerBrowser;
                            target_playlists_group.Style.imageMode_InformationContainerTabs = source_playlists_group.Style.imageMode_InformationContainerTabs;
                            target_playlists_group.Style.imageMode_PlaylistsBrowser = source_playlists_group.Style.imageMode_PlaylistsBrowser;
                            target_playlists_group.Style.imageMode_RomsBrowser = source_playlists_group.Style.imageMode_RomsBrowser;
                            target_playlists_group.Style.imageMode_StartOptions = source_playlists_group.Style.imageMode_StartOptions;

                            target_playlists_group.Style.font_CategoriesBrowser = source_playlists_group.Style.font_CategoriesBrowser;
                            target_playlists_group.Style.font_ConsolesBrowser = source_playlists_group.Style.font_ConsolesBrowser;
                            target_playlists_group.Style.font_EmulatorsBrowser = source_playlists_group.Style.font_EmulatorsBrowser;
                            target_playlists_group.Style.font_FiltersBrowser = source_playlists_group.Style.font_FiltersBrowser;
                            target_playlists_group.Style.font_InformationContainerBrowser = source_playlists_group.Style.font_InformationContainerBrowser;
                            target_playlists_group.Style.font_InformationContainerTabs = source_playlists_group.Style.font_InformationContainerTabs;
                            target_playlists_group.Style.font_PlaylistsBrowser = source_playlists_group.Style.font_PlaylistsBrowser;
                            target_playlists_group.Style.font_RomsBrowser = source_playlists_group.Style.font_RomsBrowser;
                            target_playlists_group.Style.font_StartOptions = source_playlists_group.Style.font_StartOptions;

                            target_playlists_group.Style.TabPageColor = source_playlists_group.Style.TabPageColor;
                            target_playlists_group.Style.TabPageHighlightedColor = source_playlists_group.Style.TabPageHighlightedColor;
                            target_playlists_group.Style.TabPageSelectedColor = source_playlists_group.Style.TabPageSelectedColor;
                            target_playlists_group.Style.TabPageSplitColor = source_playlists_group.Style.TabPageSplitColor;
                            target_playlists_group.Style.TabPageTextsColor = source_playlists_group.Style.TabPageTextsColor;
                            target_playlists_group.Style.listviewColumnClickColor = source_playlists_group.Style.listviewColumnClickColor;
                            target_playlists_group.Style.listviewColumnColor = source_playlists_group.Style.listviewColumnColor;
                            target_playlists_group.Style.listviewColumnHighlightColor = source_playlists_group.Style.listviewColumnHighlightColor;
                            target_playlists_group.Style.listviewColumnTextColor = source_playlists_group.Style.listviewColumnTextColor;
                            target_playlists_group.Style.listviewDrawHighlight = source_playlists_group.Style.listviewDrawHighlight;
                            target_playlists_group.Style.listviewHighlightColor = source_playlists_group.Style.listviewHighlightColor;
                            target_playlists_group.Style.listviewMouseOverColor = source_playlists_group.Style.listviewMouseOverColor;
                            target_playlists_group.Style.listviewSpecialColor = source_playlists_group.Style.listviewSpecialColor;
                            target_playlists_group.Style.listviewTextsColor = source_playlists_group.Style.listviewTextsColor;
                        }
                        #endregion
                        #region MISC
                        if (checkBox_copy_filters.Checked)
                        {
                            foreach (Filter filt in source_playlists_group.Filters)
                            {
                                Filter newFilt = new Filter();
                                newFilt.Name = filt.Name;
                                newFilt.Parameters = filt.Parameters.Clone();

                                target_playlists_group.Filters.Add(newFilt);
                            }
                        }
                        if (checkBox_copy_columns_settings.Checked)
                        {
                            foreach (ColumnItem it in source_playlists_group.Columns)
                            {
                                foreach (ColumnItem targt_it in target_playlists_group.Columns)
                                {
                                    if (it.ColumnID == targt_it.ColumnID)
                                    {
                                        targt_it.Width = it.Width;
                                        targt_it.Visible = it.Visible;
                                    }
                                }
                            }
                        }
                        #endregion
                        profileManager.Profile.OnPlaylistsGroupPropertiesChanged(target_playlists_group.Name);
                        break;
                    }
            }
            ManagedMessageBox.ShowMessage("Done !!");
            Close();
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Copy everything
        private void button3_Click(object sender, EventArgs e)
        {
            checkBox_copy_archive_settings.Checked = true;
            checkBox_copy_columns_settings.Checked = true;
            checkBox_copy_console_extensions.Checked = true;
            checkBox_copy_copy_rom_settings.Checked = true;
            checkBox_copy_filters.Checked = true;
            checkBox_copy_icon.Checked = true;
            checkBox_copy_ics.Checked = true;
            checkBox_copy_launch_options.Checked = true;
            checkBox_copy_rchive_extensions.Checked = true;
            checkBox_copy_rom_data_types.Checked = true;
            checkBox_copy_style.Checked = true;
            checkBox_copy_tab_priority_settings.Checked = true;
            checkBox__copy_win_size_and_splitters.Checked = true;
        }
        // Copy nothing
        private void button4_Click(object sender, EventArgs e)
        {
            checkBox_copy_archive_settings.Checked = false;
            checkBox_copy_columns_settings.Checked = false;
            checkBox_copy_console_extensions.Checked = false;
            checkBox_copy_copy_rom_settings.Checked = false;
            checkBox_copy_filters.Checked = false;
            checkBox_copy_icon.Checked = false;
            checkBox_copy_ics.Checked = false;
            checkBox_copy_launch_options.Checked = false;
            checkBox_copy_rchive_extensions.Checked = false;
            checkBox_copy_rom_data_types.Checked = false;
            checkBox_copy_style.Checked = false;
            checkBox_copy_tab_priority_settings.Checked = false;
            checkBox__copy_win_size_and_splitters.Checked = false;
            checkBox_clear_ics_first.Checked = false;
            checkBox_clear_rom_data_first.Checked = false;
            checkBox_copy_ics_map.Checked = false;
        }
        // Default
        private void button5_Click(object sender, EventArgs e)
        {
            checkBox_copy_archive_settings.Checked = false;
            checkBox_copy_columns_settings.Checked = false;
            checkBox_copy_console_extensions.Checked = false;
            checkBox_copy_copy_rom_settings.Checked = false;
            checkBox_copy_filters.Checked = false;
            checkBox_copy_icon.Checked = false;
            checkBox_copy_ics.Checked = false;
            checkBox_copy_launch_options.Checked = false;
            checkBox_copy_rchive_extensions.Checked = false;
            checkBox_copy_rom_data_types.Checked = false;
            checkBox_copy_style.Checked = false;
            checkBox_clear_ics_first.Checked = false;
            checkBox_clear_rom_data_first.Checked = false;
            checkBox_copy_ics_map.Checked = false;
            checkBox_copy_tab_priority_settings.Checked = false;
            checkBox__copy_win_size_and_splitters.Checked = true;
        }
        // Console settings only
        private void button6_Click(object sender, EventArgs e)
        {
            checkBox_copy_archive_settings.Checked = true;
            checkBox_copy_columns_settings.Checked = false;
            checkBox_copy_console_extensions.Checked = true;
            checkBox_copy_copy_rom_settings.Checked = true;
            checkBox_copy_filters.Checked = true;
            checkBox_copy_icon.Checked = false;
            checkBox_copy_ics.Checked = true;
            checkBox_copy_launch_options.Checked = true;
            checkBox_copy_rchive_extensions.Checked = true;
            checkBox_copy_rom_data_types.Checked = true;
            checkBox_copy_style.Checked = false;
            checkBox_copy_tab_priority_settings.Checked = true;
            checkBox__copy_win_size_and_splitters.Checked = true;
            checkBox_clear_ics_first.Checked = true;
            checkBox_clear_rom_data_first.Checked = true;
            checkBox_copy_ics_map.Checked = true;
        }
        // Style and visual only
        private void button6_Click_1(object sender, EventArgs e)
        {
            checkBox_copy_archive_settings.Checked = false;
            checkBox_copy_columns_settings.Checked = false;
            checkBox_copy_console_extensions.Checked = false;
            checkBox_copy_copy_rom_settings.Checked = false;
            checkBox_copy_filters.Checked = false;
            checkBox_copy_icon.Checked = true;
            checkBox_copy_ics.Checked = false;
            checkBox_copy_launch_options.Checked = false;
            checkBox_copy_rchive_extensions.Checked = false;
            checkBox_copy_rom_data_types.Checked = false;
            checkBox_copy_style.Checked = true;
            checkBox_copy_tab_priority_settings.Checked = false;
            checkBox__copy_win_size_and_splitters.Checked = false;
            checkBox_clear_ics_first.Checked = false;
            checkBox_clear_rom_data_first.Checked = false;
            checkBox_copy_ics_map.Checked = false;
        }
        private void checkBox_copy_ics_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_copy_ics.Enabled)
                checkBox_copy_ics_map.Enabled = checkBox_clear_ics_first.Enabled = checkBox_copy_ics.Checked;
        }
        private void checkBox_copy_rom_data_types_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_copy_rom_data_types.Enabled)
                checkBox_clear_rom_data_first.Enabled = checkBox_copy_rom_data_types.Checked;
        }
    }
}
