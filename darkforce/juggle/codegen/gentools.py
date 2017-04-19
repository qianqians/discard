# 2014-12-24
# build by qianqians
# codegenclient

import os
import argvs

def templatetype(type):
    indexb = 0
    indexe = 0
    count = 0
    for i in xrange(len(type)):
        if type[i] == '<':
            indexb = i
            count = count + 1
        if type[i] == '>':
            count = count - 1
            if count == 0:
                indexe = i
                break
    return indexb, indexe, type[indexb + 1 : indexe]

def typetocpptype(type):
    if type == 'int':
        return 'int64_t'
    if type == 'float':
        return 'double'
    if type == 'string':
        return 'std::string'
    if type.find('array') != -1:
        indexb, indexe, _templatetype = templatetype(type)
        return 'std::vector<' + typetocpptype(_templatetype) + '>  '
    if type == 'table':
        return 'boost::shared_ptr<boost::unordered_map<std::string, boost::any> > '
    return type

def maketypegetvalue(type):
    if type == 'int':
        return 'int64_t'
    if type == 'float':
        return 'double'
    if type == 'bool':
        return 'bool'
    if type == 'string' or type == 'std::string':
        return 'std::string'
    if type.find('array<') != -1:
        indexb, indexe, _templatetype = templatetype(type)
        return 'std::vector<' + typetocpptype(_templatetype) + '>  '
    if type == 'table':
        return 'boost::shared_ptr<boost::unordered_map<std::string, boost::any> > '

def  maketype(type, name):
    if type == 'int':
        return '	n.' + name + ' = ' + 'boost::any_cast<int64_t>((*r)[\"ret\"][' + name + ']);'
    if type == 'float':
        return '	n.' + name + ' = ' + 'boost::any_cast<double>((*r)[\"ret\"][' + name + ']);'
    if type == 'bool':
        return '	n.' + name + ' = ' + 'boost::any_cast<bool>((*r)[\"ret\"][' + name + ']);'
    if type == 'string' or type == 'std::string':
        return '	n.' + name + ' = ' + 'boost::any_cast<std::string>((*r)[\"ret\"][' + name + ']);'
    if type == 'array':
        code = '	for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"]).size(); i++){\n'
        code += '		n.' + name + '.push_back(boost::any_cast<' + maketypegetvalue(type) + '>((*r)[\"ret\"][i]));'
        code += '}\n'
        return code
    if type == 'table':
         return '	n.' + name + ' = ' + 'boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)[\"ret\"][' + name + ']);'

def unpackstruct(type, struct):
    for k, v in struct:
        for name, define in v:
            if type == name:
                code = '	name n;\n'
                for argv in define:
                    code += maketype(argv[0], argv[1])
                code += '	return n;'
                return code

def makearray(type):
    indexb, indexe, _templatetype = templatetype(type)
    if _templatetype == 'int':
        return '        std::vector<int64_t> ret;\n' \
               '        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"]).size(); i++){\n' \
               '	        ret.push_back(boost::any_cast<int64_t>(boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"])[i]));\n' \
               '        }\n'
    if _templatetype == 'float':
        return '        std::vector<double> ret;\n' \
               '        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"]).size(); i++){\n' \
               '	        ret.push_back(boost::any_cast<double>(boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"])[i]));\n' \
               '        }\n'
    if _templatetype == 'bool':
        return '        std::vector<bool> ret;\n' \
               '        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"]).size(); i++){\n' \
               '	        ret.push_back(boost::any_cast<bool>(boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"])[i]));\n' \
               '        }\n'
    if _templatetype == 'string' or type == 'std::string':
        return '        std::vector<std::string> ret;\n' \
               '        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"]).size(); i++){\n' \
               '	        ret.push_back(boost::any_cast<std::string>(boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"])[i]));\n' \
               '        }\n'
    if _templatetype == 'array':
        return '        std::vector<' + typetocpptype(_templatetype) + '> ret;\n' \
               '        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"]).size(); i++){\n' \
               '	        ret.push_back(boost::any_cast<' + typetocpptype(_templatetype) + '>(boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"])[i]));\n' \
               '        }\n'
    if _templatetype == 'table':
        return '        std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ret;\n' \
               '        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"]).size(); i++){\n' \
               '            ret.push_back(boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >(boost::any_cast<std::vector<boost::any> >((*r)[\"ret\"])[i]));\n' \
               '        }\n'

def makeret(type, struct):
    if type == 'int':
        return '        return boost::any_cast<int64_t>((*r)[\"ret\"]);'
    if type == 'float':
        return '        return boost::any_cast<double>((*r)[\"ret\"]);'
    if type == 'bool':
        return '        return boost::any_cast<bool>((*r)[\"ret\"]);'
    if type == 'string' or type == 'std::string':
        return '        return boost::any_cast<std::string>((*r)[\"ret\"]);'
    if type.find('array') != -1:
        return makearray(type) +  '        return ret;'
    if type == 'table':
        return '        return boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)[\"ret\"]);'
    if type == 'void':
        return ''
    else:
        return unpackstruct(type, struct)

def makecallbackret(type, struct):
    if type == 'int':
        return 'auto ret = boost::any_cast<int64_t>((*r)[\"ret\"]);'
    if type == 'float':
        return 'auto ret = boost::any_cast<double>((*r)[\"ret\"]);'
    if type == 'bool':
        return 'auto ret = boost::any_cast<bool>((*r)[\"ret\"]);'
    if type == 'string' or type == 'std::string':
        return 'auto ret = boost::any_cast<std::string>((*r)[\"ret\"]);'
    if type.find('array') != -1:
        return makearray(type)
    if type == 'table':
        return 'auto ret = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)[\"ret\"]);'
    if type == 'void':
        return ''
    else:
        return unpackstruct(type, struct)

def  maketypevalue(type, name):
    if type == 'int':
        return '	n.' + name + ' = ' + 'boost::any_cast<int64_t>((*v)[' + name + ']' + ');'
    if type == 'float':
        return '	n.' + name + ' = ' + 'boost::any_cast<double>((*v)[' + name + ']' + ');'
    if type == 'bool':
        return '	n.' + name + ' = ' + 'boost::any_cast<bool>((*v)[' + name + ']' + ');'
    if type == 'string' or type == 'std::string':
        return '	n.' + name + ' = ' + 'boost::any_cast<std::string>((*v)[' + name + ']' + ');'
    if type.find('array') != -1:
        indexb, indexe, _templatetype = templatetype(type)
        code = '	n.' + name + ';\n'
        code += '	for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)[' + name + ']).size(); i++){\n'
        code += '		n.' + name + '.push_back(boost::any_cast<' + maketypegetvalue(_templatetype) + '>(boost::any_cast<std::vector<boost::any> >((*v)[' + name + '])[i]));'
        code += '}\n'
        return code
    if type == 'table':
        return '	n.' + name + ' = ' + 'boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)[' + name + ']);'

def unpackstructvalue(type, name, struct):
    for k, v in struct:
        for typename, define in v:
            if type == typename:
                code = '	typename ' + name + ';\n'
                for argv in define:
                    code += maketypevalue(argv[0], argv[1])
                return code

def makearrayvalue(type, name):
    indexb, indexe, _templatetype = templatetype(type)
    if _templatetype == 'int':
        return '		std::vector<int64_t> ' + name + ';\n' \
               '		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"]).size(); i++){\n' \
               '			' + name + '.push_back(boost::any_cast<int64_t>(boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"])[i]));\n' \
               '		}\n'
    if _templatetype == 'float':
        return '		std::vector<double> ' + name + ';\n' \
               '		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"]).size(); i++){\n' \
               '			' + name + '.push_back(boost::any_cast<double>(boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"])[i]));\n' \
               '		}\n'
    if _templatetype == 'bool':
        return '		std::vector<bool> ' + name + ';\n' \
               '		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"]).size(); i++){\n' \
               '			' + name + '.push_back(boost::any_cast<bool>(boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"])[i]));\n' \
               '		}\n'
    if _templatetype == 'string' or _templatetype == 'std::string':
        return '		std::vector<std::string> ' + name + ';\n' \
               '		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"]).size(); i++){\n' \
               '			' + name + '.push_back(boost::any_cast<std::string>(boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"])[i]));\n' \
               '		}\n'
    if _templatetype == 'array':
        indexb, indexe, __templatetype = templatetype(_templatetype)
        return '		std::vector<' + maketypegetvalue(__templatetype) + '> ' + name + ';\n' \
               '		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"]).size(); i++){\n' \
               '			' + name + '.push_back(boost::any_cast<' + maketypegetvalue(__templatetype) + '>(boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"])[i]));\n' \
               '		}\n'
    if _templatetype == 'table':
        return '		std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ' + name + ';\n' \
               '		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"]).size(); i++){\n' \
               '			' + name + '.push_back(boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >(boost::any_cast<std::vector<boost::any> >((*v)[\"' + name + '\"])[i]));\n' \
               '		}\n'

def makevalue(type, name, struct):
    if type == 'int':
        return '		auto ' + name + ' = boost::any_cast<int64_t>((*v)[\"' + name + '\"]);\n'
    if type == 'float':
        return '		auto ' + name + ' = boost::any_cast<double>((*v)[\"' + name + '\"]);\n'
    if type == 'bool':
        return '		auto ' + name + ' = boost::any_cast<bool>((*v)[\"' + name + '\"]);\n'
    if type == 'string' or type == 'std::string':
        return '		auto ' + name + ' = boost::any_cast<std::string>((*v)[\"' + name + '\"]);\n'
    if type.find('array') != -1:
        return makearrayvalue(type, name)
    if type == 'table':
        return '		auto ' + name + ' = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)[\"' + name + '\"]);\n'
    else:
        return unpackstructvalue(type, name, struct)

def makearraytocpp(type, name):
    indexb, indexe, _templatetype = templatetype(type)
    return '	' + 'std::vector<' + typetocpptype(_templatetype) + '> ' + name + ';'

def maketypetocpptype(type):
    if type.find('array') != -1:
        indexb, indexe, _templatetype = templatetype(type)
        return 'std::vector<' + typetocpptype(_templatetype) + '> '
    if type == 'table':
        return 'boost::shared_ptr<boost::unordered_map<std::string, boost::any> >'
    if type == 'int':
        return 'int64_t'
    if type == 'float':
        return 'double'
    if type == 'string':
        return 'std::string'
    return type