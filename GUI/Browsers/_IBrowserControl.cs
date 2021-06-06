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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
namespace EmulatorsOrganizer.GUI
{
    public class IBrowserControl : UserControl
    {
        protected ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        protected LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        protected SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");

        public event EventHandler ProgressStarted;
        public event EventHandler ProgressFinished;
        public event EventHandler<ProgressArg> Progress;

        /*Properties*/
        public virtual bool CanDelete
        { get { return false; } }
        public virtual bool CanRename
        { get { return false; } }
        public virtual bool CanShowProperties
        { get { return false; } }
        public virtual bool CanChangeIcon
        { get { return false; } }

        /*Methods*/
        public virtual void ApplyStyle(EOStyle style)
        { }
        public virtual void InitializeEvents()
        {
            profileManager.CreatingNewProfile += profileManager_CreatingNewProfile;
            profileManager.OpeningProfile += profileManager_OpeningProfile;
            profileManager.NewProfileCreated += profileManager_NewProfileCreated;
            profileManager.ProfileOpened += profileManager_ProfileOpened;

            profileManager.ProfileSavingStarted += profileManager_ProfileSavingStarted;
            profileManager.ProfileSavingFinished += profileManager_ProfileSavingFinished;
        }
        public virtual void DisposeEvents()
        {
        }
        public virtual void DeleteSelected()
        { }
        public virtual void RenameSelected()
        { }
        public virtual void ChangeIcon()
        { }
        public virtual void ClearIcon()
        { }
        public virtual void ShowItemProperties()
        { }
        public virtual void LoadControlSettings()
        {
        }

        /*Events*/
        /// <summary>
        /// Raised to allow parent window to check fro buttons enables
        /// </summary>
        public event EventHandler EnableDisableButtons;

        /*Events handle*/
        private void profileManager_NewProfileCreated(object sender, EventArgs e)
        {
            OnNewProfileCreated();
        }
        private void profileManager_ProfileOpened(object sender, EventArgs e)
        {
            OnProfileOpened();
        }
        private void profileManager_OpeningProfile(object sender, EventArgs e)
        {
            OnOpeningProfile();
        }
        private void profileManager_CreatingNewProfile(object sender, EventArgs e)
        {
            OnCreatingNewProfile();
        }
        private void profileManager_ProfileSavingFinished(object sender, EventArgs e)
        {
            OnProfileSavingFinished();
        }
        private void profileManager_ProfileSavingStarted(object sender, EventArgs e)
        {
            OnProfileSavingStarted();
        }

        /// <summary>
        /// Called when a new profile created. This method calls InitializeEvents() by default.
        /// </summary>
        protected virtual void OnNewProfileCreated()
        { InitializeEvents(); }
        /// <summary>
        /// Called before creating a new profile. This method calls DisposeEvents() by default.
        /// </summary>
        protected virtual void OnCreatingNewProfile()
        { DisposeEvents(); }
        /// <summary>
        /// Called when a profile opened. This method calls InitializeEvents() by default.
        /// </summary>
        protected virtual void OnProfileOpened()
        { InitializeEvents(); }
        /// <summary>
        /// Called before opening a profile. This method calls DisposeEvents() by default.
        /// </summary>
        protected virtual void OnOpeningProfile()
        { DisposeEvents(); }
        /// <summary>
        /// Called when profile starting to save.
        /// </summary>
        protected virtual void OnProfileSavingStarted()
        {
        }
        /// <summary>
        /// Called when profile finished saving.
        /// </summary>
        protected virtual void OnProfileSavingFinished()
        {
        }
        /// <summary>
        /// Raises the EnableDisableButtons event
        /// </summary>
        protected virtual void OnEnableDisableButtons()
        {
            if (EnableDisableButtons != null)
                EnableDisableButtons(this, new EventArgs());
        }
        protected virtual void OnProgressStarted()
        {
            if (ProgressStarted != null)
                ProgressStarted(this, new EventArgs());
        }
        protected virtual void OnProgressFinished()
        {
            if (ProgressFinished != null)
                ProgressFinished(this, new EventArgs());
        }
        protected virtual void OnProgress(string status, int completed)
        {
            if (Progress != null)
                Progress(this, new ProgressArg(status, completed));
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IBrowserControl));
            this.SuspendLayout();
            // 
            // IBrowserControl
            // 
            this.Name = "IBrowserControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }
    }
}
