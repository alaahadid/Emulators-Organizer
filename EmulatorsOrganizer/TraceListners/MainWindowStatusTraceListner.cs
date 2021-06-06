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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer
{
    public class MainWindowStatusTraceListner : TraceListener
    {
        public MainWindowStatusTraceListner()
        {
            enabled = true;
            ServicesManager.DisableWindowListner += ServicesManager_DisableWindowListner;
            ServicesManager.EnableWindowListner += ServicesManager_EisableWindowListner;
        }
        private bool enabled;

        public override void Write(string message)
        {
            if (Program.Form_Main == null) return;
            if (enabled) 
                Program.Form_Main.WriteStatus(message, System.Drawing.Color.Black);
        }
        public override void WriteLine(string message)
        {
            if (Program.Form_Main == null) return;
            if (enabled)
                Program.Form_Main.WriteStatus(message, System.Drawing.Color.Black);
        }
        public override void WriteLine(string message, string category)
        {
            if (Program.Form_Main == null) return;
            if (enabled)
                Program.Form_Main.WriteStatus(category + ": " + message, System.Drawing.Color.Black);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Program.Form_Main == null) return;
            if (enabled)
                switch (eventType)
                {
                    case TraceEventType.Information: Program.Form_Main.WriteStatus(source + ": " + message, System.Drawing.Color.Black); break;
                    case TraceEventType.Warning: Program.Form_Main.WriteStatus(source + ": " + message, System.Drawing.Color.Yellow); break;
                    case TraceEventType.Error: Program.Form_Main.WriteStatus(source + ": " + message, System.Drawing.Color.Red); break;
                    default: Program.Form_Main.WriteStatus(source + ": " + message, System.Drawing.Color.Black); break;
                }
        }
        private void ServicesManager_EisableWindowListner(object sender, EventArgs e)
        {
            enabled = true;
        }
        private void ServicesManager_DisableWindowListner(object sender, EventArgs e)
        {
            enabled = false;
        }
    }
}
