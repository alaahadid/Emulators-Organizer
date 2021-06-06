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
using System.Threading.Tasks;
using System.Windows.Forms;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using EmulatorsOrganizer.Core;
namespace EmulatorsOrganizer
{
    public partial class SettingsControlGeneral : ISettingsControl
    {
        public SettingsControlGeneral()
        {
            InitializeComponent();
        }
        public override string ToString()
        {
            return ls["Title_General"];
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            checkBox_autoMinmize.Checked = (bool)settings.GetValue(DefaultProfileSettings.AutoMinimize_Key,
                true, DefaultProfileSettings.AutoMinimize);
            checkBox_loadRecentProfileInsteadOfWelcomWindow.Checked =
                (bool)settings.GetValue("Load recent profile", true, false);
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            settings.AddValue(new SettingsValue(DefaultProfileSettings.AutoMinimize_Key, checkBox_autoMinmize.Checked));
            settings.AddValue("Load recent profile", checkBox_loadRecentProfileInsteadOfWelcomWindow.Checked);
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
            checkBox_autoMinmize.Checked = true;
            checkBox_loadRecentProfileInsteadOfWelcomWindow.Checked = false;
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
    }
}
