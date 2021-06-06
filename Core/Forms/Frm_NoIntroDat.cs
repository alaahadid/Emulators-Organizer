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
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    public partial class Frm_NoIntroDat : Form
    {
        public Frm_NoIntroDat(DatabaseFile_NoIntro db)
        {
            this.db = db;
            InitializeComponent();
            //refresh
            textBox1.Text = db.dbName;
            textBox2.Text = db.dbDescription;
            textBox3.Text = db.dbVersion;
            textBox4.Text = db.dbComment;
            textBox5.Text = db.dbHeader;
            numericUpDown1.Value = db.BytesToSkip;
            checkBox1.Checked = db.IgnoreSkipForCompressed;
        }
        private DatabaseFile_NoIntro db;
        private void button1_Click(object sender, EventArgs e)
        {
            db.dbName = textBox1.Text;
            db.dbDescription = textBox2.Text;
            db.dbVersion = textBox3.Text;
            db.dbComment = textBox4.Text;
            db.dbHeader = textBox5.Text;
            db.BytesToSkip = (int)numericUpDown1.Value;
            db.IgnoreSkipForCompressed = checkBox1.Checked;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "XML (*.xml)|*.xml";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox5.Text = Path.GetFileName(op.FileName);
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
                    if (XMLread.Name == "data" & XMLread.IsStartElement())
                    {
                        if (XMLread.MoveToAttribute("value"))
                            db.dbHeaderRuleValue = XMLread.Value;
                    }
                }
                XMLread.Close();
                databaseStream.Close();
            }
        }
    }
}
