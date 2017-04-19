// ChatDlg2.cpp : 实现文件
//

#include "stdafx.h"
#include "chat.h"
#include "ChatDlg2.h"
#include "afxdialogex.h"
#include "LoginInitDlg.h"
#include "YUV2RGB.h"

#define Camera_time 1

#define MYFREEMEDIATYPE(mt)	{if ((mt).cbFormat != 0)		\
					{CoTaskMemFree((PVOID)(mt).pbFormat);	\
					(mt).cbFormat = 0;						\
					(mt).pbFormat = NULL;					\
				}											\
				if ((mt).pUnk != NULL)						\
				{											\
					(mt).pUnk->Release();					\
					(mt).pUnk = NULL;						\
				}}									


// CChatDlg2 对话框

IMPLEMENT_DYNAMIC(CChatDlg2, CDialogEx)

CChatDlg2::CChatDlg2(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_CHAT_DIALOG2, pParent)
{
}

CChatDlg2::~CChatDlg2()
{
}

void CChatDlg2::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST2, ctrluserlist);
}


BEGIN_MESSAGE_MAP(CChatDlg2, CDialogEx)
	ON_MESSAGE(WM_ADD_USER, CChatDlg2::OnAddUser)
	ON_MESSAGE(WM_REMOVE_USER, CChatDlg2::OnRemoveUser)
	ON_WM_CREATE()
	ON_BN_CLICKED(IDC_BUTTON1, &CChatDlg2::OnBnClickedButton1)
	ON_WM_CLOSE()
	ON_BN_CLICKED(IDC_BUTTON2, &CChatDlg2::OnBnClickedButton2)
	ON_WM_TIMER()
END_MESSAGE_MAP()


// CChatDlg2 消息处理程序


int CChatDlg2::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialogEx::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  在此添加您专用的创建代码
	// TODO:  在此添加您专用的创建代码
	CLoginInitDlg dlg;

	if (dlg.DoModal() == IDCANCEL)
	{
		exit(0);
	}

	theApp.main_hwnd = GetSafeHwnd();

	return 0;
}

void CChatDlg2::addUser(CString username)
{
	m.lock();
	adduserlist.push_back(username);
	m.unlock();
	
	SendMessage(WM_ADD_USER, 0, 0);
}

LRESULT CChatDlg2::OnAddUser(WPARAM wParam, LPARAM lParam)
{
	m.lock();
	for (auto user : adduserlist)
	{
		ctrluserlist.AddString(user);
	}
	adduserlist.clear();
	m.unlock();

	return 1;
}

void CChatDlg2::OnClose()
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值
	theApp.net_interface.sendexit();

	CDialogEx::OnClose();
}

void CChatDlg2::removeUser(CString username)
{
	mu_removelist.lock();
	removelist.push_back(username);
	mu_removelist.unlock();

	SendMessage(WM_REMOVE_USER, 0, 0);
}

LRESULT CChatDlg2::OnRemoveUser(WPARAM wParam, LPARAM lParam)
{
	mu_removelist.lock();
	for (auto user : removelist)
	{
		ctrluserlist.DeleteString(ctrluserlist.FindString(0, user));
	}
	removelist.clear();
	mu_removelist.unlock();

	return 1;
}

BOOL CChatDlg2::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// TODO:  在此添加额外的初始化

	CoInitialize(NULL);

	// Create the Capture Graph Builder.
	HRESULT hr = CoCreateInstance(CLSID_FilterGraph, 0, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void**)&pGraphBuilder);

	ICreateDevEnum *pDevEnum = NULL;
	IEnumMoniker *pEnum = NULL;
	hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pDevEnum));
	if (SUCCEEDED(hr))
	{
		// Create an enumerator for the category.
		hr = pDevEnum->CreateClassEnumerator(CLSID_VideoInputDeviceCategory, &pEnum, 0);
		if (hr == S_FALSE)
		{
			MessageBox("无可用视频设备", "错误提示", MB_OK);  // The category is empty. Treat as an error.
		}
		pDevEnum->Release();
	}

	IMoniker *pMoniker = NULL;
	while (pEnum->Next(1, &pMoniker, NULL) == S_OK)
	{
		IPropertyBag *pPropBag;
		HRESULT hr = pMoniker->BindToStorage(0, 0, IID_PPV_ARGS(&pPropBag));
		if (FAILED(hr))
		{
			pMoniker->Release();
			continue;
		}

		VARIANT var;
		VariantInit(&var);

		// Get description or friendly name.
		hr = pPropBag->Read(L"Description", &var, 0);
		if (FAILED(hr))
		{
			hr = pPropBag->Read(L"FriendlyName", &var, 0);
		}
		if (SUCCEEDED(hr))
		{
			printf("%S\n", var.bstrVal);
			VariantClear(&var);
		}

		hr = pPropBag->Write(L"FriendlyName", &var);

		// WaveInID applies only to audio capture devices.
		hr = pPropBag->Read(L"WaveInID", &var, 0);
		if (SUCCEEDED(hr))
		{
			printf("WaveIn ID: %d\n", var.lVal);
			VariantClear(&var);
		}

		hr = pPropBag->Read(L"DevicePath", &var, 0);
		if (SUCCEEDED(hr))
		{
			// The device path is not intended for display.
			printf("Device path: %S\n", var.bstrVal);
			VariantClear(&var);
		}

		pPropBag->Release();
		pMoniker->Release();
	}

	hr = pMoniker->BindToObject(0, 0, IID_IBaseFilter, (void**)&pCap);
	if (SUCCEEDED(hr))
	{
		hr = pGraphBuilder->AddFilter(pCap, L"Capture Filter");
	}
	else
	{
		MessageBox("无可用视频设备", "错误提示", MB_OK);
	}

	hr = CoCreateInstance(CLSID_NullRenderer, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (LPVOID*)&pNullFilter);
	hr = pGraphBuilder->AddFilter(pNullFilter, L"NullRenderer");

	hr = CoCreateInstance(CLSID_SampleGrabber, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (LPVOID*)&pSampleGrabberFilter);
	hr = pSampleGrabberFilter->QueryInterface(IID_ISampleGrabber, (void**)&pSampleGrabber);
	pGraphBuilder->AddFilter(pSampleGrabberFilter, L"Grabber");

	IEnumPins * pEnumPins;

	pCap->EnumPins(&pEnumPins);
	hr = pEnumPins->Reset();
	hr = pEnumPins->Next(1, &pCameraOutput, NULL);

	pEnumPins = NULL;
	pSampleGrabberFilter->EnumPins(&pEnumPins);
	pEnumPins->Reset();
	hr = pEnumPins->Next(1, &pGrabberInput, NULL);

	pEnumPins = NULL;
	pSampleGrabberFilter->EnumPins(&pEnumPins);
	pEnumPins->Reset();
	pEnumPins->Skip(1);
	hr = pEnumPins->Next(1, &pGrabberOutput, NULL);

	pEnumPins = NULL;
	pNullFilter->EnumPins(&pEnumPins);
	pEnum->Reset();
	hr = pEnumPins->Next(1, &pNullInputPin, NULL);

	IAMStreamConfig *iconfig = NULL;
	hr = pCameraOutput->QueryInterface(IID_IAMStreamConfig, (void**)&iconfig);

	AM_MEDIA_TYPE* pmt;
	if (iconfig->GetFormat(&pmt) != S_OK)
	{
		//printf("GetFormat Failed ! \n");
		return   false;
	}

	VIDEOINFOHEADER*   phead;
	if (pmt->formattype == FORMAT_VideoInfo)
	{
		//pmt->subtype = MEDIASUBTYPE_RGB24;
		phead = (VIDEOINFOHEADER*)pmt->pbFormat;
		width = phead->bmiHeader.biWidth;
		height = phead->bmiHeader.biHeight;
		if ((hr = iconfig->SetFormat(pmt)) != S_OK)
		{
			return   false;
		}

	}

	iconfig->Release();
	iconfig = NULL;
	MYFREEMEDIATYPE(*pmt);

	hr = pGraphBuilder->Connect(pCameraOutput, pGrabberInput);
	hr = pGraphBuilder->Connect(pGrabberOutput, pNullInputPin);

	AM_MEDIA_TYPE   mt;
	ZeroMemory(&mt, sizeof(AM_MEDIA_TYPE));
	mt.majortype = MEDIATYPE_Video;
	mt.subtype = MEDIASUBTYPE_RGB24;
	mt.formattype = FORMAT_VideoInfo;
	hr = pSampleGrabber->SetMediaType(&mt);
	MYFREEMEDIATYPE(mt);

	pSampleGrabber->SetBufferSamples(TRUE);
	pSampleGrabber->SetOneShot(TRUE);

	hr = pSampleGrabber->GetConnectedMediaType(&mt);

	buffer = new long[width*height];
	rgbbuf = new char[width*height*4];

	hr = pGraphBuilder->QueryInterface(IID_IMediaControl, (void**)&pMediaControl);
	hr = pGraphBuilder->QueryInterface(IID_IMediaEvent, (void **)&pMediaEvent);

	return TRUE;  // return TRUE unless you set the focus to a control
				  // 异常: OCX 属性页应返回 FALSE
}

void CChatDlg2::OnBnClickedButton1()
{
	// TODO: 在此添加控件通知处理程序代码

	pMediaControl->Run();

	SetTimer(Camera_time, 10, 0);
	
	theApp.videopreview->ShowWindow(SW_SHOW);
	theApp.video->ShowWindow(SW_SHOW);
}

void CChatDlg2::OnBnClickedButton2()
{
	// TODO: 在此添加控件通知处理程序代码

	KillTimer(Camera_time);

	pMediaControl->Stop();

	theApp.videopreview->ShowWindow(SW_HIDE);
	theApp.video->ShowWindow(SW_HIDE);
}


void CChatDlg2::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	switch (nIDEvent)
	{
	case Camera_time:
		{
			long evCode = 0;
			pMediaEvent->WaitForCompletion(INFINITE, &evCode);

			long size = 0;
			HRESULT hr = pSampleGrabber->GetCurrentBuffer(&size, NULL);

			memset(buffer, 0, width*height);
			hr = pSampleGrabber->GetCurrentBuffer(&size, buffer);
			
			YUV2RGB(buffer, rgbbuf, width, height, false, false);

			theApp.net_interface.sendvideo(width, height, width*height*4, rgbbuf);
			
			CDC * dc = theApp.video->GetDC();

			HBITMAP hBitmap = CreateCompatibleBitmap(dc->GetSafeHdc(), width, height);
			HBITMAP hold = (HBITMAP)dc->SelectObject(hBitmap);

			BITMAPINFO info;
			info.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
			info.bmiHeader.biWidth = width;
			info.bmiHeader.biHeight = height;
			info.bmiHeader.biPlanes = 1;
			info.bmiHeader.biBitCount = 24;
			info.bmiHeader.biCompression = BI_RGB;
			info.bmiHeader.biClrUsed = 0;
			info.bmiHeader.biSizeImage = 0;
			info.bmiHeader.biXPelsPerMeter = 0;
			info.bmiHeader.biYPelsPerMeter = 0;
			info.bmiHeader.biClrImportant = 0;
				
			StretchDIBits(dc->GetSafeHdc(), 0, 0, width, height, 0, 0, width, height, rgbbuf, &info, DIB_RGB_COLORS, SRCCOPY);

			dc->SelectObject(hold);
			DeleteObject(hBitmap);

			theApp.video->ReleaseDC(dc);
		}
		break;

	default:
		break;
	}

	CDialogEx::OnTimer(nIDEvent);
}
