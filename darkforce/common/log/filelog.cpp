/*
 * filelog.cpp
 *
 *  Created on: 2015-3-25
 *      Author: qianqians
 */
#include "log.h"

#include <boost/phoenix/bind.hpp>
#include <boost/log/expressions/attr_fwd.hpp>
#include <boost/log/expressions/attr.hpp>

namespace Fossilizid{
namespace log{

void log::setfile(std::string & file){
	logfile = file;
}

void log::setlog(){
	boost::log::add_file_log(
		boost::log::keywords::file_name = logfile + "/.%Y-%m-%d_%H-%M-%S.%N",
		boost::log::keywords::rotation_size = 10 * 1024 * 1024,
		boost::log::keywords::time_based_rotation = boost::log::sinks::file::rotation_at_time_point(0, 0, 0),
		boost::log::keywords::format = "[%TimeStamp%] (%Severity%) : %Message%",
		boost::log::keywords::min_free_space=3 * 1024 * 1024);
}

} /* namespace achieve */
} /* namespace Fossilizid */

