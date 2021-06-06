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
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_SearchTheGamesDB : Form
    {
        public Form_SearchTheGamesDB(string romID)
        {
            InitializeComponent();
            rom = profileManager.Profile.Roms[romID];
            this.textBox_gameTitle.Text = rom.Name;
            //this.textBox_platform.Text = profileManager.Profile.Consoles[rom.ParentConsoleID].Name;

            button1_Click(this, new EventArgs());
        }

        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private Rom rom;

        public int SelectedResultID
        {
            get { return (int)listView1.SelectedItems[0].Tag; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_gameTitle.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterTheGameTitleFirst"]);
                return;
            }
           /* ICollection<GameSearchResult> results = GamesDB.GetGames(textBox_gameTitle.Text, textBox_platform.Text, textBox_genre.Text);
            listView1.Items.Clear();
            foreach (GameSearchResult result in results)
            {
                ListViewItem item = new ListViewItem();
                item.Text = result.ID.ToString();
                item.SubItems.Add(result.Title);
                item.SubItems.Add(result.ReleaseDate);
                item.SubItems.Add(result.Platform);
                item.Tag = result.ID;

                listView1.Items.Add(item);
            }*/
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectOneResultFirst"]);
                return;
            }
           /* Game game = GamesDB.GetGame((int)listView1.SelectedItems[0].Tag);

            listView2.Items.Clear();

            listView2.Items.Add("Title");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.Title);
            listView2.Items.Add("Platform");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.Platform);
            listView2.Items.Add("ReleaseDate");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.ReleaseDate);
            listView2.Items.Add("Overview");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.Overview);
            listView2.Items.Add("ESRB");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.ESRB);
            listView2.Items.Add("Players");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.Players);
            listView2.Items.Add("Publisher");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.Publisher);
            listView2.Items.Add("Developer");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.Developer);
            listView2.Items.Add("Rating");
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(game.Rating);
            listView2.Items.Add("AlternateTitles");
            string titles = "";
            foreach (string t in game.AlternateTitles)
                titles += t + ", ";
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(titles);
            listView2.Items.Add("Genres");
            string genres = "";
            foreach (string t in game.Genres)
                genres += t + ", ";
            listView2.Items[listView2.Items.Count - 1].SubItems.Add(genres);*/
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectOneResultFirst"]);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
