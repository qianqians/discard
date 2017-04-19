/*
 * fsm.cpp
 *
 *  Created on: 2013-10-26
 *      Author: qianqians
 * Hemsleya fsm
 */
#include "fsm.h"

namespace Hemsleya{
namespace fsm{

fsm::fsm(){
}

fsm::~fsm(){
}

bool fsm::setstate(int state){
	bool bblocked = false;
	
	for (auto var : mapstatebeblocked[state]){
		if (std::find(vectorstate.begin(), vectorstate.end(), var) != vectorstate.end()){
			bblocked = true;
			break;
		}
	}

	if (bblocked){
		return false;
	}

	for (auto var : mapstatebreak[state]){
		std::vector<int>::iterator find = std::find(vectorstate.begin(), vectorstate.end(), var);
		if (find != vectorstate.end()){
			vectorstate.erase(find);
		}
	}

	return true;
}

bool fsm::checkstate(int state){
	std::vector<int>::iterator find = std::find(vectorstate.begin(), vectorstate.end(), state);
	if (find == vectorstate.end()){
		return false;
	}
	return true;
}

void fsm::addstatebreak(int state, int target){
	mapstatebreak[state].push_back(target);
}

void fsm::addstateblock(int state, int target){
	mapstatebeblocked[target].push_back(state);
}

}// fsm
}// Hemsleya

