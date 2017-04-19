/*
 * RecvException.h
 *
 *  Created on: May 22, 2013
 *      Author: qianqians
 * Hemsleya async_net RecvException
 */
#include "RecvException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

RecvException::RecvException(const char * info, int errcode) : exception(info, errcode){
}

RecvException::~RecvException(){
}

}// exception
}// async_net
}// Hemsleya

