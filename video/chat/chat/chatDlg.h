
// chatDlg.h : 头文件
//

#pragma once
#include "afxcmn.h"
#include "afxwin.h"


// CchatDlg 对话框
class CchatDlg : public CDialog
{
// 构造
public:
	CchatDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_CHAT_DIALOG, IDH = IDR_HTML_CHAT_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持

// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
	DECLARE_DHTML_EVENT_MAP()

public:
	afx_msg void OnEnChangeEdit2();
	afx_msg void OnEnChangeEdit3();

private:
	CTabCtrl tabctrl;
	CStatic usericon;
	CComboBox statelist;
	CEdit editctrl;
	CEdit textshow;
	CEdit input;

	CImageList imagelist;
	CImage lobby;
	CBitmap bmplobby;
	CImage user;
	CBitmap bmpuser;
	CImage group;
	CBitmap bmpgroup;
	CImage chat;
	CBitmap bmpchat;
	
	CImage min;
	CBitmap bmpmin;
	CImage max;
	CBitmap bmpmax;
	CImage close;
	CBitmap bmpclose;

	RECT minr;
	RECT maxr;
	RECT closer;

	CLobbyPanel lobbypanel;
	CChatPanel charpanel;
	CUserPanel userpanel;
	CGroupPanel grouppanel;

public:
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);

private:
public:
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);

	afx_msg void OnMimBtnClk(UINT nFlags, CPoint point);

	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnMimBtnDown(UINT nFlags, CPoint point);
	afx_msg void OnCloserBtnDown(UINT nFlags, CPoint point);

	afx_msg void OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnLobbyClk();
	afx_msg void OnUserClk();
	afx_msg void OnGroupClk();
	afx_msg void OnChatClk();

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
};
