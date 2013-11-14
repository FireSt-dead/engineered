using DirectDrawing;
using Drawing;
using Geometry;
using System;
using System.Diagnostics;
using UserInterface;
using UserInterface.Keyboard;
using UserInterface.Mouse;
using WindowsOS;

namespace WindowsUI
{
	public class Window : WindowBase
	{
		private HwndRenderTarget renderTarget;
		private Stopwatch stopwatch;

		private TimeSpan lastFrame;

		private MouseNotifications mouseNotifications;
        private bool hasMouse;

		private KeyboardNotifications keyboardNotifications;

		private Visual visual;

		public Window()
		{
			this.mouseNotifications = new MouseNotifications();
			this.keyboardNotifications = new KeyboardNotifications();

			renderTarget = DirectDrawing.Factory.Current.CreateHwndRenderTarget(this.Hwnd);
			stopwatch = Stopwatch.StartNew();
		}

		public Visual Visual
		{ 
			get
			{
				return this.visual;
			}

			set
			{
				if (this.visual != null)
				{
					KeyboardEvents.RequestKeyboard.RemoveHandler(this.visual, OnRequestKeyboard);
				}

				this.visual = value;

				if (this.visual != null)
				{
					KeyboardEvents.RequestKeyboard.AddHandler(this.visual, OnRequestKeyboard);
				}

				this.mouseNotifications.Root = value;
				this.mouseNotifications.UpdateMouse();
				this.keyboardNotifications.Root = value;
				this.keyboardNotifications.UpdateKeyboard();
			}
		}

		private void OnRequestKeyboard(KeyboardEvents args)
		{
			this.keyboardNotifications.KeyboardTarget(args.Source);
			this.keyboardNotifications.UpdateKeyboard();
		}

		protected override int OnPaint()
		{
			// System.Diagnostics.Debug.WriteLine("OnPaint " + DateTime.Now.Ticks);

			this.renderTarget.BeginDraw();

			IDrawingContext dc = this.renderTarget;

			renderTarget.Clear(new Color(1, 1, 1, 1));

			var black = renderTarget.CreateSolidColorBrush(new Color(1, 0, 0, 0));
			var yellow = renderTarget.CreateSolidColorBrush(new Color(1, 0, 1, 1));
			var blue = renderTarget.CreateSolidColorBrush(new Color(1, 0.2f, 0.5f, 1));

			var rect = new Rect() { Left = 100.5f, Top = 100.5f, Right = 200.5f, Bottom = 200.5f };

			dc.Fill = yellow;
			dc.FillRoundedRectangle(rect, 10f, 10f);
			dc.Stroke = black;
			dc.Thickness = 3f;
			dc.DrawRoundedRectangle(rect, 10f, 10f);

			var textFormat = dc.CreateTextFormat("Arial", 11);
			dc.TextFormat = textFormat;
			var textLayout = dc.CreateTextLayout("Hello World!", new Size(80f, 80f));

			Rect tp2Rect = textLayout.HitTestCharacter(3, CharacterSide.Leading).Rectangle;
			tp2Rect.Left += 110f;
			tp2Rect.Top += 110f;
			tp2Rect.Right += 110f;
			tp2Rect.Bottom += 110f;

			dc.Fill = blue;
			dc.Thickness = 1f;
			dc.FillRectangle(tp2Rect);

			dc.Foreground = black;
			dc.DrawTextLayout(textLayout, new Point(110f, 110f));

			Rect boundingBox = textLayout.BoundingBox;
			boundingBox.Left += 110f;
			boundingBox.Top += 110f;
			boundingBox.Right += 110f;
			boundingBox.Bottom += 110f;

			dc.Stroke = black;
			dc.Thickness = 1f;
			dc.DrawRectangle(boundingBox);

			dc.Stroke = black;
			dc.Fill = yellow;
			dc.Thickness = 1f;
			dc.Foreground = black;

			mouseNotifications.UpdateMouse();
			keyboardNotifications.UpdateKeyboard();

			if (this.Visual != null)
			{
				var size = new Size() { Width = this.Width, Height = this.Height };
				this.Visual.Update(this.stopwatch.Elapsed);
				this.Visual.Measure(size, this.renderTarget);
				this.Visual.Arrange(new Rect() { Left = 0, Top = 0, Right = size.Width, Bottom = size.Height }, this.renderTarget);
				this.Visual.Draw(this.renderTarget);
			}

			var current = this.stopwatch.Elapsed;
			var frame = current - lastFrame;
			double fps = 1000d / frame.TotalMilliseconds;
			this.renderTarget.TextFormat = textFormat;
			this.renderTarget.Foreground = black;
			this.renderTarget.DrawText("FPS: " + fps.ToString(), new Rect() { Left = 0, Top = 0, Right = 190, Bottom = 190 });
			this.lastFrame = this.stopwatch.Elapsed;

			this.renderTarget.EndDraw();

            this.UpdateCursor();

			return 1;
		}

		protected override int OnSize()
		{
			this.renderTarget.Resize(this.Width, this.Height);
			this.OnPaint();
			return 1;
		}

		protected override int OnChar(Char c)
		{
			Console.WriteLine("Char: " + c);
			this.keyboardNotifications.Char(c);
			return 1;
		}

		protected override int OnKeyDown(uint keyCode)
		{
			keyboardNotifications.KeyDown(keyCode);
			return 1;
		}

		protected override int OnKeyUp(uint keyCode)
		{
			keyboardNotifications.KeyUp(keyCode);
			return 1;
		}

		protected override int OnMouseMove(int x, int y)
		{
			this.mouseNotifications.MouseMove(new Point(x, y));
			this.mouseNotifications.UpdateMouse();
			this.OnPaint();
            this.hasMouse = true;
            this.UpdateCursor();
			return 1;
			// return base.OnMouseMove(x, y);
		}

        protected override int OnSetCursor()
        {
            this.UpdateCursor();
            return 1;
        }

		protected override int OnLeftButtonDown(int x, int y)
		{
			// System.Diagnostics.Debug.WriteLine("LeftButtonDown: {0}, {1}", x, y);
			this.mouseNotifications.MouseMove(new Point(x, y));
			this.mouseNotifications.MouseLeftButtonDown();
			this.OnPaint();

			return 1;
			// return base.OnLeftButtonDown(x, y);
		}

		protected override int OnLeftButtonDoubleClick(int x, int y)
		{
			this.OnLeftButtonDown(x, y);
			return base.OnLeftButtonDoubleClick(x, y);
		}

		protected override int OnLeftButtonUp(int x, int y)
		{
			// System.Diagnostics.Debug.WriteLine("LeftButtonUp: {0}, {1}", x, y);
			mouseNotifications.MouseMove(new Point(x, y));
			mouseNotifications.UpdateMouse();
			mouseNotifications.MouseLeftButtonUp();
			this.OnPaint();
			return base.OnLeftButtonUp(x, y);
		}

		protected override void OnCaptureChanged()
		{
			base.OnCaptureChanged();
			this.OnPaint();
		}

		protected override void OnMouseLeave()
		{
			this.mouseNotifications.MouseLeave();
			this.mouseNotifications.UpdateMouse();
			this.OnPaint();
            this.hasMouse = true;
			base.OnMouseLeave();
		}

        private void UpdateCursor()
        {
            if (this.hasMouse)
            {
                var cursor = this.mouseNotifications.SystemCursor ?? SystemCursor.Normal;
                this.SetSystemCursor((int)cursor);
            }
        }
	}
}
