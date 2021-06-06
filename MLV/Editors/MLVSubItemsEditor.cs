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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace MLV
{
    /// <summary>
    /// Sub items collection editor
    /// </summary>
    public class MLVSubItemsEditor : CollectionEditor
    {
        /// <summary>
        /// Sub items collection editor
        /// </summary>
        /// <param name="type"></param>
        public MLVSubItemsEditor(Type type)
            : base(type)
        {
        }
        /// <summary>
        /// Get Display Text
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetDisplayText(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(base.CollectionType);
            string text;
            if (defaultProperty != null && defaultProperty.PropertyType == typeof(string))
            {
                text = (string)defaultProperty.GetValue(value);
                if (text != null && text.Length > 0)
                {
                    return text;
                }
            }
            text = TypeDescriptor.GetConverter(value).ConvertToString(value);
            if (text == null || text.Length == 0)
            {
                text = value.GetType().Name;
            }
            return text;
        }
        /// <summary>
        /// Set Items
        /// </summary>
        /// <param name="editValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object SetItems(object editValue, object[] value)
        {
            List<ManagedListViewSubItem> original = editValue as List<ManagedListViewSubItem>;
            if (original == null)
            {
                editValue = new List<ManagedListViewSubItem>();
            }
            original.Clear();
            foreach (ManagedListViewSubItem entry in value)
            {
                original.Add(entry.Clone());
            }
            return original;
        }
        /// <summary>
        /// Create Instance
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        protected override object CreateInstance(Type itemType)
        {
            if (itemType == typeof(ManagedListViewSubItem))
            { return new ManagedListViewSubItem(); }
            return base.CreateInstance(itemType);
        }
    }
}
