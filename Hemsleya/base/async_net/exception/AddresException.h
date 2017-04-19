/*
 * AddresException.h
 *
 *  Created on: 2013-10-4
 *      Author: qianqians
 * Hemsleya async_net AddresException
 */
#ifndef ADDRESEXCEPTION_H_
#define ADDRESEXCEPTION_H_

#include <Hemsleya/base/exception/exception.h>

namespace Hemsleya{
namespace async_net{
namespace exception{

class AddresException : public Hemsleya::exception::exception{
public:
	AddresException(const char * info, int err);

	virtual ~AddresException();

};

}// exception
}// async_net
}// Hemsleya

#endif /* ADDRESEXCEPTION_H_ */
