#coding:utf-8
# 2018-5-16
# build by qianqians
# gen_common_impl

import genenum
import genstruct
import global_argv

def gen(file_name, enums, struct):
        global_argv.current_struct_type_list = global_argv.struct_type_list_file[file_name]

        head_code = "/*this common file is codegen by abelkhan for ts*/\n\n"

        enum_code = ""
        for enum_name, enum_key_values in enums.items():
                enum_code += genenum.genenum(enum_name, enum_key_values)

        struct_code = ""
        for struct_name, elems in struct.items():
                struct_code += genstruct.genstruct(struct_name, elems)

        if file_name in global_argv.quote_file_list:
                global_argv.quote_file_list.remove(file_name)
        for file_name_import in global_argv.quote_file_list:
                head_code += "import " + file_name_import + " = require(\"./" + file_name_import + "_common\");\n"
        head_code += "\n"

        return head_code + enum_code + struct_code
