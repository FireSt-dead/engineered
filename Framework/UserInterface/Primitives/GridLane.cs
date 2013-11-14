using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace UserInterface.Primitives
{
	public class GridLane : Collection<GridBand>
	{
		public GridLane()
		{
		}

		#region Lanes Layout Calculation
		// 1) First reset to minumum sizes
		public void Reset()
		{
			foreach (var lane in this)
			{
				if (lane.IsFixed)
				{
					lane.DesiredSize = lane.Length;
					lane.SpanningSize = 0;
				}
				else
				{
					lane.DesiredSize = lane.Minimum;
					lane.SpanningSize = 0;
				}
			}
		}

		// 2) Then add items to Autos
		public void PushSingleBandItem(int index, float size)
		{
			var lane = this[index];
			if (lane.IsAuto)
			{
				lane.DesiredSize = Math.Max(lane.DesiredSize, Math.Max(size, lane.Minimum));
			}
		}

		// 3) prepare for spanning items - order the lanes by available span

		// 4) Stretch for spanning items.
		public void PushSpanningItem(int index, int span, float size)
		{
			if (this.Skip(index).Take(span).Any(l => l.IsPercent))
			{
				return;
			}

			var lanes = this.Skip(index).Take(span).ToList();
			size -= lanes.Sum(s => s.DesiredSize);

			if (size <= 0)
			{
				return;
			}

			lanes = lanes.OrderBy(AvailableSpan).ToList();

			for (int i = span; i >= 1; i--)
			{
				var lane = lanes[i - 1];

				float even = size / i;
				float available = AvailableSpan(lane);

				if (available <= even)
				{
					size -= available;
					lane.SpanningSize = Math.Max(lane.SpanningSize, available);
				}
				else
				{
					size -= even;
					lane.SpanningSize = Math.Max(lane.SpanningSize, even);
				}
			}
		}

		// 5) Span Percents on remaining size.
		public void Fill(float size)
		{
			size -= this.Sum(l => l.DesiredSize + l.SpanningSize);

			if (size <= 0)
			{
				return;
			}

			var minimums = this.Where(l => l.IsPercent).OrderBy(l => l.Minimum / l.Length).ToList();
			var maximums = this.Where(l => l.IsPercent).OrderBy(l => l.Maximum / l.Length).ToList();

			var minEnum = minimums.GetEnumerator();
			var maxEnum = maximums.GetEnumerator();

			if (!minEnum.MoveNext() || !maxEnum.MoveNext())
			{
				return;
			}

			var hasMinPoints = true;
			var hasMaxPoints = true;

			var multiplier = 0f;
			var percentMultiplier = 0f;
			var accumulatedSize = 0f;
			do
			{
				var minPoint = hasMinPoints ? minEnum.Current.Minimum / minEnum.Current.Length : 0;
				var maxPoint = hasMaxPoints ? maxEnum.Current.Maximum / maxEnum.Current.Length : 0;

				if (!hasMaxPoints || (hasMinPoints && hasMaxPoints && minPoint < maxPoint))
				{
					var gain = percentMultiplier * (minPoint - multiplier);

					if (accumulatedSize <= size && accumulatedSize + gain >= size)
					{
						multiplier += (size - accumulatedSize) / percentMultiplier;
						break;
					}

					accumulatedSize += gain;
					multiplier = minPoint;

					percentMultiplier += minEnum.Current.Length;
					hasMinPoints = minEnum.MoveNext();
				}
				else if (!hasMinPoints || (hasMinPoints && hasMaxPoints && minPoint >= maxPoint))
				{
					var gain = percentMultiplier * (maxPoint - multiplier);

					if (accumulatedSize <= size && accumulatedSize + gain >= size)
					{
						multiplier += (size - accumulatedSize) / percentMultiplier;
						break;
					}

					accumulatedSize += gain;
					multiplier = maxPoint;

					percentMultiplier -= minEnum.Current.Length;
					hasMaxPoints = maxEnum.MoveNext();
				}
				else
				{
					// we have nothing left... probably...
					System.Diagnostics.Debug.Assert(!hasMinPoints && !hasMaxPoints);
					break;
				}
			}
			while (true);

			foreach (var item in this.Where(l => l.IsPercent))
			{
				item.SpanningSize = Math.Max(item.Minimum, Math.Min(item.Maximum, item.Length * multiplier)) - item.DesiredSize;
			}
		}

		private static float AvailableSpan(GridBand lane)
		{
			return lane.IsAuto ? lane.Maximum - lane.DesiredSize : 0f;
		}
		#endregion

		#region Lanes Ownership
		protected override void ClearItems()
		{
			foreach (var lane in this)
			{
				lane.GridLayout = null;
			}

			base.ClearItems();
		}

		protected override void InsertItem(int index, GridBand lane)
		{
			if (lane == null)
			{
				throw new ArgumentNullException("lane");
			}

			if (lane.GridLayout != null)
			{
				throw new InvalidOperationException("GridLane allready a child of a GridLayout.");
			}

			base.InsertItem(index, lane);
			lane.GridLayout = this;
		}

		protected override void RemoveItem(int index)
		{
			GridBand lane = this[index];
			base.RemoveItem(index);
			lane.GridLayout = null;
		}

		protected override void SetItem(int index, GridBand lane)
		{
			var previousLane = this[index];

			if (previousLane == lane)
			{
				return;
			}

			if (lane == null)
			{
				throw new ArgumentNullException("lane");
			}

			if (lane.GridLayout != null)
			{
				throw new InvalidOperationException("GridLane allready a child of a GridLayout.");
			}

			base.SetItem(index, lane);
			previousLane.GridLayout = null;
			lane.GridLayout = this;
		}
		#endregion
	}
}
