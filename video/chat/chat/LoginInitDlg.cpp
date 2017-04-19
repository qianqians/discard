// LoginInitDlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "chat.h"
#include "LoginInitDlg.h"
#include "afxdialogex.h"


// CLoginInitDlg �Ի���

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


// CLoginInitDlg ��Ϣ�������
LRESULT CLoginInitDlg::OnLogin(WPARAM wParam, LPARAM lParam)
{
	CDialogEx::OnOK();

	return 0;
}

LRESULT CLoginInitDlg::OnReUsername(WPARAM wParam, LPARAM lParam)
{
	//CDialogEx::OnOK();
	MessageBox("�û����ظ�", "������ʾ", MB_OK);

	return 0;
}

void CLoginInitDlg::OnBnClickedOk()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
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

	// TODO:  �ڴ������ר�õĴ�������
	theApp.login_hwnd = GetSafeHwnd();

	return 0;
}
