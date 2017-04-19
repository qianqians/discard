# 2015-4-26
# build by qianqians
# codegenclient

import os
import argvs
from gentools import maketypetocpptype, makevalue

def codegenstruct(filelist):
    file = open('notes.txt', 'r')
    note = file.readlines()

    if not os.path.isdir(argvs.build_path):
        os.mkdir(argvs.build_path)
    if not os.path.isdir(argvs.build_path + 'struct'):
        os.mkdir(argvs.build_path + 'struct')

    defstructlist = []

    for filename, list in filelist.items():
        code = '#include <juggle.h>\n\n'

        struct = list['struct']

        for k, v in struct.items():
            if k in defstructlist:
                raise 'redefined struct %s'% k
            code += 'struct ' + k + '{\n'
            for argv in v:
                code += '	' + maketypetocpptype(argv[0]) + ' ' + argv[1] + ';'
            code += '};\n\n'
            defstructlist.append(k)

        if code != '#include <juggle.h>\n\n':
            file = open(argvs.build_path + '../struct/' + filename + 'struct.h', 'w')
            file.write(note + code)
