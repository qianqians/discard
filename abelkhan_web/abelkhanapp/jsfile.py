# -*- coding: UTF-8 -*-
# search
# create at 2016/3/1
# autor: qianqians
from flask import *
from webapp import *
import traceback

res_data = {}

@app.route('/JSON.js')
def file_JSON():
    from io import BytesIO
    try:
        return Response(BytesIO(res_data['JSON.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONError.js')
def file_JSONError():
    from io import BytesIO
    try:
        return Response(BytesIO(res_data['JSONError.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONRequest.js')
def file_JSONRequest():
    from io import BytesIO
    try:
        return Response(BytesIO(res_data['JSONRequest.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONRequestError.js')
def file_JSONRequestError():
    from io import BytesIO
    try:
        return Response(BytesIO(res_data['JSONRequestError.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/alertWinMsg.js')
def file_alertWinMsg():
    from io import BytesIO
    try:
        return Response(BytesIO(res_data['alertWinMsg.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/jsquest.js')
def file_jsquest():
    from io import BytesIO
    try:
        return Response(BytesIO(res_data['jsquest.js']), mimetype='js')
    except:
        traceback.print_exc()

def jsfile_init():
    try:
        import os
        filejson = open(os.path.split(os.path.realpath(__file__))[0] + '/js/JSON.javascript', 'rb')
        res_data['JSON.js'] = filejson.read()
        filejson.close()

        fileJSONError = open(os.path.split(os.path.realpath(__file__))[0] + '/js/JSONError.javascript', 'rb')
        res_data['JSONError.js'] = fileJSONError.read()
        fileJSONError.close()

        fileJSONRequest = open(os.path.split(os.path.realpath(__file__))[0] + '/js/JSONRequest.javascript', 'rb')
        res_data['JSONRequest.js'] = fileJSONRequest.read()
        fileJSONRequest.close()

        fileJSONRequestError = open(os.path.split(os.path.realpath(__file__))[0] + '/js/JSONRequestError.javascript', 'rb')
        res_data['JSONRequestError.js'] = fileJSONRequestError.read()
        fileJSONRequestError.close()

        filealertWinMsg = open(os.path.split(os.path.realpath(__file__))[0] + '/js/alertWinMsg.js', 'rb')
        res_data['alertWinMsg.js'] = filealertWinMsg.read()
        filealertWinMsg.close()

        filejsquest = open(os.path.split(os.path.realpath(__file__))[0] + '/js/jsquest.js', 'rb')
        res_data['jsquest.js'] = filejsquest.read()
        filejsquest.close()

    except:
        traceback.print_exc()
