// LoginInitDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "LoginInitDlg.h"
#include "afxdialogex.h"


// CLoginInitDlg 对话框

IMPLEMENT_DYNAMIC(CLoginInitDlg, CDialogEx)

CLoginInitDlg::CLoginInitDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_DIALOG_LOGIN2, pParent)
	, valueusername(_T(""))
{

}

CLoginInitDlg::~CLoginInitDlg()
{
}

void CLoginInitDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT1, valueusername);
}


BEGIN_MESSAGE_MAP(CLoginInitDlg, CDialogEx)
	ON_BN_CLICKED(IDOK, &CLoginInitDlg::OnBnClickedOk)
	ON_MESSAGE(WM_SOCKET_MSG_LOGIN2, CLoginInitDlg::OnLogin)
	ON_MESSAGE(WM_REUSERNAME, CLoginInitDlg::OnReUsername)
	ON_WM_CREATE()
END_MESSAGE_MAP()


// CLoginInitDlg 消息处理程序
LRESULT CLoginInitDlg::OnLogin(WPARAM wParam, LPARAM lParam)
{
	CDialogEx::OnOK();

	return 0;
}

LRESULT CLoginInitDlg::OnReUsername(WPARAM wParam, LPARAM lParam)
{
	//CDialogEx::OnOK();
	MessageBox("用户名重复", "错误提示", MB_OK);

	return 0;
}

void CLoginInitDlg::OnBnClickedOk()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);

	if (valueusername != "")
	{
		theApp.net_interface.sendlogin2(valueusername);
		theApp.net_interface.username = valueusername;
	}
}


int CLoginInitDlg::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialogEx::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  在此添加您专用的创建代码
	theApp.login_hwnd = GetSafeHwnd();

	return 0;
}
