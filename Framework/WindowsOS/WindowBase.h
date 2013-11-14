#pragma once

#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

namespace WindowsOS {

	public ref class WindowBase
	{
	private:
		HWND hWnd;
		ATOM classAtom;

		[ThreadStatic]
		static Dictionary<unsigned long, WindowBase^>^ hWndWindows;

		int width;
		int height;

		bool isTrackingMouse;

	internal:
		static property Dictionary<unsigned long, WindowBase^>^ HWndWindows
		{
			Dictionary<unsigned long, WindowBase^>^ get()
			{
				if (hWndWindows == nullptr)
				{
					hWndWindows = gcnew Dictionary<unsigned long, WindowBase^>(); 
				}

				return hWndWindows;
			}
		}

		virtual int WndProcedure(int Msg, WPARAM wParam, LPARAM lParam);

	protected:

		property unsigned int Width { 
			unsigned int get();
		}

		property unsigned int Height { 
			unsigned int get();
		}

		property IntPtr^ Hwnd
		{
			IntPtr^ get()
			{
				return gcnew IntPtr(hWnd);
			}
		}

		virtual int OnCreate();

		virtual int OnPaint();
		virtual int OnSize();

		virtual int OnChar(System::Char c);
		virtual int OnKeyDown(unsigned int keyCode);
		virtual int OnKeyUp(unsigned int keyCode);

		virtual int OnMouseMove(int x, int y);
		virtual int OnSetCursor();

		virtual int OnLeftButtonDown(int x, int y);
		virtual int OnLeftButtonUp(int x, int y);
		virtual int OnLeftButtonDoubleClick(int x, int y);

		virtual int OnRightButtonDown(int x, int y);
		virtual int OnRightButtonUp(int x, int y);
		virtual int OnRightButtonDoubleClick(int x, int y);

		virtual int OnMiddleButtonDown(int x, int y);
		virtual int OnMiddleButtonUp(int x, int y);
		virtual int OnMiddleButtonDoubleClick(int x, int y);

		virtual void OnCaptureChanged();
		virtual void OnMouseLeave();

		void SetSystemCursor(int id)
		{
			// TODO: range check for the system cursors
			SetCursor(LoadCursor(NULL, MAKEINTRESOURCE(id)));
		}

	public:
		WindowBase();
		void Show();
		void Close();
	};
}
