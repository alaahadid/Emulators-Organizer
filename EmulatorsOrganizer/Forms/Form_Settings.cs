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
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using EmulatorsOrganizer.GUI;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
namespace EmulatorsOrganizer
{
    public partial class Form_Settings : Form
    {
        public Form_Settings()
        {
            InitializeComponent();
            // load controls
            Trace.WriteLine("Loading setting controls...", "Settings editor");
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type tp in types)
            {
                if (tp.IsSubclassOf(typeof(ISettingsControl)))
                {
                    controls.Add(Activator.CreateInstance(tp) as ISettingsControl);
                }
            }
            controls.Sort(new ISettingsControlComparer(true));
            foreach (ISettingsControl control in controls)
            {
                control.LoadSettings();
                listBox1.Items.Add(control.ToString());
            }
            listBox1.SelectedIndex = 0;
            Trace.WriteLine("Setting controls loaded successfully.", "Settings editor");
        }
        private List<ISettingsControl> controls = new List<ISettingsControl>();
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            ISettingsControl control = controls[listBox1.SelectedIndex];
            control.Location = new Point(0, 0);
            string test = control.ToString();
            panel1.Controls.Add(control);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Saving settings ....", "Settings editor");
            foreach (ISettingsControl control in controls)
                control.SaveSettings();
            Trace.WriteLine("Settings saved successfully.", "Settings editor");
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Reset settings to defaults.", "Settings editor");
            foreach (ISettingsControl control in controls)
                control.DefaultSettings();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            controls[listBox1.SelectedIndex].DefaultSettings();
            Trace.WriteLine(controls[listBox1.SelectedIndex].ToString() + " settings reset to defaults.", "Settings editor");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
