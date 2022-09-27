/*
 * RawWindowHandleImpl.cpp
 * Author: qianqians
 * 2021/12/13
 */
#include "gfx.h"
#include "RawWindowHandleImpl.h"
#include "exception.h"

namespace Chowder {

filament::SwapChain* Engine::CreateSwapChain(RawWindowHandle* handle) {
	auto _wnd_handle = static_cast<RawWindowHandleImpl*>(handle);
	return engine->createSwapChain(_wnd_handle->window);
}

}
