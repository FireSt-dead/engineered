#pragma once

#include "stdafx.h"

using namespace Drawing;

namespace DirectDrawing {

	public ref class Brush : public IBrush
	{
	internal:
		ID2D1Brush* brush;
		Brush(ID2D1Brush* b);
	};

	// TODO: LinearGradientBrush
	// TODO: RadialGradientBrush
}

