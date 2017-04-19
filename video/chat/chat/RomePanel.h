#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CRomePanel 对话框

class CRomePanel : public CDialogEx
{
	DECLARE_DYNAMIC(CRomePanel)

public:
	CRomePanel(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CRomePanel();

// 对话框数据
	enum { IDD = IDD_DIALOG_ROME };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
};
