/*
 * RecvfromException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net RecvfromException
 */
#include "RecvfromException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

RecvfromException::RecvfromException(const char * info, int errcode) : exception(info, errcode){
}

RecvfromException::~RecvfromException(){
}

}// exception
}// async_net
}// Hemsleya
