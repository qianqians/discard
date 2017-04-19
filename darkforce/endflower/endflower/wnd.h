/*
 *  qianqians
 *  2015-10-9
 */
#ifndef _wnd_h_
#define _wnd_h_

#include <string>

#include <boost/signal.hpp>
#include <boost/unordered_map.hpp>

#include "context.h"

class wnd{
	enum swStyle{
		showWND = 0,
		hideWND = 1,
	};

public:
	wnd(std::string title, int left, int top, int width, int height);
	~wnd();

	void showWnd(swStyle swstyle);

	HDC getDC();

	context * get_context();

private:
	static LRESULT CALLBACK WndProc(HWND hwnd, UINT umsg, WPARAM wParam, LPARAM lParam);

	HWND whandle;

	context * c;

	static boost::unordered_map<HWND, wnd*> wndmap;

	friend void run();
	friend void draw();

public:
	boost::signal<void(LPRAWINPUT) > sigRawInput;
	boost::signal<void()> sigActivate;
	boost::signal<void()> sigUnActivate;

};

wnd * get_current_wnd();

#endif 