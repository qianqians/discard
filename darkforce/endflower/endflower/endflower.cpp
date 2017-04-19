/*
* qianqians
* 2015-10-9
*/
#include "wnd.h"
#include "endflower.h"

#include <Windows.h>

PFNGLGETSHADERINFOLOGPROC glGetShaderInfoLog = 0;
PFNGLGETSHADERIVPROC glGetShaderiv = 0;
PFNGLGETATTRIBLOCATIONPROC glGetAttribLocation = 0;
PFNGLVERTEXATTRIBPOINTERPROC glVertexAttribPointer = 0;
PFNGLENABLEVERTEXATTRIBARRAYPROC glEnableVertexAttribArray = 0;
PFNGLBUFFERDATAPROC glBufferData = 0;
PFNGLBINDBUFFERPROC glBindBuffer = 0;
PFNGLGENBUFFERSPROC glGenBuffers = 0;
PFNGLGENVERTEXARRAYSPROC glGenVertexArrays = 0;
PFNGLLINKPROGRAMPROC glLinkProgram = 0;
PFNGLDETACHSHADERPROC glDetachShader = 0;
PFNGLATTACHSHADERPROC glAttachShader = 0;
PFNGLDELETESHADERPROC glDeleteShader = 0;
PFNGLCOMPILESHADERPROC glCompileShader = 0;
PFNGLSHADERSOURCEPROC glShaderSource = 0;
PFNGLCREATESHADERPROC glCreateShader = 0;
PFNGLUSEPROGRAMPROC glUseProgram = 0;
PFNGLGETPROGRAMINFOLOGPROC glGetProgramInfoLog = 0;
PFNGLGETPROGRAMIVPROC glGetProgramiv = 0;
PFNGLCREATEPROGRAMPROC glCreateProgram = 0;
PFNGLBINDVERTEXARRAYPROC glBindVertexArray = 0;
PFNGLVALIDATEPROGRAMPROC glValidateProgram = 0;

void draw() {
	wnd * _wnd = get_current_wnd();
	if (_wnd != 0) {
		glClearColor(0.0, 0.0, 0.0, 1.0);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		static shader * _current_shader = 0;
		for (auto it = _wnd->c->drawlist.begin(); it != _wnd->c->drawlist.end(); it++) {
			shader * _shader = it->get()->get_shader();
			if (_shader != _current_shader) {
				if (_current_shader != 0) {
					_current_shader->release();
				}
				_current_shader = _shader;
				_current_shader->make_current();
			}
			it->get()->drawrender(_shader->get_program());
		}
		_wnd->c->swapbuff();
	}
}

void run(){
	MSG msg;
	for (auto w : wnd::wndmap){
		while (GetMessage(&msg, w.second->whandle, 0, 0)){
			TranslateMessage(&msg);
			DispatchMessage(&msg);

			try {
				draw();
			}
			catch (...) {

			}
		}
	}
}