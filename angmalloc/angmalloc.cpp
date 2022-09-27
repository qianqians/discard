/*
 * angmalloc.cpp
 *  Created on: 2021-5-22
 *      Author: qianqians
 * angmalloc 
 */
#include <string.h>
#include "mem_heap.h"

void * angmalloc(size_t size){
	return _mempage_heap_alloc(size);
}

void * angcalloc(size_t count, size_t size){
	size_t _size = count * size;
	void * ret = angmalloc(_size);
	memset(ret, 0, _size);
	return ret;
}

void * angrealloc(void * mem, size_t size){
	return _mempage_heap_realloc(mem, size);
}

void angfree(void * mem){
	_free(mem);
}
