/*
 * ConnectException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net ConnectException
 */
#include "ConnectException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

ConnectException::ConnectException(const char * info, int errcode) : exception(info, errcode) {
}

ConnectException::ConnectException(const char * info) : exception(info) {
}

ConnectException::~ConnectException(){
}

}// exception
}// async_net
}// Hemsleya

