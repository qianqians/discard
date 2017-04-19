// RegisterInsterface.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "chat.h"
#include "RegisterInsterface.h"
#include "LoginInterface.h"
#include "afxdialogex.h"


// CRegisterInsterface �Ի���

IMPLEMENT_DYNAMIC(CRegisterInsterface, CDialogEx)

CRegisterInsterface::CRegisterInsterface(CWnd* pParent /*=NULL*/)
	: CDialogEx(CRegisterInsterface::IDD, pParent)
	, username(_T(""))
	, userkey(_T(""))
	, reuserkey(_T(""))
{

}

CRegisterInsterface::~CRegisterInsterface()
{
}

void CRegisterInsterface::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, editusername);
	DDX_Control(pDX, IDC_EDIT4, editkey);
	DDX_Control(pDX, IDC_EDIT5, editrekey);
	DDX_Text(pDX, IDC_EDIT1, username);
	DDX_Text(pDX, IDC_EDIT4, userkey);
	DDX_Text(pDX, IDC_EDIT5, reuserkey);
}


BEGIN_MESSAGE_MAP(CRegisterInsterface, CDialogEx)
	ON_BN_CLICKED(IDC_BUTTON2, &CRegisterInsterface::OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON1, &CRegisterInsterface::OnBnClickedButton1)
	ON_MESSAGE(WM_SOCKET_MSG_REGISTER, CRegisterInsterface::OnRegistered)
	ON_WM_CREATE()
END_MESSAGE_MAP()


// CRegisterInsterface ��Ϣ�������


void CRegisterInsterface::OnBnClickedButton2()
{
	// TODO:  �ڴ���ӿؼ�֪ͨ����������

	ShowWindow(SW_HIDE);
	CloseWindow();

	CLoginInterface login;
	if (login.DoModal() == IDCANCEL) {
		CDialogEx::OnCancel();
	}
}

LRESULT CRegisterInsterface::OnRegistered(WPARAM wParam, LPARAM lParam)
{
	MessageBox("��ʾ", "ע��ɹ�", MB_OK);

	ShowWindow(SW_HIDE);
	CloseWindow();

	CLoginInterface login;
	login.valueusername = username;
	login.valuekey = userkey;
	if (login.DoModal() == IDCANCEL) {
		CDialogEx::OnCancel();
	}

	return 1;
}

void CRegisterInsterface::OnBnClickedButton1()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������

	UpdateData(TRUE);

	if (userkey != reuserkey) 
	{
		MessageBox("������ʾ", "����2�����벻һ��", MB_OK);
		return;
	}

	theApp.net_interface.sendregisterinfo(username, userkey);
}


int CRegisterInsterface::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialogEx::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  �ڴ������ר�õĴ�������
	theApp.main_hwnd = GetSafeHwnd();

	return 0;
}
