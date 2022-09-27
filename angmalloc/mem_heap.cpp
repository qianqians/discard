/*
 * mem_heap.cpp
 *  Created on: 2021-5-21
 *      Author: qianqians
 * mem_heap 
 */
#ifdef _WIN32
#include <Windows.h>
#endif
#include <mutex>
#include "mem_heap.h"
#include "chunk.h"
#include "array.h"
#include "skiplist.h"

const size_t default_chunk_size = 64 * 1024;
static const size_t free_chunk_list_size = 16 * 1024;

struct mirco_heap {
	std::mutex _mu;
	struct chunk* _chunk = nullptr;
	struct chunk* _big_chunk = nullptr;

	virtual ~mirco_heap() {
		VirtualFree(_chunk, 0, MEM_RELEASE);
		VirtualFree(_big_chunk, 0, MEM_RELEASE);
	}
};

class heap {
public:
	array<mirco_heap> active_chunk;

	std::mutex _mu;
	array<struct chunk* > free_chunk;

	std::mutex _heap_mu;
	skiplist<struct chunk* > free_chunk_heap;

public:
	heap() {
		size_t concurrent_count = 8;
#ifdef _WIN32
		SYSTEM_INFO info;
		GetSystemInfo(&info);
		concurrent_count = info.dwNumberOfProcessors;
#endif //_WIN32

		active_chunk.init_array(concurrent_count);
		for (auto i = 0; i < concurrent_count; i++) {
			new(&active_chunk.array_data[active_chunk.index++]) mirco_heap();
		}

		free_chunk.init_array(free_chunk_list_size);
		free_chunk_heap.init_skiplist();
	}

	virtual ~heap() {
#ifdef _WIN32
		for (auto i = 0; i < free_chunk.index; i++) {
			VirtualFree(free_chunk.array_data[i], 0, MEM_RELEASE);
		}
		for (auto i = 0; i < free_chunk_heap.node_array.index; i++) {
			VirtualFree(free_chunk_heap.node_array.array_data[i]->data, 0, MEM_RELEASE);
		}
#endif //_WIN32
	}
};

static heap _heap;

void* _mempage_heap_alloc(size_t size) {
	void* ret = nullptr;
	for (auto index = 0; index < _heap.active_chunk.index; index++) {
		auto _mirco_heap = &_heap.active_chunk.array_data[index];
		if (!_mirco_heap->_mu.try_lock()) {
			continue;
		}

		size_t m_size = _malloc_size(size);
		if (m_size <= (default_chunk_size - CHUNK_SIZE)) {
			if (_mirco_heap->_chunk == nullptr) {
				_mirco_heap->_chunk = _create_chunk(default_chunk_size);
			}

			ret = _malloc(_mirco_heap->_chunk, size);
			if (ret == nullptr) {
				_release_chunk(_mirco_heap->_chunk);
				do {
					{
						std::lock_guard<std::mutex> l(_heap._mu);
						if (_heap.free_chunk.pop_back(_mirco_heap->_chunk)) {
							_active_chunk(_mirco_heap->_chunk);
							break;
						};
					}
					_mirco_heap->_chunk = _create_chunk(default_chunk_size);
				} while (false);
				ret = _malloc(_mirco_heap->_chunk, size);
			}
		}
		else {
			if (_mirco_heap->_big_chunk == nullptr) {
				_mirco_heap->_big_chunk = _create_chunk(m_size);
			}

			ret = _malloc(_mirco_heap->_big_chunk, size);
			if (ret == nullptr) {
				_release_chunk(_mirco_heap->_big_chunk);
				do {
					{
						std::lock_guard<std::mutex> l(_heap._heap_mu);
						if (_heap.free_chunk_heap.try_get_and_remove((uint32_t)(m_size + CHUNK_SIZE), _mirco_heap->_big_chunk)) {
							_active_chunk(_mirco_heap->_big_chunk);
							break;
						}
					}
					_mirco_heap->_big_chunk = _create_chunk(m_size);
				} while (false);
				ret = _malloc(_mirco_heap->_big_chunk, size);
			}
		}
		_mirco_heap->_mu.unlock();
		break;
	}
	return ret;
}

void* _mempage_heap_realloc(void* mem, size_t size) {
	size_t old_size = _size_of(mem) - sizeof(struct chunk*) - sizeof(size_t);
	if (old_size >= size) {
		return mem;
	}

	void * ret = _mempage_heap_alloc(size);
	memcpy(ret, mem, old_size);
	_free(mem);

	return ret;
}

void _free(void* mem) {
	_chunk_free(mem);
}

void _recover_chunk(struct chunk* _chunk) {
	if (_chunk_total_size(_chunk) > default_chunk_size) {
		std::lock_guard<std::mutex> l(_heap._heap_mu);
		if (!_heap.free_chunk_heap.insert((uint32_t)_chunk_total_size(_chunk), _chunk)) {
#ifdef _WIN32
			VirtualFree(_chunk, 0, MEM_RELEASE);
#endif
		}
		return;
	}

	std::lock_guard<std::mutex> l(_heap._mu);
	if (!_heap.free_chunk.push_back(_chunk)) {
#ifdef _WIN32
		VirtualFree(_chunk, 0, MEM_RELEASE);
#endif
	}
}