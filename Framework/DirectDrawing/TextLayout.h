#pragma once

#include "stdafx.h"

using namespace Geometry;
using namespace Drawing;
using namespace System::Collections::Generic;

namespace DirectDrawing
{
	public ref class TextLayout : public ITextLayout
	{
	internal:
		IDWriteTextLayout* textLayout;
		TextLayout(IDWriteTextLayout* tl);

	public:
		virtual property Rect BoundingBox
		{
			Rect get() sealed;
		}

		virtual CharacterLayout HitTestCharacter(unsigned int textPosition, CharacterSide side);
		virtual CharacterHit HitTestPoint(Point position);
		virtual IEnumerable<Rect>^ HitTestRange(unsigned int index, unsigned int length);
		virtual void SetForeground(IBrush^ foreground, unsigned int index, unsigned int length);
	};
}
