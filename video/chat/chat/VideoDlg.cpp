// VideoDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "VideoDlg.h"
#include "afxdialogex.h"


// CVideoDlg 对话框

IMPLEMENT_DYNAMIC(CVideoDlg, CDialogEx)

CVideoDlg::CVideoDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_DIALOG_VIDEO, pParent)
{

}

CVideoDlg::~CVideoDlg()
{
}

void CVideoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CVideoDlg, CDialogEx)
END_MESSAGE_MAP()


// CVideoDlg 消息处理程序
