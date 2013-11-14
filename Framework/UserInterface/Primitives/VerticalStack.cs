using System;
using Geometry;
using Drawing;

namespace UserInterface.Primitives
{
	public class VerticalStack : VisualContainer
	{
		protected override Size MeasureContent(Size upToSize, IDrawingContext context)
		{
			Size size = new Size();
			var childSize = new Size() { Width = upToSize.Width, Height = float.PositiveInfinity };

			foreach (var child in this)
			{
				child.Measure(childSize, context);
				size.Width = Math.Max(size.Width, child.DesiredSize.Width);
				size.Height += child.DesiredSize.Height;
			}

			return size;
		}

		protected override void ArrangeContent(Size size, IDrawingContext context)
		{
			float top = 0;
			float bottom = 0;
			float width = size.Width;

			foreach (var child in this)
			{
				bottom += child.DesiredSize.Height;
				child.Arrange(new Rect() { Left = 0, Top = top, Right = width, Bottom = bottom }, context);
				top = bottom;
			}
		}
	}
}
