// GroupPanel.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "GroupPanel.h"
#include "afxdialogex.h"


// CGroupPanel 对话框

IMPLEMENT_DYNAMIC(CGroupPanel, CDialogEx)

CGroupPanel::CGroupPanel(CWnd* pParent /*=NULL*/)
	: CDialogEx(CGroupPanel::IDD, pParent)
{

}

CGroupPanel::~CGroupPanel()
{
}

void CGroupPanel::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CGroupPanel, CDialogEx)
END_MESSAGE_MAP()


// CGroupPanel 消息处理程序
