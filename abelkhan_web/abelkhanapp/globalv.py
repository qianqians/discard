# -*- coding: UTF-8 -*-
# globalv
# create at 2016/4/24
# autor: qianqians

import traceback
import pymongo

collection_user = None
collection_text = None

def globalv_init():
    try:
        pass
    except:
        traceback.print_exc()