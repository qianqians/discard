/*
 * strtools.h
 *  Created on: 2013-4-3
 *      Author: qianqians
 * strtools
 */
#ifndef _STRTOOLS_H
#define _STRTOOLS_H

#include <string>
#include <vector>
#include <boost\cstdint.hpp>

namespace Hemsleya{
namespace strtools{

std::string str2utf8(std::string str);

std::string utf82str(std::string utf8);

std::string int2str(int32_t num);

std::string int2str(int64_t num);

int64_t str2int(std::string num_str);

std::string uint2str(uint32_t num);

std::string uint2str(uint64_t num);

uint64_t str2uint(std::string num_str);

std::string float2str(double num);

double str2float(std::string num_str);

void spitle(char ch, std::string str, std::vector<std::string> & vectorStr, int spitlecount = 0);

} //strtools
} //Hemsleya

#endif //_STRTOOLS_H