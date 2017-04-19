/*
 * service.cpp
 *
 *  Created on: 2014-10-8
 *      Author: qianqians
 */
#include "service.h"
#include "rpcsession.h"
#include "tempsession.h"
#include "obj.h"

#include "../../third_party/json/json_protocol.h"

#ifdef _WINDOWS
#include <Windows.h>
boost::uint64_t _clock(){
	return GetTickCount64();
}
#endif

namespace Fossilizid{
namespace reduce {

service * _service_handle = 0;

service::service(){
	isrun.store(true);

	clockstamp = _clock();
	timestamp = time(0);

	_service_handle = this;
}

service::~service(){
	isrun.store(false);

	_thread_group.join_all();

	for (auto v : map_session){
		remoteq::close(v.first);
	}
	map_session.clear();
}

void service::init(){
	std::function<context::context () > _wake_up = [this](){
		{
			boost::mutex::scoped_lock lock(mu_wait_context_list);

			{
				boost::mutex::scoped_lock lock(mu_wake_up_set);
				for (auto it = wake_up_set.begin(); it != wake_up_set.end();){
					auto find = wait_context_list.find(*it);
					it = wake_up_set.erase(it);
					if (find != wait_context_list.end()){
						context::context _context = std::get<1>(find->second);
						boost::shared_ptr<Json::Value> value = std::get<3>(find->second);
						(*value)["istimeout"] = false;
						return _context;
					}
				}
			}

			for (auto it = wait_context_list.begin(); it != wait_context_list.end();){
				if (std::get<2>(it->second) >= unixtime()){
					context::context _context = std::get<1>(it->second);
					boost::shared_ptr<Json::Value> value = std::get<3>(it->second);
					(*value)["istimeout"] = true;
					return _context;
				}
				else{
					it++;
				}
			}
		}

		{
			boost::mutex::scoped_lock lock(mu_wake_up_vector);
			for (auto it = wait_weak_up_context.begin(); it != wait_weak_up_context.end();){
				auto _context = it->second;
				wait_weak_up_context.erase(it);
				return _context;
			}
		}

		return (context::context)0;
	};

	std::function<void()> _run_logic = [this, _wake_up](){
		timestamp += _clock() - clockstamp;

		boost::shared_lock<boost::shared_mutex> lock(mu_map_session);
		for (auto var : map_session){
			push_current_session(var.second);
			var.second->do_logic();
			var.second->do_time(timestamp);
			pop_current_session();
		}
	};

	_loop_main = [this, _run_logic, _wake_up](){
		while (1){
			_run_network();

			_run_logic();

			context::context _run_logic_context = _wake_up();
			if (_run_logic_context != 0){
				set_current_context(_run_logic_context);
				context::yield(_run_logic_context);
			}
		}
	};

	context::context _loop_main_context = context::getcontext(_loop_main);
	tsp_loop_main_context.reset(new context::context(_loop_main_context));

	context::context _loog_return_context = context::makecontext();
	tsp_loop_return_context.reset(new context::context(_loog_return_context));
	set_current_context(_loog_return_context);

	auto run_network = [this](){
		while (isrun.load()){
			_run_network();
		}
	};

	_thread_group.create_thread(run_network);
}

void service::poll(){
	context::context _context = get_current_context();

	context::context * _loop_main_context = tsp_loop_main_context.get();
	if (_context != *_loop_main_context){
		wait_weak_up_context.insert(std::make_pair(UUID(), _context));

		set_current_context(_loop_main_context);
		context::yield(*_loop_main_context);
	}
}

boost::shared_ptr<session> service::create_rpcsession(uuid epuuid, remoteq::CHANNEL ch){
	{
		boost::unique_lock<boost::shared_mutex> lock(mu_map_session);
		map_session.erase(ch);
	}

	{
		boost::unique_lock<boost::shared_mutex> lock(mu_map_uuid_session);
		std::unordered_map<uuid, boost::shared_ptr<rpcsession> >::iterator it = map_uuid_session.find(epuuid);
		if (it == map_uuid_session.end()){
			map_uuid_session.insert(std::make_pair(epuuid, boost::shared_ptr<rpcsession>(new rpcsession(epuuid, ch))));
		} else{
			map_uuid_session[epuuid]->reset(ch);
		}

		{
			boost::unique_lock<boost::shared_mutex> lock(mu_map_session);
			map_session[ch] = static_cast<boost::shared_ptr<session> >(map_uuid_session[epuuid]);
		}
	}

	return map_uuid_session[epuuid];
}

boost::shared_ptr<session> service::get_rpcsession(uuid epuuid){
	boost::unique_lock<boost::shared_mutex> lock(mu_map_uuid_session);
	std::unordered_map<uuid, boost::shared_ptr<rpcsession> >::iterator it = map_uuid_session.find(epuuid);
	if (it == map_uuid_session.end()){
		return 0;
	}

	return static_cast<boost::shared_ptr<session> >(it->second);
}

void service::set_current_context(context::context _context){
	context::context * _c = tsp_context.get();
	if (_c == 0){
		_c = new context::context;
		tsp_context.reset(_c);
	}
	*_c = _context;
}

context::context service::get_current_context(){
	return *(tsp_context.get());
}

boost::shared_ptr<Json::Value> service::wait(uuid _uuid, boost::uint64_t wait_time){
	context::context _context = get_current_context();

	{
		boost::mutex::scoped_lock lock(mu_wait_context_list);
		wait_context_list.insert(std::make_pair(_uuid, std::make_tuple(_uuid, _context, wait_time * 1000, boost::shared_ptr<Json::Value>(0))));
	}

	context::context * _tsp_loop_main_context = tsp_loop_main_context.get();
	if (_tsp_loop_main_context != 0){
		context::yield(*_tsp_loop_main_context);
	} else if (*_tsp_loop_main_context == _context){
		context::context _context = context::getcontext(_loop_main);
		set_current_context(_context);
		context::yield(_context);
	} else {
		throw std::exception("_tsp_loop_main_context is null");
	}

	boost::shared_ptr<Json::Value> value = std::get<3>(wait_context_list[_uuid]);
	
	return value;
}

void service::register_global_obj(boost::shared_ptr<obj> obj){
	boost::mutex::scoped_lock lock(mu_map_global_obj);
	map_global_obj.insert(std::make_pair(obj->objid(), obj));
	map_global_obj_classname.insert(std::make_pair(obj->class_name(), obj));
}

boost::shared_ptr<obj> service::get_global_obj(std::string classname){
	boost::mutex::scoped_lock lock(mu_map_global_obj);
	auto find = map_global_obj_classname.find(classname);
	if (find != map_global_obj_classname.end()){
		return find->second;
	}
	return nullptr;
}

void service::global_obj_lock(){
	mu_map_global_obj.lock();
}

void service::global_obj_unlock(){
	mu_map_global_obj.unlock();
}

service::global_obj_iterator service::global_obj_begin(){
	return map_global_obj.begin();
}

service::global_obj_iterator service::global_obj_end(){
	return map_global_obj.end();
}

boost::uint64_t service::unixtime(){
	return timestamp;
}

void service::push_current_session(boost::shared_ptr<session> _session){
	std::stack<boost::shared_ptr<session> > * _tsp_stack = tsp_current_session.get();
	if (_tsp_stack == 0){
		_tsp_stack = new std::stack < boost::shared_ptr<session> >;
		tsp_current_session.reset(_tsp_stack);
	}
	_tsp_stack->push(_session);
}

void service::pop_current_session(){
	std::stack<boost::shared_ptr<session> > * _tsp_stack = tsp_current_session.get();
	if (_tsp_stack != 0){
		_tsp_stack->pop();
	}
}

boost::shared_ptr<session> service::get_current_session(){
	std::stack<boost::shared_ptr<session> > * _tsp_stack = tsp_current_session.get();
	if (_tsp_stack != 0){
		return _tsp_stack->top();
	}
	return 0;
}

void service::push_current_obj(boost::shared_ptr<obj> _obj){
	std::stack<boost::shared_ptr<obj> > * _tsp_stack = tsp_current_obj.get();
	if (_tsp_stack == 0){
		_tsp_stack = new std::stack < boost::shared_ptr<obj> >;
		tsp_current_obj.reset(_tsp_stack);
	}
	_tsp_stack->push(_obj);
}

void service::pop_current_obj(){
	std::stack<boost::shared_ptr<obj> > * _tsp_stack = tsp_current_obj.get();
	if (_tsp_stack != 0){
		_tsp_stack->pop();
	}
}

boost::shared_ptr<obj> service::get_current_obj(){
	std::stack<boost::shared_ptr<obj> > * _tsp_stack = tsp_current_obj.get();
	if (_tsp_stack != 0){
		return _tsp_stack->top();
	}
	return 0;
}


} /* namespace reduce */
} /* namespace Fossilizid */