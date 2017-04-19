/*
 *  optimisticquetest.cpp
 *  Created on: 2013-8-17
 *	    Author: qianqians
 *  optimisticque»ù×¼²âÊÔ
 */
#include <Hemsleya\base\concurrent\container\optimisticque.h>
#include <Hemsleya\test\concurrent\container\testbase\base_test.h>

int main(){
	base_test<Hemsleya::container::optimisticque<test> > test;
	test(10);

	return 1;
}