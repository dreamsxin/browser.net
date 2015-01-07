#pragma once

using namespace System;
using namespace System::Text;

namespace EotuCore {

	ref class Logger
	{
	public:
		static void Debug(String^ message);
		static void Debug(std::string message);
	};

}
