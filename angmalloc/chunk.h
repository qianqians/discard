/*
 * chunk.h
 *  Created on: 2021-5-22
 *      Author: qianqians
 * chunk 
 */
#ifndef _CHUNK_H
#define _CHUNK_H

struct chunk;
extern const size_t CHUNK_SIZE;

struct chunk* _create_chunk(size_t size);
void _active_chunk(struct chunk* _chunk);
void _release_chunk(struct chunk* _chunk);
size_t _chunk_total_size(struct chunk* _chunk);
int _chunk_count(struct chunk* _chunk);
size_t _chunk_slide(struct chunk* _chunk);

void* _malloc(struct chunk* _chunk, size_t size);
void _chunk_free(void* mem);
size_t _malloc_size(size_t size);

size_t _size_of(void * mem);


#endif //_CHUNK_H