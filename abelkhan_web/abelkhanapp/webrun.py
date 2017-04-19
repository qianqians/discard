# -*- coding: UTF-8 -*-
# search
# create at 2016/3/11
# autor: qianqians

import sys
sys.path.append('../3part/')

from flask import *
from flask.ext.cors import CORS
from webapp import *
import pymongo

from home import *
from login import *
from jsfile import *
import globalv

if __name__ == '__main__':
    try:
        conn = pymongo.Connection('localhost',27017)
        db = conn.abelkhan
        globalv.collection_user = db.user
        globalv.collection_text = db.text

        home_init()
        login_init()
        jsfile_init()

        app.run("0.0.0.0", "80", threaded = True)
    except:
        traceback.print_exc()

