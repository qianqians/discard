/*
 * qianqians
 * 2016/7/4
 * Imodule.h
 */
#ifndef _Imodule_h
#define _Imodule_h

#include <memory>
#include <functional>
#include <unordered_map>
#include <iostream>

#include "Ichannel.h"

namespace juggle {

extern std::shared_ptr<Ichannel> current_ch;

class Imodule {
public:
	void process_event(std::shared_ptr<Ichannel> _ch, std::shared_ptr<std::vector<boost::any> > _event) {
		current_ch = _ch;

		auto func_name = boost::any_cast<std::string>((*_event)[1]);
		auto func = protcolcall_set.find(func_name);
		if (func != protcolcall_set.end()) {
			func->second(boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*_event)[2]));
		}
		else {
			std::cout << "can not find function named:" << func_name << std::endl;
		}

		current_ch = nullptr;
	}

public:
	std::string module_name;

protected:
	std::unordered_map<std::string, std::function<void(std::shared_ptr<std::vector<boost::any> > )> > protcolcall_set;

};

}

#endif // !_Imodule_h
