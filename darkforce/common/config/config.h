/*
 * config.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _config_h
#define _config_h

#include <string>
#include <vector>

#include <boost/shared_ptr.hpp>
#include <boost/any.hpp>

namespace Fossilizid{
namespace config{

class config{
public:
	config(std::string & file);
	~config();

private:
	config(boost::any _handle);

	static void releaseconfig(config * _config){
		_config->~config();

	}

public:
	bool has_key(std::string key);

	bool get_value_bool(std::string key);

	int64_t get_value_int(std::string key);

	double get_value_float(std::string key);

	std::string get_value_string(std::string key);
	
	boost::shared_ptr<config> get_value_dict(std::string key);

	boost::shared_ptr<config> get_value_list(std::string key);

	size_t get_list_size();

	bool get_list_bool(int index);
	
	int64_t get_list_int(int index);
	
	double get_list_float(int index);
	
	std::string get_list_string(int index);
	
	boost::shared_ptr<config> get_list_dict(int index);

private:
	boost::any handle;

};

} /* namespace log */
} /* namespace Fossilizid */

#endif //_config_h
