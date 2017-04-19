/*
*  qianqians
*  2014-12-12
*/
#ifndef _shader_h_
#define _shader_h_

#include "glextARB.h"

class vertshader{
public:
	vertshader(char * vertfile);
	~vertshader();

private:
	GLuint _vertshader;

	friend class shader;

};

class fragshader{
public:
	fragshader(char * fragfile);
	~fragshader();

private:
	GLuint _fragshader;

	friend class shader;

};

class shader{
public:
	shader(char * vertfile, char * fragfile);
	~shader();

	void make_current();

	void release();

	GLuint get_program();

private:
	GLuint _program;
	
	vertshader vs;
	fragshader fs;

};

#endif