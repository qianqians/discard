/*
 *  qianqians
 *  2014-12-12
 */
#include "render.h"
#include "endflower.h"

render::render() {
	isupdate = false;
}

render::~render() {
}

void render::update(GLint _program) {
	if (_vertext == 0) {
		throw std::exception("inviald render");
	}

	glGenVertexArrays(1, &vao);
	glBindVertexArray(vao);

	glGenBuffers(1, &vboVertext);
	glBindBuffer(GL_ARRAY_BUFFER, vboVertext); 
	glBufferData(GL_ARRAY_BUFFER, _vertext->count()*sizeof(float), _vertext->vert, GL_STATIC_DRAW);

	if (_vertext->uv != 0) {
		GLint index = glGetAttribLocation(_program, "vertextuv");
		glEnableVertexAttribArray(index);
		glVertexAttribPointer(index, _vertext->count(), GL_FLOAT, GL_TRUE, 2, _vertext->uv);
	}

	//for (auto it : _material->element) {
	//	switch (it._materialtype) {
	//		case material::materialtype::anacampsis:
	//			{
	//				glGetAttribLocation(_program, "");
	//			}
	//			break;

	//		case material::materialtype::reflect:
	//			{
	//				glGetAttribLocation(_program, "");
	//			}
	//			break;

	//		case material::materialtype::selflight:
	//			{
	//				glGetAttribLocation(_program, "");
	//			}
	//			break;

	//		default:
	//			break;
	//	}
	//}

	
	//glTexImage2D(_texture->vert, )
	//	texture* _texture;
	//texture* _lightmap;

	glBindVertexArray(0);

	isupdate = true;
}

shader * render::get_shader() {
	return _shader;
}

void render::setvertext(vertext * vertext){
	_vertext = vertext;
	isupdate = false;
}

void render::settexture(texture * texture){
	_texture = texture;
	isupdate = false;
}

void render::setmaterial(material * material){
	_material = material;
	isupdate = false;
}

void render::setlightmap(texture * lightmap){
	_lightmap = lightmap;
	isupdate = false;
}

void render::setshader(shader * shader){
	_shader = shader;
}

void render::drawrender(GLint _program) {
	if (isupdate == false) {
		update(_program);
	}

	glBindVertexArray(vao);
	glBindBuffer(GL_ARRAY_BUFFER, vboVertext);
	glDrawArrays(GL_TRIANGLES, 0, _vertext->count());
	glBindVertexArray(0);
}