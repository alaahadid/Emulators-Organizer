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
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using SevenZip;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("SMS", new string[] { ".txt" }, CompareMode.CRC | CompareMode.SHA1, true, true, false)]
    public class DatabaseFile_SMS : DatabaseFile
    {
        public bool IgnoreSkipForCompressed = true;
        public bool _IgnoreEmptyFields = false;
        public bool _AddAllDataItems = false;
        private string _export_platform;
        private string _export_gametype;
        private bool _CalculateCRCInsideOfArchive;
        public override List<DBEntry> LoadFile(string filePath)
        {
            //clear files
            List<DBEntry> files = new List<DBEntry>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);

            OnProgressStarted("Reading SMS file ...");
            //decode lines
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))//we got a game here
                {
                    DBEntry ROM = DBEntry.Empty;
                    ROM.CRC = lines[i].Replace("[", "").Replace("]", "");
                    i++;
                    //ROM.Name = lines[i];
                    ROM.Properties.Add(new PropertyStruct("Name", lines[i]));
                    ROM.FileNames.Add(lines[i]);

                    i++;
                    while (i < lines.Length)
                    {
                        if (lines[i].Contains(":"))//we have a property here
                        {
                            string[] code = lines[i].Split(new string[] { ": " }, StringSplitOptions.None);
                            string prop = code[0];
                            string val = "";
                            if (code.Length > 1)
                                val = code[1];
                            //else
                            ROM.Properties.Add(new PropertyStruct(prop, val));
                            i++;
                        }
                        else if (lines[i] == "_________________________")
                        {
                            string desc = "";
                            i++;
                            while (i < lines.Length)
                            {
                                if (!lines[i].StartsWith("["))
                                {
                                    desc += lines[i] + "\n";
                                }
                                else
                                {
                                    //ROM.Properties.Add(new PropertyStruct("Description", ROM.Description));
                                    break;
                                }
                                i++;
                            }
                            i--;
                            ROM.Properties.Add(new PropertyStruct("Description", desc));
                            files.Add(ROM);
                            // Read description
                            break;
                        }
                    }
                }
                int x = (i * 100) / lines.Length;
                base.OnProgress(x, "Reading SMS file ... [" + x + " %]");
            }
            OnProgressFinished("Reading SMS file ...");
            return files;
        }
        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            OnProgressStarted("Saving SMS database file ....");


            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
            List<string> lines = new List<string>();
            // add header
            lines.Add("[]");
            lines.Add("!missing");
            lines.Add("Platform: " + _export_platform);
            lines.Add("Gametype: " + _export_gametype);
            lines.Add("_________________________");
            lines.Add("");
            lines.Add("");
            int i = 0;
            foreach (DBEntry file in entries)
            {
                bool useSkip = true;
                if (HelperTools.IsAIPath(file.TemporaryPathForExport))
                {
                    SevenZipExtractor extractor =
                         new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(file.TemporaryPathForExport)));
                    int index = HelperTools.GetIndexFromAIPath(file.TemporaryPathForExport);
                    try
                    {
                        FileStream mstream = new FileStream(Path.GetTempPath() + "\\eo\\test.tst", FileMode.Create, FileAccess.Write);
                        extractor.ExtractFile(index, mstream);
                        lines.Add("[" + CalculateCRC(Path.GetTempPath() + "\\eo\\test.tst", true) + "]");
                    }
                    catch//error, just save crc of compressed file 
                    {
                        lines.Add("[" + CalculateCRC(file.TemporaryPathForExport, true).ToUpper() + "]");
                    }
                }
                else if (!_CalculateCRCInsideOfArchive)
                    lines.Add("[" + CalculateCRC(file.TemporaryPathForExport, true).ToUpper() + "]");
                else
                {
                    if (archiveExtensions.Contains(Path.GetExtension(file.TemporaryPathForExport).ToLower()))
                    {
                        useSkip = !IgnoreSkipForCompressed;
                        try
                        {
                            SevenZip.SevenZipExtractor extractor = new SevenZip.SevenZipExtractor(file.TemporaryPathForExport);
                            FileStream mstream = new FileStream(Path.GetTempPath() + "\\eo\\test.tst", FileMode.Create, FileAccess.Write);
                            extractor.ExtractFile(0, mstream);
                            lines.Add("[" + CalculateCRC(Path.GetTempPath() + "\\eo\\test.tst", true) + "]");
                        }
                        catch//error, just save crc of compressed file 
                        {
                            lines.Add("[" + CalculateCRC(file.TemporaryPathForExport, useSkip).ToUpper() + "]");
                        }
                    }
                    else
                    {
                        lines.Add("[" + CalculateCRC(file.TemporaryPathForExport, true).ToUpper() + "]");
                    }
                }
                lines.Add(file.GetPropertyValue("Name"));
                if (!_AddAllDataItems)
                {
                    if (!_IgnoreEmptyFields)
                    {
                        lines.Add("Platform: " + file.GetPropertyValue("Platform"));
                        lines.Add("Region: " + file.GetPropertyValue("Region"));
                        lines.Add("Media: " + file.GetPropertyValue("Media"));
                        lines.Add("Controller: " + file.GetPropertyValue("Controller"));
                        lines.Add("Genre: " + file.GetPropertyValue("Genre"));
                        lines.Add("Gametype: " + file.GetPropertyValue("Gametype"));
                        lines.Add("Release Year: " + file.GetPropertyValue("Release Year"));
                        lines.Add("Developer: " + file.GetPropertyValue("Developer"));
                        lines.Add("Publisher: " + file.GetPropertyValue("Publisher"));
                        lines.Add("Players: " + file.GetPropertyValue("Players"));
                    }
                    else
                    {
                        if (file.GetPropertyValue("Platform") != "")
                            lines.Add("Platform: " + file.GetPropertyValue("Platform"));
                        if (file.GetPropertyValue("Region") != "")
                            lines.Add("Region: " + file.GetPropertyValue("Region"));
                        if (file.GetPropertyValue("Media") != "")
                            lines.Add("Media: " + file.GetPropertyValue("Media"));
                        if (file.GetPropertyValue("Controller") != "")
                            lines.Add("Controller: " + file.GetPropertyValue("Controller"));
                        if (file.GetPropertyValue("Genre") != "")
                            lines.Add("Genre: " + file.GetPropertyValue("Genre"));
                        if (file.GetPropertyValue("Gametype") != "")
                            lines.Add("Gametype: " + file.GetPropertyValue("Gametype"));
                        if (file.GetPropertyValue("Release Year") != "")
                            lines.Add("Release Year: " + file.GetPropertyValue("Release Year"));
                        if (file.GetPropertyValue("Developer") != "")
                            lines.Add("Developer: " + file.GetPropertyValue("Developer"));
                        if (file.GetPropertyValue("Publisher") != "")
                            lines.Add("Publisher: " + file.GetPropertyValue("Publisher"));
                        if (file.GetPropertyValue("Players") != "")
                            lines.Add("Players: " + file.GetPropertyValue("Players"));
                    }

                }
                else//add all of rom
                {
                    foreach (PropertyStruct pro in file.Properties)
                    {
                        if (pro.Property == "Name" || pro.Property == "Description")
                            continue;
                        if (!_IgnoreEmptyFields)
                        {
                            lines.Add(pro.Property + "=" + pro.Value);
                        }
                        else
                        {
                            if (pro.Value != null)
                                if (pro.Value != "")
                                    lines.Add(pro.Property + "=" + pro.Value);
                        }
                    }
                }
                lines.Add("_________________________");
                // Add rom description
                string[] descLines = file.GetPropertyValue("Description").Split(new char[] { '\n' });
                lines.AddRange(descLines);
                lines.Add("");
                int x = (i * 100) / entries.Count;
                OnProgress(x, "Saving SMS database file... [" + x + " %]");
                i++;
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.Unicode);
            OnProgressFinished("SMS database file saved successfully.");
        }
        public override SeparateItem[] GetSeparate(string filePath)
        {
            List<SeparateItem> items = new List<SeparateItem>();
            //read lines
            string[] lines = File.ReadAllLines(filePath);
            OnProgressStarted("Separating SMS database file ...");
            //decode lines
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))//we got a game here
                {
                    SeparateItem item = new SeparateItem();
                    item.Data = lines[i] + "\n";
                    i++;
                    item.FileName = lines[i] + ".txt";
                    item.Data += lines[i] + "\n";
                    i++;
                    while (true)
                    {
                        item.Data += lines[i] + "\n";
                        if (lines[i].Contains(":"))//we have a property here
                        {
                            i++;
                        }
                        else if (lines[i] == "_________________________")
                        {
                            i++;
                            while (i < lines.Length)
                            {
                                if (!lines[i].StartsWith("["))
                                {
                                    item.Data += lines[i] + "\n";
                                }
                                else
                                {
                                    break;
                                }
                                i++;
                            }
                            i--;
                            items.Add(item);
                            break;
                        }
                    }
                }
                int x = (i * 100) / lines.Length;
                OnProgress(x, "Separating SMS file ... [" + x + " %]");
            }
            OnProgressFinished("SMS separated successfully.");
            return items.ToArray();
        }
        public override void ShowExportOptions()
        {
            Frm_SMSExportOptions frm = new Frm_SMSExportOptions(this);
            frm.ShowDialog();

            _export_platform = frm.Platform;
            _export_gametype = frm.Gametype;
            _CalculateCRCInsideOfArchive = frm._CalculateCRCInsideOfArchive;
        }
    }
}
