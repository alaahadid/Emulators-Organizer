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
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class ConsoleProperties : Form
    {
        public ConsoleProperties(string consoleID, string preferedPage)
        {
            InitializeComponent();
            this.consoleID = consoleID;
            Trace.WriteLine("Loading property controls ...", "Console properties");
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type tp in types)
            {
                if (tp.IsSubclassOf(typeof(IConsolePropertiesControl)))
                {
                    controls.Add(Activator.CreateInstance(tp) as IConsolePropertiesControl);
                }
            }
            controls.Sort(new ConsolePropertiesControlComparer(true));
            int index = 0;
            int i = 0;
            foreach (IConsolePropertiesControl control in controls)
            {
                control.ConsoleID = consoleID;
                control.LoadSettings();
                listBox1.Items.Add(control.ToString());
                if (control.ToString() == preferedPage)
                    index = i;
                i++;
            }
            listBox1.SelectedIndex = index;
            Trace.WriteLine("Console property controls loaded successfully.", "Console properties");
        }
        private string consoleID;
        private List<IConsolePropertiesControl> controls = new List<IConsolePropertiesControl>();
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            IConsolePropertiesControl control = controls[listBox1.SelectedIndex];
            control.Location = new Point(0, 0);
            string test = control.ToString();
            panel1.Controls.Add(control);

            label_desc.Text = control.Description;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Saving console properties ...", "Console properties");
            int index = 0;
            foreach (IConsolePropertiesControl control in controls)
            {
                if (control.CanSaveSettings)
                    control.SaveSettings();
                else
                {
                    Trace.TraceWarning("Unable to save properties !");
                    listBox1.SelectedIndex = index;
                    return;
                }
                index++;
            }
            profileManager.Profile.OnConsolePropertiesChanged(profileManager.Profile.Consoles[consoleID].Name);
            Trace.WriteLine("Console properties saved successfully.", "Console properties");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        // Defaults all
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (IConsolePropertiesControl control in controls)
                control.DefaultSettings();
            Trace.WriteLine("All console properties reset to default.", "Console properties");
        }
        // Defaults
        private void button3_Click(object sender, EventArgs e)
        {
            controls[listBox1.SelectedIndex].DefaultSettings();
            Trace.WriteLine(controls[listBox1.SelectedIndex].ToString() + " properties reset to default.", "Console properties");
        }
        /*/ save style
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = ls["Description_SaveStyle"];
                sav.Filter = ls["Filter_Style"];
                if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_ThisWillSaveAndApplyCurrentStyleToConsoleContinue"],
                        ls["Description_SaveStyle"]);
                    if (result.ClickedButtonIndex == 0)
                    {
                        foreach (IConsolePropertiesControl control in controls)
                        {
                            if (control is ConsolePropertiesIconAndBackground || control is ConsolePropertiesListViewStyle || 
                                control is ConsolePropertiesTabControlStyle)
                                control.SaveSettings();
                        }
                        Trace.WriteLine("Saving style as ...", "Console properties");
                        FileStream fs = new FileStream(sav.FileName, FileMode.Create, FileAccess.Write);
                        XmlSerializer formatter = new XmlSerializer(typeof(EOStyle));
                        formatter.Serialize(fs, profileManager.Profile.Consoles[consoleID].Style);
                        fs.Close();
                        Trace.WriteLine("Style saved successfully.", "Console properties");
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to save style: " + ex.Message, "Console properties");
            }
        }
        // load style
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = ls["Description_LoadStyle"];
                op.Filter = ls["Filter_Style"];
                if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    Trace.WriteLine("Loading style ...", "Console properties");
                    FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                    XmlSerializer formatter = new XmlSerializer(typeof(EOStyle));
                    EOStyle style = (EOStyle)formatter.Deserialize(fs);
                    profileManager.Profile.Consoles[consoleID].Style = style;
                    foreach (IConsolePropertiesControl control in controls)
                    {
                        if (control is ConsolePropertiesIconAndBackground || control is ConsolePropertiesListViewStyle 
                            || control is ConsolePropertiesTabControlStyle)
                            control.LoadSettings();
                    }
                    fs.Close();
                    Trace.WriteLine("Style loaded successfully.", "Console properties");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to load style: " + ex.Message, "Console properties");
            }
        }*/
    }
}
