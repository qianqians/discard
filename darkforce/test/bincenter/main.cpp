/*
 * center.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "../../framework/center/center.h"

int main(){
	Fossilizid::center::center _center("test.config", "center");

	_center.run();

	return 0;
}