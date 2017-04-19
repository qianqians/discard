/*
 * msquebasetest.cpp
 *  Created on: 2013-8-17
 *	    Author: qianqians
 * msque»ù×¼²âÊÔ
 */
#include <Hemsleya\base\concurrent\container\msque.h>
#include <Hemsleya\test\concurrent\container\testbase\base_test.h>

int main(){
	base_test<Hemsleya::container::msque<test> > test;
	test(100);

	return 1;
}