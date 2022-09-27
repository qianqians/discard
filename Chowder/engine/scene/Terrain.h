/*
 * exception.h
 * Author: qianqians
 * 2022/5/13
 */
#ifndef _EXCEPTION_H_
#define _EXCEPTION_H_

#include <exception>
#include <string>

namespace Chowder {

class InitDiligentException : std::exception{
public:
	InitDiligentException(Diligent::RENDER_DEVICE_TYPE device_type) : std::exception("InitDiligentException"), strMsg("InitDiligentException"), deviceType(device_type) {

	}

public:
	std::string strMsg;
	Diligent::RENDER_DEVICE_TYPE deviceType;

};

}

#endif // !_EXCEPTION_H_
