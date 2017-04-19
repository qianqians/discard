/*
 * wait_any_test.h
 *  Created on: 2013-8-17
 *	    Author: qianqians
 * wait_any_test»ù×¼²âÊÔ
 */
#include <Hemsleya/base/concurrent/scheduling/wait_any.h>

struct test{
	int data;
};

Hemsleya::scheduling::template_version::signal<test> signal;


void dowait(){
	test test;
	test.data = 1001;
	signal.post(test);
}

int main(){

	test t;
	
	signal.wait_any(t, dowait);

	return 1;
}