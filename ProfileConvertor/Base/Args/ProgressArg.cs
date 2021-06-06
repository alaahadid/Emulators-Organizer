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
    /// <summary>
    /// Args fo progress events
    /// </summary>
    public class ProgressArg : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="progress">Precentage Completed, 0 - 100</param>
        /// <param name="status">Current status of this progress</param>
        public ProgressArg(int progress, string status)
        {
            this.progress = progress;
            this.status = status;
        }
        int progress = 0;
        string status = "";
        /// <summary>
        /// Get the progress completed in precentage, 0 - 100
        /// </summary>
        public int PrecentageCompleted
        { get { return progress; } }
        /// <summary>
        /// Get current status of the progress
        /// </summary>
        public string Status
        { get { return status; } }
    }
}
