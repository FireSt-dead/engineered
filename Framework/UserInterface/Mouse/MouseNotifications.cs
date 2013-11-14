using Geometry;
using System.Collections.Generic;
using System.Linq;

namespace UserInterface.Mouse
{
	public class MouseNotifications
	{
		private IList<Visual> mouseOveredElements;
		private Point mousePosition;
		private bool hasMouse;
        private bool pressed;

		public MouseNotifications()
		{
			this.hasMouse = false;
			this.mouseOveredElements = new List<Visual>();
		}

		public Visual Root { get; set; }

        public SystemCursor? SystemCursor { get; private set; }

		public void MouseMove(Point position)
		{
			this.mousePosition = position;
            var args = new MouseEvents();
            this.RaiseMouseEvent(this.pressed ? MouseEvents.Drag : MouseEvents.Move, args);
            this.SystemCursor = args.Cursor;
			this.hasMouse = true;
		}

		public void MouseLeave()
		{
			this.hasMouse = false;
		}

		public void UpdateMouse()
		{
            if (pressed)
            {
                return;
            }

			var newMouseOveredElements = new List<Visual>();

			if (this.hasMouse && this.Root != null)
			{
				var rootPoint = this.Root.Transform(this.mousePosition);
				Visual hit = FindHit(this.Root, rootPoint);

				while (hit != null)
				{
					newMouseOveredElements.Add(hit);
					hit = hit.Parent;
				}
			}

			int oldDifferenceIndex = mouseOveredElements.Count - 1;
			int newDifferenceIndex = newMouseOveredElements.Count - 1;

			while (oldDifferenceIndex >= 0 && newDifferenceIndex >= 0 && mouseOveredElements[oldDifferenceIndex] == newMouseOveredElements[newDifferenceIndex])
			{
				oldDifferenceIndex--;
				newDifferenceIndex--;
			}

			// Raise Leave events
			for (int i = 0; i <= oldDifferenceIndex; i++)
			{
				var visual = mouseOveredElements[i];
				MouseEvents.Leave.Raise(visual, null);
			}

			this.mouseOveredElements = newMouseOveredElements;

			// Raise Enter event
			var mousePoints = ListLocalMousePositions();
			var args = new MouseEvents();
			for (int i = newDifferenceIndex; i >= 0; i--)
			{
				args.Position = mousePoints[i];
				MouseEvents.Enter.Raise(this.mouseOveredElements[i], args);
			}
		}

		public void MouseLeftButtonDown()
		{
            RaiseMouseEvent(MouseEvents.LeftButtonDown, new MouseEvents());
            this.pressed = true;
		}

		public void MouseLeftButtonUp()
		{
			RaiseMouseEvent(MouseEvents.LeftButtonUp, new MouseEvents());
            this.pressed = false;
		}

        private void RaiseMouseEvent(Event<MouseEvents> @event, MouseEvents args)
		{
			var mousePoints = ListLocalMousePositions();
			for (int i = 0; i < this.mouseOveredElements.Count - 1; i++)
			{
				args.Position = mousePoints[i];
				@event.Raise(this.mouseOveredElements[i], args);
			}
		}

		private Point[] ListLocalMousePositions()
		{
			var position = this.mousePosition;
			var positions = new Point[this.mouseOveredElements.Count];
			for (int i = this.mouseOveredElements.Count - 1; i >= 0; i--)
			{
				Visual v = this.mouseOveredElements[i];
				position = v.Transform(position);
				positions[i] = position;
			}
			return positions;
		}

		// TODO: Implement with stack recurrsion.
		private Visual FindHit(Visual element, Point elementPoint)
		{
			foreach (var child in element.Reverse())
			{
				Point transformedPoint = child.Transform(elementPoint);
				var childHit = FindHit(child, transformedPoint);
				if (childHit != null)
				{
					return childHit;
				}
			}

			if (element.HitTest(elementPoint))
			{
				return element;
			}

			return null;
		}
    }
}
