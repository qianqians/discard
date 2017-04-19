/*
 * signaldef.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * callbackdef
 */
#ifndef _SIGNALDEF_H
#define _SIGNALDEF_H

#include <boost/function.hpp>
#include <boost/signals2.hpp>

namespace Hemsleya { 
namespace async_net { 

namespace TCP{

class socket;

typedef boost::signals2::signal<void(socket & sa) > acceptsignal;

typedef boost::signals2::signal<void() > sendsignal;

typedef boost::signals2::signal<void(char * buff, uint32_t len) > recvsignal;

typedef boost::signals2::signal<void() > connectsignal;

}// TCP

class address;

namespace UDP{

typedef boost::signals2::signal<void() > sendtosignal;

typedef boost::signals2::signal<void(char * buff, uint32_t len, const address & addr) > recvfromsignal;

}

} //async_net
} //Hemsleya


#endif // _SIGNALDEF_H
