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
using EmulatorsOrganizer.Services;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace EmulatorsOrganizer.Core
{
    public partial class Frm_HyperlistExportOptions : Form
    {
        public Frm_HyperlistExportOptions(DatabaseFile_HyperListXML db)
        {
            db_file = db;
            InitializeComponent();
            // The name should be the console name
            console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
            textBox_name.Text = console.Name;
            string date = string.Format("{0}{1}{2}-{3}{4}{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            textBox_date.Text = date;
            textBox_version.Text = textBox_date.Text;
        }
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private EmulatorsOrganizer.Core.Console console;
        private DatabaseFile_HyperListXML db_file;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "XML (*.xml)|*.xml";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_header.Text = Path.GetFileName(op.FileName);
                Stream databaseStream = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.DtdProcessing = DtdProcessing.Ignore;
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(databaseStream, sett);
                while (XMLread.Read())
                {
                    if (XMLread.Name == "name" & XMLread.IsStartElement())
                    {
                        if (XMLread.ReadString().Contains("iNES"))
                            numericUpDown1.Value = 16;
                    }
                }
                XMLread.Close();
                databaseStream.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            db_file._db_header_name = textBox_name.Text;
            db_file._db_header_version = textBox_version.Text;
            db_file._db_header_date = textBox_date.Text;
            db_file._db_header_author = textBox_author.Text;
            db_file._db_header_header_file_name = textBox_header.Text;
            db_file._db_header_plugin = textBox_plugin.Text;
            db_file.BytesToSkip = (int)numericUpDown1.Value;
            db_file.IgnoreSkipForCompressed = checkBox1.Checked;
            db_file.IncludeCategoriesOnExport = checkBox2.Checked;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
