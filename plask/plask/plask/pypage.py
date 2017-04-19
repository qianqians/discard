# -*- coding: UTF-8 -*-
#  pypage
# create at 2015/5/28
# autor: qianqians
from pyhtmlstyle import pyhtmlstyle
from pyjs import pyjs
from pysession import *
import pymsgbox

class pypage(pyhtmlstyle):
	def __init__(self, cname, url, layout):
		self.url = url
		self.divlist = []
		self.reslist = []
		self.html = ""
		self.pagename = cname

		self.page_route = []

		self.jsdata = pyjs(self.url)

		self.css = None
		self.html = None
		self.js = None

		self.def_script = ""

		super(pypage, self).__init__(cname, layout)

	def init(self):
		import os
		if not os.path.exists('./html'):
			os.makedirs('./html')
		htmlname = "./html/" + self.pagename + ".html"
		fp = open(htmlname, 'w')
		fp.write(self.flush())
		
		if not os.path.exists('./plask'):
			os.makedirs('./plask')
		flaskname = './plask/' + self.pagename + "_1.py"
		fp = open(flaskname, 'w')
		fp.write(self.flaskflush())

		if not os.path.exists('./plask'):
			os.makedirs('./plask')
		servicename = './plask/' + "serviceapp.py"
		fp = open(servicename, 'a')
		fp.write("import " + self.pagename + "_1\n")

	def add_page_route(self, route):
		self.page_route.append(route)
		
	def flaskflush(self):
		flask = "# -*- coding: UTF-8 -*-\n#gen by plask\n#a new html framework\n#web service code\nfrom flask import *\nfrom serviceapp import app\n"
		import os
		path = os.path.split(os.path.realpath(__file__))[0]
		flask += "import sys\nsys.path.append('" + path + "')\n"
		flask += "import time\nimport pysession\n\n"
		flask += "count = []\n"
		flask += "count.append(0)\n"
		flask += "llid = []\n"
		flask += "llid.append(1)\n"
		flask += "def create_sessionid(ip):\n"
		flask += "\t#获取unix时间戳\n"
		flask += "\tid = str(int(time.time()))\n"
		flask += "\t#用户IP\n"
		flask += "\tid += '-' + ip\n"
		flask += "\t#序列号\n"
		flask += "\tid += '-' + str(llid[0])\n"
		flask += "\tllid[0] += 1\n"
		flask += "\treturn id, llid[0]\n\n"
		flask += "def create_session(ip):\n"
		flask += "\tid,sid = create_sessionid(ip)\n"
		flask += "\tpysession.session[id] = {}\n"
		flask += "\tpysession.session[id][\"ip\"] = ip\n"
		flask += "\tpysession.session[id][\"llid\"] = sid\n"
		flask += "\tpysession.session[id][\"id\"] = id\n"
		flask += "\tjs = \"<script>\\nvar sid = \\\"\" + id + \"\\\";</script>\\n\"\n"
		flask += "\treturn js\n\n"
		flask += "@app.route('/" + self.pagename + "')\n"
		for route in self.page_route:
			flask += "@app.route('" + route + "')\n"
		flask += "def " + self.pagename + "index():\n"
		flask += "\tcss = \"\"\"" + self.css + "\"\"\"\n"
		flask += "\thtml = \"\"\"" + self.html + "\"\"\"\n"
		flask += "\tscript = \"\"\"" + self.script + "\"\"\" + create_session(request.remote_addr)" + "\n"
		flask += "\tcount[0] = count[0] + 1\n"
		flask += "\tprint \"user count\", count[0]\n"
		flask += "\treturn css + html + script + \"</html>\"\n\n"
		for div in self.divlist:
			flask += div.flaskflush() 
		for res in self.reslist:
			flask += res.flaskflush()
		if '/' in self.page_route:
			flask += self.jsdata.flaskflush()
		return flask

	def skip(self):

		
	def flush(self):
		head = "<!DOCTYPE html><html><head>"
		scss = "<style type=\"text/css\">\n"
		html = "\n<body>\n"
		script = "<script language=\"javascript\" src=\"" + self.jsdata.JSON_url() + "\"></script>\n"
		script += "<script language=\"javascript\" src=\"" + self.jsdata.JSONError_url() + "\"></script>\n"
		script += "<script language=\"javascript\" src=\"" + self.jsdata.JSONRequestError_url() + "\"></script>\n"
		script += "<script language=\"javascript\" src=\"" + self.jsdata.JSONRequest() + "\"></script>\n"
		script += pymsgbox.js_msgbox()
		script += self.def_script

		for div in self.divlist:
			head += div.genmeta()

			scss += div.gencss()
			
			sshtml,sjs = div.flush()
			html += sshtml
			script += sjs

		scss += "\n</style>\n"
		html += "\n</body>\n"
		head += scss + "</head>\n"

		self.css = head
		self.html = html
		self.script = script
		
		return head + html + script + "\n</html>\n"

	def set_def_script(self, script):
		self.def_script = script


