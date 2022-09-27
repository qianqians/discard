/*
 * chunk.cpp
 *  Created on: 2021-5-22
 *      Author: qianqians
 * chunk 
 */
#ifdef _WIN32
#include <Windows.h>
#endif
#include <mutex>
#include <atomic>
#include "chunk.h"
#include "mem_heap.h"

enum EM_CHUNK_FLAG {
	EM_ACTIVE,
	EM_WAIT_RECV,
	EM_FREE,
};

struct chunk {
	std::mutex _mu;
	std::atomic_int _count;
	std::atomic_int flag;
	size_t size;
	size_t slide;
}; 
const size_t CHUNK_SIZE = sizeof(struct chunk);

static size_t big_default_chunk_size = 16 * 1024 * 1024;
struct chunk* _create_chunk(size_t size) {
	size = (size + CHUNK_SIZE + default_chunk_size - 1) / default_chunk_size * default_chunk_size;
	if (size > default_chunk_size && size < big_default_chunk_size) {
		size = size;
	}
#ifdef _WIN32
	struct chunk* _chunk = (struct chunk*)VirtualAlloc(0, size, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
#endif //_WIN32
	if (_chunk != 0) {
		::new (_chunk) chunk();
		_chunk->_count.store(0);
		_chunk->flag.store(EM_ACTIVE);
		_chunk->size = size;
		_chunk->slide = sizeof(struct chunk);
	}
	return _chunk;
}

void _active_chunk(struct chunk* _chunk) {
	_chunk->flag.store(EM_ACTIVE);
}

void _release_chunk(struct chunk* _chunk) {
	_chunk->flag.store(EM_WAIT_RECV);
	if (_chunk->_count.load() == 0) {
		int flag = EM_WAIT_RECV;
		if (_chunk->flag.compare_exchange_strong(flag, EM_FREE)) {
			_chunk->slide = sizeof(struct chunk);
			_recover_chunk(_chunk);
		}
	}
}

size_t _chunk_total_size(struct chunk* _chunk) {
	return _chunk->size;
}

size_t chunk_size(struct chunk* _chunk) {
	return _chunk->size - _chunk->slide;
}

int _chunk_count(struct chunk* _chunk) {
	return _chunk->_count.load();
}

size_t _chunk_slide(struct chunk* _chunk) {
	return _chunk->slide;
}

size_t _malloc_size(size_t size) {
	size_t new_size = sizeof(struct chunk*) + sizeof(size_t) + size;
	new_size = (new_size + 7) / 8 * 8;
	return new_size;
}

void* _malloc(struct chunk* _chunk, size_t size) {
	std::lock_guard<std::mutex> l(_chunk->_mu);

	size_t new_size = _malloc_size(size);
	if (chunk_size(_chunk) < new_size) {
		return nullptr;
	}

	_chunk->_count++;
	struct chunk** tmp = (struct chunk**)((char*)_chunk + _chunk->slide);
	_chunk->slide += new_size;
	*tmp = _chunk;
	size_t* _size = (size_t*)++tmp;
	*_size = new_size;
	void * ret = (void*)++_size;

	return ret;
}

void _chunk_free(void* mem) {
	struct chunk* _chunk = *(struct chunk**)((char*)mem - sizeof(size_t) - sizeof(struct chunk*));
	if (--_chunk->_count == 0) {
		int flag = EM_WAIT_RECV;
		if (_chunk->flag.compare_exchange_strong(flag, EM_FREE)) {
			_chunk->slide = sizeof(struct chunk);
			_recover_chunk(_chunk);
		}
		else {
			std::lock_guard<std::mutex> l(_chunk->_mu);
			if (_chunk->_count.load() == 0) {
				_chunk->slide = sizeof(struct chunk);
			}
		}
	}
}

size_t _size_of(void* mem) {
	return *(size_t*)((char*)mem - sizeof(size_t));
}