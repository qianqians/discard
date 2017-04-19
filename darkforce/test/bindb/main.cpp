/*
* db.cpp
*
*  Created on: 2015-9-10
*      Author: qianqians
*/
#include "../../framework/dbproxy/dbproxy.h"

int main(){
	Fossilizid::dbproxy::dbproxy _dbproxy("test.config", "dbproxy");

	_dbproxy.run();

	return 0;
}