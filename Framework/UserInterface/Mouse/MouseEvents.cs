using Geometry;
using System;

namespace UserInterface.Mouse
{
	public sealed class MouseEvents
	{
		public static readonly Event<MouseEvents> Enter = new Event<MouseEvents>();
		public static readonly Event<MouseEvents> Leave = new Event<MouseEvents>();
        public static readonly Event<MouseEvents> Move = new Event<MouseEvents>();
        public static readonly Event<MouseEvents> Drag = new Event<MouseEvents>();

		public static readonly Event<MouseEvents> LeftButtonDown = new Event<MouseEvents>();
		public static readonly Event<MouseEvents> LeftButtonUp = new Event<MouseEvents>();

		public Point Position { get; internal set; }
        public SystemCursor? Cursor { get; set; }
	}
}
