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
using EmulatorsOrganizer.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace EmulatorsOrganizer.Core
{
    /// <summary>
    /// Represents Emulators Organizer element.
    /// </summary>
    [Serializable()]
    public abstract class IEOElement
    {
        /// <summary>
        /// Represents Emulators Organizer element.
        /// </summary>
        /// <param name="name">The element name</param>
        /// <param name="id">The element id</param>
        public IEOElement(string name, string id)
        {
            this.name = name;
            this.id = id;
        }

        protected string name;
        protected string id;

        protected Image icon;
        protected Image iconThumbnail;
        [NonSerialized]
        protected LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        protected EOStyle style = new EOStyle();
        protected List<ColumnItem> columns = new List<ColumnItem>();
        protected List<Filter> filters = new List<Filter>();
        protected bool enableEmulator;
        protected bool enableCommandlines;
        [NonSerialized]
        private object tag;

        protected string[,] defaultColumns
        {
            get
            {
                if (ls == null)
                    ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
                return new string[,]  {
          { ls["Column_ParentConsole"], "console" } ,
          { ls["Column_Name"],          "name" } ,
          { ls["Column_Size"],          "size" } ,
          { ls["Column_FileType"],      "file type" } ,
          { ls["Column_LastPlayed"],    "last played" } ,
          { ls["Column_PlayedTimes"],   "played times" } ,
          { ls["Column_PlayTime"],      "play time" } ,
          { ls["Column_Rating"],        "rating" } ,
          { ls["Column_Categories"],    "categories" } ,
          { ls["Column_Path"],          "path" }
                                      };
            }
        }

        /*Properties*/
        /// <summary>
        /// Get or set the name of this element
        /// </summary>
        public virtual string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the id of this element
        /// </summary>
        public virtual string ID
        { get { return id; } set { id = value; } }
        /// <summary>
        /// Get or set the icon of this element
        /// </summary>
        public virtual Image Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                if (value != null)
                    iconThumbnail = value.GetThumbnailImage(16, 16, null, IntPtr.Zero);
                else
                    iconThumbnail = null;
            }
        }
        /// <summary>
        /// Get or set the icon thumbnail that should be used in roms browser. This will make render faster, get set when Icon property set.
        /// </summary>
        public virtual Image IconThumbnail
        { get { return iconThumbnail; } set { iconThumbnail = value; } }
        /// <summary>
        /// Get or set the style object
        /// </summary>
        public virtual EOStyle Style
        { get { return style; } set { style = value; } }
        /// <summary>
        /// Get or set the columns collection
        /// </summary>
        public virtual List<ColumnItem> Columns
        { get { return columns; } set { columns = value; } }
        /// <summary>
        /// Get or set the emulator usage
        /// </summary>
        public virtual bool EnableEmulator
        { get { return enableEmulator; } set { enableEmulator = value; } }
        /// <summary>
        /// Get or set the commandlines usage
        /// </summary>
        public virtual bool EnableCommandlines
        { get { return enableCommandlines; } set { enableCommandlines = value; } }
        /// <summary>
        /// Get or set the filters for this element.
        /// </summary>
        public virtual List<Filter> Filters
        { get { return filters; } set { filters = value; } }

        /*Methods*/
        /// <summary>
        /// Get if given column id is a default column
        /// </summary>
        /// <param name="id">The column id</param>
        /// <returns>True if this column is default otherwise false</returns>
        public virtual bool IsDefaultColumn(string id)
        {
            for (int i = 0; i < defaultColumns.Length / 2; i++)
            {
                if (defaultColumns[i, 1] == id)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Build default columns
        /// </summary>
        /// <param name="columnIdsToExclude">List the columns you don't want them to appear for this element</param>
        public virtual bool IsFilterExist(string name)
        {
            foreach (Filter ff in filters)
            {
                if (ff.Name == name)
                    return true;
            }
            return false;
        }
        public virtual void BuildDefaultColumns(string[] columnIdsToExclude)
        {
            columns = new List<ColumnItem>();
            for (int i = 0; i < defaultColumns.Length / 2; i++)
            {
                ColumnItem item = new ColumnItem();
                item.ColumnName = defaultColumns[i, 0];
                item.ColumnID = defaultColumns[i, 1];
                item.Width = 60;
                if (columnIdsToExclude != null)
                    item.Visible = !columnIdsToExclude.Contains(defaultColumns[i, 1]);
                else
                    item.Visible = true;
                columns.Add(item);
            }
        }
        /// <summary>
        /// Update column name
        /// </summary>
        /// <param name="id">The column id</param>
        /// <param name="name">The column new name</param>
        public virtual void UpdateColumnName(string id, string name)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].ColumnID == id)
                {
                    columns[i].ColumnName = name;
                    break;
                }
            }
        }
        /// <summary>
        /// Get the name of this element
        /// </summary>
        /// <returns>Name of this element</returns>
        public override string ToString()
        {
            return name;
        }
        /// <summary>
        /// Get or set the tag
        /// </summary>
        public object Tag
        { get { return tag; } set { tag = value; } }
    }
}
