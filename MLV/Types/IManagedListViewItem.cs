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
using System.Drawing;
using System.ComponentModel;

namespace MLV
{
    /// <summary>
    /// The Advanced ListView item interfaced class.
    /// </summary>
    [DefaultProperty("Text"), ToolboxItem(false), DesignTimeVisible(false)]
    [Serializable]
    public abstract class IManagedListViewItem
    {
        private string text = "";
        private Color color = Color.Black;
        private int imageIndex;
        private bool customFontEnabled = false;
        private Font font = new Font("Tahoma", 8, FontStyle.Regular);
        private ManagedListViewItemDrawMode drawMode = ManagedListViewItemDrawMode.Text;
        private object tag;

        /// <summary>
        /// Get or set the item text.
        /// </summary>
        [Description("The item text. This used only in Thumbnails view. For details view mode, use subitems."), Category("Text")]
        public virtual string Text
        { get { return text; } set { text = value; } }
        /// <summary>
        /// Get or set this item text's color.
        /// </summary>
        [Description("The text color."), Category("Style")]
        public virtual Color Color
        { get { return color; } set { color = value; } }
        /// <summary>
        /// Get or set the image index for this item within the ImagesList collection of the parent control.
        /// </summary>
        [Description("The image index within ImageList if used. The draw mode must set to Image or TextAndImage."), Category("Style")]
        public virtual int ImageIndex
        { get { return imageIndex; } set { imageIndex = value; } }
        /// <summary>
        /// Get or set the draw mode for this item.
        /// </summary>
        [Description("The draw mode to use for this item"), Category("Style")]
        public ManagedListViewItemDrawMode DrawMode
        { get { return this.drawMode; } set { this.drawMode = value; } }
        /// <summary>
        /// Get or set if the custom font enabled for this item. Normally the control draw item texts debending on font value 
        /// of that control, but if this value enabled the control will use the font that specified in CustomFont property 
        /// of thos item.
        /// </summary>
        [Description("If enabled, the control will use the CustomFont for item text otherwise it used Font property of the control."), Category("Style")]
        public bool CustomFontEnabled
        { get { return customFontEnabled; } set { customFontEnabled = value; } }
        /// <summary>
        /// Get or set the custom font which will be used to draw text if this item when the CustomFontEnabled property is true.
        /// </summary>
        [Description("The CustomFont for item text to use when CustomFontEnabled is set. Otherwise it used Font property of the control."), Category("Style")]
        public Font CustomFont
        { get { return font; } set { font = value; } }
        /// <summary>
        /// Get or set the tag for this item.
        /// </summary>
        [Browsable(false)]
        public object Tag
        { get { return this.tag; } set { this.tag = value; } }
        /// <summary>
        /// Rises the mouse over event.
        /// </summary>
        /// <param name="mouseLocation">The mouse location within the viewport of the parent listview control.</param>
        /// <param name="charFontSize">The char font size</param>
        public virtual void OnMouseOver(Point mouseLocation, Size charFontSize) { }
        /// <summary>
        /// Rises the mouse click event
        /// </summary>
        /// <param name="mouseLocation">The mouse location within the viewport of the parent listview control.</param>
        /// <param name="charFontSize">The char font size</param>
        /// <param name="itemIndex">The item index or the part item index if this item is a subitem.</param>
        public virtual void OnMouseClick(Point mouseLocation, Size charFontSize, int itemIndex) { }
        /// <summary>
        /// Rises the mouse leave event
        /// </summary>
        public virtual void OnMouseLeave() { }
    }
}
