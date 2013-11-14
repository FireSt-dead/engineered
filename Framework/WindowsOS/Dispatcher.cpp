#include "StdAfx.h"
#include "Dispatcher.h"

namespace WindowsOS {

	Dispatcher::Dispatcher()
	{
	}

	void Dispatcher::Run()
	{
		MSG Msg;
		while(GetMessage(&Msg, NULL, 0, 0))
		{
			TranslateMessage(&Msg);
			DispatchMessage(&Msg);
		}
	}
}