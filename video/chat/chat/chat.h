
// chat.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������

#include "NetThread.h"
#include "ChatDlg2.h"
#include "VideoDlg.h"


// CchatApp: 
// �йش����ʵ�֣������ chat.cpp
//

class CchatApp : public CWinApp
{
public:
	CchatApp();

// ��д
public:
	virtual BOOL InitInstance();

	CNetThread net_interface;

	HWND login_hwnd;
	HWND main_hwnd;
	DWORD idThread;

	CChatDlg2 user_dlg;

	CVideoPreview * videopreview;
	CVideoDlg * video;

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CchatApp theApp;