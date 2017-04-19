#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CLobbyPanel 对话框

class CLobbyPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CLobbyPanel)

public:
	CLobbyPanel(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CLobbyPanel();

// 对话框数据
	enum { IDD = IDD_DIALOG_LOBBY };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
};
