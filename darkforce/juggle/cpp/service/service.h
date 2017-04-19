/*
 * service.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _service_base_h
#define _service_base_h

#include <set>

#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/unordered_map.hpp>

#include <uuid/uuid.h>
#include <context/context.h>

#include "../interface/service.h"
#include "../interface/rpcchannel.h"
#include "../interface/semaphore.h"
#include "../interface/process.h"

#include "module.h"

namespace Fossilizid{
namespace juggle{

class juggleservice : public service{
public:
	juggleservice();
	~juggleservice();

public:
	/*
	 * initialise service
	 */
	virtual void init();

	/*
	 * drive service work
	 */
	virtual void poll();
	
	/*
	 * unixtime
	 */
	virtual uint64_t unixtime();

public:
	/*
	 * register semaphore
	 */
	void register_semaphore(semaphore * _semaphore);

	/*
	 * get current context
	 */
	context::context get_current_context();

	/*
 	 * wake up context
	 */
	void wake_up_context(context::context ct);

	/*
 	 * scheduler
	 */
	void scheduler();

private:
	/*
	 * set current context
	 */
	void set_current_context(context::context _context);

	/*
	 * loop main
	 */
	void loop_main();

public:
	/*
	 * add process 
	 */
	void add_process(boost::shared_ptr<process> _process);

public:
	/*
	* set current channel
	*/
	void set_current_channel(boost::shared_ptr<channel> ch);

	/*
	* get current channel
	*/
	boost::shared_ptr<channel> get_current_channel();

private:
	boost::thread_specific_ptr<context::context> tss_current_context;

private:
	boost::thread_specific_ptr<boost::shared_ptr<channel> > tss_current_channel;

private:
	boost::mutex mu_wake_up_vector;
	std::vector<context::context> wait_weak_up_context;

	boost::mutex mu_vsemaphore;
	std::map<time_t, context::context> vsemaphore;

private:
	boost::mutex mu_process;
	std::set<boost::shared_ptr<process> > setprocess;

private:
	time_t clockstamp;
	time_t timestamp;

};

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_service_base_h