/*
 * winhdef.h
 *   Created on: 2012-10-16
 *       Author: qianqians
 * winhdef
 */
#ifndef _WINHDEF_H
#define _WINHDEF_H

#ifdef _WINDOWS
#include <WinSock2.h>
#include <ws2ipdef.h>
#include <Mstcpip.h>
#include <mswsock.h>
#include <Windows.h>

#pragma comment(lib, "ws2_32.lib")

#endif //_WINDOWS

#endif //_WINHDEF_H
