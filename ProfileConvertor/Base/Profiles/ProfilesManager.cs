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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using AHD.Forms;

namespace AHD.EO.Base
{
    /// <summary>
    /// A class for saving and opening profiles
    /// </summary>

    public class ProfilesManager
    {
        private ResourceManager resources = new ResourceManager("AHD.EO.Base.LanguageResources.Resource",
            Assembly.GetExecutingAssembly());
        private Profile profile;
        private string filePath;

        /// <summary>
        /// Get or set the project
        /// </summary>
        public Profile Profile
        { get { return profile; } set { profile = value; } }
        /// <summary>
        /// Get or set the profile file path
        /// </summary>
        public string FilePath
        { get { return filePath; } set { filePath = value; } }

        /// <summary>
        /// Save the profile into file
        /// </summary>
        /// <param name="FilePath">The file path to save into</param>
        /// <returns>True if save successed, false if not</returns>
        public bool Save(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, profile);
                fs.Close();
                filePath = fileName;
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_CantSaveProject") + "\n\n" + ex.Message
                    + "\n" + ex.ToString(),
                  resources.GetString("MessageCaption_Error"));
            }
            return false;
        }
        /// <summary>
        /// Load profile from file
        /// </summary>
        /// <param name="FilePath">The full file path</param>
        /// <returns>True if load successed, false if not</returns>
        public bool Load(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter formatter = new BinaryFormatter();
                    profile = (Profile)formatter.Deserialize(fs);
                    fs.Close();
                    filePath = fileName;
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_CantLoadProject") + "\n\n" + ex.Message
                    + "\n" + ex.ToString(),
                    resources.GetString("MessageCaption_Error"));
            }
            return false;
        }

        public bool SaveAsXML(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                XmlSerializer formatter = new XmlSerializer(typeof(Profile));
                formatter.Serialize(fs, profile);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_CantSaveProject") + "\n\n" +
                    ex.Message + "\n" + ex.ToString(),
                  resources.GetString("MessageCaption_Error"));
            }
            return false;
        }
        public bool LoadAsXML(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    XmlSerializer formatter = new XmlSerializer(typeof(Profile));
                    profile = (Profile)formatter.Deserialize(fs);
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_CantLoadProject") + "\n\n" +
                    ex.Message + "\n" + ex.ToString(),
                    resources.GetString("MessageCaption_Error"));
            }
            return false;
        }

        /// <summary>
        /// Save console as file
        /// </summary>
        /// <param name="fileName">The full file path</param>
        /// <param name="console">The console to save</param>
        /// <returns>True if save successed, false if not</returns>
        public bool SaveConsoleAsFile(string fileName, AHD.EO.Base.Console console)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, console);
                fs.Close();
                filePath = fileName;
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_CantSaveConsole") + "\n\n" + ex.Message
                    + "\n" + ex.ToString(),
                  resources.GetString("MessageCaption_Error"));
            }
            return false;
        }
        /// <summary>
        /// Load console from file
        /// </summary>
        /// <param name="FilePath">The full file path</param>
        /// <returns>Loaded console if load successed, null if not</returns>
        public AHD.EO.Base.Console LoadConsoleFromFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter formatter = new BinaryFormatter();
                    AHD.EO.Base.Console console = (AHD.EO.Base.Console)formatter.Deserialize(fs);
                    fs.Close();
                    filePath = fileName;
                    return console;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_CantLoadConsole") + "\n\n" + ex.Message
                    + "\n" + ex.ToString(),
                    resources.GetString("MessageCaption_Error"));
            }
            return null;
        }
    }
}
