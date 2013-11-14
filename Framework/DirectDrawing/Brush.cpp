#include "StdAfx.h"
#include "Brush.h"

using namespace Drawing;

namespace DirectDrawing {
	Brush::Brush(ID2D1Brush* b)
	{
		brush = b;
	}
}
