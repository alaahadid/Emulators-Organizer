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
    [Serializable()]
    public class ConfigurationFile
    {
        public ConfigurationFile()
        {
        }
        public ConfigurationFile(string filePath)
        {
            this.filePath = filePath;
            Reload();
        }
        string filePath = "";
        List<ConfigurationValue> values = new List<ConfigurationValue>();

        public string FilePath
        { get { return filePath; } set { filePath = value; } }
        public List<ConfigurationValue> Values
        { get { return values; } set { values = value; } }
        /// <summary>
        /// Check if a property exists
        /// </summary>
        /// <param name="propertyName">The property name to check</param>
        /// <returns>The propery index if found other wise -1</returns>
        public int IsPropertyExists(string propertyName)
        {
            int i = 0;
            foreach (ConfigurationValue val in values)
            {
                if (val.Name == propertyName)
                    return i;
                i++;
            }
            return -1;
        }
        /// <summary>
        /// Reload the values from the file
        /// </summary>
        public void Reload()
        {
            if (!File.Exists(HelperTools.GetFullPath(filePath)))
                return;
            string[] lines = File.ReadAllLines(HelperTools.GetFullPath(filePath));
            if (values == null)
                values = new List<ConfigurationValue>();
            string currentCategory = "";
            foreach (string line in lines)
            {
                if (line != "" && !line.StartsWith("#") && !line.StartsWith("/"))
                {
                    if (line.StartsWith("["))//category
                    {
                        currentCategory = line.Replace("[", "");
                        currentCategory = currentCategory.Replace("]", "");
                    }
                    else
                    {
                        string[] codes;
                        if (line.Contains("="))
                            codes = line.Split(new char[] { '=' });
                        else
                            codes = line.Split(new char[] { ' ' });
                        string property = codes[0];
                        int index = IsPropertyExists(property);
                        if (index == -1)//create new
                        {
                            ConfigurationValue value = new ConfigurationValue();
                            value.Name = property;
                            value.Category = currentCategory;
                            if (codes.Length > 1)
                            {
                                for (int i = 1; i < codes.Length; i++)
                                {
                                    if (codes[i] != "")
                                    {
                                        value.Value = codes[i];
                                        i++;
                                        while (i < codes.Length)
                                        {
                                            value.Value += " " + codes[i]; i++;
                                        }
                                        value.Values = new List<string>();
                                        value.Values.Add(value.Value);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                value.Value = "";
                                value.Values = new List<string>();
                                value.Values.Add("");
                            }
                            values.Add(value);
                        }
                        else//update existed
                        {
                            ConfigurationValue value = values[index];
                            if (codes.Length > 1)
                            {
                                for (int i = 1; i < codes.Length; i++)
                                {
                                    if (codes[i] != "")
                                    {
                                        value.Value = codes[i];
                                        i++;
                                        while (i < codes.Length)
                                        {
                                            value.Value += " " + codes[i]; i++;
                                        }
                                        if (!value.Values.Contains(value.Value))
                                            value.Values.Add(value.Value);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                value.Value = "";
                                if (!value.Values.Contains(""))
                                    value.Values.Add("");
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Save values to the file
        /// </summary>
        public void Save()
        {
            if (!File.Exists(HelperTools.GetFullPath(filePath)))
                return;
            string[] lines = File.ReadAllLines(HelperTools.GetFullPath(filePath));
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "" && !lines[i].StartsWith("#") && !lines[i].StartsWith("/") && !lines[i].StartsWith("["))
                {
                    //get the property
                    string[] codes;
                    string spliter = "";
                    if (lines[i].Contains("="))
                    {
                        codes = lines[i].Split(new char[] { '=' });
                        spliter = "=";
                    }
                    else
                    {
                        spliter = " ";
                        codes = lines[i].Split(new char[] { ' ' });
                        if (codes.Length > 1)
                        {
                            int j = 1;
                            while (codes[j] == "")
                            {
                                spliter += " ";
                                j++;
                                if (j == codes.Length)
                                    break;
                            }
                        }
                    }
                    string theProperty = codes[0];
                    int index = IsPropertyExists(theProperty);
                    if (index >= 0)
                    {
                        //update value
                        lines[i] = theProperty + spliter + values[index].Value;
                    }
                }
            }
            //save
            File.WriteAllLines(HelperTools.GetFullPath(filePath), lines);
        }
        public void SaveAs(string newFile)
        {
            if (!File.Exists(HelperTools.GetFullPath(filePath)))
                return;
            string[] lines = File.ReadAllLines(HelperTools.GetFullPath(filePath));
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "" && !lines[i].StartsWith("#") && !lines[i].StartsWith("/") && !lines[i].StartsWith("["))
                {
                    //get the property
                    string[] codes;
                    string spliter = "";
                    if (lines[i].Contains("="))
                    {
                        codes = lines[i].Split(new char[] { '=' });
                        spliter = "=";
                    }
                    else
                    {
                        spliter = " ";
                        codes = lines[i].Split(new char[] { ' ' });
                        if (codes.Length > 1)
                        {
                            int j = 1;
                            while (codes[j] == "")
                            {
                                spliter += " ";
                                j++;
                                if (j == codes.Length)
                                    break;
                            }
                        }
                    }
                    string theProperty = codes[0];
                    int index = IsPropertyExists(theProperty);
                    if (index >= 0)
                    {
                        //update value
                        lines[i] = theProperty + spliter + values[index].Value;
                    }
                }
            }
            //save
            File.WriteAllLines(HelperTools.GetFullPath(newFile), lines);
        }
        public override string ToString()
        {
            return HelperTools.GetFullPath(filePath);
        }
    }
}
