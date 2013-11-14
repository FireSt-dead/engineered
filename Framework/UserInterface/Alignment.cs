using Geometry;

namespace UserInterface.Layout
{
	public abstract class Alignment
	{
		public static readonly Alignment ToStart = new AlignToStart();
		public static readonly Alignment ToEnd = new AlignToEnd();
		public static readonly Alignment Center = new AlignAtCenter();
		public static readonly Alignment Stretch = null;

		protected internal abstract Interval Align(Interval interval, float length);

		private class AlignStretch : Alignment
		{
			protected internal override Interval Align(Interval interval, float length)
			{
				return interval;
			}
		}

		private class AlignToStart : Alignment
		{
			protected internal override Interval Align(Interval interval, float length)
			{
				return new Interval() { Start = interval.Start, End = interval.Start + length };
			}
		}

		private class AlignToEnd : Alignment
		{
			protected internal override Interval Align(Interval interval, float length)
			{
				return new Interval() { Start = interval.End - length, End = interval.End };
			}
		}

		private class AlignAtCenter : Alignment
		{
			protected internal override Interval Align(Interval interval, float length)
			{
				var alignedInterval = new Interval();
				alignedInterval.Start = interval.Start + (interval.Length - length) * 0.5f;
				alignedInterval.End = alignedInterval.Start + length;
				return alignedInterval;
			}
		}
	}
}
