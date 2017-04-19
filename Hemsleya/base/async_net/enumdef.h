/*
 * enumdef.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * enumdef
 */
#ifndef _ENUMDEF_H
#define _ENUMDEF_H

namespace Hemsleya { 
namespace async_net { 

enum recv_state{
	_recv,
	_no_recv,
};

namespace TCP{

enum acceptstate{
	beginaccept,
	endaccept,
};

enum connect_state{
	_connect,
	_not_connect,
};

}// TCP

} //async_net
} //Hemsleya


#endif //_ENUMDEF_H
