/*
 * tools.h
 * Created on: 2012-10-16
 *     Author: qianqians
 * c_list
 */
#ifndef _C_LIST_H
#define _C_LIST_H

#include <angelica/detail/tools.h>

typedef struct list_node{
	struct list_node * _next;
}list_head;

#ifdef __cplusplus
extern "C"{
#endif //__cplusplus

//��_s���뵽_list��_d���֮ǰ, if _d == null insert to list end
bool list_insert(list_head * _list, struct list_node *_d, struct list_node *_s);

//��_list�е�_d���ɾ��
bool list_erase(list_head * _list, struct list_node *_d);

//�ж�_list�Ƿ�Ϊ��
bool list_empty(list_head * _list);

#ifdef __cplusplus
}
#endif //__cplusplus

#endif //_C_LIST_H