#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CGroupPanel �Ի���

class CGroupPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CGroupPanel)

public:
	CGroupPanel(CWnd* pParent = NULL);   // ��׼���캯��
	virtual ~CGroupPanel();

// �Ի�������
	enum { IDD = IDD_DIALOG_GROUP };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	DECLARE_MESSAGE_MAP()
};
