/*
 * SendException.h
 *
 *  Created on: 2013-10-3
 *      Author: qianqians
 * Hemsleya async_net SendException
 */
#ifndef SENDEXCEPTION_H_
#define SENDEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class SendException : public Hemsleya::exception::exception {
public:
	SendException(const char * info, int errcode);

	virtual ~SendException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* SENDEXCEPTION_H_ */
