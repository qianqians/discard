/*
 * strtools.cpp
 *  Created on: 2013-4-13
 *      Author: qianqians
 * strtools
 */
#include "strtools.h"

namespace Hemsleya{
namespace strtools{

#ifdef _WINDOWS

#include <Windows.h>

std::string str2othercp(std::string str, uint32_t incp, uint32_t outcp){
	std::string ret;
	wchar_t * ubuff = 0;
	char * sbuff = 0;

	do{
		int wlen = MultiByteToWideChar(incp, 0, str.c_str(), -1, 0, 0);
		if (wlen <= 0){
			break;
		}
		ubuff = new wchar_t[wlen+1];
		int rwlen = MultiByteToWideChar(incp, 0, str.c_str(), str.size(), ubuff, wlen);
		ubuff[wlen] = L'\0';
		if (rwlen <= 0){
			break;
		}

		int slen = WideCharToMultiByte(outcp, 0, ubuff, -1, 0, 0, 0, false);
		if (slen <= 0){
			break;
		}
		sbuff = new char[slen+1];
		sbuff[slen] = '\0';
		int rslen = WideCharToMultiByte(outcp, 0, ubuff, rwlen, sbuff, slen, 0, false);
		if (slen <= 0){
			break;
		}

		ret = sbuff;

	}while(0);

	if (ubuff != 0){
		delete ubuff;
	}
	if (sbuff != 0){
		delete sbuff;
	}

	return ret;
}

std::string str2utf8(std::string str){
	UINT acp = GetACP();
	return str2othercp(str, acp, CP_UTF8);
}

std::string utf82str(std::string utf8){
	UINT acp = GetACP();
	return str2othercp(utf8, CP_UTF8, acp);
}

std::string int2str(int32_t num){
	std::string strNum = "0";
	if (num < 0){
		strNum = "-";
	}

	int modulo = 0;
	do{
		modulo = num%10;
		num = num/10;
		
		strNum.insert(strNum.begin(), '0'+modulo);
			
	}while(num != 0);
	if (num > 0){
		strNum.erase(strNum.begin());
	}

	return strNum;
}

std::string int2str(int64_t num){
	std::string strNum = "0";
	if (num < 0){
		strNum = "-";
	}

	int modulo = 0;
	do{
		modulo = num%10;
		num = num/10;
		
		strNum.insert(strNum.begin(), '0'+modulo);
			
	}while(num != 0);
	if (num > 0){
		strNum.erase(strNum.begin());
	}

	return strNum;
}

int64_t str2int(std::string num_str){
	int64_t ret = 0;
	
	unsigned int i = 0;
	if (num_str.front() == '-'){
		i = 1;
	}
	for( ; i < num_str.size(); i++){
		ret *= 10;
		ret += num_str.at(i) - '0';
	}
	if (num_str.front() == '-'){
		ret = 0 - ret;
	}

	return ret;
}

std::string uint2str(uint32_t num){
	std::string strNum = "0";
	uint32_t quotient = 0, modulo = 0;
	do{
		quotient = num/10;
		modulo = num%10;
		
		strNum.insert(strNum.begin(), (char)('0'+modulo));
			
	}while(quotient != 0);
	strNum.erase(strNum.begin());

	return strNum;
}

std::string uint2str(uint64_t num){
	std::string strNum = "0";
	uint64_t quotient = 0, modulo = 0;
	do{
		quotient = num/10;
		modulo = num%10;
		
		strNum.insert(strNum.begin(), (char)('0'+modulo));
			
	}while(quotient != 0);
	strNum.erase(strNum.begin());

	return strNum;
}

uint64_t str2uint(std::string num_str){
	uint64_t ret = 0;
	for(unsigned int i = 0; i < num_str.size(); i++){
		ret *= 10;
		ret += num_str.at(i) - '0';
	}

	return ret;
}

std::string float2str(double num){
	char buff[256];
	sprintf_s(buff, 256, "%f", num);

	return buff;
}

double str2float(std::string num_str){
	double ret = 0.0;

	sscanf_s(num_str.c_str(), "%f", ret);

	return ret;
}

#endif //_WINDOWS

void spitle(char ch, std::string str, std::vector<std::string> & vectorStr, int spitlecount){
	int oldpos = 0, pos = 0, count = 0;
	do{
		pos = str.find(ch, oldpos);
		vectorStr.push_back(str.substr(oldpos, pos));
		oldpos = pos+1;
		count++;
	}while(pos != std::string::npos || oldpos != str.size() || count == spitlecount);
}


} //strtools
} //Hemsleya