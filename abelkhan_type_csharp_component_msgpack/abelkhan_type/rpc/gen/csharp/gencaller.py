#coding:utf-8
# 2020-1-21
# build by qianqians
# gencaller

import uuid
import tools

def gen_module_caller(module_name, funcs, dependent_struct, dependent_enum):
    #code_begin = "using System;"

    cb_func = ""

    cb_code = "/*this cb code is codegen by abelkhan for c#*/\n"
    cb_code += "    public class rsp_cb_" + module_name + " : abelkhan.Imodule {\n"
    cb_code_constructor = "        public rsp_cb_" + module_name + "(abelkhan.modulemng modules) : base(\"rsp_cb_" + module_name + "\")\n"
    cb_code_constructor += "        {\n"
    cb_code_constructor += "            modules.reg_module(this);\n\n"
    cb_code_section = ""

    
    #code = "import uuidv1 = require('uuid/v1');\n"
    code = "    public class " + module_name + "_caller : abelkhan.Icaller {\n"
    code += "        public static rsp_cb_" + module_name + " rsp_cb_" + module_name + "_handle = null;\n"
    code += "        public " + module_name + "_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base(\"" + module_name + "\", _ch)\n"
    code += "        {\n"
    code += "            if (rsp_cb_" + module_name + "_handle == null)\n            {\n"
    code += "                rsp_cb_" + module_name + "_handle = new rsp_cb_" + module_name + "(modules);\n"
    code += "            }\n"
    code += "        }\n\n"

    for i in funcs:
        func_name = i[0]

        if i[1] == "ntf":
            code += "        public void " + func_name + "("
            count = 0
            for _type, _name in i[2]:
                code += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name 
                count = count + 1
                if count < len(i[2]):
                    code += ", "
            code += "){\n"
            #_argv_uuid = str(uuid.uuid1())
            #_argv_uuid = '_'.join(_argv_uuid.split('-'))
            code += "            var __argv_struct__ = new " + module_name + "_" + func_name + "_struct_ntf();\n"
            for _type, _name in i[2]:
                code += "            __argv_struct__." + _name + " = " + _name + ";\n"
            code += "            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);\n"
            code += "            call_module_method(\"" + func_name + "\", __byte_struct__);\n"
            code += "        }\n\n"
        elif i[1] == "req" and i[3] == "rsp" and i[5] == "err":
            cb_func += "    public class cb_" + func_name + "\n    {\n"
            cb_func += "        public delegate void " + func_name + "_handle_cb("
            count = 0
            for _type, _name in i[4]:
                cb_func += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name 
                count = count + 1
                if count < len(i[4]):
                    cb_func += ", "
            cb_func += ");\n"
            cb_func += "        public event " + func_name + "_handle_cb on" + func_name + "_cb;\n\n"

            cb_func += "        public delegate void " + func_name + "_handle_err("
            count = 0
            for _type, _name in i[6]:
                cb_func += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name
                count = count + 1
                if count < len(i[6]):
                    cb_func += ", "
            cb_func += ");\n"
            cb_func += "        public event " + func_name + "_handle_err on" + func_name + "_err;\n\n"

            cb_func += "        public void callBack(" + func_name + "_handle_cb cb, " + func_name + "_handle_err err)\n        {\n"
            cb_func += "            on" + func_name + "_cb += cb;\n"
            cb_func += "            on" + func_name + "_err += err;\n"
            cb_func += "        }\n\n"
            cb_func += "        public void call_cb("
            count = 0
            for _type, _name in i[4]:
                cb_func += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name 
                count = count + 1
                if count < len(i[4]):
                    cb_func += ", "
            cb_func += ")\n        {\n"
            cb_func += "            if (on" + func_name + "_cb != null)\n"
            cb_func += "            {\n"
            cb_func += "                on" + func_name + "_cb(" 
            count = 0
            for _type, _name in i[4]:
                cb_func += _name
                count = count + 1
                if count < len(i[4]):
                    cb_func += ", "
            cb_func += ");\n"
            cb_func += "            }\n"
            cb_func += "        }\n\n"
            cb_func += "        public void call_err("
            count = 0
            for _type, _name in i[6]:
                cb_func += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name 
                count = count + 1
                if count < len(i[6]):
                    cb_func += ", "
            cb_func += ")\n        {\n"
            cb_func += "            if (on" + func_name + "_err != null)\n"
            cb_func += "            {\n"
            cb_func += "                on" + func_name + "_err(" 
            count = 0
            for _type, _name in i[6]:
                cb_func += _name
                count = count + 1
                if count < len(i[6]):
                    cb_func += ", "
            cb_func += ");\n"
            cb_func += "            }\n"
            cb_func += "        }\n\n"
            cb_func += "    }\n\n"

            cb_code += "        public Dictionary<string, cb_" + func_name + "> map_" + func_name + ";\n"
            cb_code_constructor += "            map_" + func_name + " = new Dictionary<string, cb_" + func_name + ">();\n"
            cb_code_constructor += "            reg_method(\"" + func_name + "_rsp\", " + func_name + "_rsp);\n"
            cb_code_constructor += "            reg_method(\"" + func_name + "_err\", " + func_name + "_err);\n"

            cb_code_section += "        public void " + func_name + "_rsp(byte[] _event){\n"
            cb_code_section += "            var _struct = MessagePackSerializer.Deserialize<" + module_name + "_" + func_name + "_struct_rsp>(_event);\n"
            cb_code_section += "            var rsp = map_" + func_name + "[_struct._cb_uuid];\n"
            cb_code_section += "            rsp.call_cb("
            count = 0
            for _type, _name in i[4]:
                cb_code_section += "_struct." + _name
                count = count + 1
                if count < len(i[4]):
                    cb_code_section += ", "
            cb_code_section += ");\n"
            cb_code_section += "            map_" + func_name + ".Remove(_struct._cb_uuid);\n"
            cb_code_section += "        }\n"

            cb_code_section += "        public void " + func_name + "_err(byte[] _event){\n"
            cb_code_section += "            var _struct = MessagePackSerializer.Deserialize<" + module_name + "_" + func_name + "_struct_err>(_event);\n"
            cb_code_section += "            var rsp = map_" + func_name + "[_struct._cb_uuid];\n"
            cb_code_section += "            rsp.call_err("
            count = 0
            for _type, _name in i[6]:
                cb_code_section += "_struct." + _name
                count = count + 1
                if count < len(i[6]):
                    cb_code_section += ", "
            cb_code_section += ");\n"
            cb_code_section += "            map_" + func_name + ".Remove(_struct._cb_uuid);\n"
            cb_code_section += "        }\n"

            code += "        public cb_" + func_name + " " + func_name + "("
            count = 0
            for _type, _name in i[2]:
                code += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name 
                count = count + 1
                if count < len(i[2]):
                    code += ", "
            code += "){\n"
            #_cb_uuid_uuid = str(uuid.uuid1())
            #_cb_uuid_uuid = '_'.join(_cb_uuid_uuid.split('-'))
            code += "            var __cb_uuid_uuid__ = System.Guid.NewGuid().ToString(\"N\");\n\n"
            #_argv_uuid = str(uuid.uuid1())
            #_argv_uuid = '_'.join(_argv_uuid.split('-'))
            code += "            var __argv_struct__ = new " + module_name + "_" + func_name + "_struct_req();\n"
            code += "            __argv_struct__._cb_uuid = __cb_uuid_uuid__;\n"
            for _type, _name in i[2]:
                code += "            __argv_struct__." + _name + " = " + _name + ";\n"
            code += "            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);\n"
            code += "            call_module_method(\"" + func_name + "\", __byte_struct__);\n\n"
            code += "            var cb_" + func_name + "_obj = new cb_" + func_name + "();\n"
            code += "            rsp_cb_" + module_name + "_handle.map_" + func_name + ".Add(__cb_uuid_uuid__, cb_" + func_name + "_obj);\n"
            code += "            return cb_" + func_name + "_obj;\n"
            code += "        }\n\n"

        else:
            raise Exception("func:" + func_name + " wrong rpc type:" + i[1] + ", must req or ntf")

    cb_code_constructor += "        }\n"
    cb_code_section += "    }\n\n"
    code += "    }\n"

    return cb_func + cb_code + cb_code_constructor + cb_code_section + code

def gencaller(pretreatment):
    dependent_struct = pretreatment.dependent_struct
    dependent_enum = pretreatment.dependent_enum
    
    modules = pretreatment.module
    
    code = "/*this caller code is codegen by abelkhan codegen for c#*/\n"
    for module_name, funcs in modules.items():
        code += gen_module_caller(module_name, funcs, dependent_struct, dependent_enum)
        
    return code
