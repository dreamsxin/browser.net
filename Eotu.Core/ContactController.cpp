#include "StdAfx.h"
#include "ContactController.h"
#include "HttpClient.h"

using namespace System;
using namespace System::IO;
using namespace System::Net;

namespace EotuCore {

	ContactController::ContactController(void)
	{
	}

	List<GroupModel^>^ ContactController::getGroups(int uid, int timeout)
	{
		List<GroupModel^>^ groups = gcnew List<GroupModel^>();

		HttpClient httpClient("passport.eotu.com", 81);
		std::string uri = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi("/api/local/contact/group/" + uid)).ToPointer();

		std::string ret = httpClient.Get(uri, timeout);

		Json::Reader reader;
		Json::Value result;

		if (reader.parse(ret, result)) {
			if (result.isMember("status")) {
				std::string status = result["status"].asString();
				if (status.compare("ok") == 0)
				{
					if (result.isMember("data")) {
						Json::Value data = result["data"];
						int size = data.size();
						for (int i = 0; i < size; i++) {
							Json::Value item = data[i];
							GroupModel^ group = gcnew GroupModel();
							group->UserId = item["user_id"].asInt();
							group->GroupName = gcnew String(item["name"].asString().c_str());
							groups->Add(group);
						}  
					}
				}
			}
		}

		return groups;
	}

	List<ContactModel^>^ ContactController::getContacts(int uid, int group_id, int timeout)
	{
		List<ContactModel^>^ contacts = gcnew List<ContactModel^>();

		HttpClient httpClient("passport.eotu.com", 81);
		std::string uri = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi("/api/local/contact/list/" + uid)).ToPointer();

		std::string ret = httpClient.Get(uri, timeout);

		Json::Reader reader;
		Json::Value result;

		if (reader.parse(ret, result)) {
			if (result.isMember("status")) {
				std::string status = result["status"].asString();
				if (status.compare("ok") == 0)
				{
					if (result.isMember("data")) {
						Json::Value data = result["data"];
						int size = data.size();
						for (int i = 0; i < size; i++) {
							Json::Value item = data[i];
							ContactModel^ contact = gcnew ContactModel();
							contact->UserId = item["user_id"].asInt();
							contact->FullName = gcnew String(item["fullname"].asString().c_str());
							contacts->Add(contact);
						}  
					}
				}
			}
		}

		return contacts;
	}
}
