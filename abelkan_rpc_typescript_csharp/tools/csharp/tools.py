#coding:utf-8
# 2016-7-1
# build by qianqians
# tools

import global_argv

def gentypetocsharp(typestr):
    if typestr == 'int':
        return 'Int64'
    elif typestr == 'string':
        return 'String'
    elif typestr == 'array':
        return 'ArrayList'
    elif typestr == 'float':
        return 'Double'
    elif typestr == 'bool':
        return 'Boolean'
    elif typestr == 'table':
        return 'Hashtable'
    elif typestr in global_argv.struct_type_list:
        return typestr

    raise Exception("non exist type:%s" % typestr)

def gentypetocsharpwithlist(typestr):
    if typestr in ['int', 'string', 'float', 'bool', 'table']:
        return gentypetocsharp(typestr)
    elif typestr in global_argv.struct_type_list:
        return gentypetocsharp(typestr)
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        typestr = typestr[:-2]
        return gentypetocsharp(typestr) + "[]"

    raise Exception("non exist type:%s" % typestr)

def genlisttypeargv(list_type, list_name):
    if list_type in ['int', 'string', 'float', 'bool', 'table']:
        return "                " + list_name + ".Add(_item_2);\n"

    if list_type[len(list_type)-2] == '[' and list_type[len(list_type)-1] == ']':
        raise Exception("not support multidimensional array:%s" % list_type)

    return "                " + list_name + ".Add(" + list_type + "." + list_type +  "2table(_item_2));\n"

def genprocessargv(typestr, index):
    if typestr in ['int', 'string', 'float', 'bool', 'table']:
        return "            var input_argv" + str(index) + " = (" + gentypetocsharp(typestr) + ")argv" + str(index) + ";\n"
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        list_type = typestr[:-2]
        code = "            var input_argv" + str(index) + " = new ArrayList();\n"
        code += "            foreach(var _item_2 in argv" + str(index) + ")\n"
        code += "            {\n"
        code += genlisttypeargv(list_type, "input_argv" + str(index))
        code += "            }\n"
        return code;
    elif typestr in global_argv.struct_type_list:
        return "            var input_argv" + str(index) + " = " + typestr + "." + typestr + "2table(argv" + str(index) + ");\n"

    raise Exception("non exist type:%s" % typestr)

def genlisttypeproto(list_type, list_name):
    if list_type in ['int', 'string', 'float', 'bool', 'table']:
        return "                " + list_name + ".Add((" + gentypetocsharp(list_type) + ")_item_2);\n"

    if list_type[len(list_type)-2] == '[' and list_type[len(list_type)-1] == ']':
        raise Exception("not support multidimensional array:%s" % list_type)

    return "                " + list_name + ".Add(" + list_type + ".table2" + list_type +  "((Hashtable)_item_2));\n"

def genproto2argv(typestr, index):
    if typestr in ['int', 'string', 'float', 'bool', 'table']:
        code = "            " + gentypetocsharp(typestr) + " tmp" + str(index) + ";\n"
        if typestr in ['int', 'float']:
            code += "            Type tp" + str(index) + " = _events[" + str(index+1) + "].GetType();\n"
            code += "            if (tp" + str(index) + ".Name == \"Int64\"){\n"
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")((Int64)_events[" + str(index+1) + "]);\n"
            code += "            }\n"
            code += "            else if (tp" + str(index) + ".Name == \"Double\"){\n"
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")((Double)_events[" + str(index+1) + "]);\n"
            code += "            }\n"
            code += "            else {\n"
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")(_events[" + str(index+1) + "]);\n"
            code += "            }\n"
        else:
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")(_events[" + str(index+1) + "]);\n"
        code += "            var argv" + str(index) + " = tmp" + str(index) + ";\n"
        return code
    elif typestr in global_argv.struct_type_list:
        return "            var argv" + str(index) + " = " + typestr + ".table2" + typestr + "((Hashtable)_events[" + str(index+1) + "]);\n"
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        list_type = typestr[:-2] #gentypetocsharp(typestr[:-2])
        code = "            var list" + str(index) + " = new List<" + gentypetocsharp(list_type) + ">();\n"
        code += "            var proto_list" + str(index) + " = (ArrayList)_events[" + str(index+1) + "];\n"
        code += "            foreach(var _item_2 in proto_list" + str(index) + ")\n"
        code += "            {\n"
        code += genlisttypeproto(list_type, "list" + str(index))
        code += "            }\n"
        code += "            var argv" + str(index) + " = list" + str(index) + ".ToArray();\n"
        return code

    raise Exception("non exist type:%s" % typestr)

def genproto2argvmodule(typestr, index):
    if typestr in ['int', 'string', 'float', 'bool', 'table']:
        code = "            " + gentypetocsharp(typestr) + " tmp" + str(index) + ";\n"
        if typestr in ['int', 'float']:
            code += "            Type tp" + str(index) + " = _events[" + str(index) + "].GetType();\n"
            code += "            if (tp" + str(index) + ".Name == \"Int64\"){\n"
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")((Int64)_events[" + str(index) + "]);\n"
            code += "            }\n"
            code += "            else if (tp" + str(index) + ".Name == \"Double\"){\n"
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")((Double)_events[" + str(index) + "]);\n"
            code += "            }\n"
            code += "            else {\n"
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")(_events[" + str(index) + "]);\n"
            code += "            }\n"
        else:
            code += "                tmp" + str(index) + " = (" + gentypetocsharp(typestr) + ")(_events[" + str(index) + "]);\n"
        code += "            var argv" + str(index) + " = tmp" + str(index) + ";\n"
        return code
    elif typestr in global_argv.struct_type_list:
        return "            var argv" + str(index) + " = " + typestr + ".table2" + typestr + "((Hashtable)_events[" + str(index) + "]);\n"
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        list_type = typestr[:-2] #gentypetocsharp(typestr[:-2])
        code = "            var list" + str(index) + " = new List<" + gentypetocsharp(list_type) + ">();\n"
        code += "            var proto_list" + str(index) + " = (ArrayList)_events[" + str(index) + "];\n"
        code += "            foreach(var _item_2 in proto_list" + str(index) + ")\n"
        code += "            {\n"
        code += genlisttypeproto(list_type, "list" + str(index))
        code += "            }\n"
        code += "            var argv" + str(index) + " = list" + str(index) + ".ToArray();\n"
        return code

    raise Exception("non exist type:%s" % typestr)
