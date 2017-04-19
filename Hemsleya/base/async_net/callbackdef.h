/*
 * callbackdef.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * callbackdef
 */
#ifndef _CALLBACKDEF_H
#define _CALLBACKDEF_H

#include <boost/function.hpp>

namespace Hemsleya { 
namespace async_net { 

namespace TCP{

class socket;

typedef boost::function<void(socket & sa) > acceptcallback;

typedef boost::function<void() > sendcallback;

typedef boost::function<void(char * buff, uint32_t len) > recvcallback;

typedef boost::function<void() > connectcallback;

}// TCP

class address;

namespace UDP{

typedef boost::function<void() > sendtocallback;

typedef boost::function<void(char * buff, uint32_t len, const address & addr) > recvfromcallback;

}

} //async_net
} //Hemsleya


#endif // _CALLBACKDEF_H
