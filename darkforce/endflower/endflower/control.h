/*
*  qianqians
*  2015-11-3
*/
#ifndef _control_h_
#define _control_h_

#include <boost/array.hpp>

#include "texture.h"

class control {
protected:
	int x;
	int y;
	int width;
	int height;

	texture * _texture;

	boost::array<boost::array<float, 3>,  4> transformation();

};

#endif //_control_h_