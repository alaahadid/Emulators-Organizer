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

namespace EmulatorsOrganizer.Core
{
    [Serializable]
    public class SearchRequestArgs : EventArgs
    {
        /// <summary>
        /// The search request args
        /// </summary>
        public SearchRequestArgs()
        { }
        /// <summary>
        /// Search args
        /// </summary>
        /// <param name="searchWhat"></param>
        /// <param name="mode"></param>
        /// <param name="conditionForText"></param>
        /// <param name="conditionForNumber"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="matchWord"></param>
        public SearchRequestArgs(string searchWhat, SearchMode mode, TextSearchCondition conditionForText,
            NumberSearchCondition conditionForNumber, bool caseSensitive)
        {
            this.isDataItem = false;
            this.searchWhat = searchWhat;
            this.dataItemName = "";
            this.mode = mode;
            this.conditionForText = conditionForText;
            this.conditionForNumber = conditionForNumber;
            this.caseSensitive = caseSensitive;
        }

        /// <summary>
        /// Search args for data item.
        /// </summary>
        /// <param name="searchWhat"></param>
        /// <param name="dataItemName"></param>
        /// <param name="mode"></param>
        /// <param name="conditionForText"></param>
        /// <param name="conditionForNumber"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="matchWord"></param>
        public SearchRequestArgs(string searchWhat, string dataItemName, TextSearchCondition conditionForText,
            NumberSearchCondition conditionForNumber, bool caseSensitive)
        {
            this.isDataItem = true;
            this.searchWhat = searchWhat;
            this.dataItemName = dataItemName;
            this.conditionForText = conditionForText;
            this.conditionForNumber = conditionForNumber;
            this.caseSensitive = caseSensitive;
        }
        private string searchWhat;
        private string dataItemName;
        private SearchMode mode;
        private bool isDataItem;
        private TextSearchCondition conditionForText;
        private NumberSearchCondition conditionForNumber;
        private bool caseSensitive;

        /// <summary>
        /// The search word
        /// </summary>
        public string SearchWhat
        { get { return searchWhat; } }
        /// <summary>
        /// When IsDataItem is true, get the data item name to search
        /// </summary>
        public string DataItemName
        { get { return dataItemName; } }
        /// <summary>
        /// If true, the search mode is for a data item.
        /// </summary>
        public bool IsDataItem
        { get { return isDataItem; } }
        /// <summary>
        /// The search mode
        /// </summary>
        public SearchMode SearchMode
        { get { return mode; } }
        /// <summary>
        /// The condition if the search is for text
        /// </summary>
        public TextSearchCondition ConditionForText
        { get { return conditionForText; } }
        /// <summary>
        /// The condition if the search is for number
        /// </summary>
        public NumberSearchCondition ConditionForNumber
        { get { return conditionForNumber; } }
        /// <summary>
        /// Indicates case sensitive for text search
        /// </summary>
        public bool CaseSensitive
        { get { return caseSensitive; } }
        /// <summary>
        /// Create an exact clone of this args object
        /// </summary>
        /// <returns></returns>
        public SearchRequestArgs Clone()
        {
            SearchRequestArgs newArgs = new SearchRequestArgs();
            newArgs.caseSensitive = this.caseSensitive;
            newArgs.conditionForNumber = this.conditionForNumber;
            newArgs.conditionForText = this.conditionForText;
            newArgs.dataItemName = this.dataItemName;
            newArgs.isDataItem = this.isDataItem;
            newArgs.mode = this.mode;
            newArgs.searchWhat = this.searchWhat;
            return newArgs;
        }
    }
}
