/*
*  qianqians
*  2015-11-3
*/
#ifndef _camera_h_
#define _camera_h_

#include <boost/array.hpp>
#include <boost/shared_ptr.hpp>

class camera {
public:
	boost::array<float, 3> postion;
	boost::array<float, 3> up;
	boost::array<float, 3> lookat;
};

#endif //_camera_h_
