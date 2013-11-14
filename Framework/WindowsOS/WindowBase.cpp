#include "stdafx.h"

#include "WindowBase.h"

using namespace System;
using namespace System::Collections::Generic;

namespace WindowsOS {

	LRESULT CALLBACK GlobalWndProcedure(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
	{
		unsigned long longHwnd = (unsigned long)hWnd;
		WindowBase^ window;
		if (WindowBase::HWndWindows->TryGetValue(longHwnd, window))
		{
			LRESULT result = window->WndProcedure(msg, wParam, lParam);
			if (result)
			{
				return result;
			}
		}

		return DefWindowProc(hWnd, msg, wParam, lParam);
	}

	WindowBase::WindowBase()
	{
		LPCTSTR ClsName = L"BasicApp";
		LPCTSTR WndName = L"A Simple Window";

		if (!classAtom)
		{
			WNDCLASSEX wcx;

			//wcx.hInstance     = hThisInstance;
			wcx.lpszClassName = ClsName;
			wcx.lpfnWndProc   = (WNDPROC)GlobalWndProcedure; //(WNDPROC)GlobalWndProcedure;
			wcx.style         = CS_DBLCLKS;
			wcx.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
			wcx.hIconSm       = LoadIcon(NULL, IDI_APPLICATION);
			wcx.hCursor       = LoadCursor(NULL, IDC_ARROW);
			wcx.lpszMenuName  = NULL;
			wcx.cbSize        = sizeof (WNDCLASSEX);
			wcx.cbClsExtra    = 0;
			wcx.cbWndExtra    = 0;
			wcx.hbrBackground = GetSysColorBrush(COLOR_3DFACE);

			classAtom = RegisterClassEx(&wcx);

			if (!classAtom)
			{
				throw gcnew Exception("Register window class failed.");
			}
		}

		isTrackingMouse = false;

		hWnd = CreateWindowEx(0,
				ClsName,
				WndName,
				WS_OVERLAPPEDWINDOW | WS_VISIBLE,
				100, 100, 500, 500, // Size
				NULL, //(HWND)group,
				NULL,
				NULL, //hInstance,
				NULL);

		if (hWnd)
		{
			long longHwnd = (long)hWnd;
			WindowBase::HWndWindows->Add(longHwnd, this);
		}
		else
		{
			throw gcnew Exception("Create window failed.");
		}
	}

	void WindowBase::Show()
	{
		ShowWindow(hWnd, SW_SHOWNORMAL);
		UpdateWindow(hWnd);
	}

	void WindowBase::Close()
	{
		CloseWindow(hWnd);
	}

	int WindowBase::WndProcedure(int Msg, WPARAM wParam, LPARAM lParam)
	{
		int xPos = GET_X_LPARAM(lParam);
		int yPos = GET_Y_LPARAM(lParam);
		POINT mousePoint;
		mousePoint.x = xPos;
		mousePoint.y = yPos;

		HDC hdc;
		PAINTSTRUCT ps;
		int p;

		RECT rc;
		GetClientRect(hWnd, &rc);

		switch(Msg)
		{
			case WM_PAINT:
				width = rc.right - rc.left;
				height = rc.bottom - rc.top;
				hdc = BeginPaint(hWnd, &ps);
				p = OnPaint();
				EndPaint(hWnd, &ps);

				InvalidateRect(hWnd, NULL, false);

				return 1;

			case WM_SIZE:
				width = rc.right - rc.left;
				height = rc.bottom - rc.top;
				return OnSize();

			case WM_CHAR:
				return OnChar((System::Char)wParam);
			case WM_KEYDOWN:
				return OnKeyDown((unsigned int)wParam);
			case WM_KEYUP:
				return OnKeyUp((unsigned int)wParam);

			case WM_ERASEBKGND:
				return 1;

			case WM_MOUSEMOVE:
				if (!isTrackingMouse)
				{
					TRACKMOUSEEVENT tme;
					tme.cbSize = sizeof(TRACKMOUSEEVENT);
					tme.dwFlags = TME_LEAVE;
					tme.hwndTrack = hWnd;
					if (TrackMouseEvent(&tme))
					{
						isTrackingMouse = true;
					}
				}

				return OnMouseMove(xPos, yPos);
			case WM_SETCURSOR:
				return OnSetCursor();

			case WM_LBUTTONDOWN:
				SetCapture(hWnd);
				return OnLeftButtonDown(xPos, yPos);
			case WM_LBUTTONUP:
				ReleaseCapture();
				return OnLeftButtonUp(xPos, yPos);
			case WM_LBUTTONDBLCLK:
				return OnLeftButtonDoubleClick(xPos, yPos);

			case WM_RBUTTONDOWN:
				SetCapture(hWnd);
				return OnRightButtonDown(xPos, yPos);
			case WM_RBUTTONUP:
				ReleaseCapture();
				return OnRightButtonUp(xPos, yPos);
			case WM_RBUTTONDBLCLK:
				return OnRightButtonDoubleClick(xPos, yPos);

			case WM_MBUTTONDOWN:
				SetCapture(hWnd);
				return OnMiddleButtonDown(xPos, yPos);
			case WM_MBUTTONUP:
				ReleaseCapture();
				return OnMiddleButtonUp(xPos, yPos);
			case WM_MBUTTONDBLCLK:
				return OnMiddleButtonDoubleClick(xPos, yPos);

			case WM_CAPTURECHANGED:
				OnCaptureChanged();
				return 0;

			case WM_MOUSELEAVE:
				isTrackingMouse = false;
				OnMouseLeave();
				return 0;

			case WM_DESTROY:
				// Application window closes the application
				PostQuitMessage(WM_QUIT);
				return 1;

			case WM_CREATE:
				width = rc.right - rc.left;
				height = rc.bottom - rc.top;
				return OnCreate();
		}

		return 0;
	}

	unsigned int WindowBase::Width::get()
	{
		return width;
	}
	unsigned int WindowBase::Height::get()
	{
		return height;
	}

	int WindowBase::OnCreate()
	{
		return 1;
	}

	int WindowBase::OnPaint()
	{
		return 1;
	}

	int WindowBase::OnSize()
	{
		return 0;
	}

	int WindowBase::OnChar(System::Char c)
	{
		return 0;
	}

	int WindowBase::OnKeyDown(unsigned int keyCode)
	{
		return 0;
	}

	int WindowBase::OnKeyUp(unsigned int keyCode)
	{
		return 0;
	}

	int WindowBase::OnMouseMove(int xPos, int yPos)
	{
		return 0;
	}

	int WindowBase::OnSetCursor()
	{
		return 0;
	}

	int WindowBase::OnLeftButtonDown(int xPos, int yPos)
	{
		return 0;
	}
	int WindowBase::OnLeftButtonUp(int xPos, int yPos)
	{
		return 0;
	}
	int WindowBase::OnLeftButtonDoubleClick(int xPos, int yPos)
	{
		return 0;
	}

	int WindowBase::OnRightButtonDown(int xPos, int yPos)
	{
		return 0;
	}
	int WindowBase::OnRightButtonUp(int xPos, int yPos)
	{
		return 0;
	}
	int WindowBase::OnRightButtonDoubleClick(int xPos, int yPos)
	{
		return 0;
	}

	int WindowBase::OnMiddleButtonDown(int xPos, int yPos)
	{
		return 0;
	}
	int WindowBase::OnMiddleButtonUp(int xPos, int yPos)
	{
		return 0;
	}
	int WindowBase::OnMiddleButtonDoubleClick(int xPos, int yPos)
	{
		return 0;
	}

	void WindowBase::OnCaptureChanged()
	{
	}
	void WindowBase::OnMouseLeave()
	{
	}
}