/*
 * InitException.cpp
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net InitException
 */
#include "InitException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

InitException::InitException(const char * info, int errcode) : exception(info, errcode){
}

InitException::~InitException(){
}

}// exception
}// async_net
}// Hemsleya

