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
using System.Windows.Forms;
using System.Reflection;
namespace MLV
{
    /// <summary>
    /// Advanced ListView control.
    /// </summary>
    public partial class ManagedListView : UserControl
    {
        /// <summary>
        /// Advanced ListView control.
        /// </summary>
        public ManagedListView()
        {
            InitializeComponent();
            wheelScrollSpeed = ManagedListViewPanel1.GetVisibleItemsHeight();
            autoSetWheelScrollSpeed = true;
            ManagedListViewPanel1.columns = new ManagedListViewColumnsCollection(this);
            ManagedListViewPanel1.items = new ManagedListViewItemsCollection(this);
        }

        private ManagedListViewViewMode viewMode = ManagedListViewViewMode.Details;
        private int wheelScrollSpeed = 20;
        /// <summary>
        /// Indicates if should use the auto wheel speed set
        /// </summary>
        protected bool autoSetWheelScrollSpeed;
        private bool showSubToolTip = true;
        private string currentToolTip;
        private int toolTipX = 0;
        private int toolTipY = 0;
        private int currentItemIndexOfToolTip;
        private string currentColumnIDOfToolTip;
        private int tooltip_timer;
        private int tooltip_timer_reload = 5;
        private bool activeToolTip;

        #region properties
        /// <summary>
        /// Get or set the viewmode.
        /// </summary>
        [Description("The list view mode"), Category("ManagedListView")]
        public ManagedListViewViewMode ViewMode
        {
            get { return viewMode; }
            set
            {
                viewMode = value;
                ManagedListViewPanel1.viewMode = value;
                ManagedListViewPanel1.HscrollOffset = hScrollBar1.Value = 0;
                ManagedListViewPanel1.VscrollOffset = vScrollBar1.Value = 0;
                ManagedListViewPanel1_RefreshScrollBars(this, null);
                ManagedListViewPanel1.Invalidate();

                if (ViewModeChanged != null)
                    ViewModeChanged(this, new EventArgs());

                if (value == ManagedListViewViewMode.Thumbnails)
                    if (SwitchToNormalContextMenu != null)
                        SwitchToNormalContextMenu(this, new EventArgs());
            }
        }
        /// <summary>
        /// Get or set the column collection.
        /// </summary>
        [Description("The columns collection"), Category("ManagedListView")]
        [Editor(typeof(MLVColumnsEditor), typeof(UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ManagedListViewColumnsCollection Columns
        {
            get { return ManagedListViewPanel1.columns; }
            set { ManagedListViewPanel1.columns = value; ManagedListViewPanel1.Invalidate(); }
        }
        /// <summary>
        /// Get or set the items collection.
        /// </summary>
        [Description("The items collection"), Category("ManagedListView")]
        [Editor(typeof(MLVItemsEditor), typeof(UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ManagedListViewItemsCollection Items
        {
            get { return ManagedListViewPanel1.items; }
            set { ManagedListViewPanel1.items = value; ManagedListViewPanel1.Invalidate(); }
        }
        /// <summary>
        /// Get or set if selected items can be draged and droped
        /// </summary>
        [Description("If enabled, the selected items can be dragged and dropped"), Category("ManagedListView")]
        public bool AllowItemsDragAndDrop
        { get { return ManagedListViewPanel1.AllowItemsDragAndDrop; } set { ManagedListViewPanel1.AllowItemsDragAndDrop = value; } }
        /// <summary>
        /// Allow columns reorder ? after a column reordered, the index of that column within the columns collection get changed
        /// </summary>
        [Description("Allow columns reorder ? after a column reordered, the index of that column within the columns collection get changed"), Category("ManagedListView")]
        public bool AllowColumnsReorder
        { get { return ManagedListViewPanel1.AllowColumnsReorder; } set { ManagedListViewPanel1.AllowColumnsReorder = value; } }
        /// <summary>
        /// If enabled, the sort mode of a column get changed when the user clicks that column
        /// </summary>
        [Description("If enabled, the sort mode of a column get changed when the user clicks that column"), Category("ManagedListView")]
        public bool ChangeColumnSortModeWhenClick
        { get { return ManagedListViewPanel1.ChangeColumnSortModeWhenClick; } set { ManagedListViewPanel1.ChangeColumnSortModeWhenClick = value; } }
        /// <summary>
        /// The thunmbnail height. Work only for Thumbnails view mode.
        /// </summary>
        [Description("The thumbnail height. Work only for thumbnails view mode."), Category("ManagedListView")]
        public int ThunmbnailsHeight
        { get { return ManagedListViewPanel1.ThumbnailsHeight; } set { ManagedListViewPanel1.ThumbnailsHeight = value; ManagedListViewPanel1.Invalidate(); } }
        /// <summary>
        /// The thunmbnail width. Work only for thumbnails view mode.
        /// </summary>
        [Description("The thumbnail width. Work only for thumbnails view mode."), Category("ManagedListView")]
        public int ThunmbnailsWidth
        { get { return ManagedListViewPanel1.ThumbnailsWidth; } set { ManagedListViewPanel1.ThumbnailsWidth = value; ManagedListViewPanel1.Invalidate(); } }
        /// <summary>
        /// The speed of the scroll when using mouse wheel. Default value is 20.
        /// </summary>
        [Description("The speed of the scroll when using mouse wheel. Default value is 20."), Category("ManagedListView")]
        public int WheelScrollSpeed
        { get { return wheelScrollSpeed; } set { wheelScrollSpeed = value; } }
        /// <summary>
        /// Auto set the WheelScrollSpeed property depending on items collection change
        /// </summary>
        [Description("Auto set the WheelScrollSpeed property depending on items collection change"), Category("ManagedListView")]
        public bool AutoSetWheelScrollSpeed
        { get { return autoSetWheelScrollSpeed; } set { autoSetWheelScrollSpeed = value; } }
        /// <summary>
        /// If enabled, the item get highlighted when the mouse over it
        /// </summary>
        [Description("If enabled, the item get highlighted when the mouse over it"), Category("ManagedListView")]
        public bool DrawHighlight
        { get { return ManagedListViewPanel1.DrawHighlight; } set { ManagedListViewPanel1.DrawHighlight = value; } }
        /// <summary>
        /// The images list that will be used for draw
        /// </summary>
        [Description("The images list that will be used for draw"), Category("ManagedListView")]
        public ImageList ImagesList
        { get { return ManagedListViewPanel1.ImagesList; } set { ManagedListViewPanel1.ImagesList = value; } }
        /// <summary>
        /// Get the selected items collection
        /// </summary>
        [Browsable(false)]
        public List<ManagedListViewItem> SelectedItems
        {
            get
            {
                return ManagedListViewPanel1.SelectedItems;
            }
        }
        /// <summary>
        /// Get or set if this control can accept dropped data
        /// </summary>
        [Description("Indicate if the control accept drop data"), Category("ManagedListView")]
        public override bool AllowDrop
        {
            get
            {
                return base.AllowDrop;
            }
            set
            {
                ManagedListViewPanel1.AllowDrop = value;
                base.AllowDrop = value;
            }
        }
        /// <summary>
        /// Get or set the font of this control
        /// </summary>
        [Description("The font of the item texts"), Category("ManagedListView")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                ManagedListViewPanel1.Font = value;
            }
        }
        /// <summary>
        /// Get or set the background image
        /// </summary>
        [Description("The background of the control."), Category("ManagedListView")]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
                ManagedListViewPanel1.BackgroundImage = value;
            }
        }
        /// <summary>
        /// Specifies how the background get rendered.
        /// </summary>
        [Description("Specifies how the background get rendered."), Category("ManagedListView")]
        public ManagedListViewBackgroundRenderMode BackgroundRenderMode
        { get { return ManagedListViewPanel1.BackgroundRenderMode; } set { ManagedListViewPanel1.BackgroundRenderMode = value; ManagedListViewPanel1.CalculateBackgroundBounds(); ManagedListViewPanel1.Invalidate(); } }
        /// <summary>
        /// Get or set the background color of the control
        /// </summary>
        [Description("The background color of the control."), Category("ManagedListView")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                ManagedListViewPanel1.BackColor = base.BackColor = value;
            }
        }
        /// <summary>
        /// The item highlight color
        /// </summary>
        [Description("The highlight color of an item"), Category("Colors")]
        public Color ItemHighlightColor
        {
            get { return ManagedListViewPanel1.ItemHighlightColor; }
            set
            {
                ManagedListViewPanel1.ItemHighlightColor = value;
                ManagedListViewPanel1.Invalidate();
            }
        }
        /// <summary>
        /// The special item color
        /// </summary>
        [Description("The special color of an item that drawn if the item is a special item."), Category("Colors")]
        public Color ItemSpecialColor
        {
            get { return ManagedListViewPanel1.ItemSpecialColor; }
            set
            {
                ManagedListViewPanel1.ItemSpecialColor = value;
                ManagedListViewPanel1.Invalidate();
            }
        }
        /// <summary>
        /// The color when the mouse get over item
        /// </summary>
        [Description("The color of an item when the mouse cursor get over it."), Category("Colors")]
        public Color ItemMouseOverColor
        {
            get { return ManagedListViewPanel1.ItemMouseOverColor; }
            set
            {
                ManagedListViewPanel1.ItemMouseOverColor = value;
                ManagedListViewPanel1.Invalidate();
            }
        }
        /// <summary>
        /// The column color
        /// </summary>
        [Description("The color of the column."), Category("Colors")]
        public Color ColumnColor
        {
            get { return ManagedListViewPanel1.ColumnColor; }
            set
            {
                ManagedListViewPanel1.ColumnColor = value;
                ManagedListViewPanel1.Invalidate();
            }
        }
        /// <summary>
        /// The column highlight color
        /// </summary>
        [Description("The highlight color of the column."), Category("Colors")]
        public Color ColumnHighlightColor
        {
            get { return ManagedListViewPanel1.ColumnHighlightColor; }
            set
            {
                ManagedListViewPanel1.ColumnHighlightColor = value;
                ManagedListViewPanel1.Invalidate();
            }
        }
        /// <summary>
        /// The column click color
        /// </summary>
        [Description("The color of the column when it get clicked."), Category("Colors")]
        public Color ColumnClickColor
        {
            get { return ManagedListViewPanel1.ColumnClickColor; }
            set
            {
                ManagedListViewPanel1.ColumnClickColor = value;
                ManagedListViewPanel1.Invalidate();
            }
        }
        /// <summary>
        /// If true, the item image will be stretched using ratio stretch to fit thumbnails size. Thumbnails mode only.
        /// </summary>
        [Description("Indicate if the item image will be stretched using ratio stretch to fit thumbnails size. Thumbnails mode only."), Category("ManagedListView")]
        public bool StretchThumbnailsToFit
        {
            get { return ManagedListViewPanel1.ThumbnailsKeepImageSize; }
            set
            {
                ManagedListViewPanel1.ThumbnailsKeepImageSize = value;
                ManagedListViewPanel1.Invalidate();
            }
        }
        /// <summary>
        /// If true, the item image will be stretched using ratio stretch to fit thumbnails size. Thumbnails mode only.
        /// </summary>
        [Description("Indicate if highlighted subitem can show tool tip when the subitem's text is not fully visible."), Category("ManagedListView")]
        public bool ShowSubItemToolTip
        {
            get { return showSubToolTip; }
            set { showSubToolTip = value; }
        }
        /// <summary>
        /// Get or set if the control should show info of highlighted item when the mouse over it.
        /// </summary>
        [Description("Get or set if the control should show info of highlighted item when the mouse over it."), Category("ManagedListView")]
        public bool ShowItemInfoOnThumbnailMode
        {
            get { return ManagedListViewPanel1.ShowItemInfoOnThumbnailMode; }
            set { ManagedListViewPanel1.ShowItemInfoOnThumbnailMode = value; }
        }
        #endregion

        #region events
        /// <summary>
        /// Raised when the control need to draw a column
        /// </summary>
        [Description("Raised when the control need to draw a column. The column information will be sent along with this event args"), Category("ManagedListView")]
        public event EventHandler<ManagedListViewColumnDrawArgs> DrawColumn;
        /// <summary>
        /// Raised when the control need to draw an item. The item information will be sent along with this event args. Please note that this event Raised only with Thumbnails View Mode.
        /// </summary>
        [Description("Raised when the control need to draw an item. The item information will be sent along with this event args. Please note that this event Raised only with Thumbnails View Mode."), Category("ManagedListView")]
        public event EventHandler<ManagedListViewItemDrawArgs> DrawItem;
        /// <summary>
        /// Raised when the control need to draw a sub item
        /// </summary>
        [Description("Raised when the control need to draw a sub item. The sub item information will be sent along with this event args. NOTE: Raised only if the sub item draw mode property equal None."), Category("ManagedListView")]
        public event EventHandler<ManagedListViewSubItemDrawArgs> DrawSubItem;
        /// <summary>
        /// Raised when the mouse is over a sub item
        /// </summary>
        [Description("Raised when the mouse get over a sub item."), Category("ManagedListView")]
        public event EventHandler<ManagedListViewMouseOverSubItemArgs> MouseOverSubItem;
        /// <summary>
        /// Raised when the item selection changed
        /// </summary>
        [Description("Raised when the user select/unselect items."), Category("ManagedListView")]
        public event EventHandler SelectedIndexChanged;
        /// <summary>
        /// Raised when the user clicks a column
        /// </summary>
        [Description("Raised when the user click on column."), Category("ManagedListView")]
        public event EventHandler<ManagedListViewColumnClickArgs> ColumnClicked;
        /// <summary>
        /// Raised when the user pressed the return key
        /// </summary>
        [Description("Raised when the user pressed the return key."), Category("ManagedListView")]
        public event EventHandler EnterPressed;
        /// <summary>
        /// Raised when the user double click on item
        /// </summary>
        [Description("Raised when the user double click on item"), Category("ManagedListView")]
        public event EventHandler<ManagedListViewItemDoubleClickArgs> ItemDoubleClick;
        /// <summary>
        /// Raised when the control needs to shwitch to the columns context menu
        /// </summary>
        [Description("Raised when the control needs to switch to the columns context menu"), Category("ManagedListView")]
        public event EventHandler SwitchToColumnsContextMenu;
        /// <summary>
        /// Raised when the control needs to shwitch to the normal context menu
        /// </summary>
        [Description("Raised when the control needs to switch to the normal context menu"), Category("ManagedListView")]
        public event EventHandler SwitchToNormalContextMenu;
        /// <summary>
        /// Raised when the user finished resizing a column
        /// </summary>
        [Description("Raised when the user finished resizing a column"), Category("ManagedListView")]
        public event EventHandler AfterColumnResize;
        /// <summary>
        /// Raised when the user draged item(s)
        /// </summary>
        [Description("Raised when the user dragged item(s)"), Category("ManagedListView")]
        public event EventHandler ItemsDrag;
        /// <summary>
        /// Raised when the user changed the view mode
        /// </summary>
        [Description("Raised when the user changed the view mode"), Category("ManagedListView")]
        public event EventHandler ViewModeChanged;
        /// <summary>
        /// Raised when the user reorder a column
        /// </summary>
        [Description("Raised when the user reorder a column"), Category("ManagedListView")]
        public event EventHandler AfterColumnReorder;
        /// <summary>
        /// Raised when the user adds new item to the collection
        /// </summary>
        [Description("Raised when the user adds new item to the collection"), Category("ManagedListView")]
        public event EventHandler ItemAdded;
        /// <summary>
        /// Raised when the user removes item from the collection
        /// </summary>
        [Description("Raised when the user removes item from the collection"), Category("ManagedListView")]
        public event EventHandler ItemRemoved;
        /// <summary>
        /// Raised when the items collection get cleared
        /// </summary>
        [Description("Raised when the items collection get cleared"), Category("ManagedListView")]
        public event EventHandler ItemsCollectionCleared;
        /// <summary>
        /// Raised when the items collection get sorted
        /// </summary>
        [Description("Raised when the items collection get sorted"), Category("ManagedListView")]
        public event EventHandler ItemsCollectionSorted;
        /// <summary>
        /// Raised when the user adds new column to the collection
        /// </summary>
        [Description("Raised when the user adds new column to the collection"), Category("ManagedListView")]
        public event EventHandler ColumnAdded;
        /// <summary>
        /// Raised when the user removes column from the collection
        /// </summary>
        [Description("Raised when the user removes column from the collection"), Category("ManagedListView")]
        public event EventHandler ColumnRemoved;
        /// <summary>
        /// Raised when the columns collection get cleared
        /// </summary>
        [Description("Raised when the columns collection get cleared"), Category("ManagedListView")]
        public event EventHandler ColumnsCollectionCleared;
        /// <summary>
        /// Raised when the columns collection get sorted
        /// </summary>
        [Description("Raised when the columns collection get sorted"), Category("ManagedListView")]
        public event EventHandler ColumnsCollectionSorted;
        /// <summary>
        /// Raised when the control is about to draw an item and subitems needs to fill. Used on Details view mode only.
        /// </summary>
        [Description("Raised when the control is about to draw an item and subitems needs to fill. Used on Details view mode only."), Category("ManagedListView")]
        public event EventHandler<ManagedListViewItemSelectArgs> FillSubitemsRequest;
        /// <summary>
        /// Raised when the control needs to show info of item on thumbnail mode.
        /// </summary>
        [Description("Raised when the control needs to show info of item on thumbnail mode."), Category("ManagedListView")]
        public event EventHandler<ManagedListViewShowThumbnailTooltipArgs> ShowThumbnailInfoRequest;
        #endregion

        #region Methods
        /// <summary>
        /// Retrieve item at current cursor point
        /// </summary>
        /// <returns>The found item</returns>
        public ManagedListViewItem GetItemAtCursorPoint()
        {
            return ManagedListViewPanel1.items[ManagedListViewPanel1.GetItemIndexAtCursorPoint()];
        }
        /// <summary>
        /// Retrieve item at point
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>The found item</returns>
        public ManagedListViewItem GetItemAtPoint(Point point)
        {
            return ManagedListViewPanel1.items[ManagedListViewPanel1.GetItemIndexAtPoint(point)];
        }
        /// <summary>
        /// Retrieve item index at current cursor point
        /// </summary>
        /// <returns>The found item index</returns>
        public int GetItemIndexAtCursorPoint()
        { return ManagedListViewPanel1.GetItemIndexAtCursorPoint(); }
        /// <summary>
        /// Retrieve item index at point
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>The found item index</returns>
        public int GetItemIndexAtPoint(Point point)
        { return ManagedListViewPanel1.GetItemIndexAtPoint(point); }
        /// <summary>
        /// Scroll view port into item
        /// </summary>
        /// <param name="itemIndex">The item index</param>
        public void ScrollToItem(int itemIndex)
        {
            // try
            {
                UpdateScrollBars();
                int val = ManagedListViewPanel1.GetVscrollValueForItem(itemIndex);
                if (val > vScrollBar1.Maximum)
                    val = vScrollBar1.Maximum;
                vScrollBar1.Value = val;
                ManagedListViewPanel1.VscrollOffset = ManagedListViewPanel1.GetVscrollValueForItem(itemIndex);
            }
            // catch { }
        }
        /// <summary>
        /// Scroll view port into item
        /// </summary>
        /// <param name="item">The item to scroll into</param>
        public void ScrollToItem(ManagedListViewItem item)
        {
            ScrollToItem(ManagedListViewPanel1.items.IndexOf(item));
        }
        /// <summary>
        /// Rises the font changed event
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/></param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ManagedListViewPanel1.Font = this.Font;
        }
        /// <summary>
        /// Update the scroll bars and make the right calculations. Sometime, the control forget to calculate these values and results 
        /// hidden items.
        /// </summary>
        public void UpdateScrollBars()
        {
            if (ManagedListViewPanel1.viewMode == ManagedListViewViewMode.Details)
            {
                int size = ManagedListViewPanel1.buffered_columnsSize;
                if (size < this.Width)
                {
                    hScrollBar1.Maximum = 1;
                    ManagedListViewPanel1.HscrollOffset = hScrollBar1.Value = 0;
                    ManagedListViewPanel1.Invalidate();
                    panel1.Visible = false;
                }
                else
                {
                    hScrollBar1.Maximum = size - ManagedListViewPanel1.Width + 20;
                    panel1.Visible = true;
                    panel1.Enabled = true;
                }

                size = ManagedListViewPanel1.buffered_itemsSize;
                if (size < this.Height)
                {
                    vScrollBar1.Maximum = 1;
                    ManagedListViewPanel1.VscrollOffset = vScrollBar1.Value = 0;
                    ManagedListViewPanel1.Invalidate();

                    panel2.Visible = vScrollBar1.Visible = false;
                }
                else
                {
                    vScrollBar1.Maximum = size - ManagedListViewPanel1.Height + 40;

                    panel2.Visible = vScrollBar1.Visible = true;
                    vScrollBar1.Enabled = true;
                }
            }
            else// Thumbnails
            {
                Size size = ManagedListViewPanel1.CalculateSizeOfItemsAsThumbnails();
                if (size.Height < this.Height)
                {
                    vScrollBar1.Maximum = 1;
                    ManagedListViewPanel1.VscrollOffset = vScrollBar1.Value = 0;
                    ManagedListViewPanel1.Invalidate();

                    panel2.Visible = vScrollBar1.Visible = false;
                    vScrollBar1.Enabled = true;
                }
                else
                {
                    vScrollBar1.Maximum = size.Height - ManagedListViewPanel1.Height + 40;

                    panel2.Visible = vScrollBar1.Visible = true;
                }

                if (size.Width < this.Width)
                {
                    hScrollBar1.Maximum = 1;
                    ManagedListViewPanel1.HscrollOffset = hScrollBar1.Value = 0;
                    ManagedListViewPanel1.Invalidate();
                    panel1.Visible = false;
                }
                else
                {
                    hScrollBar1.Maximum = size.Width - ManagedListViewPanel1.Width + 20;
                    panel1.Visible = true;
                    panel1.Enabled = true;
                }
            }
            HideTooltip();
        }
        /// <summary>
        /// Raises the ItemAdded event
        /// </summary>
        public void OnItemAdded()
        {
            ManagedListViewPanel1.buffered_itemsSize = ManagedListViewPanel1.CalculateItemsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ItemAdded != null)
                ItemAdded(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ItemRemoved event
        /// </summary>
        public void OnItemRemoved()
        {
            ManagedListViewPanel1.buffered_itemsSize = ManagedListViewPanel1.CalculateItemsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ItemRemoved != null)
                ItemRemoved(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ItemsCollectionCleared event
        /// </summary>
        public void OnItemsCollectionCleared()
        {
            ManagedListViewPanel1.buffered_itemsSize = ManagedListViewPanel1.CalculateItemsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ItemsCollectionCleared != null)
                ItemsCollectionCleared(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ItemsCollectionSorted event
        /// </summary>
        public void OnItemsCollectionSorted()
        {
            ManagedListViewPanel1.buffered_itemsSize = ManagedListViewPanel1.CalculateItemsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ItemsCollectionSorted != null)
                ItemsCollectionSorted(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ColumnAdded event
        /// </summary>
        public void OnColumnAdded()
        {
            ManagedListViewPanel1.buffered_columnsSize = ManagedListViewPanel1.CalculateColumnsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ColumnAdded != null)
                ColumnAdded(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ColumnRemoved event
        /// </summary>
        public void OnColumnRemoved()
        {
            ManagedListViewPanel1.buffered_columnsSize = ManagedListViewPanel1.CalculateColumnsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ColumnRemoved != null)
                ColumnRemoved(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ColumnsCollectionCleared event
        /// </summary>
        public void OnColumnsCollectionCleared()
        {
            ManagedListViewPanel1.buffered_columnsSize = ManagedListViewPanel1.CalculateColumnsSize();
            ManagedListViewPanel1_ClearScrolls(this, null);
            ManagedListViewPanel1.OnColumnsCollectionClear();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ColumnsCollectionCleared != null)
                ColumnsCollectionCleared(this, new EventArgs());
        }
        /// <summary>
        /// Raises the ColumnsCollectionSorted event
        /// </summary>
        public void OnColumnsCollectionSorted()
        {
            ManagedListViewPanel1.buffered_columnsSize = ManagedListViewPanel1.CalculateColumnsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ColumnsCollectionSorted != null)
                ColumnsCollectionSorted(this, new EventArgs());
        }
        /// <summary>
        /// Auto set the wheel scroll speed value depending on items collection. AutoSetWheelScrollSpeed property must be set to true.
        /// </summary>
        public void FixWheelScrollSpeed()
        {
            if (autoSetWheelScrollSpeed)
                wheelScrollSpeed = ManagedListViewPanel1.GetVisibleItemsHeight() * 2;
        }
        private void HideTooltip()
        {
            // Clear tooltip
            if (currentToolTip != "")
            {
                timer_tooltip.Stop();
                toolTip1.Hide(this);
                currentToolTip = "";
            }
        }
        /// <summary>
        /// Refresh scroll bars
        /// </summary>
        public void RefreshScrollBarsView()
        {
            ManagedListViewPanel1.buffered_itemsSize = ManagedListViewPanel1.CalculateItemsSize();
            ManagedListViewPanel1_RefreshScrollBars(this, null);
            ManagedListViewPanel1.Invalidate();
            FixWheelScrollSpeed();
            if (ItemAdded != null)
                ItemAdded(this, new EventArgs());
        }
        #endregion

        private void ManagedListView_Paint(object sender, PaintEventArgs e)
        {
            ManagedListViewPanel1.Invalidate();
            UpdateScrollBars();
        }
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            ManagedListViewPanel1.HscrollOffset = hScrollBar1.Value;
            ManagedListViewPanel1.Invalidate();
        }
        private void ManagedListViewPanel1_RefreshValues(object sender, EventArgs e)
        {
            UpdateScrollBars();
        }
        private void ManagedListView_Resize(object sender, EventArgs e)
        {
            UpdateScrollBars();
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            ManagedListViewPanel1.VscrollOffset = vScrollBar1.Value;
            ManagedListViewPanel1.Invalidate();
        }

        private void ManagedListViewPanel1_AdvanceVScrollRequest(object sender, EventArgs e)
        {
            try
            {
                if (vScrollBar1.Value + wheelScrollSpeed < vScrollBar1.Maximum)
                    vScrollBar1.Value += wheelScrollSpeed;
                else
                    vScrollBar1.Value = vScrollBar1.Maximum - 1;
                ManagedListViewPanel1.VscrollOffset = vScrollBar1.Value;
                HideTooltip();
            }
            catch { }
        }
        private void ManagedListViewPanel1_ReverseVScrollRequest(object sender, EventArgs e)
        {
            try
            {
                if (vScrollBar1.Value >= wheelScrollSpeed)
                    vScrollBar1.Value -= wheelScrollSpeed;
                else
                    vScrollBar1.Value = 0;
                ManagedListViewPanel1.VscrollOffset = vScrollBar1.Value;
                HideTooltip();
            }
            catch { }
        }
        private void ManagedListViewPanel1_RefreshScrollBars(object sender, EventArgs e)
        {
            UpdateScrollBars();

        }
        private void ManagedListViewPanel1_DrawColumn(object sender, ManagedListViewColumnDrawArgs e)
        {
            if (DrawColumn != null)
                DrawColumn(this, e);
        }
        private void ManagedListViewPanel1_DrawSubItem(object sender, ManagedListViewSubItemDrawArgs e)
        {
            if (DrawSubItem != null)
                DrawSubItem(this, e);
        }
        private void ManagedListViewPanel1_MouseOverSubItem(object sender, ManagedListViewMouseOverSubItemArgs e)
        {
            if (MouseOverSubItem != null)
                MouseOverSubItem(this, e);
            if (showSubToolTip)
            {
                if (!e.IsTextFullyVisible)
                {
                    if (currentItemIndexOfToolTip != e.ItemIndex || currentColumnIDOfToolTip != e.ColumnID)
                    {
                        // Hide old tip
                        timer_tooltip.Stop();
                        toolTip1.Hide(this);
                        // Setup new one
                        toolTipX = e.SubitemRectangle.X;
                        toolTipY = e.SubitemRectangle.Y + Cursor.Size.Height;
                        currentToolTip = e.SubitemText;
                        currentItemIndexOfToolTip = e.ItemIndex;
                        currentColumnIDOfToolTip = e.ColumnID;
                        timer_tooltip.Start();
                    }
                }
                else
                {
                    // Clear tooltip
                    if (currentToolTip != "")
                    {
                        timer_tooltip.Stop();
                        toolTip1.Hide(this);
                        currentToolTip = "";
                    }
                }
            }
        }
        private void ManagedListViewPanel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, new EventArgs());
        }
        private void ManagedListViewPanel1_ColumnClicked(object sender, ManagedListViewColumnClickArgs e)
        {
            if (ColumnClicked != null)
                ColumnClicked(this, e);
        }
        private void ManagedListViewPanel1_DrawItem(object sender, ManagedListViewItemDrawArgs e)
        {
            if (DrawItem != null)
                DrawItem(this, e);
            if (ManagedListViewPanel1.viewMode == ManagedListViewViewMode.Thumbnails)
            {
                // Show tooltip of the text in this case
            }
        }

        private void ManagedListViewPanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }
        private void ManagedListViewPanel1_ClearScrolls(object sender, EventArgs e)
        {
            hScrollBar1.Maximum = 1;
            ManagedListViewPanel1.HscrollOffset = hScrollBar1.Value = 0;
            panel1.Visible = false;

            vScrollBar1.Maximum = 1;
            ManagedListViewPanel1.VscrollOffset = vScrollBar1.Value = 0;
            //  vScrollBar1.Visible = false;

            ManagedListViewPanel1.Invalidate();
        }
        private void hScrollBar1_KeyDown(object sender, KeyEventArgs e)
        {
            ManagedListViewPanel1.OnKeyDownRaised(e);
        }
        private void ManagedListViewPanel1_ItemDoubleClick(object sender, ManagedListViewItemDoubleClickArgs e)
        {
            if (ItemDoubleClick != null)
                ItemDoubleClick(this, e);
        }
        private void ManagedListViewPanel1_EnterPressedOverItem(object sender, EventArgs e)
        {
            if (EnterPressed != null)
                EnterPressed(this, new EventArgs());
        }

        private void ManagedListViewPanel1_SwitchToColumnsContextMenu(object sender, EventArgs e)
        {
            if (SwitchToColumnsContextMenu != null)
                SwitchToColumnsContextMenu(this, new EventArgs());
        }
        private void ManagedListViewPanel1_SwitchToNormalContextMenu(object sender, EventArgs e)
        {
            if (SwitchToNormalContextMenu != null)
                SwitchToNormalContextMenu(this, new EventArgs());
        }
        private void ManagedListViewPanel1_AfterColumnResize(object sender, EventArgs e)
        {
            if (AfterColumnResize != null)
                AfterColumnResize(this, new EventArgs());
        }
        private void ManagedListViewPanel1_ItemsDrag(object sender, EventArgs e)
        {
            if (ItemsDrag != null)
                ItemsDrag(this, new EventArgs());
        }

        private void ManagedListViewPanel1_DragDrop(object sender, DragEventArgs e)
        {
            OnDragDrop(e);
        }
        private void ManagedListViewPanel1_DragEnter(object sender, DragEventArgs e)
        {
            OnDragEnter(e);
        }
        private void ManagedListViewPanel1_DragLeave(object sender, EventArgs e)
        {
            OnDragLeave(e);
        }
        private void ManagedListViewPanel1_DragOver(object sender, DragEventArgs e)
        {
            OnDragOver(e);
        }
        private void ManagedListViewPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }
        private void ManagedListView_MouseLeave(object sender, EventArgs e)
        {
            ManagedListViewPanel1.OnMouseLeaveRise();
            // Clear tooltip
            if (currentToolTip != "")
            {
                timer_tooltip.Stop();
                toolTip1.Hide(this);
                currentToolTip = "";
            }
        }
        private void ManagedListView_MouseEnter(object sender, EventArgs e)
        {
            UpdateScrollBars();
        }
        private void ManagedListViewPanel1_KeyDown(object sender, KeyEventArgs e)
        {
            ManagedListViewPanel1.OnKeyDownRaised(e);
        }
        private void ManagedListViewPanel1_ScrollToSelectedItemRequest(object sender, ManagedListViewItemSelectArgs e)
        {
            ScrollToItem(e.ItemIndex);
        }
        private void ManagedListViewPanel1_AfterColumnReorder(object sender, EventArgs e)
        {
            if (AfterColumnReorder != null)
                AfterColumnReorder(this, null);
        }
        private void ManagedListViewPanel1_Enter(object sender, EventArgs e)
        {
            OnEnter(e);
        }
        private void ManagedListViewPanel1_Leave(object sender, EventArgs e)
        {
            OnLeave(e);
        }
        private void ManagedListViewPanel1_AdvanceHScrollRequest(object sender, EventArgs e)
        {
            try
            {
                if (hScrollBar1.Value + wheelScrollSpeed < hScrollBar1.Maximum)
                    hScrollBar1.Value += wheelScrollSpeed;
                else
                    hScrollBar1.Value = hScrollBar1.Maximum - 1;
                ManagedListViewPanel1.HscrollOffset = hScrollBar1.Value;
                HideTooltip();
            }
            catch { }
        }
        private void ManagedListViewPanel1_ReverseHScrollRequest(object sender, EventArgs e)
        {
            try
            {
                if (hScrollBar1.Value >= wheelScrollSpeed)
                    hScrollBar1.Value -= wheelScrollSpeed;
                else
                    hScrollBar1.Value = 0;
                ManagedListViewPanel1.HscrollOffset = hScrollBar1.Value;
                HideTooltip();
            }
            catch { }
        }
        private void ManagedListView_MouseClick(object sender, MouseEventArgs e)
        {
            HideTooltip();
        }
        private void timer_tooltip_Tick(object sender, EventArgs e)
        {
            if (tooltip_timer > 0)
                tooltip_timer--;
            else
            {
                toolTip1.Show(currentToolTip, this, toolTipX, toolTipY, 5000);
                timer_tooltip.Stop();
            }
        }
        private void ManagedListViewPanel1_FillSubitemsRequest(object sender, ManagedListViewItemSelectArgs e)
        {
            if (FillSubitemsRequest != null)
                FillSubitemsRequest(this, e);
        }
        private void ManagedListViewPanel1_ShowThumbnailTooltipRequest(object sender, ManagedListViewShowThumbnailTooltipArgs e)
        {
            if (showSubToolTip)
            {
                if (currentItemIndexOfToolTip != e.ItemsIndex)
                {
                    currentItemIndexOfToolTip = e.ItemsIndex;
                    // Setup new one
                    toolTipX = e.TextThumbnailRectangle.X;
                    toolTipY = e.TextThumbnailRectangle.Y;
                    currentToolTip = e.TextToShow;
                    currentColumnIDOfToolTip = "";
                    tooltip_timer = tooltip_timer_reload;
                    timer_tooltip.Start();
                    //toolTip1.Show(currentToolTip, this, toolTipX, toolTipY, 10000);
                }
            }
        }
        private void ManagedListViewPanel1_HideThumbnailTooltipRequest(object sender, EventArgs e)
        {
            // Clear tooltip
            if (currentToolTip != "")
            {
                timer_tooltip.Stop();
                toolTip1.Hide(this);
                currentToolTip = "";
            }
        }
        private void ManagedListViewPanel1_ShowThumbnailInfoRequest(object sender, ManagedListViewShowThumbnailTooltipArgs e)
        {
            if (ShowThumbnailInfoRequest != null)
                ShowThumbnailInfoRequest(this, e);
        }
    }
}
