using Drawing;
using Geometry;

namespace UserInterface.Primitives
{
	public class Border : VisualContainer
	{
		public Border()
		{
			this.BorderThickness = 1f;
		}

		public float BorderThickness { get; set; }
		public float CornerRadius { get; set; }

		protected override Size MeasureContent(Size upToSize, IDrawingContext context)
		{
			return new Size() { Width = 22, Height = 22 };
		}

		protected override void DrawContent(IDrawingContext context)
		{
			float halfBorder = this.BorderThickness * 0.5f;
			var borderRect = new Rect(halfBorder, halfBorder, this.ArrangedRect.Width - halfBorder, this.ArrangedRect.Height - halfBorder);
			if (this.CornerRadius == 0f)
			{
				context.DrawRectangle(borderRect);
			}
			else
			{
				context.DrawRoundedRectangle(borderRect, this.CornerRadius, this.CornerRadius);
			}
			base.DrawContent(context);
		}

		public override bool HitTest(Point point)
		{
			return point.X >= 0 && point.X <= 22 && point.Y >= 0 && point.Y <= 22;
		}
	}
}
