#coding:utf-8
# 2018-5-5
# build by qianqians
# genstruct

import tools

def genlisttypestruct(key, value, item):
    if key in ['Int64', 'Boolean', 'Double', 'String', 'Hashtable']:
        code = "                " + key + " tmp;\n"
        if key in ['Int64', 'Double']:
            code += "                Type tp = " + item + ".GetType();\n"
            code += "                if (tp.Name == \"Int64\"){\n"
            code += "                    tmp = (" + key + ")((Int64)" + item + ");\n"
            code += "                }\n"
            code += "                else if (tp.Name == \"Double\"){\n"
            code += "                    tmp = (" + key + ")((Double)" + item + ");\n"
            code += "                }\n"
            code += "                else {\n"
            code += "                    tmp = (" + key + ")(" + item + ");\n"
            code += "                }\n"
        else:
            code += "                    tmp = (" + key + ")(" + item + ");\n"
        code += "                _struct." + value + ".Add(tmp);\n"
        return code

    if key[len(key)-2] == '[' and key[len(key)-1] == ']':
        raise Exception("not support multidimensional array:%s" % key)

    return "                _struct." + value + ".Add(" + key + ".table2" + key +  "((Hashtable)" + item + "));\n"

def genelem2struct(key, value):
    if key[0:4] == 'List':
        list_type = key[5:len(key)-1]
        code = "            _struct." + value + " = new List<" + list_type + ">();\n"
        code += "            var list" + value + " = (ArrayList)_table[\"" + value + "\"];\n"
        code += "            foreach(var item_list_" + value + " in list" + value + ")\n"
        code += "            {\n"
        code += genlisttypestruct(list_type, value, "item_list_" + value)
        code += "            }\n"
        return code

    if key in ['Int64', 'Boolean', 'Double', 'String', 'Hashtable']:
        code = "            " + key + " tmp_" + value + ";\n"
        if key in ['Int64', 'Double']:
            code += "            Type tp_" + value + " = _table[\"" + value + "\"].GetType();\n"
            code += "            if (tp_" + value + ".Name == \"Int64\"){\n"
            code += "                tmp_" + value + " = (" + key + ")((Int64)_table[\"" + value + "\"]);\n"
            code += "            }\n"
            code += "            else if (tp_" + value + ".Name == \"Double\"){\n"
            code += "                tmp_" + value + " = (" + key + ")((Double)_table[\"" + value + "\"]);\n"
            code += "            }\n"
            code += "            else {\n"
            code += "                tmp_" + value + " = (" + key + ")(_table[\"" + value + "\"]);\n"
            code += "            }\n"
        else:
            code += "                tmp_" + value + " = (" + key + ")(_table[\"" + value + "\"]);\n"
        code += "            _struct." + value + " =  tmp_" + value + ";\n\n"
        return code

    return "            _struct." + value + " = " + key + ".table2" + key +  "((Hashtable)_table[\"" + value + "\"]);\n"

def gentable2struct(struct_name, elems):
    code = "        static public " + struct_name + " table2" + struct_name + "(Hashtable _table)\n"
    code += "        {\n"
    code += "            var _struct = new " + struct_name + "();\n"
    for key, value in elems:
        code += genelem2struct(key, value)
    code += "            return _struct;\n"
    code += "        }\n"

    return code

def genlisttype(key, list_name):
    if key in ['Int64', 'Boolean', 'Double', 'String', 'Hashtable']:
        return "                " + list_name + ".Add(_item_1);\n"

    if key[len(key)-2] == '[' and key[len(key)-1] == ']':
        raise Exception("not support multidimensional array:%s" % key)

    return "                " + list_name + ".Add(" + key + "." + key +  "2table(_item_1));\n"

def genelem2table(key, value):
    if key[0:4] == 'List':
        list_type = key[5:len(key)-1]
        code = "            var list" + value + " = new ArrayList();\n"
        code += "            foreach(var _item_1 in _struct." + value + ")\n"
        code += "            {\n"
        code += genlisttype(list_type, "list" + value)
        code += "            }\n"
        code += "            table.Add(\"" + value + "\", list" + value + ");\n"

        return code

    if key in ['Int64', 'Boolean', 'Double', 'String', 'Hashtable']:
        return "            table.Add(\"" + value + "\", _struct." + value + ");\n"

    return "            table.Add(\"" + value + "\", " + key + "." + key +  "2table(_struct." + value + "));\n"

def genstruct2table(struct_name, elems):
    code = "        static public Hashtable " + struct_name + "2table(" + struct_name + " _struct)\n"
    code += "        {\n"
    code += "            var table = new Hashtable();\n"
    for key, value in elems:
        code += genelem2table(key, value)
    code += "            return table;\n"
    code += "        }\n"

    return code

def transference(elems):
    type_elems = []
    for key, value in elems:
        if key[len(key)-2] == '[' and key[len(key)-1] == ']':
            type_elems.append(('List<' + tools.gentypetocsharp(key[:-2]) + '>', value))
        else:
            type_elems.append((tools.gentypetocsharp(key), value))
    return type_elems

def genstruct(struct_name, elems):
    type_elems = transference(elems)

    code = "/*this struct code is codegen by abelkhan codegen for c#*/\n\n"

    code += "    public class " + struct_name + "\n    {\n"
    for key, value in type_elems:
        code += "        public " + key + " " + str(value) + ";\n"
    code += genstruct2table(struct_name, type_elems)
    code += gentable2struct(struct_name, type_elems)
    code += "    }\n"

    return code
