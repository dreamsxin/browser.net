#pragma once

namespace EotuCore {

	public ref class LoginController
	{
	public:
		LoginController(void);

		System::Boolean login(System::String^ username, System::String^ password);
	};

}

