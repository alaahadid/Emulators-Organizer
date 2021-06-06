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

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Roms collection
    /// </summary>
    [Serializable()]
    public class RomsCollection : IList<Rom>
    {
        /// <summary>
        /// The roms collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        /// <param name="enable_caching">Indicates if this collection should use caching when loading roms.</param>
        public RomsCollection(Profile owner, bool enable_caching)
            : base()
        {
            this.owner = owner;
            items = new List<Rom>();
            profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
            this.enable_caching = enable_caching;
        }
        /// <summary>
        /// The roms collection
        /// </summary>
        /// <param name="owner">The owner profile</param>
        /// <param name="enable_caching">Indicates if this collection should use caching when loading roms.</param>
        /// <param name="roms">A roms array to add</param>
        public RomsCollection(Profile owner, bool enable_caching, Rom[] roms)
            : base()
        {
            this.owner = owner;
            items = new List<Rom>(roms);
            profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
            this.enable_caching = enable_caching;
        }

        [NonSerialized]
        private Profile owner;
        [NonSerialized]
        private ProfileManager profileManager;
        [NonSerialized]
        bool suspendEvents;
        [NonSerialized]
        bool enable_caching;

        public bool CahingEnabled
        {
            get { return enable_caching; }
            set { enable_caching = value; }
        }

        private List<Rom> items;
        /// <summary>
        /// Get or set rom using id.
        /// </summary>
        /// <param name="id">Desired rom id.</param>
        /// <returns>Rom for given id if found otherwise null.</returns>
        public Rom this[string id]
        {
            get
            {
                Rom roms = items.Find(
                      delegate (Rom rom)
                      {
                          return rom.ID == id;
                      }
                  );
                if (roms == null && enable_caching)
                {
                    if (profileManager == null)
                        profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
                    if (profileManager.IsCacheAvailable)
                    {
                        // The roms may not loaded yet, load them from the profile manager
                        roms = profileManager.LoadRomFromCachedFile(id);
                        suspendEvents = true;
                        if (roms != null)
                            this.Add(roms);
                        suspendEvents = false;
                    }
                }
                return roms;
            }
            set
            {
                Rom roms = items.Find(
                      delegate (Rom rom)
                      {
                          return rom.ID == id;
                      }
                  );
                if (roms == null && enable_caching)
                {
                    if (profileManager == null)
                        profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
                    if (profileManager.IsCacheAvailable)
                    {
                        // The roms may not loaded yet, load them from the profile manager
                        roms = profileManager.LoadRomFromCachedFile(id);
                        suspendEvents = true;
                        if (roms != null)
                            this.Add(roms);
                        suspendEvents = false;
                    }
                }
                if (roms != null)
                {
                    int index = items.IndexOf(roms);
                    if (index < items.Count)
                        this[index] = value;
                }
            }
        }
        /// <summary>
        /// Get or set rom using ids.
        /// </summary>
        /// <param name="ids">Desired rom ids.</param>
        /// <returns>Rom for given id if found otherwise null.</returns>
        public Rom[] this[string[] ids]
        {
            get
            {
                List<Rom> roms = null;

                if (profileManager == null)
                    profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
                // The roms may not loaded yet, load them from the profile manager
                if (profileManager.IsCacheAvailable && enable_caching)
                {
                    roms = new List<Rom>(profileManager.LoadRomsFromCachedFile(ids));
                }
                else
                {
                    roms = items.FindAll(
                  delegate (Rom rom)
                  {
                      return ids.Contains(rom.ID);
                  });
                }

                return roms.ToArray();
            }
        }
        /// <summary>
        /// Get roms that belong to given console.
        /// </summary>
        /// <param name="consoleID">Desired console id.</param>
        /// <param name="sort">Indicate whether to sort results by name.</param>
        /// <returns>Roms that belong to given console.</returns>
        public Rom[] this[string consoleID, bool sort]
        {
            get
            {
                return this[consoleID, sort, true, false];
            }
        }
        /// <summary>
        /// Get roms that belong to given console.
        /// </summary>
        /// <param name="consoleID">Desired console id.</param>
        /// <param name="sort">Indicate whether to sort results by name.</param>
        /// <param name="AtoZ">Sort mode : A to Z or Z to A</param>
        /// <returns>Roms that belong to given console.</returns>
        public Rom[] this[string consoleID, bool sort, bool AtoZ]
        {
            get
            {
                return this[consoleID, sort, AtoZ, false];
            }
        }
        /// <summary>
        /// Get roms that belong to given console.
        /// </summary>
        /// <param name="consoleID">Desired console id.</param>
        /// <param name="sort">Indicate whether to sort results by name.</param>
        /// <param name="AtoZ">Sort mode : A to Z or Z to A</param>
        /// <param name="parentsOnly">Indicates if should return parent roms only.</param>
        /// <returns>Roms that belong to given console.</returns>
        public Rom[] this[string consoleID, bool sort, bool AtoZ, bool parentsOnly]
        {
            get
            {
                List<Rom> roms = items.FindAll(
                      delegate (Rom rom)
                      {
                          if (parentsOnly)
                              return rom.ParentConsoleID == consoleID && rom.IsParentRom;
                          else
                              return rom.ParentConsoleID == consoleID;
                      }
                  );
                if (roms.Count == 0 && enable_caching)
                {
                    // No rom loaded maybe means that the roms aren't loaded from cache yet ...
                    if (profileManager == null)
                        profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
                    if (profileManager.IsCacheAvailable && enable_caching)
                    {
                        roms = new List<Rom>(profileManager.LoadRomsFromCachedFileForConsole(consoleID));
                        if (parentsOnly)
                        {
                            for (int i = 0; i < roms.Count; i++)
                            {
                                if (!roms[i].IsParentRom)
                                {
                                    roms.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                        suspendEvents = true;
                        // Add them so that the cache load will not be required anymore.
                        if (roms != null)
                            this.AddRange(roms);
                        suspendEvents = false;
                    }
                }
                // Sort 'em
                if (sort)
                    roms.Sort(new RomComparer(AtoZ, RomCompareType.Name));
                else// By default, order the roms as they are ordered for the console ...
                    roms.Sort(new RomComparer(true, RomCompareType.Index));

                return roms.ToArray();
            }
        }
        /// <summary>
        /// Get roms that related to console (on the memory pool, don't use cache !!)
        /// </summary>
        /// <param name="consoleID">Desired console id.</param>
        /// <returns>Roms that belong to given console.</returns>
        public Rom[] GetRomsForConsoleNotCahced(string consoleID)
        {
            return items.FindAll(
                    delegate (Rom rom)
                    {
                        return rom.ParentConsoleID == consoleID;
                    }
                ).ToArray();
        }
        /// <summary>
        /// Get roms that belong to given consoles group
        /// </summary>
        /// <param name="groupID">The consoles group id</param>
        /// <returns>Roms that belong to given consoles group</returns>
        public Rom[] GetRomsByConsolesGroup(string groupID)
        {
            List<Rom> roms = new List<Rom>();

            EmulatorsOrganizer.Core.Console[] consoles = profileManager.Profile.Consoles[groupID, false];
            foreach (Console con in consoles)
            {
                roms.AddRange(this[con.ID, false]);// This will load cached if nesseccary.
            }

            return roms.ToArray();
        }
        /// <summary>
        /// Get all roms that marked as child
        /// </summary>
        /// <param name="consoleID">The parent console id</param>
        /// <returns></returns>
        public Rom[] GetChildrenRoms(string consoleID)
        {
            List<Rom> roms = new List<Rom>(this[consoleID, false]);

            for (int i = 0; i < roms.Count; i++)
            {
                if (!roms[i].IsChildRom)
                {
                    roms.RemoveAt(i);
                    i--;
                }
            }

            return roms.ToArray();
        }
        /// <summary>
        /// Get all roms that marked as parent
        /// </summary>
        /// <param name="consoleID">The parent console id</param>
        /// <returns></returns>
        public Rom[] GetParentRoms(string consoleID)
        {
            List<Rom> roms = new List<Rom>(this[consoleID, false]);

            for (int i = 0; i < roms.Count; i++)
            {
                if (!roms[i].IsParentRom)
                {
                    roms.RemoveAt(i);
                    i--;
                }
            }

            return roms.ToArray();
        }
        public Rom[] GetSingleRoms(string consoleID)
        {
            List<Rom> roms = new List<Rom>(this[consoleID, false]);

            for (int i = 0; i < roms.Count; i++)
            {
                if (!roms[i].IsSingle)
                {
                    roms.RemoveAt(i);
                    i--;
                }
            }

            return roms.ToArray();
        }
        /// <summary>
        /// Get the index of item
        /// </summary>
        /// <param name="item">The rom to get index for</param>
        /// <returns>The index of the item if found otherwise -1</returns>
        public int IndexOf(Rom item)
        {
            return items.IndexOf(item);
        }
        /// <summary>
        /// Get index of rom
        /// </summary>
        /// <param name="itemID">The rom id to get index for</param>
        /// <returns>The given rom index if found otherwise null</returns>
        public int IndexOf(string itemID)
        {
            return items.IndexOf(this[itemID]);
        }
        /// <summary>
        /// Insert rom at index
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="item">The rom to insert</param>
        public void Insert(int index, Rom item)
        {
            items.Insert(index, item);
            if (owner != null && !suspendEvents)
                owner.OnRomAdd();
        }
        /// <summary>
        /// Remove rom at location
        /// </summary>
        /// <param name="index">The rom index to remove</param>
        public void RemoveAt(int index)
        {
            string id = items[index].ID;
            items.RemoveAt(index);
            if (owner != null && !suspendEvents)
                owner.OnRomRemoved(id);
            // In case events suspended
            if (owner != null)
                if (!owner.MarkedToBeDeleted.Contains(id))
                    owner.MarkedToBeDeleted.Add(id);
        }
        /// <summary>
        /// The roms collection
        /// </summary>
        /// <param name="index">The rom index</param>
        /// <returns>The rom if found otherwise null</returns>
        public Rom this[int index]
        {
            get
            {
                if (index < items.Count)
                    return items[index];
                return null;
            }
            set
            {
                if (index < items.Count)
                    items[index] = value;
            }
        }
        /// <summary>
        /// Add rom to the collection
        /// </summary>
        /// <param name="item">The rom to add</param>
        public void Add(Rom item)
        {
            items.Add(item);
            if (owner != null && !suspendEvents)
                owner.OnRomAdd();
        }
        /// <summary>
        /// Add rom to the collection
        /// </summary>
        /// <param name="roms">The roms to add</param>
        public void AddRange(IEnumerable<Rom> roms)
        {
            items.AddRange(roms);
        }
        /// <summary>
        /// Add rom to the collection
        /// </summary>
        /// <param name="item">The rom to add</param>
        /// <param name="raiseEvent">Indecate whether to raise the RomAdded event</param>
        public void Add(Rom item, bool raiseEvent)
        {
            items.Add(item);
            if (owner != null && raiseEvent)
                owner.OnRomAdd();
        }
        /// <summary>
        /// Clear this collection
        /// </summary>
        public void Clear()
        {
            items.Clear();
            if (owner != null && !suspendEvents)
                owner.OnRomsClear();
        }
        /// <summary>
        /// Get if rom exist in this collection
        /// </summary>
        /// <param name="item">The rom to check</param>
        /// <returns>True if rom found otherwise false</returns>
        public bool Contains(Rom item)
        {
            return items.Contains(item);
        }
        /// <summary>
        /// Get if rom exist in this collection (ONLY ROMS ON MEMORY COLLECTION, ROMS NOT LOADED FROM CACHE ARE NOT INCLUDED !)
        /// </summary>
        /// <param name="romID">The rom id to check</param>
        /// <returns>True if rom found otherwise false</returns>
        public bool Contains(string romID)
        {
            Rom rom = items.Find(
                       delegate (Rom r)
                       {
                           return r.ID == romID;
                       }
                   );
            return rom != null;
        }
        /// <summary>
        /// Check if this collection include rom using name
        /// </summary>
        /// <param name="name">The name of rom. Not case sensitive</param>
        /// <param name="romID">The rom id if found otherwise null</param>
        /// <param name="parentConsoleID">The parent console id for this rom</param>
        /// <returns>True if given rom name exists otherwise false</returns>
        public bool Contains(string name, out string romID, string parentConsoleID)
        {
            foreach (Rom rom in items)
            {
                if (rom.Name.ToLower() == name.ToLower() && rom.ParentConsoleID == parentConsoleID)
                {
                    romID = rom.ID;
                    return true;
                }
            }
            romID = null;
            return false;
        }
        /// <summary>
        /// Check if this collection include rom using name
        /// </summary>
        /// <param name="name">The name of rom. Not case sensitive</param>
        /// <param name="romID">The rom id if found otherwise null</param>
        /// <param name="romIDToIgnore">The rom to ignore during check</param>
        /// <param name="parentConsoleID">The parent console id for this rom</param>
        /// <returns>True if given rom name exists otherwise false</returns>
        public bool Contains(string name, out string romID, string romIDToIgnore, string parentConsoleID)
        {
            foreach (Rom rom in items)
            {
                if (rom.Name.ToLower() == name.ToLower() && rom.ID != romIDToIgnore &&
                    rom.ParentConsoleID == parentConsoleID)
                {
                    romID = rom.ID;
                    return true;
                }
            }
            romID = null;
            return false;
        }
        /// <summary>
        /// Check if this collection include rom using path. (AI path accepted)
        /// </summary>
        /// <param name="romPath">The file path of rom. Not case sensitive</param>
        /// <param name="name">The rom id if found otherwise null</param>
        /// <returns>True if given rom name exists otherwise false</returns>
        public bool ContainsByPath(string romPath, out string romID)
        {
            foreach (Rom rom in items)
            {
                if (HelperTools.GetFullPath(rom.Path).ToLower() == HelperTools.GetFullPath(romPath).ToLower())
                {
                    romID = rom.ID;
                    return true;
                }
            }
            romID = null;
            return false;
        }
        /// <summary>
        /// Check if this collection include rom using path
        /// </summary>
        /// <param name="romPath">The file path of rom. Not case sensitive</param>
        /// <param name="romID">The rom id if found otherwise null</param>
        /// <param name="parentConsoleID">The parent console id for this rom</param>
        /// <returns>True if given rom name exists otherwise false</returns>
        public bool ContainsByPath(string romPath, out string romID, string parentConsoleID)
        {
            foreach (Rom rom in items)
            {
                if (HelperTools.GetFullPath(rom.Path).ToLower() == HelperTools.GetFullPath(romPath).ToLower() && rom.ParentConsoleID == parentConsoleID)
                {
                    romID = rom.ID;
                    return true;
                }
            }
            romID = null;
            return false;
        }
        /// <summary>
        /// Check if this collection include rom using path
        /// </summary>
        /// <param name="romPath">The file path of rom. Not case sensitive</param>
        /// <param name="romID">The rom id if found otherwise null</param>
        /// <param name="romIDToIgnore">The rom id that get ignored during search</param>
        /// <param name="parentConsoleID">The parent console id for this rom</param>
        /// <returns>True if given rom name exists otherwise false</returns>
        public bool ContainsByPath(string romPath, out string romID, string romIDToIgnore, string parentConsoleID)
        {
            foreach (Rom rom in items)
            {
                if (HelperTools.GetFullPath(rom.Path).ToLower() == HelperTools.GetFullPath(romPath).ToLower()
                    && rom.ParentConsoleID == parentConsoleID && rom.ID != romIDToIgnore)
                {
                    romID = rom.ID;
                    return true;
                }
            }
            romID = null;
            return false;
        }
        /// <summary>
        /// Copy this collection to array
        /// </summary>
        /// <param name="array">The array to copy into</param>
        /// <param name="arrayIndex">The start index in the array to copy to</param>
        public void CopyTo(Rom[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Get the roms count
        /// </summary>
        public int Count
        {
            get
            {
                return items.Count;
            }
        }
        /// <summary>
        /// Get the total roms count in the profile. (CALCULATED)
        /// </summary>
        public int TotalRomsCount
        {
            get
            {
                if (profileManager == null)
                    profileManager = (ProfileManager)Services.ServicesManager.GetService("Profile Manager");
                int roms_count = 0;
                profileManager.OpenCacheStream();
                foreach (Core.Console con in profileManager.Profile.Consoles)
                {
                    ProfileManager.CachedConsoleInfo inf = profileManager.GetConsoleInfo(con.ID);
                    if (inf.ID != "")
                    {
                        for (int i = 0; i < inf.RomsCount; i++)
                        {
                            // Get the rom !
                            ProfileManager.CachedRomData romInf = profileManager.GetNextRom();
                            if (romInf.ID == "")
                                continue;// Curropted rom !?
                                         // Since this rom is not on the collection, see if it is marked to delete !?
                            if (profileManager.Profile.MarkedToBeDeleted.Contains(romInf.ID))
                            {
                                // To be deleted .... skip !
                                continue;
                            }

                            roms_count++;
                        }
                        // Now add the ones that not in cache
                        roms_count += profileManager.Profile.Roms.GetRomsForConsoleNotCahced(con.ID).Length;
                    }
                    else
                    {
                        // Get the count normally ... this console maybe already exist
                        roms_count += profileManager.Profile.Roms[con.ID, false].Length;
                    }
                }
                profileManager.CloseCacheStream();

                return roms_count;
            }
        }
        /// <summary>
        /// Is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Remove rom from the collection
        /// </summary>
        /// <param name="item">The rom to remove</param>
        /// <returns>True if rom removed successfully otherwise false</returns>
        public bool Remove(Rom item)
        {
            bool val = items.Remove(item);
            if (owner != null && !suspendEvents)
                owner.OnRomRemoved(item.ID);
            // In case events suspended
            if (owner != null)
                if (!owner.MarkedToBeDeleted.Contains(item.ID))
                    owner.MarkedToBeDeleted.Add(item.ID);
            return val;
        }
        /// <summary>
        /// Remove rom from the collection
        /// </summary>
        /// <param name="roms">The roms to remove</param>
        /// <returns>True if rom removed successfully otherwise false</returns>
        public void Remove(Rom[] roms, bool markToDelete)
        {
            for (int i = 0; i < roms.Length; i++)
            {
                items.Remove(roms[i]);

                if (owner != null && markToDelete)
                    if (!owner.MarkedToBeDeleted.Contains(roms[i].ID))
                        owner.MarkedToBeDeleted.Add(roms[i].ID);
            }
        }

        /// <summary>
        /// Remove rom from collection
        /// </summary>
        /// <param name="item">The rom to remove</param>
        /// <param name="raiseEvent">Indecate whether to raise the RomRemoved event or not</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(Rom item, bool raiseEvent)
        {
            bool val = items.Remove(item);
            if (owner != null && raiseEvent)
                owner.OnRomRemoved(item.ID);
            // In case events suspended
            if (owner != null)
                if (!owner.MarkedToBeDeleted.Contains(item.ID))
                    owner.MarkedToBeDeleted.Add(item.ID);
            return val;
        }
        /// <summary>
        /// Remove rom from the collection
        /// </summary>
        /// <param name="itemID">The rom id to remove</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(string itemID)
        {
            bool val = items.Remove(this[itemID]);
            if (owner != null && !suspendEvents)
                owner.OnRomRemoved(itemID);
            // In case events suspended
            if (owner != null)
                if (!owner.MarkedToBeDeleted.Contains(itemID))
                    owner.MarkedToBeDeleted.Add(itemID);
            return val;
        }
        /// <summary>
        /// Remove rom from collection
        /// </summary>
        /// <param name="itemID">The rom id to remove</param>
        /// <param name="raiseEvent">Indecate whether to raise the RomRemoved event or not</param>
        /// <returns>True if removed successfully otherwise false</returns>
        public bool Remove(string itemID, bool raiseEvent)
        {
            bool val = items.Remove(this[itemID]);
            if (owner != null && raiseEvent)
                owner.OnRomRemoved(itemID);

            // In case events suspended
            if (owner != null)
                if (!owner.MarkedToBeDeleted.Contains(itemID))
                    owner.MarkedToBeDeleted.Add(itemID);

            return val;
        }
        /// <summary>
        /// Remove all roms that belong to given console from the collection (FROM MEMORY ONLY !!)
        /// </summary>
        /// <param name="consoleID">The console id to use to remove roms</param>
        public void RemoveThatBelongToConsole(string consoleID)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ParentConsoleID == consoleID)
                {
                    // In case events suspended
                    if (owner != null)
                        if (!owner.MarkedToBeDeleted.Contains(items[i].ID))
                            owner.MarkedToBeDeleted.Add(items[i].ID);

                    items.RemoveAt(i);
                    i--;
                }
            }
        }
        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns>items.GetEnumerator()</returns>
        public IEnumerator<Rom> GetEnumerator()
        {
            return items.GetEnumerator();
        }
        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns>items.GetEnumerator()</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
        /// <summary>
        /// Move rom from location to another
        /// </summary>
        /// <param name="index">The rom index to move</param>
        /// <param name="newIndex">The new index to move rom into</param>
        public void Move(int index, int newIndex)
        {
            string name = this[index].Name;

            if (items.Count < 2) goto RaiseEvent;
            if (index == newIndex) return;
            // Get console
            Rom rom = this[index];
            items.RemoveAt(index);

            if (owner != null)
                if (!owner.MarkedToBeDeleted.Contains(rom.ID))
                    owner.MarkedToBeDeleted.Add(rom.ID);

            if (newIndex >= 0)
                items.Insert(newIndex, rom);
            else
                items.Add(rom);

            RaiseEvent:
            if (owner != null && !suspendEvents)
                owner.OnRomMoved(name);

        }
        /// <summary>
        /// Sort the collection
        /// </summary>
        /// <param name="comparer">The comparer to use</param>
        public void Sort(RomComparer comparer)
        {
            items.Sort(comparer);
            if (owner != null && !suspendEvents)
                owner.OnRomsSort();
        }
        /// <summary>
        /// Get all roms that are children of given rom
        /// </summary>
        /// <param name="parentID">The parent rom id</param>
        /// <returns></returns>
        public Rom[] GetChildrenOf(string parentID)
        {
            List<Rom> roms = items.FindAll(
                         delegate (Rom rom)
                         {
                             if (rom.IsChildRom)
                                 return rom.ParentRomID == parentID;
                             else
                                 return false;
                         }
                     );
            return roms.ToArray();
        }
    }
}
