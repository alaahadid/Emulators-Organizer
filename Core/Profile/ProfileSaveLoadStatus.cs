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
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// The profile save/load status
    /// </summary>
    public struct ProfileSaveLoadStatus
    {
        /// <summary>
        /// The profile save/load status
        /// </summary>
        /// <param name="message">Status message</param>
        /// <param name="type">Status type</param>
        public ProfileSaveLoadStatus(string message, ProfileSaveLaodType type)
        {
            this.message = message;
            this.type = type;
        }

        private string message;
        private ProfileSaveLaodType type;
        /// <summary>
        /// Get the status message
        /// </summary>
        public string Message
        { get { return message; } }
        /// <summary>
        /// Get the status type
        /// </summary>
        public ProfileSaveLaodType Type
        { get { return type; } }
    }
    /// <summary>
    /// Profile save/load type
    /// </summary>
    public enum ProfileSaveLaodType
    {
        /// <summary>
        /// Profile saved successfuly
        /// </summary>
        Success,
        /// <summary>
        /// Profile not saved
        /// </summary>
        Error
    }
}
