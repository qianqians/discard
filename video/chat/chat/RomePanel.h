#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CRomePanel �Ի���

class CRomePanel : public CDialogEx
{
	DECLARE_DYNAMIC(CRomePanel)

public:
	CRomePanel(CWnd* pParent = NULL);   // ��׼���캯��
	virtual ~CRomePanel();

// �Ի�������
	enum { IDD = IDD_DIALOG_ROME };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	DECLARE_MESSAGE_MAP()
};
