// ChatPanel.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "chat.h"
#include "ChatPanel.h"
#include "afxdialogex.h"


// CChatPanel �Ի���

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


// CChatPanel ��Ϣ�������
