#pragma once

using namespace System;

namespace EotuCore {
	public ref class ContactModel
	{
	private:
		int contact_user_id;
		String^ avatar;
		String^ phones;
		String^ telphones;
		String^ sex;
		String^ birthdate;
		String^ company;
		String^ dep;
		String^ job;
		String^ address;
		String^ intimacy;
		String^ extdata;
		String^ lasttime;
		String^ updated;
		String^ created;
	public:
		property int UserId;
		property int GroupId;;

		property String^ Email;
		property String^ Phone;
		property String^ FullName;
		property String^ NickName;
	public:

		ContactModel(void)
		{
			UserId = 0;
			FullName = String::Empty;
		}
	};
}
