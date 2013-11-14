#include "StdAfx.h"
#include "TextFormat.h"

using namespace Drawing;

namespace DirectDrawing
{
	TextFormat::TextFormat(IDWriteTextFormat* tf)
	{
		textFormat = tf;
	}
}
