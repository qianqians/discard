/*
 * RawWindowHandleImpl.cpp
 * Author: qianqians
 * 2021/12/13
 */
#include "gfx.h"
#include "RawWindowHandleImpl.h"
#include "exception.h"

namespace Chowder {

Engine::Engine() {
	engine = filament::Engine::create();
}

Engine::~Engine() {
	filament::Engine::destroy(engine);
}

}
