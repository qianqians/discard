#include "stdafx.h"
#include "YUV2RGB.h"

//////////////////////////////////////////////////////////////////////////
// YUV2RGB
// pYUV			point to the YUV data
// pRGB			point to the RGB data
// width		width of the picture
// height		height of the picture
// alphaYUV		is there an alpha channel in YUV
// alphaRGB		is there an alpha channel in RGB
//////////////////////////////////////////////////////////////////////////
int YUV2RGB(void* pYUV, void* pRGB, int width, int height, bool alphaYUV, bool alphaRGB)
{
	if (NULL == pYUV)
	{
		return -1;
	}
	unsigned char* pYUVData = (unsigned char *)pYUV;
	unsigned char* pRGBData = (unsigned char *)pRGB;
	if (NULL == pRGBData)
	{
		if (alphaRGB)
		{
			pRGBData = new unsigned char[width*height * 4];
		}
		else
			pRGBData = new unsigned char[width*height * 3];
	}
	int Y1, U1, V1, Y2, alpha1, alpha2, R1, G1, B1, R2, G2, B2;
	int C1, D1, E1, C2;
	if (alphaRGB)
	{
		if (alphaYUV)
		{
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					Y1 = *(pYUVData + i*width * 3 + j * 6);
					U1 = *(pYUVData + i*width * 3 + j * 6 + 1);
					Y2 = *(pYUVData + i*width * 3 + j * 6 + 2);
					V1 = *(pYUVData + i*width * 3 + j * 6 + 3);
					alpha1 = *(pYUVData + i*width * 3 + j * 6 + 4);
					alpha2 = *(pYUVData + i*width * 3 + j * 6 + 5);
					C1 = Y1 - 16;
					C2 = Y2 - 16;
					D1 = U1 - 128;
					E1 = V1 - 128;
					R1 = ((298 * C1 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C1 + 409 * E1 + 128) >> 8);
					G1 = ((298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8);
					B1 = ((298 * C1 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C1 + 516 * D1 + 128) >> 8);
					R2 = ((298 * C2 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C2 + 409 * E1 + 128) >> 8);
					G2 = ((298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8);
					B2 = ((298 * C2 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C2 + 516 * D1 + 128) >> 8);
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 2) = R1<0 ? 0 : R1;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 1) = G1<0 ? 0 : G1;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8) = B1<0 ? 0 : B1;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 3) = alpha1;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 6) = R2<0 ? 0 : R2;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 5) = G2<0 ? 0 : G2;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 4) = B2<0 ? 0 : B2;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 7) = alpha2;
				}
			}
		}
		else
		{
			int alpha = 255;
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					Y1 = *(pYUVData + i*width * 2 + j * 4);
					U1 = *(pYUVData + i*width * 2 + j * 4 + 1);
					Y2 = *(pYUVData + i*width * 2 + j * 4 + 2);
					V1 = *(pYUVData + i*width * 2 + j * 4 + 3);
					C1 = Y1 - 16;
					C2 = Y2 - 16;
					D1 = U1 - 128;
					E1 = V1 - 128;
					R1 = ((298 * C1 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C1 + 409 * E1 + 128) >> 8);
					G1 = ((298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8);
					B1 = ((298 * C1 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C1 + 516 * D1 + 128) >> 8);
					R2 = ((298 * C2 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C2 + 409 * E1 + 128) >> 8);
					G2 = ((298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8);
					B2 = ((298 * C2 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C2 + 516 * D1 + 128) >> 8);
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 2) = R1<0 ? 0 : R1;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 1) = G1<0 ? 0 : G1;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8) = B1<0 ? 0 : B1;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 3) = alpha;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 6) = R2<0 ? 0 : R2;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 5) = G2<0 ? 0 : G2;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 4) = B2<0 ? 0 : B2;
					*(pRGBData + (height - i - 1)*width * 4 + j * 8 + 7) = alpha;
				}
			}
		}
	}
	else
	{
		if (alphaYUV)
		{
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					Y1 = *(pYUVData + i*width * 3 + j * 4);
					U1 = *(pYUVData + i*width * 3 + j * 4 + 1);
					Y2 = *(pYUVData + i*width * 3 + j * 4 + 2);
					V1 = *(pYUVData + i*width * 3 + j * 4 + 3);
					C1 = Y1 - 16;
					C2 = Y2 - 16;
					D1 = U1 - 128;
					E1 = V1 - 128;
					R1 = ((298 * C1 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C1 + 409 * E1 + 128) >> 8);
					G1 = ((298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8);
					B1 = ((298 * C1 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C1 + 516 * D1 + 128) >> 8);
					R2 = ((298 * C2 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C2 + 409 * E1 + 128) >> 8);
					G2 = ((298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8);
					B2 = ((298 * C2 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C2 + 516 * D1 + 128) >> 8);
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 2) = R1<0 ? 0 : R1;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 1) = G1<0 ? 0 : G1;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6) = B1<0 ? 0 : B1;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 5) = R2<0 ? 0 : R2;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 4) = G2<0 ? 0 : G2;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 3) = B2<0 ? 0 : B2;
				}
			}
		}
		else
		{
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					Y1 = *(pYUVData + i*width * 2 + j * 4);
					U1 = *(pYUVData + i*width * 2 + j * 4 + 1);
					Y2 = *(pYUVData + i*width * 2 + j * 4 + 2);
					V1 = *(pYUVData + i*width * 2 + j * 4 + 3);
					C1 = Y1 - 16;
					C2 = Y2 - 16;
					D1 = U1 - 128;
					E1 = V1 - 128;
					R1 = ((298 * C1 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C1 + 409 * E1 + 128) >> 8);
					G1 = ((298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C1 - 100 * D1 - 208 * E1 + 128) >> 8);
					B1 = ((298 * C1 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C1 + 516 * D1 + 128) >> 8);
					R2 = ((298 * C2 + 409 * E1 + 128) >> 8>255 ? 255 : (298 * C2 + 409 * E1 + 128) >> 8);
					G2 = ((298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8>255 ? 255 : (298 * C2 - 100 * D1 - 208 * E1 + 128) >> 8);
					B2 = ((298 * C2 + 516 * D1 + 128) >> 8>255 ? 255 : (298 * C2 + 516 * D1 + 128) >> 8);
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 2) = R1<0 ? 0 : R1;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 1) = G1<0 ? 0 : G1;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6) = B1<0 ? 0 : B1;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 5) = R2<0 ? 0 : R2;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 4) = G2<0 ? 0 : G2;
					*(pRGBData + (height - i - 1)*width * 3 + j * 6 + 3) = B2<0 ? 0 : B2;
				}
			}
		}
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////
// RGB2YUV
// pRGB			point to the RGB data
// pYUV			point to the YUV data
// width		width of the picture
// height		height of the picture
// alphaYUV		is there an alpha channel in YUV
// alphaRGB		is there an alpha channel in RGB
//////////////////////////////////////////////////////////////////////////
int RGB2YUV(void* pRGB, void* pYUV, int width, int height, bool alphaYUV, bool alphaRGB)
{
	if (NULL == pRGB)
	{
		return -1;
	}
	unsigned char* pRGBData = (unsigned char *)pRGB;
	unsigned char* pYUVData = (unsigned char *)pYUV;
	if (NULL == pYUVData)
	{
		if (alphaYUV)
		{
			pYUVData = new unsigned char[width*height * 3];
		}
		else
			pYUVData = new unsigned char[width*height * 2];
	}
	int R1, G1, B1, R2, G2, B2, Y1, U1, Y2, V1;
	int alpha1, alpha2;
	if (alphaYUV)
	{
		if (alphaRGB)
		{
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					B1 = *(pRGBData + (height - i - 1)*width * 4 + j * 8);
					G1 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 1);
					R1 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 2);
					alpha1 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 3);
					B2 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 4);
					G2 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 5);
					R2 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 6);
					alpha2 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 7);
					Y1 = (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16) > 255 ? 255 : (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16);
					U1 = ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128);
					Y2 = (((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16)>255 ? 255 : ((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16;
					V1 = ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128);
					*(pYUVData + i*width * 3 + j * 6) = Y1;
					*(pYUVData + i*width * 3 + j * 6 + 1) = U1;
					*(pYUVData + i*width * 3 + j * 6 + 2) = Y2;
					*(pYUVData + i*width * 3 + j * 6 + 3) = V1;
					*(pYUVData + i*width * 3 + j * 6 + 4) = alpha1;
					*(pYUVData + i*width * 3 + j * 6 + 5) = alpha2;
				}
			}
		}
		else
		{
			unsigned char alpha = 255;
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					B1 = *(pRGBData + (height - i - 1)*width * 3 + j * 6);
					G1 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 1);
					R1 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 2);
					B2 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 3);
					G2 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 4);
					R2 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 5);
					Y1 = ((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16;
					U1 = ((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8 + (-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8) / 2 + 128;
					Y2 = ((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16;
					V1 = ((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8 + (112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8) / 2 + 128;
					Y1 = (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16) > 255 ? 255 : (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16);
					U1 = ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128);
					Y2 = (((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16)>255 ? 255 : ((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16;
					V1 = ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128);
					*(pYUVData + i*width * 3 + j * 6) = Y1;
					*(pYUVData + i*width * 3 + j * 6 + 1) = U1;
					*(pYUVData + i*width * 3 + j * 6 + 2) = Y2;
					*(pYUVData + i*width * 3 + j * 6 + 3) = V1;
					*(pYUVData + i*width * 3 + j * 6 + 4) = alpha;
					*(pYUVData + i*width * 3 + j * 6 + 5) = alpha;
				}
			}
		}
	}
	else
	{
		if (alphaRGB)
		{
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					B1 = *(pRGBData + (height - i - 1)*width * 4 + j * 8);
					G1 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 1);
					R1 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 2);
					B2 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 4);
					G2 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 5);
					R2 = *(pRGBData + (height - i - 1)*width * 4 + j * 8 + 6);
					Y1 = (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16) > 255 ? 255 : (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16);
					U1 = ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128);
					Y2 = (((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16)>255 ? 255 : ((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16;
					V1 = ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128);
					*(pYUVData + i*width * 2 + j * 4) = Y1;
					*(pYUVData + i*width * 2 + j * 4 + 1) = U1;
					*(pYUVData + i*width * 2 + j * 4 + 2) = Y2;
					*(pYUVData + i*width * 2 + j * 4 + 3) = V1;
				}
			}
		}
		else
		{
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					B1 = *(pRGBData + (height - i - 1)*width * 3 + j * 6);
					G1 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 1);
					R1 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 2);
					B2 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 3);
					G2 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 4);
					R2 = *(pRGBData + (height - i - 1)*width * 3 + j * 6 + 5);
					Y1 = (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16) > 255 ? 255 : (((66 * R1 + 129 * G1 + 25 * B1 + 128) >> 8) + 16);
					U1 = ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((-38 * R1 - 74 * G1 + 112 * B1 + 128) >> 8) + ((-38 * R2 - 74 * G2 + 112 * B2 + 128) >> 8)) / 2 + 128);
					Y2 = (((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16)>255 ? 255 : ((66 * R2 + 129 * G2 + 25 * B2 + 128) >> 8) + 16;
					V1 = ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128)>255 ? 255 : ((((112 * R1 - 94 * G1 - 18 * B1 + 128) >> 8) + ((112 * R2 - 94 * G2 - 18 * B2 + 128) >> 8)) / 2 + 128);
					*(pYUVData + i*width * 2 + j * 4) = Y1;
					*(pYUVData + i*width * 2 + j * 4 + 1) = U1;
					*(pYUVData + i*width * 2 + j * 4 + 2) = Y2;
					*(pYUVData + i*width * 2 + j * 4 + 3) = V1;
				}
			}
		}
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////
// pGBYUV			point to the background YUV data
// pFGYUV			point to the foreground YUV data
// width			width of the picture
// height			height of the picture
// alphaBG			is there an alpha channel in background YUV data
// alphaFG			is there an alpha channel in fourground YUV data
//////////////////////////////////////////////////////////////////////////
int YUVBlending(void* pBGYUV, void* pFGYUV, int width, int height, bool alphaBG, bool alphaFG)
{
	if (NULL == pBGYUV || NULL == pFGYUV)
	{
		return -1;
	}
	unsigned char* pBGData = (unsigned char*)pBGYUV;
	unsigned char* pFGData = (unsigned char*)pFGYUV;
	if (!alphaFG)
	{
		if (!alphaBG)
		{
			memcpy(pBGData, pFGData, width*height * 2);
		}
		else
		{
			for (int i = 0; i<height; ++i)
			{
				for (int j = 0; j<width / 2; ++j)
				{
					*(pBGData + i*width * 2 + j * 4) = *(pFGData + i*width * 2 + j * 4);
					*(pBGData + i*width * 2 + j * 4 + 1) = *(pFGData + i*width * 2 + j * 4 + 1);
					*(pBGData + i*width * 2 + j * 4 + 2) = *(pFGData + i*width * 2 + j * 4 + 2);
					*(pBGData + i*width * 2 + j * 4 + 3) = *(pFGData + i*width * 2 + j * 4 + 3);
				}
			}
		}
	}
	int Y11, U11, V11, Y12, Y21, U21, V21, Y22;
	int alpha1, alpha2;
	if (!alphaBG)
	{
		for (int i = 0; i<height; ++i)
		{
			for (int j = 0; j<width / 2; ++j)
			{
				Y11 = *(pBGData + i*width * 2 + j * 4);
				U11 = *(pBGData + i*width * 2 + j * 4 + 1);
				Y12 = *(pBGData + i*width * 2 + j * 4 + 2);
				V11 = *(pBGData + i*width * 2 + j * 4 + 3);

				Y21 = *(pFGData + i*width * 3 + j * 6);
				U21 = *(pFGData + i*width * 3 + j * 6 + 1);
				Y22 = *(pFGData + i*width * 3 + j * 6 + 2);
				V21 = *(pFGData + i*width * 3 + j * 6 + 3);
				alpha1 = *(pFGData + i*width * 3 + j * 6 + 4);
				alpha2 = *(pFGData + i*width * 3 + j * 6 + 5);

				*(pBGData + i*width * 2 + j * 4) = (Y21 - 16)*alpha1 / 255 + (Y11 - 16)*(255 - alpha1) / 255 + 16;
				*(pBGData + i*width * 2 + j * 4 + 1) = ((U21 - 128)*alpha1 / 255 + (U11 - 128)*(255 - alpha1) / 255 + (U21 - 128)*alpha2 / 255 + (U11 - 128)*(255 - alpha2) / 255) / 2 + 128;
				*(pBGData + i*width * 2 + j * 4 + 3) = ((V21 - 128)*alpha1 / 255 + (V11 - 128)*(255 - alpha1) / 255 + (V21 - 128)*alpha2 / 255 + (V11 - 128)*(255 - alpha2) / 255) / 2 + 128;
				*(pBGData + i*width * 2 + j * 4 + 2) = (Y22 - 16)*alpha2 / 255 + (Y12 - 16)*(255 - alpha2) / 255 + 16;
			}
		}
	}
	else
	{
		for (int i = 0; i<height; ++i)
		{
			for (int j = 0; j<width / 2; ++j)
			{
				Y11 = *(pBGData + i*width * 3 + j * 6);
				U11 = *(pBGData + i*width * 3 + j * 6 + 1);
				Y12 = *(pBGData + i*width * 3 + j * 6 + 2);
				V11 = *(pBGData + i*width * 3 + j * 6 + 3);

				Y21 = *(pFGData + i*width * 3 + j * 6);
				U21 = *(pFGData + i*width * 3 + j * 6 + 1);
				Y22 = *(pFGData + i*width * 3 + j * 6 + 2);
				V21 = *(pFGData + i*width * 3 + j * 6 + 3);
				alpha1 = *(pFGData + i*width * 3 + j * 6 + 4);
				alpha2 = *(pFGData + i*width * 3 + j * 6 + 5);

				*(pBGData + i*width * 3 + j * 6) = (Y21 - 16)*alpha1 / 255 + (Y11 - 16)*(255 - alpha1) / 255 + 16;
				*(pBGData + i*width * 3 + j * 6 + 1) = ((U21 - 128)*alpha1 / 255 + (U11 - 128)*(255 - alpha1) / 255 + (U21 - 128)*alpha2 / 255 + (U11 - 128)*(255 - alpha2) / 255) / 2 + 128;
				*(pBGData + i*width * 3 + j * 6 + 3) = ((V21 - 128)*alpha1 / 255 + (V11 - 128)*(255 - alpha1) / 255 + (V21 - 128)*alpha2 / 255 + (V11 - 128)*(255 - alpha2) / 255) / 2 + 128;
				*(pBGData + i*width * 3 + j * 6 + 2) = (Y22 - 16)*alpha2 / 255 + (Y12 - 16)*(255 - alpha2) / 255 + 16;
			}
		}
	}
	return 0;
}
