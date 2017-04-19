
// chatDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "chatDlg.h"
#include "afxdialogex.h"
#include "LoginInitDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CchatDlg 对话框

BEGIN_DHTML_EVENT_MAP(CchatDlg)
END_DHTML_EVENT_MAP()


CchatDlg::CchatDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CchatDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CchatDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TAB1, tabctrl);
	DDX_Control(pDX, IDC_STATIC_USER_ICO, usericon);
	DDX_Control(pDX, IDC_COMBO1, statelist);
	DDX_Control(pDX, IDC_EDIT1, editctrl);
	DDX_Control(pDX, IDC_EDIT3, textshow);
	DDX_Control(pDX, IDC_EDIT2, input);
}

BEGIN_MESSAGE_MAP(CchatDlg, CDialog)
	ON_WM_CTLCOLOR()
	ON_WM_PAINT()
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONDBLCLK()
	ON_WM_LBUTTONDOWN()
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, &CchatDlg::OnTcnSelchangeTab1)
	ON_WM_CREATE()
END_MESSAGE_MAP()


// CchatDlg 消息处理程序

BOOL CchatDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// 设置此对话框的图标。  当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO:  在此添加额外的初始化代码
	CDC dc, srcdc;
	dc.CreateCompatibleDC(GetDC());

	lobby.Load(_T("./res/image/lobby.bmp"));
	srcdc.Attach(lobby.GetDC());
	bmplobby.CreateCompatibleBitmap(GetDC(), 25, 25);
	auto old = dc.SelectObject(&bmplobby);
	dc.StretchBlt(0, 0, 25, 25, &srcdc, 0, 0, lobby.GetWidth(), lobby.GetHeight(), SRCCOPY);
	srcdc.Detach();
	dc.SelectObject(old);
	dc.DeleteDC();

	user.Load(_T("./res/image/friend.bmp"));
	srcdc.Attach(user.GetDC());
	bmpuser.CreateCompatibleBitmap(GetDC(), 25, 25);
	dc.CreateCompatibleDC(GetDC());
	old = dc.SelectObject(&bmpuser);
	dc.StretchBlt(0, 0, 25, 25, &srcdc, 0, 0, user.GetWidth(), user.GetHeight(), SRCCOPY);
	srcdc.Detach();
	dc.SelectObject(old);
	dc.DeleteDC();

	group.Load(_T("./res/image/group.bmp"));
	srcdc.Attach(group.GetDC());
	bmpgroup.CreateCompatibleBitmap(GetDC(), 25, 25);
	dc.CreateCompatibleDC(GetDC());
	old = dc.SelectObject(&bmpgroup);
	dc.StretchBlt(0, 0, 25, 25, &srcdc, 0, 0, group.GetWidth(), group.GetHeight(), SRCCOPY);
	srcdc.Detach();
	dc.SelectObject(old);
	dc.DeleteDC();

	chat.Load(_T("./res/image/chat.bmp"));
	srcdc.Attach(chat.GetDC());
	bmpchat.CreateCompatibleBitmap(GetDC(), 25, 25);
	dc.CreateCompatibleDC(GetDC());
	old = dc.SelectObject(&bmpchat);
	dc.BitBlt(0, 0, 25, 25, &srcdc, 0, 0, SRCCOPY);
	srcdc.Detach();
	dc.SelectObject(old);
	dc.DeleteDC();

	imagelist.Create(25, 25, ILC_COLOR32 | ILC_MASK, 10, 10);
	imagelist.Add(&bmplobby, RGB(0, 0, 0));
	imagelist.Add(&bmpuser, RGB(0, 0, 0));
	imagelist.Add(&bmpgroup, RGB(0, 0, 0));
	imagelist.Add(&bmpchat, RGB(0, 0, 0));

	tabctrl.SetImageList(&imagelist);

	tabctrl.InsertItem(0, _T(""), 0);
	tabctrl.InsertItem(1, _T(""), 1);
	tabctrl.InsertItem(2, _T(""), 2);
	tabctrl.InsertItem(3, _T(""), 3);

	min.Load(_T("./res/image/min.bmp"));
	max.Load(_T("./res/image/max.bmp"));
	close.Load(_T("./res/image/close.bmp"));

	RECT rect;
	tabctrl.GetWindowRect(&rect);

	lobbypanel.Create(IDD_DIALOG_LOBBY, this);
	lobbypanel.MoveWindow(rect.left - 2, rect.top + 28, rect.right - 7, rect.bottom - rect.top - 33);
	lobbypanel.ShowWindow(SW_HIDE);

	charpanel.Create(IDD_DIALOG_CHAT, this);
	charpanel.MoveWindow(rect.left - 2, rect.top + 28, rect.right - 7, rect.bottom - rect.top - 33);
	charpanel.ShowWindow(SW_HIDE);

	userpanel.Create(IDD_DIALOG_USER, this);
	userpanel.MoveWindow(rect.left - 2, rect.top + 28, rect.right - 7, rect.bottom - rect.top - 33);
	userpanel.ShowWindow(SW_HIDE);

	grouppanel.Create(IDD_DIALOG_GROUP, this);
	grouppanel.MoveWindow(rect.left - 2, rect.top + 28, rect.right - 7, rect.bottom - rect.top - 33);
	grouppanel.ShowWindow(SW_HIDE);

	tabctrl.SetCurSel(0);

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。  对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CchatDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();

		CDC * pDC = GetDC();
		CPen p;
		p.CreatePen(PS_SOLID, 1, RGB(0, 0, 0));
		auto old = pDC->SelectObject(&p);

		pDC->SetBkMode(TRANSPARENT);
		pDC->TextOut(0, 0, "chat");
		pDC->SelectObject(old);
		
		p.DeleteObject();

		RECT r;
		GetClientRect(&r);
		auto left = r.right - 3*23;
		minr.top = 0;
		minr.left = left;
		minr.right = left + 20;
		minr.bottom = 20;

		CDC srcdc;
		srcdc.Attach(min.GetDC());
		auto ret = pDC->TransparentBlt(left, 0, 20, 20, &srcdc, 0, 0, min.GetWidth(), min.GetHeight(), RGB(0, 0, 0));
		srcdc.Detach();

		left = r.right - 2 * 23;
		maxr.top = 0;
		maxr.left = left;
		maxr.right = left + 20;
		maxr.bottom = 20;

		srcdc.Attach(max.GetDC());
		ret = pDC->TransparentBlt(left, 0, 20, 20, &srcdc, 0, 0, max.GetWidth(), max.GetHeight(), RGB(0, 0, 0));
		srcdc.Detach();

		left = r.right - 23;
		closer.top = 0;
		closer.left = left;
		closer.right = left + 20;
		closer.bottom = 20;

		srcdc.Attach(close.GetDC());
		ret = pDC->TransparentBlt(left, 0, 20, 20, &srcdc, 0, 0, max.GetWidth(), max.GetHeight(), RGB(0, 0, 0));
		srcdc.Detach();

		ReleaseDC(pDC);
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CchatDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



HBRUSH CchatDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	// TODO:  在此更改 DC 的任何特性
	switch (pWnd->GetDlgCtrlID())
	{

	default:
		break;
	}

	// TODO:  如果默认的不是所需画笔，则返回另一个画笔
	return hbr;
}


void CchatDlg::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	if (point.x > minr.left && point.x < minr.right && point.y > minr.top && point.y < minr.bottom){

	}

	CDialog::OnMouseMove(nFlags, point);
}


void CchatDlg::OnLButtonDblClk(UINT nFlags, CPoint point)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	if (point.x > minr.left && point.x < minr.right && point.y > minr.top && point.y < minr.bottom){
		OnMimBtnClk(nFlags, point);
	}

	CDialog::OnLButtonDblClk(nFlags, point);
}

void CchatDlg::OnMimBtnClk(UINT nFlags, CPoint point){
	//ShowWindow(SW_MINIMIZE);
}

void CchatDlg::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	if (point.x > minr.left && point.x < minr.right && point.y > minr.top && point.y < minr.bottom){
		OnMimBtnDown(nFlags, point);
	}
	else if (point.x > closer.left && point.x < closer.right && point.y > closer.top && point.y < closer.bottom){
		OnCloserBtnDown(nFlags, point);
	}

	CDialog::OnLButtonDown(nFlags, point);
}


void CchatDlg::OnMimBtnDown(UINT nFlags, CPoint point){
	ShowWindow(SW_MINIMIZE);
}

void CchatDlg::OnCloserBtnDown(UINT nFlags, CPoint point){
	exit(0);
	//CDialog::OnCancel();
}

void CchatDlg::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO:  在此添加控件通知处理程序代码
	int i = tabctrl.GetCurSel();

	switch (i)
	{
	case 0:
		OnLobbyClk();
		break;
	case 1:
		OnUserClk();
		break;
	case 2:
		OnGroupClk();
		break;
	case 3:
		OnChatClk();
		break;

	default:
		break;
	}

	*pResult = 0;
}

void CchatDlg::OnLobbyClk(){
	lobbypanel.ShowWindow(SW_SHOW);
	charpanel.ShowWindow(SW_HIDE);
	userpanel.ShowWindow(SW_HIDE);
	grouppanel.ShowWindow(SW_HIDE);
}

void CchatDlg::OnUserClk(){
	lobbypanel.ShowWindow(SW_HIDE);
	charpanel.ShowWindow(SW_HIDE);
	userpanel.ShowWindow(SW_SHOW);
	grouppanel.ShowWindow(SW_HIDE);
}

void CchatDlg::OnGroupClk(){
	lobbypanel.ShowWindow(SW_HIDE);
	charpanel.ShowWindow(SW_HIDE);
	userpanel.ShowWindow(SW_HIDE);
	grouppanel.ShowWindow(SW_SHOW);
}

void CchatDlg::OnChatClk(){
	lobbypanel.ShowWindow(SW_HIDE);
	charpanel.ShowWindow(SW_SHOW);
	userpanel.ShowWindow(SW_HIDE);
	grouppanel.ShowWindow(SW_SHOW);
}

int CchatDlg::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  在此添加您专用的创建代码
	CLoginInitDlg dlg;

	if (dlg.DoModal() == IDCANCEL)
	{
		exit(0);
	}

	return 0;
}
