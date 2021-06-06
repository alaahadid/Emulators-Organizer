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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MMB;
using EmulatorsOrganizer.Core;
namespace EmulatorsOrganizer.GUI
{
    public partial class ConsolePropertiesInformation : IConsolePropertiesControl
    {
        public ConsolePropertiesInformation()
        {
            InitializeComponent();

        }
        public override string ToString()
        {
            return ls["Title_Information"];
        }
        public override string Description
        {
            get
            {
                return ls["ConsolePropertiesDescription_Information"];
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                if (textBox_rtf.Text.Length > 0)
                {
                    if (!File.Exists(textBox_rtf.Text))
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_InformationFileIsNotExist"],
                            ls["MessageCaption_ConsoleProperties"]);
                        return false;
                    }
                }
                if (textBox_pdf.Text.Length > 0)
                {
                    if (!File.Exists(textBox_pdf.Text))
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_ManualFileIsNotExist"],
                            ls["MessageCaption_ConsoleProperties"]);
                        return false;
                    }
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            textBox_rtf.Text = HelperTools.GetFullPath(profileManager.Profile.Consoles[consoleID].RTFPath);
            textBox_pdf.Text = HelperTools.GetFullPath(profileManager.Profile.Consoles[consoleID].PDFPath);
        }
        public override void SaveSettings()
        {
            if (textBox_rtf.Text.Length > 0)
                profileManager.Profile.Consoles[consoleID].RTFPath = HelperTools.GetFullPath(textBox_rtf.Text);
            else
                profileManager.Profile.Consoles[consoleID].RTFPath = "";
            if (textBox_pdf.Text.Length > 0)
                profileManager.Profile.Consoles[consoleID].PDFPath = HelperTools.GetFullPath(textBox_pdf.Text);
            else
                profileManager.Profile.Consoles[consoleID].PDFPath = "";
        }
        public override void DefaultSettings()
        {
            textBox_rtf.Text = textBox_pdf.Text = "";
        }
        private string GetExtensionDialogFilter(string type, string[] extensions)
        {
            string filter = type + " |";
            foreach (string ex in extensions)
            {
                filter += "*" + ex + ";";
            }
            return filter.Substring(0, filter.Length - 1);
        }
        // Clear rtf
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_rtf.Text = "";
        }
        // Clear pdf
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_pdf.Text = "";
        }
        // Change rtf
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = string.Format("{0} '{1}' {2}", ls["Title_OpenFile"], profileManager.Profile.Consoles[consoleID].Name,
                ls["Title_ConsoleInformation"]);
            op.Filter = GetExtensionDialogFilter("Information", new string[] { ".txt", ".rtf", ".doc" });
            op.FileName = textBox_rtf.Text;
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                textBox_rtf.Text = op.FileName;
            }
        }
        // Change pdf
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = string.Format("{0} '{1}' {2}", ls["Title_OpenFile"], profileManager.Profile.Consoles[consoleID].Name,
                ls["Title_ConsoleManual"]);
            op.Filter = GetExtensionDialogFilter("Manual", new string[] { ".pdf" });
            op.FileName = textBox_pdf.Text;
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                textBox_pdf.Text = op.FileName;
            }
        }
    }
}
