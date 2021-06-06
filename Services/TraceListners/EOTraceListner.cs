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

namespace EmulatorsOrganizer.Services.TraceListners
{
    public class EOTraceListner : TraceListener
    {
        public EOTraceListner()
        {
            window = new Form_TraceWindow();
            this.Name = "EOTraceListner";
        }

        private Form_TraceWindow window;

        public Form_TraceWindow TraceWindow
        { get { return window; } set { window = value; } }
        public override void Write(string message)
        {
            window.WriteLine(message, "", TraceEventType.Information);
        }
        public override void WriteLine(string message)
        {
            window.WriteLine(message, "", TraceEventType.Information);
        }
        public override void WriteLine(string message, string category)
        {
            window.WriteLine(message, category, TraceEventType.Information);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            window.WriteLine(message, source, eventType);
        }
    }
    public struct TraceLine
    {
        public TraceLine(string message, string category, DateTime time, TraceEventType type)
        {
            this.message = message;
            this.category = category;
            this.time = time;
            this.type = type;
        }
        private string message;
        private string category;
        private DateTime time;
        private TraceEventType type;

        public string Message { get { return message; } set { message = value; } }
        public string Category { get { return category; } set { category = value; } }
        public DateTime Time { get { return time; } set { time = value; } }
        public TraceEventType Type { get { return type; } set { type = value; } }
    }
}
