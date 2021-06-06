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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using EmulatorsOrganizer.Services.TraceListners;

namespace EmulatorsOrganizer.Services
{
    public partial class Form_TraceWindow : Form
    {
        public Form_TraceWindow()
        {
            InitializeComponent();
            ComboBox1.SelectedIndex = 0;
        }
        private delegate void WriteLineDelegate(string message, string category, TraceEventType type);
        private List<TraceLine> buffer = new List<TraceLine>();
        public void WriteLine(string message, string category, TraceEventType type)
        {
            if (!this.InvokeRequired)
                WriteLine1(message, category, type);
            else
                this.Invoke(new WriteLineDelegate(WriteLine1), message, category, type);
        }
        private void WriteLine1(string message, string category, TraceEventType type)
        {
            string indent = "";
            for (int i = 0; i < Trace.IndentLevel; i++)
            {
                indent += "   ";
            }
            TraceLine line = new TraceLine(indent + message, category, DateTime.Now, type);
            buffer.Add(line);
            try
            {
                // Add to listview
                if (ComboBox1.SelectedIndex == 0 || (ComboBox1.SelectedItem.ToString() == category))
                {
                    AddLine(line); 
                    if (listView1.Items.Count > 0)
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                }
                // Add category
                if (!ComboBox1.Items.Contains(category))
                    ComboBox1.Items.Add(category);
            }
            catch { }
        }
        private void AddLine(TraceLine line)
        {
            ListViewItem item = new ListViewItem(line.Message);
            item.SubItems.Add(line.Category);
            item.SubItems.Add(line.Type.ToString());
            item.SubItems.Add(line.Time.ToLocalTime().ToString());
            switch (line.Type)
            {
                case TraceEventType.Information: item.ImageIndex = 0; break;
                case TraceEventType.Warning: item.ImageIndex = 1; break;
                case TraceEventType.Critical:
                case TraceEventType.Error: item.ImageIndex = 2; break;
            }
            
            listView1.Items.Add(item);
            
        }
        private void ComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (TraceLine line in buffer)
            {
                // Add to listview
                if (ComboBox1.SelectedIndex == 0 || (ComboBox1.SelectedItem.ToString() == line.Category))
                {
                    AddLine(line);
                }
            }
            if (listView1.Items.Count > 0)
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
        }
    }
}
