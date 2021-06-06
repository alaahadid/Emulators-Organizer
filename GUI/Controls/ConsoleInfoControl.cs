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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MTC;

namespace EmulatorsOrganizer.GUI
{
    public partial class ConsoleInfoControl : UserControl
    {
        public ConsoleInfoControl()
        {
            InitializeComponent();
        }
        public ConsoleInfoControl(EmulatorsOrganizer.Core.Console console, ManagedTabControl parentMTC)
        {
            InitializeComponent();
            Initialize(console, parentMTC);
        }

        private ManagedTabControl parentMTC;
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        public string CurrentConsoleID;
        public void Initialize(EmulatorsOrganizer.Core.Console console, ManagedTabControl parentMTC)
        {
            this.parentMTC = parentMTC;
            RefreshInformation(console);
        }
        public void RefreshInformation(EmulatorsOrganizer.Core.Console console)
        {
            // Load the console
            if (console != null)
            {
                CurrentConsoleID = console.ID;
                // Set icon
                imagePanel1.ImageToView = (Bitmap)console.Icon;
                imagePanel1.Invalidate();
                // Name
                linkLabel_name.Text = console.Name;
                linkLabel_name.Tag = console.ID;
                // Short description
                if (console.ShortDescription != "")
                    richTextBox1.Text = console.ShortDescription;
                else
                    richTextBox1.Text = ls["Status_NoShortDescriptionForThisConsole"];
                // Extesnions
                textBox_extensions.Text = ListToText(console.Extensions);
                // Tabs
                textBox_tabs.Text = ListToText(console.InformationContainers);
                // Roms
                Rom[] roms = profileManager.Profile.Roms[console.ID, false];
                textBox_roms.Text = roms.Length.ToString() + " roms total";
                // AI roms
                int ai_len = 0;
                int normal_len = 0;
                long console_play_time = 0;
                string favoriteGame = "";
                Rom favGame = null;
                long most_time = 0;
                int most_times_played = 0;
                string mostPlayedGame = "";
                Rom mostGame = null;
                int highestRated = 0;
                string highestRatedGame = "";
                Rom highestGame = null;
                foreach (Rom rom in roms)
                {
                    if (HelperTools.IsAIPath(rom.Path))
                        ai_len++;
                    else
                        normal_len++;

                    console_play_time += rom.PlayedTimeLength;

                    if (rom.PlayedTimeLength > most_time)
                    {
                        most_time = rom.PlayedTimeLength;
                        favoriteGame = rom.Name;
                        favGame = rom;
                    }

                    if (rom.PlayedTimes > most_times_played)
                    {
                        most_times_played = rom.PlayedTimes;
                        mostPlayedGame = rom.Name;
                        mostGame = rom;
                    }
                    if (rom.Rating > highestRated)
                    {
                        highestRated = rom.Rating;
                        highestRatedGame = rom.Name;
                        highestGame = rom;
                    }
                }
                textBox_roms_AI.Text = ai_len.ToString() + " AI path rom";
                textBox_roms_ind.Text = normal_len.ToString() + " Individual rom";
                textBox_console_play_time.Text = TimeSpan.FromMilliseconds(console_play_time).ToString().Substring(0, 8) + " spent with this console totally.";
                if (most_time > 0)
                {
                    linkLabel_favoriteGame.Text = favoriteGame;
                    linkLabel_favoriteGame.Tag = favGame;
                    label_favoriteGame.Text = "You spent " + TimeSpan.FromMilliseconds(most_time).ToString().Substring(0, 8) + " with this game.";
                }
                else
                {
                    linkLabel_favoriteGame.Text = "";
                    label_favoriteGame.Text = "No game played so far.";
                }

                if (most_times_played > 0)
                {
                    linkLabel_mostplayed.Text = mostPlayedGame;
                    linkLabel_mostplayed.Tag = mostGame;
                    label_most_played.Text = "You played this game " + most_times_played.ToString() + " time(s)";
                }
                else
                {
                    linkLabel_mostplayed.Text = "";
                    label_most_played.Text = "No game played so far.";
                }
                if (highestRated > 0)
                {
                    linkLabel_highRated.Text = highestRatedGame;
                    linkLabel_highRated.Tag = highestGame;
                    //   rating1.Enabled = true;
                    rating1.rating = highestRated;
                }
                else
                {
                    linkLabel_highRated.Text = "";
                    //  rating1.Enabled = false;
                    rating1.rating = 0;
                }
            }
            else
                CurrentConsoleID = "";
        }
        public void RefreshInformation()
        {
            if (CurrentConsoleID != "")
                RefreshInformation(profileManager.Profile.Consoles[CurrentConsoleID]);
        }
        public void ClearInformation()
        {
            imagePanel1.ImageToView = null;
            linkLabel_name.Text = "N/A";
            linkLabel_name.Tag = null;
            textBox_console_play_time.Text =
            textBox_extensions.Text =
            textBox_roms.Text =
            textBox_roms_AI.Text =
            textBox_roms_ind.Text =
            textBox_tabs.Text =
            linkLabel_favoriteGame.Text =
            linkLabel_highRated.Text =
            linkLabel_mostplayed.Text = "";
            rating1.rating = 0;
        }
        private string ListToText(List<string> list)
        {
            string val = "";
            foreach (string ext in list)
            {
                val += ext + ", ";
            }
            if (val.Length > 0)
                val = val.Substring(0, val.Length - 2);
            return val;
        }
        private string ListToText(List<InformationContainer> list)
        {
            string val = "";
            foreach (InformationContainer ext in list)
            {
                val += ext.DisplayName + ", ";
            }
            if (val.Length > 0)
                val = val.Substring(0, val.Length - 2);
            return val;
        }

        private void linkLabel_favoriteGame_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (((LinkLabel)sender).Tag != null)
            {
                Rom game = (Rom)((LinkLabel)sender).Tag;
                if (game != null)
                {
                    // Select the console:
                    profileManager.Profile.SelectedConsoleID = game.ParentConsoleID;

                    // Select the rom !
                    profileManager.Profile.SelectedRomIDS.Clear();
                    profileManager.Profile.SelectedRomIDS.Add(game.ID);
                    profileManager.Profile.OnRomSelectionChanged();

                    // Go to the index !
                    if (parentMTC != null)
                        parentMTC.SelectedTabPageIndex = 0;
                }
            }
        }
        // Show console properties
        private void linkLabel_name_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            object id = ((LinkLabel)sender).Tag;
            if (id == null)
                return;
            if (!profileManager.Profile.Consoles.ContainsID(id.ToString()))
                return;
            ConsoleProperties frm = new ConsoleProperties(id.ToString(), ls["Title_General"]);
            frm.ShowDialog();
        }
        // Show console manual
        private void label_manual_Click(object sender, EventArgs e)
        {
            object id = linkLabel_name.Tag;
            if (id == null)
                return;
            if (!profileManager.Profile.Consoles.ContainsID(id.ToString()))
                return;
            string pdfPath = HelperTools.GetFullPath(profileManager.Profile.Consoles[id.ToString()].PDFPath);
            if (File.Exists(pdfPath))
            {
                System.Diagnostics.Process.Start(pdfPath);
            }
        }
        // Show console rtf
        private void label_rtf_Click(object sender, EventArgs e)
        {
            object id = linkLabel_name.Tag;
            if (id == null)
                return;
            if (!profileManager.Profile.Consoles.ContainsID(id.ToString()))
                return;
            string rtfPath = HelperTools.GetFullPath(profileManager.Profile.Consoles[id.ToString()].RTFPath);
            if (File.Exists(rtfPath))
            {
                System.Diagnostics.Process.Start(rtfPath);
            }
        }
    }
}
