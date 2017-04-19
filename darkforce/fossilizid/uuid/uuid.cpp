/*
 * uuid.cpp
 *
 *  Created on: 2014-10-15
 *      Author: qianqians
 */
#include "uuid.h"

#ifdef _WINDOWS

#include <windows.h> 
#include <iphlpapi.h>
#include <time.h>

#pragma comment(lib, "IPHLPAPI.lib")

#endif

namespace Fossilizid{
namespace uuid {

uuid UUID(){
	std::string _uuid;
	_uuid.resize(16);

	{
		int t = (int)time(0);

		_uuid[0] = t & 0xff;
		_uuid[1] = (t & 0xff00) >> 8;
		_uuid[2] = (t & 0xff0000) >> 16;
		_uuid[3] = (t & 0xff000000) >> 24;
	}

	{
#ifdef _WINDOWS
		static IP_ADAPTER_INFO * info = new IP_ADAPTER_INFO();
		ULONG size = sizeof(IP_ADAPTER_INFO);
		GetAdaptersInfo(info, &size);
		static bool isinit = false;
		if (!isinit){
			delete info;
			info = (IP_ADAPTER_INFO *)new char[size];
			if (GetAdaptersInfo(info, &size) == ERROR_SUCCESS){
				isinit = true;
			}
		}

		_uuid[4] = info->Address[0];
		_uuid[5] = info->Address[1];
		_uuid[6] = info->Address[2];
		_uuid[7] = info->Address[3];
		_uuid[8] = info->Address[4];
		_uuid[9] = info->Address[5];

		static DWORD id = GetCurrentProcessId();

		_uuid[10] = (char)(id & 0xff);
		_uuid[11] = (char)((id & 0xff00) >> 8);
#endif
	}

	{
		clock_t c = clock();
		
		_uuid[12] = (char)(c & 0xff);
		_uuid[13] = (char)((c & 0xff00) >> 8);
	}

	{
		_uuid[14] = (char)rand() + 1;
	}

	{
		static unsigned char key = 1;
		_uuid[15] = key++;
		key = (key == 0) ? 1 : key;
	}

	return _uuid;
}

} /* namespace juggle */
} /* namespace Fossilizid */