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
using System.IO;

namespace EmulatorsOrganizer.GUI
{
    public partial class PlaylistPropertiesIconAndBackground : IPlaylistPropertiesControl
    {
        public PlaylistPropertiesIconAndBackground()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return ls["Title_Icon"];
        }
        public override string Description
        {
            get
            {
                return ls["PlaylistPropertiesDescription_Icon"];
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        public override void LoadSettings()
        {
            pictureBox_icon.Image = profileManager.Profile.Playlists[playlistID].Icon;
        }
        public override void SaveSettings()
        {
            profileManager.Profile.Playlists[playlistID].Icon = pictureBox_icon.Image;
        }
        public override void DefaultSettings()
        {
            pictureBox_icon.Image = null;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog Op = new OpenFileDialog();
            Op.Title = ls["Title_BrowseForAnIcon"];
            Op.Filter = ls["Filter_Icon"]; ;
            Op.Multiselect = false;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                if (Path.GetExtension(Op.FileName).ToLower() == ".exe" | Path.GetExtension(Op.FileName).ToLower() == ".ico")
                { pictureBox_icon.Image = Icon.ExtractAssociatedIcon(Op.FileName).ToBitmap(); }
                else
                {
                    //Stream str = new FileStream(Op.FileName, FileMode.Open, FileAccess.Read);
                    //byte[] buff = new byte[str.Length];
                    // str.Read(buff, 0, (int)str.Length);
                    // str.Dispose();
                    // str.Close();

                    //  pictureBox_icon.Image = (Bitmap)Image.FromStream(new MemoryStream(buff));
                    using (var bmpTemp = new Bitmap(Op.FileName))
                    {
                        pictureBox_icon.Image = new Bitmap(bmpTemp);
                    }

                }
            }
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox_icon.Image = null;
        }
    }
}
