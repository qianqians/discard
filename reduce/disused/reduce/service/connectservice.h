/*
 * connectservice.h
 *
 *  Created on: 2014-11-3
 *      Author: qianqians
 */
#ifndef _connectservice_h
#define _connectservice_h

#include "service.h"

namespace Fossilizid{
namespace reduce{

class connectservice : public service{
public:
	connectservice();
	~connectservice();
	
public:
	/*
	 * connect to a remote service
	 */
	boost::shared_ptr<session> connect(char * ip, short port);

private:
	void _run_network();

private:
	remoteq::QUEUE que;

};

} /* namespace reduce */
} /* namespace Fossilizid */

#endif //_service_h