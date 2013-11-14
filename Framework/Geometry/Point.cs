using System.Diagnostics;

namespace Geometry
{
	[DebuggerDisplay("Point ({X}; {Y})")]
	public struct Point
	{
		public float X;
		public float Y;

		public Point(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public override string ToString()
		{
			return string.Format("({0}; {1})", this.X, this.Y);
		}
	}
}
