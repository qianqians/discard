# -*- coding: UTF-8 -*-
# search
# create at 2016/3/1
# autor: qianqians
from flask import *
from websearchapp import *
from urllib import quote, unquote
from homepagehtml import *
from searchpagehtml import *
from check import *
from login import *
import traceback
import pymongo
import thesaurus
import usesearch
import globalv
import time

count = []
count.append(0)
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
    #js = "var sid = \"" + id + "\";"
    return id

@app.route('/')
def index():
    try:
        count[0] = count[0] + 1
        print "user count", count[0]
        id = create_session(request.remote_addr)
            
        serachterm = request.args.get('q')
        if serachterm is not None:
            url = "http://www.abelkhan.com/s/?"
            sid = quote(id)
            url = url + "sid=" + sid + "&"
            serachterm = quote(serachterm.encode('utf-8'))
            url = url + "key=" + serachterm + "&"
            url = url + "index=" + str(1)

            return redirect(url)
        else:
            html = homehtml + "\n\t\tvar sid = \"" + id + "\";" + "\n\t</script>\n</html>"
            return html
    except:
        traceback.print_exc()

@app.route('/JSON.js')
def file_JSON():
    from io import BytesIO
    try:
        print globalv.res_data.keys()
        return Response(BytesIO(globalv.res_data['JSON.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONError.js')
def file_JSONError():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['JSONError.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONRequest.js')
def file_JSONRequest():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['JSONRequest.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/JSONRequestError.js')
def file_JSONRequestError():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['JSONRequestError.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/alertWinMsg.js')
def file_alertWinMsg():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['alertWinMsg.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/loginPop.js')
def file_loginPop():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['loginPop.js']), mimetype='js')
    except:
        traceback.print_exc()

@app.route('/opensearch.xml')
def file_opensearch():
    from io import BytesIO
    try:
        return Response(BytesIO(globalv.res_data['opensearch.xml']), mimetype='xml')
    except:
        traceback.print_exc()

def on_get_check(p):
    num,text = random_check()
    csession = globalv.session[p["sid"]]
    csession["login_check"] = num
    return {"check":text}

@app.route('/callregister', methods=['POST'])
def callregister():
    try:
        from io import BytesIO
        r = on_get_check(request.get_json())
        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/calllogin', methods=['POST'])
def calllogin():
    try:
        from io import BytesIO
        r = on_get_check(request.get_json())
        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/changecheck', methods=['POST'])
def changecheck():
    try:
        from io import BytesIO
        r = on_get_check(request.get_json())
        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/registeruser', methods=['POST'])
def registeruser():
    try:
        from io import BytesIO

        p = request.get_json()
        text = p["checktext"]
        name = p["name"]
        mail = p["mail"]
        key = p["key"]

        r = {}
        if len(name) <= 0:
            r = {"namelen":False}

        if len(key) < 6:
            r = {"keylen":False}

        if mail.find('@') == -1:
            r = {"mail":False}

        csession = globalv.session[p["sid"]]
        if not check_num(csession["login_check"], int(text)):
            r = {"checkend":False}

        if not unregister_mail(mail):
            r = {"mailberegister":False}

        regend = register_user(name, key, mail)
        r = {"registerend":regend}

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/login', methods=['POST'])
def login():
    from io import BytesIO

    p = request.get_json()
    text = p["checktext"]
    name = p["name"]
    key = p["key"]

    r = {}

    csession = globalv.session[p["sid"]]
    if not check_num(csession["login_check"], text):
        r = {"checkend":False}


    c = find_user(name)
    if (c == None):
        r = {"userisnotdefine":True}

    r = {"loginend":longin_user(c, name, key), "username": name}
    csession["name"] = name
    csession["login"] = True

    rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
    return rsp

@app.route('/loginout', methods=['POST'])
def loginout():
    from io import BytesIO

    p = request.get_json()
    csession = globalv.session[p["sid"]]

    if csession.has_key("login"):
        csession["login"] = False
        csession["name"] = None

    return Response(BytesIO(json.dumps({})), mimetype='text/json')

def defineloadjs(key, index, username):
    js = "\n\t\tindex = " + str(index) + ";"
    js = "\n\t\tdocument.getElementById(\"title_edit\").value = \"" + key + "\";"
    if username:
        js += "\n\t\tdocument.getElementById(\"titlereg\").style.visibility=\"hidden\";"
        js += "\n\t\tdocument.getElementById(\"titlereg\").style.display=\"none\";"
        js += "\n\t\tdocument.getElementById(\"titlelogin\").style.visibility=\"hidden\";"
        js += "\n\t\tdocument.getElementById(\"titlelogin\").style.display=\"none\";"
        js += "\n\t\tdocument.getElementById(\"titlelogincontainer\").style.visibility=\"hidden\";"
        js += "\n\t\tdocument.getElementById(\"titlelogincontainer\").style.display=\"none\";"

        js += "\n\t\tdocument.getElementById(\"titlelgoinout\").style.visibility=\"visible\";"
        js += "\n\t\tdocument.getElementById(\"titlelgoinout\").style.display=\"\";"
        js += "\n\t\tdocument.getElementById(\"titleusername\").style.visibility=\"visible\";"
        js += "\n\t\tdocument.getElementById(\"titleusername\").style.display=\"\";"
        js += "\n\t\tdocument.getElementById(\"titleusernamecontainer\").style.visibility=\"visible\";"
        js += "\n\t\tdocument.getElementById(\"titleusernamecontainer\").style.display=\"\";"

        js += "\n\t\tdocument.getElementById(\"titleusername\").innerHTML = \"" + username + "\";"

    return js

@app.route('/s/', methods=['GET'])
def searchpage():
    try:
        sid = request.args.get('sid')
        sid = unquote(sid)
        key = request.args.get('key')
        key = unquote(key)
        index = int(request.args.get('index'))

        username = None
        if globalv.session.has_key(sid):
            csession = globalv.session[sid]
            if csession.has_key("name") and csession.has_key("login") and csession["login"]:
                username = csession["name"]
        else:
            sid = create_session(request.remote_addr)
            url = "http://www.abelkhan.com/s/?"
            sid = quote(sid)
            url = url + "sid=" + sid + "&"
            key = quote(key.encode('utf-8'))
            url = url + "key=" + key + "&"
            url = url + "index=" + str(index)

            return redirect(url)

        pagelist, count = usesearch.find_page(key, int(index))

        searchhtml2 = searchhtml

        for urlinfo in pagelist:
            searchhtml2 += "\t\t\t\t\t\t<div style=\"margin:30px 0px 30px 0px; clear:both\">\n"
            searchhtml2 += "\t\t\t\t\t\t\t<a target=\"_blank\" onclick=\"onurlclick(this)\" style=\"font-size:120%\" href=\"" + urlinfo['url'] + "\">" + urlinfo['title'] + "</a>\n"
            searchhtml2 += "\t\t\t\t\t\t\t<div style=\"font-size:100%\">" + urlinfo['profile'] + "</div>\n"
            searchhtml2 += "\t\t\t\t\t\t\t<div style=\"font-size:80%\">" + urlinfo['url'] + "&nbsp;&nbsp;&nbsp;&nbsp;" + urlinfo['date'] + "</div>\n"
            searchhtml2 += "\t\t\t\t\t\t</div>\n"
        if count == 0:
            searchhtml2 += "\t\t\t\t\t\t<div style=\"margin:30px 0px 30px 0px; clear:both\">未查找到您需要的内容</div>\n"

        searchhtml2 += searchhtml0

        pagecount = (count+9)/10
        begin = (index-4) <= 0 and 1 or (index-4)
        end = index+4
        if (index+4) > pagecount:
            end = pagecount
        if (end - begin) < 8:
            new = end - 8
            begin = new <= 0 and 1 or new

        if index > begin:
            url = "http://www.abelkhan.com/s/?"
            sid = quote(sid)
            url = url + "sid=" + sid + "&"
            key1 = quote(key.encode('utf-8'))
            url = url + "key=" + key1 + "&"
            url = url + "index=" + str(int(index)-1)
            searchhtml2 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:20px 5px 60px 5px; float:left; font-size:120%\" target=\"_blank\" href=\"" + url + "\">上一页</a>\n"

        if (end - begin) > 0:
            for i in xrange(begin, end+1):
                if int(i) == int(index):
                    url = "http://www.abelkhan.com/s/?"
                    sid = quote(sid)
                    url = url + "sid=" + sid + "&"
                    key1 = quote(key.encode('utf-8'))
                    url = url + "key=" + key1 + "&"
                    url = url + "index=" + str(int(i))
                    searchhtml2 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:20px 5px 60px 5px; float:left; font-size:120%; color: DimGray\" target=\"_blank\" href=\"" + url + "\">" + str(int(i)) + "</a>\n"
                else:
                    url = "http://www.abelkhan.com/s/?"
                    sid = quote(sid)
                    url = url + "sid=" + sid + "&"
                    key1 = quote(key.encode('utf-8'))
                    url = url + "key=" + key1 + "&"
                    url = url + "index=" + str(int(i))
                    searchhtml2 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:20px 5px 60px 5px; float:left; font-size:120%\" target=\"_blank\" href=\"" + url + "\">" + str(int(i)) + "</a>\n"

        if index < end:
            url = "http://www.abelkhan.com/s/?"
            sid = quote(sid)
            url = url + "sid=" + sid + "&"
            key1 = quote(key.encode('utf-8'))
            url = url + "key=" + key1 + "&"
            url = url + "index=" + str(int(index)+1)
            searchhtml2 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:20px 5px 60px 5px; float:left; font-size:120%\" target=\"_blank\" href=\"" + url + "\">下一页</a>\n"

        html = searchhtml2 + searchhtml1 + defineloadjs(key.encode('utf-8'), index, username) + "\n\t\tvar sid = \"" + sid + "\";\n\t</script>\n</html>"

        return html
    except:
        traceback.print_exc()
        return redirect("http://www.abelkhan.com/")

@app.route('/skipsearchpage', methods=['POST'])
def skipsearchpage():
    try:
        from io import BytesIO

        p = request.get_json()
        sid = p["sid"]
        key = p["input"]
        index = p["index"]

        url = "http://www.abelkhan.com/s/?"
        sid = quote(sid)
        url = url + "sid=" + sid + "&"
        key = quote(key.encode('utf-8'))
        url = url + "key=" + key + "&"
        url = url + "index=" + str(index)

        return Response(BytesIO(json.dumps({"url":url})), mimetype='text/json')
    except:
        traceback.print_exc()

@app.route('/onurlclick', methods=['POST'])
def onurlclick():
    try:
        from io import BytesIO

        p = request.get_json()
        url = p['url']
        input = p['input']

        findkey = thesaurus.processinput(input)
        for key in findkey:
            usesearch.collection_url_index.find_and_modify({'key':key, 'url':url}, {"$inc":{"userchose":1}}, True)

        return Response(BytesIO(json.dumps({})), mimetype='text/json')
    except:
        traceback.print_exc()

def search_init():
    try:
        conn = pymongo.Connection('127.0.0.1',27017)
        db = conn.webseach
        thesaurus.collection_key = db.keys
        usesearch.collection_url_profile = db.urlprofile
        usesearch.collection_url_index = db.urlindex

        globalv.collection_user = db.user

        import os
        filejson = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSON.javascript', 'rb')
        globalv.res_data['JSON.js'] = filejson.read()
        filejson.close()

        fileJSONError = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSONError.javascript', 'rb')
        globalv.res_data['JSONError.js'] = fileJSONError.read()
        fileJSONError.close()

        fileJSONRequest = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSONRequest.javascript', 'rb')
        globalv.res_data['JSONRequest.js'] = fileJSONRequest.read()
        fileJSONRequest.close()

        fileJSONRequestError = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/JSONRequestError.javascript', 'rb')
        globalv.res_data['JSONRequestError.js'] = fileJSONRequestError.read()
        fileJSONRequestError.close()

        filealertWinMsg = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/alertWinMsg.js', 'rb')
        globalv.res_data['alertWinMsg.js'] = filealertWinMsg.read()
        filealertWinMsg.close()

        fileloginPop = open(os.path.split(os.path.realpath(__file__))[0] + '/javascript/loginPop.js', 'rb')
        globalv.res_data['loginPop.js'] = fileloginPop.read()
        fileloginPop.close()

        opensearch = open(os.path.split(os.path.realpath(__file__))[0] + '/opensearch.xml', 'rb')
        globalv.res_data['opensearch.xml'] = opensearch.read()
        opensearch.close()

        print globalv.res_data.keys()

    except:
        traceback.print_exc()
