// ChatPanel.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "ChatPanel.h"
#include "afxdialogex.h"


// CChatPanel 对话框

IMPLEMENT_DYNAMIC(CChatPanel, CDialogEx)

CChatPanel::CChatPanel(CWnd* pParent /*=NULL*/)
	: CDialogEx(CChatPanel::IDD, pParent)
{

}

CChatPanel::~CChatPanel()
{
}

void CChatPanel::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CChatPanel, CDialogEx)
END_MESSAGE_MAP()


// CChatPanel 消息处理程序
