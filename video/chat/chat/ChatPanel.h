#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CChatPanel �Ի���

class CChatPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CChatPanel)

public:
	CChatPanel(CWnd* pParent = NULL);   // ��׼���캯��
	virtual ~CChatPanel();

// �Ի�������
	enum { IDD = IDD_DIALOG_CHAT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	DECLARE_MESSAGE_MAP()
};
