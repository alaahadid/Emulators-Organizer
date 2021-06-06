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
using AHD.Utilities;
namespace AHD.EO.Base
{
    public class CommandlinesEncoder
    {
        public static string ToCommandlinesString(CommandlinesGroup[] groups, string rompath, List<string> attachedFiles)
        {
            string commandline = "";
            foreach (CommandlinesGroup group in groups)
            {
                if (group.Enabled)
                {
                    foreach (Commandline cm in group.Commandlines)
                    {
                        if (cm.Enabled)
                        {
                            // Decode command code
                            commandline += DecodeCommand(cm.Code.ToLower(), rompath, attachedFiles) + " ";
                            foreach (Parameter par in cm.Parameters)
                            {
                                if (par.Enabled)
                                {
                                    // Decode parameter command
                                    commandline += DecodeCommand(par.Code.ToLower(), rompath, attachedFiles) + " ";
                                }
                            }
                        }
                    }
                }
            }
            if (commandline.Length > 0)
                if (commandline.Substring(commandline.Length - 1, 1) == " ")
                    commandline = commandline.Substring(0, commandline.Length - 1);
            return commandline;
        }
        public static string DecodeCommand(string command, string rompath, List<string> attachedFiles)
        {
            string val = "";
            if (!command.Contains("><"))// no constructed
            {
                val = command.Replace("<rompath>", GetStringCommand(rompath));
                val = val.Replace("<romname>", GetStringCommand(Path.GetFileName(rompath)));
                val = val.Replace("<romnamewithoutextension>", GetStringCommand(Path.GetFileNameWithoutExtension(rompath)));
                val = val.Replace("<romfolder>", GetStringCommand(GetFileFolder(rompath)));
                // Decode attached files
                for (int i = 0; i < val.Length; i++)
                {
                    if ((val.Length - i) >= "romattachedfile".Length)
                    {
                        if (val.Substring(i, "romattachedfile".Length) == "romattachedfile")
                        {
                            int oldi = i;
                            int sindex = i + "romattachedfile".Length + 1;
                            int eindex = val.Length;
                            // keep looking for ")"
                            while (i < val.Length)
                            {
                                i++;
                                if (val.Substring(i, 1) == ")")
                                { eindex = i; break; }
                            }
                            string original = "<" + val.Substring(oldi, (eindex + 1) - oldi) + ">";
                            string code = val.Substring(sindex, eindex - sindex);

                            int index = 0;
                            if (int.TryParse(code, out index))
                            {
                                index -= 1;
                                if (attachedFiles != null)
                                {
                                    if (attachedFiles.Count > index)
                                    {
                                        val = val.Replace(original, GetStringCommand(attachedFiles[index]));
                                    }
                                    else
                                    {
                                        val = val.Replace(original, "");
                                    }
                                }
                                else
                                {
                                    val = val.Replace(original, "");
                                }
                            }

                            break;
                        }
                    }
                }
            }
            else // we have constructed values ...
            {
                val = command;
                for (int i = 0; i < val.Length; i++)
                {
                    if ((val.Length - i) >= 1)
                    {
                        if (val.Substring(i, 1) == "<")
                        {
                            string newVal = "";
                            while (i < val.Length)
                            {
                                i++;
                                if (val.Substring(i, 1) == ">")
                                {
                                    if ((val.Length - i) > 1)
                                    {
                                        if (val.Substring(i, 2) != "><")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            i += 1;
                                            newVal += "><";
                                        }
                                    }
                                    else
                                        break;
                                }
                                else
                                {
                                    newVal += val.Substring(i, 1);
                                }
                            }
                            newVal = "<" + newVal + ">";
                            // Decode new val
                            string newCode = newVal.Replace("<rompath>", rompath);
                            newCode = newCode.Replace("<romname>", Path.GetFileName(rompath));
                            newCode = newCode.Replace("<romnamewithoutextension>", Path.GetFileNameWithoutExtension(rompath));
                            newCode = newCode.Replace("<romfolder>", GetFileFolder(rompath));
                            // Now add the code
                            val = val.Replace(newVal, GetStringCommand(newCode));
                            break;
                        }
                    }
                }
                // Decode attached files
                for (int i = 0; i < val.Length; i++)
                {
                    if ((val.Length - i) >= "romattachedfile".Length)
                    {
                        if (val.Substring(i, "romattachedfile".Length) == "romattachedfile")
                        {
                            int oldi = i;
                            int sindex = i + "romattachedfile".Length + 1;
                            int eindex = val.Length;
                            // keep looking for ")"
                            while (i < val.Length)
                            {
                                i++;
                                if (val.Substring(i, 1) == ")")
                                { eindex = i; break; }
                            }
                            string original = "<" + val.Substring(oldi, (eindex + 1) - oldi) + ">";
                            string code = val.Substring(sindex, eindex - sindex);

                            int index = 0;
                            if (int.TryParse(code, out index))
                            {
                                index -= 1;
                                if (attachedFiles != null)
                                {
                                    if (attachedFiles.Count > index)
                                    {
                                        val = val.Replace(original, GetStringCommand(attachedFiles[index]));
                                    }
                                    else
                                    {
                                        val = val.Replace(original, "");
                                    }
                                }
                                else
                                {
                                    val = val.Replace(original, "");
                                }
                            }

                            break;
                        }
                    }
                }
            }
            //val = val.Replace("\"" + "\"", "");
            //System.IO.File.WriteAllText(".\\test.txt", val);
            return val;
        }
        private static string GetStringCommand(string command)
        {
            if (command.Contains(" "))
                return "\"" + command + "\"";
            else
                return command;
        }
        private static string GetFileFolder(string file)
        {
            string folder = Path.GetDirectoryName(file);
            if (folder == "")
                folder = Path.GetPathRoot(file);
            return folder + "\\";
        }
    }
}
