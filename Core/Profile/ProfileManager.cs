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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel.Composition;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Profile manager that handle profile save and load.
    /// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("Name", "Profile Manager")]
    [ExportMetadata("Description", "The class that handles profile save and load.")]
    public class ProfileManager : IService
    {
        private Profile profile;
        private LanguageResourcesService ls;
        private string filePath;
        private string filePathOfCache;
        private string temp_profile_path;
        private string temp_file;
        private RomsCollection romsCollection;
        private FileStream fs = null;
        private Thread mainThread;
        // Cache streams
        private FileStream cache_stream;
        private long cache_after_profile_point;
        public struct CachedConsoleInfo
        {
            public string ID;
            public int RomsCount;
            public int RomsTotalSize;
            /// <summary>
            /// Point within the stream where the header of this console starts.
            /// </summary>
            public long PointOnTheStream;
            public static CachedConsoleInfo Empty
            {
                get { return new CachedConsoleInfo(); }
            }
        }
        public struct CachedRomData
        {
            public string ID;
            public int DataSizeCompressed;
            public byte[] DataCompressed;
        }

        /// <summary>
        /// Raised before a new profile created
        /// </summary>
        public event EventHandler CreatingNewProfile;
        /// <summary>
        /// Raised after a new profile created
        /// </summary>
        public event EventHandler NewProfileCreated;
        /// <summary>
        /// Raised after a profile opened successfully
        /// </summary>
        public event EventHandler ProfileOpened;
        /// <summary>
        /// Raised before open a profile
        /// </summary>
        public event EventHandler OpeningProfile;
        /// <summary>
        /// Disable the style saving
        /// </summary>
        public event EventHandler DisableStyleSave;
        /// <summary>
        /// Disable the style saving
        /// </summary>
        public event EventHandler EnableStyleSave;

        public event EventHandler ProfileSavingStarted;
        public event EventHandler ProfileSavingFinished;
        public event EventHandler CacheLoadStarted;
        public event EventHandler CacheLoadFinished;
        public event EventHandler<ProgressArgs> Progress;

        /*Properties*/
        /// <summary>
        /// Get or set the profile
        /// </summary>
        public Profile Profile
        { get { return profile; } set { profile = value; } }
        /// <summary>
        /// Get or set the profile file path.
        /// </summary>
        public string FilePath
        { get { return filePath; } set { filePath = value; } }
        public bool IsCacheAvailable
        { get; private set; }
        public bool IsSaving { get; private set; }
        /*Methods*/
        /// <summary>
        /// Initialize profile manager service
        /// </summary>
        public void Initialize()
        {
            profile = new Profile();
        }
        /// <summary>
        /// Create new profile
        /// </summary>
        public void NewProfile()
        {
            if (CreatingNewProfile != null)
                CreatingNewProfile(this, new EventArgs());
            LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
            Trace.WriteLine("Creating new profile...", "Profile Manager");

            // Dispose
            if (profile != null)
            {
                profile.Dispose();
                profile = null;
            }

            // Create new
            profile = new Profile();
            profile.Name = ls["Untitled"];
            filePath = "";
            OnNewProfile();
            Trace.WriteLine("New profile created successfully.", "Profile Manager");
        }
        /// <summary>
        /// Save profile at FilePath location
        /// </summary>
        /// <param name="useThread">Indicates if threading should be used</param>
        /// <returns>The ProfileSaveLoadStatus object holds operation status</returns>
        public ProfileSaveLoadStatus SaveProfile(bool useThread)
        { return SaveProfile(filePath, useThread); }
        /// <summary>
        /// Save profile at given location
        /// </summary>
        /// <param name="fileName">The profile complete location</param>
        /// <param name="useThread">Indicates if threading should be used</param>
        /// <returns>The ProfileSaveLoadStatus object holds operation status</returns>
        public ProfileSaveLoadStatus SaveProfile(string fileName, bool useThread)
        {
            if (ls == null)
                ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
            if (mainThread != null)
            {
                if (mainThread.IsAlive)
                {
                    mainThread.Abort();
                    mainThread = null;
                }
            }
            temp_profile_path = fileName;
            temp_file = fileName + "_tmp";

            Trace.WriteLine("Saving profile ..", "Profile Manager");
            // Raise the event
            OnProfileSavingStarted();
            OnProgress(ls["Status_SavingProfile"] + " ...", 1);
            IsSaving = true;
            fs = new FileStream(temp_file, FileMode.Create, FileAccess.Write);

            // First of all, save the main header
            List<byte> header = new List<byte>();
            // First 4 bytes are the ID
            header.AddRange(ASCIIEncoding.ASCII.GetBytes("EOPR"));
            // 4 bytes for the first chunck size, which it is the profile without roms
            // First of all, serialize the profile. Profile size without the roms will be small 
            // and include all the data we need to open profile basically.
            MemoryStream mStr = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            // Clear roms first
            romsCollection = new RomsCollection(null, false, profile.Roms.ToArray());
            profile.Roms = new RomsCollection(null, false);
            // Now serialize !
            formatter.Serialize(mStr, profile);
            // Compress it !
            byte[] pdata = new byte[0];
            HelperTools.CompressData(mStr.ToArray(), out pdata);
            mStr.Dispose();

            // Return the roms !
            profile.Roms = new RomsCollection(profile, true, romsCollection.ToArray());
            // Now add the size (size after compression)
            int csize = pdata.Length;
            header.Add((byte)((csize >> 24) & 0xFF));
            header.Add((byte)((csize >> 16) & 0xFF));
            header.Add((byte)((csize >> 08) & 0xFF));
            header.Add((byte)((csize >> 00) & 0xFF));
            // 4 reserved bytes
            header.AddRange(new byte[] { 0, 0, 0, 0 });
            // Write header
            fs.Write(header.ToArray(), 0, 12);
            // After the header, save the profile chunk
            fs.Write(pdata, 0, csize);

            mStr.Dispose();

            // Save roms threaded
            if (useThread)
            {
                mainThread = new Thread(new ThreadStart(SaveProfileThreaded));
                mainThread.CurrentUICulture = ls.CultureInfo;
                mainThread.Start();
            }
            else
            {
                SaveProfileThreaded();
            }
            // clear
            
            Trace.WriteLine("Profile saved successfully", "Profile Manager");
            return new ProfileSaveLoadStatus("Profile saved successfully", ProfileSaveLaodType.Success);

            /*temp_profile_path = fileName;
            string temp_file = fileName + "_tmp";
            try
            {
                Trace.WriteLine("Saving profile ..", "Profile Manager");

                fs = new FileStream(temp_file, FileMode.Create, FileAccess.Write);

                // First of all, save the main header
                List<byte> header = new List<byte>();
                // First 4 bytes are the ID
                header.AddRange(ASCIIEncoding.ASCII.GetBytes("EOPR"));
                // 4 bytes for the first chunck size, which it is the profile without roms
                // First of all, serialize the profile. Profile size without the roms will be small 
                // and include all the data we need to open profile basically.
                MemoryStream mStr = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                // Clear roms first
                RomsCollection romsCollection = new RomsCollection(null, profile.Roms.ToArray());
                profile.Roms = new RomsCollection(null);
                // Now serialize !
                formatter.Serialize(mStr, profile);
                // Compress it !
                byte[] pdata = new byte[0];
                HelperTools.CompressData(mStr.ToArray(), out pdata);
                mStr.Dispose();

                // Return the roms !
                profile.Roms = new RomsCollection(profile, romsCollection.ToArray());
                // Now add the size (size after compression)
                int csize = pdata.Length;
                header.Add((byte)((csize >> 24) & 0xFF));
                header.Add((byte)((csize >> 16) & 0xFF));
                header.Add((byte)((csize >> 08) & 0xFF));
                header.Add((byte)((csize >> 00) & 0xFF));
                // 4 reserved bytes
                header.AddRange(new byte[] { 0, 0, 0, 0 });
                // Write header
                fs.Write(header.ToArray(), 0, 12);
                // After the header, save the profile chunk
                fs.Write(pdata, 0, csize);

                mStr.Dispose();

                // Now the roms, for each console ...
                for (int i = 0; i < profile.Consoles.Count; i++)
                {
                    header = new List<byte>();
                    // Add the id, 4 bytes
                    header.AddRange(ASCIIEncoding.ASCII.GetBytes("CONS"));
                    // The size, we don't know how much yet, we'll add 4 0s for now
                    long size_point = fs.Position + 4;
                    long roms_size = 0;// All roms size (with headers) that related to this console (console roms size total)
                    header.AddRange(new byte[] { 0, 0, 0, 0 });
                    // 4 bytes are for the roms count, set 4 0s for now as well
                    long roms_count_point = fs.Position + 8;
                    long roms_count = 0;
                    header.AddRange(new byte[] { 0, 0, 0, 0 });

                    // Write the header
                    fs.Write(header.ToArray(), 0, 12);

                    // Write 8 bytes console id !
                    fs.Write(ASCIIEncoding.ASCII.GetBytes(profile.Consoles[i].ID), 0, 8);

                    // Now the roms of this console, get 'em first
                    Rom[] roms = romsCollection[profile.Consoles[i].ID, false];
                    for (int j = 0; j < roms.Length; j++)
                    {
                        if (profile.MarkedToBeDeleted.Contains(roms[j].ID))
                        {
                            // This rom is marked to be deleted/ignored ...
                            continue;
                        }
                        // Make this rom header
                        header = new List<byte>();
                        // Add the id, 4 bytes
                        header.AddRange(ASCIIEncoding.ASCII.GetBytes("ROMD"));
                        // The size, 4 bytes
                        mStr = new MemoryStream();
                        formatter = new BinaryFormatter();
                        formatter.Serialize(mStr, roms[j]);
                        byte[] rdata = new byte[0];
                        HelperTools.CompressData(mStr.ToArray(), out rdata);

                        if (rdata.Length == 0)
                            throw new Exception("COMPRESSED ROM SIZE IS 0 !!?");
                        // Now add the size
                        csize = rdata.Length;
                        csize += 8;// 8 bytes for the ID
                        header.Add((byte)((csize >> 24) & 0xFF));
                        header.Add((byte)((csize >> 16) & 0xFF));
                        header.Add((byte)((csize >> 08) & 0xFF));
                        header.Add((byte)((csize >> 00) & 0xFF));
                        // 4 bytes reserved, add 0s
                        header.AddRange(new byte[] { 0, 0, 0, 0 });

                        // Write header
                        fs.Write(header.ToArray(), 0, 12);
                        // Save the id
                        fs.Write(ASCIIEncoding.ASCII.GetBytes(roms[j].ID), 0, 8);
                        // After the header, save the rom chunk
                        fs.Write(rdata, 0, csize - 8);

                        roms_size += 12 + csize;
                        roms_count++;

                        mStr.Dispose();
                    }
                    // Now we are done with roms, save header remain info
                    long current_pos = fs.Position;
                    // Console chunk size
                    fs.Position = size_point;
                    csize = (int)roms_size;
                    csize += 8;// The console id :D
                    fs.WriteByte((byte)((csize >> 24) & 0xFF));
                    fs.WriteByte((byte)((csize >> 16) & 0xFF));
                    fs.WriteByte((byte)((csize >> 08) & 0xFF));
                    fs.WriteByte((byte)((csize >> 00) & 0xFF));

                    // Roms count
                    fs.Position = roms_count_point;
                    csize = (int)roms_count;
                    fs.WriteByte((byte)((csize >> 24) & 0xFF));
                    fs.WriteByte((byte)((csize >> 16) & 0xFF));
                    fs.WriteByte((byte)((csize >> 08) & 0xFF));
                    fs.WriteByte((byte)((csize >> 00) & 0xFF));

                    // Now return to where we left of...
                    fs.Position = current_pos;
                }
                fs.Flush();
                fs.Close();
                fs.Dispose();
                // Done !
                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch { }
                }
                File.Move(temp_file, fileName);
                filePath = fileName;
                // Make it available for cache ...
                this.IsCacheAvailable = true;
                filePathOfCache = fileName;
                // clear
                profile.MarkedToBeDeleted.Clear();
                Trace.WriteLine("Profile saved successfully", "Profile Manager");
                return new ProfileSaveLoadStatus("Profile saved successfully", ProfileSaveLaodType.Success);
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();

                    if (File.Exists(temp_file))
                    {
                        try
                        {
                            File.Delete(temp_file);
                        }
                        catch { }
                    }
                }
                Trace.TraceError("Unable to save profile: " + ex.Message);
                return new ProfileSaveLoadStatus("Unable to save profile: " + ex.Message, ProfileSaveLaodType.Error);
            }*/
        }
        private void SaveProfileThreaded()
        {
            if (ls == null)
                ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
            int csize = 0;
            OnProgress(ls["Status_SavingRoms"] + " ...", 2);
            try
            {
                // Open the cache stream... we gonna need it !
                OpenCacheStream();
                // Now the roms, for each console ...
                for (int i = 0; i < profile.Consoles.Count; i++)
                {
                    Console console = profile.Consoles[i];
                    List<byte> header = new List<byte>();
                    // Add the id, 4 bytes
                    header.AddRange(ASCIIEncoding.ASCII.GetBytes("CONS"));
                    // The size, we don't know how much yet, we'll add 4 0s for now
                    long size_point = fs.Position + 4;
                    long roms_size = 0;// All roms size (with headers) that related to this console (console roms size total)
                    header.AddRange(new byte[] { 0, 0, 0, 0 });
                    // 4 bytes are for the roms count, set 4 0s for now as well
                    long roms_count_point = fs.Position + 8;
                    long roms_count = 0;
                    header.AddRange(new byte[] { 0, 0, 0, 0 });

                    // Write the header
                    fs.Write(header.ToArray(), 0, 12);

                    // Write 8 bytes console id !
                    fs.Write(ASCIIEncoding.ASCII.GetBytes(console.ID), 0, 8);

                    #region Load roms for console if already cahced
                    CachedConsoleInfo consoleInfo = GetConsoleInfo(console.ID);
                    if (consoleInfo.ID != "")
                    {
                        // We have this console on the cache, we just copy and paste !
                        // Get the roms
                        for (int romIndex = 0; romIndex < consoleInfo.RomsCount; romIndex++)
                        {
                            int progress = (romIndex * 100) / consoleInfo.RomsCount;
                            OnProgress(string.Format("{0} [{1}] ... {2}%", ls["Status_SavingRomsCahced"], console.Name, progress), progress);

                            // Get the rom !
                            CachedRomData romInf = GetNextRom();
                            if (romInf.ID == "")
                                continue;// Curropted rom !?
                            // Since this rom is not on the collection, see if it is marked to delete !?
                            if (profile.MarkedToBeDeleted.Contains(romInf.ID))
                            {
                                // To be deleted .... skip !
                                continue;
                            }

                            // Look for it in the roms collection
                            if (romsCollection.Contains(romInf.ID))
                            {
                                // We already have this rom on the memory, check it's status
                                Rom romOnMem = romsCollection[romInf.ID];
                                if (!romOnMem.Modified)
                                {
                                    // This rom is not modified thus it is not need to be saved again
                                    // Remove it from the collection ...
                                    romsCollection.Remove(romInf.ID, false);
                                }
                                else
                                    continue;// This rom is not cached and modified so we need to save it in normal way ..
                            }

                            // We can copy-paste this rom directly !! :D
                            // Make this rom header
                            header = new List<byte>();
                            // Add the id, 4 bytes
                            header.AddRange(ASCIIEncoding.ASCII.GetBytes("ROMD"));

                            // Now add the size
                            csize = romInf.DataSizeCompressed;
                            csize += 8;// 8 bytes for the ID
                            header.Add((byte)((csize >> 24) & 0xFF));
                            header.Add((byte)((csize >> 16) & 0xFF));
                            header.Add((byte)((csize >> 08) & 0xFF));
                            header.Add((byte)((csize >> 00) & 0xFF));
                            // 4 bytes reserved, add 0s
                            header.AddRange(new byte[] { 0, 0, 0, 0 });

                            // Write header
                            fs.Write(header.ToArray(), 0, 12);
                            // Save the id
                            fs.Write(ASCIIEncoding.ASCII.GetBytes(romInf.ID), 0, 8);
                            // After the header, save the rom chunk
                            fs.Write(romInf.DataCompressed, 0, csize - 8);

                            roms_size += 12 + csize;
                            roms_count++;
                        }
                    }
                    #endregion
                    Rom[] roms = romsCollection.GetRomsForConsoleNotCahced(console.ID);
                    for (int j = 0; j < roms.Length; j++)
                    {
                        int progress = (j * 100) / roms.Length;
                        OnProgress(string.Format("{0} [{1}] ... {2}%", ls["Status_SavingRoms"], console.Name, progress), progress);
                        if (profile.MarkedToBeDeleted.Contains(roms[j].ID))
                        {
                            // This rom is marked to be deleted/ignored ...
                            continue;
                        }
                        // Make this rom header
                        header = new List<byte>();
                        // Add the id, 4 bytes
                        header.AddRange(ASCIIEncoding.ASCII.GetBytes("ROMD"));
                        // The size, 4 bytes
                        MemoryStream mStr = new MemoryStream();
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(mStr, roms[j]);
                        byte[] rdata = new byte[0];
                        HelperTools.CompressData(mStr.ToArray(), out rdata);

                        if (rdata.Length == 0)
                            throw new Exception("COMPRESSED ROM SIZE IS 0 !!?");
                        // Now add the size
                        csize = rdata.Length;
                        csize += 8;// 8 bytes for the ID
                        header.Add((byte)((csize >> 24) & 0xFF));
                        header.Add((byte)((csize >> 16) & 0xFF));
                        header.Add((byte)((csize >> 08) & 0xFF));
                        header.Add((byte)((csize >> 00) & 0xFF));
                        // 4 bytes reserved, add 0s
                        header.AddRange(new byte[] { 0, 0, 0, 0 });

                        // Write header
                        fs.Write(header.ToArray(), 0, 12);
                        // Save the id
                        fs.Write(ASCIIEncoding.ASCII.GetBytes(roms[j].ID), 0, 8);
                        // After the header, save the rom chunk
                        fs.Write(rdata, 0, csize - 8);

                        roms_size += 12 + csize;
                        roms_count++;
                        roms[j].Modified = false;// We saved it !
                        mStr.Dispose();
                    }
                    // Now we are done with roms, save header remain info
                    long current_pos = fs.Position;
                    // Console chunk size
                    fs.Position = size_point;
                    csize = (int)roms_size;
                    csize += 8;// The console id :D
                    fs.WriteByte((byte)((csize >> 24) & 0xFF));
                    fs.WriteByte((byte)((csize >> 16) & 0xFF));
                    fs.WriteByte((byte)((csize >> 08) & 0xFF));
                    fs.WriteByte((byte)((csize >> 00) & 0xFF));

                    // Roms count
                    fs.Position = roms_count_point;
                    csize = (int)roms_count;
                    fs.WriteByte((byte)((csize >> 24) & 0xFF));
                    fs.WriteByte((byte)((csize >> 16) & 0xFF));
                    fs.WriteByte((byte)((csize >> 08) & 0xFF));
                    fs.WriteByte((byte)((csize >> 00) & 0xFF));

                    // Now return to where we left of...
                    fs.Position = current_pos;
                }
                OnProgress(ls["Status_ProfileSavedSuccessfully"] + " !!", 100);
                CloseCacheStream();
                fs.Flush();
                fs.Close();
                fs.Dispose();
                // Done !
                if (File.Exists(temp_profile_path))
                {
                    try
                    {
                        File.Delete(temp_profile_path);
                    }
                    catch { }
                }
                File.Move(temp_file, temp_profile_path);
                filePath = temp_profile_path;
                // Make it available for cache ...
                this.IsCacheAvailable = true;
                filePathOfCache = temp_profile_path;
            }
            catch (Exception ex)
            {
                OnProgress(ls["Status_ErrorSavingProfile"] + ": " + ex.Message, 100);
            }

            IsSaving = false;
            OnProfileSavingFinished();
        }

        /// <summary>
        /// Load profile at FilePath location
        /// </summary>
        /// <returns>The ProfileSaveLoadStatus object holds operation status</returns>
        public ProfileSaveLoadStatus LoadProfile()
        { return LoadProfile(filePath); }
        /// <summary>
        /// Load profile at given location
        /// </summary>
        /// <param name="fileName">The profile complete location</param>
        /// <returns>The ProfileSaveLoadStatus object holds operation status</returns>
        public ProfileSaveLoadStatus LoadProfile(string fileName)
        {
            FileStream fs = null;
            this.IsCacheAvailable = false;
            filePathOfCache = "";
            // try
            {
                if (File.Exists(fileName))
                {
                    Trace.WriteLine("Loading profile ..", "Profile Manager");
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    // Check header, read 12 bytes
                    if (fs.Length < 12)
                    {
                        fs.Dispose();
                        fs.Close();
                        Trace.TraceError("Unable to load profile: profile file size is too small and can't read the header.");
                        return new ProfileSaveLoadStatus("Unable to load profile: profile file size is too small and can't read the header.", ProfileSaveLaodType.Error);
                    }
                    byte[] header = new byte[12];
                    fs.Read(header, 0, 12);
                    // Check it
                    if (ASCIIEncoding.ASCII.GetString(header, 0, 4) != "EOPR")
                    {
                        fs.Position = 0;
                        // This is old EO profile style, load it normally.
                        BinaryFormatter formatter = new BinaryFormatter();
                        object p = formatter.Deserialize(fs);
                        fs.Close();
                        filePath = fileName;

                        if (OpeningProfile != null)
                            OpeningProfile(this, new EventArgs());

                        // Dispose
                        if (profile != null)
                        {
                            profile.Dispose();
                            profile = null;
                        }

                        profile = (Profile)p;
                        profile.Roms.CahingEnabled = true;
                        Trace.WriteLine("Profile loaded successfully", "Profile Manager");
                        profile.OnProfileOpened();
                        if (ProfileOpened != null)
                            ProfileOpened(this, new EventArgs());

                        return new ProfileSaveLoadStatus("Profile loaded successfully", ProfileSaveLaodType.Success);
                    }
                    else
                    {
                        // NEW EO PROFILE !!
                        // get the profile size
                        int csize = header[4] << 24;
                        csize |= header[5] << 16;
                        csize |= header[6] << 8;
                        csize |= header[7];
                        // Now read it !

                        byte[] pData = new byte[csize];
                        fs.Read(pData, 0, csize);

                        fs.Dispose();
                        fs.Close();

                        // Load the profile only, when roms are needed, the collction should take care of the rest.
                        // Decompress !!
                        byte[] outPData = new byte[0];
                        HelperTools.DecompressData(pData, out outPData);
                        MemoryStream mStr = new MemoryStream(outPData);
                        BinaryFormatter formatter = new BinaryFormatter();
                        object p = formatter.Deserialize(mStr);
                        mStr.Dispose();

                        filePath = fileName;

                        if (OpeningProfile != null)
                            OpeningProfile(this, new EventArgs());

                        // Dispose
                        if (profile != null)
                        {
                            profile.Dispose();
                            profile = null;
                        }

                        profile = (Profile)p;

                        profile.Roms = new RomsCollection(profile, true, new Rom[0]);

                        this.IsCacheAvailable = true;
                        filePathOfCache = fileName;

                        Trace.WriteLine("Profile loaded successfully", "Profile Manager");
                        profile.OnProfileOpened();
                        if (ProfileOpened != null)
                            ProfileOpened(this, new EventArgs());

                        return new ProfileSaveLoadStatus("Profile loaded successfully", ProfileSaveLaodType.Success);
                    }
                }
                else
                {
                    Trace.TraceError("Unable to load profile: profile file is not exist.");
                    return new ProfileSaveLoadStatus("Unable to load profile: profile file is not exist.", ProfileSaveLaodType.Error);
                }
            }
            /*catch (Exception ex)
            {
                if (fs != null)
                    fs.Close();
                Trace.TraceError("Unable to load profile: " + ex.Message);
                return new ProfileSaveLoadStatus("Unable to load profile: " + ex.Message, ProfileSaveLaodType.Error);
            }*/
        }

        public void OpenCacheStream()
        {
            if (File.Exists(filePathOfCache) && this.IsCacheAvailable)
            {
                cache_stream = new FileStream(filePathOfCache, FileMode.Open, FileAccess.Read);
                cache_after_profile_point = 0;
                // Check header, read 12 bytes
                if (cache_stream.Length < 12)
                {
                    cache_stream.Dispose();
                    cache_stream.Close();
                    cache_stream = null;
                }
                byte[] header = new byte[12];
                cache_stream.Read(header, 0, 12);
                // Check it
                if (ASCIIEncoding.ASCII.GetString(header, 0, 4) == "EOPR")
                {
                    // NEW EO PROFILE !!
                    // get the profile size
                    int csize = header[4] << 24;
                    csize |= header[5] << 16;
                    csize |= header[6] << 8;
                    csize |= header[7];
                    // Now skip it !
                    cache_stream.Position = cache_after_profile_point = 12 + csize;
                }
            }
            else
            {
                cache_stream = null;
            }
        }
        public void CloseCacheStream()
        {
            if (cache_stream != null)
            {
                cache_stream.Flush();
                cache_stream.Dispose();
                cache_stream.Close();
                cache_stream = null;
            }
        }
        public CachedConsoleInfo GetConsoleInfo(string consoleID)
        {
            if (cache_stream == null)
                return CachedConsoleInfo.Empty;
            // Seek to the point after the profile
            cache_stream.Position = cache_after_profile_point;
            // Now read until we find the console ...
            long point = 0;
            long pointOnStream = 0;
            int csize = 0;
            byte[] header = new byte[0];
            while (point < cache_stream.Length)
            {
                pointOnStream = cache_stream.Position;
                // Read
                header = new byte[12];
                cache_stream.Read(header, 0, 12);
                string HEADER_ID = ASCIIEncoding.ASCII.GetString(header, 0, 4);
                csize = header[4] << 24;
                csize |= header[5] << 16;
                csize |= header[6] << 8;
                csize |= header[7];

                if (HEADER_ID != "CONS")
                {
                    // This is something we don't want, skip it !
                    cache_stream.Position = cache_stream.Position + csize;
                    point = cache_stream.Position;
                }
                else
                {
                    // This is a console
                    // Read the console ID
                    byte[] id = new byte[8];
                    cache_stream.Read(id, 0, 8);
                    string HEADER_CONS_ID = ASCIIEncoding.ASCII.GetString(id, 0, 8);

                    if (HEADER_CONS_ID != consoleID)
                    {
                        // Just skip this console !!
                        cache_stream.Position = cache_stream.Position + csize - 8;
                        point = cache_stream.Position;
                    }
                    else
                    {
                        // See the roms count
                        int romsCount = header[8] << 24;
                        romsCount |= header[9] << 16;
                        romsCount |= header[10] << 8;
                        romsCount |= header[11];

                        CachedConsoleInfo retConInf = new CachedConsoleInfo();
                        retConInf.ID = consoleID;
                        retConInf.RomsCount = romsCount;
                        retConInf.RomsTotalSize = csize;
                        retConInf.PointOnTheStream = pointOnStream;

                        return retConInf;
                    }
                }
            }
            return CachedConsoleInfo.Empty;
        }
        public CachedRomData GetNextRom()
        {
            if (cache_stream == null)
                return new CachedRomData();

            // Read
            byte[] header = new byte[12];
            cache_stream.Read(header, 0, 12);

            int csize = header[4] << 24;
            csize |= header[5] << 16;
            csize |= header[6] << 8;
            csize |= header[7];

            if (ASCIIEncoding.ASCII.GetString(header, 0, 4) == "CONS")
            {
                return new CachedRomData();
            }
            else if (ASCIIEncoding.ASCII.GetString(header, 0, 4) != "ROMD")
            {
                // This is something we don't want, skip it !
                return new CachedRomData();
            }
            else
            {
                CachedRomData retRom = new CachedRomData();
                // This is a rom !
                // Read the ID
                byte[] id = new byte[8];
                cache_stream.Read(id, 0, 8);

                retRom.ID = ASCIIEncoding.ASCII.GetString(id);

                // This is it !!
                retRom.DataSizeCompressed = csize - 8;
                retRom.DataCompressed = new byte[retRom.DataSizeCompressed];
                cache_stream.Read(retRom.DataCompressed, 0, retRom.DataSizeCompressed);

                return retRom;
            }
        }
        public Rom LoadRomFromCachedFile(string romID)
        {
            if (profile.MarkedToBeDeleted.Contains(romID))
                return null;
            if (File.Exists(filePathOfCache) && this.IsCacheAvailable)
            {
                FileStream fs = new FileStream(filePathOfCache, FileMode.Open, FileAccess.Read);
                // Check header, read 12 bytes
                if (fs.Length < 12)
                {
                    fs.Dispose();
                    fs.Close();
                    return null;
                }
                byte[] header = new byte[12];
                fs.Read(header, 0, 12);
                // Check it
                if (ASCIIEncoding.ASCII.GetString(header, 0, 4) == "EOPR")
                {
                    // NEW EO PROFILE !!
                    // get the profile size
                    int csize = header[4] << 24;
                    csize |= header[5] << 16;
                    csize |= header[6] << 8;
                    csize |= header[7];
                    // Now skip it !
                    fs.Position = 12 + csize;
                    // Loop through consoles ...
                    long point = 0;
                    while (point < fs.Length)
                    {
                        // Read
                        header = new byte[12];
                        fs.Read(header, 0, 12);

                        csize = header[4] << 24;
                        csize |= header[5] << 16;
                        csize |= header[6] << 8;
                        csize |= header[7];

                        if (ASCIIEncoding.ASCII.GetString(header, 0, 4) == "CONS")
                        {
                            fs.Position = fs.Position + 8;

                            point = fs.Position;
                        }
                        else if (ASCIIEncoding.ASCII.GetString(header, 0, 4) != "ROMD")
                        {
                            // This is something we don't want, skip it !
                            fs.Position = fs.Position + csize;
                            point = fs.Position;
                        }
                        else
                        {
                            // This is a rom !
                            // Read the ID
                            byte[] id = new byte[8];
                            fs.Read(id, 0, 8);

                            if (ASCIIEncoding.ASCII.GetString(id, 0, 8) != romID)
                            {
                                // This is not the rom we need, skip it
                                fs.Position = fs.Position + csize - 8;
                                point = fs.Position;
                            }
                            else
                            {
                                // This is it !!
                                byte[] pData = new byte[csize - 8];
                                fs.Read(pData, 0, csize - 8);

                                fs.Dispose();
                                fs.Close();

                                // Decompress
                                byte[] outPData = new byte[0];
                                HelperTools.DecompressData(pData, out outPData);

                                MemoryStream mStr = new MemoryStream(outPData);
                                BinaryFormatter formatter = new BinaryFormatter();
                                object p = formatter.Deserialize(mStr);
                                mStr.Dispose();

                                return (Rom)p;
                            }
                        }
                    }
                    fs.Dispose();
                    fs.Close();
                }
                else
                {
                    fs.Dispose();
                    fs.Close();
                }
            }
            // Return nothing if not found.
            return null;
        }
        public Rom[] LoadRomsFromCachedFile(string[] ids)
        {
            List<string> romIDs = new List<string>(ids);
            foreach (string id in ids)
                if (profile.MarkedToBeDeleted.Contains(id))
                    romIDs.Remove(id);

            List<Rom> roms = new List<Rom>();
            // Add the roms that already cached ...
            for (int i = 0; i < romIDs.Count; i++)
            {
                if (profile.Roms.Contains(romIDs[i]))
                {
                    // This rom already cached, add it.
                    roms.Add(profile.Roms[romIDs[i]]);
                    romIDs.RemoveAt(i);
                    i--;
                }
            }
            if (romIDs.Count == 0)
            {
                // no rom remain to load from cache !
                return roms.ToArray();
            }
            if (File.Exists(filePathOfCache) && this.IsCacheAvailable)
            {
                FileStream fs = new FileStream(filePathOfCache, FileMode.Open, FileAccess.Read);
                // Check header, read 12 bytes
                if (fs.Length < 12)
                {
                    fs.Dispose();
                    fs.Close();
                    return null;
                }
                byte[] header = new byte[12];
                fs.Read(header, 0, 12);
                // Check it
                if (ASCIIEncoding.ASCII.GetString(header, 0, 4) == "EOPR")
                {
                    // NEW EO PROFILE !!
                    // get the profile size
                    int csize = header[4] << 24;
                    csize |= header[5] << 16;
                    csize |= header[6] << 8;
                    csize |= header[7];
                    // Now skip it !
                    fs.Position = fs.Position + csize;
                    // Loop through consoles ...
                    long point = 0;
                    while (point < fs.Length)
                    {
                        // Read
                        header = new byte[12];
                        fs.Read(header, 0, 12);
                        if (ASCIIEncoding.ASCII.GetString(header, 0, 4) == "CONS")
                        {
                            fs.Position = fs.Position + 8;
                            point = fs.Position;
                        }
                        else if (ASCIIEncoding.ASCII.GetString(header, 0, 4) != "ROMD")
                        {
                            csize = header[4] << 24;
                            csize |= header[5] << 16;
                            csize |= header[6] << 8;
                            csize |= header[7];
                            // This is something we don't want, skip it !
                            fs.Position = fs.Position + csize;
                            point = fs.Position;
                        }
                        else
                        {
                            // This is a rom !
                            csize = header[4] << 24;
                            csize |= header[5] << 16;
                            csize |= header[6] << 8;
                            csize |= header[7];
                            // Read the ID
                            byte[] id = new byte[8];
                            fs.Read(id, 0, 8);
                            string romid = ASCIIEncoding.ASCII.GetString(id, 0, 8);
                            if (!romIDs.Contains(romid))
                            {
                                // This is not the rom we need, skip it
                                fs.Position = fs.Position + csize - 8;
                                point = fs.Position;
                            }
                            else
                            {
                                // This is it !!
                                byte[] pData = new byte[csize - 8];
                                fs.Read(pData, 0, csize - 8);

                                // Decompress
                                byte[] outPData = new byte[0];
                                HelperTools.DecompressData(pData, out outPData);

                                MemoryStream mStr = new MemoryStream(outPData);
                                BinaryFormatter formatter = new BinaryFormatter();
                                object p = formatter.Deserialize(mStr);
                                mStr.Dispose();

                                roms.Add((Rom)p);
                                romIDs.Remove(romid);

                                // Add it to the collection as well
                                profile.Roms.Add((Rom)p, false);
                            }
                            if (romIDs.Count == 0)
                            {
                                fs.Dispose();
                                fs.Close();
                                return roms.ToArray();
                            }
                        }
                    }
                    fs.Dispose();
                    fs.Close();
                }
                else
                {
                    fs.Dispose();
                    fs.Close();
                }
            }
            // Return nothing if not found.
            return roms.ToArray();
        }
        public Rom[] LoadRomsFromCachedFileForConsole(string consoleID)
        {
            List<Rom> roms = new List<Rom>();
            if (File.Exists(filePathOfCache) && this.IsCacheAvailable)
            {
                FileStream fs = new FileStream(filePathOfCache, FileMode.Open, FileAccess.Read);
                // Check header, read 12 bytes
                if (fs.Length < 12)
                {
                    fs.Dispose();
                    fs.Close();
                    return null;
                }
                byte[] header = new byte[12];
                fs.Read(header, 0, 12);
                // Check it
                if (ASCIIEncoding.ASCII.GetString(header, 0, 4) == "EOPR")
                {
                    // NEW EO PROFILE !!
                    // get the profile size
                    int csize = header[4] << 24;
                    csize |= header[5] << 16;
                    csize |= header[6] << 8;
                    csize |= header[7];
                    // Now skip it !
                    fs.Position = fs.Position + csize;
                    // Loop through consoles ...
                    long point = 0;
                    bool done = false;
                    while (point < fs.Length)
                    {
                        // Read
                        header = new byte[12];
                        fs.Read(header, 0, 12);
                        string HEADER_ID = ASCIIEncoding.ASCII.GetString(header, 0, 4);
                        csize = header[4] << 24;
                        csize |= header[5] << 16;
                        csize |= header[6] << 8;
                        csize |= header[7];

                        if (HEADER_ID != "CONS")
                        {
                            // This is something we don't want, skip it !
                            fs.Position = fs.Position + csize;
                            point = fs.Position;
                        }
                        else
                        {
                            // This is a console
                            // Read the console ID
                            byte[] id = new byte[8];
                            fs.Read(id, 0, 8);
                            string HEADER_CONS_ID = ASCIIEncoding.ASCII.GetString(id, 0, 8);

                            if (HEADER_CONS_ID != consoleID)
                            {
                                // Skip this console !!
                                fs.Position = fs.Position + csize - 8;
                                point = fs.Position;
                            }
                            else
                            {
                                // See the roms count
                                int romsCount = header[8] << 24;
                                romsCount |= header[9] << 16;
                                romsCount |= header[10] << 8;
                                romsCount |= header[11];
                                // Load them all
                                for (int i = 0; i < romsCount; i++)
                                {
                                    // Read
                                    header = new byte[12];
                                    fs.Read(header, 0, 12);
                                    HEADER_ID = ASCIIEncoding.ASCII.GetString(header, 0, 4);
                                    csize = header[4] << 24;
                                    csize |= header[5] << 16;
                                    csize |= header[6] << 8;
                                    csize |= header[7];
                                    if (HEADER_ID != "ROMD")
                                    {
                                        // This is something we don't want, skip it !
                                        fs.Position = fs.Position + csize;
                                        point = fs.Position;
                                    }
                                    else
                                    {

                                        // This is it !!

                                        // Read the rom id ID
                                        id = new byte[8];
                                        fs.Read(id, 0, 8);
                                        string ROM_ID = ASCIIEncoding.ASCII.GetString(id, 0, 8);
                                        if (profile.MarkedToBeDeleted.Contains(ROM_ID))
                                        {
                                            // This rom is forbidden, skip it !
                                            fs.Position = fs.Position + csize - 8;
                                            point = fs.Position;
                                        }
                                        else
                                        {
                                            // Read data chunck
                                            byte[] pData = new byte[csize - 8];
                                            fs.Read(pData, 0, csize - 8);

                                            // Decompress
                                            byte[] outPData = new byte[0];
                                            HelperTools.DecompressData(pData, out outPData);

                                            MemoryStream mStr = new MemoryStream(outPData);
                                            BinaryFormatter formatter = new BinaryFormatter();
                                            object p = formatter.Deserialize(mStr);

                                            point = fs.Position;
                                            roms.Add((Rom)p);
                                        }
                                    }
                                }
                                // Done !! We only need this console.
                                done = true;
                                break;
                            }
                        }
                        if (done)
                            break;
                    }
                    fs.Dispose();
                    fs.Close();
                }
                else
                {
                    fs.Dispose();
                    fs.Close();
                }
            }

            // Return nothing if not found.
            return roms.ToArray();
        }
        public Rom GetRomFromData(byte [] dataCompressed)
        {
            // Decompress
            byte[] outPData = new byte[0];
            HelperTools.DecompressData(dataCompressed, out outPData);

            MemoryStream mStr = new MemoryStream(outPData);
            BinaryFormatter formatter = new BinaryFormatter();
            object p = formatter.Deserialize(mStr);

            return (Rom)p;
        }
        private void OnNewProfile()
        {
            if (NewProfileCreated != null)
                NewProfileCreated(this, new EventArgs());
        }
        public void OnDisableStyleSave()
        {
            if (DisableStyleSave != null)
                DisableStyleSave(null, null);
        }
        public void OnEnableStyleSave()
        {
            if (EnableStyleSave != null)
                EnableStyleSave(null, null);
        }
        private void OnProfileSavingStarted()
        {
            if (ProfileSavingStarted != null)
                ProfileSavingStarted(this, new EventArgs());
        }
        private void OnProfileSavingFinished()
        {
            if (ProfileSavingFinished != null)
                ProfileSavingFinished(this, new EventArgs());
            profile.MarkedToBeDeleted.Clear();
        }
        public void OnCacheLoadStarted()
        {
            if (CacheLoadStarted != null)
                CacheLoadStarted(this, new EventArgs());
        }
        public void OnCacheLoadFinished()
        {
            if (CacheLoadFinished != null)
                CacheLoadFinished(this, new EventArgs());
        }
        private void OnProgress(string status, int precentage)
        {
            if (Progress != null)
                Progress(this, new ProgressArgs(precentage, status));
        }
        /// <summary>
        /// Close the profiles manager service
        /// </summary>
        public void Close()
        {
            Trace.WriteLine("Clearing the temp folder ...", "Profile");
            SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");

            string temp = HelperTools.GetFullPath((string)settings.GetValue(DefaultProfileSettings.TempFolder_Key,
               true, DefaultProfileSettings.TempFolder));
            bool clear = (bool)settings.GetValue(DefaultProfileSettings.ClearTempFolder_Key,
               true, DefaultProfileSettings.ClearTempFolder);
            string[] exs = (string[])settings.GetValue(DefaultProfileSettings.TempFolderExclude_Key,
               true, DefaultProfileSettings.TempFolderExclude);

            if (!Directory.Exists(temp))
                return;

            for (int i = 0; i < Directory.GetFiles(temp, "*", SearchOption.AllDirectories).Length; i++)
            {
                if (!exs.Contains
                    (Path.GetExtension(Directory.GetFiles(temp, "*", SearchOption.AllDirectories)[i]).ToLower()))
                {
                    try
                    {
                        File.Delete(Directory.GetFiles(temp, "*", SearchOption.AllDirectories)[i]);
                        i = -1;
                    }
                    catch { }
                }
                else
                {
                    // Ignore the file at user request
                }
            }
            Trace.WriteLine("Temp folder cleared successfully.", "Profile");
        }
    }
}
