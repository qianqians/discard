// LobbyPanel.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "chat.h"
#include "LobbyPanel.h"
#include "afxdialogex.h"


// CLobbyPanel �Ի���

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


// CLobbyPanel ��Ϣ�������
