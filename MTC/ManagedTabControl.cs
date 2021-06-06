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
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Design;
namespace MTC
{
    /// <summary>
    /// Manage Tab Control
    /// </summary>
    public partial class ManagedTabControl : UserControl
    {
        /// <summary>
        /// Manage Tab Control
        /// </summary>
        public ManagedTabControl()
        {
            InitializeComponent();
        }

        private bool closeTabOnCloseButtonClick = true;
        private bool showTabPageToolTip = true;

        #region Properties
        /// <summary>
        /// Get or set the Managed Tab Control Tab Page collection
        /// </summary>
        [Description("Tab pages collection"), Category("ManagedTabControl")]
        [Editor(typeof(MTCTabPagesEditor), typeof(UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MTCTabPagesCollection TabPages
        {
            get { return managedTabControlPanel1.TabPages; }
            set { value.OWNER = managedTabControlPanel1; managedTabControlPanel1.TabPages = value; }
        }
        /// <summary>
        /// Get or set the selected tab page index.
        /// </summary>
        [Browsable(false), Description("Selected tab page index"), Category("ManagedTabControl")]
        public int SelectedTabPageIndex
        {
            get { return managedTabControlPanel1.SelectedTabPageIndex; }
            set
            {
                managedTabControlPanel1.SelectedTabPageIndex = value;
                managedTabControlPanel1_SelectedTabPageIndexChanged(this, null);
                Invalidate();
            }
        }
        /// <summary>
        /// Get or set the tab page color that will be used when drawing a tab page normally (not selected)
        /// </summary>
        [Description("Tab page color. The color that will be used when drawing a tab page normally (not selected)"), Category("ManagedTabControl")]
        public Color TabPageColor
        { get { return managedTabControlPanel1.TabPageColor; } set { managedTabControlPanel1.TabPageColor = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set the color that will be used when drawing a tab page that is selected
        /// </summary>
        [Description("Tab page selected color. The color that will be used when drawing a tab page that is selected"), Category("ManagedTabControl")]
        public Color TabPageSelectedColor
        { get { return managedTabControlPanel1.TabPageSelectedColor; } set { managedTabControlPanel1.TabPageSelectedColor = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set the color that will be used when the mouse cursor get over a tab page
        /// </summary>
        [Description("Tab page selected color. The color that will be used when the mouse cursor get over a tab page"), Category("ManagedTabControl")]
        public Color TabPageHighlightedColor
        { get { return managedTabControlPanel1.TabPageHighlightedColor; } set { managedTabControlPanel1.TabPageHighlightedColor = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set the color of the tab page splinters.
        /// </summary>
        [Description("The color of the tab page splitters."), Category("ManagedTabControl")]
        public Color TabPageSplitColor
        { get { return managedTabControlPanel1.TabPageSplitColor; } set { managedTabControlPanel1.TabPageSplitColor = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set a value indicates if the close button should be visible on each tab page
        /// </summary>
        [Description("Indicate if the close button should be visible on each tab page"), Category("ManagedTabControl")]
        public bool CloseBoxOnEachPageVisible
        { get { return managedTabControlPanel1.DrawCloseBoxOnEachPage; } set { managedTabControlPanel1.DrawCloseBoxOnEachPage = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set if the close button (if enabled via CloseBoxOnEachPageVisible) should be always visible on tab page with black color when the mouse cursor is not over that tab page.
        /// </summary>
        [Description("Indicate if the close button (if enabled via CloseBoxOnEachPageVisible) should be always visible on tab page with black color when the mouse cursor is not over that tab page."), Category("ManagedTabControl")]
        public bool CloseBoxAlwaysVisible
        {
            get { return managedTabControlPanel1.CloseBoxAlwaysVisible; }
            set { managedTabControlPanel1.CloseBoxAlwaysVisible = value; managedTabControlPanel1.Invalidate(); }
        }
        /// <summary>
        /// Get or set a value indicates if tab page can get highlighted when the mouse cursor get over it
        /// </summary>
        [Description("Indicate if tab page can get highlighted when the mouse cursor get over it"), Category("ManagedTabControl")]
        public bool DrawTabPageHighlight
        { get { return managedTabControlPanel1.DrawTabPageHighlight; } set { managedTabControlPanel1.DrawTabPageHighlight = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set if the tab pages can be reordered
        /// </summary>
        [Description("Indicate if tab page can be reordered"), Category("ManagedTabControl")]
        public bool AllowTabPagesReorder
        { get { return managedTabControlPanel1.AllowTabPagesReorder; } set { managedTabControlPanel1.AllowTabPagesReorder = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set the max width of a tab page that can get (range 50-500, default=250).
        /// </summary>
        [Description("The max width of a tab page that can get (range 50-500, default=250). This value is important when a tab page has large text so the tab page limit the width of itself to this value when caluclating width depending on string value."), Category("ManagedTabControl")]
        public int TabPageMaxWidth
        {
            get { return managedTabControlPanel1.TabPageMaxWidth; }
            set
            {
                if (value < 50)
                    value = 50;
                if (value > 500)
                    value = 500;
                managedTabControlPanel1.TabPageMaxWidth = value;
                managedTabControlPanel1.Invalidate();
            }
        }
        /// <summary>
        /// Get or set a value Indicate whether the control can perform tab page drag automatically with (Move) effect. Otherwise the event 'TabPageDrag' will raise instead and you should do drag manully.
        /// </summary>
        [Description("Indicate whether the control can perform tab page drag automatically with (Move) effect. Otherwise the event 'TabPageDrag' will raise instead and you should do drag manually."), Category("ManagedTabControl")]
        public bool AllowAutoTabPageDragAndDrop
        { get { return managedTabControlPanel1.AllowAutoTabPageDragAndDrop; } set { managedTabControlPanel1.AllowAutoTabPageDragAndDrop = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set a value Indicate whether the control can perform tab page drag.
        /// </summary>
        [Description("Indicate whether the control can perform tab page drag. If false, the 'AllowAutoTabPageDragAndDrop' get disable."), Category("ManagedTabControl")]
        public bool AllowTabPageDragAndDrop
        { get { return managedTabControlPanel1.AllowTabPageDragAndDrop; } set { managedTabControlPanel1.AllowTabPageDragAndDrop = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set if a tab page should close automatically when the user click on the close button of that tab page.
        /// </summary>
        [Description("Indicate if a tab page should close automatically when the user click on the close button of that tab page."), Category("ManagedTabControl")]
        public bool CloseTabOnCloseButtonClick
        { get { return closeTabOnCloseButtonClick; } set { closeTabOnCloseButtonClick = value; } }
        /// <summary>
        /// Get or set if the control can show a tool tip of a tab page that the text of it is not completely visible due to long char length of that text.
        /// </summary>
        [Description("Indicate if the control can show a tool tip of a tab page that the text of it is not completely visible due to large string length of that text."), Category("ManagedTabControl")]
        public bool ShowTabPageToolTip
        { get { return showTabPageToolTip; } set { showTabPageToolTip = value; } }
        /// <summary>
        /// Get or set if the control should show a tool tip of a tab page when get highlighted otherwise the tool tip get shown only when the text of that tab page is too long. ShowTabPageToolTip must be enabled
        /// </summary>
        [Description("Indicate if the control should show a tool tip of a tab page when get highlighted otherwise the tool tip get shown only when the text of that tab page is too long. ShowTabPageToolTip must be enabled."), Category("ManagedTabControl")]
        public bool ShowTabPageToolTipAlways
        { get { return managedTabControlPanel1.AlwaysShowToolTip; } set { managedTabControlPanel1.AlwaysShowToolTip = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// The images list that will be used for draw
        /// </summary>
        [Description("The images list that will be used for draw"), Category("ManagedTabControl")]
        public ImageList ImagesList
        { get { return managedTabControlPanel1.ImagesList; } set { managedTabControlPanel1.ImagesList = value; } }
        /// <summary>
        /// Get or set the font
        /// </summary>
        [Description("The font of texts of the tab pages."), Category("ManagedTabControl")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                managedTabControlPanel1.Font = value;
                managedTabControlPanel1.Invalidate();
            }
        }
        /// <summary>
        /// Get or set the draw style that should be used when drawing tab pages normally (when tab page is not selected or highlighted)
        /// </summary>
        [Description("The draw style that should be used when drawing tab pages normally (when tab page is not selected or highlighted)"), Category("ManagedTabControl")]
        public MTCDrawStyle DrawStyle
        { get { return managedTabControlPanel1.drawStyle; } set { managedTabControlPanel1.drawStyle = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// If enabled, the control will select added tab page automatically after adding it
        /// </summary>
        [Description("Indicate if the control should select added tab page automatically after adding it."), Category("ManagedTabControl")]
        public bool AutoSelectAddedTabPageAfterAddingIt
        { get { return managedTabControlPanel1.AutoSelectAddedTabPage; } set { managedTabControlPanel1.AutoSelectAddedTabPage = value; managedTabControlPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set the color of texts of the tab pages.
        /// </summary>
        [Description("The color of texts of the tab pages."), Category("ManagedTabControl")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                managedTabControlPanel1.ForeColor = value;
                managedTabControlPanel1.Invalidate();
            }
        }
        /// <summary>
        /// Get or set the background color
        /// </summary>
        [Description("The background color of the control."), Category("ManagedTabControl")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                panel_controls.BackColor = managedTabControlPanel1.BackColor = value;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Raised when the SelectedTabPageIndex value changed.
        /// </summary>
        [Description("Raised when the SelectedTabPageIndex value changed."), Category("ManagedTabControl")]
        public event EventHandler SelectedTabPageIndexChanged;
        /// <summary>
        /// Raised when the user click the close button of a tab page. The Args of this event gives the option to cancel close.
        /// </summary>
        [Description("Raised when the user click the close button of a tab page. The Args of this event gives the option to cancel."), Category("ManagedTabControl")]
        public event EventHandler<MTCTabPageCloseArgs> TabPageClose;
        /// <summary>
        /// Raised when the control need to show tool tip for tab page.
        /// </summary>
        [Description("Raised when the control need to show tool tip for tab page."), Category("ManagedTabControl")]
        public event EventHandler<MTCTabPageShowToolTipArgs> ShowTabPageToolTipRequest;
        /// <summary>
        /// Raised after a tab page get reordered.
        /// </summary>
        [Description("Raised after a tab page get reordered."), Category("ManagedTabControl")]
        public event EventHandler AfterTabPageReorder;
        /// <summary>
        /// Raised when tab page get dragged. The AllowTabPageDragAndDrop must enabled and AllowAutoTabPageDragAndDrop must be disabled.
        /// </summary>
        [Description("Raised when tab page get dragged. The AllowTabPageDragAndDrop must enabled and AllowAutoTabPageDragAndDrop must be disabled in order to enable this event."), Category("Drag Drop")]
        public event EventHandler<MTCTabPageDragArgs> TabPageDrag;
        /// <summary>
        /// Raised before a tab page get dragged using auto drag and drop. The AllowTabPageDragAndDrop and AllowAutoTabPageDragAndDrop must enabled.
        /// </summary>
        [Description("Raised before a tab page get dragged using auto drag and drop. The AllowTabPageDragAndDrop and AllowAutoTabPageDragAndDrop must enabled."), Category("Drag Drop")]
        public event EventHandler BeforeAutoTabDragAndDrop;
        /// <summary>
        /// Raised after a tab page dragged using auto drag and drop. The AllowTabPageDragAndDrop and AllowAutoTabPageDragAndDrop must enabled.
        /// </summary>
        [Description("Raised after a tab page dragged using auto drag and drop. The AllowTabPageDragAndDrop and AllowAutoTabPageDragAndDrop must enabled."), Category("Drag Drop")]
        public event EventHandler AfterAutoTabDragAndDrop;
        /// <summary>
        /// Raised when tab pages collection get cleared.
        /// </summary>
        [Description("Raised when tab pages collection get cleared."), Category("ManagedTabControl")]
        public event EventHandler TabPagesCleared;
        #endregion

        #region Methods
        /// <summary>
        /// Calculate a Tab Page width value.
        /// </summary>
        /// <param name="page">The <see cref="MTC.IMTCTabPage"/> to calculate value for</param>
        /// <returns>The width of that page otherwise 0 if unable to calculate</returns>
        public int CalculateTabPageWidth(MTCTabPage page)
        {
            return managedTabControlPanel1.CalculateTabPageWidth(page);
        }
        /// <summary>
        /// Get tab page index within TabPages collection
        /// </summary>
        /// <param name="xPos">The x coordinate value on view port</param>
        /// <returns>The tab page index value otherwise -1 if page not found</returns>
        public int GetTabPageIndex(int xPos)
        { return managedTabControlPanel1.GetTabPageIndex(xPos); }
        /// <summary>
        /// Get tab page x coordinate value on view port
        /// </summary>
        /// <param name="index">The tab page index within TabPages collection</param>
        /// <returns>The tab page x coordinate value on view port otherwise -1 if page not found</returns>
        public int GetTabPageXPos(int index)
        { return managedTabControlPanel1.GetTabPageXPos(index); }
        /// <summary>
        /// Indicate whether a tab is completely shown to the user
        /// </summary>
        /// <param name="index">The tab page index</param>
        /// <returns></returns>
        public bool IsTapPageShownCompletely(int index)
        { return managedTabControlPanel1.IsTapPageShownCompletely(index); }
        #endregion

        /// <summary>
        /// Occur when the control get invalidated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            managedTabControlPanel1.Invalidate();
        }
        private void managedTabControlPanel1_TabPageCloseRequest(object sender, MTCTabPageCloseArgs e)
        {
            MTCTabPageCloseArgs args = new MTCTabPageCloseArgs(e.TabPageID, e.TabPageIndex);
            args.Cancel = false;
            if (TabPageClose != null)
                TabPageClose(this, args);
            if (!args.Cancel)
            {
                if (closeTabOnCloseButtonClick)
                {
                    managedTabControlPanel1.TabPages.RemoveAt(e.TabPageIndex);
                    if (managedTabControlPanel1.SelectedTabPageIndex > 0)
                        managedTabControlPanel1.SelectedTabPageIndex--;
                    managedTabControlPanel1_SelectedTabPageIndexChanged(this, new EventArgs());
                    managedTabControlPanel1.Invalidate();
                }
            }
        }
        private void managedTabControlPanel1_RefreshScrollBar(object sender, EventArgs e)
        {
            int size = managedTabControlPanel1.CalculateTabPagesWidth();
            if (size < this.Width)
            {
                hScrollBar1.Visible = false;
                managedTabControlPanel1.HScrollOffset = 0;
            }
            else
            {
                hScrollBar1.Maximum = size - this.Width + 20 + hScrollBar1.Width;
                hScrollBar1.Visible = true;
            }
        }
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            managedTabControlPanel1.HScrollOffset = hScrollBar1.Value;
            managedTabControlPanel1.Invalidate();
        }
        private void ManagedTabControl_Resize(object sender, EventArgs e)
        {
            managedTabControlPanel1.HScrollOffset = 0;
            managedTabControlPanel1.Invalidate();
            managedTabControlPanel1_RefreshScrollBar(this, null);
        }
        private void managedTabControlPanel1_SelectedTabPageIndexChanged(object sender, EventArgs e)
        {
            panel_controls.Controls.Clear();
            if (managedTabControlPanel1.SelectedTabPageIndex >= 0)
            {
                if (managedTabControlPanel1.TabPages.Count > 0)
                {
                    if (SelectedTabPageIndexChanged != null)
                        SelectedTabPageIndexChanged(this, e);
                    Panel control = managedTabControlPanel1.TabPages[managedTabControlPanel1.SelectedTabPageIndex].Panel;
                    if (control != null)
                    {
                        panel_controls.Controls.Add(control);
                        control.Dock = DockStyle.Fill;
                    }
                }
            }
        }
        private void managedTabControlPanel1_ShowTabPageToolTipRequest(object sender, MTCTabPageShowToolTipArgs e)
        {
            if (showTabPageToolTip)
            {
                MTCTabPageShowToolTipArgs args = new MTCTabPageShowToolTipArgs(e.TabPageID, e.TabPageIndex, e.ToolTipText);
                if (ShowTabPageToolTipRequest != null)
                    ShowTabPageToolTipRequest(this, args);
                toolTip1.SetToolTip(managedTabControlPanel1, args.ToolTipText);
            }
        }
        private void managedTabControlPanel1_ClearToolTip(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
        }
        private void managedTabControlPanel1_ScrollToSelectedTabPage(object sender, EventArgs e)
        {
            int x = managedTabControlPanel1.GetTabPageXPos(managedTabControlPanel1.SelectedTabPageIndex);
            int w = managedTabControlPanel1.CalculateTabPageWidth(managedTabControlPanel1.TabPages[managedTabControlPanel1.SelectedTabPageIndex]);
            int scr = hScrollBar1.Value;
            if (x > 0)
            {
                while (managedTabControlPanel1.Width - x < w)
                {
                    scr++;
                    if (scr < hScrollBar1.Maximum)
                    {
                        hScrollBar1.Value = scr;
                        hScrollBar1_Scroll(this, null);
                        x = managedTabControlPanel1.GetTabPageXPos(managedTabControlPanel1.SelectedTabPageIndex);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                while (x < 0)
                {
                    scr--;
                    if (scr > 0)
                    {
                        hScrollBar1.Value = scr;
                        hScrollBar1_Scroll(this, null);
                        x = managedTabControlPanel1.GetTabPageXPos(managedTabControlPanel1.SelectedTabPageIndex);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        private void managedTabControlPanel1_AfterTabPageReorder(object sender, EventArgs e)
        {
            if (AfterTabPageReorder != null)
                AfterTabPageReorder(this, new EventArgs());
        }
        private void managedTabControlPanel1_TabPageDrag_1(object sender, MTCTabPageDragArgs e)
        {
            if (TabPageDrag != null)
                TabPageDrag(this, e);
        }
        private void managedTabControlPanel1_AfterAutoDragAndDrop(object sender, EventArgs e)
        {
            if (AfterAutoTabDragAndDrop != null)
                AfterAutoTabDragAndDrop(this, new EventArgs());

        }
        private void managedTabControlPanel1_BeforeAutoDragAndDrop(object sender, EventArgs e)
        {
            if (BeforeAutoTabDragAndDrop != null)
                BeforeAutoTabDragAndDrop(this, new EventArgs());
        }
        private void managedTabControlPanel1_TabPagesCleared(object sender, EventArgs e)
        {
            if (TabPagesCleared != null)
                TabPagesCleared(this, new EventArgs());
        }
        private void panel_controls_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            OnGiveFeedback(e);
        }
        private void managedTabControlPanel1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            OnGiveFeedback(e);
        }
    }
}
