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
using System.IO;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer.GUI
{
    public partial class FrmExportNames : Form
    {
        public FrmExportNames(Rom[] romsToSend)
        {
            this.romsToSend = romsToSend;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;

            textBox1.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EmulatorsOrganizer", "RomNames.txt");
        }
        private Rom[] romsToSend;
        // Browse
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Title = "Save rom names list file";
            sv.Filter = "Text file (*.txt)|*.txt;";
            if (textBox1.Text != "")
                sv.FileName = Path.GetFullPath(textBox1.Text);
            if (sv.ShowDialog(this) == DialogResult.OK)
            {
                textBox1.Text = sv.FileName;
            }
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please browse where to save the file first !!");
                button3_Click(this, new EventArgs());
                return;
            }
            List<string> lines = new List<string>();

            foreach (Rom r in romsToSend)
            {
                if (radioButton1.Checked)
                {
                    // Rom name
                    lines.Add(r.Name);
                }
                else if (radioButton2.Checked)
                {
                    // Rom file name
                    lines.Add(HelperTools.GetFullPath(r.Path));
                }
                else
                {
                    // Rom file name without extension
                    lines.Add(Path.GetFileNameWithoutExtension(HelperTools.GetFullPath(r.Path)));
                }
            }
            switch (comboBox1.SelectedIndex)
            {
                case 0: File.WriteAllLines(textBox1.Text, lines.ToArray(), System.Text.Encoding.ASCII); break;
                case 1: File.WriteAllLines(textBox1.Text, lines.ToArray(), System.Text.Encoding.UTF8); break;
                case 2: File.WriteAllLines(textBox1.Text, lines.ToArray(), System.Text.Encoding.Unicode); break;
            }
            MessageBox.Show("Done !!");
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + textBox1.Text);
            }
            catch { }
        }
    }
}
