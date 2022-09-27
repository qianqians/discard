#coding:utf-8
# 2016-7-1
# build by qianqians
# tools

import global_argv

def genprototype(typestr):
    if typestr == 'int':
        return 'number'
    elif typestr == 'string':
        return 'string'
    elif typestr == 'array':
        return 'any[]'
    elif typestr == 'float':
        return 'number'
    elif typestr == 'bool':
        return 'boolean'
    elif typestr == 'table':
        return 'object'
    elif typestr in global_argv.struct_type_list:
	    return 'object'
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        return 'any[]'

    raise Exception("non exist type:%s" % typestr)

def gentypetots(typestr):
    if typestr == 'int':
        return 'number'
    elif typestr == 'string':
        return 'string'
    elif typestr == 'array':
        return 'any[]'
    elif typestr == 'float':
        return 'number'
    elif typestr == 'bool':
        return 'boolean'
    elif typestr == 'table':
        return 'object'
    else:
        if typestr in global_argv.current_struct_type_list:
            return typestr

        for file_name, struct_type_list in global_argv.struct_type_list_file.items():
            if typestr in struct_type_list:
                if file_name not in global_argv.quote_file_list:
                    global_argv.quote_file_list.append(file_name)
                return file_name + "." + typestr

    raise Exception("non exist type:%s" % typestr)

def genlisttypeproto(list_type, list_name):
    if list_type in ['int', 'string', 'float', 'bool']:
        return "            " + list_name + ".push(item as " + gentypetots(list_type) + ");\n"

    if list_type[len(list_type)-2] == '[' and list_type[len(list_type)-1] == ']':
        raise Exception("not support multidimensional array:%s" % list_type)

    if list_type in global_argv.current_struct_type_list:
        return "            " + list_name + ".push(table2" + list_type +  "(item));\n"

    for file_name, struct_type_list in global_argv.struct_type_list_file.items():
        if list_type in struct_type_list:
            if file_name not in global_argv.quote_file_list:
                global_argv.quote_file_list.append(file_name)
            return "            " + list_name + ".push(" + file_name + ".table2" + list_type +  "(item));\n"

    raise Exception("non exist type:%s" % list_type)

def genprotowithlist(typestr, index):
    if typestr in ['int', 'string', 'float', 'bool']:
        return "        let input_argv" + str(index) + " : " + gentypetots(typestr) + " = argv" + str(index) + " as " + gentypetots(typestr) + ";\n"
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        list_type = typestr[:-2]
        code = "        let input_argv" + str(index) + " : " + gentypetots(list_type) + "[] = [];\n"
        code += "        for(let item of argv" + str(index) + "){\n"
        code += genlisttypeproto(list_type, "input_argv" + str(index))
        code += "        }\n"
        return code
    elif typestr in global_argv.current_struct_type_list:
        return "        let input_argv" + str(index) + " : " + gentypetots(typestr) + " = table2" + typestr + "(argv" + str(index) + ");\n"

    for file_name, struct_type_list in global_argv.struct_type_list_file.items():
        if typestr in struct_type_list:
            if file_name not in global_argv.quote_file_list:
                global_argv.quote_file_list.append(file_name)
            return "        let input_argv" + str(index) + " : " + gentypetots(typestr) + " = " + file_name + ".table2" + typestr + "(argv" + str(index) + ");\n"


    print global_argv.struct_type_list_file
    raise Exception("non exist type:%s" % typestr)

def genlocaltype(typestr):
    if typestr == 'int':
        return 'number'
    elif typestr == 'string':
        return 'string'
    elif typestr == 'array':
        return 'any[]'
    elif typestr == 'float':
        return 'number'
    elif typestr == 'bool':
        return 'boolean'
    elif typestr == 'table':
        return 'object'
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        list_type = typestr[:-2]
        list_type = gentypetots(list_type)
        return list_type + '[]'
    elif typestr in global_argv.current_struct_type_list:
    	    return typestr

    for file_name, struct_type_list in global_argv.struct_type_list_file.items():
        if typestr in struct_type_list:
            if file_name not in global_argv.quote_file_list:
                global_argv.quote_file_list.append(file_name)
            return file_name + "." + typestr

    raise Exception("non exist type:%s" % typestr)

def genlistlocaltoproto(list_type, list_name):
    if list_type in ['int', 'string', 'float', 'bool']:
        return "            " + list_name + ".push(_item_codegen);\n"

    if list_type[len(list_type)-2] == '[' and list_type[len(list_type)-1] == ']':
        raise Exception("not support multidimensional array:%s" % list_type)

    if list_type in global_argv.current_struct_type_list:
        return "            " + list_name + ".push(" + list_type +  "2table(_item_codegen));\n"

    for file_name, struct_type_list in global_argv.struct_type_list_file.items():
        if list_type in struct_type_list:
            if file_name not in global_argv.quote_file_list:
                global_argv.quote_file_list.append(file_name)
            return "            " + list_name + ".push(" + file_name + "." + list_type +  "2table(_item_codegen));\n"

    raise Exception("non exist type:%s" % list_type)

def genlocaltoproto(typestr, index):
    if typestr in ['int', 'string', 'float', 'bool']:
        return "        let input_argv" + str(index) + " : " + gentypetots(typestr) + " = argv" + str(index) + " as " + gentypetots(typestr) + ";\n"
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        list_type = typestr[:-2]
        code = "        let input_argv" + str(index) + " : any[] = [];\n"
        code += "        for(let _item_codegen of argv" + str(index) + "){\n"
        code += genlistlocaltoproto(list_type, "input_argv" + str(index))
        code += "        }\n"
        return code
    elif typestr in global_argv.current_struct_type_list:
        return "        let input_argv" + str(index) + " : object = " + typestr + "2table(argv" + str(index) + ");\n"

    for file_name, struct_type_list in global_argv.struct_type_list_file.items():
        if typestr in struct_type_list:
            if file_name not in global_argv.quote_file_list:
                global_argv.quote_file_list.append(file_name)
            return "        let input_argv" + str(index) + " : object = " + file_name + "." + typestr + "2table(argv" + str(index) + ");\n"

    raise Exception("non exist type:%s" % typestr)