#pragma once

#include "stdafx.h"

using namespace std;

namespace EotuCore {

	class HttpClient
	{
	public:
		const char *host;
		int port;

	private:
		TCPInterface *tcp;
		HTTPConnection *httpConnection;

	public:

		HttpClient(const char *in_host, const int in_port = 80)
			: host(in_host), port(in_port)
		{
			tcp = RakNet::OP_NEW<TCPInterface>(_FILE_AND_LINE_);
			httpConnection = RakNet::OP_NEW<HTTPConnection>(_FILE_AND_LINE_);

			tcp->Start(0, 32);
			httpConnection->Init(tcp, host, port);
		}

		~HttpClient(void) {
			RakNet::OP_DELETE(httpConnection, _FILE_AND_LINE_);
			RakNet::OP_DELETE(tcp, _FILE_AND_LINE_);
		}

		std::string Get(const string& url, const int timeout)
		{
			Logger::Debug("url:" + url);

			int runTime = 0;

			httpConnection->Get(url.c_str());

			while (runTime < timeout) {
				Packet *packet = tcp->Receive();
				if(packet) {
					httpConnection->ProcessTCPPacket(packet);
					tcp->DeallocatePacket(packet);
				}

				httpConnection->Update();

				if (httpConnection->IsBusy()==false) {
					RakString fileContents = httpConnection->Read();
					string result = strstr(fileContents.C_String(), "\r\n\r\n");
					getche();
					Logger::Debug("recv:" + result);
					return result;
				}

				RakSleep(30);
				runTime += 30;
			}

			return NULL;
		}

		std::string Post(const string& url, const string& data, const int timeout)
		{
			Logger::Debug("url:" + url + " data:" + data);

			int runTime = 0;

			httpConnection->Post(url.c_str(), data.c_str());

			while (runTime < timeout) {
				Packet *packet = tcp->Receive();
				if(packet) {
					httpConnection->ProcessTCPPacket(packet);
					tcp->DeallocatePacket(packet);
				}

				httpConnection->Update();

				if (httpConnection->IsBusy()==false) {
					RakString fileContents = httpConnection->Read();
					string result = strstr(fileContents.C_String(), "\r\n\r\n");
					getche();
					Logger::Debug("recv:" + result);
					return result;
				}

				RakSleep(30);
				runTime += 30;
			}

			return NULL;
		}
	};
}

