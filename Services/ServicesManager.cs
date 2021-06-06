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
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using EmulatorsOrganizer.Services.TraceListners;

namespace EmulatorsOrganizer.Services
{
    /// <summary>
    /// The services class. Include all services of EO such as language resources, settings ...etc.
    /// </summary>
    public sealed class ServicesManager
    {
        [ImportMany]
        private static IEnumerable<Lazy<IService, IServiceInfo>> services;
        private static CompositionContainer _container;
        private static string startupPath;
        /// <summary>
        /// Find then initialize all services that found.
        /// </summary>
        public static void Initialize()
        {
            Trace.WriteLine("Initializing services ..", "Services");
            FindServices();
        }
        /// <summary>
        /// Search all assemblies and find all services within. 
        /// </summary>
        public static void FindServices()
        {
            Trace.WriteLine("Looking for services ..", "Services");
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ServicesManager).Assembly));
            startupPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            if (startupPath == "")
                startupPath = ".\\";
            catalog.Catalogs.Add(new DirectoryCatalog(Path.GetFullPath(startupPath)));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                _container.ComposeParts(typeof(ServicesManager));
                services = _container.GetExports<IService, IServiceInfo>();
            }
            catch (CompositionException compositionException)
            {
                Trace.TraceError(compositionException.ToString());
            }
            foreach (Lazy<IService, IServiceInfo> service in services)
            {
                service.Value.Initialize();
                Trace.WriteLine("Service added: " + service.Metadata.Name, "Services");
            }
        }
        /// <summary>
        /// Get service by name
        /// </summary>
        /// <param name="serviceName">The service name. The name of service as presented in Service.Name property. Not case sensitive.</param>
        /// <returns>The service if found otherwise null.</returns>
        public static IService GetService(string serviceName)
        {
            if (services != null)
            {
                foreach (Lazy<IService, IServiceInfo> service in services)
                {
                    if (service.Metadata.Name.ToLower() == serviceName.ToLower())
                        return service.Value;
                }
            }
            return null;
        }
        /// <summary>
        /// Close all services.
        /// </summary>
        public static void Close()
        {
            if (services != null)
            {
                Trace.WriteLine("Closing services ...", "Services");
                foreach (Lazy<IService, IServiceInfo> service in services)
                {
                    service.Value.Close();
                }
            }
        }
        /// <summary>
        /// Define the trace listeners
        /// </summary>
        /// <param name="AddTextWriter">Indicate whether to add the text writer listener</param>
        public static void DefineLoggers(bool AddTextWriter, bool showTraceWindow)
        {
            //Trace.AutoFlush = true;
            Trace.Listeners.Clear();
            // Add console listener; only on debug !
#if DEBUG
            Trace.Listeners.Add(new ConsoleTraceListener());
#endif
            // add text writer listener
            if (AddTextWriter)
            {
                string logPath = ".\\Logs\\" + DateTime.Now.ToLocalTime().ToString() + "-EO master log.txt";
                logPath = logPath.Replace(":", "");
                logPath = logPath.Replace("/", "-");
                Trace.Listeners.Add(new TextWriterTraceListener(logPath));
            }
            // add EO writer listener
            if (showTraceWindow)
                Trace.Listeners.Add(new EOTraceListner());
        }
        /// <summary>
        /// Get available services
        /// </summary>
        public static IEnumerable<Lazy<IService, IServiceInfo>> Services
        { get { return services; } }

        public static string StartupPath
        { get { return startupPath; } }

        /// <summary>
        /// Raised when a component requests to disable the main window trace listener
        /// </summary>
        public static event EventHandler DisableWindowListner;
        /// <summary>
        /// Raised when a component requests to enable the main window trace listener
        /// </summary>
        public static event EventHandler EnableWindowListner;
        /// <summary>
        /// Raises the DisableWindowListner event
        /// </summary>
        public static void OnDisableWindowListner()
        {
            if (DisableWindowListner != null)
                DisableWindowListner(null, null);
        }
        /// <summary>
        /// Raises the EnableWindowListner
        /// </summary>
        public static void OnEnableWindowListner()
        {
            if (EnableWindowListner != null)
                EnableWindowListner(null, null);
        }
    }
}
