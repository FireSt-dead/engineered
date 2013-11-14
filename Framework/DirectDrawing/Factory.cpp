#include "StdAfx.h"
#include "Factory.h"

namespace DirectDrawing {

	Factory::Factory()
	{
		ID2D1Factory* f;
		HRESULT hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &f);

		if (!hr)
		{
			// throw
		}

		factory = f;
	}

	HwndRenderTarget^ Factory::CreateHwndRenderTarget(IntPtr^ hWndPtr)
	{
		RECT rc;
		HWND hWnd = (HWND)(hWndPtr->ToPointer());
		GetClientRect(hWnd, &rc);

		D2D1_SIZE_U size = D2D1::SizeU(rc.right - rc.left, rc.bottom - rc.top);

		ID2D1HwndRenderTarget* hwndRenderTarget;
		HRESULT hr = S_OK;

		hr = factory->CreateHwndRenderTarget(
			D2D1::RenderTargetProperties(),
			D2D1::HwndRenderTargetProperties(hWnd, size),
			&hwndRenderTarget
		);

		if (!hr)
		{
			// throw
		}

		HwndRenderTarget^ renderTarget = gcnew HwndRenderTarget(hwndRenderTarget);

		return renderTarget;
	}
}
