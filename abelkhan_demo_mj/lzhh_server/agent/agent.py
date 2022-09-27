# -*- coding: UTF-8 -*-
# search
# create at 2016/3/1
# autor: qianqians
from flask import *
from websearchapp import *
import traceback
import pymongo
import globalv
import time
import xml.dom.minidom
import requests
from xml.etree import ElementTree
from io import BytesIO

collection_agent = None
collection_objects = None

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
    globalv.session[id] = {}
    globalv.session[id]["ip"] = ip
    globalv.session[id]["llid"] = sid
    globalv.session[id]["id"] = id
    globalv.session[id]["is_login"] = False
    #js = "var sid = \"" + id + "\";"
    return id

@app.route('/')
def index():
    try:
        sid = create_session(request.remote_addr)
        url = globalv.urlname + "agent?sid=" + sid
        return redirect(url)
    except:
        traceback.print_exc()

@app.route('/agent')
def agent():
    try:
        sid = request.args.get('sid')

        if sid is None:
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "agent?sid=" + sid
            return redirect(url)
        else:
            if globalv.session.has_key(sid):
                session = globalv.session[sid]

                if session["is_login"]:
                    return render_template("manage_index.html")
                else:
                    url = globalv.urlname + "agent/login?sid=" + sid
                    return redirect(url)
            else:
                sid = create_session(request.remote_addr)
                url = globalv.urlname + "agent?sid=" + sid
                return redirect(url)
    except:
        traceback.print_exc()

@app.route('/agent/manage_info')
def manage_info():
    try:
        sid = request.args.get('sid')

        print sid

        if sid is None:
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "agent?sid=" + sid
            return redirect(url)
        else:
            if globalv.session.has_key(sid):
                session = globalv.session[sid]

                if session["is_login"]:
                    return render_template("manage_info.html")
                else:
                    url = globalv.urlname + "agent/login?sid=" + sid
                    return redirect(url)
            else:
                sid = create_session(request.remote_addr)
                url = globalv.urlname + "agent?sid=" + sid
                return redirect(url)
    except:
        traceback.print_exc()

@app.route('/agent/agent_member')
def agent_member():
    try:
        sid = request.args.get('sid')

        print sid

        if sid is None:
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "agent?sid=" + sid
            return redirect(url)
        else:
            if globalv.session.has_key(sid):
                session = globalv.session[sid]

                if session["is_login"]:
                    return render_template("agent_member.html")
                else:
                    url = globalv.urlname + "agent/login?sid=" + sid
                    return redirect(url)
            else:
                sid = create_session(request.remote_addr)
                url = globalv.urlname + "agent?sid=" + sid
                return redirect(url)
    except:
        traceback.print_exc()

@app.route('/agent/login')
def agent_login():
    try:
        sid = request.args.get('sid')

        if sid is not None:
            if globalv.session.has_key(sid):
                session = globalv.session[sid]
                return render_template("agent_login.html")
            else:
                sid = create_session(request.remote_addr)
                url = globalv.urlname + "agent?sid=" + sid
                return redirect(url)
        else:
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "agent?sid=" + sid
            return redirect(url)
    except:
        traceback.print_exc()

@app.route('/agent/user_login', methods=['POST'])
def agent_user_login():
    try:
        p = request.get_json()

        sid = p["sid"]
        mobile = p["mobile"]
        mima = p["mima"]

        c1 = collection_agent.find({"mobile":mobile}, {"_id":0})

        if c1.count() > 1:
            print "error repeated mobile agent"
            return

        from io import BytesIO

        if c1.count() <= 0:
            return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(mobile) + "的代理"})), mimetype='text/json')

        r = c1[0]
        print r

        if r["mima"] == mima:
            session = globalv.session[sid]
            session["is_login"] = True
            session["mobile"] = mobile

            url = globalv.urlname + "agent?sid=" + sid

            from io import BytesIO
            return Response(BytesIO(json.dumps({"url":url})), mimetype='text/json')
        else:
            return Response(BytesIO(json.dumps({"error":"密码错误"})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/agent/base_info', methods=['POST'])
def base_info():
    try:
        p = request.get_json()

        sid = p["sid"]
        if globalv.session.has_key(sid):
            session = globalv.session[sid]

            if session.has_key("mobile"):
                mobile = session["mobile"]

                c1 = collection_agent.find({"mobile":mobile}, {"_id":0, "mima":0})

                if c1.count() > 1:
                    print "error repeated mobile agent"
                    return

                from io import BytesIO

                if c1.count() <= 0:
                    return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(mobile) + "的代理"})), mimetype='text/json')

                r = c1[0]
                print r

                c2 = collection_agent.find({"key":r["ower_key"]}, {"_id":0})
                if c2.count() > 0:
                    r1 = c2[0]
                    r["ower_mobile"] = r1["mobile"]

                total_fee = 0;
                fee_c1 = collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"pay_record"})
                for c in fee_c1:
                    total_fee = total_fee + c["total_fee"]
                r["player_total_fee"] = float(total_fee) / 100

                r["player_count"] = collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"player"}, {"_id":1}).count()

                player_agent = list(collection_objects.find({"reg_key":int(r["game_id"]), "object_type":"player"}, {"_id":0, "mima":0}))
                player = player_agent[0]
                if player.has_key("nickname"):
                    r["nickname"] = player["nickname"]

                c2 = collection_agent.find({"ower_key":r["key"]}, {"_id":0})
                r["child_agent_count"] = c2.count()
                child_total_fee = 0
                for c in c2:
                    fee_c2 = collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"pay_record"})
                    for c3 in fee_c2:
                        child_total_fee = child_total_fee + c3["total_fee"]
                r["child_total_fee"] = float(child_total_fee) / 100

                if r["ower_key"] == '':
                    r["sum_total_fee"] = ((float(total_fee) * 60 / 100) + (float(child_total_fee) * 10 / 100)) / 100
                    r["player_total_fee_commission"] = float(total_fee) * 60 / 100 / 100
                    r["child_total_fee_commission"] = float(child_total_fee) * 10 / 100 / 100
                else:
                    r["sum_total_fee"] = ((float(total_fee) * 50 / 100) + (float(child_total_fee) * 10 / 100)) / 100
                    r["player_total_fee_commission"] = float(total_fee) * 50 / 100 / 100
                    r["child_total_fee_commission"] = float(child_total_fee) * 10 / 100 / 100

                r["surplus_total_fee"] = r["sum_total_fee"] - r["cash_out"]

                return Response(BytesIO(json.dumps(r)), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/agent/change_mima', methods=['POST'])
def change_mima():
    try:
        p = request.get_json()

        sid = p["sid"]
        old_mima = p["old_mima"]
        new_mima = p["new_mima"]

        if not globalv.session.has_key(sid):
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "agent?sid=" + sid
            return Response(BytesIO(json.dumps({"url":url})), mimetype='text/json')

        session = globalv.session[sid]
        if not session.has_key("mobile"):
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "agent?sid=" + sid
            return Response(BytesIO(json.dumps({"url":url})), mimetype='text/json')

        mobile = session["mobile"]
        c1 = collection_agent.find({"mobile":mobile}, {"_id":0})

        if c1.count() > 1:
            print "error repeated mobile agent"
            return

        r = c1[0]
        print r

        if r["mima"] != old_mima:
            return Response(BytesIO(json.dumps({"error":"密码错误"})), mimetype='text/json')

        r["mima"] = new_mima
        collection_agent.update({"mobile":mobile}, r)

        sid = create_session(request.remote_addr)
        url = globalv.urlname + "agent?sid=" + sid
        return Response(BytesIO(json.dumps({"ret":"修改完成", "url":url})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/agent/agent_info', methods=['POST'])
def agent_info():
    try:
        p = request.get_json()

        sid = p["sid"]
        if globalv.session.has_key(sid):
            session = globalv.session[sid]

            if session.has_key("mobile"):
                mobile = session["mobile"]

                c1 = collection_agent.find({"mobile":mobile}, {"_id":0})

                if c1.count() > 1:
                    print "error repeated mobile agent"
                    return

                from io import BytesIO

                if c1.count() <= 0:
                    return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(mobile) + "的代理"})), mimetype='text/json')

                r = c1[0]
                print r

                agent_list = list(collection_agent.find({"ower_key":r["key"]}, {"_id":0}))
                for agent in agent_list:
                    agent["player_count"] = collection_objects.find({"agent_reg_key":int(agent["key"]), "object_type":"player"}, {"_id":0}).count()
                    agent["child_agent_count"] = collection_agent.find({"ower_key":agent["key"]}, {"_id":0}).count()

                    total_fee = 0;
                    fee_c1 = collection_objects.find({"agent_reg_key":int(agent["key"]), "object_type":"pay_record"})
                    for c in fee_c1:
                        total_fee = total_fee + c["total_fee"]
                    agent["player_total_fee"] = float(total_fee) / 100

                    c2 = collection_agent.find({"ower_key":agent["key"]}, {"_id":0})
                    child_total_fee = 0
                    for c in c2:
                        fee_c2 = collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"pay_record"})
                        for c3 in fee_c2:
                            child_total_fee = child_total_fee + c3["total_fee"]
                    agent["child_total_fee"] = float(child_total_fee) / 100

                    if agent["ower_key"] == '':
                        agent["sum_total_fee"] = ((float(total_fee) * 60 / 100) + (float(child_total_fee) * 10 / 100)) / 100
                    else:
                        agent["sum_total_fee"] = ((float(total_fee) * 50 / 100) + (float(child_total_fee) * 10 / 100)) / 100

                return Response(BytesIO(json.dumps(agent_list)), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/agent/player_info', methods=['POST'])
def player_info():
    try:
        p = request.get_json()

        sid = p["sid"]
        if globalv.session.has_key(sid):
            session = globalv.session[sid]

            if session.has_key("mobile"):
                mobile = session["mobile"]

                c1 = collection_agent.find({"mobile":mobile}, {"_id":0})

                if c1.count() > 1:
                    print "error repeated mobile agent"
                    return

                from io import BytesIO

                if c1.count() <= 0:
                    return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(mobile) + "的代理"})), mimetype='text/json')

                r = c1[0]
                print r

                player_list = list(collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"player"}, {"_id":0, "mima":0}))

                return Response(BytesIO(json.dumps(player_list)), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/agent/player_pay_info', methods=['POST'])
def player_pay_info():
    try:
        p = request.get_json()

        sid = p["sid"]
        if globalv.session.has_key(sid):
            session = globalv.session[sid]

            if session.has_key("mobile"):
                mobile = session["mobile"]

                c1 = collection_agent.find({"mobile":mobile}, {"_id":0})

                if c1.count() > 1:
                    print "error repeated mobile agent"
                    return

                from io import BytesIO

                if c1.count() <= 0:
                    return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(mobile) + "的代理"})), mimetype='text/json')

                r = c1[0]
                print r

                player_agent = list(collection_objects.find({"reg_key":int(r["game_id"]), "object_type":"player"}, {"_id":0, "mima":0}))
                player = player_agent[0]
                print player
                if player.has_key("nickname"):
                    r["nickname"] = player["nickname"]

                player_pay_list = list(collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"pay_record"}, {"_id":0}))
                for player_pay in player_pay_list:
                    if r.has_key("nickname"):
                        player_pay["agent_nick_name"] = r["nickname"]
                    player_pay["total_fee"] = player_pay["total_fee"] / 100
                    if r["ower_key"] == '':
                        player_pay["total_fee_commission"] = float(player_pay["total_fee"]) * 60 / 100
                    else:
                        player_pay["total_fee_commission"] = float(player_pay["total_fee"]) * 50 / 100
                    player_pay["commission_type"] = "会员充值提成"
                    player_pay["agent_lv"] = r["agent_lv"]

                child_agent = collection_agent.find({"ower_key":r["key"]}, {"_id":0})
                for c in child_agent:
                    child_player_agent = list(collection_objects.find({"reg_key":int(c["game_id"]), "object_type":"player"}, {"_id":0, "mima":0}))
                    child_player = child_player_agent[0]
                    if child_player.has_key("nickname"):
                        c["nickname"] = child_player["nickname"]

                    child_agent_player_pay_list = list(collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"pay_record"}, {"_id":0}))
                    for child_agent_player_pay in child_agent_player_pay_list:
                        if c.has_key("nickname"):
                            child_agent_player_pay["agent_nick_name"] = c["nickname"]
                        child_agent_player_pay["total_fee"] = child_agent_player_pay["total_fee"] / 100
                        child_agent_player_pay["total_fee_commission"] = float(child_agent_player_pay["total_fee"]) * 10 / 100
                        child_agent_player_pay["commission_type"] = "下级代理会员充值提成"
                        child_agent_player_pay["agent_lv"] = c["agent_lv"]
                    player_pay_list.extend(child_agent_player_pay_list)

                return Response(BytesIO(json.dumps(player_pay_list)), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/agent/cash_out', methods=['POST'])
def cash_out():
    try:
        p = request.get_json()

        sid = p["sid"]
        amount = int(p["amount"])

        if globalv.session.has_key(sid):
            session = globalv.session[sid]

            if session.has_key("mobile"):
                mobile = session["mobile"]

                c1 = collection_agent.find({"mobile":mobile}, {"_id":0})

                if c1.count() > 1:
                    print "error repeated mobile agent"
                    return

                from io import BytesIO

                if c1.count() <= 0:
                    return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(mobile) + "的代理"})), mimetype='text/json')

                r = c1[0]
                print r

                total_fee = 0;
                fee_c1 = collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"pay_record"})
                for c in fee_c1:
                    total_fee = total_fee + c["total_fee"]
                r["player_total_fee"] = float(total_fee) / 100

                c2 = collection_agent.find({"ower_key":r["key"]}, {"_id":0})
                child_total_fee = 0
                for c in c2:
                    fee_c2 = collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"pay_record"})
                    for c3 in fee_c2:
                        child_total_fee = child_total_fee + c3["total_fee"]
                r["child_total_fee"] = float(child_total_fee) / 100

                if r["ower_key"] == '':
                    r["sum_total_fee"] = ((float(total_fee) * 60 / 100) + (float(child_total_fee) * 10 / 100)) / 100
                else:
                    r["sum_total_fee"] = ((float(total_fee) * 50 / 100) + (float(child_total_fee) * 10 / 100)) / 100

                if r["sum_total_fee"] < (r["cash_out"] + amount):
                    return Response(BytesIO(json.dumps({"error":"金额不足"})), mimetype='text/json')

                player_agent = list(collection_objects.find({"reg_key":int(r["game_id"]), "object_type":"player"}, {"_id":0, "mima":0}))
                r1 = player_agent[0]
                print r1

                import urllib, urllib2, uuid, hashlib

                url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers"
                nonce_str = str(uuid.uuid1()).replace('-', '')
                partner_trade_no = str(uuid.uuid1()).replace('-', '')

                impl = xml.dom.minidom.getDOMImplementation()
                dom = impl.createDocument(None, 'xml', None)
                root = dom.documentElement
                item = dom.createElement('mch_appid')
                text = dom.createTextNode(globalv.mch_appid)
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('mchid')
                text = dom.createTextNode(globalv.mchid)
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('nonce_str')
                text = dom.createTextNode(nonce_str)
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('partner_trade_no')
                text = dom.createTextNode(partner_trade_no)
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('openid')
                text = dom.createTextNode(str(r1["openid"]))
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('check_name')
                text = dom.createTextNode('NO_CHECK')
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('re_user_name')
                text = dom.createTextNode(str(r1['name']))
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('amount')
                text = dom.createTextNode(str(amount * 100))
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('desc')
                text = dom.createTextNode('微信提现')
                item.appendChild(text)
                root.appendChild(item)
                item = dom.createElement('spbill_create_ip')
                text = dom.createTextNode('192.168.0.1')
                item.appendChild(text)
                root.appendChild(item)

                #argv = {"mch_appid":"wxafd936830b3cf60f", "mchid":"1449873902", "nonce_str":str(uuid.uuid1()), "partner_trade_no":str(uuid.uuid1()), "openid":r["unionid"], "check_name":"NO_CHECK", "amount":amount, "desc":"微信提现", "spbill_create_ip":"192.168.0.1"}
                stringtmp =  "amount="+str(amount * 100)+"&"
                stringtmp += "check_name=NO_CHECK&"
                stringtmp += "desc=微信提现&"
                stringtmp += "mch_appid=" + globalv.mch_appid + "&"
                stringtmp += "mchid=" + globalv.mchid + "&"
                stringtmp += "nonce_str="+nonce_str+"&"
                stringtmp += "openid="+str(r1["openid"])+"&"
                stringtmp += "partner_trade_no="+partner_trade_no+"&"
                stringtmp += "re_user_name="+str(r1['name'])+"&"
                stringtmp += "spbill_create_ip=192.168.0.1&key=" + globalv.key
                hash_md5 = hashlib.md5(stringtmp)

                item = dom.createElement('sign')
                text = dom.createTextNode(hash_md5.hexdigest().upper())
                item.appendChild(text)
                root.appendChild(item)

                data = root.toxml()
                print data
                response = requests.post(url=url, data=data, cert=("C://Users//Administrator//Documents//agent//cert//apiclient_cert.pem", "C://Users//Administrator//Documents//agent//cert//apiclient_key.pem"))
                res = response.content

                print res
                root1 = ElementTree.fromstring(res)
                return_code = root1.find('return_code')
                result_code = root1.find('result_code')

                if (return_code.text == 'SUCCESS' and result_code.text == 'SUCCESS'):
                    r["cash_out"] += amount
                    collection_agent.update({"mobile":mobile}, r)
                    return Response(BytesIO(json.dumps({"ret":"提现完成"})), mimetype='text/json')
                else:
                    err_code_des = root1.find('err_code_des')
                    return Response(BytesIO(json.dumps({"ret":err_code_des.text})), mimetype='text/json')
    except:
        traceback.print_exc()

def agent_init():
    try:
        conn = pymongo.Connection('127.0.0.1',27017)
        db_agent = conn.agent
        db_game = conn.test

        global collection_agent
        global collection_objects

        collection_agent = db_agent.agent
        collection_objects = db_game.objects

    except:
        traceback.print_exc()
