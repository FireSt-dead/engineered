#pragma once

#include "stdafx.h"
#include "Brush.h"

using namespace Drawing;

namespace DirectDrawing {

	public ref class SolidColorBrush : public Brush, ISolidColorBrush
	{
	internal:
		SolidColorBrush(ID2D1SolidColorBrush* scb) : Brush(scb)
		{
		}
	};
}

