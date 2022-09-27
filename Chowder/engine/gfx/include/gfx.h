/*
 * gfx.h
 * Author: qianqians
 * 2021/11/3
 */
#ifndef _GFX_H_
#define _GFX_H_

#include <tuple>

#include <filament/Engine.h>
#include <filament/Renderer.h>
#include <filament/Scene.h>
#include <filament/View.h>

#include <macro.h>
#include <exception.h>
#include <RawWindowHandle.h>

namespace Chowder {

class Engine {
public:
	filament::Engine* engine;

public:
	Engine();

	virtual ~Engine();

	filament::SwapChain* CreateSwapChain(RawWindowHandle* handle);

};

}

#endif // !_GFX_H_