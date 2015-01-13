#pragma once

#include "json/json.h"

using namespace System;

namespace EotuCore {

	public ref class LoginController
	{
	private:
		Boolean _status;
		String^ _message;
	public:
		property String^ Username;
		property String^ Password;
		property String^ Token;
	public:
		LoginController(void);

		inline Boolean login(String^ username, String^ password) {
			return login(username, password, 3000);
		}

		Boolean login(String^ username, String^ password, int timeout);

		Boolean getStatus();
		String^ getMessage();
	};

}

