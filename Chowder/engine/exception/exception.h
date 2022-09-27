/*
 * exception.h
 * Author: qianqians
 * 2022/5/13
 */
#ifndef _EXCEPTION_H_
#define _EXCEPTION_H_

#include <gfx.h>

#include <exception>
#include <string>

namespace Chowder {

class RawWindowHandleException : std::exception {
public:
	RawWindowHandleException(std::string err) : std::exception(("RawWindowHandleException:" + err).c_str()), strMsg(err) {
	}

public:
	std::string strMsg;

};

}

#endif // !_EXCEPTION_H_
