/*
 * AcceptException.cpp
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net AcceptException
 */
#include "AcceptException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

AcceptException::AcceptException(const char * info, int errcode) : exception(info, errcode){
}

AcceptException::~AcceptException(){
}

}// exception
}// async_net
}// Hemsleya

