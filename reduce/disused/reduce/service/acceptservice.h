/*
 * acceptservice.h
 *
 *  Created on: 2014-11-3
 *      Author: qianqians
 */
#ifndef _acceptservice_h
#define _acceptservice_h

#include "service.h"

namespace Fossilizid{
namespace reduce{

class acceptservice : public service{
public:
	acceptservice(char * ip, short port);
	~acceptservice();

private:
	void _run_network();

private:
	remoteq::QUEUE que;
	remoteq::ENDPOINT ep;
	remoteq::ACCEPTOR acp;

};

} /* namespace reduce */
} /* namespace Fossilizid */

#endif //_acceptservice_h