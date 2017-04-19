
// chatserver.h : PROJECT_NAME 应用程序的主头文件
//

#pragma once

#ifndef __AFXWIN_H__
	#error "在包含此文件之前包含“stdafx.h”以生成 PCH 文件"
#endif

#include "resource.h"		// 主符号
#include "NetThreadSvr.h"


// CchatserverApp: 
// 有关此类的实现，请参阅 chatserver.cpp
//

class CchatserverApp : public CWinApp
{
public:
	CchatserverApp();

// 重写
public:
	virtual BOOL InitInstance();

	CNetThreadSvr netsvr_interface;

// 实现

	DECLARE_MESSAGE_MAP()
};

extern CchatserverApp theApp;