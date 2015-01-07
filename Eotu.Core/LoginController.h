#pragma once

namespace EotuCore {

	public ref class LoginController
	{
	public:
		LoginController(void);

		inline System::Boolean login(System::String^ username, System::String^ password) {
			return login(username, password, 3000);
		}

		System::Boolean login(System::String^ username, System::String^ password, int timeout);
	};

}

