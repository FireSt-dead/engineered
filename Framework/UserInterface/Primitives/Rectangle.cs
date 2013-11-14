using Geometry;

namespace UserInterface.Primitives
{
	public class Rectangle : Visual
	{
		public float RadiusX { get; set; }
		public float RadiusY { get; set; }
		public Rect Rect { get; set; }

		protected override void DrawContent(Drawing.IDrawingContext context)
		{
			if (this.RadiusX == 0 && this.RadiusY == 0)
			{
				context.DrawRectangle(this.Rect);
			}
			else
			{
				context.DrawRoundedRectangle(this.Rect, RadiusX, RadiusY);
			}
		}
	}
}
