#include "StdAfx.h"
#include "LoginController.h"

namespace EotuCore {

	LoginController::LoginController(void)
	{
	}

	System::Boolean LoginController::login(System::String^ username, System::String^ password)
	{
		std::string s_username = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(username)).ToPointer();
		std::string s_password = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(password)).ToPointer();

		if (s_username.empty() || s_password.empty()) {
			return FALSE;
		} else if (s_username.compare("admin") == 0) {
			return TRUE;
		} else {
			return FALSE;
		}
	}

}
