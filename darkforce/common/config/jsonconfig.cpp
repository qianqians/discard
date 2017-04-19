/*
 * jsonconfig.cpp
 *
 *  Created on: 2015-3-25
 *      Author: qianqians
 */
#include "config.h"

#include <JsonParser.h>

#include <fstream>
#include <boost/enable_shared_from_this.hpp>
#include <boost/make_shared.hpp>
#include <boost/any.hpp>

#include <pool/mempool.h>

namespace Fossilizid{
namespace config{

config::config(std::string & file){
	auto fs = std::ifstream(file);
	if (!fs.is_open()){
		throw std::exception(("cannot find config file" + file).c_str());
	}

	std::string buff;
	fs >> buff;

	Fossilizid::JsonParse::unpacker(handle, buff);
}
	

config::config(boost::any _handle){
	handle = _handle;
}

config::~config(){
}

bool config::has_key(std::string key){
	return (Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle))->find(key) == (Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle))->end();
}

bool config::get_value_bool(std::string key){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonBool>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle)))[key]);
}

int64_t config::get_value_int(std::string key){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonInt>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle)))[key]);
}

double config::get_value_float(std::string key){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonFloat>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle)))[key]);
}

std::string config::get_value_string(std::string key){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonString>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle)))[key]);
}
	
boost::shared_ptr<config> config::get_value_dict(std::string key){
	auto _handle = (*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle)))[key];
	config * _config = (config*)pool::mempool::allocator(sizeof(config));
	new (_config) config(_handle);
		
	return boost::shared_ptr<config>(_config, boost::bind(config::releaseconfig, _1));
}

boost::shared_ptr<config> config::get_value_list(std::string key){
	auto _handle = (*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(handle)))[key];
	config * _config = (config*)pool::mempool::allocator(sizeof(config));
	new (_config)config(_handle);

	return boost::shared_ptr<config>(_config, boost::bind(config::releaseconfig, _1));
}

size_t config::get_list_size(){
	return (Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonArray>(handle))->size();
}

bool config::get_list_bool(int index){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonBool>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonArray>(handle)))[index]);
}
	
int64_t config::get_list_int(int index){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonInt>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonArray>(handle)))[index]);
}
	
double config::get_list_float(int index){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonFloat>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonArray>(handle)))[index]);
}
	
std::string config::get_list_string(int index){
	return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonString>((*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonArray>(handle)))[index]);
}

boost::shared_ptr<config> config::get_list_dict(int index){
	auto _handle = (*(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonArray>(handle)))[index];
	config * _config = (config*)pool::mempool::allocator(sizeof(config));
	new (_config) config(_handle);
		
	return boost::shared_ptr<config>(_config, boost::bind(config::releaseconfig, _1));
}

} /* namespace config */
} /* namespace Fossilizid */

