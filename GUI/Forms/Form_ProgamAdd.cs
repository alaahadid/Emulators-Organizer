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
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_ProgamAdd : Form
    {
        public Form_ProgamAdd(ProgramProperties properties)
        {
            InitializeComponent();
            this.properties = properties;
            this.textBox_programName.Text = properties.ProgramName;
            this.textBox_arguments.Text = properties.Arguments;
            this.textBox_programPath.Text = properties.ProgramPath;
            this.radioButton_useScript.Checked = properties.BatMode;
            this.richTextBox1.Text = properties.BatScript;
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add(ls["Word_Instant"]);
            this.comboBox1.Items.Add(ls["Word_WaitToFinish"]);
            this.comboBox1.Items.Add(ls["Word_Wait"] + " " + ls["Word_Seconds"]);
            switch (properties.StartMode)
            {
                case ProgramStartMode.INSTANT: this.comboBox1.SelectedIndex = 0; break;
                case ProgramStartMode.WAIT_TO_FINISH: this.comboBox1.SelectedIndex = 1; break;
                case ProgramStartMode.WAIT_SECONDS: this.comboBox1.SelectedIndex = 2; break;
            }
            if (properties.WaitSeconds <= 0)
                properties.WaitSeconds = 1;
            this.numericUpDown1.Value = properties.WaitSeconds;
        }
        private ProgramProperties properties;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        public ProgramProperties Program
        { get { return properties; } }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox_programName.Text == "")
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseEnterProgramNameFirst"],
                ls["MessageCaption_ProgramProperties"]);
                return;
            }
            if (radioButton_useFile.Checked)
            {
                if (!File.Exists(textBox_programPath.Text))
                {
                    ManagedMessageBox.ShowMessage(ls["Message_PleaseBrowseForProgram"],
                    ls["MessageCaption_ProgramProperties"]);
                    return;
                }
            }
            else
            {
                if (richTextBox1.Text.Length == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouMustEnterScript"], ls["MessageCaption_EmulatorProperties"]);
                    return;
                }
            }
            properties.Arguments = textBox_arguments.Text;
            properties.ProgramName = textBox_programName.Text;
            properties.ProgramPath = textBox_programPath.Text;
            properties.BatMode = radioButton_useScript.Checked;
            properties.BatScript = richTextBox1.Text;
            switch (comboBox1.SelectedIndex)
            {
                case 0: properties.StartMode = ProgramStartMode.INSTANT; break;
                case 1: properties.StartMode = ProgramStartMode.WAIT_TO_FINISH; break;
                case 2: properties.StartMode = ProgramStartMode.WAIT_SECONDS; break;
            }
            properties.WaitSeconds = (int)numericUpDown1.Value;
            this.DialogResult = System.Windows.Forms.DialogResult.OK; Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "program (*.*)|*.*";
            op.FileName = textBox_programPath.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_programPath.Text = op.FileName;
                if (textBox_programName.Text == "")
                    textBox_programName.Text = Path.GetFileNameWithoutExtension(op.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = comboBox1.SelectedIndex == 2;
        }
    }
}
