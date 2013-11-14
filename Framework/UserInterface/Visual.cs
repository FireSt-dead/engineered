using Drawing;
using Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using UserInterface.Layout;

namespace UserInterface
{
	public class Visual : IEnumerable<Visual>, IAnnotatable
	{
		private Dictionary<object, object> annotations;

		public Visual()
		{
			this.annotations = new Dictionary<object, object>();
		}

		public Visual Parent { get; internal set; }

		public Rect Margin { get; set; }

		public Alignment HorizontalAlignment { get; set; }
		public Alignment VerticalAlignment { get; set; }

		public Size DesiredSize { get; protected set; }
		public Rect ArrangedRect { get; protected set; }

		public float Rotation { get; set; }
		public Point Translation { get; set; }
		// public double Skew { get; set; }

		public string Name { get; set; }

		public void SetAnnotation(object key, object value)
		{
			this.annotations[key] = value;
		}

		public bool TryGetAnnotation(object key, out object value)
		{
			return this.annotations.TryGetValue(key, out value);
		}

		public void RemoveAnnotation(object key)
		{
			this.annotations.Remove(key);
		}

		public virtual bool HitTest(Point point)
		{
			return false;
		}

		public void Update(TimeSpan ellapsedTime)
		{
			this.UpdateContent(ellapsedTime);
		}

		public void Measure(Size availableSize, IDrawingContext context)
		{
			availableSize.Intrude(this.Margin);
			var desiredSize = this.MeasureContent(availableSize, context);
			desiredSize.Extrude(this.Margin);
			this.DesiredSize = desiredSize;
		}

		public void Arrange(Rect rect, IDrawingContext context)
		{
			if (this.HorizontalAlignment != null)
			{
				var horizontalInterval = this.HorizontalAlignment.Align(new Interval() { Start = rect.Left, End = rect.Right }, this.DesiredSize.Width);
				rect.Left = horizontalInterval.Start;
				rect.Right = horizontalInterval.End;
			}

			if (this.VerticalAlignment != null)
			{
				var verticalInterval = this.VerticalAlignment.Align(new Interval() { Start = rect.Top, End = rect.Bottom }, this.DesiredSize.Height);
				rect.Top = verticalInterval.Start;
				rect.Bottom = verticalInterval.End;
			}

			rect.Intrude(this.Margin);
			this.ArrangedRect = rect;
			this.ArrangeContent(rect.Size, context);
		}

		public void Draw(IDrawingContext context)
		{
			var parentTransform = context.AffineTransform;

			// Apply layout transformations.
			context.AffineTransform = Transform(parentTransform);

			this.DrawContent(context);

			// Roll-back to parent space.
			context.AffineTransform = parentTransform;
		}

        protected virtual void UpdateContent(TimeSpan time)
        {
            foreach (var child in this)
            {
                child.Update(time);
            }
        }

        protected virtual Size MeasureContent(Size upToSize, IDrawingContext context)
        {
            Size maxSize = new Size();
            foreach (var child in this)
            {
                child.Measure(upToSize, context);
                maxSize = Size.Max(child.DesiredSize, maxSize);
            }

            return maxSize;
        }

        protected virtual void ArrangeContent(Size size, IDrawingContext context)
        {
            Rect childRect = new Rect(size);
            foreach (var child in this)
            {
                child.Arrange(childRect, context);
            }
        }

        protected virtual void DrawContent(Drawing.IDrawingContext context)
        {
            foreach (var child in this)
            {
                child.Draw(context);
            }
        }

		public virtual IEnumerator<Visual> GetEnumerator()
		{
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public AffineMatrix Transform(AffineMatrix contentTransform)
		{
			contentTransform.Translate(this.ArrangedRect.TopLeft);
			contentTransform.Translate(this.Translation);
			return contentTransform;
		}

		public Point Transform(Point mousePoint)
		{
			// Use the inverted Transform matrix instead
			mousePoint.X = mousePoint.X - this.ArrangedRect.Left - this.Translation.X;
			mousePoint.Y = mousePoint.Y - this.ArrangedRect.Top - this.Translation.Y;
			return mousePoint;
		}

		public override string ToString()
		{
			return this.Name == null ? base.ToString() : string.Concat(this.Name, " ", base.ToString());
		}
	}
}
