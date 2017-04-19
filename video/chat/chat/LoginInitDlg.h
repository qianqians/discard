#pragma once


// CLoginInitDlg 对话框

class CLoginInitDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CLoginInitDlg)

public:
	CLoginInitDlg(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CLoginInitDlg();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_DIALOG_LOGIN2 };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()

private:
	CString valueusername;

public:
	afx_msg void OnBnClickedOk();
	afx_msg LRESULT OnLogin(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnReUsername(WPARAM wParam, LPARAM lParam);

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
};
