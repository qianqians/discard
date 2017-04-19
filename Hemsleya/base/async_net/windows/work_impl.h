/*
 * work_impl.h
 *   Created on: 2013-10-2
 *       Author: qianqians
 * work_impl
 */
#ifndef _WORK_IMPL_H
#define _WORK_IMPL_H

#ifdef _WINDOWS

#include "winhdef.h"

namespace Hemsleya { 
namespace async_net { 

class work_impl{
public:
	work_impl();
	~work_impl();

	void run();

	void addsocket(SOCKET s);

private:
	HANDLE hiocp;

};  

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_WORK_IMPL_H
