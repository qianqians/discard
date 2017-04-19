# 2014-12-24
# build by qianqians
# codegenclient

import os
import argvs
from gentools import maketypetocpptype, makevalue

def codegenmodule(filelist):
	file = open('notes.txt', 'r')
	note = file.read()

	if not os.path.isdir(argvs.build_path):
		os.mkdir(argvs.build_path)
	if not os.path.isdir(argvs.build_path + 'module'):
		os.mkdir(argvs.build_path + 'module')

	defmodulelist = []

	for filename, list in filelist.items():
		code = note + '#include <juggle.h>\n#include <boost/make_shared.hpp>\n#include <string>\n\n'

		struct = list['struct']
		module = list['module']

		if len(struct) > 0:
			code += '#include \"../struct/' + filename +  'struct.h' + '\"'

		for k, v in module.items():
			if k in defmodulelist:
				raise 'redefined module %s' % k
			code += 'namespace module{\n\nclass ' + k + ': public ' + 'Fossilizid::juggle::module' + '{\n' + 'public:\n'
			code += '	' + k + '(boost::shared_ptr<Fossilizid::juggle::process> __process) : module(__process, \"' + k + '\", Fossilizid::uuid::UUID()' + '){\n'
			for func in v:
				code += '		_module_func.push_back(\"' + k + '_' + func[1] + '\");\n'
			for func in v:
				code += '		__process->register_module_method(\"' + k + '_' + func[1] + '\",' + ' boost::bind(' + '&' + k + '::' + 'call_' + func[1] + ', this, _1, _2)' + ');\n'
			code += '	}\n\n'
			code += '	' + '~' + k + '(){\n	}\n\n'
			for func in v:
				code += '	boost::signals2::signal< ' + maketypetocpptype(func[0]) + '('
				if len(func) > 2:
					code += maketypetocpptype(func[2][0]) + ' ' + func[2][1]
				for argv in func[3:]:
					code += ',' + maketypetocpptype(argv[0]) + ' ' + argv[1]
				code += ')> sig'  + func[1] + ';\n\n'
				code += '	void call_' + func[1] + '(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){\n'
				for argv in func[2:]:
					code += makevalue(argv[0], argv[1], struct)
				if func[0] != "void":
					code += '		auto ret = sig' + func[1] + '('
				else:
					code += '		sig' + func[1] + '('
				if len(func) > 2:
					code += func[2][1]
				for argv in func[3:]:
					code += ', ' + argv[1]
				code += ');\n'
				code += '		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();\n'
				code += '		(*r)[\"suuid\"] = boost::any_cast<std::string>((*v)[\"suuid\"]);\n'
				code += '		(*r)[\"method\"] = boost::any_cast<std::string>((*v)[\"method\"]);\n'
				code += '		(*r)[\"rpcevent\"] = \"reply_rpc_method\";\n\n'
				if func[1].find('std::vector') != -1:
					code += '		for(auto v : ret){\n'
					code += '			(*r)[\"ret\"].append(v);\n'
					code += '		}\n'
				else:
					if func[0] != "void":
						code += '		(*r)[\"ret\"] = ret;\n'
				code += '		ch->push(r);\n'
				code += '	}\n\n'
			code += '};\n\n}\n'

		if code != '#include <juggle.h>\n#include <boost/make_shared.hpp>\n\n':
			file = open(argvs.build_path + 'module/' + filename + 'module.h', 'w')
			file.write(note + code)
