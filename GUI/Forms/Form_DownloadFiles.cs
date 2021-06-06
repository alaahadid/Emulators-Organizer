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
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Reflection;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_DownloadFiles : Form
    {
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private Thread mainThread;
        private string _folder;
        private string[] _links;
        private List<string> Downloaded = new List<string>();
        private WebClient client = new WebClient();
        private string NameOfSavedFiles;
        public List<string> DownloadedPaths
        { get { return Downloaded; } }
        delegate void SetIntValue(int value, int index);
        delegate void SetIntvalue(int value);
        delegate void SetStringValue(string Value);
        delegate void DoVerb();
        delegate bool CheckItemImage(int value);
        void SetProgress(int value)
        {
            try
            {
                if (!this.InvokeRequired)
                    SetProgress1(value);
                else
                    this.Invoke(new SetIntvalue(SetProgress1), new object[] { value });
            }
            catch { }
        }
        void SetProgress1(int value)
        {
            progressBar1.Value = value;
        }
        bool CheckItemImageindex(int value)
        {
            try
            {
                if (!this.InvokeRequired)
                    return CheckItemImageindex1(value);
                else
                    return (bool)this.Invoke(new CheckItemImage(CheckItemImageindex1), new object[] { value });
            }
            catch { }
            return false;
        }
        bool CheckItemImageindex1(int value)
        {
            try
            {
                return listView1.Items[value].ImageIndex == 2;
            }
            catch { } return false;
        }
        void SetStatus(string value)
        {
            try
            {
                if (!this.InvokeRequired)
                    SetStatus1(value);
                else
                    this.Invoke(new SetStringValue(SetStatus1), new object[] { value });
            }
            catch { }
        }
        void SetStatus1(string value)
        {
            label1.Text = value;
        }
        void CloseSuccess()
        {
            if (!this.InvokeRequired)
            {
                CloseSuccess1();
            }
            else
            {
                this.Invoke(new DoVerb(CloseSuccess1));
            }
        }
        void CloseSuccess1()
        { DialogResult = System.Windows.Forms.DialogResult.OK; Close(); }
        void SetImageIndex(int value, int index)
        {
            try
            {
                if (!this.InvokeRequired)
                {
                    SetImageIndex1(value, index);
                }
                else
                {
                    this.Invoke(new SetIntValue(SetImageIndex1), new object[] { value, index });
                }
            }
            catch { }
        }
        void SetImageIndex1(int value, int index)
        { listView1.Items[index].ImageIndex = value; }

        public Form_DownloadFiles(string folderWhereToSave, string[] links, string NameOfSavedFiles)
        {
            this.NameOfSavedFiles = NameOfSavedFiles;
            _folder = folderWhereToSave;
            _links = links;
            InitializeComponent();
            //Add links
            foreach (string link in _links)
            {
                listView1.Items.Add(link);
                listView1.Items[listView1.Items.Count - 1].ImageIndex = 0;
            }
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            mainThread = new Thread(new ThreadStart(DownloadNext));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SetProgress(e.ProgressPercentage);
            string status = e.ProgressPercentage + "% , " + HelperTools.GetSize(e.BytesReceived) + " of " +
                HelperTools.GetSize(e.TotalBytesToReceive);
            SetStatus(status);
        }
        void DownloadNext()
        {
            int j = 0;
            for (int FileIndex = 0; FileIndex < _links.Length; FileIndex++)
            {
                if (CheckItemImageindex(FileIndex))
                    continue;
                SetImageIndex(1, FileIndex);
                try
                {
                    SetStatus(ls["Status_Downloading"]);
                    Uri uri = new Uri(_links[FileIndex]);
                    string[] splited = _links[FileIndex].Split(new char[] { '/' });
                    string ext = Path.GetExtension(splited[splited.Length - 1]);

                    while (File.Exists(Path.GetFullPath(_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext)))
                        j++;

                    client.DownloadFile(uri, Path.GetFullPath(_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext));
                    SetImageIndex(2, FileIndex);
                    Application.DoEvents();
                    Downloaded.Add(_folder + "\\" + NameOfSavedFiles + "(" + (j + 1).ToString() + ")" + ext);
                    j++;
                    Application.DoEvents();
                }
                catch
                {
                    SetImageIndex(3, FileIndex);
                }
            }
            if (_links.Length == Downloaded.Count)
                CloseSuccess();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (mainThread != null)
                if (mainThread.IsAlive)
                {
                    client.CancelAsync();
                    mainThread.Abort();
                }
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        private void Frm_DownloadFiles_Shown(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (mainThread != null)
                if (mainThread.IsAlive)
                {
                    client.CancelAsync();
                    mainThread.Abort();
                }
            mainThread = new Thread(new ThreadStart(DownloadNext));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (mainThread != null)
                if (mainThread.IsAlive)
                {
                    client.CancelAsync();
                    mainThread.Abort();
                }
            CloseSuccess();
        }
        private void copyLinkToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                Clipboard.SetData(DataFormats.Text, listView1.SelectedItems[0].Text);
        }
    }
}
