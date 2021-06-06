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
using System.Net;
using System.Windows.Forms;
namespace AHD.Utilities
{
    /// <summary>
    /// A class used to get info from MobyGames.com
    /// </summary>
    public class MobyGamesSearcher
    {
        /// <summary>
        /// Get info links for a game
        /// </summary>
        /// <param name="gameName">The game name</param>
        /// <returns>games as array</returns>
        public static MobyGamesGame[] GetLinks(string gameName)
        {
            try
            {
                List<MobyGamesGame> links = new List<MobyGamesGame>();
                WebClient cl = new WebClient();
                string code = cl.DownloadString(MakeSearchLink(gameName));
                //get links
                string[] codes = code.Split(new string[] { "<", ">" }, StringSplitOptions.None);
                for (int i = 0; i < codes.Length; i++)
                {
                    if (codes[i].Length >= "a href".Length)
                    {
                        //"a href=\\\"/game/dr-mario\\\""
                        if (codes[i].Substring(0, "a href".Length) == "a href")
                        {
                            //get link
                            string[] text = codes[i].Split(new char[] { '=' });
                            string link = text[1].Replace(@"""/", "");
                            link = link.Replace(@"""", "");
                            link = link.Replace(@"\", "/");
                            link = "http://www.mobygames.com" + link;
                            //get image link
                            string imageLink = "";
                            //"img class=\\\"searchResultImage\\\" alt=\\\"Dr. Mario Game Boy Front Cover\\\" border=\\\"0\\\" src=\\\"/images/i/28/14/5614.jpeg\\\" height=\\\"59\\\" width=\\\"60\\\" "
                            while (true)
                            {
                                if (i == codes.Length)
                                    break;
                                if (codes[i].Length >= 3)
                                {
                                    if (codes[i].Substring(0, 3) == "img")
                                    {
                                        string[] imCode = codes[i].Split(new string[] { "src=" }, StringSplitOptions.None);
                                        string[] imlinkCode = imCode[1].Split(new string[] { @"""" }, StringSplitOptions.None);
                                        imageLink = imlinkCode[1].Substring(0, imlinkCode[1].Length - 1);
                                        imageLink = "http://www.mobygames.com" + imageLink;
                                        break;
                                    }
                                }
                                i++;
                            }

                            links.Add(new MobyGamesGame(link, imageLink));
                        }
                    }
                }
                return links.ToArray();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
            }
            return new MobyGamesGame[0];
        }
        /// <summary>
        /// Get info as string from given link. 
        /// </summary>
        /// <param name="link">The link must be to MobyGame.com and for a game</param>
        /// <returns>Game info</returns>
        public static string FetchInfo(string link)
        {
            string info = "";
            WebClient cl = new WebClient();
            string code = cl.DownloadString(link);

            string[] codeLines = code.Split(new char[] { '\n' });


            for (int i = 0; i < codeLines.Length; i++)
            {
                if (codeLines[i].Contains("gameTitle"))
                {
                    //get game name
                    string gameTitle = "";
                    int index = codeLines[i].IndexOf(@"<div id=""gameTitle"">");
                    index += @"<div id=""gameTitle"">".Length;
                    for (int j = index; j < codeLines[i].Length; j++)
                    {
                        if (codeLines[i].Substring(j, 1) == ">")
                        {
                            j++;
                            while (codeLines[i][j] != '<')
                            {
                                gameTitle += codeLines[i][j];
                                j++;
                                if (j >= codeLines[i].Length)
                                    break;
                            }
                            break;
                        }
                    }
                    info += gameTitle.Replace("&#x27;", "'") + "\n";
                }
                if (codeLines[i].Contains("coreGameRelease") || codeLines[i].Contains("coreGameGenre"))
                {
                    string coreGameRelease = "";
                    string[] textlines = codeLines[i].Split(new char[] { '<', '>' });
                    for (int j = 0; j < textlines.Length; j++)
                    {
                        if (textlines[j].Length > 0)
                        {
                            if (textlines[j] != "br" && textlines[j] != "div")
                            {
                                if (!textlines[j].Contains("td width=") && !textlines[j].Contains("a href=")
                                    && !textlines[j].Contains("img alt=") && !textlines[j].Contains("div class=")
                                    && !textlines[j].Contains("div id=") && !textlines[j].Contains("div style="))
                                {
                                    if (textlines[j].Substring(0, 1) != "/")
                                    {
                                        if (!textlines[j].Contains(","))
                                            coreGameRelease += textlines[j].Replace("&nbsp;", "") + "\n";
                                        else
                                            coreGameRelease = coreGameRelease.Substring(0, coreGameRelease.Length - 1) + " " + textlines[j].Replace("&nbsp;", "") + "\n";
                                    }
                                }
                            }
                        }
                    }
                    info += coreGameRelease + "\n";
                }
                if (codeLines[i].Contains("Description"))
                {
                    string description = "";
                    string[] desc = codeLines[i].Split(new char[] { '<', '>' });
                    for (int j = 0; j < desc.Length; j++)
                    {
                        if (desc[j] == "edit description")
                            break;
                        if (desc[j].Length > 1)
                        {
                            if (desc[j] != "td" && desc[j] != "br" && desc[j].Substring(0, 1) != "/")
                            {
                                if (
                                   !desc[j].Contains("a href=")
                                && !desc[j].Contains("img alt=") && !desc[j].Contains("div class=")
                                && !desc[j].Contains("div id=") && !desc[j].Contains("div style=")
                                && !desc[j].Contains("h2 class="))
                                {
                                    description += desc[j] + "\n";
                                }
                            }
                        }
                    }
                    info += description;
                }
            }

            return info;
        }
        public static string FetchCoverLink(string link)
        {
            string imglink = "";
            WebClient cl = new WebClient();
            string code = cl.DownloadString(link);

            string[] codeLines = code.Split(new char[] { '\n' });


            for (int i = 0; i < codeLines.Length; i++)
            {
                if (codeLines[i].Contains("coreGameCover"))
                {
                   string[] textlines = codeLines[i].Split(new char[] { '<', '>' });
                   for (int j = 0; j < textlines.Length; j++)
                   {
                       if (textlines[j].Length > 0)
                       {
                           if (textlines[j].Contains("img alt="))
                           {
                               int index = textlines[j].IndexOf("src=");
                               index += 5;
                               for (int o = index; o < textlines[j].Length; o++)
                               {
                                   if (textlines[j][o].ToString() != @"""")
                                   {
                                       imglink += textlines[j][o];
                                   }
                                   else
                                       break;
                               }
                               break;
                           }
                       }
                   }
                }
            }

            return imglink;
        }
        /// <summary>
        /// Make a search request link for game
        /// </summary>
        /// <param name="gameName">The game name</param>
        /// <returns>The link</returns>
        public static string MakeSearchLink(string gameName)
        {
            return "http://www.mobygames.com/search/quick?ajax=1&sFilter=1&p=-1&sG=on&q=" + gameName + "&offset=0";
        }
    }
}
