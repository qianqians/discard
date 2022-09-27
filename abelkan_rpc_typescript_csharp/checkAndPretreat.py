#coding:utf-8
# # 2018-5-15
# build by qianqians
# checkAndPretreat

import global_argv

def checkAndPretreat(fname, module, enum, struct, defmodulelist, module_type):
        struct_type_list = []
        modules = {}
        for module_name, module_info in module.items():
                if module_name in defmodulelist:
                        raise Exception('redefined module %s' % module_name)

                if module_name in global_argv.ban_keyworld:
                        raise Exception('invalid module name %s' % module_name)

                if module_info["module_type"] != module_type:
                        raise Exception('%s has wrong module type %s' % (module_name, module_info["module_type"]))

                for func in module_info["method"]:
                        if func[0] in global_argv.ban_keyworld:
                            raise Exception('invalid func name %s' % func[0])

                        if module_name != func[0]:
                                continue
                        raise Exception('func name %s can not be modele name: %s' % (func[0], module_name))

                defmodulelist.append(module_name)
                modules[module_name] = module_info#["method"]

        for enum_name, enums in enum.items():
                if enum_name in defmodulelist:
                        raise Exception('redefined enum %s' % enum_name)

                if enum_name in global_argv.ban_keyworld:
                        raise Exception('invalid enum name %s' % enum_name)

                for enum_elem in enums:
                        key, value = enum_elem
                        if key in global_argv.ban_keyworld:
                            raise Exception('invalid enum name %s' % key)
                        if enum_name != key:
                                continue
                        raise Exception('enum element name %s can not be enum name: %s' % (key, enum_name))

                defmodulelist.append(enum_name)

        for struct_name, elems in struct.items():
                if struct_name in defmodulelist:
                        raise Exception('redefined struct %s' % struct_name)

                if struct_name in global_argv.ban_keyworld:
                        raise Exception('invalid struct name %s' % struct_name)

                for element in elems:
                        _type, _name = element
                        if _name in global_argv.ban_keyworld:
                            raise Exception('invalid struct element name %s' % _name)
                        if struct_name != _name:
                                continue
                        raise Exception('struct element name %s can not be struct name: %s' % (_name, struct_name))

                defmodulelist.append(struct_name)
                struct_type_list.append(struct_name)
                global_argv.struct_type_list.append(struct_name)

        global_argv.struct_type_list_file[fname] = struct_type_list

        return modules
