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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer.GUI
{
    public partial class IPlaylistPropertiesControl : UserControl
    {
        public IPlaylistPropertiesControl()
        {
            InitializeComponent();
            profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
            ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        }
        protected string playlistID;
        protected ProfileManager profileManager;
        protected LanguageResourcesService ls;

        public string PlaylistID
        { get { return playlistID; } set { playlistID = value; } }
        public virtual bool CanSaveSettings
        {
            get { return false; }
        }
        public virtual string Description { get { return ""; } }
        public virtual void LoadSettings() { }
        public virtual void SaveSettings() { }
        public virtual void DefaultSettings() { }
    }
    public class PlaylistPropertiesControlComparer : IComparer<IPlaylistPropertiesControl>
    {
        bool AtoZ;
        public PlaylistPropertiesControlComparer(bool AtoZ)
        {
            this.AtoZ = AtoZ;
        }
        public int Compare(IPlaylistPropertiesControl x, IPlaylistPropertiesControl y)
        {
            if (AtoZ)
                return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.ToString(), y.ToString());
            else
                return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.ToString(), y.ToString()));
        }
    }
}
