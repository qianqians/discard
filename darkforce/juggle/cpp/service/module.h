/*
 * module.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _module_h
#define _module_h

#include <string>
#include <vector>

#include <boost/shared_ptr.hpp>
#include <boost/any.hpp>

#include "../interface/rpcchannel.h"
#include "../interface/process.h"

#include <uuid/uuid.h>

namespace Fossilizid{
namespace juggle{

class module{
public:
	module(boost::shared_ptr<process> __process, std::string modulename, uuid::uuid & moduleid);
	~module();

public:
	/*
	 * get moudle name
	 */
	std::string module_name();

	/*
	 * get moudle id
	 */
	uuid::uuid module_id();

	/*
	 * moudle info
	 */
	void moudle_info(boost::shared_ptr<boost::unordered_map<std::string, boost::any > > & info);

	/*
	 * call rpc mothed
	 */
	void call_module_method(boost::shared_ptr<channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any > > value);

protected:
	boost::shared_ptr<process> _process;

	std::vector<boost::any> _module_func;
	std::string _module_name;
	uuid::uuid _module_id;
	
};

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_obj_h