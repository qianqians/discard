/*
 * service.cpp
 *
 *  Created on: 2014-10-8
 *      Author: qianqians
 */
#include "connectservice.h"
#include "rpcsession.h"
#include "tempsession.h"
#include "obj.h"

#include "../../third_party/json/json_protocol.h"

namespace Fossilizid{
namespace reduce {

connectservice::connectservice() : service(){
	que = remoteq::queue();
}

connectservice::~connectservice(){
	remoteq::close(que);
}

boost::shared_ptr<session> connectservice::connect(char * ip, short port){
	Fossilizid::remoteq::ENDPOINT ep = Fossilizid::remoteq::endpoint(ip, port);
	Fossilizid::remoteq::CHANNEL ch = Fossilizid::remoteq::connect(ep, que);

	if (ch != 0){
		boost::unique_lock<boost::shared_mutex> lock(mu_map_session);
		map_session[ch] = boost::shared_ptr<session>(new tempsession(ch, "connectservice"));
		
		return map_session[ch];
	}

	return 0;
}

void connectservice::_run_network(){
	remoteq::EVENT ev = remoteq::queue(que);
	switch (ev.type)
	{
	case remoteq::event_type_none:
		break;
				
	case remoteq::event_type_recv:
		{	
			remoteq::CHANNEL ch = ev.handle.ch;
				
			Json::Value value;
			while (remoteq::pop(ch, value, json_parser::buf_to_json)){
				Json::Value _suuid = value.get("suuid", Json::nullValue);
				if (_suuid.isNull()){
					continue;
				}

				boost::mutex::scoped_lock _scoped_lock(mu_wait_context_list);
				auto finduuidcontex = wait_context_list.find(_suuid.asString());
				if (finduuidcontex != wait_context_list.end()){
					Json::Value _eventtype = value.get("eventtype", Json::nullValue);
					if (_eventtype.asString() != "rpc_event"){
						boost::shared_ptr<session> _session = 0;
						{
							boost::shared_lock<boost::shared_mutex> lock(mu_map_session);
							_session = map_session[ch];
						}
						_session->do_pop(map_session[ch], value);
					}
						
					std::get<3>(finduuidcontex->second) = boost::shared_ptr<Json::Value>(new Json::Value(value));
					boost::mutex::scoped_lock lock(mu_wake_up_set);
					wake_up_set.insert(_suuid.asString());
				}else{
					boost::shared_ptr<session> _session = 0;
					{
						boost::shared_lock<boost::shared_mutex> lock(mu_map_session);
						_session = map_session[ch];
					}
					_session->do_pop(map_session[ch], value);
				}
			}
		}
		break;

	case remoteq::event_type_disconnect:
		{
			boost::unique_lock<boost::shared_mutex> l(mu_map_session);
			map_session[ev.handle.ch];

			boost::unique_lock<boost::shared_mutex> ll(mu_map_uuid_session);
			map_uuid_session.erase(boost::static_pointer_cast<rpcsession>(map_session[ev.handle.ch])->epuuid());

			map_session.erase(ev.handle.ch);

			remoteq::close(ev.handle.ch);
		}
		break;

	default:
		break;
	}
}

} /* namespace reduce */
} /* namespace Fossilizid */