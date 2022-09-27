/*
 * skiplist.h
 *  Created on: 2021-7-17
 *      Author: qianqians
 * skiplist 
 */
#ifndef _SKIPLIST_H
#define _SKIPLIST_H

#include <cstdint>
#include <random>

#include "array.h"

#define node_num 1024

static std::default_random_engine e;

template<class T>
struct skiplist {
private:
	struct node {
		uint32_t key;
		T data;
	};
	node pre_alloc_data_node[node_num];

	struct skip_node {
		struct node* data_node;
		struct skip_node* pre;
		struct skip_node* next;
		struct skip_node* low_layer;
		struct skip_node* up_layer;
	};
	skip_node pre_alloc_skip_node[node_num * 2];
	array<skip_node*> skip_node_array;

	struct skip_node* begin, * end;
public:
	array<node*> node_array;

private:
	bool rand_up() {
		return (e() % 2) == 0;
	}

public:
	void init_skiplist() {
		node_array.init_array(node_num);
		for (int i = 0; i < node_num; i++) {
			node_array.push_back(&(pre_alloc_data_node[i]));
		}
		skip_node_array.init_array(node_num * 2);
		for (int i = 0; i < node_num * 2; i++) {
			skip_node_array.push_back(&pre_alloc_skip_node[i]);
		}

		node* min = nullptr, * max = nullptr;
		node_array.pop_back(min);
		node_array.pop_back(max);
		min->key = 0;
		max->key = UINT_MAX;
		skip_node* b = nullptr, * e = nullptr;
		for (int i = 0; i < 5; i++) {
			skip_node* _b = nullptr, * _e = nullptr;
			skip_node_array.pop_back(_b);
			skip_node_array.pop_back(_e);

			_b->data_node = min;
			_e->data_node = max;

			_b->pre = nullptr;
			_e->next = nullptr;
			_b->next = _e;
			_e->pre = _b;
			_b->low_layer = b;
			_e->low_layer = e;

			if (b != nullptr && e != nullptr) {
				b->up_layer = _b;
				e->up_layer = _e;
			}

			b = _b;
			e = _e;
		}
		b->up_layer = nullptr;
		e->up_layer = nullptr;
		begin = b;
		end = e;
	}

	bool insert(uint32_t key, T& data);
	bool try_get_and_remove(uint32_t key, T& data);
};

template<class T>
bool skiplist<T>::insert(uint32_t key, T& data) {
	node* pnode = nullptr;
	if (!node_array.pop_back(pnode)) {
		return false;
	}
	pnode->key = key;
	pnode->data = data;

	skip_node* pskipnode = nullptr;
	if (!skip_node_array.pop_back(pskipnode)) {
		node_array.push_back(pnode);
		return false;
	}
	pskipnode->data_node = pnode;
	pskipnode->next = nullptr;
	pskipnode->low_layer = nullptr;
	
	skip_node* i_0, * e_0, * i_1, * e_1, * i_2, * e_2, * i_3, * e_3, * i_4, * e_4;
	skip_node* _begin = begin, * _end = end;
	int round = 0;
	do {
		auto _node = _begin;
		for (; ; _node = _node->next) {
			if (_node->next == _end) {
				break;
			}
			if (_node->next->data_node->key > key) {
				break;
			}
		}

		if (round == 0) {
			i_0 = _node;
			e_0 = _node->next;
		}
		else if (round == 1) {
			i_1 = _node;
			e_1 = _node->next;
		}
		else if (round == 2) {
			i_2 = _node;
			e_2 = _node->next;
		}
		else if (round == 3) {
			i_3 = _node;
			e_3 = _node->next;
		}
		else if (round == 4) {
			i_4 = _node;
			e_4 = _node->next;
		}

		_begin = _node->low_layer;
		_end = _node->next->low_layer;
		round++;
	} while (_begin != nullptr);

	skip_node* low_layer = nullptr;
	{
		i_4->next = pskipnode;
		pskipnode->pre = i_4;
		pskipnode->next = e_4;
		e_4->pre = pskipnode;
		pskipnode->low_layer = low_layer;
		low_layer = pskipnode;
		low_layer->up_layer = nullptr;
	}
	if (rand_up()) {
		if (!skip_node_array.pop_back(pskipnode)) {
			return true;
		}
		pskipnode->data_node = pnode;
		i_3->next = pskipnode;
		pskipnode->pre = i_3;
		pskipnode->next = e_3;
		e_3->pre = pskipnode;
		pskipnode->low_layer = low_layer;
		low_layer->up_layer = pskipnode;
		low_layer = pskipnode;
		low_layer->up_layer = nullptr;
	}
	else {
		return true;
	}
	if (rand_up()) {
		if (!skip_node_array.pop_back(pskipnode)) {
			return true;
		}
		pskipnode->data_node = pnode;
		i_2->next = pskipnode;
		pskipnode->pre = i_2;
		pskipnode->next = e_2;
		e_2->pre = pskipnode;
		pskipnode->low_layer = low_layer;
		low_layer->up_layer = pskipnode;
		low_layer = pskipnode;
		low_layer->up_layer = nullptr;
	}
	else {
		return true;
	}
	if (rand_up()) {
		if (!skip_node_array.pop_back(pskipnode)) {
			return true;
		}
		pskipnode->data_node = pnode;
		i_1->next = pskipnode;
		pskipnode->pre = i_1;
		pskipnode->next = e_1;
		e_1->pre = pskipnode;
		pskipnode->low_layer = low_layer;
		low_layer->up_layer = pskipnode;
		low_layer = pskipnode;
		low_layer->up_layer = nullptr;
	}
	else {
		return true;
	}
	if (rand_up()) {
		if (!skip_node_array.pop_back(pskipnode)) {
			return true;
		}
		pskipnode->data_node = pnode;
		i_0->next = pskipnode;
		pskipnode->pre = i_0;
		pskipnode->next = e_0;
		e_0->pre = pskipnode;
		pskipnode->low_layer = low_layer;
		low_layer->up_layer = pskipnode;
		low_layer = pskipnode;
		low_layer->up_layer = nullptr;
	}

	return true;
}

template<class T>
bool skiplist<T>::try_get_and_remove(uint32_t key, T& data) {
	auto _node = begin;
	auto _hit = end;
	auto _end = end;
	do {
		for (; ; _node = _node->next) {
			if (_node->next == _hit) {
				break;
			}
			if (_node->next->data_node->key > key) {
				break;
			}
		}

		if (_node->low_layer == nullptr) {
			break;
		}

		_node = _node->low_layer;
		_hit = _node->next->low_layer;
		_end = _end->low_layer;

	} while (true);

	if (_node->next == _end) {
		return false;
	}

	if (_node->next->data_node->key < key) {
		return false;
	}

	data = _node->next->data_node->data;

	auto _release = _node->next;
	while (_release != nullptr) {
		_node = _release->pre;
		_node->next = _release->next;
		_release->next->pre = _node;
		auto _tmp = _release;
		_release = _release->up_layer;
		skip_node_array.push_back(_tmp);
	}

	return true;
}

#endif //_SKIPLIST_H