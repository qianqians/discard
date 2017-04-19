#pragma once

int YUV2RGB(void* pYUV, void* pRGB, int width, int height, bool alphaYUV, bool alphaRGB);
int RGB2YUV(void* pRGB, void* pYUV, int width, int height, bool alphaYUV, bool alphaRGB);
int YUVBlending(void* pBGYUV, void* pFGYUV, int width, int height, bool alphaBG, bool alphaFG);