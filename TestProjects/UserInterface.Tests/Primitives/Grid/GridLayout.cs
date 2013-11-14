using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserInterface.Primitives;

namespace UserInterface.Tests.Primitives.Grid
{
	/// <summary>
	/// Summary description for GridLayout
	/// </summary>
	[TestClass]
	public class GridLayoutTests
	{
#region Autos
		[TestMethod]
		public void FixedSizeWontStretch()
		{
			var layout = new GridLane()
			{
				new GridBand() { Length = 10, Unit = GridBandUnit.Points },
				new GridBand() { Length = 10, Unit = GridBandUnit.Points }
			};

			layout.Reset();

			layout.PushSingleBandItem(0, 5f);
			layout.PushSingleBandItem(0, 10f);
			layout.PushSingleBandItem(0, 15f);
			layout.PushSingleBandItem(1, 15f);
			layout.PushSingleBandItem(1, 10f);
			layout.PushSingleBandItem(1, 5f);

			layout.Fill(50f);

			Assert.AreEqual(10f, layout[0].ArrangedSize);
			Assert.AreEqual(10f, layout[0].ArrangedSize);
		}

		[TestMethod]
		public void AutoSizeNonSpanningItems()
		{
			var layout = new GridLane()
			{
				new GridBand() { Unit = GridBandUnit.Auto },
				new GridBand() { Unit = GridBandUnit.Auto }
			};

			layout.Reset();

			layout.PushSingleBandItem(0, 10f);
			layout.PushSingleBandItem(0, 5f);
			layout.PushSingleBandItem(1, 5f);
			layout.PushSingleBandItem(1, 10f);

			layout.Fill(50f);

			Assert.AreEqual(10f, layout[0].ArrangedSize);
			Assert.AreEqual(10f, layout[0].ArrangedSize);
		}

		[TestMethod]
		public void AutoSizeWithSpanningItems()
		{
			var layout = new GridLane()
			{
				new GridBand() { Unit = GridBandUnit.Auto },
				new GridBand() { Unit = GridBandUnit.Auto }
			};

			layout.Reset();

			layout.PushSingleBandItem(0, 10f);
			layout.PushSingleBandItem(0, 5f);
			layout.PushSingleBandItem(1, 10f);

			layout.PushSpanningItem(0, 2, 30f);
			layout.PushSpanningItem(0, 2, 25f);

			layout.Fill(50f);

			Assert.AreEqual(15f, layout[0].ArrangedSize);
			Assert.AreEqual(15f, layout[1].ArrangedSize);
		}
#endregion

#region Percents
		[TestMethod]
		public void SimplePercent()
		{
			var layout = new GridLane()
			{
				new GridBand() { Unit = GridBandUnit.Auto },
				new GridBand() { Length = 2, Unit = GridBandUnit.Percent },
				new GridBand() { Length = 1, Unit = GridBandUnit.Percent },
				new GridBand() { Unit = GridBandUnit.Auto }
			};

			layout.Reset();

			layout.PushSingleBandItem(0, 10f);
			layout.PushSingleBandItem(3, 10f);

			layout.Fill(50f);

			Assert.AreEqual(10f, layout[0].ArrangedSize);
			Assert.AreEqual(20f, layout[1].ArrangedSize);
			Assert.AreEqual(10f, layout[2].ArrangedSize);
			Assert.AreEqual(10f, layout[3].ArrangedSize);
		}

		[TestMethod]
		public void MinWidthPercent()
		{
			var layout = new GridLane()
			{
				new GridBand() { Unit = GridBandUnit.Auto },
				new GridBand() { Length = 2, Unit = GridBandUnit.Percent },
				new GridBand() { Length = 1, Unit = GridBandUnit.Percent, Minimum = 15f },
				new GridBand() { Unit = GridBandUnit.Auto }
			};

			layout.Reset();

			layout.PushSingleBandItem(0, 10f);
			layout.PushSingleBandItem(3, 10f);

			layout.Fill(50f);

			Assert.AreEqual(10f, layout[0].ArrangedSize);
			Assert.AreEqual(15f, layout[1].ArrangedSize);
			Assert.AreEqual(15f, layout[2].ArrangedSize);
			Assert.AreEqual(10f, layout[3].ArrangedSize);
		}

		[TestMethod]
		public void SpanningThroughPercentsDoesAffectLanes()
		{
			var layout = new GridLane()
			{
				new GridBand() { Unit = GridBandUnit.Auto },
				new GridBand() { Length = 1, Unit = GridBandUnit.Percent },
				new GridBand() { Unit = GridBandUnit.Auto }
			};

			layout.Reset();

			layout.PushSingleBandItem(0, 20f);
			layout.PushSingleBandItem(1, 20f);
			layout.PushSingleBandItem(2, 20f);

			layout.PushSpanningItem(0, 2, 100f);
			layout.PushSpanningItem(1, 2, 100f);

			layout.Fill(150f);

			Assert.AreEqual(20f, layout[0].ArrangedSize);
			Assert.AreEqual(110f, layout[1].ArrangedSize);
			Assert.AreEqual(20f, layout[2].ArrangedSize);
		}
#endregion

#region UseCases
		[TestMethod]
		public void UseCase_ScrollBar()
		{
			var length = new GridLane()
			{
				new GridBand() { Unit = GridBandUnit.Auto },
				new GridBand() { Length = 1, Unit = GridBandUnit.Percent },
				new GridBand() { Unit = GridBandUnit.Auto }
			};

			length.Reset();
			
			length.PushSingleBandItem(0, 24f);
			length.PushSingleBandItem(1, 24f);
			length.PushSingleBandItem(2, 24f);

			length.Fill(100);

			Assert.AreEqual(24f, length[0].ArrangedSize);
			Assert.AreEqual(52f, length[1].ArrangedSize);
			Assert.AreEqual(24f, length[2].ArrangedSize);
		}
#endregion
	}
}
