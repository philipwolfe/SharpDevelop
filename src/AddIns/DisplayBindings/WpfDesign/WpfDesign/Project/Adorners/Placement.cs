﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace ICSharpCode.WpfDesign.Adorners
{
	// We have to support the different coordinate spaces as explained in
	// http://myfun.spaces.live.com/blog/cns!AC1291870308F748!242.entry
	
	/// <summary>
	/// Defines how a design-time adorner is placed.
	/// </summary>
	public abstract class Placement
	{
		/// <summary>
		/// A placement instance that places the adorner above the content, using the same bounds as the content.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly Placement FillContent = new FillContentPlacement();
		
		/// <summary>
		/// Arranges the adorner element on the specified adorner panel.
		/// </summary>
		public abstract void Arrange(AdornerPanel panel, UIElement adorner, Size adornedElementSize);
		
		sealed class FillContentPlacement : Placement
		{
			public override void Arrange(AdornerPanel panel, UIElement adorner, Size adornedElementSize)
			{
				adorner.Arrange(new Rect(adornedElementSize));
			}
		}
	}
	
	/// <summary>
	/// Placement class providing properties for different kinds of relative placements.
	/// </summary>
	public sealed class RelativePlacement : Placement
	{
		double widthRelativeToDesiredWidth, heightRelativeToDesiredHeight;
		
		/// <summary>
		/// Gets/Sets the width of the adorner relative to the desired adorner width.
		/// </summary>
		public double WidthRelativeToDesiredWidth {
			get { return widthRelativeToDesiredWidth; }
			set { widthRelativeToDesiredWidth = value; }
		}
		
		/// <summary>
		/// Gets/Sets the height of the adorner relative to the desired adorner height.
		/// </summary>
		public double HeightRelativeToDesiredHeight {
			get { return heightRelativeToDesiredHeight; }
			set { heightRelativeToDesiredHeight = value; }
		}
		
		double widthRelativeToContentWidth, heightRelativeToContentHeight;
		
		/// <summary>
		/// Gets/Sets the width of the adorner relative to the width of the adorned item.
		/// </summary>
		public double WidthRelativeToContentWidth {
			get { return widthRelativeToContentWidth; }
			set { widthRelativeToContentWidth = value; }
		}
		
		/// <summary>
		/// Gets/Sets the height of the adorner relative to the height of the adorned item.
		/// </summary>
		public double HeightRelativeToContentHeight {
			get { return heightRelativeToContentHeight; }
			set { heightRelativeToContentHeight = value; }
		}
		
		double widthOffset, heightOffset;
		
		/// <summary>
		/// Gets/Sets an offset that is added to the adorner width for the size calculation.
		/// </summary>
		public double WidthOffset {
			get { return widthOffset; }
			set { widthOffset = value; }
		}
		
		/// <summary>
		/// Gets/Sets an offset that is added to the adorner height for the size calculation.
		/// </summary>
		public double HeightOffset {
			get { return heightOffset; }
			set { heightOffset = value; }
		}
		
		Size CalculateSize(UIElement adorner, Size adornedElementSize)
		{
			Size size = new Size(widthOffset, heightOffset);
			if (widthRelativeToDesiredWidth != 0 || heightRelativeToDesiredHeight != 0) {
				size.Width += widthRelativeToDesiredWidth * adorner.DesiredSize.Width;
				size.Height += heightRelativeToDesiredHeight * adorner.DesiredSize.Height;
			}
			size.Width += widthRelativeToContentWidth * adornedElementSize.Width;
			size.Height += heightRelativeToContentHeight * adornedElementSize.Height;
			return size;
		}
		
		double xOffset, yOffset;
		
		/// <summary>
		/// Gets/Sets an offset that is added to the adorner position.
		/// </summary>
		public double XOffset {
			get { return xOffset; }
			set { xOffset = value; }
		}
		
		/// <summary>
		/// Gets/Sets an offset that is added to the adorner position.
		/// </summary>
		public double YOffset {
			get { return yOffset; }
			set { yOffset = value; }
		}
		
		Point CalculatePosition(Size adornedElementSize, Size adornerSize)
		{
			return new Point(xOffset, yOffset);
		}
		
		/// <summary>
		/// Arranges the adorner element on the specified adorner panel.
		/// </summary>
		public override void Arrange(AdornerPanel panel, UIElement adorner, Size adornedElementSize)
		{
			Size adornerSize = CalculateSize(adorner, adornedElementSize);
			adorner.Arrange(new Rect(CalculatePosition(adornedElementSize, adornerSize), adornerSize));
		}
	}

	/// <summary>
	/// Describes the space in which an adorner is placed.
	/// </summary>
	public enum PlacementSpace
	{
		/// <summary>
		/// The adorner is affected by the render transform of the adorned element.
		/// </summary>
		Render,
		/// <summary>
		/// The adorner is affected by the layout transform of the adorned element.
		/// </summary>
		Layout,
		/// <summary>
		/// The adorner is not affected by transforms of designed controls.
		/// </summary>
		Designer
	}

	/// <summary>
	/// The possible layers where adorners can be placed.
	/// </summary>
	public enum AdornerZLayer
	{
		/// <summary>
		/// This layer is below the other adorner layers.
		/// </summary>
		Low,
		/// <summary>
		/// This layer is for normal background adorners.
		/// </summary>
		Normal,
		/// <summary>
		/// This layer is for selection adorners
		/// </summary>
		Selection,
		/// <summary>
		/// This layer is for primary selection adorners
		/// </summary>
		PrimarySelection,
		/// <summary>
		/// This layer is above the other layers.
		/// It is used for temporary drawings, e.g. the selection frame while selecting multiple controls with the mouse.
		/// </summary>
		High
	}
}
