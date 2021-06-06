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
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_CommandlineProperties : Form
    {
        ICommandline cm;
        public Form_CommandlineProperties(ICommandline cm)
        {
            InitializeComponent();
            this.cm = cm;
            textBox1.Text = cm.Name;
            comboBox1.Text = cm.Code;
        }
        public string EnteredName
        { get { return textBox1.Text; } }
        public string EnteredCode
        { get { return comboBox1.Text; } }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 5)
            {
                OpenFileDialog op = new OpenFileDialog();
                if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    comboBox1.Items.Add(@"""" + op.FileName + @"""");
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                }
            }
        }
    }
}
