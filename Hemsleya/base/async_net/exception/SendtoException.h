/*
 * SendtoException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net SendtoException.h
 */
#ifndef SENDTOEXCEPTION_H_
#define SENDTOEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class SendtoException : public Hemsleya::exception::exception{
public:
	SendtoException(const char * info, int errcode);

	virtual ~SendtoException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* EXCEPTION_H_ */
