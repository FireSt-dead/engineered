using System;
using Geometry;
using Drawing;

namespace UserInterface.Primitives
{
	public class HorizontalStack : VisualContainer
	{
		protected override Size MeasureContent(Size upToSize, IDrawingContext context)
		{
			Size size = new Size();
			var childSize = new Size() { Width = float.PositiveInfinity, Height = upToSize.Height };

			foreach (var child in this)
			{
				child.Measure(childSize, context);
				size.Width += child.DesiredSize.Width;
				size.Height = Math.Max(size.Height, child.DesiredSize.Height);
			}

			return size;
		}

		protected override void ArrangeContent(Size size, IDrawingContext context)
		{
			float left = 0;
			float right = 0;
			float height = size.Height;

			foreach (var child in this)
			{
				right += child.DesiredSize.Width;
				child.Arrange(new Rect() { Left = left, Top = 0, Right = right, Bottom = height }, context);
				left = right;
			}
		}
	}
}
