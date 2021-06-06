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

namespace EmulatorsOrganizer.GUI
{
    public partial class Rating : UserControl
    {
        public Rating()
        {
            InitializeComponent();
        }
        int _rating = 0;
        public int rating
        { get { return _rating; } set { _rating = value; label6.Visible = false; RefreshRating(); } }
        public void RefreshRating()
        {
            if (_rating == 0)
            {
                label1.ImageIndex = label2.ImageIndex = label3.ImageIndex = label4.ImageIndex = label5.ImageIndex = 1;
            }
            else if (_rating == 1)
            {
                label1.ImageIndex = 0;
                label2.ImageIndex = label3.ImageIndex = label4.ImageIndex = label5.ImageIndex = 1;
            }
            else if (_rating == 2)
            {
                label2.ImageIndex = label1.ImageIndex = 0;
                label3.ImageIndex = label4.ImageIndex = label5.ImageIndex = 1;
            }
            else if (_rating == 3)
            {
                label3.ImageIndex = label2.ImageIndex = label1.ImageIndex = 0;
                label4.ImageIndex = label5.ImageIndex = 1;
            }
            else if (_rating == 4)
            {
                label4.ImageIndex = label3.ImageIndex = label2.ImageIndex = label1.ImageIndex = 0;
                label5.ImageIndex = 1;
            }
            else if (_rating == 5)
            {
                label5.ImageIndex = label4.ImageIndex = label3.ImageIndex = label2.ImageIndex = label1.ImageIndex = 0;
            }
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label6.ImageIndex = 2;
            label6.Visible = true;
            label1.ImageIndex = 0;
            label2.ImageIndex = label3.ImageIndex = label4.ImageIndex = label5.ImageIndex = 1;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label6.Visible = true; label6.ImageIndex = 2;
            label2.ImageIndex = label1.ImageIndex = 0;
            label3.ImageIndex = label4.ImageIndex = label5.ImageIndex = 1;
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label6.ImageIndex = 2;
            label6.Visible = true;
            label3.ImageIndex = label2.ImageIndex = label1.ImageIndex = 0;
            label4.ImageIndex = label5.ImageIndex = 1;
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label6.Visible = true; label6.ImageIndex = 2;
            label4.ImageIndex = label3.ImageIndex = label2.ImageIndex = label1.ImageIndex = 0;
            label5.ImageIndex = 1;
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label6.Visible = true; label6.ImageIndex = 2;
            label5.ImageIndex = label4.ImageIndex = label3.ImageIndex = label2.ImageIndex = label1.ImageIndex = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            _rating = 1; RefreshRating();
            if (RatingChanged != null)
                RatingChanged(this, null);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            _rating = 2; RefreshRating();
            if (RatingChanged != null)
                RatingChanged(this, null);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            _rating = 3; RefreshRating();
            if (RatingChanged != null)
                RatingChanged(this, null);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            _rating = 4; RefreshRating();
            if (RatingChanged != null)
                RatingChanged(this, null);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            _rating = 5; RefreshRating();
            if (RatingChanged != null)
                RatingChanged(this, null);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            _rating = 0; RefreshRating();
            if (RatingChanged != null)
                RatingChanged(this, null);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label6.ImageIndex = 2;
            RefreshRating();
        }

        public event EventHandler RatingChanged;

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.Visible = true;
            label6.ImageIndex = 3;
            label5.ImageIndex = label4.ImageIndex = label3.ImageIndex = label2.ImageIndex = label1.ImageIndex = 1;
        }

        private void Rating_MouseLeave(object sender, EventArgs e)
        {
            label6.Visible = false;
            RefreshRating();
        }
    }
}
