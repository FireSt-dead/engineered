#pragma once

#include "stdafx.h"

#include "Brush.h"
#include "SolidColorBrush.h"
#include "TextFormat.h"
#include "TextLayout.h"

using namespace System;

using namespace Geometry;

namespace DirectDrawing {

	public ref class HwndRenderTarget : public IDrawingContext
	{
	private:
		ID2D1HwndRenderTarget* renderTarget;
		ID2D1Factory* directWriteFactory;
		AffineMatrix affineTransform;

		float thickness;

		Brush^ stroke;
		Brush^ fill;
		Brush^ foreground;

		TextFormat^ textFormat;

	internal:
		HwndRenderTarget(ID2D1HwndRenderTarget* rt);

	public:
		virtual property float Thickness
		{
			float get() sealed;
			void set(float thickness) sealed;
		}

		virtual property IBrush^ Stroke
		{
			IBrush^ get() sealed;
			void set(IBrush^ brush) sealed;
		}

		virtual property IBrush^ Fill
		{
			IBrush^ get() sealed;
			void set(IBrush^ brush) sealed;
		}

		virtual property IBrush^ Foreground
		{
			IBrush^ get() sealed;
			void set(IBrush^ brush) sealed;
		}

		virtual property ITextFormat^ TextFormat
		{
			ITextFormat^ get() sealed;
			void set(ITextFormat^ brush) sealed;
		}

		virtual property AffineMatrix AffineTransform
		{
			AffineMatrix get() sealed;
			void set(AffineMatrix matrix) sealed;
		}

		void BeginDraw();
		void EndDraw();
		void Clear(Color^ color);

		void DrawRectangle(Rect rect, SolidColorBrush^ brush, float thickness);
		void FillRectangle(Rect rect, SolidColorBrush^ brush);
		void DrawRoundedRectangle(Rect rect, float radiusX, float radiusY, SolidColorBrush^ brush, float thickness);
		void FillRoundedRectangle(Rect rect, float radiusX, float radiusY, SolidColorBrush^ brush);

		void DrawText(String^ text, Rect^ rect, DirectDrawing::TextFormat^ textFormat, Brush^ brush);
		void DrawTextLayout(Point origin, TextLayout^ textLayout, Brush^ brush);

		void Resize(unsigned int width, unsigned int height)
		{
			D2D_SIZE_U size = D2D1::SizeU(width, height);
			renderTarget->Resize(&size);
		}

		virtual ISolidColorBrush^ CreateSolidColorBrush(Color color);
		virtual ITextFormat^ CreateTextFormat(String^ font, float size);
		virtual ITextLayout^ CreateTextLayout(String^ text, Size size);

		virtual void DrawRectangle(Rect rect);
		virtual void FillRectangle(Rect rect);
		virtual void DrawRoundedRectangle(Rect rect, float radiusX, float radiusY);
		virtual void FillRoundedRectangle(Rect rect, float radiusX, float radiusY);

		virtual void DrawText(String^ text, Rect rect);
		virtual void DrawTextLayout(ITextLayout^ textLayout, Point origin);
	};
}
