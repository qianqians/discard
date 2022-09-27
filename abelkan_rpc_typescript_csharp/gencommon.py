#coding:utf-8
# 2018-3-16
# build by qianqians
# genjs

import sys
sys.path.append("./")
sys.path.append("./parser")

import os
import jparser

from checkAndPretreatCommon import *

def gen(inputdir, lang, outputdir):
        syspath = "./common/"
        c_suffix = ""
        if lang == 'csharp':
                sys.path.append("./struct/csharp")
                sys.path.append("./enum/csharp")
                sys.path.append("./tools/csharp")
                syspath += "csharp/"
                c_suffix = "cs"
        elif lang == 'ts':
                sys.path.append("./struct/ts")
                sys.path.append("./enum/ts")
                sys.path.append("./tools/ts")
                syspath += "ts/"
                c_suffix = "ts"
        sys.path.append(syspath)
        import gen_common_impl
        sys.path.remove(syspath)

        if not os.path.isdir(outputdir):
                os.mkdir(outputdir)

        defmodulelist = []
        lexical_tree = []
        for filename in os.listdir(inputdir):
                fname = os.path.splitext(filename)[0]
                fex = os.path.splitext(filename)[1]
                if fex != '.juggle':
                        continue

                file = open(inputdir + '//' + filename, 'r')
                genfilestr = file.readlines()

                module, enum, struct = jparser.parser(genfilestr)
                checkAndPretreatCommon(fname, module, enum, struct, defmodulelist)
                lexical_tree.append((fname, enum, struct))

        for fname, enum, struct in lexical_tree:
                global_argv.quote_file_list = []
                callercode = gen_common_impl.gen(fname, enum, struct)
                file = open(outputdir + '//' + fname + '_common.' + c_suffix, 'w')
                file.write(callercode)
                file.close()

if __name__ == '__main__':
        gen(sys.argv[1], sys.argv[2], sys.argv[3])
