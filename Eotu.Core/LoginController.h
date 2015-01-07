#pragma once

#include "json/json.h"

namespace EotuCore {

	public ref class LoginController
	{
	private:
		System::Boolean _status;
		System::String^ _message;
	public:
		LoginController(void);

		inline System::Boolean login(System::String^ username, System::String^ password) {
			return login(username, password, 3000);
		}

		System::Boolean login(System::String^ username, System::String^ password, int timeout);

		System::Boolean getStatus();
		System::String^ getMessage();
	};

}

