// UserPanel.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "chat.h"
#include "UserPanel.h"
#include "afxdialogex.h"


// CUserPanel �Ի���

IMPLEMENT_DYNAMIC(CUserPanel, CDialogEx)

CUserPanel::CUserPanel(CWnd* pParent /*=NULL*/)
	: CDialogEx(CUserPanel::IDD, pParent)
{

}

CUserPanel::~CUserPanel()
{
}

void CUserPanel::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CUserPanel, CDialogEx)
END_MESSAGE_MAP()


// CUserPanel ��Ϣ�������
