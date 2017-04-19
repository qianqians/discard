/*
 * c_list.c
 * Created on: 2012-11-5
 *     Author: qianqians
 * c_list
 */
#include "c_list.h"

//��_s���뵽_list��_d���֮ǰ, if _d == null insert to list end
bool list_insert(list_head * _list, struct list_node *_d, struct list_node *_s){
	struct list_node *_d1 = (struct list_node*)_list;
	for( ; _d1->_next != _d; _d1 = _d1->_next);

	if(_d1->_next != _d)
		return false;

	_d1->_next = _s;
	_s->_next = _d;

	return true;
}

//��_list�е�_d���ɾ��
bool list_erase(list_head * _list, struct list_node *_d){
	struct list_node *_d1 = (struct list_node*)_list;
	for( ; _d1 != _d; _d1 = _d1->_next);
	
	if(_d1 == 0 || _d1->_next != _d)
		return false;

	_d1->_next = _d->_next;

	return true;
}

//�ж�_list�Ƿ�Ϊ��
bool list_empty(list_head * _list){
	if (((struct list_node*)_list)->_next == 0)
		return true;

	return false;
}
