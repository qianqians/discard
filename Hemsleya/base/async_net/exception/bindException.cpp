/*
 * bindException.cpp
 *
 *  Created on: 2013-10-6
 *      Author: qianqians
 * Hemsleya async_net bindException
 */
#include "bindException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

bindException::bindException(const char * info, int errcode) : exception(info, errcode){
}

bindException::~bindException(){
}

}// exception
}// async_net
}// Hemsleya

