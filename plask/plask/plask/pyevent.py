# pyevent
# create at 2016/2/26
# autor: qianqians
from tools import argv_instance, tuple_rbg
from pyelement import pyelement
from pyhtmlstyle import pyhtmlstyle

class pyevent(pyelement):
    def __init__(self, cname, praframe):
        super(pyevent, self).__init__(cname, pyhtmlstyle.margin_auto, praframe)

    def sub(self, id = None):
        return ""

    def flush(self):
        # if img is not none, use img for button,
        # if img is none, use text for button,
        # handle onclick in js and send a requst to service
        # codegen css in page
        return "", ""
