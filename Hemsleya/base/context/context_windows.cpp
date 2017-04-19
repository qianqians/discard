/*
 * context.cpp
 *
 *  Created on: 2013-9-14
 *      Author: qianqians
 * Hemsleya context
 */
#ifdef _WINDOWS

#include <Windows.h>
#include "context.h"

namespace Hemsleya{
namespace context{

namespace detail{

void WINAPI _PFIBER_START_ROUTINE(LPVOID lpFiberParameter){
	if(lpFiberParameter){
		((context*)lpFiberParameter)->fn_handle();
	}
}

}//detail

context::context(boost::function<void()> _fn_handle){
	_contex = CreateFiber(65536, detail::_PFIBER_START_ROUTINE, this);
}

context::~context(){
	DeleteFiber(_contex);
}

void context::operator()(){
	SwitchToFiber(_contex);
}

void yield(context & _context){
	SwitchToFiber(_context._contex);
}

}// context
}// Hemsleya

#endif //_WINDOWS