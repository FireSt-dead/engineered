#include "StdAfx.h"
#include "TextLayout.h"
#include "Brush.h"

using namespace Drawing;
using namespace System::Collections::Generic;

namespace DirectDrawing
{
	TextLayout::TextLayout(IDWriteTextLayout* tl)
	{
		textLayout = tl;
	}

	Rect TextLayout::BoundingBox::get()
	{
		HRESULT hr = S_OK;
		DWRITE_TEXT_METRICS metrix;
		textLayout->GetMetrics(&metrix);
		Rect boundingBox = Rect(metrix.left, metrix.top, metrix.left + metrix.widthIncludingTrailingWhitespace, metrix.top + metrix.height);
		return boundingBox;
	}

	CharacterLayout TextLayout::HitTestCharacter(unsigned int textPosition, CharacterSide side)
	{
		float pointX;
		float pointY;
		DWRITE_HIT_TEST_METRICS hitTestMetrics;
		HRESULT hr = S_OK;
		bool isTrailingHit = side == CharacterSide::Trailing;
		hr = textLayout->HitTestTextPosition(textPosition, isTrailingHit, &pointX, &pointY, &hitTestMetrics);

		CharacterLayout characterLayout;
		characterLayout.Rectangle = Rect(hitTestMetrics.left, hitTestMetrics.top, hitTestMetrics.left + hitTestMetrics.width, hitTestMetrics.top + hitTestMetrics.height);
		characterLayout.Position = Point(pointX, pointY);

		return characterLayout;
	}

	CharacterHit TextLayout::HitTestPoint(Point position)
	{
		BOOL isTrailingHit;
		BOOL isInside;
		DWRITE_HIT_TEST_METRICS hitTestMetrics;
		HRESULT hr = S_OK;
		hr = textLayout->HitTestPoint(position.X, position.Y, &isTrailingHit, &isInside, &hitTestMetrics);

		CharacterHit characterHit;
		characterHit.Position = hitTestMetrics.textPosition;
		characterHit.Side = isTrailingHit ? CharacterSide::Trailing : CharacterSide::Leading;
		characterHit.IsInside = isInside != 0;

		return characterHit;
	}

	IEnumerable<Rect>^ TextLayout::HitTestRange(unsigned int index, unsigned int length)
	{
		unsigned int actualHitTestMetrixCount;
		DWRITE_HIT_TEST_METRICS* hitTestMetrics = new DWRITE_HIT_TEST_METRICS[16];
		HRESULT hr = S_OK;
		hr = textLayout->HitTestTextRange(index, length, 0, 0, hitTestMetrics, 16, &actualHitTestMetrixCount);
		List<Rect>^ result = gcnew List<Rect>();
		if (hr == E_NOT_SUFFICIENT_BUFFER)
		{
		}

		for (unsigned int i = 0; i < actualHitTestMetrixCount; i++)
		{
			DWRITE_HIT_TEST_METRICS& htm = hitTestMetrics[i];
			result->Add(Rect(htm.left, htm.top, htm.left + htm.width, htm.top + htm.height));
		}

		delete[] hitTestMetrics;

		return result;
	}

	void TextLayout::SetForeground(IBrush^ foreground, unsigned int index, unsigned int length)
	{
		Brush^ brush = safe_cast<Brush^>(foreground);
		DWRITE_TEXT_RANGE textRange;
		textRange.startPosition = index;
		textRange.length = length;
		textLayout->SetDrawingEffect(brush->brush, textRange);
	}
}