/*
 * array.h
 *  Created on: 2021-6-15
 *      Author: qianqians
 * array 
 */
#ifndef _ARRAY_H
#define _ARRAY_H

template<class T>
struct array {
public:
	T* array_data;
	size_t size;
	size_t index;

public:
	virtual ~array() {
		for (size_t i = 0; i < index; i++) {
			array_data[i].~T();
		}

		VirtualFree(array_data, 0, MEM_RELEASE);
	}

	void init_array(size_t _size) {
		size = _size;
		index = 0;
		array_data = (T*)VirtualAlloc(0, size * sizeof(T), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
	}

	bool push_back(const T& data) {
		if (index == size) {
			return false;
		}

		array_data[index++] = data;
		return true;
	}
	bool pop_back(T& data) {
		if (index <= 0) {
			return false;
		}

		data = array_data[--index];
		return true;
	}
};

#endif //_ARRAY_H