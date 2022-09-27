//#include <mimalloc-new-delete.h>

#define GLM_FORCE_DEPTH_ZERO_TO_ONE 1
#define GLM_FORCE_LEFT_HANDED 1

#include <iostream>

#include <gfx.h>

#include <SDL.h>
#include <SDL_syswm.h>

//#include <glm/glm.hpp>
//#include <glm/ext.hpp>
//
//#include <Graphics/GraphicsTools/interface/MapHelper.hpp>
//
//#include "Graphics/GraphicsEngineD3D11/interface/EngineFactoryD3D11.h"
//#include "Platforms/Win32/interface/Win32NativeWindow.h"

#include <filamentapp/MeshAssimp.h>
#include <math/mat4.h>

#include "../engine/gfx/src/platform/windows/RawWindowHandleImpl.h"
#include "textures2D.h"
#include "model.h"
//
//void draw(Diligent::RefCntAutoPtr<Diligent::IDeviceContext> _immediate_context, common::program& program, common::Model& _model) {
//
//	const glm::vec3 at(0.0f, 0.0f, 10.0f);
//	const glm::vec3 eye(0.0f, 10.0f, -5.0f);
//	const glm::vec3 up(0.0f, 1.0f, 0.0f);
//
//	glm::mat4x4 view = glm::lookAt(eye, at, up);
//	glm::mat4x4 proj = glm::perspective(55.0f, float(1024) / float(768), 1.0f, 100.0f);
//	auto ViewProjMatrix = proj * view;
//
//	glm::vec4 view_at(0.0f, 0.0f, -20.0f, 0.0f);
//
//	{
//		Diligent::MapHelper<common::Constants> CBConstants(_immediate_context, program.pConstants, Diligent::MAP_WRITE, Diligent::MAP_FLAG_DISCARD);
//		CBConstants->ViewProjMat = glm::transpose(ViewProjMatrix);
//		CBConstants->ViewPos = glm::vec4(at, 1);
//		CBConstants->LightPos = view_at;
//	}
//	_model.Draw(program);
//
//	program.swapchain->Present();
//}

int main(int _argc, char* _argv[]) {
	if (SDL_Init(SDL_INIT_GAMECONTROLLER | SDL_INIT_TIMER) != 0) {
		std::cout << "SDL fail to initialize! " << SDL_GetError() << std::endl;
		return 1;
	}

	Uint32 windowFlags = SDL_WINDOW_SHOWN | SDL_WINDOW_ALLOW_HIGHDPI | SDL_WINDOW_INPUT_FOCUS | SDL_WINDOW_RESIZABLE;
	auto _sdlWindow = SDL_CreateWindow("triangle", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 1024, 768, windowFlags);

	auto _engine = new Chowder::Engine();

	

	bool is_mouse_lbtn_down = false;
	int x, y;
	while (true) {
		SDL_Event _event;
		if (SDL_PollEvent(&_event) != 0) {
			if (_event.type == SDL_QUIT) {
				break;
			}

			if (_event.type == SDL_MOUSEBUTTONDOWN) {
				if (_event.button.button == SDL_BUTTON_LEFT) {
					is_mouse_lbtn_down = true;
					x = _event.button.x;
					y = _event.button.y;
					std::cout << "on mouse down! x:" << x << " y:" << y << std::endl;
				}
			}

			if (_event.type == SDL_MOUSEBUTTONUP) {
				if (_event.button.button == SDL_BUTTON_LEFT) {
					is_mouse_lbtn_down = false;
					std::cout << "on mouse up!" << std::endl;
				}
			}

			if (_event.type == SDL_MOUSEMOTION) {
				if (is_mouse_lbtn_down) {
					float dx = (0.25f * static_cast<float>(_event.button.x - x) * common::PI / 180);
					float dy = (0.05f * static_cast<float>(_event.button.y - y) * common::PI / 180);

					_model.update_model(dx, dy);

					x = _event.button.x;
					y = _event.button.y;

					std::cout << "on mouse move! dx:" << dx << " dy:" << dy << std::endl;
				}
			}
		}

		//draw(_immediate_context, program, _model);
	}

	return 0;
}