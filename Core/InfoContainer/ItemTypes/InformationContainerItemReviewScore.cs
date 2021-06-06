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

namespace EmulatorsOrganizer.Core
{
    [Serializable]
    public class InformationContainerItemReviewScore : InformationContainerItem
    {
        public InformationContainerItemReviewScore(string id, string parentID)
            : base(id, parentID)
        {
            Scores = new Dictionary<string, int>();
        }
        /// <summary>
        /// Get game scores as dictionary in formula [field name, score ranged 0-100]
        /// </summary>
        public Dictionary<string, int> Scores { get; set; }
        /// <summary>
        /// Get the total score for this game (calculated)
        /// </summary>
        public int TotalScore
        {
            get
            {
                int val = 0;
                foreach (string k in Scores.Keys)
                {
                    val += Scores[k];
                }
                val /= Scores.Count;
                return val;
            }
        }
    }
}
