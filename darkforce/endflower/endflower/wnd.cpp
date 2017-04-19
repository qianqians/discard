/*
*  qianqians
*  2015-10-9
*/

#include "wnd.h"

boost::unordered_map<HWND, wnd*> wnd::wndmap;

static wnd * _current_wnd = 0;

wnd::wnd(std::string title, int left, int top, int width, int height){
	WNDCLASS wcx;

	// Fill in the window class structure with parameters 
	// that describe the main window. 

	wcx.style = CS_HREDRAW | CS_VREDRAW;                     // redraw if size changes 
	wcx.lpfnWndProc = wnd::WndProc;							 // points to window procedure 
	wcx.cbClsExtra = 0;										 // no extra class memory 
	wcx.cbWndExtra = 0;										 // no extra window memory 
	wcx.hInstance = NULL;									 // handle to instance 
	wcx.hIcon = LoadIcon(NULL, IDI_APPLICATION);			 // predefined app. icon 
	wcx.hCursor = LoadCursor(NULL, IDC_ARROW);               // predefined arrow 
	wcx.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH); // white background brush 
	wcx.lpszMenuName = NULL;	 							 // name of menu resource 
	wcx.lpszClassName = "wnd";								 // name of window class 

	// Register the window class. 
	RegisterClass(&wcx);

	whandle = CreateWindow(
		"wnd",				 // name of window class 
		title.c_str(),		 // title-bar string 
		WS_OVERLAPPEDWINDOW, // top-level window 
		CW_USEDEFAULT,       // default horizontal position 
		CW_USEDEFAULT,       // default vertical position 
		CW_USEDEFAULT,       // default width 
		CW_USEDEFAULT,       // default height 
		(HWND)NULL,          // no owner window 
		(HMENU)NULL,         // use class menu 
		NULL,	             // handle to application instance 
		(LPVOID)NULL);       // no window-creation data 

	if (!whandle){
		auto error = GetLastError();
		throw std::exception("create window failed");
	}

	wndmap.insert(std::make_pair(whandle, this));
	c = new context(this);

	ShowWindow(whandle, SW_SHOW);
	UpdateWindow(whandle);
}

wnd::~wnd(){
	CloseWindow(whandle);
}

LRESULT CALLBACK wnd::WndProc(HWND hwnd, UINT umsg, WPARAM wParam, LPARAM lParam){
	auto it = wndmap.find(hwnd);
	
	if (it != wndmap.end()){
		switch (umsg)
		{
		case WM_INPUT:
			if (wParam == RIM_INPUT) {
				it->second->sigRawInput((LPRAWINPUT)lParam);
			}
			break;

		case WM_KILLFOCUS:
			_current_wnd = 0;
			it->second->c->release();
			it->second->sigUnActivate();
			break;

		case WM_SETFOCUS:
			_current_wnd = it->second;
			it->second->c->make_current();
			it->second->sigActivate();
			break;

		default:
			break;
		}
	}

	return DefWindowProc(hwnd, umsg, wParam, lParam);
}

void wnd::showWnd(swStyle swstyle){
	if (swstyle == showWND){
		ShowWindow(whandle, SW_SHOW);
	} else if (swstyle == showWND){
		ShowWindow(whandle, SW_HIDE);
	}
}

HDC wnd::getDC(){
	return GetDC(whandle);
}

context * wnd::get_context() {
	return c;
}

wnd * get_current_wnd() {
	return _current_wnd;
}