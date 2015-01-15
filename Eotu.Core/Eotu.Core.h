// Eotu.Core.h

#pragma once

using namespace System;
using namespace System::Text;	
using namespace Runtime::InteropServices;

namespace EotuCore {

	public ref class Config
	{
	public:
		property static String^ command;
		property static String^ domain;
		property static String^ host;
		property static int port;
		property static String^ username;
		property static String^ password;
		property static String^ token;

	};

	public ref class Util
	{
	public:
		static void MarshalString(String ^ s, std::string& os) {
			const char* chars = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
			os = chars;
			Marshal::FreeHGlobal(IntPtr((void*)chars));
		}

		static String^ DecodeFromUtf8(const std::string& str)
		{
			int len = str.length();
			array<Byte>^ bytes = gcnew array<Byte>(len);
			for (int i=0;i < len;++i) {
				bytes[i] = (Byte)str[i];
			}

			return Encoding::UTF8->GetString(bytes, 0, bytes->Length);
		}

		static std::string EncodeToUtf8(const std::string& str)
		{
			return Util::EncodeToUtf8(gcnew System::String(str.c_str()));
		}

		static std::string EncodeToUtf8(String^ str)
		{
			array<Byte>^ bytes = Encoding::UTF8->GetBytes(str);

			std::string utf8str;
			utf8str.resize(bytes->Length);
			Marshal::Copy(bytes, 0, IntPtr((void*)utf8str.data()), bytes->Length);

			return utf8str;
		}
	};
}
