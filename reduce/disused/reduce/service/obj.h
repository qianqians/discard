/*
 * obj.h
 *
 *  Created on: 2014-10-14
 *      Author: qianqians
 */
#ifndef _obj_h
#define _obj_h

#include <map>
#include <string>
#include <unordered_map>

#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/function.hpp>
#include <boost/cstdint.hpp>

#include "../json/jsoncpp/include/json/json.h"
#include "../../container/msque.h"

#include "uuid.h"

namespace Fossilizid{
namespace reduce{

class session;

enum obj_state{
	obj_state_working,
	obj_state_pause,
};

class obj{
public:
	obj();
	~obj();

public:
	/*
	 * get class name
	 */
	std::string class_name();

	/*
	 * get object id
	 */
	uuid objid();

public:
	void push_rpc_event(std::pair<boost::shared_ptr<session>, Json::Value> & _event);

public:
	virtual void call_do_logic();

	virtual void call_rpc_event(boost::shared_ptr<session> session, Json::Value & value);
	
	virtual void call_do_time(boost::uint64_t time);

	virtual void call_add_time(boost::uint64_t time, boost::function<void(boost::uint64_t)> timefn);

protected:
	virtual void call_rpc_mothed(Json::Value & value);

	virtual void call_rpc_mothed_ret(Json::Value & value);

protected:
	virtual void push_rcp_mothed_ret(boost::shared_ptr<session> session, Json::Value & value);

protected:
	std::string _class_name;
	uuid _objid;
	
protected:
	boost::mutex mu_call_rpc_mothed_ret_callback;
	std::unordered_map<uuid, boost::function<void(Json::Value &)> > call_rpc_mothed_ret_callback;

protected:
	boost::mutex mu_call_time_callback;
	std::map<boost::uint64_t, boost::function<void(boost::uint64_t) > > call_time_callback;

protected:
	container::msque<std::pair<boost::shared_ptr<session>, Json::Value> > cmdque;

	boost::atomic_int states;

};

} /* namespace reduce */
} /* namespace Fossilizid */

#endif //_obj_h