/*
*  qianqians
*  2014-12-12
*/
#ifndef _render_h
#define _render_h

#include <list>

#include "shader.h"
#include "vertext.h"
#include "texture.h"
#include "material.h"

class render{
public:
	render();
	~render();

	shader * get_shader();
	
	void update(GLint _program);

	void setvertext(vertext * vertext);

	void settexture(texture * texture);

	void setmaterial(material * material);

	void setlightmap(texture * lightmap);

	void setshader(shader * shader);

private:
	void drawrender(GLint _program);

	friend void draw();

public:
	bool isupdate;

	shader * _shader;
	vertext * _vertext;
	texture* _texture;
	texture* _lightmap;
	material * _material;

	GLuint vao;
	GLuint vboVertext;

};

#endif _render_h