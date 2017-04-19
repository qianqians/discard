// VideoPreview.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "chat.h"
#include "VideoPreview.h"
#include "afxdialogex.h"


// CVideoPreview �Ի���

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


// CVideoPreview ��Ϣ�������


int CVideoPreview::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialogEx::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  �ڴ������ר�õĴ�������

	return 0;
}


void CVideoPreview::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: �ڴ������Ϣ�����������/�����Ĭ��ֵ

	CDialogEx::OnLButtonDown(nFlags, point);

	ismove = true;

	//CDialogEx::OnNcLButtonDown(nFlags, point);

	PostMessage(WM_NCLBUTTONDOWN, HTCAPTION, MAKELPARAM(point.x, point.y));
}


void CVideoPreview::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO: �ڴ������Ϣ�����������/�����Ĭ��ֵ

	//CDialogEx::OnLButtonUp(nFlags, point);

	ismove = false;

	CDialogEx::OnNcLButtonUp(nFlags, point);
}


void CVideoPreview::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: �ڴ������Ϣ�����������/�����Ĭ��ֵ

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
	// TODO: �ڴ������Ϣ�����������/�����Ĭ��ֵ

	CDialogEx::OnNcLButtonDown(nHitTest, point);
}


void CVideoPreview::OnNcLButtonUp(UINT nHitTest, CPoint point)
{
	// TODO: �ڴ������Ϣ�����������/�����Ĭ��ֵ

	CDialogEx::OnNcLButtonUp(nHitTest, point);
}
