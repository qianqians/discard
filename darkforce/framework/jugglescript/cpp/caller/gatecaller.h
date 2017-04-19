/*
 * juggle codegen code explain notes
 *
 * juggle is a rpc framework under the GPL v2 License
 * 
 * juggle codegen generate rcp code
 * only for send message and dispense message
 * so users need write code handle process message
 *
 * this notes create in 2015.4.26
 * made for juggle codegen
 * autor qianqians
 */

#include <juggle.h>
#include <boost/make_shared.hpp>

namespace sync{

class gate: public Fossilizid::juggle::caller{
public:
	gate(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "gate"){
	}

	~gate(){
	}

};

}

namespace async{

class gate: public Fossilizid::juggle::caller{
public:
	gate(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "gate"){
	}

	~gate(){
	}

};

}

