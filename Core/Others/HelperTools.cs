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
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using ComponentAce.Compression.Libs.zlib;
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// The helper tools
    /// </summary>
    public class HelperTools
    {
        static string _startUpPath = Application.StartupPath;
        public static void Initialize()
        {
            _startUpPath = Application.StartupPath;
            Trace.WriteLine(" Application.StartupPath = " + Application.StartupPath, "CORE");
            Trace.WriteLine(" Environment.GetCommandLineArgs()[0] = " + Environment.GetCommandLineArgs()[0], "CORE");
        }
        /// <summary>
        /// Get or set the program start up path. Should set at program startup
        /// </summary>
        public static string StartUpPath
        { get { return _startUpPath; } }
        /// <summary>
        /// Get a file size as label with unit
        /// </summary>
        /// <param name="FilePath">The file path</param>
        /// <returns>The file size label with unit</returns>
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
        /// <summary>
        /// Get full file path, if the path is a dot path, it returns actual path.
        /// </summary>
        /// <param name="FilePath">The file path</param>
        /// <returns>The full file path</returns>
        public static string GetFullPath(string FilePath)
        {
            if (FilePath == null)
                return "";
            if (FilePath.Length == 0)
                return FilePath;
            string ai_path_code = "";
            if (FilePath.StartsWith("AI"))
            {
                for (int i = 0; i < FilePath.Length; i++)
                {
                    if (FilePath[i] == ')')
                    {
                        ai_path_code = FilePath.Substring(0, i + 1);
                        FilePath = FilePath.Substring(i + 1, FilePath.Length - (i + 1));
                        break;
                    }
                }
            }
            // Update
            if (FilePath.Substring(0, 1) == ".")
            {
                FilePath = _startUpPath + FilePath.Substring(1);
            }
            return ai_path_code + FilePath;
        }
        /// <summary>
        /// Get the dot path of file.
        /// </summary>
        /// <param name="FilePath">The file path</param>
        /// <returns>The dot path if the file is inside program folder otherwise it returns the same path</returns>
        public static string GetDotPath(string FilePath)
        {
            if (FilePath == "")
                return "";
            string ai_path_code = "";
            if (FilePath.StartsWith("AI"))
            {
                for (int i = 0; i < FilePath.Length; i++)
                {
                    if (FilePath[i] == ')')
                    {
                        ai_path_code = FilePath.Substring(0, i + 1);
                        FilePath = FilePath.Substring(i + 1, FilePath.Length - (i + 1));
                    }
                }

            }

            if (Path.GetDirectoryName(FilePath).Length >= _startUpPath.Length)
            {
                if (Path.GetDirectoryName(FilePath).Substring(0, _startUpPath.Length) == _startUpPath)
                {
                    FilePath = "." + FilePath.Substring(_startUpPath.Length);
                }
            }
            FilePath = ai_path_code + FilePath;
            return FilePath;
        }
        /// <summary>
        /// Get size label with unit
        /// </summary>
        /// <param name="size">The size in bytes</param>
        /// <returns>The size label with unit</returns>
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
        /// <summary>
        /// Get size label with unit
        /// </summary>
        /// <param name="size">The size in bytes</param>
        /// <returns>The size label with unit</returns>
        public static string GetSize(ulong size)
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
        /// <summary>
        /// Get file size in bytes
        /// </summary>
        /// <param name="FilePath">The file path</param>
        /// <returns>The file size in bytes.</returns>
        public static long GetSizeAsBytes(string FilePath)
        {
            if (File.Exists(GetFullPath(GetPathFromAIPath(FilePath))) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                return Info.Length;
            }
            return 0;
        }
        /// <summary>
        /// Rename a file in disk
        /// </summary>
        /// <param name="filePath">The file path to rename</param>
        /// <param name="newName">The new name for this file</param>
        /// <param name="newPath">The new path for the file after rename.</param>
        /// <param name="failException">If the renaming failed, this exception message returns</param>
        /// <returns>True if file renamed successfuly otherwise false</returns>
        public static bool RenameFile(string filePath, string newName, out string newPath, out string failException)
        {
            try
            {
                newName = newName.Replace(":", " ");
                newName = newName.Replace("/", "-");
                newName = newName.Replace(@"\", "-");
                newName = newName.Replace("*", " ");
                newName = newName.Replace("<", " ");
                newName = newName.Replace(">", " ");
                newName = newName.Replace("|", " ");
                newName = newName.Replace(@"""", "'");
                string fol = Path.GetDirectoryName(GetFullPath(filePath));
                if (fol == "")
                    fol = Path.GetPathRoot(GetFullPath(filePath));
                string OREGENALPATH = GetFullPath(filePath);
                string Original = GetFullPath(filePath);
                string ext = Path.GetExtension(filePath);

                newPath = fol + "\\" + newName + ext;
                //File.Copy(Original, newPath);
                //FileInfo inf = new FileInfo(Original);
                //inf.IsReadOnly = false;
                //File.Delete(Original);
                // failException = "";
                // return true;

                File.Move(Original, newPath);
                failException = "";
                return true;
            }
            catch (Exception ex) { failException = ex.Message; }
            newPath = "";
            return false;
        }
        /// <summary>
        /// Check if string contain numbers
        /// </summary>
        /// <param name="text">The string to check</param>
        /// <returns>True if given string contain numbers otherwise false</returns>
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
        /// <summary>
        /// Calculate file CRC
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        /// <returns>File CRC</returns>
        public static string CalculateCRC(string filePath)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] fileBuffer = new byte[fileStream.Length];
                fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
                fileStream.Close();
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
        /// Calculate file CRC
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        /// <param name="bytesToSkip">How many bytes should be skipped from the file (header)</param>
        /// <returns>File CRC</returns>
        public static string CalculateCRC(string filePath, int bytesToSkip)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                byte[] fileBuffer = new byte[fileStream.Length - bytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                fileStream.Close();

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
        /// Calculate SHA1 for file
        /// </summary>
        /// <param name="filePath">The file path to calculate SHA1 for</param>
        /// <returns>The SHA1 for given file otherwise null</returns>
        public static string CalculateSHA1(string filePath)
        {
            filePath = GetFullPath(filePath);
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = GetBuffer(filePath);

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
        /// Get file bytes buffer
        /// </summary>
        /// <param name="filePath">The file path to get buffer for</param>
        /// <returns>The file buffer otherwise null</returns>
        public static byte[] GetBuffer(string filePath)
        {
            byte[] fileBuffer;
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileBuffer = new byte[fileStream.Length];
            fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
            fileStream.Close();

            return fileBuffer;
        }
        /// <summary>
        /// Get the directory path for a file
        /// </summary>
        /// <param name="FilePath">The full file path</param>
        /// <returns>The file folder path</returns>
        public static string GetDirectory(string FilePath)
        {
            string val = Path.GetDirectoryName(FilePath);
            if (val == "")
                val = Path.GetPathRoot(FilePath);
            return val;
        }
        /// <summary>
        /// Return the archive file path from AI path. The archive path returned will be DOT path.
        /// </summary>
        /// <param name="AIPath">The AI path.</param>
        /// <returns></returns>
        public static string GetPathFromAIPath(string AIPath)
        {
            if (AIPath.StartsWith("AI"))
            {
                for (int i = 0; i < AIPath.Length; i++)
                {
                    if (AIPath[i] == ')')
                    {
                        return AIPath.Substring(i + 1, AIPath.Length - (i + 1));
                    }
                }
            }
            return AIPath;
        }
        /// <summary>
        /// Check a path to indicates if it is AI path
        /// </summary>
        /// <param name="AIPath">The AI path to check</param>
        /// <returns></returns>
        public static bool IsAIPath(string AIPath)
        {
            return AIPath.StartsWith("AI");
        }
        public static int GetIndexFromAIPath(string AIPath)
        {
            string v = "";
            if (AIPath.StartsWith("AI"))
            {

                for (int i = 3; i < AIPath.Length; i++)
                {
                    if (AIPath[i] == ')')
                    {
                        break;
                    }
                    else
                    {
                        v += AIPath[i];
                    }
                }

                int ov = -1;
                int.TryParse(v, out ov);
                return ov;
            }
            return -1;
        }

        // 7zip
        public static void CompressData(byte[] inData, out byte[] outData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream, zlibConst.Z_DEFAULT_COMPRESSION))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                outData = outMemoryStream.ToArray();
            }
        }
        public static void DecompressData(byte[] inData, out byte[] outData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                outData = outMemoryStream.ToArray();
            }
        }
        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }
    }
}
