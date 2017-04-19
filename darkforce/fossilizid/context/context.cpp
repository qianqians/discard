/*
 * context.cpp
 *
 *  Created on: 2014-8-18
 *      Author: qianqians
 */
#include "context.h"

#include <boost/thread/mutex.hpp>
#include <map>

namespace Fossilizid{
namespace context {

class contextimpl{
public:
	contextimpl(boost::function<void()> fn){
		contextfn = fn;
		pFiber = CreateFiber(1024 * 64, contextimpl::contextFiberProc, this);
	}

	~contextimpl(){
		DeleteFiber(pFiber);
	}

private:
#ifdef _WINDOWS
	static VOID CALLBACK contextFiberProc(_In_ PVOID lpParameter){
		contextimpl * ct = static_cast<contextimpl*>(lpParameter);
		if (!ct->contextfn.empty()){
			ct->contextfn();
		}
	}
#endif

public:
#ifdef _WINDOWS
	LPVOID pFiber;
#endif

private:
	boost::function<void()> contextfn;

};

context getcontext(boost::function<void()> fn){
#ifdef _WINDOWS
	contextimpl * _contextimpl = new contextimpl(fn);
#endif

	return _contextimpl->pFiber;
}

void yield(context ct){
#ifdef _WINDOWS
	SwitchToFiber(ct);
#endif 
}

void closecontext(context ct){
	DeleteFiber(ct);
}

} /* namespace context */
} /* namespace Fossilizid */