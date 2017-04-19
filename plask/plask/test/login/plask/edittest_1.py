# -*- coding: UTF-8 -*-
#gen by plask
#a new html framework
#web service code
from flask import *
from serviceapp import app
import sys
sys.path.append('C:\Users\qianqians\Documents\workspace\DarkForce\plask\plask')
import time
import pysession

llid = []
llid.append(1)
def create_sessionid(ip):
	#获取unix时间戳
	id = str(int(time.time()))
	#用户IP
	id += '-' + ip
	#序列号
	id += '-' + str(llid[0])
	llid[0] += 1
	return id, llid[0]

def create_session(ip):
	id,sid = create_sessionid(ip)
	pysession.session[id] = {}
	pysession.session[id]["ip"] = ip
	pysession.session[id]["llid"] = sid
	pysession.session[id]["id"] = id
	print pysession.session
	js = "var sid = \"" + id + "\";"
	return js

@app.route('/edittest')
@app.route('/')
def edittestindex():
	css = """<!DOCTYPE html><html><head><style type="text/css">
div#edit_1{ visibility:visible; margin:1px auto auto 0px;}div#button_1{ visibility:visible; margin:0px auto auto 180px;}div#text_1{ visibility:hidden; margin:0px auto auto 0px;}
</style>
</head>
"""
	html = """
<body>
<div id="edit_1"><input id="edit" type="text"></div><div id="button_1"><button id="button" type="button"  onclick="buttononclick(this)" >button</button></div><div id="text_1"><p id="text">text</p></div>
</body>
"""
	script = """<script language="javascript" src="http://127.0.0.1:5000/JSON.js"></script>
<script language="javascript" src="http://127.0.0.1:5000/JSONError.js"></script>
<script language="javascript" src="http://127.0.0.1:5000/JSONRequestError.js"></script>
<script language="javascript" src="http://127.0.0.1:5000/JSONRequest.js"></script>
<script>
function buttononclick(id){var params = {"sid":sid};
params["input"]=document.getElementById("edit").value;

JSONRequest.post("http://127.0.0.1:5000/button/submit",
params,function (requestNumber, value, exception){document.getElementById("edit").value=value["output"];
document.getElementById("edit").style.visibility="hidden";
document.getElementById("button").style.visibility="hidden";
document.getElementById("text").style.visibility="visible";
document.getElementById("text").innerHTML="456";});}
""" + create_session(request.remote_addr)
	return css + html + script + "</script></html>"

@app.route('/button/submit',methods=['POST'])
def buttonsubmit():
	import traceback
	from appglobal import cb_mothed
	from io import BytesIO
	import json
	r = {}
	try:
		for cb in cb_mothed["button"]["submit"]:
			r.update(cb(request.get_json()))
		return Response(BytesIO(json.dumps(r)), mimetype='text/json')
	except:
		from log import log
		log(traceback.format_exc())

@app.route('/JSON.js')
def file_JSON():
	from io import BytesIO
	from appglobal import res_data
	import traceback
	try:
		return Response(BytesIO(res_data['JSON.js']), mimetype='js')
	except:
		from log import log
		log(traceback.format_exc())

@app.route('/JSONError.js')
def file_JSONError():
	from io import BytesIO
	from appglobal import res_data
	import traceback
	try:
		return Response(BytesIO(res_data['JSONError.js']), mimetype='js')
	except:
		from log import log
		log(traceback.format_exc())

@app.route('/JSONRequest.js')
def file_JSONRequest():
	from io import BytesIO
	from appglobal import res_data
	import traceback
	try:
		return Response(BytesIO(res_data['JSONRequest.js']), mimetype='js')
	except:
		from log import log
		log(traceback.format_exc())

@app.route('/JSONRequestError.js')
def file_JSONRequestError():
	from io import BytesIO
	from appglobal import res_data
	import traceback
	try:
		return Response(BytesIO(res_data['JSONRequestError.js']), mimetype='js')
	except:
		from log import log
		log(traceback.format_exc())

