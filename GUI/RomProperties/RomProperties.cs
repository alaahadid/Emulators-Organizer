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
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer.GUI
{
    public partial class RomProperties : Form
    {
        public RomProperties(string romID, string consoleID)
        {
            this.consoleID = consoleID;
            InitializeComponent();
            // Load all roms of selected console
            Rom[] roms = profileManager.Profile.Roms[consoleID, false];
            romIDs = new List<string>();
            foreach (Rom rom in roms)
                romIDs.Add(rom.ID);
            Trace.WriteLine("Loading property controls ...", "Rom properties");
            // Load controls
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type tp in types)
            {
                if (tp.IsSubclassOf(typeof(IRomPropertiesControl)))
                {
                    controls.Add(Activator.CreateInstance(tp) as IRomPropertiesControl);
                }
            }
            controls.Sort(new RomPropertiesControlComparer(true));
            Trace.WriteLine("Rom property controls loaded successfully.", "Rom properties");

            selectedRomIndex = romIDs.IndexOf(romID);
            UpdatebuttonsState();
            LoadRomAtIndex(true);
            listBox1.SelectedIndex = generalControlIndex;
        }
        private List<string> romIDs;
        private string consoleID;
        private int selectedRomIndex;
        private int generalControlIndex = 0;
        private List<IRomPropertiesControl> controls = new List<IRomPropertiesControl>();
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");

        private void LoadRomAtIndex(bool load)
        {
            int i = 0;
            foreach (IRomPropertiesControl control in controls)
            {
                control.RomID = romIDs[selectedRomIndex];
                control.ConsoleID = consoleID;
                control.LoadSettings();
                if (load)
                    listBox1.Items.Add(control.ToString());

                if (control.ToString() == ls["Title_General"])
                    generalControlIndex = i;
                i++;
            }
        }
        private bool IsOK()
        {
            Trace.WriteLine("Saving Rom properties ...", "Rom properties");
            int index = 0;
            bool refreshRequired = false;
            foreach (IRomPropertiesControl control in controls)
            {
                if (control.CanSaveSettings)
                {
                    control.SaveSettings();
                    if (control.RomsRefreshRequired && !refreshRequired)
                        refreshRequired = true;
                }
                else
                {
                    Trace.TraceWarning("Unable to save properties !");
                    listBox1.SelectedIndex = index;
                    return false;
                }
                index++;
            }
            profileManager.Profile.OnRomPropertiesChanged(profileManager.Profile.Roms[romIDs[selectedRomIndex]].Name,
                profileManager.Profile.Roms[romIDs[selectedRomIndex]].ID, refreshRequired);
            Trace.WriteLine("Rom properties saved successfully.", "Rom properties");
            return true;
        }
        private void UpdatebuttonsState()
        {
            button_previous.Enabled = selectedRomIndex > 0;
            button_Next.Enabled = selectedRomIndex < romIDs.Count;
            label_selectionInfo.Text = (selectedRomIndex + 1) + " / " + romIDs.Count;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            IRomPropertiesControl control = controls[listBox1.SelectedIndex];
            control.Location = new Point(0, 0);
            string test = control.ToString();
            panel1.Controls.Add(control);

            label_desc.Text = control.Description;
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsOK())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }
        // Defaults all
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (IRomPropertiesControl control in controls)
                control.DefaultSettings();
            Trace.WriteLine("All rom properties reset to default.", "Rom properties");
        }
        // Defaults
        private void button3_Click(object sender, EventArgs e)
        {
            controls[listBox1.SelectedIndex].DefaultSettings();
            Trace.WriteLine(controls[listBox1.SelectedIndex].ToString() + " properties reset to default.", "Rom properties");
        }
        // Next
        private void button5_Click(object sender, EventArgs e)
        {
            if (IsOK())
            {
                selectedRomIndex++;
                LoadRomAtIndex(false);
                UpdatebuttonsState();
            }
        }
        // Previous
        private void button_previous_Click(object sender, EventArgs e)
        {
            if (IsOK())
            {
                selectedRomIndex--;
                LoadRomAtIndex(false);
                UpdatebuttonsState();
            }
        }
    }
}
