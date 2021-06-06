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
using System.Windows.Forms;
using System.Security.Cryptography;
namespace AHD.Utilities
{
    public class HelperTools
    {
        static string _startUpPath = Application.StartupPath;
        public static string startUpPath
        { get { return _startUpPath; } set { _startUpPath = value; } }
        public static string GetFileSize(string FilePath)
        {
            if (File.Exists(Path.GetFullPath(FilePath)) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                string Unit = " Byte";
                double Len = Info.Length;
                if (Info.Length >= 1024)
                {
                    Len = Info.Length / 1024.00;
                    Unit = " KB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " MB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " GB";
                }
                return Len.ToString("F2") + Unit;
            }
            return "";
        }
        public static string GetFullPath(string FilePath)
        {
            if (FilePath == null)
                return "";
            if (FilePath.Length == 0)
                return FilePath;
            if (FilePath.Substring(0, 1) == ".")
            {
                return (_startUpPath + FilePath.Substring(1));
            }
            else
                return FilePath;
        }
        public static string GetDotPath(string FilePath)
        {
            if (FilePath == "")
                return "";
            string Dir = FilePath;
            if (Path.GetDirectoryName(Dir).Length >= _startUpPath.Length)
            {
                if (Path.GetDirectoryName(Dir).Substring(0, _startUpPath.Length) == _startUpPath)
                {
                    Dir = "." + Dir.Substring(_startUpPath.Length);
                }
            }
            return Dir;
        }
        public static string GetSize(long size)
        {
            string Unit = " Byte";
            double Len = size;
            if (size >= 1024)
            {
                Len = size / 1024.00;
                Unit = " KB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " MB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " GB";
            }
            if (Len < 0)
                return "???";
            return Len.ToString("F2") + Unit;
        }
        public static long GetSizeAsBytes(string FilePath)
        {
            if (File.Exists(Path.GetFullPath(FilePath)) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                return Info.Length;
            }
            return 0;
        }
        public static bool RenameFile(string filePath, string newName, out string newPath, out string failException)
        {
            try
            {
                string fol = Path.GetDirectoryName(GetFullPath(filePath));
                if (fol == "")
                    fol = Path.GetPathRoot(GetFullPath(filePath));
                string OREGENALPATH = GetFullPath(filePath);
                string Original = GetFullPath(filePath);
                string ext = Path.GetExtension(filePath);

                newPath = fol + "\\" + newName + ext;
                File.Copy(Original, newPath);
                FileInfo inf = new FileInfo(Original);
                inf.IsReadOnly = false;
                File.Delete(Original);
                failException = "";
                return true;
            }
            catch (Exception ex) { failException = ex.Message; }
            newPath = "";
            return false;
        }
        public static bool IsStringContainsNumbers(string text)
        {
            foreach (char chr in text.ToCharArray())
            {
                int tt = 0;
                if (int.TryParse(chr.ToString(), out tt))
                    return true;
            }
            return false;
        }
        public static string CalculateSHA1(string filePath)
        {
            filePath = GetFullPath(filePath);
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = getBuffer(filePath);

                string Sha1 = "";
                SHA1Managed managedSHA1 = new SHA1Managed();
                byte[] shaBuffer = managedSHA1.ComputeHash(fileBuffer);

                foreach (byte b in shaBuffer)
                    Sha1 += b.ToString("x2").ToLower();

                return Sha1;
            }
            return "";
        }
        private static byte[] getBuffer(string filePath)
        {
            byte[] fileBuffer;
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileBuffer = new byte[fileStream.Length];
            fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
            fileStream.Close();

            return fileBuffer;
        }
    }
}
