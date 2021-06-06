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
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace EmulatorsOrganizer.Services
{
    /// <summary>
    /// The EO language resources service
    /// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("Name", "EO Language Resources")]
    [ExportMetadata("Description", "Service for initialize language resources of EO. Can be used as resource manager too.")]
    public class LanguageResourcesService : IService
    {
        private string[,] supportedLanguages; // This should filled at startup
        private ResourceManager resources;
        /// <summary>
        /// Initialize the service
        /// </summary>
        public void Initialize()
        {
            resources = new ResourceManager("EmulatorsOrganizer.Services.LanguageResources.Resource",
            Assembly.GetAssembly(this.GetType()));
        }
        /// <summary>
        /// Detect available languages for EO
        /// </summary>
        public void DetectSupportedLanguages()
        {
            Trace.WriteLine("Detecting languages ...", "EO Language Resources");
            string[] langsFolders = Directory.GetDirectories(Application.StartupPath);
            List<string> ids = new List<string>();
            List<string> englishNames = new List<string>();
            List<string> NativeNames = new List<string>();
            foreach (string folder in langsFolders)
            {
                try
                {
                    CultureInfo inf = new CultureInfo(Path.GetFileName(folder));
                    // no errors lol add the id
                    ids.Add(Path.GetFileName(folder));
                    englishNames.Add(inf.EnglishName);
                    NativeNames.Add(inf.NativeName);
                    Trace.WriteLine("Language pack added: " + inf.EnglishName, "EO Language Resources");
                }
                catch
                {
                    Trace.WriteLine("Can't add language pack (" + folder + ")", "EO Language Resources");
                }
            }
            if (ids.Count > 0)
            {
                supportedLanguages = new string[ids.Count, 3];
                for (int i = 0; i < ids.Count; i++)
                {
                    supportedLanguages[i, 0] = englishNames[i];
                    supportedLanguages[i, 1] = ids[i];
                    supportedLanguages[i, 2] = NativeNames[i];
                }
            }
        }
        /// <summary>
        /// Get the supported languages.
        /// </summary>
        public string[,] SupportedLanguages
        { get { return supportedLanguages; } }
        /// <summary>
        /// Get or set the selected language
        /// </summary>
        public string Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.NativeName;
            }
            set
            {
                for (int i = 0; i < SupportedLanguages.Length / 3; i++)
                {
                    if (SupportedLanguages[i, 0] == value)
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(SupportedLanguages[i, 1]);
                        Trace.WriteLine("Language set to: " + supportedLanguages[i, 0], "EO Language Resources");
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Get or set current CultureInfo
        /// </summary>
        public CultureInfo CultureInfo
        { get { return Thread.CurrentThread.CurrentUICulture; } }
        /// <summary>
        /// Get the ResourceManager
        /// </summary>
        public ResourceManager ResourceManager
        { get { return resources; } }
        /// <summary>
        /// Get a string value from resources
        /// </summary>
        /// <param name="value">The value for associated language</param>
        /// <returns>The value for associated language</returns>
        public string GetString(string value)
        {
            return resources.GetString(value);
        }
        /// <summary>
        /// Resources service. Get a string value from resource that associated with selected language (for EmulatorsOrganizer.exe only)
        /// </summary>
        /// <param name="value">The value to get</param>
        /// <returns>Value from resource that associated with selected language</returns>
        public string this[string value]
        {
            get { return GetString(value); }
        }
        /// <summary>
        /// Close the languages resources service
        /// </summary>
        public void Close()
        {
        }
    }
}
