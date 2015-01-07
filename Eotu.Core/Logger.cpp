#include "StdAfx.h"
#include "Logger.h"

namespace EotuCore {

	void Logger::Debug(String^ message)
	{
		Console::WriteLine(message);
	}

	void Logger::Debug(std::string message)
	{
		std::cout << message << std::endl;
	}

}