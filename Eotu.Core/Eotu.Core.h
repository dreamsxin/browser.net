// Eotu.Core.h

#pragma once

using namespace System;

namespace EotuCore {

	public ref class Util
	{
	public:
		static void MarshalString(String ^ s, std::string& os) {
			using namespace Runtime::InteropServices;
			const char* chars = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
			os = chars;
			Marshal::FreeHGlobal(IntPtr((void*)chars));
		}
	};
}
