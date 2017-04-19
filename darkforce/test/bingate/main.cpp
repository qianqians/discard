/*
* gate.cpp
*
*  Created on: 2015-9-10
*      Author: qianqians
*/
#include "../../framework/gate/gate.h"

int main(){
	Fossilizid::gate::gate _gate("test.config", "gate");

	_gate.run();

	return 0;
}