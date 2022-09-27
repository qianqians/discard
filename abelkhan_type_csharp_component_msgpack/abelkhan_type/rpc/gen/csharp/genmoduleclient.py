#coding:utf-8
# 2020-1-21
# build by qianqians
# genmodule

import uuid
import tools

def gen_module_module(module_name, funcs, dependent_struct, dependent_enum):
    code_constructor = "    public class " + module_name + "_module : abelkhan.Imodule {\n"
    code_constructor += "        private readonly abelkhan.modulemng modules;\n"
    code_constructor += "        private readonly MessagePackSerializerOptions op;\n"
    code_constructor += "        public " + module_name + "_module(abelkhan.modulemng _modules, MessagePackSerializerOptions _op) : base(\"" + module_name + "\")\n"
    code_constructor += "        {\n"
    code_constructor += "            op = _op;\n"
    code_constructor += "            modules = _modules;\n"
    code_constructor += "            modules.reg_module(this);\n\n"
        
    code_constructor_cb = ""
    rsp_code = ""
    code_func = ""
    for i in funcs:
        func_name = i[0]

        if i[1] == "ntf":
            code_constructor += "            reg_method(\"" + func_name + "\", " + func_name + ");\n"
                
            code_func += "        public delegate void cb_" + func_name + "_handle("
            count = 0
            for _type, _name in i[2]:
                code_func += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name 
                count += 1
                if count < len(i[2]):
                    code_func += ", "
            code_func += ");\n"
            code_func += "        public event cb_" + func_name + "_handle on" + func_name + ";\n\n"

            code_func += "        public void " + func_name + "(byte[] _event){\n"
            code_func += "            var _struct = MessagePackSerializer.Deserialize<" + module_name + "_" + func_name + "_struct_ntf>(_event, op);\n"
            code_func += "            if (on" + func_name + " != null){\n"
            code_func += "                on" + func_name + "("
            count = 0
            for _type, _name in i[2]:
                code_func += "_struct." + _name
                count = count + 1
                if count < len(i[2]):
                    code_func += ", "
            code_func += ");\n"
            code_func += "            }\n"
            code_func += "        }\n\n"
        elif i[1] == "req" and i[3] == "rsp" and i[5] == "err":
            code_constructor += "            reg_method(\"" + func_name + "\", " + func_name + ");\n"
            
            code_func += "        public delegate void cb_" + func_name + "_handle("
            count = 0
            for _type, _name in i[2]:
                code_func += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name
                count += 1
                if count < len(i[2]):
                    code_func += ", "
            code_func += ");\n"
            code_func += "        public event cb_" + func_name + "_handle on" + func_name + ";\n\n"
            
            code_func += "        public void " + func_name + "(byte[] _event){\n"
            code_func += "            var _struct = MessagePackSerializer.Deserialize<" + module_name + "_" + func_name + "_struct_req>(_event, op);\n"
            code_func += "            rsp = new rsp_" + func_name + "(current_ch, _struct._cb_uuid, op);\n"
            code_func += "            if (on" + func_name + " != null){\n"
            code_func += "                on" + func_name + "("
            count = 0
            for _type, _name in i[2]:
                code_func += "_struct." + _name
                count = count + 1
                if count < len(i[2]):
                    code_func += ", "
            code_func += ");\n"
            code_func += "            }\n"
            code_func += "            rsp = null;\n"
            code_func += "        }\n\n"

            rsp_code += "    public class rsp_" + func_name + " : abelkhan.Response {\n"
            rsp_code += "        private readonly string uuid;\n"
            rsp_code += "        private readonly MessagePackSerializerOptions op;\n"
            rsp_code += "        public rsp_" + func_name + "(abelkhan.Ichannel _ch, String _uuid, MessagePackSerializerOptions _op) : base(\"rsp_cb_" + module_name + "\", _ch)\n"
            rsp_code += "        {\n"
            rsp_code += "            uuid = _uuid;\n"
            rsp_code += "            op = _op;\n"
            rsp_code += "        }\n\n"

            rsp_code += "        public void rsp("
            for _type, _name in i[4]:
                rsp_code += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name 
                count = count + 1
                if count < len(i[4]):
                    rsp_code += ", "
            rsp_code += "){\n"
            #_argv_uuid = str(uuid.uuid1())
            #_argv_uuid = '_'.join(_argv_uuid.split('-'))
            rsp_code += "            var __argv_struct__ = new " + module_name + "_" + func_name + "_struct_rsp();\n"
            rsp_code += "            __argv_struct__._cb_uuid = uuid;\n"
            for _type, _name in i[4]:
                rsp_code += "            __argv_struct__." + _name + " = " + _name + ";\n"
            rsp_code += "            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__, op);\n"
            rsp_code += "            call_module_method(\"" + func_name + "_rsp\", __byte_struct__);\n"
            rsp_code += "        }\n\n"

            rsp_code += "        public void err("
            count = 0
            for _type, _name in i[6]:
                rsp_code += tools.convert_type(_type, dependent_struct, dependent_enum) + " " + _name
                count = count + 1
                if count < len(i[6]):
                    rsp_code += ", "
            rsp_code += "){\n"
            #_argv_uuid = str(uuid.uuid1())
            #_argv_uuid = '_'.join(_argv_uuid.split('-'))
            rsp_code += "            var __argv_struct__ = new " + module_name + "_" + func_name + "_struct_err();\n"
            rsp_code += "            __argv_struct__._cb_uuid = uuid;\n"
            for _type, _name in i[6]:
                rsp_code += "            __argv_struct__." + _name + " = " + _name + ";\n"
            rsp_code += "            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__, op);\n"
            rsp_code += "            call_module_method(\"" + func_name + "_err\", __byte_struct__);\n"
            rsp_code += "        }\n\n"
            rsp_code += "    }\n\n"

        else:
            raise "func:%s wrong rpc type:%s must req or ntf" % (func_name, i[1])

    code_constructor_end = "        }\n\n"
    code = "    }\n"
        
    return rsp_code + code_constructor + code_constructor_cb + code_constructor_end + code_func + code
        

def genmodule(pretreatment):
    dependent_struct = pretreatment.dependent_struct
    dependent_enum = pretreatment.dependent_enum
    
    modules = pretreatment.module
        
    code = "/*this module code is codegen by abelkhan codegen for c#*/\n"
    for module_name, funcs in modules.items():
        code += gen_module_module(module_name, funcs, dependent_struct, dependent_enum)
                
    return code