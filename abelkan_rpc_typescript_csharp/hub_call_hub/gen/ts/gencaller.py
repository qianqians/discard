#coding:utf-8
# 2016-7-1
# build by qianqians
# gencaller
import tools
import genenum
import genstruct
import global_argv

def gen_module_caller(module_name, funcs):
        cb_func = ""

        cb_code = "/*this cb code is codegen by abelkhan for js*/\n"
        cb_code += "export class cb_" + module_name + "_handle{\n"
        cb_code += "    constructor(){\n"
        cb_code_section = ""

        code = "export class " + module_name + "{\n"
        code += "    private hub_handle:any;\n"
        code += "    private _cb_" + module_name + "_handle:cb_" + module_name + "_handle;\n"
        code += "    constructor(_hub_handle:any){\n"
        code += "        this.hub_handle = _hub_handle;\n"
        code += "        this._cb_" + module_name + "_handle = new cb_" + module_name + "_handle();\n"
        code += "        this.hub_handle.modules.add_module(\"cb_" + module_name + "\", this._cb_" + module_name + "_handle);\n\n"
        code += "    }\n"

        code += "    get_hub(hub_name:string){\n"
        code += "        return new " + module_name + "_hubproxy(hub_name, this.hub_handle, this._cb_" + module_name + "_handle);\n"
        code += "    }\n"
        code += "}\n\n"

        code += "export class " + module_name + "_hubproxy{\n"
        code += "    private hub_name:string;\n"
        code += "    private hub_handle:any;\n"
        code += "    private " + module_name + "_handle:cb_" + module_name + "_handle;\n"
        code += "    constructor(_hub_name:string, _hub_handle:any, _" + module_name + "_handle:cb_" + module_name + "_handle){\n"
        code += "        this.hub_name = _hub_name;\n"
        code += "        this.hub_handle = _hub_handle;\n"
        code += "        this." + module_name + "_handle = _" + module_name + "_handle;\n"
        code += "    }\n\n"

        for i in funcs:
                func_name = i[0]
                if i[1] == "ntf":
                        code += "    " + func_name + "("
                        count = 0
                        for item in i[2]:
                                code += "argv" + str(count) + ":" + tools.genlocaltype(item)
                                count = count + 1
                                if count < len(i[2]):
                                        code += ", "
                        code += "){\n"
                        count = 0
                        for item in i[2]:
                                code += tools.genlocaltoproto(item, count)
                                count = count + 1
                        code += "        this.hub_handle.hubs.call_hub(this.hub_name, \"" + module_name + "\", \"" + func_name + "\""
                        count = 0
                        for item in i[2]:
                                code += ", input_argv" + str(count)
                                count = count + 1
                        code += ");\n    }\n\n"
                elif i[1] == "req" and i[3] == "rsp" and i[5] == "err":
                        code += "    " + func_name + "("
                        count = 0
                        for item in i[2]:
                                code += "argv" + str(count) + ":" + tools.genlocaltype(item)
                                count = count + 1
                                if count < len(i[2]):
                                        code += ", "
                        code += "){\n"
                        count = 0
                        for item in i[2]:
                                code += tools.genlocaltoproto(item, count)
                                count = count + 1
                        code += "        const uuidv1 = require('uuid/v1');\n"
                        code += "        var uuid = uuidv1();\n\n"
                        code += "        this.hub_handle.hubs.call_hub(this.hub_name, \"" + module_name + "\", \"" + func_name + "\", this.hub_handle.name, uuid"
                        count = 0
                        for item in i[2]:
                                code += ", input_argv" + str(count)
                                count = count + 1
                        code += ");\n\n"
                        code += "        var cb_" + func_name + "_obj = new cb_" + func_name + "();\n"
                        code += "        this." + module_name + "_handle.map_" + func_name + "[uuid] = cb_" + func_name + "_obj;\n\n"
                        code += "        return cb_" + func_name + "_obj;\n    }\n\n"

                        cb_code += "        this.map_" + func_name + " = {};\n"

                        cb_code_section += "    public map_" + func_name + " : any;\n"
                        cb_code_section += "    " + func_name + "_rsp("
                        cb_code_section += "uuid:string"
                        count = 0
                        for item in i[4]:
                                cb_code_section += ", argv" + str(count) + ":" + tools.genprototype(item)
                                count = count + 1
                        cb_code_section += ")\n    {\n"
                        count = 0
                        for item in i[4]:
                                cb_code_section += tools.genprotowithlist(item, count)
                                count = count + 1
                        cb_code_section += "        var rsp = this.map_" + func_name + "[uuid];\n"
                        cb_code_section += "        rsp.cb("
                        count = 0
                        for item in i[4]:
                                cb_code_section += "input_argv" + str(count)
                                count = count + 1
                                if count < len(i[4]):
                                        cb_code_section += ", "
                        cb_code_section += ");\n"
                        cb_code_section += "    }\n\n"

                        cb_code_section += "    " + func_name + "_err("
                        cb_code_section += "uuid:string"
                        count = 0
                        for item in i[6]:
                                cb_code_section += ", argv" + str(count) + ":" + tools.genprototype(item)
                                count = count + 1
                                if count < len(i[6]):
                                        cb_code_section += ", "
                        cb_code_section += ")\n    {\n"
                        count = 0
                        for item in i[6]:
                                cb_code_section += tools.genprotowithlist(item, count)
                                count = count + 1
                        cb_code_section += "        var rsp = this.map_" + func_name + "[uuid];\n"
                        cb_code_section += "        rsp.err("
                        count = 0
                        for item in i[6]:
                                cb_code_section += "input_argv" + str(count)
                                count = count + 1
                                if count < len(i[6]):
                                        cb_code_section += ", "
                        cb_code_section += ");\n"
                        cb_code_section += "    }\n\n"

                        cb_func += "export class cb_" + func_name + "{\n"
                        cb_func += "    private event_" + func_name + "_handle_cb:Function|null;\n"
                        cb_func += "    private event_" + func_name + "_handle_err:Function|null;\n"
                        cb_func += "    constructor(){\n"
                        cb_func += "        this.event_" + func_name + "_handle_cb = null;\n"
                        cb_func += "        this.event_" + func_name + "_handle_err = null;\n"
                        cb_func += "    }\n\n"
                        cb_func += "    cb("
                        count = 0
                        for item in i[4]:
                                cb_func += "argv" + str(count) + ":" + tools.genlocaltype(item)
                                count = count + 1
                                if count < len(i[4]):
                                        cb_func += ", "
                        cb_func += ")\n    {\n"
                        cb_func += "        if (this.event_" + func_name + "_handle_cb !== null)\n        {\n"
                        cb_func += "            this.event_" + func_name + "_handle_cb("
                        count = 0
                        for item in i[4]:
                                cb_func += "argv" + str(count)
                                count = count + 1
                                if count < len(i[4]):
                                        cb_func += ", "
                        cb_func += ");\n        }\n"
                        cb_func += "    }\n\n"

                        cb_func += "    err("
                        count = 0
                        for item in i[6]:
                                cb_func += "argv" + str(count) + ":" + tools.genlocaltype(item)
                                count = count + 1
                                if count < len(i[6]):
                                        cb_func += ", "
                        cb_func += ")\n    {\n"
                        cb_func += "        if (this.event_" + func_name + "_handle_err != null)\n        {\n"
                        cb_func += "            this.event_" + func_name + "_handle_err("
                        count = 0
                        for item in i[6]:
                                cb_func += "argv" + str(count)
                                count = count + 1
                                if count < len(i[6]):
                                        cb_func += ", "
                        cb_func += ");\n        }\n"
                        cb_func += "    }\n\n"

                        cb_func += "    callBack(_cb:Function, _err:Function)\n    {\n"
                        cb_func += "        this.event_" + func_name + "_handle_cb = _cb;\n"
                        cb_func += "        this.event_" + func_name + "_handle_err = _err;\n"
                        cb_func += "    }\n"
                        cb_func += "}\n\n"
                else:
                        raise Exception("func:" + func_name + " wrong rpc type:" + i[1] + ", must req or ntf")

        cb_code += "    }\n\n"
        cb_code_section += "}\n\n"
        code += "}\n"

        return cb_func + cb_code + cb_code_section + code

def gencaller(file_name, modules, enums, struct):
        global_argv.current_struct_type_list = global_argv.struct_type_list_file[file_name]

        head_code = "/*this req file is codegen by abelkhan for js*/\n\n"

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
                head_code += "import " + file_name_import + " = require(\"./" + file_name_import + "_common\");\n"
        head_code += "\n"

        return head_code + enum_code + struct_code + module_code
