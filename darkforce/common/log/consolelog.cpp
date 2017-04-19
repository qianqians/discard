/*
 * consolelog.cpp
 *
 *  Created on: 2015-3-25
 *      Author: qianqians
 */
#include "log.h"

#include <exception>

#include <boost/phoenix/bind.hpp>
#include <boost/log/expressions/attr_fwd.hpp>
#include <boost/log/expressions/attr.hpp>

namespace Fossilizid{
namespace log{

void log::setfile(std::string & file){
	throw std::exception("log handle not a filelog");
}

void log::setlog(){
}

} /* namespace log */
} /* namespace Fossilizid */

