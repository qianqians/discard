/*
*  qianqians
*  2015-10-28
*/
#ifndef _glextARB_h_
#define _glextARB_h_

#ifdef _WINDOWS

#include <gl/glcorearb.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#include <gl/GLU.h>

#include <Windows.h>

#pragma comment(lib, "opengl32.lib")
#pragma comment(lib, "glu32.lib") 

#endif

extern PFNGLGETSHADERINFOLOGPROC glGetShaderInfoLog;
extern PFNGLGETSHADERIVPROC glGetShaderiv;
extern PFNGLVERTEXATTRIB1FPROC glVertexAttrib1f;
extern PFNGLVERTEXATTRIB3FPROC glVertexAttrib3f;
extern PFNGLGETATTRIBLOCATIONPROC glGetAttribLocation;
extern PFNGLVERTEXATTRIBPOINTERPROC glVertexAttribPointer;
extern PFNGLENABLEVERTEXATTRIBARRAYPROC glEnableVertexAttribArray;
extern PFNGLBUFFERDATAPROC glBufferData;
extern PFNGLBINDBUFFERPROC glBindBuffer;
extern PFNGLGENBUFFERSPROC glGenBuffers;
extern PFNGLGENVERTEXARRAYSPROC glGenVertexArrays;
//extern PFNGLTEXIMAGE2DPROC glTexImage2D;
extern PFNGLLINKPROGRAMPROC glLinkProgram;
extern PFNGLDETACHSHADERPROC glDetachShader;
extern PFNGLATTACHSHADERPROC glAttachShader;
extern PFNGLDELETESHADERPROC glDeleteShader;
extern PFNGLCOMPILESHADERPROC glCompileShader;
extern PFNGLSHADERSOURCEPROC glShaderSource;
extern PFNGLCREATESHADERPROC glCreateShader;
extern PFNGLUSEPROGRAMPROC glUseProgram;
extern PFNGLGETPROGRAMINFOLOGPROC glGetProgramInfoLog;
extern PFNGLGETPROGRAMIVPROC glGetProgramiv;
extern PFNGLCREATEPROGRAMPROC glCreateProgram;
//extern PFNGLDRAWARRAYSPROC glDrawArrays;
extern PFNGLBINDVERTEXARRAYPROC glBindVertexArray;
extern PFNGLVALIDATEPROGRAMPROC glValidateProgram;

#endif