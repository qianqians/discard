#pragma once

#include <afxdialogex.h>

#include "afxcmn.h"
#include "afxwin.h"
#include "Resource.h"

// CLobbyPanel �Ի���

class CLobbyPanel : public CDialogEx
{
	DECLARE_DYNAMIC(CLobbyPanel)

public:
	CLobbyPanel(CWnd* pParent = NULL);   // ��׼���캯��
	virtual ~CLobbyPanel();

// �Ի�������
	enum { IDD = IDD_DIALOG_LOBBY };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	DECLARE_MESSAGE_MAP()
};
