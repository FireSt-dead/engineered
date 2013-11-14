using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drawing
{
	public struct Color
	{
		public float Alpha;
		public float Red;
		public float Green;
		public float Blue;

		public Color(float red, float green, float blue)
		{
			this.Alpha = 1f;
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
		}

		public Color(float alpha, float red, float green, float blue)
		{
			this.Alpha = alpha;
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
		}
	}
}
