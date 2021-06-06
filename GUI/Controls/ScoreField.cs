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
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class ScoreField : UserControl
    {
        public ScoreField()
        {
            InitializeComponent();
        }
        private int score;
        private bool isTitle;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                linkLabel_score.Text = value.ToString() + " %";
                panel_score.Invalidate();
            }
        }
        public bool IsTitleScore
        {
            get { return isTitle; }
            set
            {
                isTitle = value;
                label_name.Font = new Font("Tahoma", isTitle ? 10 : 8, isTitle ? FontStyle.Bold : FontStyle.Regular);
            }
        }
        public string FieldName
        { get { return label_name.Text; } set { label_name.Text = value; } }

        public event EventHandler ScoreChanged;

        private void panel_score_Paint(object sender, PaintEventArgs e)
        {
            int w = ((100 - score) * panel_score.Width) / 100;
            Rectangle rect = new Rectangle(0, 0, panel_score.Width, panel_score.Height);
            // Fill all
            e.Graphics.FillRectangle(new LinearGradientBrush(rect, Color.Green, Color.Blue, 180, true), rect);
            // Discard score area
            e.Graphics.FillRectangle(Brushes.White,
                new Rectangle(panel_score.Width - w, 0, w, panel_score.Height));
        }
        private void panel_score_MouseClick(object sender, MouseEventArgs e)
        {
            Score = (e.X * 100) / panel_score.Width;
            if (ScoreChanged != null)
                ScoreChanged(this, new EventArgs());
        }
        private void linkLabel_score_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_ScoreEdit frm = new Form_ScoreEdit(score);
            frm.Location = new Point(Cursor.Position.X - (frm.Width / 2), Cursor.Position.Y - 10);
            frm.ShowDialog(this);

            Score = frm.Score;
            if (ScoreChanged != null)
                ScoreChanged(this, new EventArgs());
        }
    }
}
