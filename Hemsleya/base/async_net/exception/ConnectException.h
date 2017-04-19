/*
 * ConnectException.h
 *
 *  Created on: May 22, 2013
 *      Author: qianqians
 * Hemsleya async_net ConnectException
 */
#ifndef CONNECTEXCEPTION_H_
#define CONNECTEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class ConnectException : public Hemsleya::exception::exception{
public:
	ConnectException(const char * info);
	ConnectException(const char * info, int errcode);

	virtual ~ConnectException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* CONNECTEXCEPTION_H_ */
