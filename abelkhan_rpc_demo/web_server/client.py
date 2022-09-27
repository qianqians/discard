# -*- coding: UTF-8 -*-
# search
# create at 2016/3/1
# autor: qianqians
from flask import *
import uuid
import traceback

from websearchapp import *

import os
fileclient = open(os.path.split(os.path.realpath(__file__))[0] + '/../lib/client.js', 'r')
fileclientdata = fileclient.read()
fileclient.close()

fileccallhcaller = open(os.path.split(os.path.realpath(__file__))[0] + '/proto/ccallhcaller.js', 'r')
fileccallhcallerdata = fileccallhcaller.read()
fileccallhcaller.close()

filehcallcmodule = open(os.path.split(os.path.realpath(__file__))[0] + '/proto/hcallcmodule.js', 'r')
filehcallcmoduledata = filehcallcmodule.read()
filehcallcmodule.close()

fileevent_cb = open(os.path.split(os.path.realpath(__file__))[0] + '/proto/event_cb.js', 'r')
fileevent_cbdata = fileevent_cb.read()
fileevent_cb.close()

@app.route('/client.js')
def file_client():
    from io import BytesIO
    try:
        return Response(BytesIO(fileclientdata), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/ccallhcaller.js')
def file_ccallhcaller():
    from io import BytesIO
    try:
        return Response(BytesIO(fileccallhcallerdata), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/hcallcmodule.js')
def file_hcallcmodule():
    from io import BytesIO
    try:
        return Response(BytesIO(filehcallcmoduledata), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/event_cb.js')
def file_event_cb():
    from io import BytesIO
    try:
        return Response(BytesIO(fileevent_cbdata), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/')
def index():
    try:
        sid = str(uuid.uuid1())
        return render_template("client_html.html", uuid=sid)
    except:
        traceback.print_exc()
