/*
 * locale_obj.h
 *
 *  Created on: 2014-10-14
 *      Author: qianqians
 */
#ifndef _locale_obj_h
#define _locale_obj_h

#include <map>
#include <string>

#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/cstdint.hpp>
#include <boost/function.hpp>

#include "../json/jsoncpp/include/json/json.h"

#include "obj.h"

namespace Fossilizid{
namespace reduce{

class locale_obj : public obj{
public:
	locale_obj(std::string _class_name);
	~locale_obj();

public:
	/*
	 * register rpc mothed
	 */
	void register_rpc_mothed(std::pair<std::string, boost::function<Json::Value(Json::Value &) > > rpc_mothed);

private:
	virtual void call_rpc_mothed(Json::Value & value);

private:
	boost::mutex mu_maprpccall;
	std::map<std::string, boost::function<Json::Value(Json::Value &) > > maprpccall;
	
};

} /* namespace reduce */
} /* namespace Fossilizid */

#endif //_locale_obj_h