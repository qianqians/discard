#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CGroupPanel 对话框

class CGroupPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CGroupPanel)

public:
	CGroupPanel(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CGroupPanel();

// 对话框数据
	enum { IDD = IDD_DIALOG_GROUP };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
};
