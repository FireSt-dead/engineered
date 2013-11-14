using Geometry;
using Drawing;

namespace UserInterface.Primitives
{
	public class Grid : VisualContainer
	{
		public static readonly Property<int> Row = new Property<int>(0);
		public static readonly Property<int> Column = new Property<int>(0);
		public static readonly Property<int> RowSpan = new Property<int>(1);
		public static readonly Property<int> ColumnSpan = new Property<int>(1);

		public Grid()
		{
			this.Rows = new GridLane();
			this.Columns = new GridLane();
		}

		public GridLane Rows { get; private set; }
		public GridLane Columns { get; private set; }

		// TODO:
		protected override Size MeasureContent(Size upToSize, IDrawingContext context)
		{
			return base.MeasureContent(upToSize, context);
		}

		protected override void ArrangeContent(Size size, IDrawingContext context)
		{
			base.ArrangeContent(size, context);
		}
	}
}
