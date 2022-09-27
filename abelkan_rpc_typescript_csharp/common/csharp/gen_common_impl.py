#coding:utf-8
# 2018-5-16
# build by qianqians
# gen_common_impl

import genstruct
import genenum

def gen(file_name, enums, struct):
        head_code = "/*this common file is codegen by abelkhan for c#*/\n"
        head_code += "using System;\n"
        head_code += "using System.Collections;\n"
        head_code += "using System.Collections.Generic;\n\n"

        head_code += "namespace abelkhan_code_gen\n"
        head_code += "{\n"

        end_code = "}\n"

        enum_code = ""
        for enum_name, enum_key_values in enums.items():
                enum_code += genenum.genenum(enum_name, enum_key_values)

        struct_code = ""
        for struct_name, elems in struct.items():
                struct_code += genstruct.genstruct(struct_name, elems)

        return head_code + enum_code + struct_code + end_code
