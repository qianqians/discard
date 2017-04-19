/*
 * socket_function.h
 *   Created on: 2013-10-2
 *       Author: qianqians
 * socket_function
 */
#ifndef _SOCKET_FUNCTION_H
#define _SOCKET_FUNCTION_H

#ifdef _WINDOWS

#include "winhdef.h"

#include "../buffstruct.h"
#include "../socket.h"

#include <vector>

#include <boost/cstdint.hpp>

namespace Hemsleya{
namespace async_net{

void async_disconnect(SOCKET s);

namespace TCP{

void async_accept(SOCKET sl, SOCKET sa, char * recvbuff, boost::function<void() > acceptcallback);

void async_connect(SOCKET s, const sockaddr * addr, boost::function<void() > connectcallback);

void async_send(SOCKET s, const std::vector<buffstruct> * const buff, boost::function<void() > sendcallback);

void async_recv(SOCKET s, char * buff, const uint32_t buflen, boost::function<void(char * buff, uint32_t recvlen) > recvcallback);

} // TCP

namespace UDP{

void async_sendto(SOCKET s, const sockaddr * addr, const std::vector<buffstruct> * const buff, boost::function<void() > sendtocallback);

void async_recvfrom(SOCKET s, char * buff, const uint32_t buflen, boost::function<void(char * buff, uint32_t len, const address & addr) > recvfromcallback);

} // UDP

} // async_net
} // Hemsleya

#endif //_WINDOWS

#endif //_SOCKET_FUNCTION_H