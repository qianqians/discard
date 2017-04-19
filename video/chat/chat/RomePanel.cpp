// RomePanel.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "RomePanel.h"
#include "afxdialogex.h"


// CRomePanel 对话框

IMPLEMENT_DYNAMIC(CRomePanel, CDialogEx)

CRomePanel::CRomePanel(CWnd* pParent /*=NULL*/)
	: CDialogEx(CRomePanel::IDD, pParent)
{

}

CRomePanel::~CRomePanel()
{
}

void CRomePanel::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CRomePanel, CDialogEx)
END_MESSAGE_MAP()


// CRomePanel 消息处理程序
