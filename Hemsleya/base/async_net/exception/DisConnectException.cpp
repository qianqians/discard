/*
 * DisConnectException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net DisConnectException
 */
#include "DisConnectException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

DisConnectException::DisConnectException(const char * info, int errcode) : exception(info, errcode) {
}

DisConnectException::DisConnectException(const char * info) : exception(info) {
}

DisConnectException::~DisConnectException(){
}

}// exception
}// async_net
}// Hemsleya

