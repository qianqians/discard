// LobbyPanel.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "LobbyPanel.h"
#include "afxdialogex.h"


// CLobbyPanel 对话框

IMPLEMENT_DYNAMIC(CLobbyPanel, CDialogEx)

CLobbyPanel::CLobbyPanel(CWnd* pParent /*=NULL*/)
	: CDialogEx(CLobbyPanel::IDD, pParent)
{

}

CLobbyPanel::~CLobbyPanel()
{
}

void CLobbyPanel::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CLobbyPanel, CDialogEx)
END_MESSAGE_MAP()


// CLobbyPanel 消息处理程序
