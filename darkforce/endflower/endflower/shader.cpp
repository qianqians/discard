/*
*  qianqians
*  2014-12-12
*/
#include <stdio.h>
#include <string.h>
#include <iostream>

#include "shader.h"
#include "endflower.h"

vertshader::vertshader(char * vertfile){
	FILE * f;
	fopen_s(&f, vertfile, "r");

	fseek(f, 0, SEEK_END);
	GLint size = ftell(f);
	fseek(f, 0, SEEK_SET);

	char * fbuf = new char[size+1];
	memset(fbuf, 0, size+1);

	fread_s(fbuf, size + 1, size, 1, f);

	_vertshader = glCreateShader(GL_VERTEX_SHADER);
	glShaderSource(_vertshader, 1, &fbuf, &size);
	glCompileShader(_vertshader);
	GLint cStatus = 0;
	glGetShaderiv(_vertshader, GL_COMPILE_STATUS, &cStatus);
	if (GL_FALSE == cStatus) {
		GLint logLen;
		glGetShaderiv(_vertshader, GL_INFO_LOG_LENGTH, &logLen);
		char *log = (char *)malloc(logLen);
		glGetShaderInfoLog(_vertshader, GL_INFO_LOG_LENGTH, &logLen, log);
		std::cerr << "vertshader log : " << std::endl;
		std::cerr << log << std::endl;
	}
	fclose(f);
}

vertshader::~vertshader(){
	glDeleteShader(_vertshader);
}

fragshader::fragshader(char * fragfile){
	FILE * f;
	fopen_s(&f, fragfile, "r");

	fseek(f, 0, SEEK_END);
	GLint size = ftell(f);
	fseek(f, 0, SEEK_SET);

	char * fbuf = new char[size + 1];
	memset(fbuf, 0, size + 1);

	fread_s(fbuf, size + 1, size, 1, f);

	_fragshader = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(_fragshader, 1, &fbuf, &size);
	glCompileShader(_fragshader);
	GLint cStatus = 0;
	glGetShaderiv(_fragshader, GL_COMPILE_STATUS, &cStatus);
	if (GL_FALSE == cStatus) {	
		GLint logLen;
		glGetShaderiv(_fragshader, GL_INFO_LOG_LENGTH, &logLen);
		char *log = (char *)malloc(logLen);
		glGetShaderInfoLog(_fragshader, GL_INFO_LOG_LENGTH, &logLen, log);
		std::cerr << "fragshader log : " << std::endl;
		std::cerr << log << std::endl;
	}
	fclose(f);
}

fragshader::~fragshader(){
	glDeleteShader(_fragshader);
}

shader::shader(char * vertfile, char * fragfile) : vs(vertfile), fs(fragfile){
	_program = glCreateProgram();
}

shader::~shader(){
}

void shader::make_current(){
	if (_program) {
		glAttachShader(_program, vs._vertshader);
		glAttachShader(_program, fs._fragshader);

		GLint linkStatus = 0;
		glGetProgramiv(_program, GL_LINK_STATUS, &linkStatus);
		if (GL_FALSE == linkStatus) {
			glLinkProgram(_program);
			glGetProgramiv(_program, GL_LINK_STATUS, &linkStatus);
			if (GL_FALSE == linkStatus) {
				auto error = glGetError();
				std::cerr << "ERROR : link program failed" << std::endl;
				GLint logLen;
				glGetProgramiv(_program, GL_INFO_LOG_LENGTH, &logLen);
				if (logLen > 0)
				{
					char *log = (char *)malloc(logLen);
					GLsizei written;
					glGetProgramInfoLog(_program, logLen, &written, log);
					std::cerr << "program log : " << std::endl;
					std::cerr << log << std::endl;
				}
				glValidateProgram(_program);
				GLint status;
				glGetProgramiv(_program, GL_VALIDATE_STATUS, &status);
				if (status == GL_FALSE){
					std::cerr << "Error validating shader " << _program << std::endl;
				}
			} else {
				glUseProgram(_program);
			}
		}

		glDeleteShader(fs._fragshader);
		glDeleteShader(vs._vertshader);
	}
}

GLuint shader::get_program() {
	return _program;
}

void shader::release() {
	if (_program) {
		glDetachShader(_program, vs._vertshader);
		glDetachShader(_program, fs._fragshader);
	}
}