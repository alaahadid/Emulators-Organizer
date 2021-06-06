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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    public partial class ImagePanel : Control
    {
        public ImagePanel()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);

            _StringFormat = new StringFormat();

            this.Text = "";
        }

        private ImageViewMode imageViewMode = ImageViewMode.StretchIfLarger;
        private Bitmap imageToView;
        private Bitmap defaultImage;
        private StringFormat _StringFormat;

        public bool DrawDefaultImageWhenViewImageIsNull = true;
        public System.Drawing.Drawing2D.InterpolationMode DrawInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        public int viewImageWidth = 0;
        public int viewImageHeight = 0;
        public int drawX = 0;
        public int drawY = 0;
        public float zoom = 1;// Set to -1 to set the image original size

        private int olddrawX = 0;
        private int olddrawY = 0;
        private Point downPoint;

        //events
        public event EventHandler DisableScrollBars;
        public event EventHandler EnableScrollBars;
        public event EventHandler CalculateScrollValues;
        public event EventHandler ImageViewModeChanged;

        //properties
        public Bitmap ImageToView
        {
            get { return imageToView; }
            set
            {
                //if (imageToView != null)
                 //   imageToView.Dispose();
                imageToView = value;
                zoom = 1;
                CalculateImageValues();
                if (value != null)
                    if (ImageAnimator.CanAnimate(value))
                    {
                        ImageAnimator.Animate(value, new EventHandler(onAnimate));
                    }
            }
        }
        /// <summary>
        /// Get or set the default image to draw when the image to view property is null
        /// </summary>
        public Image DefaultImage
        { get { return defaultImage; } set { defaultImage = (Bitmap)value; } }
        /// <summary>
        /// Get or set the image view mode, specifies how the image get rendered.
        /// </summary>
        public ImageViewMode ImageViewMode
        {
            get { return imageViewMode; }
            set
            {
                imageViewMode = value;
                if (ImageViewModeChanged != null)
                    ImageViewModeChanged(this, new EventArgs());
                if (value == Core.ImageViewMode.Normal)
                {
                    zoom = 1;
                }
            }
        }

        public void CalculateImageValues()
        {
            if (imageToView == null)
            {
                if (DisableScrollBars != null)
                    DisableScrollBars(this, null);
                return;
            }

            switch (this.imageViewMode)
            {
                case EmulatorsOrganizer.Core.ImageViewMode.StretchIfLarger:
                    CalculateStretchedImageValues();
                    CenterImage();
                    if (DisableScrollBars != null)
                        DisableScrollBars(this, null);
                    break;
                case EmulatorsOrganizer.Core.ImageViewMode.StretchToFit:
                    CalculateStretchToFitImageValues();
                    CenterImage();
                    if (DisableScrollBars != null)
                        DisableScrollBars(this, null);
                    break;
                case EmulatorsOrganizer.Core.ImageViewMode.Normal:
                    CalculateNormalImageValues();
                    CenterImage();
                    if (EnableScrollBars != null)
                        EnableScrollBars(this, null);
                    if (CalculateScrollValues != null)
                        CalculateScrollValues(this, null);
                    break;
            }
        }
        void CalculateStretchedImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)imageToView.Width / imageToView.Height;

            if (this.Width >= imageToView.Width && this.Height >= imageToView.Height)
            {
                viewImageWidth = imageToView.Width;
                viewImageHeight = imageToView.Height;
            }
            else if (this.Width > imageToView.Width && this.Height < imageToView.Height)
            {
                viewImageHeight = this.Height;
                viewImageWidth = (int)(this.Height * imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height > imageToView.Height)
            {
                viewImageWidth = this.Width;
                viewImageHeight = (int)(this.Width / imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height < imageToView.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                }
            }
        }
        void CalculateStretchToFitImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)imageToView.Width / imageToView.Height;

            if (this.Width >= imageToView.Width && this.Height >= imageToView.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                }
            }
            else if (this.Width > imageToView.Width && this.Height < imageToView.Height)
            {
                viewImageHeight = this.Height;
                viewImageWidth = (int)(this.Height * imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height > imageToView.Height)
            {
                viewImageWidth = this.Width;
                viewImageHeight = (int)(this.Width / imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height < imageToView.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                }
            }

        }
        void CalculateNormalImageValues()
        {
            if (zoom == -1)
            {
                viewImageWidth = imageToView.Width;
                viewImageHeight = imageToView.Height;
                return;
            }
            float maxWidth = this.Width * zoom;
            float maxHeight = this.Height * zoom;
            float pRatio = maxWidth / maxHeight;
            float imRatio = (float)imageToView.Width / imageToView.Height;

            if (maxWidth >= imageToView.Width && maxHeight >= imageToView.Height)
            {
                if (maxWidth >= maxHeight)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = (int)maxWidth;
                        viewImageHeight = (int)(maxWidth / imRatio);
                    }
                    else
                    {
                        viewImageHeight = (int)maxHeight;
                        viewImageWidth = (int)(maxHeight * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = (int)maxHeight;
                        viewImageWidth = (int)(maxHeight * imRatio);
                    }
                    else
                    {
                        viewImageWidth = (int)maxWidth;
                        viewImageHeight = (int)(maxWidth / imRatio);
                    }
                }
            }
            else if (maxWidth > imageToView.Width && maxHeight < imageToView.Height)
            {
                viewImageHeight = (int)maxHeight;
                viewImageWidth = (int)(maxHeight * imRatio);
            }
            else if (maxWidth < imageToView.Width && maxHeight > imageToView.Height)
            {
                viewImageWidth = (int)maxWidth;
                viewImageHeight = (int)(maxWidth / imRatio);
            }
            else if (maxWidth < imageToView.Width && maxHeight < imageToView.Height)
            {
                if (maxWidth >= maxHeight)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = (int)maxWidth;
                        viewImageHeight = (int)(maxWidth / imRatio);
                    }
                    else
                    {
                        viewImageHeight = (int)maxHeight;
                        viewImageWidth = (int)(maxHeight * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = (int)maxHeight;
                        viewImageWidth = (int)(maxHeight * imRatio);
                    }
                    else
                    {
                        viewImageWidth = (int)maxWidth;
                        viewImageHeight = (int)(maxWidth / imRatio);
                    }
                }
            }
        }
        void CalculateStretchNoAspectRatio()
        {
            viewImageWidth = this.Width;
            viewImageHeight = this.Height;
            drawX = drawY = 0;
        }
        void CenterImage()
        {
            drawY = (int)((this.Height - viewImageHeight) / 2.0);
            drawX = (int)((this.Width - viewImageWidth) / 2.0);
        }
        private void onAnimate(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = DrawInterpolationMode;
            base.OnPaint(pe);
            if (ImageToView == null)
            {
                if (DrawDefaultImageWhenViewImageIsNull)
                    if (defaultImage != null)
                        pe.Graphics.DrawImage(defaultImage, new Rectangle(0, 0, this.Width, this.Height));
                goto DRAWTEXT;
            }
            ImageAnimator.UpdateFrames();
            switch (this.imageViewMode)
            {
                case EmulatorsOrganizer.Core.ImageViewMode.StretchIfLarger:
                    CalculateStretchedImageValues();
                    CenterImage();
                    break;
                case EmulatorsOrganizer.Core.ImageViewMode.StretchToFit:
                    CalculateStretchToFitImageValues();
                    CenterImage();
                    break;
                case EmulatorsOrganizer.Core.ImageViewMode.Normal:
                    CalculateNormalImageValues();
                    break;
                case Core.ImageViewMode.StretchNoAspectRatio:
                    CalculateStretchNoAspectRatio();
                    break;
            }
            pe.Graphics.DrawImage(imageToView,
            new Rectangle(drawX, drawY, viewImageWidth, viewImageHeight));
        // Draw this text !
        DRAWTEXT:
            if (this.Text != "")
            {
                pe.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor),
                    new Rectangle(0, 0, this.Width, this.Height));
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.imageViewMode == EmulatorsOrganizer.Core.ImageViewMode.Normal)
            {
                CenterImage();
                if (CalculateScrollValues != null)
                    CalculateScrollValues(this, null);
            }
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            downPoint = e.Location;
            olddrawX = drawX;
            olddrawY = drawY;
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.X != downPoint.X || e.Y != downPoint.Y)
                {
                    this.Cursor = Cursors.Hand;
                    int xShift = e.X - downPoint.X;
                    int yShift = e.Y - downPoint.Y;

                    drawX = olddrawX + xShift;
                    drawY = olddrawY + yShift;

                    this.Invalidate();
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            base.OnMouseUp(e);
        }
    }
}
