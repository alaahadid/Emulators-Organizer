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

namespace AHD.EO.Base
{
    [Serializable()]
    public class SpecialCommandlinesGroup
    {
        Emulator emu;
        List<CommandlinesGroup> commandlineGroups = new List<CommandlinesGroup>();

        public Emulator Emulator
        { get { return emu; } set { emu = value; } }
        public List<CommandlinesGroup> CommandlinesGroups
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

        public SpecialCommandlinesGroup Clone()
        {
            SpecialCommandlinesGroup newG = new SpecialCommandlinesGroup();
            newG.emu = this.emu;
            newG.commandlineGroups = new List<CommandlinesGroup>();
            foreach (CommandlinesGroup gr in this.commandlineGroups)
            {
                newG.commandlineGroups.Add(gr.Clone());
            }
            return newG;
        }
    }
}
