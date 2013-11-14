#pragma once

#include "stdafx.h"

using namespace System;

namespace WindowsOS {

	public ref class Dispatcher
	{
	private:
		Dispatcher();

		//[ThreadStatic]
		static Dispatcher^ dispatcher;

	public:
		static property Dispatcher^ Current
		{
			Dispatcher^ get()
			{
				if (dispatcher == nullptr)
				{
					dispatcher = gcnew Dispatcher();
				}

				return dispatcher;
			}
		}

		void Run();
	};
}
