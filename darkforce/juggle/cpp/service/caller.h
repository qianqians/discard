/*
 * caller.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _caller_h
#define _caller_h

#include <string>

#include <boost/shared_ptr.hpp>
#include <boost/function.hpp>

#include "../interface/rpcchannel.h"
#include "../interface/process.h"

#include <uuid/uuid.h>

namespace Fossilizid{
namespace juggle{

class caller{
public:
	caller(boost::shared_ptr<process> __process, boost::shared_ptr<channel> ch, std::string modulename);
	~caller();

public:
	/*
	 * get class name
	 */
	std::string module_name();

	/*
	 * get object id
	 */
	uuid::uuid module_id();

	/*
	 * call rpc mothed
	 */
	boost::shared_ptr<boost::unordered_map<std::string, boost::any > > call_module_method_sync(std::string methodname, boost::shared_ptr<boost::unordered_map<std::string, boost::any > > value);

	/*
	 * call rpc mothed fast
	 */
	void call_module_method_async(std::string methodname, boost::shared_ptr<boost::unordered_map<std::string, boost::any > > value, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any > > ) > callback);

protected:
	boost::shared_ptr<process> _process;

	std::string _module_name;
	uuid::uuid _module_id;
	boost::shared_ptr<channel> _ch;
	
};

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_caller_h