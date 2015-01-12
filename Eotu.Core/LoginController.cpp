#include "StdAfx.h"
#include "Logger.h"
#include "LoginController.h"
#include "HttpClient.h"

using namespace System;
using namespace System::IO;
using namespace System::Text;
using namespace System::Net;

namespace EotuCore {

	LoginController::LoginController(void)
	{
	}

	System::Boolean LoginController::login(System::String^ username, System::String^ password, int timeout)
	{
		_status = FALSE;
		_message = System::String::Empty;

		if (System::String::IsNullOrEmpty(username) || System::String::IsNullOrEmpty(password)) {
			return FALSE;
		} else {
			std::string s_username = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(username)).ToPointer();
			std::string s_password = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(password)).ToPointer();
			
			Json::Value data;

			data["username"] = s_username;
			data["password"] = s_password;

			std::string host;
			Util::MarshalString(Config::host, host);
			HttpClient httpClient(host.c_str(), Config::port);
			std::string ret = httpClient.Post("/api/local/account/auth", data.toString(), timeout);

			Json::Reader reader;
			Json::Value result;

			if (reader.parse(ret, result)) {
				if (result.isMember("status")) {
					std::string status = result["status"].asString();
					_status = (status.compare("ok") == 0);
				}
				if (result.isMember("message")) {
					std::string message = result["message"].asString();
					_message = gcnew String(message.c_str());
				}
			}
		}

		return _status;
	}

	System::Boolean LoginController::getStatus()
	{
		return _status;
	}

	System::String^ LoginController::getMessage()
	{
		return _message;
	}

}
