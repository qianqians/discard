/*
 * bindException.h
 *
 *  Created on: 2013-10-6
 *      Author: qianqians
 * Hemsleya async_net bindException
 */
#ifndef BINDEXCEPTION_H_
#define BINDEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class bindException : public Hemsleya::exception::exception{
public:
	bindException(const char * info, int err);

	virtual ~bindException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* BINDEXCEPTION_H_ */
