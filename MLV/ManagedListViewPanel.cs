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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLV
{
    /// <summary>
    /// The Advanced ListView panel. This control should be used only on ManagedListView user control.
    /// </summary>
    public partial class ManagedListViewPanel : Control
    {
        /// <summary>
        /// The Advanced ListView panel.
        /// </summary>
        public ManagedListViewPanel()
        {
            InitializeComponent();
            //ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            //this.SetStyle(flag, true);
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer;
            this.SetStyle(flag, true);
            this.AllowDrop = true;
            ShowItemInfoOnThumbnailMode = true;

            _StringFormat = new StringFormat();
            _StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            _StringFormat.Trimming = StringTrimming.EllipsisCharacter;
        }

        private StringFormat _StringFormat;
        private Point DownPoint = new Point();
        private Point UpPoint = new Point();
        private Point DownPointAsViewPort = new Point();
        private Point CurrentMousePosition = new Point();
        /// <summary>
        /// The view mode
        /// </summary>
        public ManagedListViewViewMode viewMode = ManagedListViewViewMode.Details;
        /// <summary>
        /// The columns collection
        /// </summary>
        public ManagedListViewColumnsCollection columns;
        /// <summary>
        /// The items collection
        /// </summary>
        public ManagedListViewItemsCollection items;
        /// <summary>
        /// The images list
        /// </summary>
        public ImageList ImagesList = new ImageList();
        /// <summary>
        /// The selected items collection
        /// </summary>
        public List<ManagedListViewItem> SelectedItems
        {
            get
            {
                List<ManagedListViewItem> selectedItems = new List<ManagedListViewItem>();
                foreach (ManagedListViewItem item in items)
                {
                    if (item.Selected)
                    {
                        selectedItems.Add(item);
                    }
                }
                return selectedItems;
            }
        }
        /// <summary>
        /// The horisontal scroll offset
        /// </summary>
        public int HscrollOffset = 0;
        /// <summary>
        /// The vertical scroll offset value
        /// </summary>
        public int VscrollOffset = 0;

        /// <summary>
        /// Indecate whether the user can drag and drop items
        /// </summary>
        public bool AllowItemsDragAndDrop = true;
        /// <summary>
        /// Indecate whether the user can reorder the columns.
        /// </summary>
        public bool AllowColumnsReorder = true;
        /// <summary>
        /// Indecate whether the sort mode value of a column get changed when the user click that column.
        /// </summary>
        public bool ChangeColumnSortModeWhenClick = true;
        /// <summary>
        /// The thumbnail height value
        /// </summary>
        public int ThumbnailsHeight = 36;
        /// <summary>
        /// The thumbnails width value
        /// </summary>
        public int ThumbnailsWidth = 36;
        /// <summary>
        /// The item text height
        /// </summary>
        public int itemTextHeight = 28;
        /// <summary>
        /// Indecate whether to draw item highlight when the mouse over that item.
        /// </summary>
        public bool DrawHighlight = true;
        /// <summary>
        /// The item height
        /// </summary>
        public int itemHeight = 15;
        /// <summary>
        /// If true, the image will not be stretched to thumbnails size if it's larger than image original size.
        /// </summary>
        public bool ThumbnailsKeepImageSize;
        private ManagedListViewMoveType moveType = ManagedListViewMoveType.None;

        private int selectedColumnIndex = -1;
        private bool highlightSelectedColumn = false;
        private bool highlightItemAsOver = false;
        private int overItemSelectedIndex = 0;
        private int OldoverItemSelectedIndex = 0;
        private int LatestOverItemSelectedIndex = 0;
        private int originalcolumnWidth = 0;
        private int downX = 0;
        private bool isMouseDown = false;
        private int SelectRectanglex;
        private int SelectRectangley;
        private int SelectRectanglex1;
        private int SelectRectangley1;
        private bool DrawSelectRectangle;
        private bool isSecectingItems = false;
        private bool isMovingColumn = false;
        private int currentColumnMovedIndex = 0;
        private int columnHeight = 24;
        private int columnh = 8;
        private int itemh = 6;
        private int highlightSensitive = 6;
        private int spaceBetweenItemsThunmbailsView = 5;
        private string currentSearchChar = "";
        private int previousShift = -1;
        /// <summary>
        /// The buffered columns size
        /// </summary>
        public int buffered_columnsSize;
        /// <summary>
        /// Buffered items total size
        /// </summary>
        public int buffered_itemsSize;
        /*Colors and images*/
        private Image backgroundThumbnail;
        private int backgroundDrawX;
        private int backgroundDrawY;
        private int backgroundDrawH;
        private int backgroundDrawW;
        public bool ShowItemInfoOnThumbnailMode;
        private ManagedListViewShowThumbnailTooltipArgs thumb_tooltip;

        /// <summary>
        /// The item highlight color
        /// </summary>
        public Color ItemHighlightColor = Color.LightSkyBlue;
        /// <summary>
        /// The special item color
        /// </summary>
        public Color ItemSpecialColor = Color.YellowGreen;
        /// <summary>
        /// The color when the mouse get over item
        /// </summary>
        public Color ItemMouseOverColor = Color.LightGray;
        /// <summary>
        /// The column color
        /// </summary>
        public Color ColumnColor = Color.Silver;
        /// <summary>
        /// The column highlight color
        /// </summary>
        public Color ColumnHighlightColor = Color.LightSkyBlue;
        /// <summary>
        /// The column click color
        /// </summary>
        public Color ColumnClickColor = Color.PaleVioletRed;

        #region events
        /// <summary>
        /// Raised when the control requests an advance for vertical scroll value
        /// </summary>
        public event EventHandler AdvanceVScrollRequest;
        /// <summary>
        /// Raised when the control requests a reverse for vertical scroll value
        /// </summary>
        public event EventHandler ReverseVScrollRequest;
        /// <summary>
        /// Raised when the control requests an advance for horizontal scroll value
        /// </summary>
        public event EventHandler AdvanceHScrollRequest;
        /// <summary>
        /// Raised when the control requests a reverse for horizontal scroll value
        /// </summary>
        public event EventHandler ReverseHScrollRequest;
        /// <summary>
        /// Raised when the control requests a refresh for scroll bars
        /// </summary>
        public event EventHandler RefreshScrollBars;
        /// <summary>
        /// Raised when selected items value changed
        /// </summary>
        public event EventHandler SelectedIndexChanged;
        /// <summary>
        /// Raised when the control needs to draw column
        /// </summary>
        public event EventHandler<ManagedListViewColumnDrawArgs> DrawColumn;
        /// <summary>
        /// Raised when the control needs to draw item
        /// </summary>
        public event EventHandler<ManagedListViewItemDrawArgs> DrawItem;
        /// <summary>
        /// Raised when the control needs to draw subitem
        /// </summary>
        public event EventHandler<ManagedListViewSubItemDrawArgs> DrawSubItem;
        /// <summary>
        /// Raised when the mouse cursor over a subiem.
        /// </summary>
        public event EventHandler<ManagedListViewMouseOverSubItemArgs> MouseOverSubItem;
        /// <summary>
        /// Raised when a column get clicked
        /// </summary>
        public event EventHandler<ManagedListViewColumnClickArgs> ColumnClicked;
        /// <summary>
        /// Raised when an item double click occures
        /// </summary>
        public event EventHandler<ManagedListViewItemDoubleClickArgs> ItemDoubleClick;
        /// <summary>
        /// Raised when the user presses enter after selecting one item.
        /// </summary>
        public event EventHandler EnterPressedOverItem;
        /// <summary>
        /// Raised when the control requests to switch into the columns context menu strip
        /// </summary>
        public event EventHandler SwitchToColumnsContextMenu;
        /// <summary>
        /// Raised when the control requests to switch into the normal context menu strip
        /// </summary>
        public event EventHandler SwitchToNormalContextMenu;
        /// <summary>
        /// Raised when a column get resized
        /// </summary>
        public event EventHandler AfterColumnResize;
        /// <summary>
        /// Raised when a column get reordered
        /// </summary>
        public event EventHandler AfterColumnReorder;
        /// <summary>
        /// Raised when an item get draged
        /// </summary>
        public event EventHandler ItemsDrag;
        /// <summary>
        /// Raised when the control requests to scroll into given item
        /// </summary>
        public event EventHandler<ManagedListViewItemSelectArgs> ScrollToSelectedItemRequest;
        /// <summary>
        /// Raised when the control is about to draw an item and subitems needs to fill. Used on Details view mode only.
        /// </summary>
        public event EventHandler<ManagedListViewItemSelectArgs> FillSubitemsRequest;
        /// <summary>
        /// Raised when values refresh required.
        /// </summary>
        public event EventHandler RefreshValues;
        public event EventHandler<ManagedListViewShowThumbnailTooltipArgs> ShowThumbnailTooltipRequest;
        public event EventHandler<ManagedListViewShowThumbnailTooltipArgs> ShowThumbnailInfoRequest;
        public event EventHandler HideThumbnailTooltipRequest;
        #endregion

        /// <summary>
        /// Get or set the font
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                if (RefreshScrollBars != null)
                    RefreshScrollBars(this, null);
                Invalidate();
            }
        }
        private enum ManagedListViewMoveType
        {
            None, ColumnVLine, Column
        }
        /// <summary>
        /// Get item index at cursor point
        /// </summary>
        /// <returns>The item index if found otherwise -1</returns>
        public int GetItemIndexAtCursorPoint()
        {
            return GetItemIndexAtPoint(CurrentMousePosition);
        }
        /// <summary>
        /// Get item index at point
        /// </summary>
        /// <param name="location">The location within the viewport</param>
        /// <returns>The item index if found otherwise -1</returns>
        public int GetItemIndexAtPoint(Point location)
        {
            int index = -1;
            if (viewMode == ManagedListViewViewMode.Details)
            {
                int y = location.Y;
                y -= columnHeight;
                if (y > 0 && y < buffered_itemsSize)
                {
                    index = (VscrollOffset + y) / itemHeight;
                }
            }
            else
            {
                //Thumbnails view select item
                int offset = VscrollOffset % (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                int vLines = this.Height / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                int passedRows = VscrollOffset / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                int itemIndex = passedRows * hLines;

                int mouseVlines = (location.Y + offset) / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                int mouseHlines = location.X / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);

                int indexAsMouse = (mouseVlines * hLines) + mouseHlines;
                if (indexAsMouse + itemIndex < items.Count)
                {
                    if (location.X < hLines * (spaceBetweenItemsThunmbailsView + ThumbnailsWidth))
                    {
                        index = indexAsMouse + itemIndex;
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// Calculate all columns width
        /// </summary>
        /// <returns>The columns width (all columns)</returns>
        public int CalculateColumnsSize()
        {
            int size = 0;
            foreach (ManagedListViewColumn column in columns)
            {
                size += column.Width;
            }
            return size;
        }
        /// <summary>
        /// Calculate all items size (height). Works with Details view mode only
        /// </summary>
        /// <returns>The height of all items</returns>
        public int CalculateItemsSize()
        {
            Size CharSize = TextRenderer.MeasureText("TEST", this.Font);
            itemHeight = CharSize.Height + itemh;
            return itemHeight * items.Count;
        }
        /// <summary>
        /// Get the height of one item. Works with Details view mode only
        /// </summary>
        /// <returns></returns>
        public int GetItemHeight()
        {
            Size CharSize = TextRenderer.MeasureText("TEST", this.Font);
            return CharSize.Height + itemh;
        }
        /// <summary>
        /// Get the visible items total height
        /// </summary>
        /// <returns></returns>
        public int GetVisibleItemsHeight()
        {
            Size CharSize = TextRenderer.MeasureText("TEST", this.Font);
            int ih = CharSize.Height + itemh;
            return (this.Height / ih) + 2;
        }
        /// <summary>
        /// Get vertical scroll value for item
        /// </summary>
        /// <param name="itemIndex">The item index</param>
        /// <returns>The vertical scroll value</returns>
        public int GetVscrollValueForItem(int itemIndex)
        {
            if (viewMode == ManagedListViewViewMode.Details)
            {
                return itemIndex * itemHeight;
            }
            else
            {
                int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                // used too many math calculation to get this lol
                int val = (itemIndex * (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight)) / hLines;
                val -= val % (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                return val;
            }
        }
        /// <summary>
        /// Calculate all items size (height). Works with Thumbnails view mode only
        /// </summary>
        /// <returns></returns>
        public Size CalculateSizeOfItemsAsThumbnails()
        {
            if (items.Count == 0)
                return Size.Empty;
            int w = 0;
            int h = 0;
            int vLines = this.Height / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
            if (vLines == 0)
                vLines = 1;

            int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
            if (hLines == 0)
                hLines = 1;

            double itemRows = Math.Ceiling((double)items.Count / hLines);
            h = (int)(itemRows * (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight));

            if ((spaceBetweenItemsThunmbailsView + ThumbnailsWidth) > this.Width)
                w = ThumbnailsWidth;

            return new Size(w, h);
        }
        /// <summary>
        /// Raise the key own event
        /// </summary>
        /// <param name="e">The key event arguments</param>
        public void OnKeyDownRaised(KeyEventArgs e)
        {
            if (items.Count > 0)
            {
                if (e.KeyCode == Keys.PageUp)
                {
                    // select none
                    foreach (ManagedListViewItem item in items)
                        item.Selected = false;
                    // select first one
                    items[0].Selected = true;
                    // scroll
                    if (ScrollToSelectedItemRequest != null)
                        ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(0));
                    return;
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    // select none
                    foreach (ManagedListViewItem item in items)
                        item.Selected = false;
                    // select first one
                    items[items.Count - 1].Selected = true;
                    // scroll
                    if (ScrollToSelectedItemRequest != null)
                        ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(items.Count - 1));
                    return;
                }
            }
            if (viewMode == ManagedListViewViewMode.Details)
            {
                #region single selection
                if (SelectedItems.Count == 1)
                {
                    int index = items.IndexOf(SelectedItems[0]);
                    if (e.KeyCode == Keys.Down)
                    {
                        if (index + 1 < items.Count)
                        {
                            items[index].Selected = false;
                            items[index + 1].Selected = true;

                            //see if we need to scroll
                            int lines = (this.Height / itemHeight) - 2;
                            int maxLineIndex = (VscrollOffset / itemHeight) + lines;

                            if (index + 1 > maxLineIndex)
                                if (AdvanceVScrollRequest != null)
                                    AdvanceVScrollRequest(this, null);

                            if (SelectedIndexChanged != null)
                                SelectedIndexChanged(this, new EventArgs());
                        }
                    }
                    else if (e.KeyCode == Keys.Up)
                    {
                        if (index - 1 >= 0)
                        {
                            items[index].Selected = false;
                            items[index - 1].Selected = true;

                            int lowLineIndex = (VscrollOffset / itemHeight) + 1;

                            if (index - 1 < lowLineIndex)
                                if (ReverseVScrollRequest != null)
                                    ReverseVScrollRequest(this, null);

                            if (SelectedIndexChanged != null)
                                SelectedIndexChanged(this, new EventArgs());
                        }
                    }
                    else if (e.KeyCode == Keys.Return)
                    {
                        if (EnterPressedOverItem != null)
                            EnterPressedOverItem(this, new EventArgs());
                    }
                    else//char ?
                    {
                        KeysConverter conv = new KeysConverter();
                        if (conv.ConvertToString(e.KeyCode) != currentSearchChar)
                        {
                            items[index].Selected = false;
                            index = 0;
                        }
                        currentSearchChar = conv.ConvertToString(e.KeyCode);
                        for (int i = index + 1; i < items.Count; i++)
                        {
                            ManagedListViewSubItem subItem = items[i].GetSubItemByID(columns[0].ID);
                            if (subItem != null)
                            {
                                if (subItem.Text.Length > 0)
                                {
                                    if (subItem.Text.Substring(0, 1) == conv.ConvertToString(e.KeyCode))
                                    {
                                        items[index].Selected = false;
                                        items[i].Selected = true;
                                        if (ScrollToSelectedItemRequest != null)
                                            ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(i));
                                        this.Invalidate();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region multi selection
                else if (SelectedItems.Count > 1)
                {
                    int index = items.IndexOf(SelectedItems[0]);
                    KeysConverter conv = new KeysConverter();
                    for (int i = index + 1; i < items.Count; i++)
                    {
                        ManagedListViewSubItem subItem = items[i].GetSubItemByID(columns[0].ID);
                        if (subItem != null)
                        {
                            if (subItem.Text.Length > 0)
                            {
                                if (subItem.Text.Substring(0, 1) == conv.ConvertToString(e.KeyCode))
                                {
                                    items[index].Selected = false;
                                    items[i].Selected = true;
                                    if (ScrollToSelectedItemRequest != null)
                                        ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(i));
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region No selection
                else
                {
                    KeysConverter conv = new KeysConverter();
                    for (int i = 0; i < items.Count; i++)
                    {
                        ManagedListViewSubItem subItem = items[i].GetSubItemByID(columns[0].ID);
                        if (subItem != null)
                        {
                            if (subItem.Text.Length > 0)
                            {
                                if (subItem.Text.Substring(0, 1) == conv.ConvertToString(e.KeyCode))
                                {
                                    items[i].Selected = true;
                                    if (ScrollToSelectedItemRequest != null)
                                        ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(i));
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            else// Thumbnails
            {
                #region single selection
                if (SelectedItems.Count == 1)
                {
                    int index = items.IndexOf(SelectedItems[0]);
                    // in this mode we got 4 directions so calculations may be more complicated
                    if (e.KeyCode == Keys.Right)
                    {
                        if (index + 1 < items.Count)
                        {
                            // advance selection
                            items[index].Selected = false;
                            items[index + 1].Selected = true;

                            // see if the new selected item is under the viewport
                            int vscroll = GetVscrollValueForItem(index + 1);
                            if (this.Height - vscroll < (ThumbnailsHeight + itemTextHeight))
                            {
                                if (ScrollToSelectedItemRequest != null)
                                    ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(index + 1));
                            }
                            if (SelectedIndexChanged != null)
                                SelectedIndexChanged(this, new EventArgs());
                        }
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        if (index - 1 >= 0)
                        {
                            items[index].Selected = false;
                            items[index - 1].Selected = true;

                            int vscroll = GetVscrollValueForItem(index - 1);

                            if (vscroll < VscrollOffset)
                                if (ScrollToSelectedItemRequest != null)
                                    ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(index - 1));

                            if (SelectedIndexChanged != null)
                                SelectedIndexChanged(this, new EventArgs());
                        }
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        // find out the index of the item below the selected one
                        int vLines = this.Height / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                        int passedRows = VscrollOffset / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        int itemIndexOfFirstItemInViewPort = passedRows * hLines;
                        int addit = index - itemIndexOfFirstItemInViewPort;
                        int newIndex = itemIndexOfFirstItemInViewPort + hLines + addit;
                        // now let's see if we can select this one
                        if (newIndex < items.Count)
                        {
                            // advance selection
                            items[index].Selected = false;
                            items[newIndex].Selected = true;

                            // see if the new selected item is under the viewport
                            int vscroll = GetVscrollValueForItem(newIndex);
                            if (this.Height - vscroll < (ThumbnailsHeight + itemTextHeight))
                            {
                                if (ScrollToSelectedItemRequest != null)
                                    ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(newIndex));
                            }
                            if (SelectedIndexChanged != null)
                                SelectedIndexChanged(this, new EventArgs());
                        }
                    }
                    else if (e.KeyCode == Keys.Up)
                    {
                        // find out the index of the item above the selected one
                        int vLines = this.Height / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                        int passedRows = VscrollOffset / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        int itemIndexOfFirstItemInViewPort = passedRows * hLines;
                        int addit = index - itemIndexOfFirstItemInViewPort;
                        int newIndex = itemIndexOfFirstItemInViewPort - hLines + addit;
                        // now let's see if we can select this one
                        if (newIndex >= 0)
                        {
                            // advance selection
                            items[index].Selected = false;
                            items[newIndex].Selected = true;

                            // see if the new selected item is under the viewport
                            int vscroll = GetVscrollValueForItem(newIndex);
                            if (vscroll < VscrollOffset)
                            {
                                if (ScrollToSelectedItemRequest != null)
                                    ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(newIndex));
                            }
                            if (SelectedIndexChanged != null)
                                SelectedIndexChanged(this, new EventArgs());
                        }
                    }
                    else if (e.KeyCode == Keys.Return)
                    {
                        if (EnterPressedOverItem != null)
                            EnterPressedOverItem(this, new EventArgs());
                    }
                    else//char ?
                    {
                        KeysConverter conv = new KeysConverter();
                        for (int i = index + 1; i < items.Count; i++)
                        {
                            if (items[i].Text.Length > 0)
                            {
                                if (items[i].Text.Substring(0, 1) == conv.ConvertToString(e.KeyCode))
                                {
                                    items[index].Selected = false;
                                    items[i].Selected = true;
                                    if (ScrollToSelectedItemRequest != null)
                                        ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(i));
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region multi selection
                else if (SelectedItems.Count > 1)
                {
                    int index = items.IndexOf(SelectedItems[0]);
                    KeysConverter conv = new KeysConverter();
                    for (int i = index + 1; i < items.Count; i++)
                    {
                        ManagedListViewSubItem sub = items[i].GetSubItemByID(columns[0].ID);
                        if (sub == null)
                            continue;
                        if (sub.Text.Length > 0)
                        {
                            if (sub.Text.Substring(0, 1) == conv.ConvertToString(e.KeyCode))
                            {
                                items[index].Selected = false;
                                items[i].Selected = true;
                                ScrollToSelectedItemRequest?.Invoke(this, new ManagedListViewItemSelectArgs(i));
                                break;
                            }
                        }
                    }
                }
                #endregion
                #region No selection
                else
                {
                    KeysConverter conv = new KeysConverter();
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].Text.Length > 0)
                        {
                            if (items[i].Text.Substring(0, 1) == conv.ConvertToString(e.KeyCode))
                            {
                                items[i].Selected = true;
                                if (ScrollToSelectedItemRequest != null)
                                    ScrollToSelectedItemRequest(this, new ManagedListViewItemSelectArgs(i));
                                break;
                            }
                        }
                    }
                }
                #endregion
            }
            this.Invalidate();
        }
        /// <summary>
        /// Raise the refresh scroll bars event
        /// </summary>
        public void OnRefreshScrollBars()
        {
            if (RefreshScrollBars != null)
                RefreshScrollBars(this, null);
        }
        /// <summary>
        /// Raise the mouse leave event
        /// </summary>
        public void OnMouseLeaveRise()
        {
            OnMouseLeave(new EventArgs());
        }
        /// <summary>
        /// Call this when the columns collection get cleared
        /// </summary>
        public void OnColumnsCollectionClear()
        {
            highlightSelectedColumn = false;
            highlightItemAsOver = false;
            overItemSelectedIndex = -1;
            OldoverItemSelectedIndex = -1;
            LatestOverItemSelectedIndex = -1;
            HscrollOffset = 0;
            VscrollOffset = 0;
        }

        /// <summary>
        /// Raise the paint event
        /// </summary>
        /// <param name="pe"><see cref="PaintEventArgs"/></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            // Draw background color
            pe.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);

            // Draw background
            if (backgroundThumbnail != null)
                pe.Graphics.DrawImage(backgroundThumbnail, backgroundDrawX, backgroundDrawY, backgroundDrawW, backgroundDrawH);
            if (viewMode == ManagedListViewViewMode.Details)
                DrawDetailsView(pe);
            else
                DrawThumbailsView(pe);


            //select rectangle
            if (DrawSelectRectangle)
                pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Gray)),
                    SelectRectanglex, SelectRectangley, SelectRectanglex1 - SelectRectanglex, SelectRectangley1 - SelectRectangley);
        }
        /// <summary>
        /// Draw the background
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // base.OnPaintBackground(pevent);

        }
        /// <summary>
        /// Raise the mouse down event
        /// </summary>
        /// <param name="e"><see cref="MouseEventArgs"/></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DownPoint = e.Location;
            DownPointAsViewPort = new Point(e.X + HscrollOffset, e.Y + VscrollOffset);
            isMouseDown = true;
            if (viewMode == ManagedListViewViewMode.Details)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (e.Y > columnHeight)
                    {
                        if (e.X > buffered_columnsSize - HscrollOffset)
                        {
                            DrawSelectRectangle = true;
                        }
                        else if (e.Y > buffered_itemsSize - VscrollOffset)
                        {
                            DrawSelectRectangle = true;
                        }
                        else
                            DrawSelectRectangle = false;
                    }
                    else
                        DrawSelectRectangle = false;
                }
                else
                    DrawSelectRectangle = false;
            }
            else
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (!highlightItemAsOver && overItemSelectedIndex < 0)
                    {
                        DrawSelectRectangle = true;
                    }
                    else
                    {
                        DrawSelectRectangle = false;
                    }
                }
            }

            Invalidate();
        }
        /// <summary>
        /// Raise the mouse up event
        /// </summary>
        /// <param name="e"><see cref="MouseEventArgs"/></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            UpPoint = e.Location;
            base.OnMouseUp(e);
            #region resort column

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (moveType == ManagedListViewMoveType.ColumnVLine && selectedColumnIndex >= 0)
                {
                    if (AfterColumnResize != null)
                        AfterColumnResize(this, new EventArgs());
                }
                if (moveType == ManagedListViewMoveType.Column && selectedColumnIndex >= 0 && isMovingColumn)
                {
                    //get index
                    int cX = 0;
                    int x = 0;
                    int i = 0;
                    foreach (ManagedListViewColumn column in columns)
                    {
                        cX += column.Width;
                        if (cX >= HscrollOffset)
                        {
                            if (e.X >= (x - HscrollOffset) && e.X <= (cX - HscrollOffset) + 3)
                            {
                                selectedColumnIndex = i;
                            }
                        }
                        i++;
                        x += column.Width;
                        if (x - HscrollOffset > this.Width)
                            break;
                    }
                    ManagedListViewColumn currentColumn = columns[currentColumnMovedIndex];
                    columns.Remove(columns[currentColumnMovedIndex]);
                    columns.Insert(selectedColumnIndex, currentColumn);
                    if (AfterColumnReorder != null)
                        AfterColumnReorder(this, null);
                }
            }
            #endregion
            isMovingColumn = false;
            DrawSelectRectangle = false;
            SelectRectanglex = 0;
            SelectRectangley = 0;
            SelectRectanglex1 = 0;
            SelectRectangley1 = 0;
            previousShift = -1;
            isMouseDown = false;
            Invalidate();
        }
        /// <summary>
        /// Raise the mouse move event
        /// </summary>
        /// <param name="e"><see cref="MouseEventArgs"/></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            CurrentMousePosition = e.Location;
            bool needToInvalidate = true;
            if (viewMode == ManagedListViewViewMode.Details)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (moveType == ManagedListViewMoveType.ColumnVLine && selectedColumnIndex >= 0)
                    {
                        int shift = e.X - downX;
                        if (previousShift != shift & originalcolumnWidth + shift > 5)
                        {
                            Cursor = Cursors.VSplit;
                            columns[selectedColumnIndex].Width = originalcolumnWidth + shift;
                            buffered_columnsSize = CalculateColumnsSize();// reset buffer
                            if (RefreshValues != null)
                                RefreshValues(this, null);
                            previousShift = shift;
                            Invalidate();
                        }
                        return;
                    }
                }
                #region moving column vertical line
                if (e.Y <= columnHeight)
                {
                    if (SwitchToColumnsContextMenu != null)
                        SwitchToColumnsContextMenu(this, new EventArgs());
                    highlightSelectedColumn = true;
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (moveType == ManagedListViewMoveType.ColumnVLine && selectedColumnIndex >= 0)
                        {
                            int shift = e.X - downX;
                            if (previousShift != shift & originalcolumnWidth + shift > 5)
                            {
                                Cursor = Cursors.VSplit;
                                columns[selectedColumnIndex].Width = originalcolumnWidth + shift;
                                buffered_columnsSize = CalculateColumnsSize();// reset buffer
                                if (RefreshValues != null)
                                    RefreshValues(this, null);
                                previousShift = shift;
                            }
                        }
                        if (AllowColumnsReorder)
                        {
                            if (moveType == ManagedListViewMoveType.Column && selectedColumnIndex >= 0)
                            {
                                Cursor = Cursors.SizeAll;
                                currentColumnMovedIndex = selectedColumnIndex;
                                if (e.X > DownPoint.X + 3 || e.X < DownPoint.X - 3)
                                    isMovingColumn = true;
                                if (isMovingColumn)
                                {
                                    if (e.X < 10)
                                        OnReverseHScrollRequest();
                                    else if (e.X > this.Width - 10)
                                        OnAdvanceHScrollRequest();
                                }
                                buffered_columnsSize = CalculateColumnsSize();// reset buffer
                            }
                        }
                    }
                    else
                    {

                        moveType = ManagedListViewMoveType.None;
                        int cX = 0;
                        int x = 0;
                        int i = 0;
                        bool found = false;
                        foreach (ManagedListViewColumn column in columns)
                        {
                            cX += column.Width;
                            if (cX >= HscrollOffset)
                            {
                                if (e.X >= (x - HscrollOffset) && e.X <= (cX - HscrollOffset) + 3)
                                {
                                    selectedColumnIndex = i;
                                    moveType = ManagedListViewMoveType.Column;
                                }
                                //vertical line select ?
                                int min = cX - HscrollOffset - 3;
                                int max = cX - HscrollOffset + 3;
                                if (e.X >= min && e.X <= max)
                                {
                                    downX = e.X;
                                    originalcolumnWidth = column.Width;
                                    moveType = ManagedListViewMoveType.ColumnVLine;
                                    Cursor = Cursors.VSplit;
                                    found = true;
                                    break;
                                }
                            }

                            i++;
                            x += column.Width;

                            if (x - HscrollOffset > this.Width)
                                break;
                        }
                        if (!found)
                            Cursor = Cursors.Default;
                    }
                }
                else
                {
                    if (SwitchToNormalContextMenu != null)
                        SwitchToNormalContextMenu(this, new EventArgs());
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (moveType == ManagedListViewMoveType.ColumnVLine && selectedColumnIndex >= 0)
                        {
                            int shift = e.X - downX;
                            if (previousShift != shift & originalcolumnWidth + shift > 5)
                            {
                                Cursor = Cursors.VSplit;
                                columns[selectedColumnIndex].Width = originalcolumnWidth + shift;
                                buffered_columnsSize = CalculateColumnsSize();// reset buffer
                                if (RefreshValues != null)
                                    RefreshValues(this, null);
                                previousShift = shift;
                            }
                        }
                        if (AllowColumnsReorder)
                        {
                            if (moveType == ManagedListViewMoveType.Column && selectedColumnIndex >= 0)
                            {
                                currentColumnMovedIndex = selectedColumnIndex;
                                if (e.X > DownPoint.X + 3 || e.X < DownPoint.X - 3)
                                    isMovingColumn = true;
                                buffered_columnsSize = CalculateColumnsSize();// reset buffer

                                if (isMovingColumn)
                                {
                                    if (e.X < 10)
                                        OnReverseHScrollRequest();
                                    else if (e.X > this.Width - 10)
                                        OnAdvanceHScrollRequest();
                                }
                            }
                        }
                    }
                    else
                    {
                        //clear
                        moveType = ManagedListViewMoveType.None;
                        selectedColumnIndex = -1;
                        highlightSelectedColumn = false;
                        isMovingColumn = false;
                    }
                }
                #endregion
                #region item select
                if (e.Y > columnHeight)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        Cursor = Cursors.Default;
                        if (DrawSelectRectangle)
                        {
                            //draw select rectangle
                            SelectRectanglex = DownPointAsViewPort.X - HscrollOffset;
                            SelectRectangley = DownPointAsViewPort.Y - VscrollOffset;
                            SelectRectanglex1 = e.X;
                            SelectRectangley1 = e.Y;
                            if (SelectRectanglex1 < SelectRectanglex)
                            {
                                SelectRectanglex = e.X;
                                SelectRectanglex1 = DownPointAsViewPort.X - HscrollOffset;
                            }
                            if (SelectRectangley1 < SelectRectangley)
                            {
                                SelectRectangley = e.Y;
                                SelectRectangley1 = DownPointAsViewPort.Y - VscrollOffset;
                            }
                            if (e.Y > this.Height)
                            {
                                for (int y = 0; y < 10; y++)
                                    if (AdvanceVScrollRequest != null)
                                        AdvanceVScrollRequest(this, null);
                            }

                            //select the items 
                            if (ModifierKeys != Keys.Control)
                            {
                                foreach (ManagedListViewItem item in items)
                                    item.Selected = false;
                            }
                            if (SelectRectanglex + HscrollOffset < buffered_columnsSize)
                            {
                                Size CharSize = TextRenderer.MeasureText("TEST", this.Font);

                                isSecectingItems = true;
                                bool itemSelected = false;
                                for (int i = VscrollOffset + SelectRectangley; i < VscrollOffset + SelectRectangley1; i++)
                                {
                                    int itemIndex = (i - columnHeight) / itemHeight;
                                    if (itemIndex < items.Count)
                                    {
                                        items[itemIndex].Selected = true; itemSelected = true;
                                    }
                                }
                                if (itemSelected)
                                    if (SelectedIndexChanged != null)
                                        SelectedIndexChanged(this, new EventArgs());
                            }
                        }
                        //drag and drop
                        if (AllowItemsDragAndDrop)
                        {
                            if (e.X > DownPoint.X + 3 || e.X < DownPoint.X - 3 || e.Y > DownPoint.Y + 3 | e.Y < DownPoint.Y - 3)
                            {
                                if (highlightItemAsOver)
                                {
                                    if (overItemSelectedIndex >= 0)
                                    {
                                        if (items[overItemSelectedIndex].Selected)
                                        {
                                            if (ItemsDrag != null)
                                                ItemsDrag(this, new EventArgs());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                        if (e.X < buffered_columnsSize - HscrollOffset)
                        {
                            Size CharSize = TextRenderer.MeasureText("TEST", this.Font);
                            int itemIndex = (e.Y + (VscrollOffset - columnHeight) - (itemHeight / highlightSensitive)) / itemHeight;
                            if (itemIndex < items.Count)
                            {
                                highlightItemAsOver = true;
                                overItemSelectedIndex = itemIndex;
                                if (OldoverItemSelectedIndex != itemIndex)
                                {
                                    if (OldoverItemSelectedIndex >= 0 && OldoverItemSelectedIndex < items.Count)
                                        items[OldoverItemSelectedIndex].OnMouseLeave();
                                }

                                int cX = 0;
                                int x = 0;
                                int i = 0;
                                foreach (ManagedListViewColumn column in columns)
                                {
                                    cX += column.Width;
                                    if (cX >= HscrollOffset)
                                    {
                                        if (cX > e.X + HscrollOffset)
                                        {
                                            if (i < items[itemIndex].SubItems.Count)
                                            {
                                                ManagedListViewSubItem sitem = items[itemIndex].GetSubItemByID(column.ID);
                                                if (sitem != null)
                                                {
                                                    sitem.OnMouseOver(new Point(e.X - (x - HscrollOffset), 0), CharSize);
                                                    if (sitem.GetType() == typeof(ManagedListViewRatingSubItem))
                                                    {
                                                        ((ManagedListViewRatingSubItem)sitem).DrawOverImage = true;
                                                    }
                                                }

                                                if (MouseOverSubItem != null)
                                                {
                                                    Font drawFont = sitem.CustomFontEnabled ? sitem.CustomFont : this.Font;
                                                    int textW = TextRenderer.MeasureText(sitem.Text, drawFont).Width;
                                                    textW += 2;// add offset
                                                    switch (sitem.DrawMode)
                                                    {
                                                        case ManagedListViewItemDrawMode.TextAndImage:
                                                            {
                                                                if (sitem.ImageIndex < ImagesList.Images.Count)
                                                                    textW += itemHeight;
                                                                break;
                                                            }
                                                        case ManagedListViewItemDrawMode.UserDraw:
                                                            {
                                                                // We can't raise event here, just add additional width in case
                                                                textW += itemHeight;
                                                                break;
                                                            }
                                                    }
                                                    bool isTextVisible = textW < (cX - x - HscrollOffset);

                                                    // Get y; index = (VscrollOffset + y) / itemHeight;
                                                    int sy = (itemIndex * itemHeight) - VscrollOffset + columnHeight;
                                                    Rectangle subitemrect = new Rectangle(x - HscrollOffset, sy, cX - x - HscrollOffset, itemHeight);

                                                    MouseOverSubItem(this, new ManagedListViewMouseOverSubItemArgs(itemIndex,
                                                        columns[i].ID, e.X - HscrollOffset, subitemrect, sitem.Text, isTextVisible));
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    x += column.Width;
                                    i++;
                                    if (x - HscrollOffset > this.Width)
                                        break;
                                }
                                // refresh draw
                                //needToInvalidate = false;

                                OldoverItemSelectedIndex = itemIndex;
                            }
                            else
                            {
                                highlightItemAsOver = false;
                                if (overItemSelectedIndex < items.Count && overItemSelectedIndex >= 0)
                                    items[OldoverItemSelectedIndex].OnMouseLeave();
                                OldoverItemSelectedIndex = overItemSelectedIndex = -1;
                            }
                        }
                        else
                        {
                            highlightItemAsOver = false;
                            if (overItemSelectedIndex < items.Count && overItemSelectedIndex >= 0)
                                items[OldoverItemSelectedIndex].OnMouseLeave();
                            OldoverItemSelectedIndex = overItemSelectedIndex = -1;
                        }
                    }
                }
                if (DrawSelectRectangle)
                {
                    if (e.Y < columnHeight)
                    {
                        for (int y = 0; y < 10; y++)
                            if (ReverseVScrollRequest != null)
                                ReverseVScrollRequest(this, null);
                    }
                }
                #endregion
            }
            else
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (DrawSelectRectangle)
                    {
                        //draw select rectangle
                        SelectRectanglex = DownPointAsViewPort.X - HscrollOffset;
                        SelectRectangley = DownPointAsViewPort.Y - VscrollOffset;
                        SelectRectanglex1 = e.X;
                        SelectRectangley1 = e.Y;
                        if (SelectRectanglex1 < SelectRectanglex)
                        {
                            SelectRectanglex = e.X;
                            SelectRectanglex1 = DownPointAsViewPort.X - HscrollOffset;
                        }
                        if (SelectRectangley1 < SelectRectangley)
                        {
                            SelectRectangley = e.Y;
                            SelectRectangley1 = DownPointAsViewPort.Y - VscrollOffset;
                        }
                        if (e.Y > this.Height)
                        {
                            for (int y = 0; y < 10; y++)
                                if (AdvanceVScrollRequest != null)
                                    AdvanceVScrollRequest(this, null);
                        }
                        if (e.Y < 0)
                        {
                            for (int y = 0; y < 10; y++)
                                if (ReverseVScrollRequest != null)
                                    ReverseVScrollRequest(this, null);
                        }

                        //select the items 
                        if (ModifierKeys != Keys.Control)
                        {
                            foreach (ManagedListViewItem item in items)
                                item.Selected = false;
                        }
                        bool itemSelected = false;
                        isSecectingItems = true;
                        int offset = VscrollOffset % (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        int vLines = this.Height / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                        int passedRows = VscrollOffset / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        int itemIndex = passedRows * hLines;
                        for (int i = VscrollOffset + SelectRectangley; i < VscrollOffset + SelectRectangley1; i++)
                        {
                            for (int j = HscrollOffset + SelectRectanglex; j < HscrollOffset + SelectRectanglex1; j++)
                            {
                                int recVlines = (i + offset) / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                                int recHlines = j / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                                if (recHlines < hLines)
                                {
                                    int indexAsrec = (recVlines * hLines) + recHlines;
                                    if (indexAsrec < items.Count)
                                    { items[indexAsrec].Selected = true; itemSelected = true; }
                                }
                                j += (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                            }
                            i += (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                        }
                        if (itemSelected)
                            if (SelectedIndexChanged != null)
                                SelectedIndexChanged(this, new EventArgs());
                    }
                    //drag and drop
                    if (AllowItemsDragAndDrop)
                    {
                        if (e.X > DownPoint.X + 3 || e.X < DownPoint.X - 3 || e.Y > DownPoint.Y + 3 | e.Y < DownPoint.Y - 3)
                        {
                            if (highlightItemAsOver)
                            {
                                if (overItemSelectedIndex >= 0)
                                {
                                    if (items[overItemSelectedIndex].Selected)
                                    {
                                        if (ItemsDrag != null)
                                            ItemsDrag(this, new EventArgs());
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Thumbnails view select item
                    int offset = VscrollOffset % (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                    int vLines = this.Height / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                    int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
                    int passedRows = VscrollOffset / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                    int itemIndex = passedRows * hLines;

                    int mouseVlines = (e.Y + offset) / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                    int mouseHlines = e.X / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);

                    int indexAsMouse = (mouseVlines * hLines) + mouseHlines;
                    if (indexAsMouse + itemIndex < items.Count)
                    {
                        if (e.X < hLines * (spaceBetweenItemsThunmbailsView + ThumbnailsWidth))
                        {
                            highlightItemAsOver = true;
                            overItemSelectedIndex = indexAsMouse + itemIndex;
                        }
                        else
                        {
                            highlightItemAsOver = false;
                            OldoverItemSelectedIndex = overItemSelectedIndex = -1;
                        }
                    }
                    else
                    {
                        highlightItemAsOver = false;
                        OldoverItemSelectedIndex = overItemSelectedIndex = -1;
                    }
                }
            }
            if (needToInvalidate)
                Invalidate();
        }
        /// <summary>
        /// Raise the mouse click event
        /// </summary>
        /// <param name="e"><see cref="MouseEventArgs"/></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (viewMode == ManagedListViewViewMode.Details)
            {
                #region item select
                if (e.Y > columnHeight)
                {
                    if ((e.X < buffered_columnsSize - HscrollOffset))
                    {
                        if (e.Button == System.Windows.Forms.MouseButtons.Left && !isSecectingItems && overItemSelectedIndex >= 0)
                        {
                            bool currentItemStatus = items[overItemSelectedIndex].Selected;
                            bool isShiftSelection = false;
                            if (ModifierKeys == Keys.Shift)
                            {
                                isShiftSelection = true;
                                if (LatestOverItemSelectedIndex == -1)
                                    isShiftSelection = false;
                            }
                            else if (ModifierKeys != Keys.Control)
                            {
                                LatestOverItemSelectedIndex = -1;
                                foreach (ManagedListViewItem item in items)
                                    item.Selected = false;
                            }

                            if (highlightItemAsOver)
                            {
                                if (overItemSelectedIndex >= 0)
                                {
                                    if (!isShiftSelection)
                                    {
                                        items[overItemSelectedIndex].Selected = true;
                                        LatestOverItemSelectedIndex = overItemSelectedIndex;
                                        if (SelectedIndexChanged != null && !currentItemStatus)
                                            SelectedIndexChanged(this, new EventArgs());

                                        int cX = 0;
                                        int x = 0;
                                        int i = 0;
                                        foreach (ManagedListViewColumn column in columns)
                                        {
                                            cX += column.Width;
                                            if (cX >= HscrollOffset)
                                            {
                                                if (cX > e.X + HscrollOffset)
                                                {
                                                    ManagedListViewSubItem sub = items[overItemSelectedIndex].GetSubItemByID(column.ID);
                                                    if (sub != null)
                                                    {
                                                        sub.OnMouseClick(new Point(e.X - (x - HscrollOffset), 0),
                                                            TextRenderer.MeasureText("TEST", this.Font), overItemSelectedIndex);
                                                    }
                                                    break;
                                                }
                                            }
                                            x += column.Width;
                                            i++;
                                            if (x - HscrollOffset > this.Width)
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        foreach (ManagedListViewItem item in items)
                                            item.Selected = false;
                                        if (overItemSelectedIndex > LatestOverItemSelectedIndex)
                                        {
                                            for (int j = LatestOverItemSelectedIndex; j < overItemSelectedIndex + 1; j++)
                                            {
                                                items[j].Selected = true;
                                            }
                                            if (SelectedIndexChanged != null)
                                                SelectedIndexChanged(this, new EventArgs());
                                        }
                                        else if (overItemSelectedIndex < LatestOverItemSelectedIndex)
                                        {
                                            for (int j = overItemSelectedIndex; j < LatestOverItemSelectedIndex + 1; j++)
                                            {
                                                items[j].Selected = true;
                                            }
                                            if (SelectedIndexChanged != null)
                                                SelectedIndexChanged(this, new EventArgs());
                                        }
                                    }
                                }

                            }
                        }
                        if (e.Button == System.Windows.Forms.MouseButtons.Left && !isSecectingItems && overItemSelectedIndex == -1)
                        {
                            LatestOverItemSelectedIndex = -1;
                            bool raiseEvent = false;
                            foreach (ManagedListViewItem item in items)
                            {
                                if (item.Selected)
                                {
                                    if (!raiseEvent) raiseEvent = true;
                                }
                                item.Selected = false;
                            }
                            if (raiseEvent)
                            {
                                if (SelectedIndexChanged != null)
                                    SelectedIndexChanged(this, new EventArgs());
                            }
                        }
                    }
                    else
                    {
                        if (!isSecectingItems)
                        {
                            bool raiseEvent = false;
                            foreach (ManagedListViewItem item in items)
                            {
                                if (item.Selected)
                                {
                                    if (!raiseEvent) raiseEvent = true;
                                }
                                item.Selected = false;
                            }
                            if (raiseEvent)
                            {
                                if (SelectedIndexChanged != null)
                                    SelectedIndexChanged(this, new EventArgs());
                            }
                        }
                    }
                }
                #endregion
                #region Column click
                if (e.Y < columnHeight)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (moveType == ManagedListViewMoveType.Column && selectedColumnIndex >= 0 && !isMovingColumn)
                        {
                            if (ChangeColumnSortModeWhenClick)
                            {
                                switch (columns[selectedColumnIndex].SortMode)
                                {
                                    case ManagedListViewSortMode.None: columns[selectedColumnIndex].SortMode = ManagedListViewSortMode.AtoZ; break;
                                    case ManagedListViewSortMode.AtoZ: columns[selectedColumnIndex].SortMode = ManagedListViewSortMode.ZtoA; break;
                                    case ManagedListViewSortMode.ZtoA: columns[selectedColumnIndex].SortMode = ManagedListViewSortMode.None; break;
                                }
                            }
                            if (ColumnClicked != null)
                                ColumnClicked(this, new ManagedListViewColumnClickArgs(columns[selectedColumnIndex].ID));
                        }
                    }
                }
                #endregion
            }
            else
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && !isSecectingItems && overItemSelectedIndex >= 0 && items.Count > 0)
                {
                    bool currentItemStatus = items[overItemSelectedIndex].Selected;
                    bool isShiftSelection = false;
                    if (ModifierKeys == Keys.Shift)
                    {
                        isShiftSelection = true;
                        if (LatestOverItemSelectedIndex == -1)
                            isShiftSelection = false;
                    }
                    else if (ModifierKeys != Keys.Control)
                    {
                        LatestOverItemSelectedIndex = -1;
                        foreach (ManagedListViewItem item in items)
                            item.Selected = false;
                    }

                    if (highlightItemAsOver)
                    {
                        if (overItemSelectedIndex >= 0)
                        {
                            if (!isShiftSelection)
                            {
                                items[overItemSelectedIndex].Selected = true;
                                LatestOverItemSelectedIndex = overItemSelectedIndex;
                                if (SelectedIndexChanged != null && !currentItemStatus)
                                    SelectedIndexChanged(this, new EventArgs());
                            }
                            else
                            {
                                foreach (ManagedListViewItem item in items)
                                    item.Selected = false;
                                if (overItemSelectedIndex > LatestOverItemSelectedIndex)
                                {
                                    for (int j = LatestOverItemSelectedIndex; j < overItemSelectedIndex + 1; j++)
                                    {
                                        items[j].Selected = true;
                                    }
                                    if (SelectedIndexChanged != null)
                                        SelectedIndexChanged(this, new EventArgs());
                                }
                                else if (overItemSelectedIndex < LatestOverItemSelectedIndex)
                                {
                                    for (int j = overItemSelectedIndex; j < LatestOverItemSelectedIndex + 1; j++)
                                    {
                                        items[j].Selected = true;
                                    }
                                    if (SelectedIndexChanged != null)
                                        SelectedIndexChanged(this, new EventArgs());
                                }
                            }
                        }

                    }
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Left && !isSecectingItems && overItemSelectedIndex == -1 && items.Count > 0)
                {
                    LatestOverItemSelectedIndex = -1;
                    bool raiseEvent = false;
                    foreach (ManagedListViewItem item in items)
                    {
                        if (item.Selected)
                        {
                            if (!raiseEvent) raiseEvent = true;
                        }
                        item.Selected = false;
                    }
                    if (raiseEvent)
                    {
                        if (SelectedIndexChanged != null)
                            SelectedIndexChanged(this, new EventArgs());
                    }
                }
            }
            if (!Focused)
            {
                base.Focus();
            }
            isSecectingItems = false;
            Invalidate();
        }
        /// <summary>
        /// Raise the mouse wheel event
        /// </summary>
        /// <param name="e"><see cref="MouseEventArgs"/></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (ReverseVScrollRequest != null)
                    ReverseVScrollRequest(this, null);
            }
            if (e.Delta < 0)
            {
                if (AdvanceVScrollRequest != null)
                    AdvanceVScrollRequest(this, null);
            }
            base.OnMouseWheel(e);
            Invalidate();
        }
        /// <summary>
        /// Raise the mouse double click event
        /// </summary>
        /// <param name="e"><see cref="MouseEventArgs"/></param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.Y > columnHeight)
            {
                if (ItemDoubleClick != null)
                    ItemDoubleClick(this, new ManagedListViewItemDoubleClickArgs(overItemSelectedIndex));
            }
        }
        /// <summary>
        /// Raise the mouse leave event
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (highlightItemAsOver)
            {
                highlightItemAsOver = false;
                if (overItemSelectedIndex < items.Count && overItemSelectedIndex >= 0)
                    if (OldoverItemSelectedIndex >= 0 && OldoverItemSelectedIndex < items.Count)
                        if (items[OldoverItemSelectedIndex] != null)
                            items[OldoverItemSelectedIndex].OnMouseLeave();
                OldoverItemSelectedIndex = overItemSelectedIndex = -1;
                Invalidate();
            }
        }
        /// <summary>
        /// On enter
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            OnRefreshScrollBars();
        }
        /// <summary>
        /// Get or set the background image
        /// </summary>
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
                CalculateBackgroundBounds();
            }
        }
        /// <summary>
        /// Specifies how the background get rendered.
        /// </summary>
        public ManagedListViewBackgroundRenderMode BackgroundRenderMode
        { get; set; }

        public void CalculateBackgroundBounds()
        {
            if (this.BackgroundImage != null)
            {
                switch (BackgroundRenderMode)
                {
                    case ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio:// Stretch image without aspect ratio, always..
                        {
                            backgroundDrawX = backgroundDrawY = 0;
                            backgroundDrawH = this.Height;
                            backgroundDrawW = this.Width;
                            break;
                        }
                    case ManagedListViewBackgroundRenderMode.StretchWithAspectRatioIfLarger:
                        {
                            CalculateBKGStretchedImageValues();
                            CenterBKGImage();
                            break;
                        }
                    case ManagedListViewBackgroundRenderMode.StretchWithAspectRatioToFit:
                        {
                            CalculateBKGStretchToFitImageValues();
                            CenterBKGImage();
                            break;
                        }
                }
                backgroundThumbnail = this.BackgroundImage.GetThumbnailImage(backgroundDrawW, backgroundDrawH, null, IntPtr.Zero);
            }
            else
                backgroundThumbnail = null;
        }
        /// <summary>
        /// On resize
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalculateBackgroundBounds();
        }

        private void DrawDetailsView(PaintEventArgs pe)
        {
            if (columns == null) return;
            int cX = 0;
            int x = 0;
            int i = 0;
            //get default size of word
            Size CharSize = TextRenderer.MeasureText("TEST", this.Font);
            columnHeight = CharSize.Height + columnh;
            int columnTextOffset = columnh / 2;
            itemHeight = CharSize.Height + itemh;
            int itemTextOffset = itemh / 2;
            int lines = (this.Height / itemHeight) + 2;
            bool resetX = false;
            int offset = VscrollOffset % itemHeight;

            foreach (ManagedListViewColumn column in columns)
            {
                cX += column.Width;

                if (cX >= HscrollOffset)
                {
                    if (items != null)
                    {
                        int lineIndex = VscrollOffset / itemHeight;

                        //draw sub items related to this column
                        for (int j = 0; j < lines; j++)
                        {
                            if (lineIndex < items.Count)
                            {
                                if (resetX)
                                {
                                    resetX = false;
                                    x = 0;
                                }
                                if (x == 0)
                                {
                                    if (items[lineIndex].IsChildItem)
                                    {
                                        x = 15;
                                        resetX = true;
                                    }
                                }
                                //clear
                                if (backgroundThumbnail == null)
                                {
                                    pe.Graphics.FillRectangle(new SolidBrush(base.BackColor),
                                       new Rectangle(x - HscrollOffset + 1, (j * itemHeight) - offset + columnHeight + 1,
                                           column.Width - 1, itemHeight));
                                }
                                if (items[lineIndex].IsSpecialItem)
                                {
                                    pe.Graphics.FillRectangle(new SolidBrush(ItemSpecialColor),
                                    new Rectangle(x - HscrollOffset + 1, (j * itemHeight) - offset + columnHeight + 1,
                                        column.Width - 1, itemHeight));
                                }
                                if (items[lineIndex].Selected)
                                {
                                    pe.Graphics.FillRectangle(new SolidBrush(ItemHighlightColor),
                                      new Rectangle(x - HscrollOffset + 1, (j * itemHeight) - offset + columnHeight + 1,
                                          column.Width - 1, itemHeight));
                                }
                                else
                                {
                                    if (highlightItemAsOver)
                                    {
                                        if (lineIndex == overItemSelectedIndex)
                                        {
                                            if (DrawHighlight)
                                                pe.Graphics.FillRectangle(new SolidBrush(ItemMouseOverColor),
                                                new Rectangle(x - HscrollOffset + 1, (j * itemHeight) - offset + columnHeight + 1,
                                                    column.Width - 1, itemHeight));
                                        }
                                    }
                                }
                                if (!items[lineIndex].IsSubitemsReady)
                                {
                                    // Raise the event
                                    if (FillSubitemsRequest != null)
                                    {
                                        FillSubitemsRequest(this, new ManagedListViewItemSelectArgs(lineIndex));
                                    }
                                }
                                if (!items[lineIndex].IsSubitemsReady)
                                    continue;// Still not ready. ..

                                ManagedListViewSubItem subitem = items[lineIndex].GetSubItemByID(column.ID);
                                if (subitem != null)
                                {
                                    Color drawColor = subitem.Color;
                                    Font drawFont = subitem.CustomFontEnabled ? subitem.CustomFont : this.Font;

                                    if (subitem.GetType() == typeof(ManagedListViewRatingSubItem))
                                    {
                                        ((ManagedListViewRatingSubItem)subitem).OnRefreshRating(lineIndex, itemHeight);
                                        Image img = Properties.Resources.noneRating;
                                        if (!((ManagedListViewRatingSubItem)subitem).DrawOverImage)
                                        {
                                            switch (((ManagedListViewRatingSubItem)subitem).Rating)
                                            {
                                                case 1: img = Properties.Resources.star_1; break;
                                                case 2: img = Properties.Resources.star_2; break;
                                                case 3: img = Properties.Resources.star_3; break;
                                                case 4: img = Properties.Resources.star_4; break;
                                                case 5: img = Properties.Resources.star_5; break;
                                            }
                                        }
                                        else
                                        {
                                            switch (((ManagedListViewRatingSubItem)subitem).OverRating)
                                            {
                                                case 1: img = Properties.Resources.star_1; break;
                                                case 2: img = Properties.Resources.star_2; break;
                                                case 3: img = Properties.Resources.star_3; break;
                                                case 4: img = Properties.Resources.star_4; break;
                                                case 5: img = Properties.Resources.star_5; break;
                                            }
                                        }
                                        img = img.GetThumbnailImage(itemHeight * 4, itemHeight - 1, null, IntPtr.Zero);
                                        int imgWidth = itemHeight * 4;
                                        if (imgWidth > column.Width)
                                            imgWidth = column.Width;
                                        pe.Graphics.DrawImageUnscaledAndClipped(img,
                                        new Rectangle(x - HscrollOffset + 2, (j * itemHeight) - offset + columnHeight + 1,
                                        imgWidth, itemHeight - 1));

                                        ((ManagedListViewRatingSubItem)subitem).DrawOverImage = false;
                                    }
                                    else
                                    {
                                        switch (subitem.DrawMode)
                                        {
                                            case ManagedListViewItemDrawMode.Text:
                                                pe.Graphics.DrawString(subitem.Text, drawFont, new SolidBrush(drawColor),
                                                    new Rectangle(x - HscrollOffset + 2,
                                                         (j * itemHeight) - offset + columnHeight + itemTextOffset,
                                                        column.Width, CharSize.Height), _StringFormat);
                                                break;
                                            case ManagedListViewItemDrawMode.Image:
                                                if (subitem.ImageIndex < ImagesList.Images.Count)
                                                {
                                                    pe.Graphics.DrawImage(ImagesList.Images[subitem.ImageIndex],
                                                        new Rectangle(x - HscrollOffset + 2, (j * itemHeight) - offset + columnHeight + 1,
                                                            itemHeight - 1, itemHeight - 1));
                                                }
                                                break;
                                            case ManagedListViewItemDrawMode.TextAndImage:
                                                int plus = 2;
                                                if (subitem.ImageIndex < ImagesList.Images.Count)
                                                {
                                                    pe.Graphics.DrawImage(ImagesList.Images[subitem.ImageIndex],
                                                      new Rectangle(x - HscrollOffset + 2, (j * itemHeight) - offset + columnHeight + 1,
                                                          itemHeight - 1, itemHeight - 1));
                                                    plus += itemHeight;
                                                }
                                                pe.Graphics.DrawString(subitem.Text, drawFont, new SolidBrush(drawColor),
                                                    new Rectangle(x - HscrollOffset + 2 + plus,
                                                         (j * itemHeight) - offset + columnHeight + itemTextOffset,
                                                        column.Width - plus, CharSize.Height), _StringFormat);
                                                break;
                                            case ManagedListViewItemDrawMode.UserDraw:    //Raise the event
                                                if (DrawSubItem != null)
                                                {
                                                    ManagedListViewSubItemDrawArgs args = new ManagedListViewSubItemDrawArgs(column.ID, lineIndex, items[lineIndex]);
                                                    DrawSubItem(this, args);
                                                    int p = 2;
                                                    if (args.ImageToDraw != null)
                                                    {
                                                        pe.Graphics.DrawImage(args.ImageToDraw,
                                                            new Rectangle(x - HscrollOffset + 2, (j * itemHeight) - offset + columnHeight + 1
                                                                , itemHeight - 1, itemHeight - 1));
                                                        p += itemHeight;
                                                    }
                                                    pe.Graphics.DrawString(args.TextToDraw, drawFont, new SolidBrush(drawColor),
                                                    new Rectangle(x - HscrollOffset + 2 + p,
                                                         (j * itemHeight) - offset + columnHeight + itemTextOffset,
                                                        column.Width - p, CharSize.Height), _StringFormat);
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                            lineIndex++;
                        }
                        if (resetX)
                        {
                            resetX = false;
                            x = 0;
                        }
                    }
                    //draw the column rectangle, draw the column after the item to hide the offset
                    Color Hcolor = ColumnColor;
                    if (highlightSelectedColumn && selectedColumnIndex == i)
                    {
                        if (!isMouseDown)
                            Hcolor = ColumnHighlightColor;
                        else
                        {
                            if (moveType != ManagedListViewMoveType.ColumnVLine)
                                Hcolor = ColumnClickColor;
                            else
                                Hcolor = ColumnHighlightColor;
                        }
                    }
                    //DRAW COLUMN
                    pe.Graphics.FillRectangle(new LinearGradientBrush(new Point(), new Point(0, columnHeight), Hcolor, Color.White),
                            new Rectangle(x - HscrollOffset + 1, 1, column.Width, columnHeight));
                    //draw the column line
                    pe.Graphics.DrawLine(new Pen(Brushes.LightGray), new Point(cX - HscrollOffset, 1),
                        new Point(cX - HscrollOffset, this.Height - 1));
                    //draw the column text
                    pe.Graphics.DrawString(column.HeaderText, this.Font,
                        new SolidBrush(column.HeaderTextColor), new Rectangle(x - HscrollOffset + 2, columnTextOffset, column.Width,
                            columnHeight), _StringFormat);
                    //Raise the event
                    if (DrawColumn != null)
                        DrawColumn(this, new ManagedListViewColumnDrawArgs(column.ID, pe.Graphics,
                            new Rectangle(x - HscrollOffset, 2, column.Width, columnHeight)));
                    //draw sort triangle
                    switch (column.SortMode)
                    {
                        case ManagedListViewSortMode.AtoZ:
                            pe.Graphics.DrawImage(Properties.Resources.SortAlpha.ToBitmap(),
                                new Rectangle(x - HscrollOffset + column.Width - 14, 2, 12, 16));
                            break;
                        case ManagedListViewSortMode.ZtoA:
                            pe.Graphics.DrawImage(Properties.Resources.SortZ.ToBitmap(),
                                new Rectangle(x - HscrollOffset + column.Width - 14, 2, 12, 16));
                            break;
                    }
                }
                x += column.Width;
                i++;
                if (x - HscrollOffset > this.Width)
                    break;
            }
            if (columns.Count > 0)
                pe.Graphics.DrawLine(new Pen(new SolidBrush(Color.Gray)), new Point(0, columnHeight), new Point(this.Width, columnHeight));
            if (isMovingColumn)
            {
                pe.Graphics.FillRectangle(new LinearGradientBrush(new Point(), new Point(0, columnHeight), Color.White, Color.Silver),
                           new Rectangle(CurrentMousePosition.X, 1, columns[selectedColumnIndex].Width, columnHeight));
                //draw the column line
                pe.Graphics.DrawLine(new Pen(Brushes.LightGray), new Point(cX - HscrollOffset, 1),
                    new Point(cX - HscrollOffset, this.Height - 1));
                //draw the column text
                pe.Graphics.DrawString(columns[selectedColumnIndex].HeaderText, this.Font,
                    new SolidBrush(columns[selectedColumnIndex].HeaderTextColor), new Rectangle(CurrentMousePosition.X, 2,
                        columns[selectedColumnIndex].Width, columnHeight), _StringFormat);
            }
        }
        private void DrawThumbailsView(PaintEventArgs pe)
        {
            thumb_tooltip = null;
            Size CharSize = TextRenderer.MeasureText("TEST", this.Font);
            itemTextHeight = CharSize.Height * 2;
            string tooltip_text = "";
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            //format.LineAlignment = StringAlignment.Center;
            format.Trimming = StringTrimming.EllipsisCharacter;
            int vLines = this.Height / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
            int hLines = this.Width / (spaceBetweenItemsThunmbailsView + ThumbnailsWidth);
            if (hLines == 0) hLines = 1;
            int passedRows = VscrollOffset / (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
            int itemIndex = passedRows * hLines;
            if (itemIndex >= items.Count)
                return;
            int y = 2;
            for (int i = 0; i < vLines + 2; i++)
            //for (int i = vLines + 1; i >= 0; i--)
            {
                int x = spaceBetweenItemsThunmbailsView;
                for (int j = 0; j < hLines; j++)
                //for (int j = hLines - 1; j >= 0; j--)
                {
                    int offset = VscrollOffset % (spaceBetweenItemsThunmbailsView + ThumbnailsHeight + itemTextHeight);
                    if (highlightItemAsOver)
                    {
                        if (itemIndex == overItemSelectedIndex)
                        {
                            pe.Graphics.FillRectangle(new SolidBrush(ItemMouseOverColor),
                                new Rectangle(x - 2, y - offset - 2, ThumbnailsWidth + 4, ThumbnailsHeight + itemTextHeight + 4));
                        }
                    }
                    if (items[itemIndex].Selected)
                        pe.Graphics.FillRectangle(new SolidBrush(ItemHighlightColor),
                            new Rectangle(x - 2, y - offset - 2, ThumbnailsWidth + 4, ThumbnailsHeight + itemTextHeight + 4));


                    string textToDraw = "";
                    Image imageToDraw = null;

                    switch (items[itemIndex].DrawMode)
                    {
                        case ManagedListViewItemDrawMode.Text:
                        case ManagedListViewItemDrawMode.Image:
                        case ManagedListViewItemDrawMode.TextAndImage:
                            if (items[itemIndex].ImageIndex < ImagesList.Images.Count)
                            {
                                imageToDraw = ImagesList.Images[items[itemIndex].ImageIndex];
                            }
                            textToDraw = items[itemIndex].Text;
                            break;

                        case ManagedListViewItemDrawMode.UserDraw:
                            ManagedListViewItemDrawArgs args = new ManagedListViewItemDrawArgs(itemIndex);
                            if (DrawItem != null)
                                DrawItem(this, args);
                            imageToDraw = args.ImageToDraw;
                            textToDraw = args.TextToDraw;
                            break;
                    }
                    tooltip_text = textToDraw;
                    // Draw image
                    if (imageToDraw != null)
                    {
                        Size siz = ThumbnailsKeepImageSize ?
                            CalculateStretchImageValuesStretch(imageToDraw.Width, imageToDraw.Height) :
                            CalculateStretchImageValues(imageToDraw.Width, imageToDraw.Height);

                        int imgX = x + (ThumbnailsWidth / 2) - (siz.Width / 2);
                        int imgY = (y - offset) + (ThumbnailsHeight / 2) - (siz.Height / 2);
                        pe.Graphics.DrawImage(imageToDraw, new Rectangle(imgX, imgY, siz.Width, siz.Height));
                    }
                    // Draw text
                    pe.Graphics.DrawString(textToDraw, this.Font, new SolidBrush(items[itemIndex].Color),
                    new Rectangle(x, y + ThumbnailsHeight + 1 - offset, ThumbnailsWidth, itemTextHeight), format);
                    // Show tooltip if the item is highlighted
                    if (itemIndex == overItemSelectedIndex)
                    {
                        Size text_size = TextRenderer.MeasureText(tooltip_text, this.Font);
                        int w = text_size.Width + 8;
                        if (w >= this.Width)
                            w = this.Width - 1;

                        if (ShowItemInfoOnThumbnailMode)
                        {
                            thumb_tooltip = new ManagedListViewShowThumbnailTooltipArgs(itemIndex, tooltip_text,
                                            new Rectangle(x, y + ThumbnailsHeight + 1 - offset, w, text_size.Height + 1));
                            if (ShowThumbnailInfoRequest != null)
                            {
                                ShowThumbnailInfoRequest(this, thumb_tooltip);
                                tooltip_text = thumb_tooltip.TextToShow;
                                int t_rating = thumb_tooltip.Rating;
                                text_size = TextRenderer.MeasureText(tooltip_text, this.Font);
                                w = text_size.Width + 8;
                                if (w >= this.Width)
                                    w = this.Width - 1;
                                thumb_tooltip = new ManagedListViewShowThumbnailTooltipArgs(itemIndex, tooltip_text, t_rating,
                                           new Rectangle(x, y + ThumbnailsHeight + 1 - offset, w, text_size.Height + 1));
                            }
                        }

                        if (ShowThumbnailTooltipRequest != null)
                        {
                            ShowThumbnailTooltipRequest(this,
                                new ManagedListViewShowThumbnailTooltipArgs(itemIndex, tooltip_text,
                                new Rectangle(x, y + ThumbnailsHeight + 1 - offset, text_size.Width, itemTextHeight)));
                        }
                    }

                    // advance
                    x += ThumbnailsWidth + spaceBetweenItemsThunmbailsView;
                    itemIndex++;
                    if (itemIndex == items.Count)
                        break;
                }
                y += ThumbnailsHeight + itemTextHeight + spaceBetweenItemsThunmbailsView;
                if (itemIndex == items.Count)
                    break;
            }
            if (thumb_tooltip != null)
            {
                if (ShowItemInfoOnThumbnailMode)
                {
                    int t_w = thumb_tooltip.TextThumbnailRectangle.Width;
                    int t_h = thumb_tooltip.TextThumbnailRectangle.Height;
                    if (thumb_tooltip.Rating != -1)
                        t_h += 20;
                    int t_x = 1;
                    int t_y = this.Height - t_h + 1;

                    pe.Graphics.FillRectangle(new SolidBrush(ItemMouseOverColor), t_x, t_y, t_w, t_h);
                    pe.Graphics.DrawRectangle(Pens.Gray, t_x, t_y, t_w, t_h - 2);
                    format.Alignment = StringAlignment.Near;
                    pe.Graphics.DrawString(thumb_tooltip.TextToShow, this.Font, new SolidBrush(Color.Black),
                    new Rectangle(t_x + 1, t_y, t_w, (thumb_tooltip.Rating != -1) ? (t_h - 20) : t_h), format);
                    if (thumb_tooltip.Rating != -1)
                    {
                        switch (thumb_tooltip.Rating)
                        {
                            case 0: pe.Graphics.DrawImage(Properties.Resources.noneRating, t_x + 1, this.Height - 20, 58, 18); break;
                            case 1: pe.Graphics.DrawImage(Properties.Resources.star_1, t_x + 1, this.Height - 20, 58, 18); break;
                            case 2: pe.Graphics.DrawImage(Properties.Resources.star_2, t_x + 1, this.Height - 20, 58, 18); break;
                            case 3: pe.Graphics.DrawImage(Properties.Resources.star_3, t_x + 1, this.Height - 20, 58, 18); break;
                            case 4: pe.Graphics.DrawImage(Properties.Resources.star_4, t_x + 1, this.Height - 20, 58, 18); break;
                            case 5: pe.Graphics.DrawImage(Properties.Resources.star_5, t_x + 1, this.Height - 20, 58, 18); break;
                        }
                    }
                }
            }
            else
            {
                if (HideThumbnailTooltipRequest != null)
                {
                    HideThumbnailTooltipRequest(this, new EventArgs());
                }
            }
        }
        private Size CalculateStretchImageValues(int imgW, int imgH)
        {
            float pRatio = (float)ThumbnailsWidth / ThumbnailsHeight;
            float imRatio = (float)imgW / imgH;
            int viewImageWidth = 0;
            int viewImageHeight = 0;

            if (ThumbnailsWidth >= imgW && ThumbnailsHeight >= imgH)
            {
                viewImageWidth = imgW;
                viewImageHeight = imgH;
            }
            else if (ThumbnailsWidth > imgW && ThumbnailsHeight < imgH)
            {
                viewImageHeight = ThumbnailsHeight;
                viewImageWidth = (int)(ThumbnailsHeight * imRatio);
            }
            else if (ThumbnailsWidth < imgW && ThumbnailsHeight > imgH)
            {
                viewImageWidth = ThumbnailsWidth;
                viewImageHeight = (int)(ThumbnailsWidth / imRatio);
            }
            else if (ThumbnailsWidth < imgW && ThumbnailsHeight < imgH)
            {
                if (ThumbnailsWidth >= ThumbnailsHeight)
                {
                    //width image
                    if (imgW >= imgH && imRatio >= pRatio)
                    {
                        viewImageWidth = ThumbnailsWidth;
                        viewImageHeight = (int)(ThumbnailsWidth / imRatio);
                    }
                    else
                    {
                        viewImageHeight = ThumbnailsHeight;
                        viewImageWidth = (int)(ThumbnailsHeight * imRatio);
                    }
                }
                else
                {
                    if (imgW < imgH && imRatio < pRatio)
                    {
                        viewImageHeight = ThumbnailsHeight;
                        viewImageWidth = (int)(ThumbnailsHeight * imRatio);
                    }
                    else
                    {
                        viewImageWidth = ThumbnailsWidth;
                        viewImageHeight = (int)(ThumbnailsWidth / imRatio);
                    }
                }
            }

            return new Size(viewImageWidth, viewImageHeight);
        }
        private Size CalculateStretchImageValuesStretch(int imgW, int imgH)
        {
            float pRatio = (float)ThumbnailsWidth / ThumbnailsHeight;
            float imRatio = (float)imgW / imgH;
            int viewImageWidth = 0;
            int viewImageHeight = 0;

            if (ThumbnailsWidth >= imgW && ThumbnailsHeight >= imgH)
            {
                if (ThumbnailsWidth >= ThumbnailsHeight)
                {
                    //width image
                    if (imgW >= imgH && imRatio >= pRatio)
                    {
                        viewImageWidth = ThumbnailsWidth;
                        viewImageHeight = (int)(ThumbnailsWidth / imRatio);
                    }
                    else
                    {
                        viewImageHeight = ThumbnailsHeight;
                        viewImageWidth = (int)(ThumbnailsWidth * imRatio);
                    }
                }
                else
                {
                    if (imgW < imgH && imRatio < pRatio)
                    {
                        viewImageHeight = ThumbnailsHeight;
                        viewImageWidth = (int)(ThumbnailsHeight * imRatio);
                    }
                    else
                    {
                        viewImageWidth = ThumbnailsWidth;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                }
            }
            else if (ThumbnailsWidth > imgW && ThumbnailsHeight < imgH)
            {
                viewImageHeight = ThumbnailsHeight;
                viewImageWidth = (int)(ThumbnailsHeight * imRatio);
            }
            else if (ThumbnailsWidth < imgW && ThumbnailsHeight > imgH)
            {
                viewImageWidth = ThumbnailsWidth;
                viewImageHeight = (int)(ThumbnailsWidth / imRatio);
            }
            else if (ThumbnailsWidth < imgW && ThumbnailsHeight < imgH)
            {
                if (ThumbnailsWidth >= ThumbnailsHeight)
                {
                    //width image
                    if (imgW >= imgH && imRatio >= pRatio)
                    {
                        viewImageWidth = ThumbnailsWidth;
                        viewImageHeight = (int)(ThumbnailsWidth / imRatio);
                    }
                    else
                    {
                        viewImageHeight = ThumbnailsHeight;
                        viewImageWidth = (int)(ThumbnailsHeight * imRatio);
                    }
                }
                else
                {
                    if (imgW < imgH && imRatio < pRatio)
                    {
                        viewImageHeight = ThumbnailsHeight;
                        viewImageWidth = (int)(ThumbnailsHeight * imRatio);
                    }
                    else
                    {
                        viewImageWidth = ThumbnailsWidth;
                        viewImageHeight = (int)(ThumbnailsWidth / imRatio);
                    }
                }
            }
            return new Size(viewImageWidth, viewImageHeight);
        }

        private void CalculateBKGStretchedImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)this.BackgroundImage.Width / this.BackgroundImage.Height;

            if (this.Width >= this.BackgroundImage.Width && this.Height >= this.BackgroundImage.Height)
            {
                backgroundDrawW = BackgroundImage.Width;
                backgroundDrawH = BackgroundImage.Height;
            }
            else if (this.Width > BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                backgroundDrawH = this.Height;
                backgroundDrawW = (int)(this.Height * imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height > BackgroundImage.Height)
            {
                backgroundDrawW = this.Width;
                backgroundDrawH = (int)(this.Width / imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        backgroundDrawW = this.Width;
                        backgroundDrawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        backgroundDrawH = this.Height;
                        backgroundDrawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        backgroundDrawH = this.Height;
                        backgroundDrawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        backgroundDrawW = this.Width;
                        backgroundDrawH = (int)(this.Width / imRatio);
                    }
                }
            }
        }
        private void CalculateBKGStretchToFitImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)BackgroundImage.Width / BackgroundImage.Height;

            if (this.Width >= BackgroundImage.Width && this.Height >= BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        backgroundDrawW = this.Width;
                        backgroundDrawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        backgroundDrawH = this.Height;
                        backgroundDrawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        backgroundDrawH = this.Height;
                        backgroundDrawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        backgroundDrawW = this.Width;
                        backgroundDrawH = (int)(this.Width / imRatio);
                    }
                }
            }
            else if (this.Width > BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                backgroundDrawH = this.Height;
                backgroundDrawW = (int)(this.Height * imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height > BackgroundImage.Height)
            {
                backgroundDrawW = this.Width;
                backgroundDrawH = (int)(this.Width / imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        backgroundDrawW = this.Width;
                        backgroundDrawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        backgroundDrawH = this.Height;
                        backgroundDrawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        backgroundDrawH = this.Height;
                        backgroundDrawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        backgroundDrawW = this.Width;
                        backgroundDrawH = (int)(this.Width / imRatio);
                    }
                }
            }

        }
        private void CenterBKGImage()
        {
            backgroundDrawY = (int)((this.Height - backgroundDrawH) / 2.0);
            backgroundDrawX = (int)((this.Width - backgroundDrawW) / 2.0);
        }

        private void OnReverseHScrollRequest()
        {
            if (ReverseHScrollRequest != null)
                ReverseHScrollRequest(this, new EventArgs());
        }
        private void OnAdvanceHScrollRequest()
        {
            if (AdvanceHScrollRequest != null)
                AdvanceHScrollRequest(this, new EventArgs());
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 257)// Key up !
            {
                // Fix when the window is no longer receive keydown events.
                OnKeyDownRaised(new KeyEventArgs((Keys)m.WParam));
            }
        }
    }
}