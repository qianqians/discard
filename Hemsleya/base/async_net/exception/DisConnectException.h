/*
 * DisConnectException.h
 *
 *  Created on: May 22, 2013
 *      Author: qianqians
 * Hemsleya async_net DisConnectException
 */
#ifndef DISCONNECTEXCEPTION_H_
#define DISCONNECTEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class DisConnectException : public Hemsleya::exception::exception{
public:
	DisConnectException(const char * info);
	DisConnectException(const char * info, int errcode);

	virtual ~DisConnectException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* DisConnectException */
