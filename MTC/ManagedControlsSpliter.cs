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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTC
{
    public partial class ManagedControlsSpliter : UserControl
    {
        public ManagedControlsSpliter()
        {
            InitializeComponent();
            panels = new ManagedControlsSpliterPanelsCollection(this);
        }
        private ManagedControlsSpliterPanelsCollection panels;
        private ManagedControlsSpliterAlignment alignement = ManagedControlsSpliterAlignment.Vertically;
        private List<ManagedControlsSpliterBufferItem> splitersBuffer = new List<ManagedControlsSpliterBufferItem>();
        private int splitterThickness = 1;
        private Color splitterColor = SystemColors.Control;
        private Color ellipsesColor = Color.Gray;
        private bool drawEllipses;
        private Point downPoint;
        private int targetItemPreviousIndex = -1;
        private int targetItemPreviousThickness = -1;
        private int targetItemNextThickness = -1;
        private bool isMovingSpliter = false;
        private int lastPanelThickness = -1;
        private bool doFix = false;
        // eclipses
        int _e_w = 2;
        int _e_h = 2;
        int _e_count = 5;
        int _e_space = 6;

        /*Properties*/
        /// <summary>
        /// Get or set the panels collection
        /// </summary>
        public ManagedControlsSpliterPanelsCollection Panels
        {
            get { return panels; }
            set { panels = value; }
        }
        /// <summary>
        /// Get or set panels alignemet
        /// </summary>
        public ManagedControlsSpliterAlignment Alignement
        {
            get { return alignement; }
            set
            {
                alignement = value;
                AutoCalculate();
                RefreshControlSpliters(false);
            }
        }
        /// <summary>
        /// Get or set the splitter color
        /// </summary>
        public Color SplitterColor
        { get { return splitterColor; } set { splitterColor = value; } }
        /// <summary>
        /// Get or set the splitter ellipses color
        /// </summary>
        public Color SplitterEllipesColor
        { get { return ellipsesColor; } set { ellipsesColor = value; } }
        /// <summary>
        /// Get or set the spliter width
        /// </summary>
        public int SplitterThickness
        {
            get { return splitterThickness; }
            set
            {
                if (value > 3)
                    splitterThickness = value;
                else
                    splitterThickness = 4;

                _e_h = _e_w = splitterThickness - 2;
                _e_space = (_e_w * 2) + 1;
            }
        }
        /// <summary>
        /// Get or set if the control should draw the splitter ellipses
        /// </summary>
        public bool DrawEllipses
        { get { return drawEllipses; } set { drawEllipses = value; } }

        public bool AutoCalculate()
        {
            return AutoCalculate(false);
        }
        /// <summary>
        /// Auto calculate the thickness value for all panels.
        /// </summary>
        /// <param name="OnlyIfNeedTo">If true, the calculation will be done only when the controls have to calculate (the thickness value of one control or more is invalid)</param>
        /// <returns>Returns if the auto calculate done successfully.</returns>
        public bool AutoCalculate(bool OnlyIfNeedTo)
        {
            if (panels.Count <= 0)
                return false;

            if (OnlyIfNeedTo)
            {
                bool need = false;
                foreach (ManagedControlsSpliterPanel panel in panels)
                {
                    if (panel.Thickness <= 0)
                    {
                        need = true;
                        break;
                    }
                }
                if (!need) return false;
            }
           
            if (panels.Count == 1)
            {
                switch (alignement)
                {
                    case ManagedControlsSpliterAlignment.Horizontally: panels[0].Thickness = this.Height; break;
                    case ManagedControlsSpliterAlignment.Vertically: panels[0].Thickness = this.Width; break;
                }
            }
            else
            {
                switch (alignement)
                {
                    case ManagedControlsSpliterAlignment.Horizontally:
                        {
                            int index = 0;
                            int adds = 0;
                            int fixedThickness = this.Height / panels.Count;
                            foreach (ManagedControlsSpliterPanel panel in panels)
                            {
                                panel.Thickness = fixedThickness;
                                if (index == panels.Count - 1)
                                {
                                    panel.Thickness = this.Height - adds;
                                }
                                adds += fixedThickness;
                                index++;
                            }
                            break;
                        }
                    case ManagedControlsSpliterAlignment.Vertically:
                        {
                            int adds = 0;
                            int index = 0;
                            int fixedThickness = this.Width / panels.Count;
                            foreach (ManagedControlsSpliterPanel panel in panels)
                            {
                                panel.Thickness = fixedThickness;
                                if (index == panels.Count - 1)
                                {
                                    panel.Thickness = this.Width - adds;
                                }
                                adds += fixedThickness;
                                index++;
                            }
                            break;
                        }
                }

            }
            return true;
        }
        public void RefreshControlSpliters(bool clearItems)
        {
            if (clearItems)
            {
                this.Controls.Clear();
                splitersBuffer.Clear();
            }
            if (panels == null) return;
            int offset = 0;
            int index = 0;
            foreach (ManagedControlsSpliterPanel panel in panels)
            {
                Panel pan = panel.Panel;

                // Calculate coordinates
                if (panels.Count == 1)
                {
                    pan.Location = new Point(0, 0);
                    pan.Size = this.Size;
                }
                else// More than one panel
                {
                    ManagedControlsSpliterBufferItem spliter = new ManagedControlsSpliterBufferItem();
                    spliter.PreviousPanelIndex = index - 1;

                    if (doFix && panels.Count > 1 && (index == panels.Count - 2))
                    {
                        doFix = false;
                        // We need to this to fix last control size
                        switch (alignement)
                        {
                            case ManagedControlsSpliterAlignment.Vertically:
                                {
                                    Point location = new Point(offset, 0);
                                    if (location != pan.Location)
                                        pan.Location = location;

                                    panel.Thickness = this.Width - offset - lastPanelThickness;

                                    Size size = new Size(panel.Thickness, this.Height);
                                    if (pan.Size != size)
                                        pan.Size = size;

                                    spliter.Rectangle = new Rectangle(offset - splitterThickness, 0, splitterThickness, this.Height);
                                    break;
                                }
                            case ManagedControlsSpliterAlignment.Horizontally:
                                {
                                    Point location = new Point(0, offset);
                                    if (location != pan.Location)
                                        pan.Location = location;

                                    panel.Thickness = this.Height - offset - lastPanelThickness;

                                    Size size = new Size(this.Width, panel.Thickness);
                                    if (size != pan.Size)
                                        pan.Size = size;
                                    spliter.Rectangle = new Rectangle(0, offset - splitterThickness, this.Width, splitterThickness);
                                    break;
                                }
                        }
                    }
                    else if (index == panels.Count - 1)
                    {
                        switch (alignement)
                        {
                            case ManagedControlsSpliterAlignment.Vertically:
                                {
                                    Point location = new Point(offset, 0);
                                    if (pan.Location != location)
                                        pan.Location = location;

                                    int suggestedThickness = this.Width - offset;
                                    if (panel.Thickness < suggestedThickness)
                                        panel.Thickness = suggestedThickness;
                                    Size size = new Size(panel.Thickness, this.Height);
                                    if (size != pan.Size)
                                        pan.Size = size;
                                    spliter.Rectangle = new Rectangle(offset - splitterThickness, 0, splitterThickness, this.Height);
                                    break;
                                }
                            case ManagedControlsSpliterAlignment.Horizontally:
                                {
                                    Point location = new Point(0, offset);
                                    if (pan.Location != location)
                                        pan.Location = location;

                                    int suggestedThickness = this.Height - offset;
                                    if (panel.Thickness < suggestedThickness)
                                        panel.Thickness = suggestedThickness;

                                    Size size = new Size(this.Width, panel.Thickness);
                                    if (pan.Size != size)
                                        pan.Size = size;
                                    spliter.Rectangle = new Rectangle(0, offset - splitterThickness, this.Width, splitterThickness);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        switch (alignement)
                        {
                            case ManagedControlsSpliterAlignment.Vertically:
                                {
                                    Point location = new Point(offset, 0);
                                    if (pan.Location != location)
                                        pan.Location = location;

                                    Size size = new Size(panel.Thickness, this.Height);
                                    if (pan.Size != size)
                                        pan.Size = size;
                                    spliter.Rectangle =
                                        new Rectangle(offset - splitterThickness, 0, splitterThickness, this.Height);
                                    break;
                                }
                            case ManagedControlsSpliterAlignment.Horizontally:
                                {
                                    Point location = new Point(0, offset);
                                    if (pan.Location != location)
                                        pan.Location = location;

                                    Size size = new Size(this.Width, panel.Thickness);
                                    if (pan.Size != size)
                                        pan.Size = size;
                                    spliter.Rectangle =
                                        new Rectangle(0, offset - splitterThickness, this.Width, splitterThickness);
                                    break;
                                }
                        }
                    }
                    if (index > 0)
                        splitersBuffer.Add(spliter);
                }
                offset += panel.Thickness + splitterThickness;
                index++;
                // Add panel to controls collection
                if (clearItems)
                    Controls.Add(pan);
            }
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (splitersBuffer == null) return;
            foreach (ManagedControlsSpliterBufferItem item in splitersBuffer)
            {
                e.Graphics.FillRectangle(new SolidBrush(splitterColor), item.Rectangle);
                // Draw ellipses
                if (drawEllipses)
                {
                    switch (alignement)
                    {
                        case ManagedControlsSpliterAlignment.Horizontally:
                            {
                                int e_total_w = _e_count * (_e_w + _e_space);
                                int e_x = (item.Rectangle.Width / 2) - (e_total_w / 2);
                                e_x += item.Rectangle.X;
                                int e_y = (item.Rectangle.Height / 2) - (_e_h / 2);
                                e_y += item.Rectangle.Y;
                                // Draw them !
                                for (int i = 0; i < _e_count; i++)
                                {
                                    e.Graphics.FillEllipse(new SolidBrush(ellipsesColor), e_x, e_y, _e_w, _e_h);
                                    e_x += _e_space;
                                }
                                break;
                            }
                        case ManagedControlsSpliterAlignment.Vertically:
                            {
                                int e_total_h = _e_count * (_e_h + _e_space);
                                int e_x = (item.Rectangle.Width / 2) - (_e_h / 2);
                                e_x += item.Rectangle.X;
                                int e_y = (item.Rectangle.Height / 2) - (e_total_h / 2);
                                e_y += item.Rectangle.Y;
                                // Draw them !
                                for (int i = 0; i < _e_count; i++)
                                {
                                    e.Graphics.FillEllipse(new SolidBrush(ellipsesColor), e_x, e_y, _e_w, _e_h);
                                    e_y += _e_space;
                                }
                                break;
                            }
                    }
                }
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (panels != null)
            {
                if (panels.Count > 1)
                {
                    lastPanelThickness = panels[panels.Count - 1].Thickness;
                    doFix = true;
                }
                else
                    lastPanelThickness = -1;
            }
            RefreshControlSpliters(false);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (isMovingSpliter)
                {
                    int shift = e.X - downPoint.X;
                    switch (alignement)
                    {
                        case ManagedControlsSpliterAlignment.Horizontally:
                            {
                                shift = e.Y - downPoint.Y;
                                break;
                            }
                        case ManagedControlsSpliterAlignment.Vertically:
                            {
                                shift = e.X - downPoint.X;
                                break;
                            }
                    }
                    // Do movements
                    if (targetItemPreviousIndex > -1)
                    {
                        panels[targetItemPreviousIndex].Thickness = targetItemPreviousThickness + shift;
                        if (panels[targetItemPreviousIndex].Thickness <= 5)
                            panels[targetItemPreviousIndex].Thickness = 5;

                        panels[targetItemPreviousIndex + 1].Thickness = targetItemNextThickness - shift;
                        if (panels[targetItemPreviousIndex + 1].Thickness <= 5)
                            panels[targetItemPreviousIndex + 1].Thickness = 5;

                        RefreshControlSpliters(false);
                    }
                }
            }
            else// Detect splitter
            {
                switch (alignement)
                {
                    case ManagedControlsSpliterAlignment.Horizontally:
                        {
                            foreach (ManagedControlsSpliterBufferItem item in splitersBuffer)
                            {
                                if (item.Rectangle.Contains(e.Location))
                                {
                                    Cursor = Cursors.SizeNS;
                                    break;
                                }
                            }
                            break;
                        }
                    case ManagedControlsSpliterAlignment.Vertically:
                        {
                            foreach (ManagedControlsSpliterBufferItem item in splitersBuffer)
                            {
                                if (item.Rectangle.Contains(e.Location))
                                {
                                    Cursor = Cursors.SizeWE;
                                    break;
                                }
                            }
                            break;
                        }
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            targetItemPreviousIndex = -1; 
            isMovingSpliter = false;
            Cursor = Cursors.Default;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e); Cursor = Cursors.Default;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            downPoint = e.Location;
            base.OnMouseDown(e);
            // Find the item
            if (targetItemPreviousIndex == -1)
            {
                switch (alignement)
                {
                    case ManagedControlsSpliterAlignment.Horizontally:
                        {
                            foreach (ManagedControlsSpliterBufferItem item in splitersBuffer)
                            {
                                if (item.Rectangle.Contains(e.Location))
                                {
                                    Cursor = Cursors.SizeNS;
                                    targetItemPreviousIndex = item.PreviousPanelIndex;
                                    targetItemPreviousThickness = panels[targetItemPreviousIndex].Thickness;
                                    targetItemNextThickness = panels[targetItemPreviousIndex + 1].Thickness;
                                    isMovingSpliter = true;
                                    break;
                                }
                            }
                            break;
                        }
                    case ManagedControlsSpliterAlignment.Vertically:
                        {
                            foreach (ManagedControlsSpliterBufferItem item in splitersBuffer)
                            {
                                if (item.Rectangle.Contains(e.Location))
                                {
                                    Cursor = Cursors.SizeWE;
                                    targetItemPreviousIndex = item.PreviousPanelIndex;
                                    targetItemPreviousThickness = panels[targetItemPreviousIndex].Thickness;
                                    targetItemNextThickness = panels[targetItemPreviousIndex + 1].Thickness;
                                    isMovingSpliter = true;
                                    break;
                                }
                            }
                            break;
                        }
                }
            }
        }
        public void OnPanelAdd(ManagedControlsSpliterPanel panel)
        {
            AutoCalculate();
            RefreshControlSpliters(true);
        }
        public void OnPanelRemove()
        {
            AutoCalculate();
            RefreshControlSpliters(true);
        }
        public void OnPanelsClear()
        {
            AutoCalculate();
            RefreshControlSpliters(true);
        }
    }
}
