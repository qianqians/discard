/*
 * fitle.h
 *
 *  Created on: 2015-3-25
 *      Author: qianqians
 */
#ifndef _fitle_h
#define _fitle_h

#include <boost/shared_ptr.hpp>

namespace Fossilizid{
namespace fitle{

template<class T>
class fitle{
public:
	fitle(){
	}
	
	~fitle(){
	}

	void setfitle(std::function<bool(T ) > _cmp){
		cmp = _cmp;
	}

	bool isfitle(T v){
		return cmp(v);
	}

private:
	std::function<bool(T ) > cmp;

};

} /* namespace acceptor */
} /* namespace Fossilizid */

#endif //_fitle_h
