/*
 * semaphore.h
 *
 *  Created on: 2014-12-27
 *      Author: qianqians
 */
#ifndef _semaphore_h
#define _semaphore_h

#include <queue>

#include <boost/thread/mutex.hpp>
#include <boost/unordered_map.hpp>
#include <boost/any.hpp>
#include <boost/shared_ptr.hpp>

#include <context/context.h>
#include <uuid/uuid.h>

namespace Fossilizid{
namespace juggle{

class semaphore{
public:
	semaphore();
	~semaphore();

	/*
	 * post a signal 
	 */
	void post(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > signal);

	/*
	 * wait a signal
	 */
	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > wait(time_t timeout);

private:
	boost::mutex mu_signal;
	std::queue<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > _signal;
	uint64_t _timeout;
	context::context wait_ct;

	friend class juggleservice;
	
};

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_semaphore_h