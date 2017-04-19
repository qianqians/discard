/*
 * InitException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net InitException
 */
#ifndef INITEXCEPTION_H_
#define INITEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class InitException : public Hemsleya::exception::exception{
public:
	InitException(const char * info, int err);

	virtual ~InitException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* INITEXCEPTION_H_ */
