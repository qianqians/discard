# -*- coding: UTF-8 -*-
# darkforce uuid
# create at 2015/9/30
# autor: qianqians
import math
import os
import time
import random

def mac(self):
    import netifaces
    devlist = []
    devices = netifaces.interfaces()
    for dev in devices:
        devlist.append(dev)

        for devinfo in devlist:
            infos = netifaces.ifaddresses(devinfo)
            if len(infos) < 2:
                continue
            ip = infos[netifaces.AF_INET][0]['addr']
            if ip != '':
                if ip != '127.0.0.1':
                    return infos[netifaces.AF_LINK][0]['addr']
                    break

    return ""

llid = 0

def uuid():
    #获取unix时间戳
    id = str(int(time.time()))
    #获取进程ID
    id += str(os.getpid())
    #随机数
    id += str(random.randint(1, 9999))
    #序列号
    id += str(llid)
    llid = llid + 1
    # mac 地址
    id += mac()

    return id