/*
 * channel.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _interface_channel_h
#define _interface_channel_h

#include <string>

#include <boost/shared_ptr.hpp>
#include <boost/unordered_map.hpp>
#include <boost/any.hpp>

namespace Fossilizid{
namespace juggle{

class channel{
public:
	/*
	 * push a object to channel
	 */
	virtual void push(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v) = 0;
	
	/*
	 * get a object from channel
	 */
	virtual boost::shared_ptr<boost::unordered_map<std::string, boost::any> > pop() = 0;

};

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_interface_channel_h