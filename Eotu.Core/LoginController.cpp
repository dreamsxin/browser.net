#include "StdAfx.h"
#include "Logger.h"
#include "LoginController.h"

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
		Logger::Debug("登录处理");
		if (System::String::IsNullOrEmpty(username) || System::String::IsNullOrEmpty(password)) {
			return FALSE;
		} else {
			std::string s_username = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(username)).ToPointer();
			std::string s_password = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(password)).ToPointer();
			
			Json::Value json;

			json["username"] = s_username;
			json["password"] = s_password;

			Logger::Debug(json.toStyledString());

			Json::FastWriter writer;  
			std::string data = writer.write(json); 

			std::cout << "开始请求服务器" << std::endl;

			TCPInterface *tcp;
			HTTPConnection *httpConnection;

			tcp = RakNet::OP_NEW<TCPInterface>(__FILE__, __LINE__);
			httpConnection = RakNet::OP_NEW<HTTPConnection>(__FILE__, __LINE__);

			tcp->Start(0, 2);

			httpConnection->Init(tcp, "passport.eotu.com", 81);
			httpConnection->Post("/api/local/account/auth", data.c_str());

			while (timeout) {
				Packet *packet = tcp->Receive();
				if(packet) {
					httpConnection->ProcessTCPPacket(packet);
					tcp->DeallocatePacket(packet);
				}

				httpConnection->Update();

				if (httpConnection->IsBusy()==false) {
					RakString fileContents = httpConnection->Read();
					std::cout << "接收到数据：" << fileContents << std::endl;

					getche();
					return TRUE;
				}

				RakSleep(30);
				timeout = timeout - 30;
			}

			RakNet::OP_DELETE(httpConnection,_FILE_AND_LINE_);
			RakNet::OP_DELETE(tcp,_FILE_AND_LINE_);
			return FALSE;
		}
	}

}
