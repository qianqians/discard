#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CUserPanel �Ի���

class CUserPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CUserPanel)

public:
	CUserPanel(CWnd* pParent = NULL);   // ��׼���캯��
	virtual ~CUserPanel();

// �Ի�������
	enum { IDD = IDD_DIALOG_USER };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	DECLARE_MESSAGE_MAP()
};
