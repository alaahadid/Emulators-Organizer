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

namespace EmulatorsOrganizer.Core
{
    /*Information container master class...all information container items must inherit this !*/
    [Serializable]
    public abstract class InformationContainer
    {
        public InformationContainer(string id)
        {
            this.id = id;
            LoadAttrs();
        }
        protected string id;
        protected string name;
        protected string displayName;
        protected bool columnable;
        protected bool dectable;

        private void LoadAttrs()
        {
            foreach (Attribute attr in Attribute.GetCustomAttributes(this.GetType()))
            {
                if (attr.GetType() == typeof(InformationContainerDescription))
                {
                    InformationContainerDescription a = ((InformationContainerDescription)attr);
                    this.name = a.Name;
                    this.columnable = a.Columnable;
                    this.dectable = a.Dectable;
                    break;
                }
            }
        }

        // Properties
        /// <summary>
        /// Get or set the id of this container
        /// </summary>
        public string ID
        { get { return id; } set { id = value; } }
        /// <summary>
        /// Get or set the name of this container
        /// </summary>
        public virtual string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the 'visible to user' name of this container
        /// </summary>
        public virtual string DisplayName
        { get { return displayName; } set { displayName = value; } }
        /// <summary>
        /// Get if this information container can be showed as column in roms list
        /// </summary>
        public virtual bool Columnable
        { get { return columnable; } }
        /// <summary>
        /// Indicates if this information container can be detect
        /// </summary>
        public virtual bool Dectable
        { get { return dectable; } }

        public override string ToString()
        {
            return displayName;
        }
    }
}
