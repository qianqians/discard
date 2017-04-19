/*
 * logicservice.h
 *
 *  Created on: 2015-3-25
 *      Author: qianqians
 */
#ifndef _logicservice_h
#define _logicservice_h

#include <juggle.h>

namespace Fossilizid{
namespace logic{

class logicservice{
public:
	static boost::shared_ptr<logicservice> createinstance();

	static boost::shared_ptr<logicservice> getinstance();

public:
	void poll();

	uint64_t unixtime();

private:
	logicservice();
	~logicservice();

	void init();

private:
	static void releaselogicservice(logicservice * _plogicservice);

private:
	boost::shared_ptr<juggle::service> _service;

private:
	static boost::shared_ptr<logicservice> _logicservice;

};

}
}

#endif //_logicservice_h