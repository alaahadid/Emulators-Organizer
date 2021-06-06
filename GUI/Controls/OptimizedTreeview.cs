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
using System.Drawing;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
namespace EmulatorsOrganizer.GUI
{
    /*This tree view allows to change background*/
    public class OptimizedTreeview : TreeView
    {
        public OptimizedTreeview()
            : base()
        {
            base.DoubleBuffered = true;
            this.BackgroundImageMode = ImageViewMode.Normal;
            drawX = drawY = 0;
            drawH = this.Height;
            drawW = this.Width;
        }
        private Image backgroundThumbnail;
        private int drawX;
        private int drawY;
        private int drawH;
        private int drawW;

        private void CalculateStretchedImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)this.BackgroundImage.Width / this.BackgroundImage.Height;

            if (this.Width >= this.BackgroundImage.Width && this.Height >= this.BackgroundImage.Height)
            {
                drawW = BackgroundImage.Width;
                drawH = BackgroundImage.Height;
            }
            else if (this.Width > BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                drawH = this.Height;
                drawW = (int)(this.Height * imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height > BackgroundImage.Height)
            {
                drawW = this.Width;
                drawH = (int)(this.Width / imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                }
            }
        }
        private void CalculateStretchToFitImageValues()
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
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                }
            }
            else if (this.Width > BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                drawH = this.Height;
                drawW = (int)(this.Height * imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height > BackgroundImage.Height)
            {
                drawW = this.Width;
                drawH = (int)(this.Width / imRatio);
            }
            else if (this.Width < BackgroundImage.Width && this.Height < BackgroundImage.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (BackgroundImage.Width >= BackgroundImage.Height && imRatio >= pRatio)
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (BackgroundImage.Width < BackgroundImage.Height && imRatio < pRatio)
                    {
                        drawH = this.Height;
                        drawW = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        drawW = this.Width;
                        drawH = (int)(this.Width / imRatio);
                    }
                }
            }

        }
        private void CenterImage()
        {
            drawY = (int)((this.Height - drawH) / 2.0);
            drawX = (int)((this.Width - drawW) / 2.0);
        }
        public void CalculateBackgroundBounds()
        {
            if (this.BackgroundImage != null)
            {
                switch (BackgroundImageMode)
                {
                    case ImageViewMode.Normal:// Stretch image without aspect ratio, always..
                        {
                            drawX = drawY = 0;
                            drawH = this.Height;
                            drawW = this.Width;
                            break;
                        }
                    case ImageViewMode.StretchIfLarger:
                        {
                            CalculateStretchedImageValues();
                            CenterImage();
                            break;
                        }
                    case ImageViewMode.StretchToFit:
                        {
                            CalculateStretchToFitImageValues();
                            CenterImage();
                            break;
                        }
                }
                try
                {
                    backgroundThumbnail = this.BackgroundImage.GetThumbnailImage(drawW, drawH, null, IntPtr.Zero);
                }
                catch { backgroundThumbnail = this.BackgroundImage; }
            }
            else
                backgroundThumbnail = null;
            Invalidate();
        }
        // Properties
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
                try
                {
                    if (value != null)
                        backgroundThumbnail = value.GetThumbnailImage(this.Width, this.Height, null, IntPtr.Zero);
                    else
                        backgroundThumbnail = null;
                }
                catch { backgroundThumbnail = value; }
            }
        }
        public ImageViewMode BackgroundImageMode
        { get; set; }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalculateBackgroundBounds();
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (!this.Visible) return;
            if (m.Msg == 0x14 && backgroundThumbnail != null)
            {
                using (var gr = Graphics.FromHdc(m.WParam))
                {
                    gr.DrawImage(backgroundThumbnail, drawX, drawY, drawW, drawH);
                }
            }
        }
    }
}
