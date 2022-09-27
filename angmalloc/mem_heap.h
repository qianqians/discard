/*
 * mem_heap.h
 *  Created on: 2021-5-22
 *      Author: qianqians
 * mem_heap 
 */
#ifndef _MEM_HEAP_H
#define _MEM_HEAP_H

#ifndef __cplusplus
extern "C"{
#endif //__cplusplus

void * _mempage_heap_alloc(size_t size);
void * _mempage_heap_realloc(void * mem, size_t size);
void _free(void * mem);

struct chunk;
void _recover_chunk(struct chunk * _chunk);

extern const size_t default_chunk_size;

#ifndef __cplusplus
}
#endif __cplusplus

#endif //_MEM_HEAP_H