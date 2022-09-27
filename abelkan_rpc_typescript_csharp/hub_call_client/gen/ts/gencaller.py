#coding:utf-8
# 2018-3-16
# build by qianqians
# genmodule
import tools
import genenum
import genstruct
import global_argv

def gen_module_caller(module_name, funcs):
        code = "export class " + module_name + "{\n"
        code += "    private hub_handle : any;\n"
        code += "    constructor(_hub_ptr:any){\n"
        code += "        this.hub_handle = _hub_ptr;\n"
        code += "    }\n"
        code += "    get_client(uuid:string){\n"
        code += "        return new " + module_name + "_cliproxy(uuid, this.hub_handle);\n"
        code += "    }\n"
        code += "    get_multicast(uuids:string[]){\n"
        code += "        return new " + module_name + "_cliproxy_multi(uuids, this.hub_handle);\n"
        code += "    }\n"
        code += "    get_broadcast(){\n"
        code += "        return new " + module_name + "_broadcast(this.hub_handle);\n"
        code += "    }\n"
        code += "}\n"

        cp_code = "export class " + module_name + "_cliproxy{\n"
        cp_code += "    private hub_handle : any;\n"
        cp_code += "    private uuid : string;\n"
        cp_code += "    constructor(_uuid:string, hub_ptr:any){\n"
        cp_code += "        this.hub_handle = hub_ptr;\n"
        cp_code += "        this.uuid = _uuid;\n"
        cp_code += "    }\n\n"

        cm_code = "export class " + module_name + "_cliproxy_multi{\n"
        cm_code += "    private hub_handle:any;\n"
        cm_code += "    private uuids:string[];\n"
        cm_code += "    constructor(_uuids:string[], hub_ptr:any){\n"
        cm_code += "        this.hub_handle = hub_ptr;\n"
        cm_code += "        this.uuids = _uuids;\n"
        cm_code += "    }\n"

        cb_code = "export class " + module_name + "_broadcast{\n"
        cb_code += "    private hub_handle:any;\n"
        cb_code += "    constructor(hub_ptr:any){\n"
        cb_code += "        this.hub_handle = hub_ptr;\n"
        cb_code += "    }\n"

        for i in funcs:
                func_name = i[0]

                if i[1] != "ntf" and i[1] != "multicast" and i[1] != "broadcast":
                        raise Exception("func:" + func_name + " wrong rpc type:" + i[1] + ", must ntf or broadcast")

                argvs1 = ""
                count = 0
                for item in i[2]:
                        argvs1 += "argv" + str(count) + ":" + tools.genlocaltype(item)
                        count = count + 1
                        if count < len(i[2]):
                            argvs1 += ", "

                argvs = ""
                count = 0
                for item in i[2]:
                        argvs += ", input_argv" + str(count)
                        count = count + 1

                tmp_code = "    " + func_name + "("
                tmp_code += argvs1
                tmp_code += "){\n"

                count = 0
                for item in i[2]:
                        tmp_code += tools.genlocaltoproto(item, count)
                        count = count + 1

                if i[1] == "ntf":
                        cp_code += tmp_code
                        cp_code += "        this.hub_handle.gates.call_client(this.uuid, \"" + module_name + "\", \"" + func_name + "\"" + argvs + ");\n"
                        cp_code += "    }\n"
                elif i[1] == "multicast":
                        cm_code += tmp_code
                        cm_code += "        this.hub_handle.gates.call_group_client(this.uuids, \"" + module_name + "\", \"" + func_name + "\"" + argvs + ");\n"
                        cm_code += "    }\n"
                elif i[1] == "broadcast":
                        cb_code += tmp_code
                        cb_code += "        this.hub_handle.gates.call_global_client(\"" + module_name + "\", \"" + func_name + "\"" + argvs + ");\n"
                        cb_code += "    }\n"

        cp_code += "}\n"
        cm_code += "}\n"
        cb_code += "}\n"

        return cp_code + cm_code + cb_code + code

def gencaller(file_name, modules, enums, struct):
        global_argv.current_struct_type_list = global_argv.struct_type_list_file[file_name]

        code = "/*this ntf file is codegen by ablekhan for js*/\n\n"

        module_code = ""
        for module_name, module in modules.items():
                module_code += gen_module_caller(module_name, module["method"])

        enum_code = ""
        for enum_name, enum_key_values in enums.items():
                enum_code += genenum.genenum(enum_name, enum_key_values)

        struct_code = ""
        for struct_name, elems in struct.items():
                struct_code += genstruct.genstruct(struct_name, elems)

        if file_name in global_argv.quote_file_list:
                global_argv.quote_file_list.remove(file_name)
        for file_name_import in global_argv.quote_file_list:
                code += "import " + file_name_import + " = require(\"./" + file_name_import + "_common\");\n"
        code += "\n"

        return code + enum_code + struct_code + module_code
