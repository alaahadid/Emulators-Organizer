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
using EmulatorsOrganizer.GUI;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
namespace EmulatorsOrganizer
{
    public partial class SettingsControlGoogleImageSearch : ISettingsControl
    {
        public SettingsControlGoogleImageSearch()
        {
            InitializeComponent();
        }
        public override string ToString()
        {
            return ls["Title_GoogleImages"];
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            comboBox_searchMethod.SelectedIndex = (int)settings.GetValue("GoogleImage:SearchNameMethod", true, 0);
            textBox_customName.Text = (string)settings.GetValue("GoogleImage:CustomName", true, "");
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            settings.AddValue("GoogleImage:SearchNameMethod", comboBox_searchMethod.SelectedIndex);
            settings.AddValue("GoogleImage:CustomName", textBox_customName.Text);
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
            comboBox_searchMethod.SelectedIndex = 0;
            textBox_customName.Text = "";
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
