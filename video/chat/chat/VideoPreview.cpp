// VideoPreview.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "VideoPreview.h"
#include "afxdialogex.h"


// CVideoPreview 对话框

IMPLEMENT_DYNAMIC(CVideoPreview, CDialogEx)

CVideoPreview::CVideoPreview(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_DIALOG_VIDEOPREVIEW, pParent)
{
	ismove = false;
}

CVideoPreview::~CVideoPreview()
{
}

void CVideoPreview::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CVideoPreview, CDialogEx)
	ON_WM_CREATE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_NCLBUTTONDOWN()
	ON_WM_NCLBUTTONUP()
END_MESSAGE_MAP()


// CVideoPreview 消息处理程序


int CVideoPreview::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialogEx::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  在此添加您专用的创建代码

	return 0;
}


void CVideoPreview::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	CDialogEx::OnLButtonDown(nFlags, point);

	ismove = true;

	//CDialogEx::OnNcLButtonDown(nFlags, point);

	PostMessage(WM_NCLBUTTONDOWN, HTCAPTION, MAKELPARAM(point.x, point.y));
}


void CVideoPreview::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	//CDialogEx::OnLButtonUp(nFlags, point);

	ismove = false;

	CDialogEx::OnNcLButtonUp(nFlags, point);
}


void CVideoPreview::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	if (ismove)
	{
		CDialogEx::OnNcMouseMove(nFlags, point);
	}
	else
	{
		CDialogEx::OnMouseMove(nFlags, point);
	}
}


void CVideoPreview::OnNcLButtonDown(UINT nHitTest, CPoint point)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	CDialogEx::OnNcLButtonDown(nHitTest, point);
}


void CVideoPreview::OnNcLButtonUp(UINT nHitTest, CPoint point)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	CDialogEx::OnNcLButtonUp(nHitTest, point);
}
