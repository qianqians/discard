// RomePanel.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "chat.h"
#include "RomePanel.h"
#include "afxdialogex.h"


// CRomePanel �Ի���

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


// CRomePanel ��Ϣ�������
