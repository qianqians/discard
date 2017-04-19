# pymeta
# create at 2016/2/28
# autor: qianqians
from pyhtmlstyle import pyhtmlstyle
from pyelement import pyelement

class pymeta(pyelement):
    def __init__(self, id, meta, praframe):
        super(pymeta, self).__init__(id, pyhtmlstyle.margin_auto, praframe)

        self.meta = meta

    def genmeta(self):
        return self.meta

    def gencss(self):
        return ""

    def flush(self):
        return "", ""
