/*
 * buffstruct.h
 *
 *  Created on: May 22, 2013
 *      Author: qianqians
 * Hemsleya async_net buffstruct
 */
#ifndef BUFFSTRUCT_H_
#define BUFFSTRUCT_H_

#include <boost/cstdint.hpp>

namespace Hemsleya{
namespace async_net{

struct buffstruct{
	char * buff;
	uint32_t len;
};

}// async_net
}// Hemsleya

#endif /* BUFFSTRUCT_H_ */
