/*
 * channel.h
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#ifndef _json_channel_h
#define _json_channel_h

#include <container/msque.h>
#include <remote_queue.h>
#include <rpcchannel.h>
#include <JsonParser.h>

#include <boost/shared_ptr.hpp>

namespace Fossilizid{
namespace achieve{

class channel : public juggle::channel{
public:
	channel(remoteq::CHANNEL _ch){
		ch = _ch;
	}

	~channel(){
	}

	/*
	 * push a object to channel
	 */
	virtual void push(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		remoteq::ipv4::tcp::push(ch, v, Fossilizid::JsonParse::packer);
	}

	/*
	 * get a object from channel
	 */
	virtual boost::shared_ptr<boost::unordered_map<std::string, boost::any> > pop(){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = 0;
		if (que.pop(v)){
			return v;
		}
		return 0;
	}

private:
	void handle_recv(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
			que.push(v);
	}

	friend class sessioncontainer;

private:
	remoteq::CHANNEL ch;
	container::msque<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > que;

};

} /* namespace achieve */
} /* namespace Fossilizid */

#endif //_channel_h