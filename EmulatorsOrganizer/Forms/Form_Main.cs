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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using EmulatorsOrganizer.Services.TraceListners;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.GUI;
using MTC;
using MMB;
namespace EmulatorsOrganizer
{
    public partial class Form_Main : Form
    {
        public Form_Main(string[] args)
        {
            Trace.Listeners.Add(new MainWindowStatusTraceListner());
            settings = (SettingsService)ServicesManager.GetService("Global Settings");
            ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
            profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
            // initialize important events
            profileManager.NewProfileCreated += profileManager_NewProfileCreated;
            profileManager.CreatingNewProfile += profileManager_CreatingNewProfile;
            profileManager.OpeningProfile += profileManager_CreatingNewProfile;
            profileManager.ProfileOpened += profileManager_ProfileOpened;
            profileManager.DisableStyleSave += profileManager_DisableStyleSave;
            profileManager.EnableStyleSave += profileManager_EnableStyleSave;
            InitializeComponent();
            InitializeControls();
            LoadSettings();

            // Create new profile
            profileManager.NewProfile();

            Save = false;

            ApplyStyle(null);// Reset style to defaults

            // Open args
            try
            {
                if (args != null)
                {
                    int i = 0;
                    foreach (string l in args)
                    {
                        // First one must profile path
                        if (i == 0)
                        {
                            if (System.IO.File.Exists(args[0]))
                            {
                                ProfileSaveLoadStatus status = profileManager.LoadProfile(l);
                                if (status.Type == ProfileSaveLaodType.Success)
                                {
                                    Save = false;
                                    AddRecent(args[0]);
                                    DisableQuickScreen = true;
                                }
                                else
                                {
                                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantOpenProfile"] + ":\n" + status.Message,
                                        ls["MessageCaption_OpenProfile"]);
                                }
                            }
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                ManagedMessageBox.ShowErrorMessage(ex.ToString());
            }
            // TODO: apply download from db 
            detectAndDownloadFromTheGamesDBnetForSelectedConsoleToolStripMenuItem.Visible = false;
        }

        #region Controls
        /*Browser controls*/
        private ConsolesBrowser consolesBrowser;
        private EmulatorsBrowser emulatorsBrowser;
        private PlaylistsBrowser playlistsBrowser;
        private RomsBrowser romsBrowser;
        private FiltersBrowser filtersBrowser;
        private CategoiresBrowser categoriesBrowser;
        private InformationContainersBrowser informationContainersBrowser1;
        private StartOptionsPanelBrowser startOptionsBrowser;
        private ProfileStatusBrowser profileStatusBrowser;
        private ConsoleStatusBrowser consoleStatusBrowser;
        #endregion
        /*Services*/
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");

        private delegate void WriteStatusDelegate(string text, Color color);
        private ActiveTab activeTab = ActiveTab.None;
        private bool save = false;
        private EOStyle defaultStyle = new EOStyle();
        private FormWindowState originalWindowState = FormWindowState.Normal;
        private delegate void ShowProgress(string status, int precent);
        private int quickSearchTimer;
        public bool DisableQuickScreen;
        private bool IsLoadingStyle;

        private void LoadSettings()
        {
            //languages
            string lang = (string)settings.GetValue("Language", true, "English (United States)");
            for (int i = 0; i < ls.SupportedLanguages.Length / 3; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = ls.SupportedLanguages[i, 2];
                item.Checked = ls.SupportedLanguages[i, 0] == lang;
                languageToolStripMenuItem.DropDownItems.Add(item);
            }
            // Assuming settings already loaded at startup of application.
            // Load settings from file. If not found, set to default
            this.Location = (Point)settings.GetValue("Main window location", true, new Point(10, 10));
            this.Size = (Size)settings.GetValue("Main window size", true, new Size(1024, 720));

            //this.splitContainer_left.SplitterDistance = (int)settings.GetValue("splitContainer_left", true, (int)200);
            //this.splitContainer_left_down.SplitterDistance = (int)settings.GetValue("splitContainer_left_down", true, (int)200);
            //this.splitContainer_main.SplitterDistance = (int)settings.GetValue("splitContainer_main", true, (int)200);
            //this.splitContainer_right.SplitterDistance = (int)settings.GetValue("splitContainer_right", true, (int)500);

            this.defaultStyle = (EOStyle)settings.GetValue("Default Style", true, new EOStyle());
            this.toolStrip_main.Visible = (bool)settings.GetValue("Show toolsbar", true, true);
            this.statusStrip1.Visible = (bool)settings.GetValue("Show statusstrip", true, true);
            this.menuStrip1.Visible = (bool)settings.GetValue("Show main menu", true, true);
            bool showOptionsTab = (bool)settings.GetValue("Show launch options tab", true, true);
            splitContainer_left_down.Panel2Collapsed = !showOptionsTab;

            informationContainersBrowser1.ShowHideCloseButtons(
                (bool)settings.GetValue("Show close buttons on tabs", true, true),
                (bool)settings.GetValue("Close buttons always visible on tabs", true, false),
                (bool)settings.GetValue("Show icons on tabs", true, true));
            LoadRecents();
        }
        private void SaveSettings()
        {
            settings.AddValue("Main window location", this.Location);
            settings.AddValue("Main window size", this.Size);

            //settings.AddValue("splitContainer_left", splitContainer_left.SplitterDistance);
            //settings.AddValue("splitContainer_left_down", splitContainer_left_down.SplitterDistance);
            //settings.AddValue("splitContainer_main", splitContainer_main.SplitterDistance);
            //settings.AddValue("splitContainer_right", splitContainer_right.SplitterDistance);

            settings.AddValue("Show launch options tab", !splitContainer_left_down.Panel2Collapsed);
            settings.SaveSettings();
        }
        public void AddRecent(string filePath)
        {
            // Load old collection
            string[] recentProfiles = (string[])settings.GetValue("Recent profiles", true, new string[0]);
            List<string> rec = new List<string>(recentProfiles);
            // If the path already exist, remove it first
            if (rec.Contains(filePath))
                rec.Remove(filePath);
            // Insert new one at the beginning 
            rec.Insert(0, filePath);
            // Limit to 10
            if (rec.Count == 11)
                rec.RemoveAt(10);
            // Save back
            settings.AddValue("Recent profiles", rec.ToArray());
            // Refresh
            LoadRecents();
        }
        private void LoadRecents()
        {
            string[] recentProfiles = (string[])settings.GetValue("Recent profiles", true, new string[0]);
            recentToolStripMenuItem.DropDownItems.Clear();
            foreach (string file in recentProfiles)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = System.IO.Path.GetFileNameWithoutExtension(file);
                item.Tag = item.ToolTipText = file;
                recentToolStripMenuItem.DropDownItems.Add(item);
            }
        }
        public void WriteStatus(string text, Color color)
        {
            if (!this.InvokeRequired)
                WriteStatus1(text, color);
            else
                this.Invoke(new WriteStatusDelegate(WriteStatus1), text, color);
        }
        private void WriteStatus1(string text, Color color)
        {
            StatusLabel1.ForeColor = color;
            StatusLabel1.Text = text;
        }
        private bool IsSaveRequired()
        {
            if (!Save)
                return true;
            ManagedMessageBoxResult result = ManagedMessageBox.ShowMessage(this, ls["Message_DoYouWantToSaveProfile"],
                 ls["MessageCaption_SaveProfile"], new string[] { ls["Button_Save"], ls["Button_DontSave"], ls["Button_Cancel"] }, 0,
                 ManagedMessageBoxIcon.Save);
            switch (result.ClickedButtonIndex)
            {
                case 0:// save
                    {
                        if (profileManager.FilePath == null || profileManager.FilePath == "")
                        {
                            SaveFileDialog saveDialog = new SaveFileDialog();
                            saveDialog.Title = ls["MessageCaption_SaveProfile"];
                            saveDialog.Filter = ls["Filter_EOP"];
                            saveDialog.FileName = profileManager.Profile.Name;
                            if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                ProfileSaveLoadStatus status = profileManager.SaveProfile(saveDialog.FileName, false);
                                if (status.Type == ProfileSaveLaodType.Success)
                                {
                                    return true;
                                }
                                else
                                {
                                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantSaveProfile"] + ":\n" + status.Message,
                                        ls["MessageCaption_SaveProfile"]);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            ProfileSaveLoadStatus status = profileManager.SaveProfile(false);
                            if (status.Type == ProfileSaveLaodType.Success)
                            {
                                Save = false; return true;
                            }
                            else
                            {
                                ManagedMessageBox.ShowErrorMessage(ls["Message_CantSaveProfile"] + ":\n" + status.Message,
                                    ls["MessageCaption_SaveProfile"]);
                                return false;
                            }
                        }
                    }
                    break;
                case 1:// No 
                    { return true; }
            }
            return false;
        }
        private void EnableDisableButtons(IBrowserControl control)
        {
            deleteToolStripMenuItem.Enabled = control.CanDelete;
            renameToolStripMenuItem.Enabled = control.CanRename;
            propertiesToolStripMenuItem.Enabled = control.CanShowProperties;
            clearIconToolStripMenuItem.Enabled = changeIconToolStripMenuItem.Enabled = control.CanChangeIcon;

            romsfolderScanToolStripMenuItem.Enabled =
            romsToolStripMenuItem.Enabled = consolesBrowser.CanAddRoms;

            playToolStripMenuItem1.Enabled = romsBrowser.CanPlayRom;
        }
        private void ApplyStyle(EOStyle style)
        {
            IsLoadingStyle = true;
            if (style == null)
            {
                try
                {
                    if (defaultStyle.mainWindowResize)
                        this.Size = defaultStyle.mainWindowSize;
                    //this.splitContainer_main.Panel1Collapsed = !defaultStyle.mainWindowShowLeftPanel;
                    this.splitContainer_left.SplitterDistance = defaultStyle.mainWindowSplitContainer_left;
                    this.splitContainer_left_down.SplitterDistance = defaultStyle.mainWindowSplitContainer_left_down;
                    this.splitContainer_main.SplitterDistance = defaultStyle.mainWindowSplitContainer_main;
                    this.splitContainer_right.SplitterDistance = defaultStyle.mainWindowSplitContainer_right;
                }
                catch
                {
                }
                consolesBrowser.ApplyStyle(defaultStyle);
                emulatorsBrowser.ApplyStyle(defaultStyle);
                playlistsBrowser.ApplyStyle(defaultStyle);
                filtersBrowser.ApplyStyle(defaultStyle);
                romsBrowser.ApplyStyle(defaultStyle);
                profileStatusBrowser.ApplyStyle(defaultStyle);
                consoleStatusBrowser.ApplyStyle(defaultStyle);
                categoriesBrowser.ApplyStyle(defaultStyle);
                informationContainersBrowser1.ApplyStyle(defaultStyle);
                startOptionsBrowser.ApplyStyle(defaultStyle);

                toolStripContainer1.TopToolStripPanel.BackColor = this.BackColor =
                toolStripTextBox_quickSearch.BackColor = toolStrip_main.BackColor = menuStrip1.BackColor =
                statusStrip1.BackColor =
                MTC_left_top.BackColor = MTC_left_middle.BackColor =
                MTC_left_down.BackColor = MTC_middle.BackColor = defaultStyle.bkgColor_MainWindow;
                menuStrip1.ForeColor = defaultStyle.txtColor_MainWindowMainMenu;
                for (int i = 0; i < menuStrip1.Items.Count; i++)
                {
                    menuStrip1.Items[i].ForeColor = defaultStyle.txtColor_MainWindowMainMenu;
                }
                MTC_left_top.TabPageColor = MTC_left_middle.TabPageColor =
                MTC_left_down.TabPageColor = MTC_middle.TabPageColor = defaultStyle.TabPageColor;
                MTC_left_top.TabPageSelectedColor = MTC_left_middle.TabPageSelectedColor =
                MTC_left_down.TabPageSelectedColor = MTC_middle.TabPageSelectedColor = defaultStyle.TabPageSelectedColor;
                MTC_left_top.TabPageHighlightedColor = MTC_left_middle.TabPageHighlightedColor =
                MTC_left_down.TabPageHighlightedColor = MTC_middle.TabPageHighlightedColor = defaultStyle.TabPageHighlightedColor;
                MTC_left_top.TabPageSplitColor = MTC_left_middle.TabPageSplitColor =
                MTC_left_down.TabPageSplitColor = MTC_middle.TabPageSplitColor = defaultStyle.TabPageSplitColor;
                MTC_left_top.ForeColor = MTC_left_middle.ForeColor =
                MTC_left_down.ForeColor = MTC_middle.ForeColor =
                defaultStyle.TabPageTextsColor;
                ManagedMessageBox.DefaultBackgroundColor = defaultStyle.bkgColor_MainWindow;
            }
            else
            {
                try
                {
                    if (defaultStyle.mainWindowResize)
                        this.Size = style.mainWindowSize;
                    this.splitContainer_main.Panel1Collapsed = style.mainWindowHideLeftPanel;
                    this.splitContainer_left.SplitterDistance = style.mainWindowSplitContainer_left;
                    this.splitContainer_left_down.SplitterDistance = style.mainWindowSplitContainer_left_down;
                    this.splitContainer_main.SplitterDistance = style.mainWindowSplitContainer_main;
                    this.splitContainer_right.SplitterDistance = style.mainWindowSplitContainer_right;
                }
                catch
                {
                }
                consolesBrowser.ApplyStyle(style);
                emulatorsBrowser.ApplyStyle(style);
                playlistsBrowser.ApplyStyle(style);
                filtersBrowser.ApplyStyle(style);
                romsBrowser.ApplyStyle(style);
                profileStatusBrowser.ApplyStyle(style);
                consoleStatusBrowser.ApplyStyle(style);
                categoriesBrowser.ApplyStyle(style);
                informationContainersBrowser1.ApplyStyle(style);
                startOptionsBrowser.ApplyStyle(style);

                toolStripContainer1.TopToolStripPanel.BackColor = menuStrip1.BackColor = toolStrip_main.BackColor =
                toolStripTextBox_quickSearch.BackColor = statusStrip1.BackColor = this.BackColor = MTC_left_top.BackColor = MTC_left_middle.BackColor =
                MTC_left_down.BackColor = MTC_middle.BackColor = style.bkgColor_MainWindow;
                menuStrip1.ForeColor = style.txtColor_MainWindowMainMenu;
                for (int i = 0; i < menuStrip1.Items.Count; i++)
                {
                    menuStrip1.Items[i].ForeColor = style.txtColor_MainWindowMainMenu;
                }
                MTC_left_top.TabPageColor = MTC_left_middle.TabPageColor =
                MTC_left_down.TabPageColor = MTC_middle.TabPageColor = style.TabPageColor;
                MTC_left_top.TabPageSelectedColor = MTC_left_middle.TabPageSelectedColor =
                MTC_left_down.TabPageSelectedColor = MTC_middle.TabPageSelectedColor = style.TabPageSelectedColor;
                MTC_left_top.TabPageHighlightedColor = MTC_left_middle.TabPageHighlightedColor =
                MTC_left_down.TabPageHighlightedColor = MTC_middle.TabPageHighlightedColor = style.TabPageHighlightedColor;
                MTC_left_top.TabPageSplitColor = MTC_left_middle.TabPageSplitColor =
                MTC_left_down.TabPageSplitColor = MTC_middle.TabPageSplitColor = style.TabPageSplitColor;
                MTC_left_top.ForeColor = MTC_left_middle.ForeColor =
                MTC_left_down.ForeColor = MTC_middle.ForeColor = style.TabPageTextsColor;
                ManagedMessageBox.DefaultBackgroundColor = style.bkgColor_MainWindow;
            }
            IsLoadingStyle = false;
        }
        private void SaveSizesIntoStyle()
        {
            if (IsLoadingStyle) return;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSize = this.Size;
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowHideLeftPanel = this.splitContainer_main.Panel1Collapsed;
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_left = this.splitContainer_left.SplitterDistance;
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_left_down = this.splitContainer_left_down.SplitterDistance;
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_main = this.splitContainer_main.SplitterDistance;
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_right = this.splitContainer_right.SplitterDistance;
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSize = this.Size;
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowHideLeftPanel = this.splitContainer_main.Panel1Collapsed;
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_left = this.splitContainer_left.SplitterDistance;
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_left_down = this.splitContainer_left_down.SplitterDistance;
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_main = this.splitContainer_main.SplitterDistance;
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_right = this.splitContainer_right.SplitterDistance;
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSize = this.Size;
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowHideLeftPanel = this.splitContainer_main.Panel1Collapsed;
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_left = this.splitContainer_left.SplitterDistance;
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_left_down = this.splitContainer_left_down.SplitterDistance;
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_main = this.splitContainer_main.SplitterDistance;
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_right = this.splitContainer_right.SplitterDistance;
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSize = this.Size;
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowHideLeftPanel = this.splitContainer_main.Panel1Collapsed;
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_left = this.splitContainer_left.SplitterDistance;
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_left_down = this.splitContainer_left_down.SplitterDistance;
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_main = this.splitContainer_main.SplitterDistance;
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_right = this.splitContainer_right.SplitterDistance;
                        break;
                    }
            }
        }
        private void OnProgress(string status, int precent)
        {
            Bar1.Value = precent;
            StatusLabel1.Text = status;
        }
        private void HideStuffForSave()
        {
            // Called on save started.
            // Show status
            Bar1.Visible = true;
            StatusLabel1.Visible = true;
            statusStrip1.Refresh();
            // We don't need the main menu anymore ...
            menuStrip1.Enabled = false;
            // We still can use few buttons on the toolbar, so hide the ones we can't use
            toolStripButton1.Enabled =
                toolStripButton2.Enabled =
                toolStripSplitButton1.Enabled =
                toolStripButton3.Enabled =
                toolStripButton4.Enabled =
                toolStripButton5.Enabled =
                toolStripButton8.Enabled =
                toolStripButton7.Enabled =
                toolStripButton9.Enabled =
                toolStripSplitButton4.Enabled =
                toolStripButton10.Enabled =
                toolStripButton12.Enabled = false;
        }
        private void ShowStuffAfterSave()
        {
            // Called on save finished.
            Bar1.Visible = false;

            menuStrip1.Enabled = true;
            toolStripButton1.Enabled =
                toolStripButton2.Enabled =
                toolStripSplitButton1.Enabled =
                toolStripButton3.Enabled =
                toolStripButton4.Enabled =
                toolStripButton5.Enabled =
                toolStripButton8.Enabled =
                toolStripButton7.Enabled =
                toolStripButton9.Enabled =
                toolStripSplitButton4.Enabled =
                toolStripButton10.Enabled =
                toolStripButton12.Enabled = true;
            EnableDisableButtons(romsBrowser);
        }
        #region Initialize
        private void InitializeControls()
        {
            #region Left Panel
            // LEFT TOP
            MTC_left_top.AllowTabPageDragAndDrop = false;
            MTC_left_top.AllowTabPagesReorder = false;
            MTC_left_top.CloseBoxAlwaysVisible = false;
            MTC_left_top.CloseBoxOnEachPageVisible = false;
            MTC_left_top.DrawStyle = MTCDrawStyle.Flat;
            MTC_left_top.ImagesList = imageList_tabs;
            InitializeTopLeftTabControls();

            // LEFT MIDDLE
            MTC_left_middle.AllowTabPageDragAndDrop = false;
            MTC_left_middle.AllowTabPagesReorder = false;
            MTC_left_middle.CloseBoxAlwaysVisible = false;
            MTC_left_middle.CloseBoxOnEachPageVisible = false;
            MTC_left_middle.DrawStyle = MTCDrawStyle.Flat;
            MTC_left_middle.ImagesList = imageList_tabs;
            InitializeMiddleLeftTabControls();

            // LEFT DOWN
            MTC_left_down.AllowTabPageDragAndDrop = false;
            MTC_left_down.AllowTabPagesReorder = false;
            MTC_left_down.CloseBoxAlwaysVisible = false;
            MTC_left_down.CloseBoxOnEachPageVisible = true;
            MTC_left_down.DrawStyle = MTCDrawStyle.Flat;
            MTC_left_down.ImagesList = imageList_tabs;
            InitializeDownLeftTabControls();
            #endregion
            #region Midd Panel
            MTC_middle.AllowTabPageDragAndDrop = false;
            MTC_middle.AllowTabPagesReorder = false;
            MTC_middle.CloseBoxAlwaysVisible = false;
            MTC_middle.CloseBoxOnEachPageVisible = false;
            MTC_middle.DrawStyle = MTCDrawStyle.Flat;
            MTC_middle.ImagesList = imageList_tabs;
            InitializeMiddleControls();
            #endregion
            #region Right Panel
            informationContainersBrowser1 = new InformationContainersBrowser();
            splitContainer_right.Panel2.Controls.Add(informationContainersBrowser1);
            informationContainersBrowser1.Dock = DockStyle.Fill;
            #endregion
        }
        private void InitializeMiddleControls()
        {
            MTCTabPage page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_Roms"];
            romsBrowser = new RomsBrowser();
            romsBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(romsBrowser);
            page.ImageIndex = 4;
            romsBrowser.Dock = DockStyle.Fill;
            MTC_middle.TabPages.Add(page);

            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_ProfileStatus"];
            profileStatusBrowser = new ProfileStatusBrowser(MTC_middle);
            profileStatusBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(profileStatusBrowser);
            page.ImageIndex = 7;
            profileStatusBrowser.Dock = DockStyle.Fill;
            MTC_middle.TabPages.Add(page);

            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_ConsoleStatus"];
            consoleStatusBrowser = new ConsoleStatusBrowser(MTC_middle);
            consoleStatusBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(consoleStatusBrowser);
            page.ImageIndex = 8;
            consoleStatusBrowser.Dock = DockStyle.Fill;
            MTC_middle.TabPages.Add(page);
        }
        private void InitializeTopLeftTabControls()
        {
            // Add consoles browser
            MTCTabPage page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_Consoles"];
            consolesBrowser = new ConsolesBrowser();
            consolesBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(consolesBrowser);
            consolesBrowser.Dock = DockStyle.Fill;
            page.ImageIndex = 0;
            MTC_left_top.TabPages.Add(page);

            // Add playlists browser
            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_Playlists"];
            playlistsBrowser = new PlaylistsBrowser();
            playlistsBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(playlistsBrowser);
            playlistsBrowser.Dock = DockStyle.Fill;
            page.ImageIndex = 1;
            MTC_left_top.TabPages.Add(page);
        }
        private void InitializeDownLeftTabControls()
        {
            // Add Start options control
            MTCTabPage page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tabs_LaunchOptions"];
            page.ID = "launch options";
            startOptionsBrowser = new StartOptionsPanelBrowser();
            startOptionsBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(startOptionsBrowser);
            page.ImageIndex = 6;
            startOptionsBrowser.Dock = DockStyle.Fill;

            MTC_left_down.TabPages.Add(page);
        }
        private void InitializeMiddleLeftTabControls()
        {
            // Add emulators browser
            MTCTabPage page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_Emulators"];
            emulatorsBrowser = new EmulatorsBrowser();
            emulatorsBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(emulatorsBrowser);
            page.ImageIndex = 2;
            emulatorsBrowser.Dock = DockStyle.Fill;

            MTC_left_middle.TabPages.Add(page);

            // Add categories browser
            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_Categories"];
            categoriesBrowser = new CategoiresBrowser();
            categoriesBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(categoriesBrowser);
            page.ImageIndex = 5;
            categoriesBrowser.Dock = DockStyle.Fill;

            MTC_left_middle.TabPages.Add(page);
            // Add filters browser
            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.TextAndImage;
            page.Text = ls["Tab_Filters"];
            filtersBrowser = new FiltersBrowser();
            filtersBrowser.LoadControlSettings();
            page.Panel = new Panel();
            page.Panel.Controls.Add(filtersBrowser);
            page.ImageIndex = 3;
            filtersBrowser.Dock = DockStyle.Fill;

            MTC_left_middle.TabPages.Add(page);
        }
        private void InitializeEvents()
        {
            /*Controls*/
            consolesBrowser.Enter += consolesBrowser_Enter;
            playlistsBrowser.Enter += playlistsBrowser_Enter;
            filtersBrowser.Enter += filtersBrowser_Enter;
            romsBrowser.Enter += romsBrowser_Enter;
            profileStatusBrowser.Enter += profileStatusBrowser_Enter;
            consoleStatusBrowser.Enter += consoleStatusBrowser_Enter;
            emulatorsBrowser.Enter += emulatorsBrowser_Enter;
            categoriesBrowser.Enter += categoriesBrowser_Enter;
            informationContainersBrowser1.Enter += informationContainersBrowser1_Enter;
            startOptionsBrowser.Enter += startOptionsBrowser_Enter;

            consolesBrowser.EnableDisableButtons += EnableDisableButtonsHandle;
            playlistsBrowser.EnableDisableButtons += EnableDisableButtonsHandle;
            filtersBrowser.EnableDisableButtons += EnableDisableButtonsHandle;
            romsBrowser.EnableDisableButtons += EnableDisableButtonsHandle;
            emulatorsBrowser.EnableDisableButtons += EnableDisableButtonsHandle;
            categoriesBrowser.EnableDisableButtons += EnableDisableButtonsHandle;
            informationContainersBrowser1.EnableDisableButtons += EnableDisableButtonsHandle;
            startOptionsBrowser.EnableDisableButtons += EnableDisableButtonsHandle;
            informationContainersBrowser1.SaveRequest += Profile_SetSave;

            consolesBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            consolesBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            consolesBrowser.Progress += consolesBrowser_Progress;
            playlistsBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            playlistsBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            playlistsBrowser.Progress += consolesBrowser_Progress;
            filtersBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            filtersBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            filtersBrowser.Progress += consolesBrowser_Progress;
            romsBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            romsBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            romsBrowser.Progress += consolesBrowser_Progress;
            profileStatusBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            profileStatusBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            profileStatusBrowser.Progress += consolesBrowser_Progress;
            consoleStatusBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            consoleStatusBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            consoleStatusBrowser.Progress += consolesBrowser_Progress;
            emulatorsBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            emulatorsBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            emulatorsBrowser.Progress += consolesBrowser_Progress;
            categoriesBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            categoriesBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            categoriesBrowser.Progress += consolesBrowser_Progress;
            informationContainersBrowser1.ProgressStarted += consolesBrowser_ProgressStarted;
            informationContainersBrowser1.ProgressFinished += consolesBrowser_ProgressFinished;
            informationContainersBrowser1.Progress += consolesBrowser_Progress;
            startOptionsBrowser.ProgressStarted += consolesBrowser_ProgressStarted;
            startOptionsBrowser.ProgressFinished += consolesBrowser_ProgressFinished;
            startOptionsBrowser.Progress += consolesBrowser_Progress;
            /*Profile*/
            profileManager.ProfileSavingStarted += ProfileManager_ProfileSavingStarted;
            profileManager.ProfileSavingFinished += ProfileManager_ProfileSavingFinished;
            profileManager.Progress += ProfileManager_Progress;
            profileManager.Profile.ElementIconChanged += Profile_SetSave;
            /*Console Groups and Consoles*/
            profileManager.Profile.ConoslesGroupAdded += Profile_ConosleAdded;
            profileManager.Profile.ConoslesGroupRemoved += Profile_SetSave;
            profileManager.Profile.ConoslesGroupsCleared += Profile_SetSave;
            profileManager.Profile.ConsoleAdded += Profile_ConosleAdded;
            profileManager.Profile.ConsoleRemoved += Profile_SetSave;
            profileManager.Profile.ConoslesCleared += Profile_SetSave;
            profileManager.Profile.ConoslesGroupsRenamed += Profile_SetSave;
            profileManager.Profile.ConosleRenamed += Profile_SetSave;
            profileManager.Profile.ConoslesGroupsMoved += Profile_SetSave;
            profileManager.Profile.ConsoleMoved += Profile_SetSave;
            profileManager.Profile.ConoslesGroupsSorted += Profile_SetSave;
            profileManager.Profile.ConsolesSorted += Profile_SetSave;
            profileManager.Profile.ConsoleColumnResized += Profile_SetSave;
            profileManager.Profile.ConsoleColumnReorder += Profile_SetSave;
            profileManager.Profile.ConsoleColumnVisibleChanged += Profile_SetSave;
            profileManager.Profile.ConsolesGroupColumnVisibleChanged += Profile_SetSave;
            profileManager.Profile.ConsolesGroupColumnResized += Profile_SetSave;
            profileManager.Profile.ConsolesGroupColumnReorder += Profile_SetSave;
            profileManager.Profile.ConsolePropertiesChanged += Profile_ConsolePropertiesChanged;
            profileManager.Profile.ConsoleSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupPropertiesChanged += Profile_ConsolePropertiesChanged;
            /*Playlist Groups and Playlists*/
            profileManager.Profile.PlaylistsGroupAdded += Profile_PlaylistsGroupAdded;
            profileManager.Profile.PlaylistsGroupRemoved += Profile_SetSave;
            profileManager.Profile.PlaylistGroupsCleared += Profile_SetSave;
            profileManager.Profile.PlaylistAdded += Profile_PlaylistsGroupAdded;
            profileManager.Profile.PlaylistRemoved += Profile_SetSave;
            profileManager.Profile.PlaylistsCleared += Profile_SetSave;
            profileManager.Profile.PlaylistGroupsRenamed += Profile_SetSave;
            profileManager.Profile.PlaylistRenamed += Profile_SetSave;
            profileManager.Profile.PlaylistsGroupMoved += Profile_SetSave;
            profileManager.Profile.PlaylistMoved += Profile_SetSave;
            profileManager.Profile.PlaylistGroupsSorted += Profile_SetSave;
            profileManager.Profile.PlaylistsSorted += Profile_SetSave;
            profileManager.Profile.PlaylistColumnResized += Profile_SetSave;
            profileManager.Profile.PlaylistColumnReorder += Profile_SetSave;
            profileManager.Profile.PlaylistColumnVisibleChanged += Profile_SetSave;
            profileManager.Profile.PlaylistsGroupColumnVisibleChanged += Profile_SetSave;
            profileManager.Profile.PlaylistsGroupColumnResized += Profile_SetSave;
            profileManager.Profile.PlaylistsGroupColumnReorder += Profile_SetSave;
            profileManager.Profile.PlaylistPropertiesChanged += Profile_ConsolePropertiesChanged;
            profileManager.Profile.PlaylistSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupPropertiesChanged += Profile_ConsolePropertiesChanged;
            profileManager.Profile.PlaylistRomsReorder += Profile_SetSave;
            /*Roms*/
            profileManager.Profile.RomAdded += Profile_SetSave;
            profileManager.Profile.RomMoved += Profile_SetSave;
            profileManager.Profile.RomRatingChanged += Profile_SetSave;
            profileManager.Profile.RomFinishedPlayed += Profile_SetSave;
            profileManager.Profile.RomRemoved += Profile_SetSave;
            profileManager.Profile.RomsCleared += Profile_SetSave;
            profileManager.Profile.RomsSorted += Profile_SetSave;
            profileManager.Profile.RomsAdded += Profile_SetSave;
            profileManager.Profile.RomsRemoved += Profile_SetSave;
            profileManager.Profile.RomIconsChanged += Profile_SetSave;
            profileManager.Profile.RomRenamed += Profile_SetSave;
            profileManager.Profile.RomsAddedToPlaylist += Profile_SetSave;
            profileManager.Profile.RomsRemovedFromPlaylist += Profile_SetSave;
            profileManager.Profile.RomPropertiesChanged += Profile_SetSave;
            profileManager.Profile.RomMultiplePropertiesChanged += Profile_SetSave;
            /*Emulators*/
            profileManager.Profile.EmulatorAdded += Profile_SetSave;
            profileManager.Profile.EmulatorMoved += Profile_SetSave;
            profileManager.Profile.EmulatorRemoved += Profile_SetSave;
            profileManager.Profile.EmulatorRenamed += Profile_SetSave;
            profileManager.Profile.EmulatorsCleared += Profile_SetSave;
            profileManager.Profile.EmulatorsSorted += Profile_SetSave;
            profileManager.Profile.EmulatorPropertiesChanged += Profile_SetSave;
            /*OTHERS*/
            profileManager.Profile.MainWindowMinimize += Profile_MainWindowMinimize;
            profileManager.Profile.MainWindowReturnToNormal += Profile_MainWindowReturnToNormal;
            profileManager.Profile.FilterAdded += Profile_SetSave;
            profileManager.Profile.FilterRemoved += Profile_SetSave;
            profileManager.Profile.FilterEdit += Profile_SetSave;
            profileManager.Profile.InformationContainerAdded += Profile_SetSave;
            profileManager.Profile.InformationContainerRemoved += Profile_SetSave;
            profileManager.Profile.InformationContainerVisibiltyChanged += Profile_SetSave;
            profileManager.Profile.InformationContainerItemsDetected += Profile_SetSave;
            profileManager.Profile.InformationContainerItemsModified += Profile_SetSave;
            profileManager.Profile.InformationContainerMoved += Profile_SetSave;
            profileManager.Profile.ProfileCleanUpFinished += Profile_SetSave;
            profileManager.Profile.DatabaseImported += Profile_SetSave;
        }
        private void DisposeEvents()
        {
            /*Controls*/
            consolesBrowser.Enter -= consolesBrowser_Enter;
            playlistsBrowser.Enter -= playlistsBrowser_Enter;
            filtersBrowser.Enter -= filtersBrowser_Enter;
            romsBrowser.Enter -= romsBrowser_Enter;
            emulatorsBrowser.Enter -= emulatorsBrowser_Enter;
            categoriesBrowser.Enter -= categoriesBrowser_Enter;
            informationContainersBrowser1.Enter -= informationContainersBrowser1_Enter;
            startOptionsBrowser.Enter -= startOptionsBrowser_Enter;

            consolesBrowser.EnableDisableButtons -= EnableDisableButtonsHandle;
            playlistsBrowser.EnableDisableButtons -= EnableDisableButtonsHandle;
            filtersBrowser.EnableDisableButtons -= EnableDisableButtonsHandle;
            romsBrowser.EnableDisableButtons -= EnableDisableButtonsHandle;
            emulatorsBrowser.EnableDisableButtons -= EnableDisableButtonsHandle;
            categoriesBrowser.EnableDisableButtons -= EnableDisableButtonsHandle;
            informationContainersBrowser1.EnableDisableButtons -= EnableDisableButtonsHandle;
            startOptionsBrowser.EnableDisableButtons -= EnableDisableButtonsHandle;
            informationContainersBrowser1.SaveRequest -= Profile_SetSave;

            consolesBrowser.ProgressStarted -= consolesBrowser_ProgressStarted;
            consolesBrowser.ProgressFinished -= consolesBrowser_ProgressFinished;
            consolesBrowser.Progress -= consolesBrowser_Progress;
            playlistsBrowser.ProgressStarted -= consolesBrowser_ProgressStarted;
            playlistsBrowser.ProgressFinished -= consolesBrowser_ProgressFinished;
            playlistsBrowser.Progress -= consolesBrowser_Progress;
            filtersBrowser.ProgressStarted -= consolesBrowser_ProgressStarted;
            filtersBrowser.ProgressFinished -= consolesBrowser_ProgressFinished;
            filtersBrowser.Progress -= consolesBrowser_Progress;
            romsBrowser.ProgressStarted -= consolesBrowser_ProgressStarted;
            romsBrowser.ProgressFinished -= consolesBrowser_ProgressFinished;
            romsBrowser.Progress -= consolesBrowser_Progress;
            emulatorsBrowser.ProgressStarted -= consolesBrowser_ProgressStarted;
            emulatorsBrowser.ProgressFinished -= consolesBrowser_ProgressFinished;
            emulatorsBrowser.Progress -= consolesBrowser_Progress;
            categoriesBrowser.ProgressStarted -= consolesBrowser_ProgressStarted;
            categoriesBrowser.ProgressFinished -= consolesBrowser_ProgressFinished;
            categoriesBrowser.Progress -= consolesBrowser_Progress;
            informationContainersBrowser1.ProgressStarted -= consolesBrowser_ProgressStarted;
            informationContainersBrowser1.ProgressFinished -= consolesBrowser_ProgressFinished;
            informationContainersBrowser1.Progress -= consolesBrowser_Progress;
            startOptionsBrowser.ProgressStarted -= consolesBrowser_ProgressStarted;
            startOptionsBrowser.ProgressFinished -= consolesBrowser_ProgressFinished;
            startOptionsBrowser.Progress -= consolesBrowser_Progress;
            /*Profile*/
            //profileManager.NewProfileCreated -= profileManager_NewProfileCreated;
            //profileManager.ProfileOpened -= profileManager_ProfileOpened;
            //profileManager.Profile.ElementIconChanged -= Profile_SetSave;
            /*Console Groups and Consoles*/
            profileManager.Profile.ConoslesGroupAdded -= Profile_ConosleAdded;
            profileManager.Profile.ConoslesGroupRemoved -= Profile_SetSave;
            profileManager.Profile.ConoslesGroupsCleared -= Profile_SetSave;
            profileManager.Profile.ConsoleAdded -= Profile_ConosleAdded;
            profileManager.Profile.ConsoleRemoved -= Profile_SetSave;
            profileManager.Profile.ConoslesCleared -= Profile_SetSave;
            profileManager.Profile.ConoslesGroupsRenamed -= Profile_SetSave;
            profileManager.Profile.ConosleRenamed -= Profile_SetSave;
            profileManager.Profile.ConoslesGroupsMoved -= Profile_SetSave;
            profileManager.Profile.ConsoleMoved -= Profile_SetSave;
            profileManager.Profile.ConoslesGroupsSorted -= Profile_SetSave;
            profileManager.Profile.ConsolesSorted -= Profile_SetSave;
            profileManager.Profile.ConsoleColumnResized -= Profile_SetSave;
            profileManager.Profile.ConsoleColumnReorder -= Profile_SetSave;
            profileManager.Profile.ConsoleColumnVisibleChanged -= Profile_SetSave;
            profileManager.Profile.ConsolesGroupColumnVisibleChanged -= Profile_SetSave;
            profileManager.Profile.ConsolesGroupColumnResized -= Profile_SetSave;
            profileManager.Profile.ConsolesGroupColumnReorder -= Profile_SetSave;
            profileManager.Profile.ConsolePropertiesChanged -= Profile_ConsolePropertiesChanged;
            profileManager.Profile.ConsoleSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupPropertiesChanged -= Profile_ConsolePropertiesChanged;
            /*Playlist Groups and Playlists*/
            profileManager.Profile.PlaylistsGroupAdded -= Profile_PlaylistsGroupAdded;
            profileManager.Profile.PlaylistsGroupRemoved -= Profile_SetSave;
            profileManager.Profile.PlaylistGroupsCleared -= Profile_SetSave;
            profileManager.Profile.PlaylistAdded -= Profile_PlaylistsGroupAdded;
            profileManager.Profile.PlaylistRemoved -= Profile_SetSave;
            profileManager.Profile.PlaylistsCleared -= Profile_SetSave;
            profileManager.Profile.PlaylistGroupsRenamed -= Profile_SetSave;
            profileManager.Profile.PlaylistRenamed -= Profile_SetSave;
            profileManager.Profile.PlaylistsGroupMoved -= Profile_SetSave;
            profileManager.Profile.PlaylistMoved -= Profile_SetSave;
            profileManager.Profile.PlaylistGroupsSorted -= Profile_SetSave;
            profileManager.Profile.PlaylistsSorted -= Profile_SetSave;
            profileManager.Profile.PlaylistColumnResized -= Profile_SetSave;
            profileManager.Profile.PlaylistColumnReorder -= Profile_SetSave;
            profileManager.Profile.PlaylistColumnVisibleChanged -= Profile_SetSave;
            profileManager.Profile.PlaylistsGroupColumnVisibleChanged -= Profile_SetSave;
            profileManager.Profile.PlaylistsGroupColumnResized -= Profile_SetSave;
            profileManager.Profile.PlaylistsGroupColumnReorder -= Profile_SetSave;
            profileManager.Profile.PlaylistPropertiesChanged -= Profile_ConsolePropertiesChanged;
            profileManager.Profile.PlaylistSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupPropertiesChanged -= Profile_ConsolePropertiesChanged;
            profileManager.Profile.PlaylistRomsReorder -= Profile_SetSave;
            /*Roms*/
            profileManager.Profile.RomAdded -= Profile_SetSave;
            profileManager.Profile.RomMoved -= Profile_SetSave;
            profileManager.Profile.RomRatingChanged -= Profile_SetSave;
            profileManager.Profile.RomFinishedPlayed -= Profile_SetSave;
            profileManager.Profile.RomRemoved -= Profile_SetSave;
            profileManager.Profile.RomsCleared -= Profile_SetSave;
            profileManager.Profile.RomsSorted -= Profile_SetSave;
            profileManager.Profile.RomsAdded -= Profile_SetSave;
            profileManager.Profile.RomsRemoved -= Profile_SetSave;
            profileManager.Profile.RomIconsChanged -= Profile_SetSave;
            profileManager.Profile.RomRenamed -= Profile_SetSave;
            profileManager.Profile.RomsAddedToPlaylist -= Profile_SetSave;
            profileManager.Profile.RomsRemovedFromPlaylist -= Profile_SetSave;
            profileManager.Profile.RomPropertiesChanged -= Profile_SetSave;
            profileManager.Profile.RomMultiplePropertiesChanged -= Profile_SetSave;
            /*Emulators*/
            profileManager.Profile.EmulatorAdded -= Profile_SetSave;
            profileManager.Profile.EmulatorMoved -= Profile_SetSave;
            profileManager.Profile.EmulatorRemoved -= Profile_SetSave;
            profileManager.Profile.EmulatorRenamed -= Profile_SetSave;
            profileManager.Profile.EmulatorsCleared -= Profile_SetSave;
            profileManager.Profile.EmulatorsSorted -= Profile_SetSave;
            profileManager.Profile.EmulatorPropertiesChanged -= Profile_SetSave;
            /*OTHERS*/
            profileManager.Profile.MainWindowMinimize -= Profile_MainWindowMinimize;
            profileManager.Profile.MainWindowReturnToNormal -= Profile_MainWindowReturnToNormal;
            profileManager.Profile.FilterAdded -= Profile_SetSave;
            profileManager.Profile.FilterRemoved -= Profile_SetSave;
            profileManager.Profile.FilterEdit -= Profile_SetSave;
            profileManager.Profile.InformationContainerAdded -= Profile_SetSave;
            profileManager.Profile.InformationContainerRemoved -= Profile_SetSave;
            profileManager.Profile.InformationContainerVisibiltyChanged -= Profile_SetSave;
            profileManager.Profile.InformationContainerItemsDetected -= Profile_SetSave;
            profileManager.Profile.InformationContainerItemsModified -= Profile_SetSave;
            profileManager.Profile.InformationContainerMoved -= Profile_SetSave;
            profileManager.Profile.ProfileCleanUpFinished -= Profile_SetSave;
            profileManager.Profile.DatabaseImported -= Profile_SetSave;
        }
        #endregion

        /*Properties*/
        public bool Save
        {
            get { return save; }
            set
            {
                save = value;
                if (!this.InvokeRequired)
                    SetSaveText();
                else
                    this.Invoke(new Action(SetSaveText));
            }
        }
        private void SetSaveText()
        {
            this.Text = profileManager.Profile.Name + (save ? "*" : "") + " - Emulators Organizer";
        }
        //For styles
        private void Profile_ConsolePropertiesChanged(object sender, EventArgs e)
        {
            Save = true;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        ApplyStyle(profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style);
                        /*Enable/disable emulator and commandlines*/
                        EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].EnableEmulator;
                        CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].EnableCommandlines;
                        toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = true;
                        toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].ParentAndChildrenMode;
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        ConsolesGroup gr = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        if (gr != null)
                        {
                            ApplyStyle(profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style);
                            /*Enable/disable emulator and commandlines*/
                            EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].EnableEmulator;
                            CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].EnableCommandlines;

                            toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = false;
                            toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = false;
                        }
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        ApplyStyle(profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style);
                        /*Enable/disable emulator and commandlines*/
                        EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].EnableEmulator;
                        CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].EnableCommandlines;
                        toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = false;
                        toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = false;
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        ApplyStyle(profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style);
                        /*Enable/disable emulator and commandlines*/
                        EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].EnableEmulator;
                        CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].EnableCommandlines;
                        toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = false;
                        toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = false;
                        break;
                    }
                default: ApplyStyle(null); break;
            }
        }
        //For styles
        private void Profile_ConsoleSelectionChanged(object sender, EventArgs e)
        {
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        ApplyStyle(profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style);
                        /*Enable/disable emulator and commandlines*/
                        EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].EnableEmulator;
                        CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].EnableCommandlines;
                        toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].ParentAndChildrenMode;
                        toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = true;
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        ApplyStyle(profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style);
                        /*Enable/disable emulator and commandlines*/
                        EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].EnableEmulator;
                        CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].EnableCommandlines;
                        toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = false;
                        toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = false;
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        ApplyStyle(profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style);
                        /*Enable/disable emulator and commandlines*/
                        EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].EnableEmulator;
                        CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].EnableCommandlines;
                        toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = false;
                        toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = false;
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        ApplyStyle(profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style);
                        /*Enable/disable emulator and commandlines*/
                        EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].EnableEmulator;
                        CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].EnableCommandlines;
                        toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked = false;
                        toolStripButton_parentMode.Enabled = parentAndChildrenModeToolStripMenuItem.Enabled = false;
                        break;
                    }
                default: ApplyStyle(null); break;
            }
        }
        private void EnableDisableButtonsHandle(object sender, EventArgs e)
        {
            EnableDisableButtons((IBrowserControl)sender);
        }
        private void profileManager_CreatingNewProfile(object sender, EventArgs e)
        {
            DisposeEvents();
        }
        private void profileManager_NewProfileCreated(object sender, EventArgs e)
        {
            InitializeEvents();
            // Apply default style
            ApplyStyle(null);
            Save = false;
        }
        private void profileManager_ProfileOpened(object sender, EventArgs e)
        {
            InitializeEvents();
            // Apply default style
            ApplyStyle(null);
            Save = false;
            if (profileManager.Profile.RememberLatestSelectedConsoleOnProfileOpen)
            {
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.Console:
                    case SelectionType.ConsolesGroup:
                        {
                            MTC_left_top.SelectedTabPageIndex = 0;
                            break;
                        }
                    case SelectionType.Playlist:
                    case SelectionType.PlaylistsGroup:
                        {
                            MTC_left_top.SelectedTabPageIndex = 1;
                            break;
                        }
                }
            }
            // Apply element style !
            Profile_ConsolePropertiesChanged(this, null);
        }
        private void profileManager_EnableStyleSave(object sender, EventArgs e)
        {
            IsLoadingStyle = false;
        }
        private void profileManager_DisableStyleSave(object sender, EventArgs e)
        {
            IsLoadingStyle = true;
        }
        private void emulatorsBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.Emulators;
        }
        private void romsBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.Roms;
        }
        private void profileStatusBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.ProfileStatus;
        }
        private void consoleStatusBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.ConsoleStatus;
        }
        private void filtersBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.Filters;
        }
        private void categoriesBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.Categories;
        }
        private void informationContainersBrowser1_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.InfoTabs;
            EnableDisableButtons(informationContainersBrowser1);// this will disable all buttons
        }
        private void startOptionsBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.LaunchOptions;
            EnableDisableButtons(startOptionsBrowser);// this will disable all buttons
        }
        private void playlistsBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.Playlists;
        }
        private void consolesBrowser_Enter(object sender, EventArgs e)
        {
            activeTab = ActiveTab.Consoles;
        }
        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (profileManager.IsSaving)
            {
                ManagedMessageBox.ShowMessage("Profile is saving !! please wait.");
                e.Cancel = true;
                return;
            }
            if (IsSaveRequired())
            {
                SaveSettings();
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Profile_SetSave(object sender, EventArgs e)
        {
            Save = true;
        }
        private void consolesBrowser_Progress(object sender, ProgressArg e)
        {
            StatusLabel1.Text = e.Status;
            Bar1.Value = e.Completed;
            statusStrip1.Refresh();
        }
        private void consolesBrowser_ProgressFinished(object sender, EventArgs e)
        {
            Bar1.Visible = false;
        }
        private void consolesBrowser_ProgressStarted(object sender, EventArgs e)
        {
            Bar1.Visible = true;
        }
        private void consolesBrowser_RequestAddRomsFolderScan(object sender, EventArgs e)
        {
            consolesBrowser.AddRomsFolderScan();
        }
        private void consolesBrowser_RequestAddRoms(object sender, EventArgs e)
        {
            consolesBrowser.AddRoms();
        }
        private void Profile_PlaylistsGroupAdded(object sender, EventArgs e)
        {
            Save = true;
            if (MTC_left_top.SelectedTabPageIndex != 1)
                MTC_left_top.SelectedTabPageIndex = 1;
        }
        private void Profile_ConosleAdded(object sender, EventArgs e)
        {
            Save = true;
            if (MTC_left_top.SelectedTabPageIndex != 0)
                MTC_left_top.SelectedTabPageIndex = 0;
        }
        private void Profile_MainWindowReturnToNormal(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
                ReturnToNormalState();
            else
                this.Invoke(new Action(ReturnToNormalState));
        }
        private void ReturnToNormalState()
        {
            this.WindowState = this.originalWindowState;
            this.Activate();
        }
        private void Profile_MainWindowMinimize(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
                MinimizeState();
            else
                this.Invoke(new Action(MinimizeState));
        }
        private void MinimizeState()
        {
            originalWindowState = this.WindowState;
            this.WindowState = FormWindowState.Minimized;
        }
        /*Profile stuff*/
        private void NewProfile(object sender, EventArgs e)
        {
            if (!IsSaveRequired())
                return;
            profileManager.NewProfile();
        }
        private void OpenProfile(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = ls["MessageCaption_OpenProfile"];
            openDialog.Filter = ls["Filter_EOP"];
            if (openDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (!IsSaveRequired())
                    return;
                ProfileSaveLoadStatus status = profileManager.LoadProfile(openDialog.FileName);
                if (status.Type == ProfileSaveLaodType.Success)
                {
                    Save = false;
                    AddRecent(openDialog.FileName);
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantOpenProfile"] + ":\n" + status.Message,
                        ls["MessageCaption_OpenProfile"]);
                }
            }
        }
        private void SaveProfile(object sender, EventArgs e)
        {
            if (profileManager.FilePath == null || profileManager.FilePath == "")
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Title = ls["MessageCaption_SaveProfile"];
                saveDialog.Filter = ls["Filter_EOP"];
                saveDialog.FileName = profileManager.Profile.Name;
                if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ProfileSaveLoadStatus status = profileManager.SaveProfile(saveDialog.FileName, true);
                    if (status.Type == ProfileSaveLaodType.Success)
                    {
                        Save = false; AddRecent(saveDialog.FileName);
                    }
                    else
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_CantSaveProfile"] + ":\n" + status.Message,
                            ls["MessageCaption_SaveProfile"]);
                    }
                }
            }
            else
            {
                ProfileSaveLoadStatus status = profileManager.SaveProfile(true);
                if (status.Type == ProfileSaveLaodType.Success)
                {
                    Save = false;
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantSaveProfile"] + ":\n" + status.Message,
                        ls["MessageCaption_SaveProfile"]);
                }
            }
        }
        private void SaveProfileAs(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = ls["MessageCaption_SaveProfile"];
            saveDialog.Filter = ls["Filter_EOP"];
            saveDialog.FileName = profileManager.Profile.Name;
            if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ProfileSaveLoadStatus status = profileManager.SaveProfile(saveDialog.FileName, true);
                if (status.Type == ProfileSaveLaodType.Success)
                {
                    Save = false; AddRecent(saveDialog.FileName);
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_CantSaveProfile"] + ":\n" + status.Message,
                        ls["MessageCaption_SaveProfile"]);
                }
            }
        }

        /*Delete/Rename*/
        private void DeleteItem(object sender, EventArgs e)
        {
            switch (activeTab)
            {
                case ActiveTab.Consoles: consolesBrowser.DeleteSelected(); break;
                case ActiveTab.Playlists: playlistsBrowser.DeleteSelected(); break;
                case ActiveTab.Roms: romsBrowser.DeleteSelected(); break;
                case ActiveTab.Emulators: emulatorsBrowser.DeleteSelected(); break;
                case ActiveTab.Filters: filtersBrowser.DeleteSelected(); break;
                case ActiveTab.Categories: categoriesBrowser.DeleteSelected(); break;
            }
        }
        private void RenameItem(object sender, EventArgs e)
        {
            switch (activeTab)
            {
                case ActiveTab.Consoles: consolesBrowser.RenameSelected(); break;
                case ActiveTab.Playlists: playlistsBrowser.RenameSelected(); break;
                case ActiveTab.Roms: romsBrowser.RenameSelected(); break;
                case ActiveTab.Emulators: emulatorsBrowser.RenameSelected(); break;
                case ActiveTab.Filters: filtersBrowser.RenameSelected(); break;
                case ActiveTab.Categories: categoriesBrowser.RenameSelected(); break;
            }
        }
        private void ClearIcon(object sender, EventArgs e)
        {
            switch (activeTab)
            {
                case ActiveTab.Consoles: consolesBrowser.ClearIcon(); break;
                case ActiveTab.Playlists: playlistsBrowser.ClearIcon(); break;
                case ActiveTab.Roms: romsBrowser.ClearIcon(); break;
                case ActiveTab.Emulators: emulatorsBrowser.ClearIcon(); break;
                case ActiveTab.Filters: filtersBrowser.ClearIcon(); break;
                case ActiveTab.Categories: categoriesBrowser.ClearIcon(); break;
            }
        }
        private void ChangeIcon(object sender, EventArgs e)
        {
            switch (activeTab)
            {
                case ActiveTab.Consoles: consolesBrowser.ChangeIcon(); break;
                case ActiveTab.Playlists: playlistsBrowser.ChangeIcon(); break;
                case ActiveTab.Roms: romsBrowser.ChangeIcon(); break;
                case ActiveTab.Emulators: emulatorsBrowser.ChangeIcon(); break;
                case ActiveTab.Filters: filtersBrowser.ChangeIcon(); break;
                case ActiveTab.Categories: categoriesBrowser.ChangeIcon(); break;
            }
        }
        private void ShowProperties(object sender, EventArgs e)
        {
            switch (activeTab)
            {
                case ActiveTab.Consoles: consolesBrowser.ShowItemProperties(); break;
                case ActiveTab.Playlists: playlistsBrowser.ShowItemProperties(); break;
                case ActiveTab.Roms: romsBrowser.ShowItemProperties(); break;
                case ActiveTab.Emulators: emulatorsBrowser.ShowItemProperties(); break;
                case ActiveTab.Filters: filtersBrowser.ShowItemProperties(); break;
                case ActiveTab.Categories: categoriesBrowser.ShowItemProperties(); break;
            }
        }

        /*Help/About*/
        private void ViewHelp(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, HelperTools.StartUpPath + "\\Help.chm", HelpNavigator.TableOfContents);
            try
            {
                Process.Start(HelperTools.StartUpPath + "\\Help\\index.htm");
            }
            catch (Exception ex)
            { ManagedMessageBox.ShowErrorMessage(ex.Message); }
        }
        private void aboutEmulatorsOrganizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_About frm = new Form_About();
            frm.ShowDialog(this);
        }
        private void consolesGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profileManager.Profile.AddConsolesGroup();
        }
        private void rootConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.AddRootConsole();
        }
        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.AddConsole();
        }
        private void AddConsoleStepStep(object sender, EventArgs e)
        {
            consolesBrowser.AddConsoleStepStep();
        }
        private void deleteToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton4.Enabled = deleteToolStripMenuItem.Enabled;
        }
        private void renameToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton5.Enabled = renameToolStripMenuItem.Enabled;
        }
        private void changeIconToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton7.Enabled = changeIconToolStripMenuItem.Enabled;
        }
        private void propertiesToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton8.Enabled = propertiesToolStripMenuItem.Enabled;
        }
        private void romsToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            romsToolStripMenuItem1.Enabled = romsToolStripMenuItem.Enabled;
        }
        private void romsfolderScanToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            romsfolderScanToolStripMenuItem1.Enabled = romsfolderScanToolStripMenuItem.Enabled;
        }
        private void romsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.AddRoms();
        }
        private void romsfolderScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.AddRomsFolderScan();
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Settings frm = new Form_Settings();
            frm.ShowDialog(this);
            // apply
            this.defaultStyle = (EOStyle)settings.GetValue("Default Style", true, new EOStyle());
            Profile_ConsoleSelectionChanged(this, null);
            // apply to controls
            categoriesBrowser.LoadControlSettings();
            consolesBrowser.LoadControlSettings();
            emulatorsBrowser.LoadControlSettings();
            romsBrowser.LoadControlSettings();
            profileStatusBrowser.LoadControlSettings();
            consoleStatusBrowser.LoadControlSettings();
            filtersBrowser.LoadControlSettings();
            playlistsBrowser.LoadControlSettings();
            informationContainersBrowser1.LoadControlSettings();
            startOptionsBrowser.LoadControlSettings();

            informationContainersBrowser1.ShowHideCloseButtons(
    (bool)settings.GetValue("Show close buttons on tabs", true, true),
    (bool)settings.GetValue("Close buttons always visible on tabs", true, false),
    (bool)settings.GetValue("Show icons on tabs", true, true));
        }
        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            columnsToolStripMenuItem.DropDownItems.Clear();
            tabsToolStripMenuItem.DropDownItems.Clear();
            showLaunchOptionsTabToolStripMenuItem.Checked = !splitContainer_left_down.Panel2Collapsed;
            showstatusbarToolStripMenuItem.Checked = statusStrip1.Visible;
            showToolsbarToolStripMenuItem.Checked = toolStrip_main.Visible;
            showmenuBarToolStripMenuItem.Checked = menuStrip1.Visible;
            showLeftPanelconsolesEmulatorsAnLaunchOptionsToolStripMenuItem.Checked = !this.splitContainer_main.Panel1Collapsed;
            if (romsBrowser.element == null)
                return;
            // Columns
            foreach (ColumnItem item in romsBrowser.element.Columns)
            {
                ToolStripMenuItem mitem = new ToolStripMenuItem();
                mitem.Text = item.ColumnName;
                mitem.Checked = item.Visible;
                columnsToolStripMenuItem.DropDownItems.Add(mitem);
            }
            // Tabs
            if (informationContainersBrowser1.ConsoleID != null && informationContainersBrowser1.ConsoleID != "")
            {
                EmulatorsOrganizer.Core.Console con = profileManager.Profile.Consoles[informationContainersBrowser1.ConsoleID];
                foreach (InformationContainer ic in con.InformationContainers)
                {
                    ToolStripMenuItem mitem = new ToolStripMenuItem();
                    mitem.Text = ic.DisplayName;
                    mitem.Checked = con.InformationContainersMap.IsContainerVisible(ic.ID);
                    mitem.Tag = ic.ID;
                    tabsToolStripMenuItem.DropDownItems.Add(mitem);
                }
            }
        }
        private void columnsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            romsBrowser.OnColumnMenuItemClick(e.ClickedItem.Text);
        }
        private void clearIconToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton9.Enabled = clearIconToolStripMenuItem.Enabled;
        }
        private void playToolStripMenuItem1_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton_play.Enabled = playToolStripMenuItem1.Enabled;
        }
        private void playlistGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profileManager.Profile.AddPlaylistsGroup();
        }
        private void playlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistsBrowser.AddPlaylist();
        }
        private void rootPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistsBrowser.AddRootPlaylist();
        }
        /*Emulator/Commandlines enable/disable*/
        private void enableEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableEmulatorToolStripMenuItem.Checked = !enableEmulatorToolStripMenuItem.Checked;
            EmulatorButton.Checked = enableEmulatorToolStripMenuItem.Checked;
            CommandlinesButton.Enabled = enableEmulatorToolStripMenuItem.Checked;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].EnableEmulator =
                            enableEmulatorToolStripMenuItem.Checked;
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].EnableEmulator =
                            enableEmulatorToolStripMenuItem.Checked;
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].EnableEmulator =
                            enableEmulatorToolStripMenuItem.Checked;
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].EnableEmulator =
                            enableEmulatorToolStripMenuItem.Checked;
                        break;
                    }
            }
            Save = true;
        }
        private void enablecommandlinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enablecommandlinesToolStripMenuItem.Checked = !enablecommandlinesToolStripMenuItem.Checked;
            CommandlinesButton.Checked = enablecommandlinesToolStripMenuItem.Checked;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].EnableCommandlines =
                            enablecommandlinesToolStripMenuItem.Checked;
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].EnableCommandlines =
                            enablecommandlinesToolStripMenuItem.Checked;
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].EnableCommandlines =
                            enablecommandlinesToolStripMenuItem.Checked;
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].EnableCommandlines =
                            enablecommandlinesToolStripMenuItem.Checked;
                        break;
                    }
            }
            Save = true;
            startOptionsBrowser.RefreshSelection(true);
        }
        private void playToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                profileManager.Profile.PlayRom();
            }
            catch (Exception ex)
            {
                ManagedMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        private void AddEmulator(object sender, EventArgs e)
        {
            if (profileManager.Profile.Consoles.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_TheresNoConsoleToAddEmulator"], ls["MessageCaption_AddEmulator"]);
                return;
            }
            Form_AddEmulator frm = new Form_AddEmulator(true);
            frm.ShowDialog(this);
        }
        private void emulatorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Emulators frm = new Form_Emulators();
            frm.ShowDialog(this);
        }
        private void renameProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Caption_EnterProfileName"], profileManager.Profile.Name, true, false);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                profileManager.Profile.Name = frm.EnteredName;
                Save = true;
                Trace.WriteLine("Profile name changed successfully.", "Profile");
            }
        }
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.OwnedForms)
            {
                if (frm.Tag.ToString() == "find")
                {
                    frm.Select(); break;
                }
            }
            Form_FindRoms ffrm = new Form_FindRoms();
            ffrm.Show(this);
        }
        private void FindRoms(object sender, EventArgs e)
        {
            timer_quickSearch.Stop();
            if (toolStripTextBox_quickSearch.Text.Length > 0)
            {
                TextSearchCondition condition = matchWordToolStripMenuItem.Checked ? TextSearchCondition.Is : TextSearchCondition.Contains;

                profileManager.Profile.OnRequestSearch(new SearchRequestArgs(toolStripTextBox_quickSearch.Text,
                    SearchMode.Name, condition, NumberSearchCondition.None, caseSensitiveToolStripMenuItem.Checked));
            }
            else
            {
                romsBrowser.RefreshParentSelection(false, null);// Cancel search
            }
        }
        private void toolStripTextBox_quickSearch_TextChanged(object sender, EventArgs e)
        {
            bool realTimeSearch = (bool)settings.GetValue("RealTimeSearchEnabled", true, true);
            if (realTimeSearch)
            {
                quickSearchTimer = 5;
                timer_quickSearch.Start();
            }
        }
        private void timer_quickSearch_Tick(object sender, EventArgs e)
        {
            if (quickSearchTimer > 0)
                quickSearchTimer--;
            else
            {
                FindRoms(this, null);
            }
        }
        private void toolStripSplitButton2_DropDownOpening(object sender, EventArgs e)
        {
        }
        private void toolStripTextBox_quickSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                FindRoms(this, null);
            }
        }
        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romsBrowser.ListViewMode = MLV.ManagedListViewViewMode.Details;
        }
        private void thumbnailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romsBrowser.ListViewMode = MLV.ManagedListViewViewMode.Thumbnails;
        }
        private void toolStripSplitButton3_DropDownOpening(object sender, EventArgs e)
        {
            detailsToolStripMenuItem.Checked = thumbnailsToolStripMenuItem.Checked = false;
            switch (romsBrowser.ListViewMode)
            {
                case MLV.ManagedListViewViewMode.Details: detailsToolStripMenuItem.Checked = true; break;
                case MLV.ManagedListViewViewMode.Thumbnails: thumbnailsToolStripMenuItem.Checked = true; break;
            }
        }
        private void clearUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ClearProfile frm = new Form_ClearProfile();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                profileManager.Profile.OnProfileCleanUpFinished();// We should raise event here to avoid threads conflicts.
            }
        }
        private void compressionToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_CompressionTool frm = new Form_CompressionTool();
            frm.ShowDialog(this);
        }
        private void changeExtensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romsBrowser.ChangeExtension();
        }
        private void traceWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show trace window.
            bool exist = false;
            foreach (TraceListener t in Trace.Listeners)
            {
                if (t is EOTraceListner)
                {
                    exist = true; break;
                }
            }
            if (!exist)
            {
                EOTraceListner l = new EOTraceListner();
                Trace.Listeners.Add(l);
                if (l.TraceWindow == null)
                    l.TraceWindow = new Form_TraceWindow();
                if (l.TraceWindow.IsDisposed)
                    l.TraceWindow = new Form_TraceWindow();
                l.TraceWindow.Show();
            }
            else
            {
                EOTraceListner l = (EOTraceListner)Trace.Listeners["EOTraceListner"];
                if (l.TraceWindow == null)
                    l.TraceWindow = new Form_TraceWindow();
                if (l.TraceWindow.IsDisposed)
                    l.TraceWindow = new Form_TraceWindow();
                l.TraceWindow.Show();
            }
        }
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romsBrowser.SelectAllRoms();
        }
        private void tabsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (informationContainersBrowser1.ConsoleID != null && informationContainersBrowser1.ConsoleID != "")
            {
                string id = e.ClickedItem.Tag.ToString();
                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[informationContainersBrowser1.ConsoleID];
                if (console.InformationContainersMap.IsContainerVisible(id))
                {
                    // Hide from map
                    console.InformationContainersMap.CloseContainerID(id);
                }
                else
                {
                    // Add it to map if not exist
                    if (!console.InformationContainersMap.AddNewContainerID(id))
                    {
                        if (console.InformationContainersMap.ContainerIDS == null)
                            console.InformationContainersMap.ContainerIDS = new List<string>();
                        console.InformationContainersMap.ContainerIDS.Add(id);
                    }
                }
                // Event for refresh request
                profileManager.Profile.OnInformationContainerVisibiltyChanged();
            }
        }
        private void showLaunchOptionsTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer_left_down.Panel2Collapsed = !splitContainer_left_down.Panel2Collapsed;
        }
        private void MTC_left_down_TabPageClose(object sender, MTCTabPageCloseArgs e)
        {
            e.Cancel = true;
            if (e.TabPageID == "launch options")
            {
                splitContainer_left_down.Panel2Collapsed = true;
            }
        }
        private void getDataFromMobyGamescomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romsBrowser.GetRomDataFromMobyGames();
        }
        private void exportSelectedRomsToDatabaseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romsBrowser.ExportToDatabase();
        }
        private void separateDatabaseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_SeparateDatabaseFile frm = new Form_SeparateDatabaseFile();
            frm.ShowDialog(this);
        }
        private void logsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(HelperTools.GetFullPath(".//Logs//"));
            }
            catch
            {

            }
        }
        private void visitEmulatorsOrganizerWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/alaahadid/Emulators-Organizer");
            }
            catch
            {

            }
        }

        private void databaseToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            getImagesFromDatabaseForToolStripMenuItem.DropDownItems.Clear();
            getInfoFromDatabaseForToolStripMenuItem.DropDownItems.Clear();
            exportSelectedRomsToDatabaseFileToolStripMenuItem.Enabled = romsBrowser.SelectedRomsCount() > 0;
            getDataFromMobyGamescomToolStripMenuItem.Enabled = romsBrowser.SelectedRomsCount() == 1;

            if (profileManager.Profile.RecentSelectedType != SelectionType.Console)
            {
                getImagesFromDatabaseForToolStripMenuItem.Enabled = false;
                getInfoFromDatabaseForToolStripMenuItem.Enabled = false;
                importDatabaseFileToConsoleToolStripMenuItem.Enabled = false;
                detectAndDownloadFromTheGamesDBnetForSelectedConsoleToolStripMenuItem.Enabled = false;
                return;
            }
            detectAndDownloadFromTheGamesDBnetForSelectedConsoleToolStripMenuItem.Enabled = true;
            importDatabaseFileToConsoleToolStripMenuItem.Enabled = true;
            // Get the console
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
            // Load ics
            foreach (InformationContainer ic in console.InformationContainers)
            {
                if (ic is InformationContainerImage)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Text = ic.DisplayName;
                    item.Tag = ic.ID;
                    getImagesFromDatabaseForToolStripMenuItem.DropDownItems.Add(item);
                }
                else if (ic is InformationContainerInfoText)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Text = ic.DisplayName;
                    item.Tag = ic.ID;
                    getInfoFromDatabaseForToolStripMenuItem.DropDownItems.Add(item);
                }
            }
            getImagesFromDatabaseForToolStripMenuItem.Enabled = getImagesFromDatabaseForToolStripMenuItem.DropDownItems.Count > 0;
            getInfoFromDatabaseForToolStripMenuItem.Enabled = getInfoFromDatabaseForToolStripMenuItem.DropDownItems.Count > 0;
        }
        private void getImagesFromDatabaseForToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (profileManager.Profile.RecentSelectedType != SelectionType.Console)
            {
                return;
            }
            Frm_GetImagesFromDatabase frm = new Frm_GetImagesFromDatabase(profileManager.Profile.SelectedConsoleID, e.ClickedItem.Tag.ToString());
            frm.ShowDialog(this);
        }
        private void getInfoFromDatabaseForToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (profileManager.Profile.RecentSelectedType != SelectionType.Console)
            {
                return;
            }
            Frm_GetInfoFromDatabase frm = new Frm_GetInfoFromDatabase(profileManager.Profile.SelectedConsoleID, e.ClickedItem.Tag.ToString());
            frm.ShowDialog(this);
        }
        private void recentToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!System.IO.File.Exists(e.ClickedItem.Tag.ToString()))
                return;
            if (!IsSaveRequired())
                return;
            ProfileSaveLoadStatus status = profileManager.LoadProfile(e.ClickedItem.Tag.ToString());
            if (status.Type == ProfileSaveLaodType.Success)
            {
                Save = false;
                AddRecent(e.ClickedItem.Tag.ToString());
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_CantOpenProfile"] + ":\n" + status.Message,
                    ls["MessageCaption_OpenProfile"]);
            }
        }
        private void welcomeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_RecentProfiles frm = new Form_RecentProfiles();
            frm.ShowDialog(Program.Form_Main);
        }
        private void profileSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ProfileSettings frm = new Form_ProfileSettings();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Save = true;
                Trace.WriteLine("Profile name changed to: " + profileManager.Profile.Name, "Profile");
                Trace.WriteLine("Profile settings changed.", "Profile");
            }
        }
        private void applyRomNamesToFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romsBrowser.ApplyNameToRoms();
        }
        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            applyRomNamesToFilesToolStripMenuItem.Enabled = romsBrowser.SelectedRomsCount() > 0;
        }
        private void consolestepByStepToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            consolesBrowser.AddConsoleStepStep();
        }
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            consolesBrowser.AddConsoleStepStep();
        }
        private void importConsoleFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.ImportConsoleFile();
        }
        private void importConsoleFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            consolesBrowser.ImportConsoleFile();
        }
        private void Form_Main_ResizeEnd(object sender, EventArgs e)
        {
            SaveSizesIntoStyle();
        }
        private void splitContainer_main_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SaveSizesIntoStyle();
        }
        private void splitContainer_right_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SaveSizesIntoStyle();
        }
        private void splitContainer_left_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SaveSizesIntoStyle();
        }
        private void splitContainer_left_down_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SaveSizesIntoStyle();
        }
        private void changeStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size mainWindowSize = new Size(1024, 720);
            int mainWindowSplitContainer_left = 200;
            int mainWindowSplitContainer_left_down = 200;
            int mainWindowSplitContainer_main = 200;
            int mainWindowSplitContainer_right = 500;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        if (console != null)
                        {
                            mainWindowSize = console.Style.mainWindowSize;
                            mainWindowSplitContainer_left = console.Style.mainWindowSplitContainer_left;
                            mainWindowSplitContainer_left_down = console.Style.mainWindowSplitContainer_left_down;
                            mainWindowSplitContainer_main = console.Style.mainWindowSplitContainer_main;
                            mainWindowSplitContainer_right = console.Style.mainWindowSplitContainer_right;

                            Form_StyleEditor frm = new Form_StyleEditor(console.Style.Clone());
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style = frm.StyleSaved.Clone();
                                profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSize = mainWindowSize;
                                profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_left = mainWindowSplitContainer_left;
                                profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_left_down = mainWindowSplitContainer_left_down;
                                profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_main = mainWindowSplitContainer_main;
                                profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.mainWindowSplitContainer_right = mainWindowSplitContainer_right;

                                profileManager.Profile.OnConsolePropertiesChanged(console.Name);
                            }
                        }
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        EmulatorsOrganizer.Core.ConsolesGroup gr = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        if (gr != null)
                        {
                            mainWindowSize = gr.Style.mainWindowSize;
                            mainWindowSplitContainer_left = gr.Style.mainWindowSplitContainer_left;
                            mainWindowSplitContainer_left_down = gr.Style.mainWindowSplitContainer_left_down;
                            mainWindowSplitContainer_main = gr.Style.mainWindowSplitContainer_main;
                            mainWindowSplitContainer_right = gr.Style.mainWindowSplitContainer_right;

                            Form_StyleEditor frm = new Form_StyleEditor(gr.Style.Clone());
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style = frm.StyleSaved.Clone();
                                profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSize = mainWindowSize;
                                profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_left = mainWindowSplitContainer_left;
                                profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_left_down = mainWindowSplitContainer_left_down;
                                profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_main = mainWindowSplitContainer_main;
                                profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.mainWindowSplitContainer_right = mainWindowSplitContainer_right;
                                profileManager.Profile.OnConsolesGroupPropertiesChanged(gr.Name);
                            }
                        }
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        EmulatorsOrganizer.Core.Playlist pl = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        if (pl != null)
                        {
                            mainWindowSize = pl.Style.mainWindowSize;
                            mainWindowSplitContainer_left = pl.Style.mainWindowSplitContainer_left;
                            mainWindowSplitContainer_left_down = pl.Style.mainWindowSplitContainer_left_down;
                            mainWindowSplitContainer_main = pl.Style.mainWindowSplitContainer_main;
                            mainWindowSplitContainer_right = pl.Style.mainWindowSplitContainer_right;

                            Form_StyleEditor frm = new Form_StyleEditor(pl.Style.Clone());
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style = frm.StyleSaved.Clone();
                                profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSize = mainWindowSize;
                                profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_left = mainWindowSplitContainer_left;
                                profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_left_down = mainWindowSplitContainer_left_down;
                                profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_main = mainWindowSplitContainer_main;
                                profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.mainWindowSplitContainer_right = mainWindowSplitContainer_right;
                                profileManager.Profile.OnPlaylistPropertiesChanged(pl.Name);
                            }
                        }
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        EmulatorsOrganizer.Core.PlaylistsGroup pl = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                        if (pl != null)
                        {
                            mainWindowSize = pl.Style.mainWindowSize;
                            mainWindowSplitContainer_left = pl.Style.mainWindowSplitContainer_left;
                            mainWindowSplitContainer_left_down = pl.Style.mainWindowSplitContainer_left_down;
                            mainWindowSplitContainer_main = pl.Style.mainWindowSplitContainer_main;
                            mainWindowSplitContainer_right = pl.Style.mainWindowSplitContainer_right;

                            Form_StyleEditor frm = new Form_StyleEditor(pl.Style.Clone());
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style = frm.StyleSaved.Clone();
                                profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSize = mainWindowSize;
                                profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_left = mainWindowSplitContainer_left;
                                profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_left_down = mainWindowSplitContainer_left_down;
                                profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_main = mainWindowSplitContainer_main;
                                profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.mainWindowSplitContainer_right = mainWindowSplitContainer_right;
                                profileManager.Profile.OnPlaylistsGroupPropertiesChanged(pl.Name);
                            }
                        }
                        break;
                    }
            }
        }
        private void saveStyleToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = ls["Description_SaveStyle"];
            sav.Filter = ls["Filter_Style"];
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                EOStyle.SaveStyle(sav.FileName, defaultStyle);
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.Console:
                        {
                            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                            if (console != null)
                            {
                                EOStyle.SaveStyle(sav.FileName, profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style.Clone());
                            }
                            break;
                        }
                    case SelectionType.ConsolesGroup:
                        {
                            EmulatorsOrganizer.Core.ConsolesGroup gr = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                            if (gr != null)
                            {
                                EOStyle.SaveStyle(sav.FileName, profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style.Clone());
                            }
                            break;
                        }
                    case SelectionType.Playlist:
                        {
                            EmulatorsOrganizer.Core.Playlist pl = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                            if (pl != null)
                            {
                                EOStyle.SaveStyle(sav.FileName, profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style.Clone());
                            }
                            break;
                        }
                    case SelectionType.PlaylistsGroup:
                        {
                            EmulatorsOrganizer.Core.PlaylistsGroup pl = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                            if (pl != null)
                            {
                                EOStyle.SaveStyle(sav.FileName, profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style.Clone());
                            }
                            break;
                        }
                }
            }

        }
        private void loadStyleFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = ls["Description_LoadStyle"];
            op.Filter = ls["Filter_Style"];
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                EOStyle style = EOStyle.LoadStyle(op.FileName);
                if (style != null)
                {
                    switch (profileManager.Profile.RecentSelectedType)
                    {
                        case SelectionType.Console:
                            {
                                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                                if (console != null)
                                {
                                    profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style = style;
                                    profileManager.Profile.OnConsolePropertiesChanged(console.Name);
                                }
                                break;
                            }
                        case SelectionType.ConsolesGroup:
                            {
                                EmulatorsOrganizer.Core.ConsolesGroup gr = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                                if (gr != null)
                                {
                                    profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style = style.Clone();
                                    profileManager.Profile.OnConsolesGroupPropertiesChanged(gr.Name);
                                }
                                break;
                            }
                        case SelectionType.Playlist:
                            {
                                EmulatorsOrganizer.Core.Playlist pl = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                                if (pl != null)
                                {
                                    profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].Style = style.Clone();
                                    profileManager.Profile.OnPlaylistPropertiesChanged(pl.Name);
                                }
                                break;
                            }
                        case SelectionType.PlaylistsGroup:
                            {
                                EmulatorsOrganizer.Core.PlaylistsGroup pl = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                                if (pl != null)
                                {
                                    profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].Style = style.Clone();
                                    profileManager.Profile.OnPlaylistsGroupPropertiesChanged(pl.Name);
                                }
                                break;
                            }
                    }
                }
            }
        }
        private void tabsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        ConsoleProperties frm = new ConsoleProperties(profileManager.Profile.SelectedConsoleID,
                            ls["Title_Tabs"]);
                        frm.ShowDialog(this);
                        break;
                    }
                default:
                    {
                        ManagedMessageBox.ShowMessage(ls["Message_YouMustSelectaConsoleToCompleteThisOperation"]);
                        break;
                    }
            }
        }
        private void dataInfoToInfoTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        Form_DataInfoToInfoTab frm = new Form_DataInfoToInfoTab(profileManager.Profile.SelectedConsoleID);
                        frm.ShowDialog(this);
                        break;
                    }
                default:
                    {
                        ManagedMessageBox.ShowMessage(ls["Message_YouMustSelectaConsoleToCompleteThisOperation"]);
                        break;
                    }
            }
        }
        private void mAMEConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.AddMameConsole();
        }
        private void showToolsbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip_main.Visible = !toolStrip_main.Visible;
            settings.AddValue("Show toolsbar", toolStrip_main.Visible);
        }
        private void showstatusbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Visible = !statusStrip1.Visible;
            settings.AddValue("Show statusstrip", statusStrip1.Visible);
        }
        private void showmenuBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip1.Visible = !menuStrip1.Visible;
            settings.AddValue("Show main menu", menuStrip1.Visible);
            if (!menuStrip1.Visible)
            {
                bool hid = (bool)settings.GetValue("Hiding main menu for first time", true, true);
                if (hid)
                {
                    ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(this, ls["Message_HidingMainMenuFirtTime"],
                          "Emulators Organizer", true, false, ls["MessageCheckBox_AlwaysRemindMe"]);
                    settings.AddValue("Hiding main menu for first time", res.Checked);
                }
            }
        }
        private void showLeftPanelconsolesEmulatorsAnLaunchOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer_main.Panel1Collapsed = !splitContainer_main.Panel1Collapsed;
            SaveSizesIntoStyle();
        }
        private void toolStripButton_parentMode_Click(object sender, EventArgs e)
        {
            parentAndChildrenModeToolStripMenuItem.Checked = !parentAndChildrenModeToolStripMenuItem.Checked;
            toolStripButton_parentMode.Checked = parentAndChildrenModeToolStripMenuItem.Checked;
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].ParentAndChildrenMode =
                            parentAndChildrenModeToolStripMenuItem.Checked;
                        break;
                    }
            }
            Save = true;
            romsBrowser.RefreshParentSelection(false, null);
        }
        private void importDatabaseFileToConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileManager.Profile.RecentSelectedType != SelectionType.Console)
                return;
            Form_ImportDatabaseFileUniversal frm = new Form_ImportDatabaseFileUniversal(profileManager.Profile.SelectedConsoleID);
            frm.ShowDialog(this);
        }
        private void detectAndDownloadFromTheGamesDBnetForSelectedConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.DetectAndDownloadFromTHeGamesDB();
        }
        private void languageToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int i = 0;
            int index = 0;
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                if (item.Text == e.ClickedItem.Text)
                {
                    item.Checked = true;
                    index = i;
                }
                else
                    item.Checked = false;
                i++;
            }

            ls.Language = ls.SupportedLanguages[index, 0];
            settings.AddValue("Language", ls.SupportedLanguages[index, 0]);

            MessageBox.Show(ls["Message_YouMustRestartTheProgramToApplyLanguage"],
             ls["MessageCaption_ApplyLanguage"]);
        }
        private void ProfileManager_Progress(object sender, ProgressArgs e)
        {
            if (IsDisposed)
                return;
            if (!this.InvokeRequired)
                OnProgress(e.Status, e.Completed);
            else
                this.Invoke(new ShowProgress(OnProgress), e.Status, e.Completed);
        }
        private void ProfileManager_ProfileSavingFinished(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;
            if (!this.InvokeRequired)
            {
                ShowStuffAfterSave();
            }
            else
            {
                this.Invoke(new Action(ShowStuffAfterSave));
            }
        }
        private void ProfileManager_ProfileSavingStarted(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;
            HideStuffForSave();
        }
        private void cloneConsoleSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consolesBrowser.CloneConsoleSettings();
        }
        private void clonePlaylistsGroupplaylistSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistsBrowser.ClonePlaylistSettings();
        }
        private void wikiOnlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/alaahadid/Emulators-Organizer/wiki");
            }
            catch
            {

            }
        }

        private void forumsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://emulator-organizer-f.forumotion.com/");
            }
            catch
            {

            }
        }
    }
    public enum ActiveTab
    {
        None, Consoles, Playlists, Categories, Filters, Emulators, Roms, InfoTabs, LaunchOptions, ProfileStatus, ConsoleStatus
    }
}
