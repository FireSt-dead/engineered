namespace Geometry
{
	public struct Rect
	{
		public float Left;
		public float Top;
		public float Right;
		public float Bottom;

		public Rect(float uniform)
		{
			this.Left = uniform;
			this.Top = uniform;
			this.Right = uniform;
			this.Bottom = uniform;
		}

		public Rect(float left, float top, float right, float bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		public Rect(Size size)
		{
			Left = 0;
			Top = 0;
			Right = size.Width;
			Bottom = size.Height;
		}

		public Rect(Interval horizontalInterval, Interval verticalInterval)
		{
			this.Left = horizontalInterval.Start;
			this.Top = verticalInterval.Start;
			this.Right = horizontalInterval.End;
			this.Bottom = verticalInterval.End;
		}

		public float Width
		{
			get
			{
				return Right - Left;
			}
		}

		public float Height
		{
			get
			{
				return Bottom - Top;
			}
		}

		public Size Size
		{
			get
			{
				return new Size() { Width = this.Right - this.Left, Height = this.Bottom - this.Top };
			}
		}

		public Point TopLeft
		{
			get
			{
				return new Point(this.Left, this.Top);
			}

			set
			{
				this.Left = value.X;
				this.Top = value.Y;
			}
		}

        public Point BottomRight
        {
            get
            {
                return new Point(this.Right, this.Bottom);
            }

            set
            {
                this.Right = value.X;
                this.Bottom = value.Y;
            }
        }

        // TODO: Intrude and Extrude should work with RectThickness or Padding struct. It will be the same as the Rect struct but will have other meaning and usage.
		public void Intrude(Rect rect)
		{
			this.Left += rect.Left;
			this.Top += rect.Top;
			this.Right -= rect.Right;
			this.Bottom -= rect.Bottom;
		}

        public void Extrude(Rect rect)
        {
            this.Left -= rect.Left;
            this.Top -= rect.Top;
            this.Right += rect.Right;
            this.Bottom += rect.Bottom;
        }

		public bool Contains(Point point)
		{
			return this.Left <= point.X && this.Right >= point.X && this.Top <= point.Y && this.Bottom >= point.Y;
		}
	}
}
