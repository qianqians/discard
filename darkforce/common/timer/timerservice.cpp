/*
 * timerservice.cpp
 *
 *  Created on: 2015-3-30
 *      Author: qianqians
 */
#include "timerservice.h"

#include <pool/mempool.h>

#include <boost/make_shared.hpp>

namespace Fossilizid{
namespace timer{

boost::shared_ptr<timerservice> Fossilizid::timer::_timerservice = nullptr;
boost::shared_ptr<timerservice> Fossilizid::timer::timerservice::_timerservice = nullptr;

void timerservice::releasetimerservice(timerservice * _ptimerservice){
	_ptimerservice->~timerservice();
	pool::mempool::deallocator(_ptimerservice, sizeof(timerservice));
}

boost::shared_ptr<timerservice> timerservice::createinstance(){
	timerservice * _ptimerservice = (timerservice *)pool::mempool::allocator(sizeof(timerservice));
	new (_ptimerservice)timerservice();
	timerservice::_timerservice = boost::shared_ptr<timerservice>(_ptimerservice, boost::bind(timerservice::releasetimerservice, _1));
	Fossilizid::timer::_timerservice = timerservice::_timerservice;
	return timerservice::_timerservice;
}

boost::shared_ptr<timerservice> timerservice::getinstance(){
	return timerservice::_timerservice;
}

timerservice::timerservice(){
}

timerservice::~timerservice(){
}

void timerservice::init(){
}

void timerservice::poll(uint64_t t){
	boost::mutex::scoped_lock l(_mu);
	auto it = _timetimer.begin();
	for( ; it != _timetimer.end();){
		if (it->first <= t){
			{
				boost::mutex::scoped_lock l(it->second->mu);
				if (!it->second->iscancel){
					it->second->fun(t);
				}
			}
			_idtimer.erase(it->second->id);
			it = _timetimer.erase(it);
		}else{
			it++;
		}
	}

	timetmp = t;
}

uint64_t timerservice::get_event_time(){
	if (_timetimer.empty()){
		return _timetimer.begin()->first;
	}
	return 0;
}

uint64_t timerservice::addtimer(uint64_t t, std::function<void(uint64_t)> timerfun){
	boost::mutex::scoped_lock l(_mu);
	auto end = --(_timetimer.end());
	auto id = end->first + 1;
	auto timer_ = boost::make_shared<timer>();
	timer_->fun = timerfun;
	timer_->id = id;
	timer_->iscancel = false;
	_timetimer.insert(std::make_pair(t + timetmp, timer_));
	_idtimer.insert(std::make_pair(id, timer_));

	return id;
}

void timerservice::canceltimer(uint64_t timerid){
	auto find = _idtimer.find(timerid);
	boost::mutex::scoped_lock l(find->second->mu);
	find->second->iscancel = true;
}

} /* namespace logic */
} /* namespace Fossilizid */

