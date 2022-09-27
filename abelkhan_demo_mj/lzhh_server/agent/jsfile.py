# -*- coding: UTF-8 -*-
# search
# create at 2016/3/1
# autor: qianqians
from flask import *
from websearchapp import *
import traceback
import globalv

@app.route('/JSON.js')
def file_JSON():
    from io import BytesIO
    try:
        print globalv.res_data.keys()
        return Response(BytesIO(globalv.res_data['JSON.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONError.js')
def file_JSONError():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['JSONError.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONRequest.js')
def file_JSONRequest():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['JSONRequest.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONRequestError.js')
def file_JSONRequestError():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['JSONRequestError.js']), mimetype='js')
    except:
        traceback.print_exc()

def jsfile_init():
    try:
        import os
        filejson = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSON.javascript', 'rb')
        globalv.res_data['JSON.js'] = filejson.read()
        filejson.close()

        fileJSONError = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSONError.javascript', 'rb')
        globalv.res_data['JSONError.js'] = fileJSONError.read()
        fileJSONError.close()

        fileJSONRequest = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSONRequest.javascript', 'rb')
        globalv.res_data['JSONRequest.js'] = fileJSONRequest.read()
        fileJSONRequest.close()

        fileJSONRequestError = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSONRequestError.javascript', 'rb')
        globalv.res_data['JSONRequestError.js'] = fileJSONRequestError.read()
        fileJSONRequestError.close()

        print globalv.res_data.keys()

    except:
        traceback.print_exc()
