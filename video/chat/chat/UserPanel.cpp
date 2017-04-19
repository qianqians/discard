// UserPanel.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "UserPanel.h"
#include "afxdialogex.h"


// CUserPanel 对话框

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


// CUserPanel 消息处理程序
