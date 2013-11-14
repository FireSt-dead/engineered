using System;

namespace UserInterface.Primitives
{
	public class GridBand
	{
		internal float DesiredSize;
		internal float SpanningSize;
		internal GridLane GridLayout;

		public GridBand()
		{
			this.Unit = GridBandUnit.Auto;
			this.Maximum = float.PositiveInfinity;
		}

		public GridBandUnit Unit { get; set; }
		public float Length { get; set; }

		public float Minimum { get; set; }
		public float Maximum { get; set; }

		public float ArrangedSize
		{
			get
			{
				return this.DesiredSize + this.SpanningSize;
			}
		}

		internal bool IsFixed
		{
			get
			{
				return this.Unit == GridBandUnit.Points;
			}
		}

		internal bool IsAuto
		{
			get
			{
				return this.Unit == GridBandUnit.Auto;
			}
		}

		internal bool IsPercent
		{
			get
			{
				return this.Unit == GridBandUnit.Percent;
			}
		}
	}
}
