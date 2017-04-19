/*
 *  qianqians
 *  2014-11-28
 */
#ifndef _context_h_
#define _context_h_

#include "glextARB.h"

#include <list>
#include <boost/shared_ptr.hpp>

#include "render.h"
#include "camera .h"

class wnd;

class context{
public:
	context(wnd * wnd);
	~context();

	void addrender(boost::shared_ptr<render> _render);

	bool swapbuff();

	void lookat(float postionx, float postiony, float postionz, float upx, float upy, float upz, float lookatx, float lookaty, float lookatz);

	void make_current();

	void release();

	camera * get_camera();

	//GLuint get_program();

private:
	HDC hdc;
	HGLRC rc;

	//GLuint __program;
	std::list<boost::shared_ptr<render> > drawlist;

	camera _camera;

	friend void draw();


};

#endif