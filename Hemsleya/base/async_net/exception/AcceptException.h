/*
 * AcceptException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net AcceptException
 */
#ifndef ACCEPTEXCEPTION_H_
#define ACCEPTEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class AcceptException : public Hemsleya::exception::exception{
public:
	AcceptException(const char * info, int err);

	virtual ~AcceptException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* ACCEPTEXCEPTION_H_ */
