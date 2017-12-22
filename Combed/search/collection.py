# -*- coding: UTF-8 -*-
# collection
# create at 2016/3/24
# autor: qianqians

from flask import *
from websearchapp import *
from collectionhtml import *
import traceback
import globalv

@app.route('/zhifubao.png')
def file_zhifubao():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['zhifubao.png']), mimetype='png')
    except:
        traceback.print_exc()

@app.route('/collection/')
def collection_index():
    return collectionhtml

def collection_init():
    import os
    filezhifubao = open(os.path.split(os.path.realpath(__file__))[0] + '/zhifubao.png', 'rb')
    globalv.res_data['zhifubao.png'] = filezhifubao.read()
    filezhifubao.close()

    print globalv.res_data.keys()