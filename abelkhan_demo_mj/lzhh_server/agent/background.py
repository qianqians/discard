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

@app.route('/background')
def background():
    try:
        sid = request.args.get('sid')

        if sid is None:
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "background?sid=" + sid
            return redirect(url)
        else:
            if globalv.session.has_key(sid):
                session = globalv.session[sid]

                if session["is_login"]:
                    return render_template("background.html")
                else:
                    url = globalv.urlname + "background/login?sid=" + sid
                    return redirect(url)
            else:
                sid = create_session(request.remote_addr)
                url = globalv.urlname + "background?sid=" + sid
                return redirect(url)
    except:
        traceback.print_exc()

@app.route('/background/login')
def background_login():
    try:
        sid = request.args.get('sid')

        if sid is not None:
            if globalv.session.has_key(sid):
                session = globalv.session[sid]
                return render_template("background_login.html")
            else:
                sid = create_session(request.remote_addr)
                url = globalv.urlname + "background?sid=" + sid
                return redirect(url)
        else:
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "background?sid=" + sid
            return redirect(url)
    except:
        traceback.print_exc()

@app.route('/background/user_login', methods=['POST'])
def user_login():
    try:
        p = request.get_json()

        sid = p["sid"]
        mima = p["mima"]

        if mima == "sanyuan888":
            session = globalv.session[sid]
            session["is_login"] = True

            url = globalv.urlname + "background?sid=" + sid

            from io import BytesIO
            return Response(BytesIO(json.dumps({"url":url})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/add_agent', methods=['POST'])
def add_agent():
    try:
        p = request.get_json()

        name = p["name"]
        mobile = p["mobile"]
        wechat = p["wechat"]
        identity = p["identity"]
        game_id = p["game_id"]
        key = p["key"]
        ower_key = p["ower_key"]
        agent_lv = p["agent_lv"]
        mima = p["mima"]

        from io import BytesIO

        if collection_agent.find({"mobile":mobile}).count() > 0:
            return Response(BytesIO(json.dumps({"ret":"此手机号已经注册为代理"})), mimetype='text/json')

        if collection_agent.find({"key":key}).count() > 0:
            return Response(BytesIO(json.dumps({"ret":"此代理号已经注册为代理"})), mimetype='text/json')

        if collection_agent.find({"game_id":game_id}).count() > 0:
            return Response(BytesIO(json.dumps({"ret":"此游戏ID已经注册为代理"})), mimetype='text/json')

        if collection_agent.find({"wechat":wechat}).count() > 0:
            return Response(BytesIO(json.dumps({"ret":"此微信号已经注册为代理"})), mimetype='text/json')

        ret = collection_agent.insert({"name":name, "mobile":mobile, "wechat":wechat, "identity":identity, "game_id":game_id, "key":key, "ower_key":ower_key, "mima":mima, "agent_lv":agent_lv, "cash_out":0})

        return Response(BytesIO(json.dumps({"ret":"添加代理成功"})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/query_agent_key', methods=['POST'])
def query_agent_key():
    try:
        p = request.get_json()

        key = p["key"]

        c1 = collection_agent.find({"key":key}, {"_id":0})

        if c1.count() > 1:
            print "error repeated mobile agent"
            return

        from io import BytesIO

        if c1.count() <= 0:
            return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(key) + "的代理"})), mimetype='text/json')

        r = c1[0]
        print r

        agent_use_list = list(collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"player"}, {"_id":0}))
        for use in agent_use_list:
            c_total_fee = collection_objects.find({"player_reg_key":int(use["reg_key"]), "object_type":"pay_record"}, {"_id":0})
            sum_total_fee = 0
            for c in c_total_fee:
                sum_total_fee = sum_total_fee + c["total_fee"]
            use["sum_total_fee"] = float(sum_total_fee) / 100

        r["player_count"] = len(agent_use_list)
        r["agent_use"] = agent_use_list

        total_fee = 0;
        fee_c1 = collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"pay_record"})
        for c in fee_c1:
            total_fee = total_fee + c["total_fee"]
        r["player_total_fee"] = float(total_fee) / 100

        c2 = collection_agent.find({"ower_key":r["key"]}, {"_id":0, "mima":0})
        r["child_agent_count"] = c2.count()

        child_use_count = 0
        child_total_fee = 0
        for c in c2:
            child_use_count = child_use_count + collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"player"}).count()
            fee_c2 = collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"pay_record"})
            for c3 in fee_c2:
                child_total_fee = child_total_fee + c3["total_fee"]
        r["child_player_count"] = child_use_count
        r["child_total_fee"] = float(child_total_fee) / 100

        if r["ower_key"] == '':
            r["sum_total_fee"] = ((float(total_fee) * 60 / 100) + (float(child_total_fee) * 10 / 100)) / 100
        else:
            r["sum_total_fee"] = ((float(total_fee) * 50 / 100) + (float(child_total_fee) * 10 / 100)) / 100

        return Response(BytesIO(json.dumps(r)), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/query_agent', methods=['POST'])
def query_agent():
    try:
        p = request.get_json()

        key = p["key"]

        c1 = collection_agent.find({"mobile":key}, {"_id":0})

        if c1.count() > 1:
            print "error repeated mobile agent"
            return

        from io import BytesIO

        if c1.count() <= 0:
            return Response(BytesIO(json.dumps({"error":"不存在手机号为" + str(key) + "的代理"})), mimetype='text/json')

        r = c1[0]
        print r

        agent_use_list = list(collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"player"}, {"_id":0}))
        for use in agent_use_list:
            c_total_fee = collection_objects.find({"player_reg_key":int(use["reg_key"]), "object_type":"pay_record"}, {"_id":0})
            sum_total_fee = 0
            for c in c_total_fee:
                sum_total_fee = sum_total_fee + c["total_fee"]
            use["sum_total_fee"] = float(sum_total_fee) / 100

        r["player_count"] = len(agent_use_list)
        r["agent_use"] = agent_use_list

        total_fee = 0;
        fee_c1 = collection_objects.find({"agent_reg_key":int(r["key"]), "object_type":"pay_record"})
        for c in fee_c1:
            total_fee = total_fee + c["total_fee"]
        r["player_total_fee"] = float(total_fee) / 100

        c2 = collection_agent.find({"ower_key":r["key"]}, {"_id":0, "mima":0})
        r["child_agent_count"] = c2.count()

        child_use_count = 0
        child_total_fee = 0
        for c in c2:
            child_use_count = child_use_count + collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"player"}).count()
            fee_c2 = collection_objects.find({"agent_reg_key":int(c["key"]), "object_type":"pay_record"})
            for c3 in fee_c2:
                child_total_fee = child_total_fee + c3["total_fee"]
        r["child_player_count"] = child_use_count
        r["child_total_fee"] = float(child_total_fee) / 100

        if r["ower_key"] == '':
            r["sum_total_fee"] = ((float(total_fee) * 60 / 100) + (float(child_total_fee) * 10 / 100)) / 100
        else:
            r["sum_total_fee"] = ((float(total_fee) * 50 / 100) + (float(child_total_fee) * 10 / 100)) / 100

        return Response(BytesIO(json.dumps(r)), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/del_agent', methods=['POST'])
def del_agent():
    try:
        p = request.get_json()

        key = p["key"]

        from io import BytesIO

        if collection_agent.find({"mobile":key}).count() <= 0:
            return Response(BytesIO(json.dumps({"ret":"此手机号" + str(key) + "并不是代理"})), mimetype='text/json')

        collection_agent.remove({"mobile":key})

        return Response(BytesIO(json.dumps({"ret":"删除手机号为" + str(key) + "的代理成功"})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/query_player', methods=['POST'])
def query_player():
    try:
        p = request.get_json()

        player_id = p["player_id"]

        from io import BytesIO

        c = collection_objects.find({"reg_key":int(player_id), "object_type":"player"}, {"_id":0})

        if c.count() > 1:
            print "error repeated player id"
            return

        if c.count() <= 0:
            return Response(BytesIO(json.dumps({"error":"不存在ID为" + str(player_id) + "的玩家"})), mimetype='text/json')

        r = c[0]

        c_total_fee = collection_objects.find({"player_reg_key":int(player_id), "object_type":"pay_record"}, {"_id":0})
        r["total_fee"] = list(c_total_fee)

        return Response(BytesIO(json.dumps(r)), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/change_agent', methods=['POST'])
def change_agent():
    try:
        p = request.get_json()

        old_agent = p["old_agent"]
        new_agent = p["new_agent"]

        print old_agent, new_agent

        collection_objects.update({"agent_reg_key":int(old_agent), "object_type":"player"}, {"$set":{"agent_reg_key":int(new_agent)}})

        from io import BytesIO

        return Response(BytesIO(json.dumps({"ret":"修改成功"})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/change_player_agent', methods=['POST'])
def change_player_agent():
    try:
        p = request.get_json()

        player_id = p["player_id"]
        new_agent = p["new_agent"]

        print player_id, new_agent

        collection_objects.update({"reg_key":int(player_id), "object_type":"player"}, {"$set":{"agent_reg_key":int(new_agent)}})

        from io import BytesIO

        return Response(BytesIO(json.dumps({"ret":"修改成功"})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/background/detail')
def detail():
    try:
        sid = request.args.get('sid')

        if sid is None:
            sid = create_session(request.remote_addr)
            url = globalv.urlname + "background?sid=" + sid
            return redirect(url)
        else:
            if globalv.session.has_key(sid):
                session = globalv.session[sid]

                if session["is_login"]:
                    return render_template("detail_info.html")
                else:
                    url = globalv.urlname + "background/login?sid=" + sid
                    return redirect(url)
            else:
                sid = create_session(request.remote_addr)
                url = globalv.urlname + "background?sid=" + sid
                return redirect(url)
    except:
        traceback.print_exc()

@app.route('/background/detail_info', methods=['POST'])
def detail_info():
    try:
        agent_list = list(collection_agent.find({}, {"_id":0}))
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

        from io import BytesIO

        return Response(BytesIO(json.dumps(agent_list)), mimetype='text/json')
    except:
        traceback.print_exc()

def background_init():
    try:
        conn = pymongo.Connection('127.0.0.1',27017)
        db_agent = conn.agent
        db_game = conn.test

        global collection_agent
        global collection_objects

        collection_agent = db_agent.agent
        collection_objects = db_game.objects

        collection_agent.create_index('mobile', unique=True)
        collection_agent.create_index('key', unique=True)

        print collection_agent
    except:
        traceback.print_exc()
