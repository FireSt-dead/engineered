#pragma once

#include "stdafx.h"

using namespace Drawing;

namespace DirectDrawing
{
	public ref class TextFormat : public ITextFormat
	{
	internal:
		IDWriteTextFormat* textFormat;
		TextFormat(IDWriteTextFormat* tf);
	};
}

