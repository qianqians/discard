/*
 * clock.cpp
 *  Created on: 2013-5-12
 *      Author: qianqians
 * clock
 */
#include "clock.h"

#include <time.h>

namespace Hemsleya{
namespace timer{

#ifdef _WINDOWS

#include <Windows.h>

namespace detail{

struct clock_init{
	uint32_t round;
	uint64_t begintime;
	uint64_t beginclock;
	bool bGetTickCount;

	clock_init(){
		OSVERSIONINFOA VersionInformation;
		if (GetVersionEx(&VersionInformation)){
			if (VersionInformation.dwMajorVersion >= 0x6000){
				bGetTickCount = true;
			}
		}else{
			bGetTickCount = false;
		}

		round = 0;
		if (bGetTickCount){
			beginclock = GetTickCount();
		}else{
			beginclock = GetTickCount64();
		}

		time_t _time;
		begintime = (uint64_t)time(&_time);
	}
};

struct clock_init _clock_init;

}

uint64_t clock(){
	if (detail::_clock_init.bGetTickCount){
		time_t _time;
		return ((uint64_t)time(&_time) - detail::_clock_init.begintime)*1000 + GetTickCount()%1000;
	}else{
		return GetTickCount64() - detail::_clock_init.beginclock;
	}

	return 0;
}

#endif //_WINDOWS

}// timer
}// Hemsleya