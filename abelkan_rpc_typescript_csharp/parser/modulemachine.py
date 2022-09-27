#coding:utf-8
# 2015-5-15
# build by qianqians
# modulemachine

from deletenonespacelstrip import deleteNoneSpacelstrip

class func(object):
    def __init__(self):
        self.keyworld = ''
        self.func = []
        self.argvtuple = None
        self.ignore = False

    def clear(self):
        self.keyworld = ''
        self.func = []
        self.argvtuple = None
        self.ignore = False

    def push(self, ch):
        if ch in [' ', '    ', '\r', '\n', '\t', '\0']:
            self.keyworld = deleteNoneSpacelstrip(self.keyworld)
            if self.keyworld != '':
                if self.argvtuple is None:
                    self.func.append(deleteNoneSpacelstrip(self.keyworld))
                    self.keyworld = ''
                else:
                    self.ignore = True
            return False

        if ch == ',':
            if self.keyworld != '':
                self.argvtuple.append(deleteNoneSpacelstrip(self.keyworld))
                self.keyworld = ''
                self.ignore = False

            return False

        if ch == '(':
            self.keyworld = deleteNoneSpacelstrip(self.keyworld)
            if self.keyworld != '':
                self.func.append(deleteNoneSpacelstrip(self.keyworld))
            self.argvtuple = []
            self.keyworld = ''
            return False

        if ch == ')':
            if self.keyworld != '':
                self.argvtuple.append(deleteNoneSpacelstrip(self.keyworld))

            if self.argvtuple is None:
                self.func.append([])
            else:
                self.func.append(self.argvtuple)

            self.ignore = False
            self.argvtuple = None
            self.keyworld = ''

            return False

        if ch == ';':
            return True

        if not self.ignore:
            self.keyworld += ch

        return False

class module(object):
    def __init__(self):
        self.keyworld = ''
        self.name = ''
        self.module = {}
        self.machine = None

    def push(self, ch):
        if ch == '}':
            self.machine = None
            return True

        if self.machine is not None:
            if self.machine.push(ch):
                self.module["method"].append(self.machine.func)
                self.machine.clear()
        else:
            if ch == '{':
                self.keyworld = ''
                self.module["method"] = []
                self.machine = func()
                return False

            if ch == '(':
                self.keyworld = deleteNoneSpacelstrip(self.keyworld)
                if self.keyworld != '':
                    self.name = deleteNoneSpacelstrip(self.keyworld)
                self.keyworld = ''
                return False

            if ch == ')':
                self.keyworld = deleteNoneSpacelstrip(self.keyworld)
                if self.keyworld != '':
                    types = self.keyworld.split()
                    if types[0] in ['client_call_hub', 'hub_call_client', 'hub_call_hub']:
                        self.module["module_type"] = types[0]
                    if len(types) == 2:
                        self.module["type"] = types[1]
                self.keyworld = ''
                return False

            self.keyworld += ch

        return False
