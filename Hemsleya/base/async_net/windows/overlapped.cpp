/*
 * overlapped.cpp
 *   Created on: 2013-10-2
 *       Author: qianqians
 * overlapped
 */
#ifdef _WINDOWS

#include "overlapped.h"
#include "overlappedaccept.h"
#include "overlappedsend.h"
#include "overlappedrecv.h"
#include "overlappedrecvfrom.h"
#include "overlappedconnect.h"
#include "overlappeddisconnect.h"

#include "../socket.h"

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya { 
namespace async_net { 

abstract_factory::abstract_factory<overlapped> _abstract_factory_overlapped;
abstract_factory::abstract_factory<overlappedaccept> _abstract_factory_overlappedaccept;
abstract_factory::abstract_factory<overlappedsend> _abstract_factory_overlappedsend;
abstract_factory::abstract_factory<overlappedrecv> _abstract_factory_overlappedrecv;
abstract_factory::abstract_factory<overlappedrecvfrom> _abstract_factory_overlappedrecvfrom;
abstract_factory::abstract_factory<overlappedconnect> _abstract_factory_overlappedconnect;
abstract_factory::abstract_factory<overlappeddisconnect> _abstract_factory_overlappeddisconnect;

overlapped * GetOverlapped(eventtype _eventtype){
	overlapped * poverlapped = 0;
	
	do{
		if (_eventtype == event_accept){
			poverlapped = _abstract_factory_overlappedaccept.create_product();
		} else if (_eventtype == event_send || _eventtype == event_sendto){
			poverlapped = _abstract_factory_overlappedsend.create_product();
		} else if (_eventtype == event_recv){
			poverlapped = _abstract_factory_overlappedrecv.create_product();
		} else if (_eventtype == event_recvfrom){
			poverlapped = _abstract_factory_overlappedrecvfrom.create_product();
		} else if (_eventtype == event_connect){
			poverlapped = _abstract_factory_overlappedconnect.create_product();
		} else if (_eventtype == event_disconnect){
			poverlapped = _abstract_factory_overlappeddisconnect.create_product();
		} else{
			poverlapped = _abstract_factory_overlapped.create_product();
		}
	}while(0);
	
	poverlapped->type = _eventtype;
	
	return poverlapped;
}

void ReleaseOverlapped(overlapped * poverlapped){
	do{
		if (poverlapped->type == event_accept){
			_abstract_factory_overlappedaccept.release_product(static_cast<overlappedaccept*>(poverlapped));
		} else if (poverlapped->type == event_send || poverlapped->type == event_sendto){
			_abstract_factory_overlappedsend.release_product(static_cast<overlappedsend*>(poverlapped));
		} else if (poverlapped->type == event_recv){
			_abstract_factory_overlappedrecv.release_product(static_cast<overlappedrecv*>(poverlapped));
		} else if (poverlapped->type == event_recvfrom){
			_abstract_factory_overlappedrecvfrom.release_product(static_cast<overlappedrecvfrom*>(poverlapped));
		} else if (poverlapped->type == event_connect){
			_abstract_factory_overlappedconnect.release_product(static_cast<overlappedconnect*>(poverlapped));
		} else if (poverlapped->type == event_disconnect){
			_abstract_factory_overlappeddisconnect.release_product(static_cast<overlappeddisconnect*>(poverlapped));
		} else{
			_abstract_factory_overlapped.release_product(poverlapped);
		}
	} while (0);
}

} //async_net
} //Hemsleya

#endif //_WINDOWS