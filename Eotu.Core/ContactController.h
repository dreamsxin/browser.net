#pragma once

#include "GroupModel.h"
#include "ContactModel.h"

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

		inline List<ContactModel^>^ getContacts(int uid, int group_id) {
			return getContacts(uid, 0, 3000);
		}
		List<ContactModel^>^ getContacts(int uid, int group_id, int timeout);
	};
}

