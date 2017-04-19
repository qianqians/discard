/*
 * RecvfromException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net RecvfromException
 */
#ifndef RECVFROMEXCEPTION_H_
#define RECVFROMEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class RecvfromException : public Hemsleya::exception::exception{
public:
	RecvfromException(const char * info, int errcode);
	
	virtual ~RecvfromException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* RECVFROMEXCEPTION_H_ */
