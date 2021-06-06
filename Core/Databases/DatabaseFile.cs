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
using EmulatorsOrganizer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Represents a database file
    /// </summary>
    public abstract class DatabaseFile
    {
        /// <summary>
        /// Represents a database file
        /// </summary>
        public DatabaseFile()
        {
            LoadAttributes();
            profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        }
        /// <summary>
        /// Load attributes of this database file
        /// </summary>
        protected virtual void LoadAttributes()
        {
            IsVisible = true;
            foreach (Attribute attr in Attribute.GetCustomAttributes(this.GetType()))
            {
                if (attr.GetType() == typeof(DatabaseInfoAttribute))
                {
                    DatabaseInfoAttribute inf = (DatabaseInfoAttribute)attr;
                    this.Name = inf.Name;
                    this.Extensions = inf.Extensions;
                    this.ComparingMode = inf.ComparingMode;
                    this.Exportable = inf.Exportable;
                    this.SupportPerfectMatch = inf.SupportPerfectMatch;
                    this.Separable = inf.Separable;
                }
                if (attr.GetType() == typeof(ForceNesDatabaseAttribute))
                {
                    this.ForceNes = true;
                }
                if (attr.GetType() == typeof(DatabaseVisibiltyAttribute))
                {
                    DatabaseVisibiltyAttribute inf = (DatabaseVisibiltyAttribute)attr;
                    this.IsVisible = inf.IsVisible;
                }
            }
        }
        protected ProfileManager profileManager;

        /// <summary>
        /// Get the database name
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Get the database file extensions
        /// </summary>
        public string[] Extensions { get; protected set; }
        /// <summary>
        /// Get the database file comparing modes
        /// </summary>
        public CompareMode ComparingMode { get; protected set; }
        /// <summary>
        /// Get if this database can be exported.
        /// </summary>
        public bool Exportable { get; protected set; }
        /// <summary>
        /// Get if this database can be separated. Separable
        /// </summary>
        public bool Separable { get; protected set; }
        /// <summary>
        /// Get a value indicates if this format support perfect match option
        /// </summary>
        public bool SupportPerfectMatch { get; protected set; }
        /// <summary>
        /// Get or set the bytes to skip value when calculating CRC/MD5/SHA1
        /// </summary>
        public int BytesToSkip
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set if this database is for nes console.
        /// </summary>
        public bool IsNesConsole
        {
            get;
            set;
        }
        public bool IsVisible { get; protected set; }
        public bool AddFilters { get; protected set; }
        public List<Filter> Filters { get; protected set; }
        /// <summary>
        /// Get if this database is forced to be nes.
        /// </summary>
        public bool ForceNes
        { get; protected set; }
        /// <summary>
        /// Get the options control. This will create new instance of the control for each call.
        /// </summary>
        public virtual Control OptionsControl
        {
            get { return null; }
        }
        /// <summary>
        /// Load a file using this database file, return entries loaded.
        /// </summary>
        /// <param name="filePath">The database file path.</param>
        /// <returns></returns>
        public abstract List<DBEntry> LoadFile(string filePath);
        /// <summary>
        /// Save database entries into a file.
        /// </summary>
        /// <param name="fileName">The file path where to save the file.</param>
        /// <param name="entries">The entries to save.</param>
        public virtual void SaveFile(string fileName, List<DBEntry> entries)
        {
        }
        /// <summary>
        /// Show database options for export; called before the the save process.
        /// </summary>
        public virtual void ShowExportOptions()
        {
        }
        /// <summary>
        /// Get separated items from file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual SeparateItem[] GetSeparate(string filePath)
        { return null; }
        /// <summary>
        /// Apply renaming / data item changes to rom.
        /// </summary>
        /// <param name="rom">The rom to apply name/data item changes on.</param>
        /// <param name="entry">The entry to use</param>
        public virtual void ApplyName(Rom rom, DBEntry entry, bool applyName, bool applyDataInfo)
        {
            if (applyName)
            {
                rom.Name = entry.GetPropertyValue("Name");
                Trace.WriteLine("Rom renamed to: " + rom.Name, this.Name + " database");
            }
            // Database info items
            if (applyDataInfo)
            {
                foreach (PropertyStruct p in entry.Properties)
                {
                    if (p.Property != "Name")
                    {
                        AddNewIC(p.Property, rom.ParentConsoleID);
                        AddDataToRom(p.Property, p.Value, rom);
                    }
                }
            }
            Trace.WriteLine("->Rom data updated.", this.Name + " database");
        }
        /// <summary>
        /// Export database file
        /// </summary>
        /// <param name="fileName">The file path where to write</param>
        /// <param name="dbFiles">The DB entries to use</param>
        /// <returns>True if export operation completed successfully otherwise false.</returns>
        public virtual bool ExportFile(string fileName, List<DBEntry> dbEntries)
        {
            return false;
        }
        protected virtual void AddNewIC(string newic, string consoleID)
        {
            //search the profile for information container that match the same name
            bool found = false;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            foreach (RomData ic in console.RomDataInfoElements)
            {
                if (ic.Name.ToLower() == newic.ToLower())
                {
                    ic.Type = RomDataType.Text;
                    found = true; break;
                }
            }
            if (!found)
            {
                //add new ic with this project
                RomData newIC = new RomData(profileManager.Profile.GenerateID(), newic, RomDataType.Text);
                console.RomDataInfoElements.Add(newIC);
            }
        }
        protected virtual void AddDataToRom(string icName, string data, Rom rom)
        {
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
            foreach (RomData ic in console.RomDataInfoElements)
            {
                if (ic.Name.ToLower() == icName.ToLower())
                {
                    rom.UpdateDataInfoItemValue(ic.ID, data);
                    break;
                }
            }
        }

        /// <summary>
        /// Get file buffer
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="useSkip">If true, the buffer will skip first bytes, number given by BytesToSkip</param>
        /// <returns></returns>
        protected virtual byte[] GetBuffer(string filePath, bool useSkip)
        {
            byte[] fileBuffer;
            if (useSkip)
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[BytesToSkip], 0, BytesToSkip);
                fileBuffer = new byte[fileStream.Length - BytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - BytesToSkip));
                fileStream.Close();
            }
            else
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //  fileStream.Read(new byte[BytesToSkip], 0, BytesToSkip);
                fileBuffer = new byte[fileStream.Length];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length));
                fileStream.Close();
            }
            return fileBuffer;
        }
        /// <summary>
        /// Calculate SHA1 for a file
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="useSkip">If true, the buffer will skip first bytes, number given by BytesToSkip</param>
        /// <returns>SHA1 of the file</returns>
        public virtual string CalculateSHA1(string filePath, bool useSkip)
        {
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = GetBuffer(filePath, useSkip);

                string Sha1 = "";
                SHA1Managed managedSHA1 = new SHA1Managed();
                byte[] shaBuffer = managedSHA1.ComputeHash(fileBuffer);

                foreach (byte b in shaBuffer)
                    Sha1 += b.ToString("x2").ToLower();

                return Sha1;
            }
            return "";
        }
        /// <summary>
        /// Calculate MD5 for a file
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="useSkip">If true, the buffer will skip first bytes, number given by BytesToSkip</param>
        /// <returns>MD5 of the file</returns>
        public virtual string CalculateMD5(string filePath, bool useSkip)
        {
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = GetBuffer(filePath, useSkip);

                string md5 = "";
                MD5 m = MD5.Create();
                byte[] md5Buffer = m.ComputeHash(fileBuffer);

                foreach (byte b in md5Buffer)
                    md5 += b.ToString("x2").ToLower();

                return md5;
            }
            return "";
        }
        /// <summary>
        /// Calculate CRC for a file
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="useSkip">If true, the buffer will skip first bytes, number given by BytesToSkip</param>
        /// <returns>CRC of the file</returns>
        public virtual string CalculateCRC(string filePath, bool useSkip)
        {
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = GetBuffer(filePath, useSkip);

                string crc = "";
                Crc32 crc32 = new Crc32();
                byte[] crc32Buffer = crc32.ComputeHash(fileBuffer);

                foreach (byte b in crc32Buffer)
                    crc += b.ToString("x2").ToLower();

                return crc;
            }
            return "";
        }
        /// <summary>
        /// Dispose this database file.
        /// </summary>
        public virtual void Dispose()
        {
            this.Progress = null;
            this.ProgressFinished = null;
            this.ProgressStarted = null;
            Filters = new List<Filter>();
            AddFilters = false;
        }

        /// <summary>
        /// Raises after a progress start
        /// </summary>
        public event EventHandler<ProgressArgs> ProgressStarted;
        /// <summary>
        /// Raises when a progress occur
        /// </summary>
        public event EventHandler<ProgressArgs> Progress;
        /// <summary>
        /// Raises after a progress finish.
        /// </summary>
        public event EventHandler<ProgressArgs> ProgressFinished;

        /// <summary>
        /// Raise the ProgressStarted event
        /// </summary>
        /// <param name="message">The message</param>
        protected void OnProgressStarted(string message)
        {
            if (ProgressStarted != null)
                ProgressStarted(this, new ProgressArgs(0, message));
        }
        /// <summary>
        /// Raises the ProgressFinished event
        /// </summary>
        /// <param name="message">The message</param>
        protected void OnProgressFinished(string message)
        {
            if (ProgressFinished != null)
                ProgressFinished(this, new ProgressArgs(100, message));
        }
        /// <summary>
        /// Raises the Progress event
        /// </summary>
        /// <param name="complete">Percentage complete</param>
        /// <param name="message">The message</param>
        protected void OnProgress(int complete, string message)
        {
            if (ProgressFinished != null)
                ProgressFinished(this, new ProgressArgs(complete, message));
        }
        /// <summary>
        /// Get the name of this database file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string exes = "";
            foreach (string ex in Extensions)
                exes += ex + ", ";
            if (exes.Length > 1)
                exes = exes.Substring(0, exes.Length - 2);
            return this.Name + " (" + exes + ")";
        }
    }
}
