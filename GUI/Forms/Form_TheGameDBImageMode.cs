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
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_TheGamesDBImageMode : Form
    {
        public Form_TheGamesDBImageMode()
        {
            InitializeComponent();
        }
       /* public Form_TheGamesDBImageMode(TheGamesDBAPI.Game.GameImages Images)
        {
            InitializeComponent();
            radioButton_Banners.Enabled = Images.Banners.Count > 0;
            radioButton_boxart_back.Enabled = Images.BoxartBack != null;
            radioButton_boxart_front.Enabled = Images.BoxartFront != null;
            radioButton_Fanart.Enabled = Images.Fanart.Count > 0;
            radioButton_Screenshots.Enabled = Images.Screenshots.Count > 0;

            if (radioButton_Screenshots.Enabled)
                radioButton_Screenshots.Enabled = true;
            else if (radioButton_Fanart.Enabled)
                radioButton_Fanart.Enabled = true;
            else if (radioButton_Banners.Enabled)
                radioButton_Banners.Enabled = true;
            else if (radioButton_boxart_back.Enabled)
                radioButton_boxart_back.Enabled = true;
            else if (radioButton_boxart_front.Enabled)
                radioButton_boxart_front.Enabled = true;
        }*/
        public bool SelectedBoxArtFront
        {
            get { return radioButton_boxart_front.Checked; }
        }
        public bool SelectedBanners
        {
            get { return radioButton_Banners.Checked; }
        }
        public bool SelectedBoxArtBack
        {
            get { return radioButton_boxart_back.Checked; }
        }
        public bool SelectedFanart
        {
            get { return radioButton_Fanart.Checked; }
        }
        public bool SelectedScreenshots
        {
            get { return radioButton_Screenshots.Checked; }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
