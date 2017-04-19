# pyaddjs
# create at 2015/7/2
# autor: qianqians
from pyhtmlstyle import pyhtmlstyle
from pyelement import pyelement

class pyaddjs(pyelement):
    def __init__(self, id, js, praframe):
        super(pyaddjs, self).__init__(id, pyhtmlstyle.margin_auto, praframe)

        self.js = js

    def flush(self):
        self.js = self.js[0:8] + " id = " + self.id + " " + self.js[8:]
        return "<div id=\"" + self.id + "_1\">" + self.js + "</div>", ""

    def server_set_src(self, src):
        return "document.getElementById(\"" + self.id + "\").src = \"" + src + "\";\n"

    def server_set_async(self):
        return "document.getElementById(\"" + self.id + "\").async = \"async\";\n"


