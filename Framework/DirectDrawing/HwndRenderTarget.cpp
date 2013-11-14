#include "StdAfx.h"
#include "HwndRenderTarget.h"
#include "WriteFactory.h"

using namespace Geometry;

namespace DirectDrawing {

	HwndRenderTarget::HwndRenderTarget(ID2D1HwndRenderTarget* rt)
	{
		affineTransform = AffineMatrix::Identity;
		renderTarget = rt;
	}

	float HwndRenderTarget::Thickness::get()
	{
		return thickness;
	}

	void HwndRenderTarget::Thickness::set(float t)
	{
		thickness = t;
	}

	IBrush^ HwndRenderTarget::Stroke::get()
	{
		return stroke;
	}

	void HwndRenderTarget::Stroke::set(IBrush^ brush)
	{
		stroke = safe_cast<Brush^>(brush);
	}

	IBrush^ HwndRenderTarget::Fill::get()
	{
		return fill;
	}

	void HwndRenderTarget::Fill::set(IBrush^ brush)
	{
		fill = safe_cast<Brush^>(brush);
	}

	IBrush^ HwndRenderTarget::Foreground::get()
	{
		return foreground;
	}

	void HwndRenderTarget::Foreground::set(IBrush^ brush)
	{
		foreground = safe_cast<Brush^>(brush);
	}

	ITextFormat^ HwndRenderTarget::TextFormat::get()
	{
		return textFormat;
	}

	void HwndRenderTarget::TextFormat::set(ITextFormat^ format)
	{
		textFormat = safe_cast<DirectDrawing::TextFormat^>(format);
	}

	AffineMatrix HwndRenderTarget::AffineTransform::get()
	{
		return affineTransform;
	}

	void HwndRenderTarget::AffineTransform::set(AffineMatrix matrix)
	{
		renderTarget->SetTransform(D2D1::Matrix3x2F::Matrix3x2F(matrix.A11, matrix.A12, matrix.A21, matrix.A22, matrix.T1, matrix.T2));
		affineTransform = matrix;
	}

	void HwndRenderTarget::BeginDraw()
	{
		renderTarget->BeginDraw();
	}

	void HwndRenderTarget::EndDraw()
	{
		renderTarget->EndDraw();
	}

	void HwndRenderTarget::Clear(Color^ color)
	{
		renderTarget->Clear(D2D1::ColorF(color->Red, color->Green, color->Blue, color->Alpha));
	}

	ISolidColorBrush^ HwndRenderTarget::CreateSolidColorBrush(Color color)
	{
		HRESULT hr = S_OK;
		ID2D1SolidColorBrush* solidColorBrush;
		hr = renderTarget->CreateSolidColorBrush(D2D1::ColorF(color.Red, color.Green, color.Blue, color.Alpha), &solidColorBrush);
		return gcnew SolidColorBrush(solidColorBrush);
	}

	ITextFormat^ HwndRenderTarget::CreateTextFormat(String^ font, float size)
	{
		return WriteFactory::Current->CreateTextFormat(font, size);
	}

	ITextLayout^ HwndRenderTarget::CreateTextLayout(String^ text, Size size)
	{
		return WriteFactory::Current->CreateTextLayout(text, textFormat, size);
	}

	void HwndRenderTarget::DrawRectangle(Rect rect, SolidColorBrush^ brush, float thickness)
	{
		D2D1_RECT_F rectangle = D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom);
		renderTarget->DrawRectangle(&rectangle, brush->brush, thickness);
	}

	void HwndRenderTarget::FillRectangle(Rect rect, SolidColorBrush^ brush)
	{
		D2D1_RECT_F rectangle = D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom);
		renderTarget->FillRectangle(&rectangle, brush->brush);
	}

	void HwndRenderTarget::DrawRoundedRectangle(Rect rect, float radiusX, float radiusY, SolidColorBrush^ brush, float thickness)
	{
		D2D1_ROUNDED_RECT roundedRect = D2D1::RoundedRect(D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom), radiusX, radiusY);
		renderTarget->DrawRoundedRectangle(roundedRect, brush->brush, thickness);
	}

	void HwndRenderTarget::FillRoundedRectangle(Rect rect, float radiusX, float radiusY, SolidColorBrush^ brush)
	{
		D2D1_ROUNDED_RECT roundedRect = D2D1::RoundedRect(D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom), radiusX, radiusY);
		renderTarget->FillRoundedRectangle(roundedRect, brush->brush);
	}

	void HwndRenderTarget::DrawText(String^ text, Rect^ rect, DirectDrawing::TextFormat^ format, Brush^ brush)
	{
		// Pin memory so GC can't move it while native function is called
		pin_ptr<const wchar_t> wCharText = PtrToStringChars(text);
		D2D1_RECT_F rectangle = D2D1::RectF(rect->Left, rect->Top, rect->Right, rect->Bottom);
		renderTarget->DrawText(wCharText, text->Length, format->textFormat, &rectangle, brush->brush);
	}

	void HwndRenderTarget::DrawTextLayout(Point origin, TextLayout^ textLayout, Brush^ brush)
	{
		D2D1_POINT_2F originP2F = D2D1::Point2F(origin.X, origin.Y);
		renderTarget->DrawTextLayout(originP2F, textLayout->textLayout, brush->brush);
	}

	void HwndRenderTarget::DrawRectangle(Rect rect)
	{
		D2D1_RECT_F rectangle = D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom);
		// TODO: Move validation in the set Stroke method. Cache the Brush^.
		Brush^ b = safe_cast<Brush^>(stroke);
		renderTarget->DrawRectangle(&rectangle, b->brush, thickness);
	}

	void HwndRenderTarget::FillRectangle(Rect rect)
	{
		D2D1_RECT_F rectangle = D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom);
		// TODO: Move validation in the set Stroke method. Cache the Brush^.
		Brush^ b = safe_cast<Brush^>(fill);
		renderTarget->FillRectangle(&rectangle, b->brush);
	}

	void HwndRenderTarget::DrawRoundedRectangle(Rect rect, float radiusX, float radiusY)
	{
		D2D1_ROUNDED_RECT roundedRect = D2D1::RoundedRect(D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom), radiusX, radiusY);
		renderTarget->DrawRoundedRectangle(roundedRect, stroke->brush, thickness);
	}

	void HwndRenderTarget::FillRoundedRectangle(Rect rect, float radiusX, float radiusY)
	{
		D2D1_ROUNDED_RECT roundedRect = D2D1::RoundedRect(D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom), radiusX, radiusY);
		renderTarget->FillRoundedRectangle(roundedRect, fill->brush);
	}

	void HwndRenderTarget::DrawText(String^ text, Rect rect)
	{
		// Pin memory so GC can't move it while native function is called
		pin_ptr<const wchar_t> wCharText = PtrToStringChars(text);
		D2D1_RECT_F rectangle = D2D1::RectF(rect.Left, rect.Top, rect.Right, rect.Bottom);
		renderTarget->DrawText(wCharText, text->Length, textFormat->textFormat, &rectangle, foreground->brush);
	}

	void HwndRenderTarget::DrawTextLayout(ITextLayout^ textLayout, Point origin)
	{
		TextLayout^ dTextLayout = safe_cast<TextLayout^>(textLayout);
		D2D1_POINT_2F originP2F = D2D1::Point2F(origin.X, origin.Y);
		renderTarget->DrawTextLayout(originP2F, dTextLayout->textLayout, foreground->brush);
	}
}
