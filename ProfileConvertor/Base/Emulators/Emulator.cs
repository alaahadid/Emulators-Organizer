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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace AHD.EO.Base
{
    [Serializable()]
    [XmlInclude(typeof(Bitmap))]
    public class Emulator
    {
        private string name = "";
        private string folder = "";
        private string executablePath = "";
        private Image icon;
        private List<CommandlinesGroup> commandlineGroups;
        List<ConfigurationFile> configFiles = new List<ConfigurationFile>();
        private List<ProgramProperties> programsToLaunchBefore = new List<ProgramProperties>();
        private List<ProgramProperties> programsToLaunchAfter = new List<ProgramProperties>();

        public string Name
        { get { return name; } set { name = value; } }
        public string Folder
        { get { return folder; } set { folder = value; } }
        public string ExecutablePath
        { get { return executablePath; } set { executablePath = value; } }
        [XmlIgnore()]
        public Image Icon
        { get { return icon; } set { icon = value; } }
        public List<CommandlinesGroup> CommandlineGroups
        {
            get
            {
                if (commandlineGroups == null)
                    commandlineGroups = new List<CommandlinesGroup>();
                if (!IsDefaultGroupExist())
                {
                    CommandlinesGroup gr = new CommandlinesGroup();
                    gr.Name = "<default>";
                    gr.IsReadOnly = true;
                    gr.Enabled = true;

                    Commandline cm = new Commandline();
                    cm.Name = "<default>";
                    cm.Code = "<rompath>";
                    cm.Enabled = true;
                    cm.IsReadOnly = true;
                    gr.Commandlines.Add(cm);

                    commandlineGroups.Add(gr);
                }
                return commandlineGroups;
            }
            set
            {
                commandlineGroups = value;
                if (commandlineGroups == null)
                    commandlineGroups = new List<CommandlinesGroup>();
                if (!IsDefaultGroupExist())
                {
                    CommandlinesGroup gr = new CommandlinesGroup();
                    gr.Name = "<default>";
                    gr.IsReadOnly = true;
                    gr.Enabled = true;

                    Commandline cm = new Commandline();
                    cm.Name = "<default>";
                    cm.Code = "<rompath>";
                    cm.Enabled = true;
                    cm.IsReadOnly = true;
                    gr.Commandlines.Add(cm);

                    commandlineGroups.Add(gr);
                }
            }
        }
        bool IsDefaultGroupExist()
        {
            foreach (CommandlinesGroup gr in commandlineGroups)
            {
                if (gr.IsReadOnly && gr.Name == "<default>")
                    return true;
            }
            return false;
        }
        public AHD.EO.Base.Console GetParentConsole(Profile profile)
        {
            foreach (ConsolesGroup gr in profile.ConsoleGroups)
            {
                foreach (AHD.EO.Base.Console cn in gr.Consoles)
                {
                    foreach (Emulator emu in cn.Emulators)
                    {
                        if (emu.Name == this.name)
                            return cn;
                    }
                }
            }
            return null;
        }
        public List<ConfigurationFile> ConfigFiles
        { get { return configFiles; } set { configFiles = value; } }
        public List<ProgramProperties> ProgramsToLaucnhBefore
        { get { return programsToLaunchBefore; } set { programsToLaunchBefore = value; } }
        public List<ProgramProperties> ProgramsToLaucnhAfter
        { get { return programsToLaunchAfter; } set { programsToLaunchAfter = value; } }
        public override string ToString()
        {
            return name;
        }
    }
}
