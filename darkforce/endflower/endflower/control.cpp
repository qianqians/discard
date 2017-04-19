/*
*  qianqians
*  2015-11-3
*/
#include "control.h"
#include "camera .h"
#include "wnd.h"

boost::array<boost::array<float, 3>, 4> control::transformation() {
	boost::array<boost::array<float, 3>, 4> coordinate;

	GLdouble projectmatrix[16], modelviewmatrix[16];
	glGetDoublev(GL_PROJECTION_MATRIX, projectmatrix);
	glGetDoublev(GL_MODELVIEW_MATRIX, modelviewmatrix);
	GLint view[4];
	glGetIntegerv(GL_VIEWPORT, view);

	GLdouble fx, fy, fz;
	gluUnProject(x, view[3] - y, 0.0, modelviewmatrix, projectmatrix, view, &fx, &fy, &fz);
	coordinate[0][0] = fx;
	coordinate[0][1] = fy;
	coordinate[0][2] = fz;

	gluUnProject(x+width, view[3] - y, 0.0, modelviewmatrix, projectmatrix, view, &fx, &fy, &fz);
	coordinate[1][0] = fx;
	coordinate[1][1] = fy;
	coordinate[1][2] = fz;

	gluUnProject(x + width, view[3] - y - height, 0.0, modelviewmatrix, projectmatrix, view, &fx, &fy, &fz);
	coordinate[2][0] = fx;
	coordinate[2][1] = fy;
	coordinate[2][2] = fz;

	gluUnProject(x, view[3] - y - height, 0.0, modelviewmatrix, projectmatrix, view, &fx, &fy, &fz);
	coordinate[3][0] = fx;
	coordinate[3][1] = fy;
	coordinate[3][2] = fz;

	return coordinate;
}