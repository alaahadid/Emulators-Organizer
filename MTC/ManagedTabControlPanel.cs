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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTC
{
    /// <summary>
    /// Managed Tab Control Top Panel Control
    /// </summary>
    public partial class ManagedTabControlPanel : Control
    {
        /// <summary>
        /// Managed Tab Control Top Panel Control
        /// </summary>
        public ManagedTabControlPanel()
        {
            InitializeComponent();
            TabPages = new MTCTabPagesCollection(this);
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);
            CalculateDrawViraibles();
            _StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            _StringFormat.Trimming = StringTrimming.EllipsisCharacter;
        }
        private StringFormat _StringFormat;
        private const int tabxSize = 14;
        private const int defaultImageSize = 16;
        private const int pageLineY = 1;
        private Size charSize;
        private int tabTextOffset = 2;
        private int highlightedTabPageIndex = -1;
        private bool highlightCloseButton = false;
        private int currentTabPageThatShowToolTip = -1;
        private Point mouseDownPoint;
        private Point mouseDownPointAsViewPort;
        private bool movingLock = false;
       
        // Changeable variables
        /// <summary>
        /// Tab pages collection
        /// </summary>
        public MTCTabPagesCollection TabPages;
        /// <summary>
        /// The draw style
        /// </summary>
        public MTCDrawStyle drawStyle = MTCDrawStyle.Normal;
        /// <summary>
        /// The horizontal offset value.
        /// </summary>
        public int HScrollOffset = 0;
        /// <summary>
        /// Selected tab index
        /// </summary>
        public int SelectedTabPageIndex = 0;
        /// <summary>
        /// The tab page color
        /// </summary>
        public Color TabPageColor = Color.Silver;
        /// <summary>
        /// The tab page color when selected
        /// </summary>
        public Color TabPageSelectedColor = Color.SkyBlue;
        /// <summary>
        /// The tab page color when highlightes
        /// </summary>
        public Color TabPageHighlightedColor = Color.LightBlue;
        /// <summary>
        /// The tab pages spliter color
        /// </summary>
        public Color TabPageSplitColor = Color.Gray;
        /// <summary>
        /// Draw close button on each tab page ?
        /// </summary>
        public bool DrawCloseBoxOnEachPage = true;
        /// <summary>
        /// Tab pages can be highlighted or not
        /// </summary>
        public bool DrawTabPageHighlight = true;
        /// <summary>
        /// Allow tab page reorder
        /// </summary>
        public bool AllowTabPagesReorder = true;
        /// <summary>
        /// Tab page max width
        /// </summary>
        public int TabPageMaxWidth = 250;
        /// <summary>
        /// Allow auto tab page drag and drop
        /// </summary>
        public bool AllowAutoTabPageDragAndDrop = true;
        /// <summary>
        /// Allow tab page drag and drop
        /// </summary>
        public bool AllowTabPageDragAndDrop = true;
        /// <summary>
        /// The images list
        /// </summary>
        public ImageList ImagesList = new ImageList();
        /// <summary>
        /// Close box always visible
        /// </summary>
        public bool CloseBoxAlwaysVisible = false;
        /// <summary>
        /// If the control should show tool tip for highlighted tab pages always
        /// </summary>
        public bool AlwaysShowToolTip = false;
        /// <summary>
        /// If enabled, the control will select that added page automaticaly after adding it
        /// </summary>
        public bool AutoSelectAddedTabPage = false;

        // Events
        /// <summary>
        /// SelectedTabPageIndexChanged
        /// </summary>
        public event EventHandler SelectedTabPageIndexChanged;
        /// <summary>
        /// TabPageCloseRequest
        /// </summary>
        public event EventHandler<MTCTabPageCloseArgs> TabPageCloseRequest;
        /// <summary>
        /// RefreshScrollBar
        /// </summary>
        public event EventHandler RefreshScrollBar;
        /// <summary>
        /// ShowTabPageToolTipRequest
        /// </summary>
        public event EventHandler<MTCTabPageShowToolTipArgs> ShowTabPageToolTipRequest;
        /// <summary>
        /// ClearToolTip
        /// </summary>
        public event EventHandler ClearToolTip;
        /// <summary>
        /// AfterTabPageReorder
        /// </summary>
        public event EventHandler AfterTabPageReorder;
        /// <summary>
        /// TabPageDrag
        /// </summary>
        public event EventHandler<MTCTabPageDragArgs> TabPageDrag;
        /// <summary>
        /// ScrollToSelectedTabPage
        /// </summary>
        public event EventHandler ScrollToSelectedTabPage;

        public event EventHandler BeforeAutoDragAndDrop;
        public event EventHandler AfterAutoDragAndDrop;
        public event EventHandler TabPagesCleared;

        /// <summary>
        /// The font
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
                CalculateDrawViraibles();
            }
        }

        private void CalculateDrawViraibles()
        {
            charSize = TextRenderer.MeasureText("TEST", this.Font);
            tabTextOffset = (this.Height / 2) - (charSize.Height / 2);
        }
        /// <summary>
        /// Calculate a Tab Page width value.
        /// </summary>
        /// <param name="page">The <see cref="MTC.IMTCTabPage"/> to calculate value for</param>
        /// <returns>The width of that page otherwise 0 if unable to calculate</returns>
        public int CalculateTabPageWidth(MTCTabPage page)
        {
            MTCDrawPageObjects args = GetDrawObjects(page);
            int width = 0;
            if (args.Text.Length > 0)
            {
                width = TextRenderer.MeasureText(args.Text, this.Font).Width + 2;
            }
            else
            {
                width = 10;// Empty string
            }
            if (args.Image != null)
                width += defaultImageSize + 1;
            if (DrawCloseBoxOnEachPage)
            {
                width += tabxSize;
            }

            return width;
        }
        /// <summary>
        /// Calculate all tab pages width value
        /// </summary>
        /// <returns>The width of all pages otherwise 0 if unable to calculate</returns>
        public int CalculateTabPagesWidth()
        {
            int width = 0;
            if (TabPages != null)
            {
                foreach (MTCTabPage page in TabPages)
                {
                    int pw = CalculateTabPageWidth(page);
                    if (pw >= TabPageMaxWidth)
                    {
                        pw = TabPageMaxWidth;
                    }
                    width += pw;
                }
            }
            return width;
        }
        /// <summary>
        /// Get tab page index within TabPages collection
        /// </summary>
        /// <param name="xPos">The x coordinate value on view port</param>
        /// <returns>The tab page index value otherwise -1 if page not found</returns>
        public int GetTabPageIndex(int xPos)
        {
            if (xPos < 0)
                return -1;
            int cX = 0;
            int x = 0;
            int i = 0;
            foreach (MTCTabPage page in TabPages)
            {
                int width = CalculateTabPageWidth(page);
                if (width >= TabPageMaxWidth)
                {
                    width = TabPageMaxWidth;
                }
                cX += width;
                if (cX >= HScrollOffset)
                {
                    if (xPos >= (x - HScrollOffset) && xPos <= (cX - HScrollOffset) + 3)
                    {
                        return i;
                    }
                }
                x += width;
                i++;
                if (x - HScrollOffset > this.Width)
                    break;
            }
            return -1;
        }
        /// <summary>
        /// Get tab page x coordinate value on view port
        /// </summary>
        /// <param name="index">The tab page index within TabPages collection</param>
        /// <returns>The tab page x coordinate value on view port otherwise -1 if page not found</returns>
        public int GetTabPageXPos(int index)
        {
            if (index < 0)
                return -1;
            int cX = 0;
            int x = 0;
            int i = 0;
            foreach (MTCTabPage page in TabPages)
            {
                int width = CalculateTabPageWidth(page);
                if (width >= TabPageMaxWidth)
                {
                    width = TabPageMaxWidth;
                }
                cX += width;
                if (index == i)
                {
                    if (cX >= HScrollOffset)
                    {
                        return x - HScrollOffset;
                    }
                }
                x += width;
                i++;
                if (x - HScrollOffset > this.Width)
                    break;
            }
            return -1;
        }
        private MTCDrawPageObjects GetDrawObjects(MTCTabPage page)
        {
            MTCDrawPageObjects obj = new MTCDrawPageObjects();
            obj.Text = "";
            obj.Image = null;
            switch (page.DrawType)
            {
                case MTCTabPageDrawType.Text: obj.Text = page.Text; break;
                case MTCTabPageDrawType.Image:
                    if (ImagesList.Images.Count > 0)
                    {
                        if (page.ImageIndex < ImagesList.Images.Count)
                            obj.Image = ImagesList.Images[page.ImageIndex];
                    }
                    break;
                case MTCTabPageDrawType.TextAndImage:
                    obj.Text = page.Text;
                    if (ImagesList.Images.Count > 0)
                    {
                        if (page.ImageIndex < ImagesList.Images.Count)
                            obj.Image = ImagesList.Images[page.ImageIndex];
                    }
                    break;
            }
            return obj;
        }
        /// <summary>
        /// Indicate whether a tab is completely shown to the user
        /// </summary>
        /// <param name="index">The tab page index</param>
        /// <returns></returns>
        public bool IsTapPageShownCompletely(int index)
        {
            if (index < 0)
                return false;
            int cX = 0;
            int x = 0;
            int i = 0;
            foreach (MTCTabPage page in TabPages)
            {
                int width = CalculateTabPageWidth(page);
                if (width >= TabPageMaxWidth)
                {
                    width = TabPageMaxWidth;
                }
                cX += width;
                if (index == i)
                {
                    if (cX - HScrollOffset > this.Width || x - HScrollOffset < 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                x += width;
                i++;
                if (x - HScrollOffset > this.Width)
                    break;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (TabPages == null)
                return;

            int cX = 0;
            int x = 0;
            int i = 0;
            CalculateDrawViraibles();
            if (SelectedTabPageIndex >= 0 && TabPages.Count > 0)
            {
                pe.Graphics.DrawLine(new Pen(new SolidBrush(TabPageSplitColor)), new Point(0, this.Height - 2),
                   new Point(this.Width, this.Height - 2));
            }
            foreach (MTCTabPage page in TabPages)
            {
                int width = CalculateTabPageWidth(page);
                if (width >= TabPageMaxWidth)
                    width = TabPageMaxWidth;
                cX += width;
                if (cX >= HScrollOffset)
                {
                    // Draw page Rectangle
                    if (SelectedTabPageIndex == i)
                    {
                        pe.Graphics.FillRectangle(new SolidBrush(TabPageSelectedColor),
                                new Rectangle(x - HScrollOffset + 1, pageLineY, width, this.Height));
                    }
                    else
                    {
                        Color colorToDraw = TabPageColor;

                        if (highlightedTabPageIndex == i)
                        {
                            if (DrawTabPageHighlight)
                            {
                                // Draw highlight
                                colorToDraw = TabPageHighlightedColor;
                            }
                        }
                        Brush brush = null;
                        switch (drawStyle)
                        {
                            case MTCDrawStyle.Flat: brush = new SolidBrush(colorToDraw); break;
                            case MTCDrawStyle.Normal: brush = new LinearGradientBrush(new Point(), new Point(0, this.Height), Color.White, colorToDraw); break;
                            case MTCDrawStyle.BackwardDiagonal: brush = new HatchBrush(HatchStyle.BackwardDiagonal, colorToDraw, Color.White); break;
                            case MTCDrawStyle.DashedDownwardDiagonal: brush = new HatchBrush(HatchStyle.DashedDownwardDiagonal, colorToDraw, Color.White); break;
                            case MTCDrawStyle.Divot: brush = new HatchBrush(HatchStyle.Divot, colorToDraw, Color.White); break;
                            case MTCDrawStyle.DottedGrid: brush = new HatchBrush(HatchStyle.DottedGrid, colorToDraw, Color.White); break;
                            case MTCDrawStyle.HorizontalBrick: brush = new HatchBrush(HatchStyle.HorizontalBrick, colorToDraw, Color.White); break;
                            case MTCDrawStyle.NarrowHorizontal: brush = new HatchBrush(HatchStyle.NarrowHorizontal, colorToDraw, Color.White); break;
                            case MTCDrawStyle.NarrowVertical: brush = new HatchBrush(HatchStyle.NarrowVertical, colorToDraw, Color.White); break;
                            case MTCDrawStyle.Shingle: brush = new HatchBrush(HatchStyle.Shingle, colorToDraw, Color.White); break;
                            case MTCDrawStyle.Wave: brush = new HatchBrush(HatchStyle.Wave, colorToDraw, Color.White); break;
                            case MTCDrawStyle.ZigZag: brush = new HatchBrush(HatchStyle.ZigZag, colorToDraw, Color.White); break;
                        }
                        pe.Graphics.FillRectangle(brush, new Rectangle(x - HScrollOffset + 1, pageLineY, width, this.Height));
                    }
                    // Draw the page line (split line)
                    pe.Graphics.DrawLine(new Pen(new SolidBrush(TabPageSplitColor)), new Point(cX - HScrollOffset, pageLineY),
                        new Point(cX - HScrollOffset, this.Height));
                    pe.Graphics.DrawLine(new Pen(new SolidBrush(TabPageSplitColor)), new Point(x - HScrollOffset, pageLineY),
                      new Point(x - HScrollOffset, this.Height));
                    pe.Graphics.DrawLine(new Pen(new SolidBrush(TabPageSplitColor)), new Point(x - HScrollOffset, pageLineY),
                       new Point(cX - HScrollOffset, pageLineY));
                    // Draw text and image
                    MTCDrawPageObjects drawArgs = GetDrawObjects(page);
                    int textc = 0;
                    // Draw image if presented
                    if (drawArgs.Image != null)
                    {
                        pe.Graphics.DrawImage(drawArgs.Image, new Rectangle(x - HScrollOffset + 3, tabTextOffset,
                            defaultImageSize, defaultImageSize));
                        textc = defaultImageSize + 1;
                    }
                    // Draw the page text
                    if (drawArgs.Text.Length > 0)
                    {
                        int textWidth = width - (tabxSize + 3) - textc;
                        if (!DrawCloseBoxOnEachPage) 
                            textWidth = width - 3 - textc;
                        pe.Graphics.DrawString(drawArgs.Text, this.Font,
                            new SolidBrush(base.ForeColor), new Rectangle(x - HScrollOffset + 2 + textc, tabTextOffset,
                                textWidth + 2,
                                this.Height), _StringFormat);
                    }

                    // Draw close button
                    if (DrawCloseBoxOnEachPage)
                    {
                        Image closeButtonImage = null;
                        if (CloseBoxAlwaysVisible)
                            closeButtonImage = Properties.Resources.cross_black;

                        if (highlightedTabPageIndex == i)
                        {
                            if (DrawTabPageHighlight)
                            {
                                if (highlightCloseButton)
                                    closeButtonImage = Properties.Resources.cross;
                            }
                        }
                        if (closeButtonImage != null)
                            pe.Graphics.DrawImage(closeButtonImage, new Rectangle(cX - HScrollOffset - tabxSize, tabTextOffset+1, 12, 12));
                    }

                }

                x += width;
                i++;
                if (x - HScrollOffset > this.Width)
                    break;
            }
            // Draw vertical line on bottom when a page is selected
            if (SelectedTabPageIndex >= 0 && TabPages.Count > 0)
            {
                pe.Graphics.DrawLine(new Pen(new SolidBrush(TabPageSelectedColor), 1), new Point(0, this.Height - 1),
                   new Point(this.Width, this.Height - 1));
                pe.Graphics.DrawLine(new Pen(new SolidBrush(TabPageSplitColor)), new Point(this.Width - 1, this.Height - 2),
                  new Point(this.Width - 1, this.Height));
                pe.Graphics.DrawLine(new Pen(new SolidBrush(TabPageSplitColor)), new Point(0, pageLineY),
                 new Point(0, this.Height));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                #region Tab page reorder
                if (AllowTabPagesReorder)
                {
                    if (SelectedTabPageIndex == highlightedTabPageIndex)
                    {
                        if ((e.X - mouseDownPoint.X) > 3 || (e.X - mouseDownPoint.X) < 3)
                        {
                            movingLock = true;
                            int newIndex = GetTabPageIndex(e.X);
                            if (newIndex >= 0)
                            {
                                if (newIndex != SelectedTabPageIndex)
                                {
                                    MTCTabPage currentPage = TabPages[SelectedTabPageIndex];
                                    TabPages.Remove(TabPages[SelectedTabPageIndex]);
                                    TabPages.Insert(newIndex, currentPage);
                                    highlightedTabPageIndex = SelectedTabPageIndex = newIndex;
                                    if (AfterTabPageReorder != null)
                                        AfterTabPageReorder(this, new EventArgs());
                                    if (SelectedTabPageIndexChanged != null)
                                        SelectedTabPageIndexChanged(this, new EventArgs());
                                }
                            }
                            else
                            {
                                if (AllowTabPageDragAndDrop)
                                {
                                    if (AllowAutoTabPageDragAndDrop)
                                    {
                                        if (BeforeAutoDragAndDrop != null)
                                        {
                                            BeforeAutoDragAndDrop(this, new EventArgs());
                                        }
                                        DoDragDrop(TabPages[SelectedTabPageIndex], DragDropEffects.Move);
                                        if (AfterAutoDragAndDrop != null)
                                        {
                                            AfterAutoDragAndDrop(this, new EventArgs());
                                        }
                                    }
                                    else
                                    {
                                        if (TabPageDrag != null)
                                            TabPageDrag(this, new MTCTabPageDragArgs(TabPages[SelectedTabPageIndex].ID,
                                                SelectedTabPageIndex));
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Set highlighted tab page index
                if (e.Y < 2 || e.Y > this.Height - 2)
                {
                    currentTabPageThatShowToolTip = highlightedTabPageIndex = -1;
                    highlightCloseButton = false;
                    if (ClearToolTip != null)
                        ClearToolTip(this, new EventArgs());
                }
                else if (e.X > 1 & e.X < CalculateTabPagesWidth() - HScrollOffset)
                {
                    int cX = 0;
                    int x = 0;
                    int i = 0;
                    bool found = false;
                    foreach (MTCTabPage page in TabPages)
                    {
                        int width = CalculateTabPageWidth(page);
                        bool shouldShowTooltip = false;
                        if (width >= TabPageMaxWidth)
                        {
                            width = TabPageMaxWidth;
                            shouldShowTooltip = true;
                        }
                        cX += width;
                        if (cX >= HScrollOffset)
                        {
                            if (e.X >= (x - HScrollOffset) && e.X <= (cX - HScrollOffset) + 3)
                            {
                                highlightedTabPageIndex = i;
                                highlightCloseButton = (e.X >= (cX - HScrollOffset - tabxSize) && e.X <= (cX - HScrollOffset) + 3);
                                if (AlwaysShowToolTip)
                                {
                                    if (currentTabPageThatShowToolTip != i)
                                    {
                                        currentTabPageThatShowToolTip = i;
                                        if (ShowTabPageToolTipRequest != null)
                                            ShowTabPageToolTipRequest(this, new MTCTabPageShowToolTipArgs(TabPages[highlightedTabPageIndex].ID,
                                                i, TabPages[highlightedTabPageIndex].Text));
                                    }
                                }
                                else
                                {
                                    if (shouldShowTooltip & currentTabPageThatShowToolTip != i)
                                    {
                                        shouldShowTooltip = false;
                                        currentTabPageThatShowToolTip = i;
                                        if (ShowTabPageToolTipRequest != null)
                                            ShowTabPageToolTipRequest(this, new MTCTabPageShowToolTipArgs(TabPages[highlightedTabPageIndex].ID,
                                                i, TabPages[highlightedTabPageIndex].Text));
                                    }
                                }
                                found = true;
                                break;
                            }
                        }
                        x += width;
                        i++;
                        if (x - HScrollOffset > this.Width)
                            break;
                    }
                    if (!found)
                    {
                        if (ClearToolTip != null)
                            ClearToolTip(this, new EventArgs());
                        currentTabPageThatShowToolTip = highlightedTabPageIndex = -1;
                        highlightCloseButton = false;
                    }
                }
                else
                {
                    if (ClearToolTip != null)
                        ClearToolTip(this, new EventArgs());
                    currentTabPageThatShowToolTip = highlightedTabPageIndex = -1;
                    highlightCloseButton = false;
                }
                #endregion
            }
            Invalidate();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseDownPoint = e.Location;
            mouseDownPointAsViewPort = new Point(e.X + HScrollOffset, e.Y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            movingLock = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            currentTabPageThatShowToolTip = highlightedTabPageIndex = -1;
            highlightCloseButton = false;
            if (ClearToolTip != null)
                ClearToolTip(this, new EventArgs());
            Invalidate();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (movingLock)
                return;
            if (highlightedTabPageIndex >= 0)
            {
                if (DrawCloseBoxOnEachPage & highlightCloseButton)
                {
                    // Close request
                    MTCTabPageCloseArgs args = new MTCTabPageCloseArgs(TabPages[highlightedTabPageIndex].ID, highlightedTabPageIndex);
                    if (TabPageCloseRequest != null)
                        TabPageCloseRequest(this, args);
                }
                else
                {
                    // Select request
                    SelectedTabPageIndex = highlightedTabPageIndex;
                    if (SelectedTabPageIndexChanged != null)
                        SelectedTabPageIndexChanged(this, new EventArgs());

                    if (!IsTapPageShownCompletely(SelectedTabPageIndex))
                    {
                        if (ScrollToSelectedTabPage != null)
                            ScrollToSelectedTabPage(this, new EventArgs());
                    }
                }
            }
            Invalidate();
        }

        /// <summary>
        /// Call this when a tab page added to the TabPages collection
        /// </summary>
        public void OnTabPagesCollectionClear()
        {
            if (RefreshScrollBar != null)
                RefreshScrollBar(this, new EventArgs());

            SelectedTabPageIndex = -1;
            highlightedTabPageIndex = -1;
            highlightCloseButton = false;

            if (SelectedTabPageIndexChanged != null)
                SelectedTabPageIndexChanged(this, new EventArgs());

            if (TabPagesCleared != null)
                TabPagesCleared(this, new EventArgs());
            Invalidate();
        }
        /// <summary>
        /// Call this when a tab page removed from the TabPages collection
        /// </summary>
        public void OnTabPagesItemRemoved()
        {
            if (RefreshScrollBar != null)
                RefreshScrollBar(this, new EventArgs());
            if (TabPages.Count == 0)
            {
                SelectedTabPageIndex = -1;
                highlightedTabPageIndex = -1;
                highlightCloseButton = false;
            }
            if (TabPages.Count > 0)
            {
                SelectedTabPageIndex = 0;
                if (SelectedTabPageIndexChanged != null)
                    SelectedTabPageIndexChanged(this, null);
            }
            Invalidate();
        }
        /// <summary>
        /// Call this when the TabPages collection get cleared.
        /// </summary>
        public void OnTabPagesItemAdded()
        {
            if (RefreshScrollBar != null)
                RefreshScrollBar(this, new EventArgs());
            if (TabPages.Count == 1)
            {
                SelectedTabPageIndex = 0;
                if (SelectedTabPageIndexChanged != null)
                    SelectedTabPageIndexChanged(this, new EventArgs());
            }
            else
            {
                if (AutoSelectAddedTabPage)
                {
                    SelectedTabPageIndex = TabPages.Count - 1;
                    if (SelectedTabPageIndexChanged != null)
                        SelectedTabPageIndexChanged(this, new EventArgs());
                }
            }
            Invalidate();
        }
    }
}
