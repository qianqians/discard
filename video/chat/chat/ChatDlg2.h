#pragma once
#include "afxwin.h"
#include "VideoPreview.h"

#include <mutex>
#include <list>

#include <dshow.h>
#include <qedit.h>

#pragma comment(lib, "strmiids.lib")
#pragma comment(lib, "strmbase.lib")
#pragma comment(lib, "quartz.lib")
#pragma comment(lib, "winmm.lib")
#pragma comment(lib, "uuid.lib")

// CChatDlg2 对话框

class CChatDlg2 : public CDialogEx
{
	DECLARE_DYNAMIC(CChatDlg2)

public:
	CChatDlg2(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CChatDlg2();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_CHAT_DIALOG2 };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()

public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg LRESULT OnAddUser(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnRemoveUser(WPARAM wParam, LPARAM lParam);

	CListBox ctrluserlist;

public:
	void addUser(CString username);
	void removeUser(CString username);

private:
	std::mutex m;
	std::list<CString> adduserlist;

	std::mutex mu_removelist;
	std::list<CString> removelist;

	int width;
	int height;

	long * buffer;
	char * rgbbuf;

	IGraphBuilder *pGraphBuilder;
	IMediaControl *pMediaControl;
	IMediaEvent * pMediaEvent;
	IBaseFilter *pCap;
	IBaseFilter *pNullFilter;
	IBaseFilter *pSampleGrabberFilter;
	ISampleGrabber *pSampleGrabber;
	IPin * pGrabberInput;
	IPin * pGrabberOutput;
	IPin * pCameraOutput;
	IPin * pNullInputPin;

public:
	afx_msg void OnBnClickedButton1();
	afx_msg void OnClose();
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedButton2();

	afx_msg void OnTimer(UINT_PTR nIDEvent);
};
