/*
 * swapquebasetest.cpp
 *  Created on: 2013-8-17
 *	    Author: qianqians
 * swapquebasetest»ù×¼²âÊÔ
 */
#include <Hemsleya\base\concurrent\container\swapque.h>
#include <Hemsleya\test\concurrent\container\testbase\base_test.h>

int main(){
	base_test<Hemsleya::container::swapque<test> > test;
	test(100);

	return 1;
}