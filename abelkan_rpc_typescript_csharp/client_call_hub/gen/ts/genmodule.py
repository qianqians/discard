#coding:utf-8
# 2016-7-1
# build by qianqians
# genmodule
import tools
import genenum
import genstruct
import global_argv

def gen_module_module(module_name, funcs):
        rsp_code = ""

        code = "import event_cb = require(\"../../abelkhan_rpc/node_module/event_cb\");\n"
        code += "export class " + module_name + " extends event_cb.event_cb {\n"
        code += "    private hub_handle : any;\n"
        code += "    private module_name : string;\n"
        code += "    constructor(_hub:any){\n"
        code += "        super();\n\n"
        code += "        this.hub_handle = _hub;\n"
        code += "        this.module_name = \"" + module_name + "\";\n"
        code += "        this.hub_handle.modules.add_module(\"" + module_name + "\", this);\n\n"
        code += "    }\n\n"

        for i in funcs:
                func_name = i[0]

                if i[1] == "ntf":
                        code += "    " + func_name + "("
                        count = 0
                        for item in i[2]:
                                code += "argv" + str(count) + ":" + tools.genprototype(item)
                                count = count + 1
                                if count < len(i[2]):
                                        code += ", "
                        code += "){\n"

                        count = 0
                        for item in i[2]:
                                code += tools.genprotowithlist(item, count)
                                count = count + 1

                        code += "        this.call_event(\"" + func_name + "\", ["
                        count = 0
                        for item in i[2]:
                                code += "input_argv" + str(count)
                                count = count + 1
                                if count < len(i[2]):
                                        code += ", "
                        code += "]);\n"
                elif i[1] == "req" and i[3] == "rsp" and i[5] == "err":
                        code += "    " + func_name + "(uuid:string"
                        count = 0
                        for item in i[2]:
                                code += ", argv" + str(count) + ":" + tools.genprototype(item)
                                count = count + 1
                        code += "){\n"

                        count = 0
                        for item in i[2]:
                                code += tools.genprotowithlist(item, count)
                                count = count + 1

                        code += "        this.hub_handle.modules.rsp = new rsp_" + func_name + "(this.hub_handle, uuid, this.hub_handle.gates.current_client_uuid);\n"

                        code += "        this.call_event(\"" + func_name + "\", ["
                        count = 0
                        for item in i[2]:
                                code += "input_argv" + str(count)
                                count = count + 1
                                if count < len(i[2]):
                                        code += ", "
                        code += "]);\n"

                        rsp_code += "export class rsp_" + func_name + "{\n"
                        rsp_code += "    private hub_handle : any;\n"
                        rsp_code += "    private uuid : string;\n"
                        rsp_code += "    private client_uuid : string;\n"
                        rsp_code += "    constructor(_hub:any, _uuid:string, _client_uuid:string){\n"
                        rsp_code += "        this.hub_handle = _hub;\n"
                        rsp_code += "        this.uuid = _uuid;\n"
                        rsp_code += "        this.client_uuid = _client_uuid;\n"
                        rsp_code += "    }\n\n"

                        rsp_code += "    rsp("
                        count = 0
                        for item in i[4]:
                                rsp_code += "argv" + str(count) + ":" + tools.genlocaltype(item)
                                count = count + 1
                                if count < len(i[4]):
                                        rsp_code += ", "
                        rsp_code += "){\n"
                        count = 0
                        for item in i[4]:
                                rsp_code += tools.genlocaltoproto(item, count)
                                count = count + 1
                        rsp_code += "        this.hub_handle.gates.call_client(this.client_uuid, \"" + module_name + "\", \"" + func_name + "_rsp\", this.uuid"
                        count = 0
                        for item in i[4]:
                                rsp_code += ", input_argv" + str(count)
                                count = count + 1
                        rsp_code += ");\n"
                        rsp_code += "    }\n"

                        rsp_code += "    err("
                        count = 0
                        for item in i[6]:
                                rsp_code += "argv" + str(count) + ":" + tools.genlocaltype(item)
                                count = count + 1
                                if count < len(i[6]):
                                        rsp_code += ", "
                        rsp_code += "){\n"
                        count = 0
                        for item in i[6]:
                                rsp_code += tools.genlocaltoproto(item, count)
                                count = count + 1
                        rsp_code += "        this.hub_handle.gates.call_client(this.client_uuid, \"" + module_name + "\", \"" + func_name + "_err\", this.uuid"
                        count = 0
                        for item in i[6]:
                                rsp_code += ", input_argv" + str(count)
                                count = count + 1
                        rsp_code += ");\n"
                        rsp_code += "    }\n"
                        rsp_code += "}\n"
                else:
                        raise "func:%s wrong rpc type:%s must req or ntf" % (func_name, i[1])

                if i[1] == "ntf":
                        pass
                elif i[1] == "req" and i[3] == "rsp" and i[5] == "err":
                        code += "        this.hub_handle.modules.rsp = null;\n"
                else:
                        raise "func:%s wrong rpc type:%s must req or ntf" % (func_name, i[1])

                code += "    }\n\n"

        code += "}\n"

        return rsp_code + code

def genmodule(file_name, modules, enums, struct):
        global_argv.current_struct_type_list = global_argv.struct_type_list_file[file_name]

        head_code = "/*this rsp file is codegen by abelkhan for ts*/\n\n"

        module_code = ""
        for module_name, module in modules.items():
                module_code += gen_module_module(module_name, module["method"])

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

        return head_code + enum_code + struct_code + module_code
