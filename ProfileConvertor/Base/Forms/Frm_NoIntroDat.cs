/* This file is part of Emulators Organizer
   A program that can organize roms and emulators

   Copyright © Ali Hadid and Ala Hadid 2009 - 2013

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
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

namespace AHD.EO.Base
{
    public partial class Frm_NoIntroDat : Form
    {
        DB_NoIntroDat format;
        public Frm_NoIntroDat(DB_NoIntroDat format)
        {
            InitializeComponent();
            this.format = format;
            //refresh
            textBox1.Text = format.dbName;
            textBox2.Text = format.dbDescription;
            textBox3.Text = format.dbVersion;
            textBox4.Text = format.dbComment;
            textBox5.Text = format.dbHeader;
            numericUpDown1.Value = format.bytesToSkip;
            checkBox1.Checked = format.IgnoreSkipForCompressed;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            format.dbName = textBox1.Text;
            format.dbDescription = textBox2.Text;
            format.dbVersion = textBox3.Text;
            format.dbComment = textBox4.Text;
            format.dbHeader = textBox5.Text;
            format.bytesToSkip = (int)numericUpDown1.Value;
            format.IgnoreSkipForCompressed = checkBox1.Checked;
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
                            format.dbHeaderRuleValue = XMLread.Value;
                    }
                }
                XMLread.Close();
                databaseStream.Close();
            }
        }
    }
}
