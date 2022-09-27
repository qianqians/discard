#coding:utf-8
# 2019-12-27
# build by qianqians
# genstruct

import tools
import uuid

def genmainstruct(struct_name, elems, dependent_struct, dependent_enum):
    code = "    [MessagePackObject]\n"
    code += "    public class " + struct_name + "\n    {\n"
    names = []
    count = 0
    for key, value in elems:
        if value in names:
            raise Exception("repeat struct elem:%s in struct:%s" % (key, struct_name))
        names.append(value)
        code += "        [Key(" + str(count) + ")]\n"
        code += "        public " + tools.convert_type(key, dependent_struct, dependent_enum) + " " + value + ";\n"
        count += 1
    code += "    }\n\n" 
    return code

def genstruct(pretreatment):
    dependent_struct = pretreatment.dependent_struct
    dependent_enum = pretreatment.dependent_enum
    
    struct = pretreatment.struct
    
    code = "/*this struct code is codegen by abelkhan codegen for c#*/\n"
    for struct_name, elems in struct.items():
        code += genmainstruct(struct_name, elems, dependent_struct, dependent_enum)
    return code
