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
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using EmulatorsOrganizer.Services.TraceListners;
namespace EmulatorsOrganizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            // Do application stuff
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;

            // Trace listeners
            bool addTextWritterLogger = false;
            bool showTraceWindow = false;
            if (Args != null)
            {
                if (Args.Contains("/logger"))
                    addTextWritterLogger = true;

                if (Args.Contains("/trace"))
                    showTraceWindow = true;
            }
            ServicesManager.DefineLoggers(addTextWritterLogger, showTraceWindow);
            Trace.WriteLine("Emulators Organizer launched at " + DateTime.Now.ToLocalTime());
            Trace.WriteLine("--------------------------------");
            if (addTextWritterLogger)
            {
                Trace.WriteLine("Text Logger enabled !");
                // Enable auto flush in case of unexpected shutdown.
                Trace.AutoFlush = true;
            }
            if (showTraceWindow)
            {
                EOTraceListner l = (EOTraceListner)Trace.Listeners["EOTraceListner"];
                l.TraceWindow.Show();
            }
            // Helpers tool
            HelperTools.Initialize();
            // Create working folder
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EmulatorsOrganizer\\");
            Directory.CreateDirectory(HelperTools.StartUpPath + "\\Logs\\");
            // Initialize services
            Services.ServicesManager.Initialize();
            // Load languages
            LanguageResourcesService languageService = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
            if (languageService != null)
            {
                languageService.DetectSupportedLanguages();
            }
            else
            {
                Trace.TraceError("Unable to load language resources, 'EO Languages' service failed to load.");
            }
            //Load settings
            SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
            if (settings != null)
            {
                settings.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EmulatorsOrganizer\\GlobalSettings.set";
                settings.LoadSettings();
                // Set language
                languageService.Language = (string)settings.GetValue("Language", true, "English (United States)");
            }
            else
            {
                Trace.TraceError("Unable to load setting, settings service failed to load. Reset to defaults.");
            }

            //show splash and load things
            Form_StartUp startUp = new Form_StartUp(Args);
            startUp.Show();

            Application.Run();
        }
        private static Form_Main frm_main;
        public static string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
        public static string StartUpPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }
        public static Form_Main Form_Main
        { get { return frm_main; } set { frm_main = value; } }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Services.ServicesManager.Close();
            Trace.WriteLine("--------------------------------");
            Trace.Close();
        }
    }
}
