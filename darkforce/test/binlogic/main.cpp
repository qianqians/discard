/*
* logic.cpp
*
*  Created on: 2015-9-10
*      Author: qianqians
*/
#include "../../framework/logic/logic.h"

int main(){
	Fossilizid::logic::logic _logic("test.config", "logic");

	_logic.run();

	return 0;
}