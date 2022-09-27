# -*- coding: UTF-8 -*-
# search
# create at 2016/3/11
# autor: qianqians

import sys
sys.path.append('../3rdparty/')

from flask import *
from flask.ext.cors import CORS
from websearchapp import *
from background import *
from jsfile import *
from agent import *

if __name__ == '__main__':
    try:
        jsfile_init()
        background_init()
        agent_init()

        app.run("0.0.0.0", "80", threaded = True)
    except:
        traceback.print_exc()
