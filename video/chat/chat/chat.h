
// chat.h : PROJECT_NAME 应用程序的主头文件
//

#pragma once

#ifndef __AFXWIN_H__
	#error "在包含此文件之前包含“stdafx.h”以生成 PCH 文件"
#endif

#include "resource.h"		// 主符号

#include "NetThread.h"
#include "ChatDlg2.h"
#include "VideoDlg.h"


// CchatApp: 
// 有关此类的实现，请参阅 chat.cpp
//

class CchatApp : public CWinApp
{
public:
	CchatApp();

// 重写
public:
	virtual BOOL InitInstance();

	CNetThread net_interface;

	HWND login_hwnd;
	HWND main_hwnd;
	DWORD idThread;

	CChatDlg2 user_dlg;

	CVideoPreview * videopreview;
	CVideoDlg * video;

// 实现

	DECLARE_MESSAGE_MAP()
};

extern CchatApp theApp;