#pragma once
#include "afxwin.h"


// CLoginInterface �Ի���

class CLoginInterface : public CDialogEx
{
	DECLARE_DYNAMIC(CLoginInterface)

public:
	CLoginInterface(CWnd* pParent = NULL);   // ��׼���캯��
	virtual ~CLoginInterface();

// �Ի�������
	enum { IDD = IDD_DIALOG_LOGIN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	DECLARE_MESSAGE_MAP()

private:
	CEdit editusername;
	CEdit editkey;
	CStatic username;
	CStatic key;


public:
	afx_msg void OnClose();
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedButton2();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);

public:
	CString valueusername;
	CString valuekey;

};
