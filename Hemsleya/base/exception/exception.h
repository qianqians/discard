/*
 * exception.h
 *
 *  Created on: May 22, 2013
 *      Author: qianqians
 * Hemsleya exception
 */
#ifndef EXCEPTION_H_
#define EXCEPTION_H_

#include <stdexcept>
#include <string>
#include <sstream>

namespace Hemsleya{
namespace exception{

class exception : public std::exception{
public:
	exception(const char * info);

	exception(const std::string & info);

	exception(const char * info, int errcode);

	exception(const std::string & info, int errcode);

	virtual ~exception();

	virtual const char *what( ) const;

private:
	exception();

protected:
	std::string err;

};

}// exception
}// Hemsleya

#endif /* EXCEPTION_H_ */
