/*
 * RawWindowHandle.h
 * Author: qianqians
 * 2021/10/30
 */
#ifndef _RAWWINDOWHANDLE_H_
#define _RAWWINDOWHANDLE_H_

#include <SDL.h>

#include <macro.h>

namespace Chowder {

class ChowderExport RawWindowHandle {
public:
	uint32_t Width = 0;
	uint32_t Height = 0;
};

ChowderExport RawWindowHandle* GetRawWindowHandle(SDL_Window* window);

}

#endif // !_RAWWINDOWHANDLE_H_
