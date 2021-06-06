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
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using MMB;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using EmulatorsOrganizer.GUI;
namespace EmulatorsOrganizer
{
    public partial class Form_StartUp : Form
    {
        public Form_StartUp(string[] args)
        {
            this.args = args;
            InitializeComponent();
            label_version.Text = ls["Version"] + " " + Program.Version;

            foreach (Attribute attr in Attribute.GetCustomAttributes(Assembly.GetExecutingAssembly()))
            {
                if (attr.GetType() == typeof(AssemblyCopyrightAttribute))
                {
                    AssemblyCopyrightAttribute inf = (AssemblyCopyrightAttribute)attr;
                    label_copyright.Text = inf.Copyright;
                }
            }
        }
        private LanguageResourcesService ls = (LanguageResourcesService)EmulatorsOrganizer.Services.ServicesManager.GetService("EO Language Resources");
        private string[] args;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            // Load window
            Program.Form_Main = new Form_Main(args);
            Program.Form_Main.Show();
            SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");

            if ((bool)settings.GetValue("Load recent profile", true, false))
            {
                string[] recentProfiles = (string[])settings.GetValue("Recent profiles", true, new string[0]);
                if (recentProfiles != null)
                {
                    if (recentProfiles.Length > 0)
                    {
                        if (File.Exists(recentProfiles[0]))
                        {
                            LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
                            ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
                            ProfileSaveLoadStatus status = profileManager.LoadProfile(recentProfiles[0]);
                            if (status.Type == ProfileSaveLaodType.Success)
                            {
                                Program.Form_Main.Save = false;
                                Program.Form_Main.AddRecent(recentProfiles[0]);
                            }
                            else
                            {
                                ManagedMessageBox.ShowErrorMessage(ls["Message_CantOpenProfile"] + ":\n" + status.Message,
                                    ls["MessageCaption_OpenProfile"]);
                            }
                        }
                    }
                }
            }
            // Load start up window
            else if (!Program.Form_Main.DisableQuickScreen)
            {
                if ((bool)settings.GetValue("Show startup window", true, true))
                {
                    Form_RecentProfiles frm = new Form_RecentProfiles();
                    frm.ShowDialog(Program.Form_Main);
                }
            }
            this.Close();
        }

        private void Form_StartUp_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
