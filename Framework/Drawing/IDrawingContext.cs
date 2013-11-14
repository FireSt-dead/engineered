using Geometry;

namespace Drawing
{
	public interface IDrawingContext
	{
		float Thickness { get; set; }

		IBrush Stroke { get; set; }
		IBrush Fill { get; set; }
		IBrush Foreground { get; set; }

		ITextFormat TextFormat { get; set; }

		ISolidColorBrush CreateSolidColorBrush(Color color);
		ITextFormat CreateTextFormat(string font, float size);
		ITextLayout CreateTextLayout(string text, Size size);

		AffineMatrix AffineTransform { get; set; }

		void DrawRectangle(Rect rect);
		void FillRectangle(Rect rect);
		void DrawRoundedRectangle(Rect rect, float radiusX, float radiusY);
		void FillRoundedRectangle(Rect rect, float radiusX, float radiusY);

		void DrawText(string text, Rect rect);
		void DrawTextLayout(ITextLayout textLayout, Point point);
	}
}
