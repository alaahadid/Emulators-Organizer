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
namespace MTC
{
    /// <summary>
    /// The Managed Tab Control Draw Style
    /// </summary>
    [System.Serializable] 
    public enum MTCDrawStyle
    {
        /// <summary>
        /// Draw tab pages with normal mode (using <see cref="System.Drawing.Drawing2D.LinearGradientBrush"/> brush) 
        /// </summary>
        Normal,
        /// <summary>
        /// Draw tab pages using solide brush
        /// </summary>
        Flat,
        /// <summary>
        /// Draw tab pages using HatchBrush with BackwardDiagonal style
        /// </summary>
        BackwardDiagonal,
        /// <summary>
        /// Draw tab pages using HatchBrush with DashedDownwardDiagonal style
        /// </summary>
        DashedDownwardDiagonal,
        /// <summary>
        /// Draw tab pages using HatchBrush with Divot style
        /// </summary>
        Divot,
        /// <summary>
        /// Draw tab pages using HatchBrush with DottedGrid style
        /// </summary>
        DottedGrid,
        /// <summary>
        /// Draw tab pages using HatchBrush with HorizontalBrick style
        /// </summary>
        HorizontalBrick,
        /// <summary>
        /// Draw tab pages using HatchBrush with NarrowHorizontal style
        /// </summary>
        NarrowHorizontal,
        /// <summary>
        /// Draw tab pages using HatchBrush with NarrowVertical style
        /// </summary>
        NarrowVertical,
        /// <summary>
        /// Draw tab pages using HatchBrush with Shingle style
        /// </summary>
        Shingle,
        /// <summary>
        /// Draw tab pages using HatchBrush with Wave style
        /// </summary>
        Wave,
        /// <summary>
        /// Draw tab pages using HatchBrush with ZigZag style
        /// </summary>
        ZigZag
    }
}
