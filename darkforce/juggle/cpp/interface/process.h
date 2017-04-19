/*
 * process.h
 *
 *  Created on: 2015-6-2
 *      Author: qianqians
 */
#ifndef _modules_h
#define _modules_h

#include <set>
#include <string>
#include <vector>

#include <boost/thread/mutex.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/enable_shared_from_this.hpp>
#include <boost/function.hpp>
#include <boost/unordered_map.hpp>

#include "../interface/rpcchannel.h"

#include <uuid/uuid.h>

namespace Fossilizid{
namespace juggle{

class process : public boost::enable_shared_from_this<process>{
public:
	process();
	~process();

public:
	/* 
	 * process event
	 */
	virtual void run();

public:
	/*
	 * add rpcsession
	 */
	virtual void add_rpcsession(boost::shared_ptr<channel> ch);
	/*
	 * remove rpcsession
	 */
	virtual void remove_rpcsession(boost::shared_ptr<channel> ch);

public:
	/*
	* register module method
	*/
	void register_module_method(std::string methodname, boost::function<void(boost::shared_ptr<channel>, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > ) > modulemethod);

	/*
	* register rpc callback
	*/
	void register_rpc_callback(uuid::uuid, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > ) > callback);

protected:
	boost::mutex mu_method_map;
	boost::unordered_map<std::string, boost::function<void(boost::shared_ptr<channel>, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > ) > > method_map;

	boost::mutex mu_method_callback_map;
	boost::unordered_map<uuid::uuid, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > ) > > method_callback_map;

protected:
	boost::mutex mu_channel;
	std::set<boost::shared_ptr<channel> > set_channel;

	boost::mutex mu_new_channel;
	std::vector<boost::shared_ptr<channel> > array_new_channel;
	
};

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_moudles_h