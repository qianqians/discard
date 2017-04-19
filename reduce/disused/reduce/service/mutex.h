/*
 * mutex.h
 *
 *  Created on: 2014-12-27
 *      Author: qianqians
 */
#ifndef _mutex_h
#define _mutex_h

#include "service.h"
#include "uuid.h"

namespace Fossilizid{
namespace reduce{

class mutex{
public:
	mutex();
	~mutex();

	void lock();

	void unlock();

private:
	bool _mutex;
	uuid _mutexid;

	std::vector<context::context * > wait_context_list;

};

} /* namespace reduce */
} /* namespace Fossilizid */

#endif //_acceptservice_h