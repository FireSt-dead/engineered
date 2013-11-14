#include "StdAfx.h"
#include "WriteFactory.h"

namespace DirectDrawing {

	WriteFactory::WriteFactory()
	{
		HRESULT hr = S_OK;
		IDWriteFactory* f;

		hr = DWriteCreateFactory(
			DWRITE_FACTORY_TYPE_ISOLATED,
			__uuidof(IDWriteFactory),
			reinterpret_cast<IUnknown**>(&f)
			);

		writeFactory = f;
	}
}
