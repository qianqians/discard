# -*- coding: UTF-8 -*-
# search
# create at 2016/3/11
# autor: qianqians

import sys
sys.path.append('../3rdparty/')

from flask import *
from flask.ext.cors import CORS
from websearchapp import *
from pay_sdk import *

if __name__ == '__main__':
    try:
        pay_sdk_init()
        
        app.run("0.0.0.0", "5000", threaded = True)
    except:
        traceback.print_exc()
