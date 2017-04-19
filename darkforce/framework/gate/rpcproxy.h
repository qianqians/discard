/*
 * rpcproxy.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _rpcproxy_h
#define _rpcproxy_h

#include <boost/shared_ptr.hpp>

#include <uuid/uuid.h>

#include <rpcchannel.h>
#include <process.h>

namespace Fossilizid{
namespace gate{

class rpcproxy : public juggle::process{
public:
	rpcproxy(boost::function<boost::shared_ptr<juggle::channel>(boost::shared_ptr<juggle::channel>, uuid::uuid) > _routing);
	~rpcproxy();

public:
	virtual void run();
	
public:
	void proxy(boost::shared_ptr<juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > value);

	void handle_callback(boost::shared_ptr<juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > result);

private:
	boost::function<boost::shared_ptr<juggle::channel>(boost::shared_ptr<juggle::channel>, uuid::uuid) > routing;

};

} /* namespace gate */
} /* namespace Fossilizid */

#endif //_rpcproxy_h
