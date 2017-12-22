# -*- coding: UTF-8 -*-
# search
# create at 2016/3/11
# autor: qianqians

import sys
sys.path.append('./common/')
sys.path.append('../3part/')

from flask import *
from flask.ext.cors import CORS
from websearchapp import *
from search import *
from guestbook import *
from collection import *

if __name__ == '__main__':
    try:
        search_init()
        guestbook_init()
        collection_init()

        app.run("0.0.0.0", "80", threaded = True)
    except:
        traceback.print_exc()
