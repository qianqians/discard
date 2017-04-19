#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CChatPanel 对话框

class CChatPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CChatPanel)

public:
	CChatPanel(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CChatPanel();

// 对话框数据
	enum { IDD = IDD_DIALOG_CHAT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
};
