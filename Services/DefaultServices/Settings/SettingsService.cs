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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;

namespace EmulatorsOrganizer.Services.DefaultServices.Settings
{
    /// <summary>
    /// The settings service, handles settings save and load.
    /// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("Name", "Global Settings")]
    [ExportMetadata("Description", "The settings service that handle settings load and save.")]
    public class SettingsService : IService
    {
        private SettingsDataHolder holder;
        private string settingsPath;

        /*Properties*/
        /// <summary>
        /// Get or set the settings path. Default= ".\\settings.set"
        /// </summary>
        public string FilePath
        { get { return settingsPath; } set { settingsPath = value; } }

        /*Methods*/
        /// <summary>
        /// Initialize the settings service. Settings will be load at given file path.
        /// </summary>
        public void Initialize()
        {
            holder = new SettingsDataHolder();
        }
        /// <summary>
        /// Close the settings service
        /// </summary>
        public void Close()
        {

        }
        /// <summary>
        /// Save the settings with <see cref="BinaryFormatter"/> at FilePath location
        /// </summary>
        public void SaveSettings()
        {
            try
            {
                Trace.WriteLine("Saving settings ..", "Global Settings");
                FileStream fs = new FileStream(settingsPath, FileMode.Create, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, holder);
                fs.Close();
                Trace.WriteLine("Settings saved successfully", "Global Settings");
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to save settings: " + ex.Message);
            }
        }
        /// <summary>
        /// Load settings file at FilePath location.
        /// </summary>
        public void LoadSettings()
        {
            FileStream fs = null;
            try
            {
                if (File.Exists(settingsPath))
                {
                    Trace.WriteLine("Loading settings ..", "Global Settings");
                    fs = new FileStream(settingsPath, FileMode.Open, FileAccess.Read);
                    BinaryFormatter formatter = new BinaryFormatter();
                    holder = (SettingsDataHolder)formatter.Deserialize(fs);
                    fs.Close();
                    Trace.WriteLine("Settings loaded successfully", "Global Settings");
                }
                else
                {
                    Trace.TraceError("Unable to load settings: settings file is not exist.");
                }
            }
            catch (Exception ex)
            {
                if (fs != null)
                    fs.Close();
                Trace.TraceError("Unable to load settings: " + ex.Message);
            }
        }
        /// <summary>
        /// Add a value to the settings. If the value name already exist, the value property of this value get replaced.
        /// </summary>
        /// <param name="value">The value to add</param>
        public void AddValue(SettingsValue value)
        {
            if (holder == null)
                holder = new SettingsDataHolder();
            if (holder.Values == null)
                holder.Values = new List<SettingsValue>();

            bool found = false;
            foreach (SettingsValue val in holder.Values)
            {
                if (val.Name == value.Name)
                {
                    found = true;
                    val.Value = value.Value;
                    break;
                }
            }
            if (!found)
            {
                holder.Values.Add(value);
            }
        }
        /// <summary>
        /// Add a value to the settings. If the value name already exist, the value property of this value get replaced.
        /// </summary>
        /// <param name="name">The value's name</param>
        /// <param name="value">The value's value</param>
        public void AddValue(string name, object value)
        {
            SettingsValue settingsValue = new SettingsValue(name, value);
            if (holder == null)
                holder = new SettingsDataHolder();
            if (holder.Values == null)
                holder.Values = new List<SettingsValue>();

            bool found = false;
            foreach (SettingsValue val in holder.Values)
            {
                if (val.Name == settingsValue.Name)
                {
                    found = true;
                    val.Value = settingsValue.Value;
                    break;
                }
            }
            if (!found)
            {
                holder.Values.Add(settingsValue);
            }
        }
        /// <summary>
        /// Get value
        /// </summary>
        /// <param name="name">The name of the settings value. Case sensitive.</param>
        /// <returns>The value found for given name. Null if not found.</returns>
        public object GetValue(string name)
        {
            return GetValue(name, false, null);
        }
        /// <summary>
        /// Get value
        /// </summary>
        /// <param name="name">The name of the settings value. Case sensitive.</param>
        /// <param name="addIfNotFound">If the value is not exist, add a new settings value object using given name and value given at next parameter</param>
        /// <param name="defaultValue">If parameter addIfNotFound=true, use this value to create new settings value object and added to the settings stack</param>
        /// <returns>The value found for given name. Null if not found.</returns>
        public object GetValue(string name, bool addIfNotFound, object defaultValue)
        {
            if (holder.Values != null)
            {
                foreach (SettingsValue val in holder.Values)
                {
                    if (val.Name == name)
                    {
                        return val.Value;
                    }
                }
            }
            // It reached here so value not found ...
            if (addIfNotFound)
            {
                holder.Values.Add(new SettingsValue(name, defaultValue));
                return defaultValue;
            }
            else
            {
                return null;
            }
        }
    }
}
