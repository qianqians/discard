
/*
 * mutex.h
 *
 *  Created on: 2014-12-27
 *      Author: qianqians
 */
#ifndef _mutex_h
#define _mutex_h

#include <vector>
#include <context/context.h>
#include <uuid/uuid.h>

namespace Fossilizid{
namespace juggle{

class mutex{
public:
	mutex();
	~mutex();

	/*
	 * lock
	 */
	void lock();

	/*
	 * unlock
	 */
	void unlock();

private:
	bool _mutex;
	std::vector<context::context * > wait_context_list;

};

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_acceptservice_h