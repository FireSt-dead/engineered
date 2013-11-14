using Drawing;
using Geometry;
using UserInterface;
using UserInterface.Mouse;

namespace DirectXTestApp
{
	public class MyButton : Composite
	{
		private bool isMouseOver;
		private bool isPressed;

		private Visual content;

		public MyButton()
		{
			MouseEvents.Enter.AddHandler(this, OnMouseEnter);
			MouseEvents.Leave.AddHandler(this, OnMouseLeave);
			MouseEvents.LeftButtonDown.AddHandler(this, OnMouseLeftButtonDown);
			MouseEvents.LeftButtonUp.AddHandler(this, OnMouseLeftButtonUp);
            MouseEvents.Move.AddHandler(this, OnMouseMove);
            MouseEvents.Drag.AddHandler(this, OnMouseDrag);
            //MouseCursor.System.SetValue(this, SystemCursor.Hand);
		}

        private void OnMouseMove(MouseEvents e)
        {
            e.Cursor = e.Cursor ?? SystemCursor.Hand;
        }

        private void OnMouseDrag(MouseEvents e)
        {
            e.Cursor = e.Cursor ?? SystemCursor.Hand;
        }

		public Visual Content
		{
			get
			{
				return this.content;
			}

			set
			{
				if (this.content != value)
				{
					this.content = value;
					if (this.content != null)
					{
						this.Children.Add(value);
					}
				}
			}
		}

		protected override Size MeasureContent(Size size, IDrawingContext context)
		{
			var childSize = base.MeasureContent(size, context);
			return Size.Max(new Size(22f, 22f), childSize);
		}

		protected override void DrawContent(IDrawingContext context)
		{
			if (this.isPressed)
			{
				context.Fill = context.CreateSolidColorBrush(new Color(0.2f, 1.0f, 0.4f));
			}
			else if (this.isMouseOver)
			{
				context.Fill = context.CreateSolidColorBrush(new Color(0f, 0.8f, 0.2f));
			}
			else
			{
				context.Fill = context.CreateSolidColorBrush(new Color(0f, 0.4f, 0.1f));
			}

			var rect = new Rect(0.5f, 0.5f, this.ArrangedRect.Width - 0.5f, this.ArrangedRect.Height - 0.5f);
			context.Thickness = 1f;
			context.Stroke = context.CreateSolidColorBrush(new Color(1f, 0f, 0f, 0f));
			context.DrawRectangle(rect);
			context.FillRectangle(rect);

			base.DrawContent(context);
		}

		public override bool HitTest(Point point)
		{
			return new Rect(0, 0, this.ArrangedRect.Width, this.ArrangedRect.Height).Contains(point);
		}

		private void OnMouseEnter(MouseEvents args)
		{
			this.isMouseOver = true;
		}

		private void OnMouseLeave(MouseEvents args)
		{
			this.isMouseOver = false;
			this.isPressed = false;
		}

		private void OnMouseLeftButtonDown(MouseEvents args)
		{
			this.isPressed = true;
		}

		private void OnMouseLeftButtonUp(MouseEvents args)
		{
			this.isPressed = false;
		}
	}
}
