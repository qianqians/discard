#coding:utf-8
# 2019-12-26
# build by qianqians
# tools

class TypeType():
    Original = 0
    Custom = 1
    Array = 2
    String = 3
    Int32 = 4
    Int64 = 5
    Uint32 = 6
    Uint64 = 7
    Float = 8
    Double = 9
    Bool = 10

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
        return TypeType.Int32
    elif typestr == 'int64':
        return TypeType.Int64
    elif typestr == 'uint32':
        return TypeType.Uint32
    elif typestr == 'uint64':
        return TypeType.Uint64
    elif typestr == 'string':
        return TypeType.Original
    elif typestr == 'float':
        return TypeType.Float
    elif typestr == 'double':
        return TypeType.Double
    elif typestr == 'bool':
        return TypeType.Bool
    elif check_in_dependent(typestr, dependent_struct):
	    return TypeType.Custom
    elif check_in_dependent(typestr, dependent_enum):
    	return TypeType.Original
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        return TypeType.Array

    raise Exception("non exist type:%s" % typestr)

def convert_type(typestr, dependent_struct, dependent_enum):
    if typestr == 'int32':
        return 'int32_t'
    elif typestr == 'int64':
        return 'int64_t'
    elif typestr == 'uint32':
        return 'uint32_t'
    elif typestr == 'uint64':
        return 'uint64_t'
    elif typestr == 'string':
        return 'std::string'
    elif typestr == 'float':
        return 'float'
    elif typestr == 'double':
        return 'double'
    elif typestr == 'bool':
        return 'bool'
    elif check_in_dependent(typestr, dependent_struct):
	    return typestr
    elif check_in_dependent(typestr, dependent_enum):
    	return typestr
    elif typestr[len(typestr)-2] == '[' and typestr[len(typestr)-1] == ']':
        array_type = typestr[:-2]
        array_type = convert_type(array_type, dependent_struct, dependent_enum)
        return 'std::vector<' + array_type+'>'

    raise Exception("non exist type:%s" % typestr)
    
