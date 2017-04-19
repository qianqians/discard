# 2014-12-24
# build by qianqians
# rpcmake

import os
import argvs
from codegencaller import codegencaller
from codegenmodule import codegenmodule
from codegenstruct import codegenstruct

import deletenote
import statemachine

def traversalclass(dirpath):
    filelist = {}

    for filename in os.listdir(dirpath):
        fname = os.path.splitext(filename)[0]
        fex = os.path.splitext(filename)[1]
        if fex == '.juggle':
            file = open(dirpath+filename, 'r')
            genfilestr = deletenote.deletenote(file.readlines())

            smc = statemachine.statemachine()
            smc.syntaxanalysis(genfilestr)
            module = smc.getmodule()
            struct = smc.getstruct()

            filelist[fname] = {}
            filelist[fname]['module'] = module
            filelist[fname]['struct'] = struct

    codegencaller(filelist)
    codegenmodule(filelist)
    codegenstruct(filelist)

if __name__ == '__main__':
    import sys
    argvs.build_path = sys.argv[2]
    traversalclass(sys.argv[1])
