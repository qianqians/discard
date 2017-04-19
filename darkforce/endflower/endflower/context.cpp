/*
 *  qianqians
 *  2014-12-12
 */
#include <exception>
#include "context.h"
#include "wnd.h"
#include "endflower.h"

context::context(wnd * wnd){
	hdc = wnd->getDC();
}

context::~context(){
}

camera * context::get_camera() {
	return &_camera;
}

void context::addrender(boost::shared_ptr<render> _render) {
	drawlist.push_back(_render);
}

void context::lookat(float postionx, float postiony, float postionz, float upx, float upy, float upz, float lookatx, float lookaty, float lookatz){
	_camera.postion[0] = postionx;
	_camera.postion[1] = postiony;
	_camera.postion[2] = postionz;
	
	_camera.up[0] = upx;
	_camera.up[1] = upy;
	_camera.up[2] = upz;
	
	_camera.lookat[0] = lookatx;
	_camera.lookat[1] = lookaty;
	_camera.lookat[2] = lookatz;
}

bool context::swapbuff() {
	return SwapBuffers(hdc);
}

void context::make_current(){
	if (rc == 0) {
		static PIXELFORMATDESCRIPTOR pfd =
		{
			sizeof(PIXELFORMATDESCRIPTOR),
			1,
			PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER,
			PFD_TYPE_RGBA,
			16,
			0, 0, 0, 0, 0, 0,
			0,
			0,
			0,
			0, 0, 0, 0,
			16,
			0,
			0,
			PFD_MAIN_PLANE,
			0,
			0, 0, 0
		};

		GLint iPixelFormat;
		if ((iPixelFormat = ChoosePixelFormat(hdc, &pfd)) == 0){
			throw std::exception("ChoosePixelFormat Failed");
		}

		if (SetPixelFormat(hdc, iPixelFormat, &pfd) == false){
			throw std::exception("SetPixelFormat Failed");
		}
		
		rc = wglCreateContext(hdc);
	}

	wglMakeCurrent(hdc, rc);

	if (glGetShaderInfoLog == 0) {
		glGetShaderInfoLog = (PFNGLGETSHADERINFOLOGPROC)wglGetProcAddress("glGetShaderInfoLog");
	}
	if (glGetShaderiv == 0) {
		glGetShaderiv = (PFNGLGETSHADERIVPROC)wglGetProcAddress("glGetShaderiv");
	}
	if (glDetachShader == 0) {
		glDetachShader = (PFNGLDETACHSHADERPROC)wglGetProcAddress("glDetachShader");
	}
	if (glAttachShader == 0) {
		glAttachShader = (PFNGLATTACHSHADERPROC)wglGetProcAddress("glAttachShader");
	}
	if (glDeleteShader == 0) {
		glDeleteShader = (PFNGLDELETESHADERPROC)wglGetProcAddress("glDeleteShader");
	}
	if (glCompileShader == 0) {
		glCompileShader = (PFNGLCOMPILESHADERPROC)wglGetProcAddress("glCompileShader");
	}
	if (glShaderSource == 0) {
		glShaderSource = (PFNGLSHADERSOURCEPROC)wglGetProcAddress("glShaderSource");
	}
	if (glCreateShader == 0) {
		glCreateShader = (PFNGLCREATESHADERPROC)wglGetProcAddress("glCreateShader");
	}
	if (glUseProgram == 0) {
		glUseProgram = (PFNGLUSEPROGRAMPROC)wglGetProcAddress("glUseProgram");
	}
	if (glGetProgramInfoLog == 0) {
		glGetProgramInfoLog = (PFNGLGETPROGRAMINFOLOGPROC)wglGetProcAddress("glGetProgramInfoLog");
	}
	if (glGetProgramiv == 0) {
		glGetProgramiv = (PFNGLGETPROGRAMIVPROC)wglGetProcAddress("glGetProgramiv");
	}
	if (glCreateProgram == 0) {
		glCreateProgram = (PFNGLCREATEPROGRAMPROC)wglGetProcAddress("glCreateProgram");
	}
	if (glBindVertexArray == 0) {
		glBindVertexArray = (PFNGLBINDVERTEXARRAYPROC)wglGetProcAddress("glBindVertexArray");
	}
	if (glLinkProgram == 0) {
		glLinkProgram = (PFNGLLINKPROGRAMPROC)wglGetProcAddress("glLinkProgram");
	}
	if (glVertexAttribPointer == 0) {
		glVertexAttribPointer = (PFNGLVERTEXATTRIBPOINTERPROC)wglGetProcAddress("glVertexAttribPointer");
	}
	if (glEnableVertexAttribArray == 0) {
		glEnableVertexAttribArray = (PFNGLENABLEVERTEXATTRIBARRAYPROC)wglGetProcAddress("glEnableVertexAttribArray");
	}
	if (glBufferData == 0) {
		glBufferData = (PFNGLBUFFERDATAPROC)wglGetProcAddress("glBufferData");
	}
	if (glBindBuffer == 0) {
		glBindBuffer = (PFNGLBINDBUFFERPROC)wglGetProcAddress("glBindBuffer");
	}
	if (glGenBuffers == 0) {
		glGenBuffers = (PFNGLGENBUFFERSPROC)wglGetProcAddress("glGenBuffers");
	}
	if (glGenVertexArrays == 0) {
		glGenVertexArrays = (PFNGLGENVERTEXARRAYSPROC)wglGetProcAddress("glGenVertexArrays");
	}
	if (glGetAttribLocation == 0) {
		glGetAttribLocation = (PFNGLGETATTRIBLOCATIONPROC)wglGetProcAddress("glGetAttribLocation");
	}
	if (glValidateProgram == 0) {
		glValidateProgram = (PFNGLVALIDATEPROGRAMPROC)wglGetProcAddress("glValidateProgram");
	}

	//if (__program == 0) {
	//	__program = glCreateProgram();
	//}

	gluLookAt(_camera.lookat[0], _camera.lookat[1], _camera.lookat[2], _camera.postion[0], _camera.postion[1], _camera.postion[2], _camera.up[0], _camera.up[1], _camera.up[2]);
}
//
//GLuint context::get_program() {
//	return __program;
//}

void context::release(){
	//GLint linkStatus;
	//glGetProgramiv(__program, GL_LINK_STATUS, &linkStatus);
	//if (GL_TRUE == linkStatus) {
	//	glLinkProgram(0);
	//	glUseProgram(0);
	//}
		   
	wglMakeCurrent(0, 0);
}