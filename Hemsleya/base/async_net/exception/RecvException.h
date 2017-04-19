/*
 * RecvException.h
 *
 *  Created on: May 22, 2013
 *      Author: qianqians
 * Hemsleya async_net RecvException
 */
#ifndef RECVEXCEPTION_H_
#define RECVEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class RecvException : public Hemsleya::exception::exception{
public:
	RecvException(const char * info, int errcode);

	virtual ~RecvException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* RECVEXCEPTION_H_ */
