using System.Globalization;
using Drawing;
using Geometry;
using UserInterface.Keyboard;
using UserInterface.Mouse;
using System;

namespace UserInterface.Primitives
{
	public class TextField : Visual
	{
		private string text;
		bool isFocused;

		IBrush background;
		IBrush borderBrush;
		IBrush borderBrushFocused;
		IBrush carretBrush;

		IBrush foreground;
		ITextFormat textFormat;
		ITextLayout textLayout;

		uint carretPosition;
		uint selectionStart;
		uint selectionEnd;

		public TextField()
		{
			MouseEvents.LeftButtonDown.AddHandler(this, OnMouseLeftButtonDown);
            MouseEvents.Drag.AddHandler(this, OnMouseDrag);
            MouseEvents.Move.AddHandler(this, OnMouseMove);

			KeyboardEvents.Char.AddHandler(this, OnKeyboardChar);
			KeyboardEvents.KeyDown.AddHandler(this, OnKeyboardKeyDown);

			KeyboardEvents.Enter.AddHandler(this, OnKeyboardEnter);
			KeyboardEvents.Leave.AddHandler(this, OnKeyboardLeave);
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
				this.textLayout = null;
			}
		}

		protected override Size MeasureContent(Size size, IDrawingContext context)
		{
			this.textFormat = this.textFormat ?? context.CreateTextFormat("Arial", 11);
			this.textLayout = this.textLayout ?? context.CreateTextLayout(this.Text, size);
			var boundingBox = this.textLayout.BoundingBox;
			return boundingBox.Size;
		}

		protected override void DrawContent(IDrawingContext context)
		{
			this.foreground = this.foreground ?? context.CreateSolidColorBrush(new Color(1f, 0f, 0f, 0f));
			this.background = this.background ?? context.CreateSolidColorBrush(new Color(1f, 1f, 1f, 1f));
			this.borderBrush = this.borderBrush ?? context.CreateSolidColorBrush(new Color(1f, 0f, 0f, 0f));
			this.borderBrushFocused = this.borderBrushFocused ?? context.CreateSolidColorBrush(new Color(1f, 1f, 0.8f, 0f));
			this.carretBrush = this.carretBrush ?? context.CreateSolidColorBrush(new Color(1f, 1f, 0f, 0f));

			context.Fill = this.background;
			context.Stroke = this.isFocused ? this.borderBrushFocused : this.borderBrush;
			context.Foreground = this.foreground;
			context.Thickness = 2f;

			Rect rect = new Rect(0, 0, this.ArrangedRect.Width, this.ArrangedRect.Height);

			context.FillRectangle(rect);
			context.DrawRectangle(rect);

            uint start = Math.Min(selectionStart, selectionEnd);
            uint length = Math.Max(selectionStart, selectionEnd) - Math.Min(selectionStart, selectionEnd);
            this.textLayout.SetForeground(this.foreground, 0, (uint)this.Text.Length);
            this.textLayout.SetForeground(this.background, start, length);
            var selection = this.textLayout.HitTestRange(start, length);
            foreach (var selectionRect in selection)
            {
                context.Fill = this.foreground;
                context.Stroke = this.foreground;
                context.Thickness = 1f;
                context.FillRectangle(selectionRect);
            }

			if (this.isFocused)
			{
				var charLocation = this.textLayout.HitTestCharacter(this.carretPosition, CharacterSide.Leading);
				context.Fill = this.carretBrush;
				context.Stroke = this.carretBrush;
				context.Thickness = 1f;
				context.DrawRectangle(new Rect(charLocation.Rectangle.Left, charLocation.Rectangle.Top, charLocation.Rectangle.Left, charLocation.Rectangle.Bottom));
			}

			context.DrawTextLayout(this.textLayout, new Point(0, 0));

			base.DrawContent(context);
		}

		public override bool HitTest(Point point)
		{
			return new Rect(0, 0, this.ArrangedRect.Width, this.ArrangedRect.Height).Contains(point);
		}

		private void OnMouseLeftButtonDown(MouseEvents e)
		{
			if (this.textLayout != null)
			{
				var point = e.Position;
				CharacterHit hit = this.textLayout.HitTestPoint(point);
                var clickCharacter = hit.Position + (hit.Side == CharacterSide.Leading ? 0u : 1u);
				this.carretPosition = clickCharacter;
                this.selectionStart = clickCharacter;
                this.selectionEnd = clickCharacter;
			}

			KeyboardEvents.RequestKeyboard.RaiseRouted(new KeyboardEvents() { Source = this }, this);
		}

        private void OnMouseDrag(MouseEvents e)
        {
            if (this.textLayout != null)
            {
                var point = e.Position;
                CharacterHit hit = this.textLayout.HitTestPoint(point);
                var clickCharacter = hit.Position + (hit.Side == CharacterSide.Leading ? 0u : 1u);
                this.carretPosition = clickCharacter;
                this.selectionEnd = clickCharacter;
            }

            e.Cursor = SystemCursor.IBeam;
        }

        private void OnMouseMove(MouseEvents e)
        {
            e.Cursor = SystemCursor.IBeam;
        }

		private void OnKeyboardChar(KeyboardEvents e)
		{
			if (e.InputChar != '\b')
			{
				this.Text = this.Text.Insert((int)this.carretPosition, e.InputChar.ToString(CultureInfo.InvariantCulture));
				this.carretPosition++;
			}
		}

		private void OnKeyboardKeyDown(KeyboardEvents e)
		{
			switch (e.KeyCode)
			{
				case 0x2E: // delete
					if (carretPosition >= 0 && carretPosition < this.Text.Length)
					{
						this.Text = this.Text.Substring(0, (int)carretPosition) + this.Text.Substring((int)carretPosition + 1);
					}
					break;
				case 0x08: // backspace
					if (carretPosition > 0 && carretPosition <= this.Text.Length)
					{
						carretPosition = carretPosition - 1;
						this.Text = this.Text.Substring(0, (int)carretPosition) + this.Text.Substring((int)carretPosition + 1);
					}
					break;
				case 0x25: // left
					if (carretPosition > 0)
					{
						carretPosition--;
					}
					break;
				case 0x27: // right
					if (carretPosition < this.Text.Length)
					{
						carretPosition++;
					}
					break;
				default:
					break;
			}
		}

		private void OnKeyboardEnter(KeyboardEvents e)
		{
			this.isFocused = true;
		}

		private void OnKeyboardLeave(KeyboardEvents e)
		{
			this.isFocused = false;
		}
	}
}
