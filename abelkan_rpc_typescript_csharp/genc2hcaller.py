#coding:utf-8
# 2018-3-16
# build by qianqians
# genjs

import sys
sys.path.append("./")
sys.path.append("./parser")

import os
import jparser

from checkAndPretreat import *
from checkAndPretreatCommon import *

def gen(dependentdir, inputdir, lang, outputdir):
        syspath = "./client_call_hub/gen/"
        c_suffix = ""
        if lang == 'csharp':
                sys.path.append("./struct/csharp")
                sys.path.append("./tools/csharp")
                sys.path.append("./enum/csharp")
                syspath += "csharp/"
                c_suffix = "cs"
        sys.path.append(syspath)
        import gencaller
        sys.path.remove(syspath)

        if not os.path.isdir(outputdir):
                os.mkdir(outputdir)

        defmodulelist = []
        for filename in os.listdir(dependentdir):
                fname = os.path.splitext(filename)[0]
                fex = os.path.splitext(filename)[1]
                if fex != '.juggle':
                        continue

                file = open(dependentdir + '//' + filename, 'r')
                genfilestr = file.readlines()

                module, enum, struct = jparser.parser(genfilestr)
                checkAndPretreatCommon(fname, module, enum, struct, defmodulelist)

        lexical_tree = []
        for filename in os.listdir(inputdir):
                fname = os.path.splitext(filename)[0]
                fex = os.path.splitext(filename)[1]
                if fex != '.juggle':
                        continue

                file = open(inputdir + '//' + filename, 'r')
                genfilestr = file.readlines()

                module, enum, struct = jparser.parser(genfilestr)
                modules = checkAndPretreat(fname, module, enum, struct, defmodulelist, "client_call_hub")
                lexical_tree.append((fname, modules, enum, struct))

        for fname, modules, enum, struct in lexical_tree:
                global_argv.quote_file_list = []
                callercode = gencaller.gencaller(fname, modules, enum, struct)
                file = open(outputdir + '//' + fname + '_caller.' + c_suffix, 'w')
                file.write(callercode)
                file.close()

if __name__ == '__main__':
        gen(sys.argv[1], sys.argv[2], sys.argv[3], sys.argv[4])
