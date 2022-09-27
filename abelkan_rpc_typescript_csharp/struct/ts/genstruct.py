#coding:utf-8
# 2018-5-5
# build by qianqians
# genstruct

import tools

def genlisttypestruct(key, value, item):
    if key in ['number', 'string', 'boolean', 'object']:
        return "            _" + value + ".push(" + item + ");\n"

    if key[len(key)-2] == '[' and key[len(key)-1] == ']':
        raise Exception("not support multidimensional array:%s" % key)

    tmp_type = key.split('.')
    if len(tmp_type) == 2:
        return "            _" + value + ".push(" + tmp_type[0] + ".table2" + tmp_type[1] +  "(" + item + "));\n"

    return "            _" + value + ".push(table2" + key +  "(" + item + "));\n"

def genelem2struct(key, value):
    if key[0:5] == 'Array':
        list_type = key[6:len(key)-1]
        code = "    let _" + value + " = tmp_undefined as " + list_type + "[];\n"
        code += "    let list_" + value + " = _table[\"" + value + "\"] as any[];\n"
        code += "    if (list_" + value + "){\n"
        code += "        _" + value + " = new Array<" + list_type + ">();\n"
        code += "        for(let item_list_" + value + " of list_" + value + ")\n"
        code += "        {\n"
        code += genlisttypestruct(list_type, value, "item_list_" + value)
        code += "        }\n"
        code += "    }\n"
        
        return code

    if key in ['number', 'string', 'boolean', 'object']:
        return "    let _" + value + " =  _table[\"" + value + "\"] as " + key + ";\n"

    tmp_type = key.split('.')
    if len(tmp_type) == 2:
        code = "    let _" + value + " = tmp_undefined as " + key + ";\n"
        code += "    if (_table[\"" + value + "\"]){\n"
        code += "        _" + value + " = " + tmp_type[0] + ".table2" + tmp_type[1] +  "(_table[\"" + value + "\"]);\n"
        code += "    }\n"
        return code

    code = "    let _" + value + " = tmp_undefined as " + key + ";\n"
    code += "    if (_table[\"" + value + "\"]){\n"
    code += "        _" + value + " = table2" + key +  "(_table[\"" + value + "\"]);\n"
    code += "    }\n"
    return code

def gentable2struct(struct_name, elems):
    code = "export function table2" + struct_name + "(_table:any)\n"
    code += "{\n"
    code += "    let tmp_undefined : any = undefined;\n"
    for key, value in elems:
        code += genelem2struct(key, value)
    code += "    var _struct = new " + struct_name + "("
    count = 0
    for key, value in elems:
        code += "_" + value
        count = count + 1
        if count < len(elems):
            code += ", "
    code += ");\n"
    code += "    return _struct;\n"
    code += "}\n"

    return code

def genlisttype(key, list_name):
    if key in ['number', 'string', 'boolean', 'object']:
        return "        " + list_name + ".push(_item_1);\n"

    if key[len(key)-2] == '[' and key[len(key)-1] == ']':
        raise Exception("not support multidimensional array:%s" % key)

    return "       " + list_name + ".push(" + key +  "2table(_item_1));\n"

def genelem2table(key, value):
    if key[0:5] == 'Array':
        list_type = key[6:len(key)-1]
        code = "    let list_" + value + " = [];\n"
        code += "    for(let _item_1 of _struct." + value + ")\n"
        code += "    {\n"
        code += genlisttype(list_type, "list_" + value)
        code += "    }\n"
        code += "    table[\"" + value + "\"] = list_" + value + ";\n"

        return code

    if key in ['number', 'string', 'boolean', 'object']:
        return "    table[\"" + value + "\"] = _struct." + value + ";\n"

    return "    table[\"" + value + "\"] = " + key +  "2table(_struct." + value + ");\n"

def genstruct2table(struct_name, elems):
    code = "export function " + struct_name + "2table(_struct:" + struct_name + ")\n"
    code += "{\n"
    code += "    let table:any = {};\n"
    for key, value in elems:
        code += genelem2table(key, value)
    code += "    return table;\n"
    code += "}\n"

    return code

def transference(elems):
    type_elems = []
    for key, value in elems:
        if key[len(key)-2] == '[' and key[len(key)-1] == ']':
            type_elems.append(('Array<' + tools.gentypetots(key[:-2]) + '>', value))
        else:
            type_elems.append((tools.gentypetots(key), value))
    return type_elems

def genstruct(struct_name, elems):
    type_elems = transference(elems)

    code = "/*this struct code is codegen by abelkhan codegen for typescript*/\n\n"

    code += "export class " + struct_name + "\n{\n"
    for key, value in type_elems:
        code += "    public " + value + " : " + key + ";\n"
    code += "\n    constructor("
    count = 0
    for key, value in type_elems:
        code += "_" + value + " : " + key
        count = count + 1
        if count < len(type_elems):
            code += ", "
    code += "){\n"
    for key, value in type_elems:
        code += "        this." + value + " = _" + value + ";\n"
    code += "    }\n"
    code += "}\n\n"
    code += genstruct2table(struct_name, type_elems)
    code += gentable2struct(struct_name, type_elems)

    return code
