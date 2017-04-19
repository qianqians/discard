/*
 * AddresException.cpp
 *
 *  Created on: 2013-10-4
 *      Author: qianqians
 * Hemsleya async_net AddresException
 */
#include "AddresException.h"

namespace Hemsleya{
namespace async_net{
namespace exception{

AddresException::AddresException(const char * info, int errcode) : exception(info, errcode){
}

AddresException::~AddresException(){
}

}// exception
}// async_net
}// Hemsleya

