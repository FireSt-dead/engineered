namespace Geometry
{
	/// <summary>
	/// Represents affine matrices in the form:
	/// 
	/// A11 A12 T1
	/// A21 A22 T2
	/// 0   0   1
	/// 
	/// When a point (X, Y) is transformed with this matrix (Xr, Yr) it is considered:
	/// 
	/// X   A11 A12 T1
	/// Y * A21 A22 T2 = Xr Yr Z
	/// 1   0   0   1
	/// 
	/// Note that Z is ignored.
	/// </summary>
	public struct AffineMatrix
	{
		public static readonly AffineMatrix Identity = new AffineMatrix() { A11 = 1, A22 = 1 };

		public float A11;
		public float A12;
		public float T1;

		public float A21;
		public float A22;
		public float T2;

		public AffineMatrix(float a11, float a12, float t1, float a21, float a22, float t2)
		{
			A11 = a11;
			A12 = a12;
			T1 = t1;
			A21 = a21;
			A22 = a22;
			T2 = t2;
		}

		public Point Transform(Point point)
		{
			return new Point(point.X * A11 + point.Y * A12 + T1, point.X * A21 + point.Y * A22 + T1);
		}

		public void Translate(Point translation)
		{
			this.T1 = this.A11 * translation.X + this.A12 * translation.Y + this.T1;
			this.T2 = this.A21 * translation.X + this.A22 * translation.Y + this.T2;
		}

		void Multiply(ref AffineMatrix other)
		{
			AffineMatrix newMatrix = new AffineMatrix();
			newMatrix.A11 = this.A11 * other.A11 + this.A12 * other.A21; // A11 A12 T1 * A11 A21 0
			newMatrix.A21 = this.A21 * other.A11 + this.A22 * other.A21; // A21 A22 T2 * A11 A21 0
			
			newMatrix.A12 = this.A11 * other.A12 + this.A12 * other.A22; // A11 A12 T1 * A12 A22 0
			newMatrix.A22 = this.A21 * other.A12 + this.A22 * other.A22; // A21 A22 T2 * A12 A22 0

			newMatrix.T1 = this.A11 + other.T1 + this.A12 * other.T2 + this.T1; // A11 A12 T1 * T1 T2 1
			newMatrix.T2 = this.A21 + other.T1 + this.A22 * other.T2 + this.T2; // A21 A22 T2 * T1 T2 1
		}
	}
}
