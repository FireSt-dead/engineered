#include "stdafx.h"
#include "TextFormat.h"
#include "TextLayout.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Geometry;

namespace DirectDrawing {
	
	public ref class WriteFactory
	{
	private:
		[ThreadStatic]
		static WriteFactory^ currentWriteFactory;

		IDWriteFactory* writeFactory;

		WriteFactory();

	public:

		static property WriteFactory^ Current
		{
			WriteFactory^ get()
			{
				if (currentWriteFactory == nullptr)
				{
					currentWriteFactory = gcnew WriteFactory();
				}

				return currentWriteFactory;
			}
		}

		// FontCollection^ CreateFontCollection

		TextFormat^ CreateTextFormat(String^ fontFamilyName, float fontSize)
		{
			HRESULT hr = S_OK;
			IDWriteTextFormat* wtf;

			pin_ptr<const wchar_t> fontFamilyNameW = PtrToStringChars(fontFamilyName);

			hr = writeFactory->CreateTextFormat(
				fontFamilyNameW,
				NULL, //IDWriteFontCollection* fontCollection,
				DWRITE_FONT_WEIGHT_NORMAL,
				DWRITE_FONT_STYLE_NORMAL,
				DWRITE_FONT_STRETCH_NORMAL,
				fontSize,
				L"", //locale
				&wtf);

			TextFormat^ tf = gcnew TextFormat(wtf);

			return tf;
		}

		TextLayout^ CreateTextLayout(String^ text, TextFormat^ textFormat, Size size)
		{
			HRESULT hr = S_OK;
			IDWriteTextLayout* wtl;

			pin_ptr<const wchar_t> textW = PtrToStringChars(text);
			unsigned int stringLength = text->Length;

			IDWriteTextFormat* directWriteTextFormat = textFormat->textFormat;

			hr = writeFactory->CreateTextLayout(
				textW,
				stringLength,
				directWriteTextFormat,
				size.Width,
				size.Height,
				&wtl);

			TextLayout^ tl = gcnew TextLayout(wtl);

			return tl;
		}
	};
}