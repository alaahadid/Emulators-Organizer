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
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer
{
    public partial class ISettingsControl : UserControl
    {
        public ISettingsControl()
        {
            InitializeComponent();
        }
        protected LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        protected ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        protected SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        public virtual void SaveSettings()
        { }
        public virtual void LoadSettings()
        { }
        public virtual void DefaultSettings()
        { }
        public virtual bool CanSaveSettings
        { get { return false; } }
    }
    public class ISettingsControlComparer : IComparer<ISettingsControl>
    {
        bool AtoZ;
        public ISettingsControlComparer(bool AtoZ)
        {
            this.AtoZ = AtoZ;
        }
        public int Compare(ISettingsControl x, ISettingsControl y)
        {
            if (AtoZ)
                return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.ToString(), y.ToString());
            else
                return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.ToString(), y.ToString()));
        }
    }
}
