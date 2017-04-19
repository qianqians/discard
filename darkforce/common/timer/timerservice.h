/*
 * timerservice.h
 *
 *  Created on: 2015-3-25
 *      Author: qianqians
 */
#ifndef _timerservice_h
#define _timerservice_h

#include <cstdint>
#include <functional>
#include <map>

#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>

namespace Fossilizid{
namespace timer{

class timerservice{
private:
	struct timer{
		uint64_t id;
		bool iscancel;
		boost::mutex mu;
		std::function<void(uint64_t)> fun;
	};

public:
	static boost::shared_ptr<timerservice> createinstance();

	static boost::shared_ptr<timerservice> getinstance();

private:
	timerservice();
	~timerservice();

	static void releasetimerservice(timerservice * _ptimerservice);

public:
	void init();

	void poll(uint64_t t);

	uint64_t addtimer(uint64_t t, std::function<void(uint64_t)>);

	void canceltimer(uint64_t timerid);

	uint64_t get_event_time();

private:
	std::map<uint64_t, boost::shared_ptr<timer> > _timetimer;

	std::map<uint64_t, boost::shared_ptr<timer> > _idtimer;

	uint64_t timetmp;

	boost::mutex _mu;

private:
	static boost::shared_ptr<timerservice> _timerservice;

};

extern boost::shared_ptr<timerservice> _timerservice;

#define add_timer(t, func) {if (_timerservice != 0){ \
		_timerservice->addtimer(t, func); \
	}else{\
		throw std::exception("timer handle is null");\
	}}

}
}

#endif //_logicservice_h