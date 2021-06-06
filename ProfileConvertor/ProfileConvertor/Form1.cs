/* This file is part of Emulators Organizer
   A program that can organize roms and emulators

   Copyright © Ali Ibrahim Hadid and Ala Ibrahim Hadid 2009 - 2014

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
using System.Windows.Forms;
using System.IO;
using EmulatorsOrganizer.Services;
namespace ProfileConvertor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Open Emulators Organizer version 5 Profile";
            op.Title = "Emulators Organizer version 5 Profile (*.eop)|*.eop";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_version5.Text = op.FileName;
                if (textBox_version6.Text.Length == 0)
                    textBox_version6.Text = Path.GetDirectoryName(op.FileName) + "\\" +
                        Path.GetFileNameWithoutExtension(op.FileName) + " (CONVERTED).eop";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save Emulators Organizer version 6 Profile";
            save.Title = "Emulators Organizer version 6 Profile (*.eop)|*.eop";
            if (save.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_version6.Text = save.FileName;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Start
        private void button3_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox_version5.Text))
            {
                MessageBox.Show("Please browse for the version 5 profile.");
                return;
            }
            if (textBox_version6.Text.Length == 0)
            {
                MessageBox.Show("Please browse where to save the version 6 profile.");
                return;
            }
            AHD.EO.Base.ProfilesManager version5PRofile = new AHD.EO.Base.ProfilesManager();
            if (!version5PRofile.Load(textBox_version5.Text))
            {
                MessageBox.Show("Unable to load the profile of version 5.", "Emulators Organizer Profile Converter",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 1 Create version 6 profile
            EmulatorsOrganizer.Core.ProfileManager version6Profile = (EmulatorsOrganizer.Core.ProfileManager)ServicesManager.GetService("Profile Manager");
            version6Profile.NewProfile();
            // 2 Copy categories
            foreach (AHD.EO.Base.ConsolesGroup gr in version5PRofile.Profile.ConsoleGroups)
            {
                // Create group
                EmulatorsOrganizer.Core.ConsolesGroup newGR = new EmulatorsOrganizer.Core.ConsolesGroup(version6Profile.Profile.GenerateID());
                newGR.Icon = gr.Icon;
                newGR.Name = gr.Name;
                version6Profile.Profile.ConsoleGroups.Add(newGR);
                // Add consoles of this group
                foreach (AHD.EO.Base.Console con in gr.Consoles)
                {
                    EmulatorsOrganizer.Core.Console newConsole = new EmulatorsOrganizer.Core.Console(version6Profile.Profile.GenerateID(), newGR.ID);
                    newConsole.Name = con.Name;
                    newConsole.ExtractRomIfArchive = con.ExtractRomFirstIfArchive;
                    // Extensions
                    newConsole.Extensions = new List<string>();
                    foreach (string ex in con.Extensions)
                    {
                        string newEX = ex;
                        if (!newEX.StartsWith("."))
                            newEX = "." + newEX;
                        newConsole.Extensions.Add(newEX);
                    }
                    newConsole.Icon = con.Icon;
                    Dictionary<string, string> emuIDS = new Dictionary<string, string>();
                    // Emulators
                    foreach (AHD.EO.Base.Emulator emu in con.Emulators)
                    {
                        EmulatorsOrganizer.Core.Emulator newEMU = new EmulatorsOrganizer.Core.Emulator(version6Profile.Profile.GenerateID());
                        emuIDS.Add(emu.Name, newEMU.ID);
                        newEMU.Name = emu.Name;
                        newEMU.ExcutablePath = emu.ExecutablePath;
                        newEMU.Icon = emu.Icon;
                        // Commandlines
                        EmulatorsOrganizer.Core.EmulatorParentConsole par = new EmulatorsOrganizer.Core.EmulatorParentConsole(newConsole.ID);
                        par.CommandlineGroups = new List<EmulatorsOrganizer.Core.CommandlinesGroup>();
                        par.CommandlineGroups.Clear();
                        foreach (AHD.EO.Base.CommandlinesGroup comGR in emu.CommandlineGroups)
                        {
                            EmulatorsOrganizer.Core.CommandlinesGroup newComGR = new EmulatorsOrganizer.Core.CommandlinesGroup();
                            newComGR.Enabled = comGR.Enabled;
                            newComGR.IsReadOnly = comGR.IsReadOnly;
                            newComGR.Name = comGR.Name;
                            newComGR.Commandlines = new List<EmulatorsOrganizer.Core.Commandline>();
                            // Commandlines
                            foreach (AHD.EO.Base.Commandline command in comGR.Commandlines)
                            {
                                EmulatorsOrganizer.Core.Commandline newcommand = new EmulatorsOrganizer.Core.Commandline();
                                newcommand.Code = command.Code;
                                newcommand.Enabled = command.Enabled;
                                newcommand.IsReadOnly = command.IsReadOnly;
                                newcommand.Name = command.Name;
                                newcommand.Parameters = new List<EmulatorsOrganizer.Core.Parameter>();
                                // Parameters
                                foreach (AHD.EO.Base.Parameter para in command.Parameters)
                                {
                                    EmulatorsOrganizer.Core.Parameter newpara = new EmulatorsOrganizer.Core.Parameter();
                                    newpara.Code = para.Code;
                                    newpara.Enabled = para.Enabled;
                                    newpara.IsReadOnly = para.IsReadOnly;
                                    newpara.Name = para.Name;
                                    newcommand.Parameters.Add(newpara);
                                }
                                newComGR.Commandlines.Add(newcommand);
                            }
                            par.CommandlineGroups.Add(newComGR);
                        }
                        newEMU.ParentConsoles.Add(par);
                        // Add the emulator
                        version6Profile.Profile.Emulators.Add(newEMU);
                    }
                    newConsole.RomDataInfoElements = new List<EmulatorsOrganizer.Core.RomData>();
                    newConsole.InformationContainers = new List<EmulatorsOrganizer.Core.InformationContainer>();
                    newConsole.InformationContainersMap = new EmulatorsOrganizer.Core.InformationContainerTabsPanel();
                    //        <old id, new id>
                    Dictionary<string, string> icIDS = new Dictionary<string, string>();
                    Dictionary<string, string> dataIDS = new Dictionary<string, string>();
                    // Information containers !
                    foreach (AHD.EO.Base.InformationContainer ic in version5PRofile.Profile.InformationContainers)
                    {
                        if (ic is AHD.EO.Base.RomDataInformationContainer)
                        {
                            EmulatorsOrganizer.Core.RomData newData = new EmulatorsOrganizer.Core.RomData
                                (version6Profile.Profile.GenerateID(), ic.Name, EmulatorsOrganizer.Core.RomDataType.Text);
                            dataIDS.Add(ic.ID, newData.ID);
                            newConsole.RomDataInfoElements.Add(newData);
                        }
                        else if (ic is AHD.EO.Base.InfoTextInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerInfoText newIC =
                                new EmulatorsOrganizer.Core.InformationContainerInfoText(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                        else if (ic is AHD.EO.Base.LinksInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerLinks newIC =
                                new EmulatorsOrganizer.Core.InformationContainerLinks(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                        else if (ic is AHD.EO.Base.ManualsInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerPDF newIC =
                                new EmulatorsOrganizer.Core.InformationContainerPDF(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                        else if (ic is AHD.EO.Base.SoundsInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerMedia newIC =
                                new EmulatorsOrganizer.Core.InformationContainerMedia(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                        else if (ic is AHD.EO.Base.VideosInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerMedia newIC =
                                new EmulatorsOrganizer.Core.InformationContainerMedia(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                        else if (ic is AHD.EO.Base.YoutubeInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerYoutubeVideo newIC =
                                new EmulatorsOrganizer.Core.InformationContainerYoutubeVideo(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                        else if (ic is AHD.EO.Base.RomInfoInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerRomInfo newIC =
                                new EmulatorsOrganizer.Core.InformationContainerRomInfo(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                        else if (ic is AHD.EO.Base.ImagesInformationContainer)
                        {
                            EmulatorsOrganizer.Core.InformationContainerImage newIC =
                                new EmulatorsOrganizer.Core.InformationContainerImage(version6Profile.Profile.GenerateID());
                            newIC.DisplayName = ic.Name;
                            icIDS.Add(ic.ID, newIC.ID);
                            newConsole.InformationContainersMap.ContainerIDS.Add(newIC.ID);
                            newConsole.InformationContainers.Add(newIC);
                        }
                    }
                    newConsole.BuildDefaultColumns(new string[0]);
                    newConsole.FixColumnsForRomDataInfo();

                    version6Profile.Profile.Consoles.Add(newConsole);
                    // Roms
                    foreach (AHD.EO.Base.Rom rom in con.Roms)
                    {
                        EmulatorsOrganizer.Core.Rom newROM = new EmulatorsOrganizer.Core.Rom(version6Profile.Profile.GenerateID());
                        newROM.ParentConsoleID = newConsole.ID;
                        newROM.Categories = new List<string>();
                        foreach (string cat in rom.Categories)
                        {
                            newROM.Categories.Add(cat);
                        }
                        newROM.Icon = rom.IconImage;
                        newROM.Name = rom.Name;
                        newROM.Path = rom.Path;
                        newROM.PlayedTimeLength = (long)rom.PlayedTimeLength;
                        newROM.PlayedTimes = rom.PlayedTimes;
                        newROM.Rating = rom.Rating;
                        newROM.FileSize = DecodeSizeLabel(rom.Size);
                        // Commandlines
                        List<EmulatorsOrganizer.Core.CommandlinesGroup> commandlineGroups = new List<EmulatorsOrganizer.Core.CommandlinesGroup>();
                        foreach (AHD.EO.Base.SpecialCommandlinesGroup sp in rom.SpecialCommandlineGroups)
                        {
                            foreach (AHD.EO.Base.CommandlinesGroup comGR in sp.CommandlinesGroups)
                            {
                                if (comGR.Name == "<default>")
                                    continue;
                                EmulatorsOrganizer.Core.CommandlinesGroup newComGR = new EmulatorsOrganizer.Core.CommandlinesGroup();
                                newComGR.Enabled = comGR.Enabled;
                                newComGR.IsReadOnly = comGR.IsReadOnly;
                                newComGR.Name = comGR.Name;
                                newComGR.Commandlines = new List<EmulatorsOrganizer.Core.Commandline>();
                                // Commandlines
                                foreach (AHD.EO.Base.Commandline command in comGR.Commandlines)
                                {
                                    EmulatorsOrganizer.Core.Commandline newcommand = new EmulatorsOrganizer.Core.Commandline();
                                    newcommand.Code = command.Code;
                                    newcommand.Enabled = command.Enabled;
                                    newcommand.IsReadOnly = command.IsReadOnly;
                                    newcommand.Name = command.Name;
                                    newcommand.Parameters = new List<EmulatorsOrganizer.Core.Parameter>();
                                    // Parameters
                                    foreach (AHD.EO.Base.Parameter para in command.Parameters)
                                    {
                                        EmulatorsOrganizer.Core.Parameter newpara = new EmulatorsOrganizer.Core.Parameter();
                                        newpara.Code = para.Code;
                                        newpara.Enabled = para.Enabled;
                                        newpara.IsReadOnly = para.IsReadOnly;
                                        newpara.Name = para.Name;
                                        newcommand.Parameters.Add(newpara);
                                    }
                                    newComGR.Commandlines.Add(newcommand);
                                }
                                commandlineGroups.Add(newComGR);
                            }
                            newROM.UpdateEmulatorCommandlines(emuIDS[sp.Emulator.Name], commandlineGroups);
                        }
                        // Data elements and information containers !
                        foreach (AHD.EO.Base.InformationContainerItem it in rom.ICItems)
                        {
                            if (it is AHD.EO.Base.RomDataICItem)
                            {
                                if (dataIDS.ContainsKey(it.ContainerID))
                                    newROM.UpdateDataInfoItemValue(dataIDS[it.ContainerID], ((AHD.EO.Base.RomDataICItem)it).Text);
                            }
                            else if (it is AHD.EO.Base.InformationContainerFilesInFolderItem)
                            {
                                if (icIDS.ContainsKey(it.ContainerID))
                                {
                                    EmulatorsOrganizer.Core.InformationContainerItemFiles newIT =
                                        new EmulatorsOrganizer.Core.InformationContainerItemFiles(version6Profile.Profile.GenerateID(), icIDS[it.ContainerID]);
                                    newIT.Files = ((AHD.EO.Base.InformationContainerFilesInFolderItem)it).Files;
                                    newROM.RomInfoItems.Add(newIT);
                                }
                            }
                            else if (it is AHD.EO.Base.InformationContainerLinksItem)
                            {
                                if (icIDS.ContainsKey(it.ContainerID))
                                {
                                    EmulatorsOrganizer.Core.InformationContainerItemLinks newIT =
                                        new EmulatorsOrganizer.Core.InformationContainerItemLinks(version6Profile.Profile.GenerateID(), icIDS[it.ContainerID]);
                                    newIT.Links = new Dictionary<string, string>();
                                    foreach (string link in ((AHD.EO.Base.InformationContainerLinksItem)it).Links)
                                    {
                                        newIT.Links.Add(link, link);
                                    }
                                    newROM.RomInfoItems.Add(newIT);
                                }
                            }
                            else if (it is AHD.EO.Base.InformationContainerYoutubeVideoItem)
                            {
                                if (icIDS.ContainsKey(it.ContainerID))
                                {
                                    EmulatorsOrganizer.Core.InformationContainerItemLinks newIT =
                                        new EmulatorsOrganizer.Core.InformationContainerItemLinks(version6Profile.Profile.GenerateID(), icIDS[it.ContainerID]);
                                    newIT.Links = new Dictionary<string, string>();
                                    foreach (AHD.EO.Base.YoutubeLink link in ((AHD.EO.Base.InformationContainerYoutubeVideoItem)it).Videos)
                                    {
                                        newIT.Links.Add(link.Name, link.Link);
                                    }
                                    newROM.RomInfoItems.Add(newIT);
                                }
                            }
                        }
                        // Add the rom 
                        version6Profile.Profile.Roms.Add(newROM);
                    }
                }
            }
            // Platlists !!
            foreach (AHD.EO.Base.PlaylistsGroup gr in version5PRofile.Profile.PlaylistGroups)
            {
                EmulatorsOrganizer.Core.PlaylistsGroup newGR = new EmulatorsOrganizer.Core.PlaylistsGroup(version6Profile.Profile.GenerateID());
                newGR.Icon = gr.Icon;
                newGR.Name = gr.Name;
                version6Profile.Profile.PlaylistGroups.Add(newGR);
                // Add the playlists
                foreach (AHD.EO.Base.Playlist pl in gr.PlayLists)
                {
                    EmulatorsOrganizer.Core.Playlist newPL =
                        new EmulatorsOrganizer.Core.Playlist(version6Profile.Profile.GenerateID(), newGR.ID);
                    newPL.Icon = pl.Icon;
                    newPL.Name = pl.Name;
                    newPL.RomIDS = new List<string>();
                    // Add roms
                    //  foreach (AHD.EO.Base.Rom rom in pl.Roms)
                    //  {
                    //     newPL.RomIDS.Add(romIDPATH[rom.ConsoleID + rom.Path]);
                    //  }
                }
            }
            // Save the profile !!
            version6Profile.SaveProfile(textBox_version6.Text);
            // Show done :D
            MessageBox.Show("Done !!\n\nCongrats ! your profile can be opened with Emulators Organizer version 6.x.x.x now");
        }
        private long DecodeSizeLabel(string sizeLabel)
        {
            // Let's see given parameter (size)
            string t = sizeLabel.ToLower();
            t = t.Replace("kb", "");
            t = t.Replace("mb", "");
            t = t.Replace("gb", "");
            t = t.Replace(" ", "");
            double value = 0;
            double.TryParse(t, out value);

            if (sizeLabel.ToLower().Contains("kb"))
                value *= 1024;
            else if (sizeLabel.ToLower().Contains("mb"))
                value *= 1024 * 1024;
            else if (sizeLabel.ToLower().Contains("gb"))
                value *= 1024 * 1024 * 1024;

            return (long)value;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
