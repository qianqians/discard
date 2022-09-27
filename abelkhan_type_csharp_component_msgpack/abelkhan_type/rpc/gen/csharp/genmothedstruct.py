#coding:utf-8
# 2020-6-12
# build by qianqians
# genmothedstruct

import uuid
import tools

def gen_mothed_struct(module_name, funcs, dependent_struct, dependent_enum):
    code = "/*this struct code is codegen by abelkhan for c#*/\n"
    for i in funcs:
        func_name = i[0]
        if i[1] == "ntf":
            code += "    [MessagePackObject]\n"
            code += "    public class " + module_name + "_" + func_name + "_struct_ntf\n    {\n"
            count = 0
            for _type, _name in i[2]:
                _type_ = tools.convert_type(_type, dependent_struct, dependent_enum)
                code += "        [Key(" + str(count) + ")]\n"
                code += "        public " + _type_ + " " + _name + ";\n"
                count += 1
            code += "    }\n\n" 
        elif i[1] == "req" and i[3] == "rsp" and i[5] == "err":
            code += "    [MessagePackObject]\n"
            code += "    public class " + module_name + "_" + func_name + "_struct_req\n    {\n"
            count = 0
            code += "        [Key(" + str(count) + ")]\n"
            code += "        public string _cb_uuid;\n"
            count += 1
            for _type, _name in i[2]:
                _type_ = tools.convert_type(_type, dependent_struct, dependent_enum)
                code += "        [Key(" + str(count) + ")]\n"
                code += "        public " + _type_ + " " + _name + ";\n"
                count += 1
            code += "    }\n\n" 

            code += "    [MessagePackObject]\n"
            code += "    public class " + module_name + "_" + func_name + "_struct_rsp\n    {\n"
            count = 0
            code += "        [Key(" + str(count) + ")]\n"
            code += "        public string _cb_uuid;\n"
            count += 1
            for _type, _name in i[4]:
                _type_ = tools.convert_type(_type, dependent_struct, dependent_enum)
                code += "        [Key(" + str(count) + ")]\n"
                code += "        public " + _type_ + " " + _name + ";\n"
                count += 1
            code += "    }\n\n" 

            code += "    [MessagePackObject]\n"
            code += "    public class " + module_name + "_" + func_name + "_struct_err\n    {\n"
            count = 0
            code += "        [Key(" + str(count) + ")]\n"
            code += "        public string _cb_uuid;\n"
            count += 1
            for _type, _name in i[6]:
                _type_ = tools.convert_type(_type, dependent_struct, dependent_enum)
                code += "        [Key(" + str(count) + ")]\n"
                code += "        public " + _type_ + " " + _name + ";\n"
                count += 1
            code += "    }\n\n" 
        
    return code

def genmothedstruct(pretreatment):
    dependent_struct = pretreatment.dependent_struct
    dependent_enum = pretreatment.dependent_enum
    
    modules = pretreatment.module
        
    code = "/*this module code is codegen by abelkhan codegen for c#*/\n"
    for module_name, funcs in modules.items():
        code += gen_mothed_struct(module_name, funcs, dependent_struct, dependent_enum)
                
    return code
