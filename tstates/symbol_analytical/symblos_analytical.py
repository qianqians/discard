# -*- coding: utf-8 -*-

import os
import sys

def analytical(file):
    symlist = {}
    symlistret = []
    syms = os.popen('nm -C %s'%file).readlines()
    for sym in syms:
        index = sym.find('T')
        index1 = sym.find('(')
        index2 = sym.find(')')
        if index != -1 and index1 != -1 and index2 != -1:
           symlist[sym[:index-1]] = sym[index+2:]
    print(repr(symlist))
    syms = os.popen('nm %s'%file).readlines()
    for sym in syms:
        index = sym.find('T')
        if index != -1:
            print(sym[:index-1])    
            if sym[:index-1] in symlist:
               symlistret.append((symlist[sym[:index-1]], sym[index+2:]))
    print(repr(symlistret))
    return symlistret

def analysis(sym):
    print(sym)
    index1 = sym.find('(')
    index2 = sym.find(')')
    argvdef = sym[index1+1:index2]
    while(1):
        index11 = argvdef.find('<')
        index22 = argvdef.find('>')
        if index11 != -1 and index22 != -1:
            argvdef = argvdef.replace(argvdef[index11:index22], '', 1)
        else:
            break
    return len(argvdef.split(',')), sym[:index1]

def codegen(symlist):
    file = open('tstates.cpp', 'r')
    try:
        tstate = file.read()
        for sym, symbol in symlist:
            len1, func = analysis(sym)
            index1 = tstate.find('%%')
            index2 = tstate.find('%%', index1 + 2)
            tstate1 = tstate.replace(tstate[index1:index2+2], '"'+symbol[:len(symbol)-1]+'"')
            index1 = tstate.find('&&')
            index2 = tstate.find('&&', index1+2)
            argv = ''
            for i in range(len1):
                argv += ', INT32 argv%d'%i
            print(argv)
            tstate1 = tstate1.replace(tstate[index1:index2+2], argv)
            function = ''
            for i in range(len1):
                function += 'log << "argv%d:" << %s << std::endl;\n'%(i, 'argv%d'%i)
            index1 = tstate.find('$$')
            index2 = tstate.find('$$', index1+2)
            print(function, tstate[index1:index2+2])
            tstate1 = tstate1.replace(tstate[index1:index2+3], function)
            insert = ''
            for i in range(len1):
                insert += 'IARG_FUNCARG_CALLSITE_VALUE, %d,\n'%i
            index1 = tstate.find('**')
            index2 = tstate.find('**', index1+2)
            print(insert)
            tstate1 = tstate1.replace(tstate[index1:index2+3], insert)
            filename = '%ststate.cpp'%func
            print(filename)
            tstatefile = open(filename, 'w')
            tstatefile.write(tstate1)
            tstatefile.close()
    finally:
        file.close()

if __name__=="__main__":
    codegen(analytical(sys.argv[1]))
