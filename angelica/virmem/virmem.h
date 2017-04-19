/*
 * virmem.h
 * Created on: 2012-10-24
 *	   Author: qianqians
 * �����ڴ� 
 */
#ifndef _VIRMEM_H
#define _VIRMEM_H

#ifdef _WIN32
#include <Windows.h>
#endif

#include <angelica/detail/tools.h>

struct vmhandle {
	unsigned int cache_count;
	unsigned int cache_clock;
	void * mem;
	unsigned int size;

#ifdef _WIN32
	HANDLE hFileMapping;
#endif
	unsigned int startaddr;
	
	unsigned int offset;
};

#ifdef __cplusplus
extern "C"{
#endif //__cplusplus

//��ʼ��
void vminit();

//����һ�������ڴ�
struct vmhandle * vmalloc(unsigned int size);

//���������ڴ� ����ͨ���˺������� ������׼ȷ��¼������Ϣ
void * mem_access(struct vmhandle * handle);

//����ҳ
bool swap_in(struct vmhandle * handle);
//����ҳ
bool swap_out(struct vmhandle * handle);

//�ͷ������ڴ�
void vfree(struct vmhandle * handle);

//����
void vmdestructor();

#ifdef __cplusplus
}
#endif //__cplusplus

#endif //_VIRMEM_H