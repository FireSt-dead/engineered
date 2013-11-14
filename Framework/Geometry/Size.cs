using System;

namespace Geometry
{
	public struct Size
	{
		public float Width;
		public float Height;

		public Size(float width, float height)
		{
			this.Width = width;
			this.Height = height;
		}

		public static Size Max(Size x, Size y)
		{
			return new Size()
			{
				Width = Math.Max(x.Width, y.Width),
				Height = Math.Max(x.Height, y.Height)
			};
		}

		public static Size Min(Size x, Size y)
		{
			return new Size()
			{
				Width = Math.Min(x.Width, y.Width),
				Height = Math.Min(x.Height, y.Height)
			};
		}

        // TODO: Intrude and Extrude should work with RectThickness or Padding struct. It will be the same as the Rect struct but will have other meaning and usage.
		public void Intrude(Rect rect)
		{
			this.Width = this.Width - rect.Left - rect.Right;
			this.Height = this.Height - rect.Top - rect.Bottom;
		}

		public void Extrude(Rect rect)
		{
			this.Width = this.Width + rect.Left + rect.Right;
			this.Height = this.Height + rect.Top + rect.Bottom;
		}
	}
}
