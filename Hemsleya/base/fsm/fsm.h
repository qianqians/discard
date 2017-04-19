/*
 * fsm.h
 *
 *  Created on: 2013-10-4
 *      Author: qianqians
 * Hemsleya fsm
 */
#ifndef FSM_H_
#define FSM_H_

#include <map>
#include <vector>

namespace Hemsleya{
namespace fsm{

class fsm{
public:
	fsm();
	~fsm();

	bool checkstate(int state);

	bool setstate(int state);

	void addstatebreak(int state, int target);

	void addstateblock(int state, int target);

protected:
	std::map<int, std::vector<int> > mapstatebreak;
	std::map<int, std::vector<int> > mapstatebeblocked;
	std::vector<int> vectorstate;
};

}// fsm
}// Hemsleya

#endif /* FSM_H_ */
