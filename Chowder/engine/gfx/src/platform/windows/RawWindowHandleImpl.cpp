/*
 * RawWindowHandleImpl.cpp
 * Author: qianqians
 * 2022/5/19
 */
#include <iostream>

#include <exception.h>
#include <RawWindowHandleImpl.h>

#ifdef _WIN32
#include <Windows.h>
#endif

#include <SDL_syswm.h>

namespace Chowder {

RawWindowHandle* GetRawWindowHandle(SDL_Window* window) {
	SDL_SysWMinfo wmi;
	SDL_VERSION(&wmi.version);
	if (!SDL_GetWindowWMInfo(window, &wmi))
	{
		throw RawWindowHandleException("SDL fail to Get WindowWMInfo!" + std::string(SDL_GetError()));
	}

	auto impl = new RawWindowHandleImpl();
	impl->instance = wmi.info.win.hinstance;
	impl->window = wmi.info.win.window;

	RECT rct;
	GetClientRect(impl->window, &rct);
	impl->Width = rct.right - rct.left;
	impl->Height = rct.bottom - rct.top;

	std::cout << "width:" << impl->Width << std::endl;
	std::cout << "height:" << impl->Height << std::endl;

	return impl;
}

}
