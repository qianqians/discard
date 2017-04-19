#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CUserPanel 对话框

class CUserPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CUserPanel)

public:
	CUserPanel(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CUserPanel();

// 对话框数据
	enum { IDD = IDD_DIALOG_USER };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
};
