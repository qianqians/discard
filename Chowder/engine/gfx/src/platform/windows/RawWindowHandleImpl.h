/*
 * RawWindowHandleImpl.h
 * Author: qianqians
 * 2021/12/13
 */
#ifndef _RAWWINDOWHANDLEIMPL_H_
#define _RAWWINDOWHANDLEIMPL_H_

#include <RawWindowHandle.h>

#ifdef _WIN32
#include <Windows.h>
#endif

namespace Chowder {

class RawWindowHandleImpl : public RawWindowHandle {
public:
#ifdef _WIN32
    HINSTANCE instance;
    HWND window;
#endif

};

}

#endif //!_RAWWINDOWHANDLEIMPL_H_