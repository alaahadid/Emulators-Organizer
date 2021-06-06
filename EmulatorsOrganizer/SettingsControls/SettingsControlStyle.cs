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
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.GUI;
namespace EmulatorsOrganizer
{
    public partial class SettingsControlStyle : ISettingsControl
    {
        public SettingsControlStyle()
        {
            InitializeComponent();
        }
        private EOStyle defaultStyle;

        private Size mainWindowSize = new Size(1024, 720);
        private int mainWindowSplitContainer_left = 200;
        private int mainWindowSplitContainer_left_down = 200;
        private int mainWindowSplitContainer_main = 200;
        private int mainWindowSplitContainer_right = 500;

        public override string ToString()
        {
            return ls["Title_DefaultStyle"];
        }
        public override void LoadSettings()
        {
            defaultStyle = (EOStyle)settings.GetValue("Default Style", true, new EOStyle());
            mainWindowSize = defaultStyle.mainWindowSize;
            mainWindowSplitContainer_left = defaultStyle.mainWindowSplitContainer_left;
            mainWindowSplitContainer_left_down = defaultStyle.mainWindowSplitContainer_left_down;
            mainWindowSplitContainer_main = defaultStyle.mainWindowSplitContainer_main;
            mainWindowSplitContainer_right = defaultStyle.mainWindowSplitContainer_right;

            base.LoadSettings();
        }
        public override void SaveSettings()
        {
            settings.AddValue("Default Style", defaultStyle);
            base.SaveSettings();
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        // Edit
        private void button1_Click(object sender, EventArgs e)
        {
            Form_StyleEditor frm = new Form_StyleEditor(defaultStyle);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                defaultStyle = frm.StyleSaved;
                // Get old values
                defaultStyle.mainWindowSize = mainWindowSize;
                defaultStyle.mainWindowSplitContainer_left = mainWindowSplitContainer_left;
                defaultStyle.mainWindowSplitContainer_left_down = mainWindowSplitContainer_left_down;
                defaultStyle.mainWindowSplitContainer_main = mainWindowSplitContainer_main;
                defaultStyle.mainWindowSplitContainer_right = mainWindowSplitContainer_right;
            }
        }
        // Export
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = ls["Description_SaveStyle"];
            sav.Filter = ls["Filter_Style"];
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                EOStyle.SaveStyle(sav.FileName, defaultStyle);
            }
        }
        // Import
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = ls["Description_LoadStyle"];
            op.Filter = ls["Filter_Style"];
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                EOStyle style = EOStyle.LoadStyle(op.FileName);
                if (style != null)
                {
                    defaultStyle = style;
                    // Get old values
                    defaultStyle.mainWindowSize = mainWindowSize;
                    defaultStyle.mainWindowSplitContainer_left = mainWindowSplitContainer_left;
                    defaultStyle.mainWindowSplitContainer_left_down = mainWindowSplitContainer_left_down;
                    defaultStyle.mainWindowSplitContainer_main = mainWindowSplitContainer_main;
                    defaultStyle.mainWindowSplitContainer_right = mainWindowSplitContainer_right;
                }
            }
        }
    }
}
