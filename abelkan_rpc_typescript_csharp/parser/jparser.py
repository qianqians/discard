#coding:utf-8
# 2016-6-10
# build by qianqians
# parser

import statemachine
import deletenote

def parser(str):
    machine = statemachine.statemachine()

    machine.syntaxanalysis(deletenote.deletenote(str))

    return machine.getmodule(), machine.getenum(), machine.getstruct()

