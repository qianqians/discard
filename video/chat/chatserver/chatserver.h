
// chatserver.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������
#include "NetThreadSvr.h"


// CchatserverApp: 
// �йش����ʵ�֣������ chatserver.cpp
//

class CchatserverApp : public CWinApp
{
public:
	CchatserverApp();

// ��д
public:
	virtual BOOL InitInstance();

	CNetThreadSvr netsvr_interface;

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CchatserverApp theApp;