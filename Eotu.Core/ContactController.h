#pragma once

#include "GroupModel.h"

using namespace System;
using namespace System::Collections::Generic;

namespace EotuCore {

	public ref class ContactController
	{
	public:
		ContactController(void);


		inline List<GroupModel^>^ getGroups(int uid) {
			return getGroups(uid, 3000);
		}

		List<GroupModel^>^ getGroups(int uid, int timeout);
	};
}

