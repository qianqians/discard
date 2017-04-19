# 2014-12-24
# build by qianqians
# codegenclient

import os
import argvs
from gentools import maketypetocpptype, makeret, makecallbackret

def codegencaller(filelist):
    if not os.path.isdir(argvs.build_path):
        os.mkdir(argvs.build_path)
    if not os.path.isdir(argvs.build_path + 'caller'):
        os.mkdir(argvs.build_path + 'caller')

    file = open('notes.txt', 'r')
    note = file.read()

    defmodulelist = []

    for filename, list in filelist.items():
        code = '#include <juggle.h>\n#include <boost/make_shared.hpp>\n\n'

        struct = list['struct']
        module = list['module']

        if len(struct) > 0:
            code += '#include \"../struct/' + filename +  'struct.h' + '\"'

        for k, v in module.items():
            if k in defmodulelist:
                raise 'redefined module %s' % k
            code += 'namespace sync{\n\n'
            code += 'class ' + k + ': public ' + 'Fossilizid::juggle::caller' + '{\n' + 'public:\n'
            code += '	' + k + '(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, \"' + k + '\"){\n' + '	}\n\n'
            code += '	~' + k + '(){\n' + '	}\n\n'
            for func in v:
                code += '	' + maketypetocpptype(func[0]) + ' ' + func[1] + '('
                if len(func) > 2:
                    code += maketypetocpptype(func[2][0]) + ' ' + func[2][1]
                for argv in func[3:]:
                    code += ',' + maketypetocpptype(argv[0]) + ' ' + argv[1]
                code += '){\n'
                code += '		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();\n'
                for argv in func[2:]:
                    code += '		(*v)[\"' + argv[1] + '\"] = ' + argv[1] + ';\n'
                code += '		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync(\"' + k + '_' + func[1] + '\", v);\n'
                code += makeret(func[0], struct) + '\n'
                code += '	}\n\n'
            code += '};\n\n'
            code += '}\n\n'

            code += 'namespace async{\n\n'
            code += 'class ' + k + ': public ' + 'Fossilizid::juggle::caller' + '{\n' + 'public:\n'
            code += '	' + k + '(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, \"' + k + '\"' + '){\n	}\n\n'
            code += '	~' + k + '(){\n	}\n\n'
            for func in v:
                code += '	' + maketypetocpptype(func[0]) + ' ' + func[1] + '('
                for argv in func[2:]:
                    code += maketypetocpptype(argv[0]) + ' ' + argv[1] + ', '
                code += 'boost::function<void(' + maketypetocpptype(func[0]) + ')> callback){\n'
                code += '		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();\n'
                for argv in func[2:]:
                    code += '		(*v)[\"' + argv[1] + '\"] = ' + argv[1] + ';\n'
                code += '		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){\n'
                if func[0] != 'void':
                    code += '   ' + makecallbackret(func[0], struct)
                    code += '			callback(ret);\n        };\n'
                else:
                    code += '		};\n'
                code += '		call_module_method_async(\"' + k + '_' + func[1] + '\", v, cb' + ');\n'
                code += '	}\n\n'
            code += '};\n\n'
            code += '}\n\n'
            defmodulelist.append(k)

        if code != '#include <juggle.h>\n#include <boost/make_shared.hpp>\n\n':
            file = open(argvs.build_path + 'caller\\' + filename + 'caller.h', 'w')
            file.write(note + code)