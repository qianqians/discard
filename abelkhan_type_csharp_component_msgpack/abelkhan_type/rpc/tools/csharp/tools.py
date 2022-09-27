#coding:utf-8
# 2019-12-26
# build by qianqians
# tools

class TypeType():
    Original = 0
    Number = 1
    Custom = 2
    Array = 3

def check_in_dependent(typestr, dependent):
    for _type, _import in dependent:
        if _type == typestr:
            return True
    return False

def get_import(typestr, dependent):
    for _type, _import in dependent:
        if _type == typestr:
            return _import
    return ""

def check_type(typestr, dependent_struct, dependent_enum):
    if typestr == 'int32':
        return TypeType.Number
    elif typestr == 'int64':
        return TypeType.Number
    elif typestr == 'uint32':
        return TypeType.Number
    elif typestr == 'uint64':
        return TypeType.Number
    elif typestr == 'float':
        return TypeType.Number
    elif typestr == 'double':
        return TypeType.Number
    elif typestr == 'string':
        return TypeType.Original
    elif typestr == 'bool':
        return TypeType.Original
    elif check_in_dependent(typestr, dependent_struct):
	    return TypeType.Custom
    elif check_in_dependent(typestr, dependent_enum):
    	return TypeType.Original
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        return TypeType.Array

    raise Exception("non exist type:%s" % typestr)

def convert_type(typestr, dependent_struct, dependent_enum):
    if typestr == 'int32':
        return 'Int32'
    elif typestr == 'int64':
        return 'Int64'
    elif typestr == 'uint32':
        return 'UInt32'
    elif typestr == 'uint64':
        return 'UInt64'
    elif typestr == 'string':
        return 'String'
    elif typestr == 'float':
        return 'Single'
    elif typestr == 'double':
        return 'Double'
    elif typestr == 'bool':
        return 'Boolean'
    elif check_in_dependent(typestr, dependent_struct):
	    return typestr
    elif check_in_dependent(typestr, dependent_enum):
    	return typestr
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        array_type = typestr[:-2]
        array_type = convert_type(array_type, dependent_struct, dependent_enum)
        return 'List<' + array_type+'>'

    raise Exception("non exist type:%s" % typestr)
    