/*
 * SendException.h
 *
 *  Created on: May 22, 2013
 *      Author: qianqians
 * Hemsleya aysnc_net SendException
 */
#include "SendException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

SendException::SendException(const char * info, int errcode) : exception(info, errcode) {
}

SendException::~SendException(){
}

}// exception
}// async_net
}// Hemsleya

