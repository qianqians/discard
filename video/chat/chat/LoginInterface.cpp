// LoginInterface.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "LoginInterface.h"
#include "afxdialogex.h"

#include "RegisterInsterface.h"

// CLoginInterface 对话框

IMPLEMENT_DYNAMIC(CLoginInterface, CDialogEx)

CLoginInterface::CLoginInterface(CWnd* pParent /*=NULL*/)
	: CDialogEx(CLoginInterface::IDD, pParent)
	, valueusername(_T(""))
	, valuekey(_T(""))
{

}

CLoginInterface::~CLoginInterface()
{
}

void CLoginInterface::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, editusername);
	DDX_Control(pDX, IDC_EDIT4, editkey);
	DDX_Control(pDX, IDC_STATIC_USERNAME, username);
	DDX_Control(pDX, IDC_STATIC_KEY, key);
	DDX_Text(pDX, IDC_EDIT1, valueusername);
	DDX_Text(pDX, IDC_EDIT4, valuekey);
}


BEGIN_MESSAGE_MAP(CLoginInterface, CDialogEx)
	ON_WM_CLOSE()
	ON_BN_CLICKED(IDC_BUTTON1, &CLoginInterface::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, &CLoginInterface::OnBnClickedButton2)
	ON_WM_CREATE()
END_MESSAGE_MAP()


// CLoginInterface 消息处理程序


void CLoginInterface::OnClose()
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值

	CDialogEx::OnCancel();
}


void CLoginInterface::OnBnClickedButton1()
{
	// TODO:  在此添加控件通知处理程序代码

	UpdateData(TRUE);

	theApp.net_interface.sendlogin(valueusername, valuekey);
}


void CLoginInterface::OnBnClickedButton2()
{
	// TODO:  在此添加控件通知处理程序代码

	ShowWindow(SW_HIDE);
	CloseWindow();

	CRegisterInsterface reg;
	if (reg.DoModal() == IDCANCEL) {
		CDialogEx::OnCancel();
	}
}


int CLoginInterface::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialogEx::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  在此添加您专用的创建代码
	theApp.main_hwnd = GetSafeHwnd();

	return 0;
}
