/*
 * tools.c
 * Created on: 2012-10-25
 *     Author: qianqians
 * tools
 */
#include <stdarg.h>
#include <stdio.h>
//#include <memory.h>

#include "tools.h"

void _trace(char * format, ...){
#ifdef _DEBUG
	char buff[2048];
	//memset(buff, 0, 2048);

	va_list argptr;
	va_start(argptr, format);	
	vsnprintf_s(buff, 2048, _TRUNCATE, format, argptr);
	va_end(argptr);

	printf(buff);
#endif //_DEBUG
}

#ifdef _WINDOWS
#include <Windows.h>

uint32_t count = 0;

uint32_t processors_count(){
	if (count == 0){
		SYSTEM_INFO info;
		GetSystemInfo(&info);
		count = info.dwNumberOfProcessors;
	}

	return count;
}
#endif //_WINDOWS