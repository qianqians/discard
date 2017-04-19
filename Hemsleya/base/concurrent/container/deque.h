/*
 * deque.h
 *  Created on: 2013-1-16
 *	    Author: qianqians
 * deque: a lock free deque
 */
#ifndef _DEQUE_H
#define _DEQUE_H

#include <boost/atomic.hpp>

#include <Hemsleya/base/concurrent/container/detail/_hazard_ptr.h>
#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya{
namespace container{

template <typename T, typename _Allocator = std::allocator<T> >
class deque{
private:
	struct _que_node{
		_que_node() : _next(0), _next_back(0), _prev(0), _prev_back(0){}
		_que_node(const T & val) : data(val), _next(0), _next_back(0), _prev(0), _prev_back(0){}
		~_que_node () {}

		T data;
		_que_node * _next;
		boost::atomic<_que_node *> _next_back;
		_que_node * _prev;
		boost::atomic<_que_node *> _prev_back;

	};

	struct _list{
		boost::atomic<_que_node *> _front, _detail;
		boost::atomic_uint64_t size;
	}

	boost::atomic<_list *> _list;

	typedef Hemsleya::container::detail::_hazard_ptr<_que_node> _hazard_node_ptr;
	typedef Hemsleya::container::detail::_hazard_system<_que_node> _hazard_node_system;
	typedef Hemsleya::container::detail::_hazard_ptr<_list> _hazard_list_ptr;
	typedef Hemsleya::container::detail::_hazard_system<_list> _hazard_list_system;
	typedef typename _Allocator::template rebind<_que_node>::other _node_alloc;
	typedef typename _Allocator::template rebind<_list>::other _list_alloc;

	_hazard_node_system _node_system;


public:
	deque();
	~deque();


private:

};	
	
} //container
} //Hemsleya

#endif //_DEQUE_H