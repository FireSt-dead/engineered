#pragma once

#include "stdafx.h"
#include "HwndRenderTarget.h"

using namespace System;
using namespace System::Collections::Generic;

namespace DirectDrawing {

	public ref class Factory
	{
	private:
		[ThreadStatic]
		static Factory^ currentFactory;

		ID2D1Factory* factory;

		Factory();

	public:

		static property Factory^ Current
		{
			Factory^ get()
			{
				if (currentFactory == nullptr)
				{
					currentFactory = gcnew Factory();
				}

				return currentFactory;
			}
		}

		HwndRenderTarget^ CreateHwndRenderTarget(IntPtr^ hWnd);
	};
}

