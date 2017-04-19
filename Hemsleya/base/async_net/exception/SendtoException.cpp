/*
 * SendtoException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net SendtoException
 */
#include "SendtoException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

SendtoException::SendtoException(const char * info, int errcode) : exception(info, errcode){
}

SendtoException::~SendtoException(){
}



}// exception
}// async_net
}// Hemsleya

