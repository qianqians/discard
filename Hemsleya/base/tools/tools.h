/*
 * tools.h
 * Created on: 2012-10-16
 *     Author: qianqians
 * tools
 */
#ifndef _TOOLS_H
#define _TOOLS_H

#ifndef __cplusplus
#define bool char 
#define true 1
#define false 0
#endif

#ifndef container_of
// copy from include/linux/kernel.h
#define container_of(ptr, type, member) (type *)( (char *)ptr - offsetof(type, member) )
#endif //container_of

void _trace(char * format, ...);

#include <boost/cstdint.hpp>

uint32_t processors_count();


#endif //_tools_h