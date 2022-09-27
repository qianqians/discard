#coding:utf-8
# 2018-5-15
# build by qianqians
# global_argv

struct_type_list_file = {}
struct_type_list = []

quote_file_list = []

current_struct_type_list = None

ban_keyworld = ['class', 'struct', 'new', 'public', 'private', 'protected', 'base',
                'extends', 'internal', 'for', 'if', 'else', 'break', 'continue', 'self',
                'super', 'goto', 'elif', 'then', 'int', 'int64_t', 'int32_t', 'int16_t',
                'char', 'byte', 'Byte', 'Int64', 'Int32', 'Int16', 'float', 'double', 'Single',
                'Double', 'System', 'bool', 'Boolean', 'null']