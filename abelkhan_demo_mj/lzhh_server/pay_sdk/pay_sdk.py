# -*- coding: UTF-8 -*-
# search
# create at 2016/3/1
# autor: qianqians
from flask import *
from websearchapp import *
import traceback
import requests
import xml.dom.minidom
from xml.etree import ElementTree
from io import BytesIO
import redis

redis_proxy = None

@app.route('/api/pay', methods=['POST'])
def api_pay():
    try:
        data = request.get_data()

        print data

        root = ElementTree.fromstring(data)
        return_code = root.find('return_code')
        result_code = root.find('result_code')
        out_trade_no = root.find('out_trade_no')
        total_fee = root.find('total_fee')

        if return_code.text == 'SUCCESS' and result_code.text == 'SUCCESS':
            redis_proxy.lpush("out_trade_no", json.dumps({"out_trade_no":out_trade_no.text, "total_fee":int(total_fee.text)}))

        impl = xml.dom.minidom.getDOMImplementation()
        dom = impl.createDocument(None, 'xml', None)
        root = dom.documentElement
        item = dom.createElement('return_code')
        text = dom.createTextNode('SUCCESS')
        item.appendChild(text)
        root.appendChild(item)
        item = dom.createElement('return_msg')
        text = dom.createTextNode('OK')
        item.appendChild(text)
        root.appendChild(item)
        return Response(BytesIO(root.toxml()), mimetype='text/json')

    except:
        traceback.print_exc()

def pay_sdk_init():
    try:
        redis_proxy = redis.Redis(host='127.0.0.1', port=6479, db=0)

        global redis_proxy

    except:
        traceback.print_exc()
