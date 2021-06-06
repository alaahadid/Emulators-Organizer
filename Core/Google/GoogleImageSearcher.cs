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
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Diagnostics;
using Google.API.Search;

namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Class for searching images using goolge
    /// </summary>
    public class GoogleImageSearcher
    {
        public static string RESULT = "";
        public static string[] ImageThunmbnails;
        public static string[] ImageLinks;
        public static string[] ImageSizes;
        public static int ResultsCount = 20;
        static Google.API.Search.GimageSearchClient client;
        static IList<IImageResult> results;
        public static void GetImages(string SearchName, string imageSize)
        {
            client = new GimageSearchClient("http://www.google.com");
            //IAsyncResult result = client.BeginSearch(SearchName, ResultsCount, new AsyncCallback(AsyncCallbackV), null);
            results = client.Search(SearchName, ResultsCount);
            if (imageSize == "")
            {
                List<string> li = new List<string>();
                List<string> rli = new List<string>();
                List<string> sili = new List<string>();
                for (int i = 0; i < results.Count; i++)
                {
                    li.Add(results[i].TbImage.Url);
                    rli.Add(results[i].Url);
                    sili.Add(results[i].Width + " x " + results[i].Height);
                }
                ImageThunmbnails = li.ToArray();
                ImageLinks = rli.ToArray();
                ImageSizes = sili.ToArray();
            }
            else
            {
                string[] ss = imageSize.Split(new char[] { 'x' });
                int w = int.Parse(ss[0]);
                int h = int.Parse(ss[1]);
                List<string> li = new List<string>();
                List<string> rli = new List<string>();
                List<string> sili = new List<string>();
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].Width >= w & results[i].Height >= h)
                    {
                        li.Add(results[i].TbImage.Url);
                        rli.Add(results[i].Url);
                        sili.Add(results[i].Width + " x " + results[i].Height);
                    }
                }
                ImageThunmbnails = li.ToArray();
                ImageLinks = rli.ToArray();
                ImageSizes = sili.ToArray();
            }
        }
    }
    public enum GoogleSearchMode
    { RomName, RomName_Console, RomName_Console_Emulator }
}
