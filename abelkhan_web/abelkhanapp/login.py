# -*- coding: UTF-8 -*-
# login
# create at 2016/3/13
# autor: qianqians

from flask import *
from webapp import *
import traceback
import pymongo
import smtplib
from email.mime.text import MIMEText
from email.header import Header
from bson import objectid
import time

import globalv
from html import loginhtml
from html import regsuchtml
from html import regcheckhtml

backimagelist = []

@app.route('/post_login', methods=['POST'])
def post_login():
    try:
        p = request.get_json()
        usermail = p['usermail']
        key = p['key']

        r = {}

        while True:
            c = globalv.collection_user.find_one({'usermail':usermail})
            if c == None:
                r['sucess'] = False
                r['error'] = "不存在的用户"
                break

            if not c['checked']:
                r['sucess'] = False
                r['error'] = "账户需要邮件激活!"
                break


            if key.encode('utf-8') != c['key'].encode('utf-8'):
                r['sucess'] = False
                r['error'] = "密码错误"
                break

            globalv.collection_user.update({'_id':c['_id']}, {'$set':{'login':True}})
            r['url'] = "http://abelkhan.com/user/?uid=" + str(c['_id'])
            r['usermail'] = c['usermail']
            r['sucess'] = True

            break

        from io import BytesIO
        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/post_check_login', methods=['POST'])
def post_check_login():
    try:
        p = request.get_json()
        usermail = p['usermail']

        r = {}
        c = globalv.collection_user.find_one({'usermail':usermail})
        if c == None:
            r['url'] = "http://abelkhan.com/login/"
        else:
            if c['login']:
                r['url'] = "http://abelkhan.com/user/?uid=" + str(c['_id'])
            else:
                r['url'] = "http://abelkhan.com/login/"

        from io import BytesIO
        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/checkregisternext', methods=['POST'])
def checkregisternext():
    try:
        p = request.get_json()
        regid = p['regusrid']
        nickname = p['nickname']

        r = {}

        if regid:
            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(regid)})
            if c == None:
                r['url'] = "http://abelkhan.com/login/"
                r['sucess'] = True
            else:
                cf = globalv.collection_user.find({'nickname':nickname})
                if cf.count() > 0:
                    r['sucess'] = False
                    r['error'] = "昵称已被注册!"
                else:
                    globalv.collection_user.update({'_id':objectid.ObjectId(regid)}, {'$set':{'nickname':nickname, 'login':True}})

                    r['url'] = "http://abelkhan.com/user/?uid=" + regid.encode('utf-8')
                    r['usermail'] = c['usermail']
                    r['sucess'] = True
        else:
            r['url'] = "http://abelkhan.com/login/"
            r['sucess'] = True

        from io import BytesIO
        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/checkregister/')
def checkregister():
    try:
        regid = request.args.get('regid')
        if regid:
            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(regid)})
            if c == None:
                return redirect("http://abelkhan.com/login/")

            if c['checked']:
                return redirect("http://abelkhan.com/login/")

            globalv.collection_user.update({'_id':objectid.ObjectId(regid)}, {'$set':{'checked':True}})

            return regcheckhtml.regcheckhtml + "\n\t\tvar regusrid = \"" + regid.encode('utf-8') + "\";\n\t</script>\n</html>\n"
        else:
            return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

def sendmail(recver, regid):
    mailtext = "您好，" + recver
    mailtext += "\n\nabelkhan是一款专为图片分享设计的的轻博客产品，旨在为“热爱记录生活、追求时尚品质、崇尚自由空间”的你，打造一个全新的展示平台！ \n请点击下面的链接完成注册："
    mailtext += "http://abelkhan.com/checkregister/?regid=" + regid + "\n"
    mailtext += "如果以上链接无法点击，请把上面网页地址复制到浏览器地址栏中打开\n\n\n"
    mailtext += "abelkhan - 图片记录生活\n"

    msg = MIMEText(mailtext, 'plain', 'utf-8')
    msg['Subject'] = Header('abelkhan帐号激活邮件 ', 'utf-8')
    msg['From'] = "abelkhan@abelkhan.com"
    msg['To'] = recver

    smtp = smtplib.SMTP()
    smtp.connect('smtp.abelkhan.com')
    smtp.login("abelkhan@abelkhan.com", "******")

    smtp.sendmail("abelkhan@abelkhan.com", recver, msg.as_string())
    smtp.quit()

@app.route('/post_resendmail', methods=['POST'])
def post_resendmail():
    try:
        r = {}

        from io import BytesIO

        p = request.get_json()
        regid = p['regusrid']

        c = globalv.collection_user.find_one({'_id':objectid.ObjectId(regid)})
        if c == None:
            r['sucess'] = False
            r['error'] = "未注册的用户!"

        sendmail(c['usermail'].encode('utf-8'), regid.encode('utf-8'))
        r['sucess'] = True

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/post_register', methods=['POST'])
def post_register():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        usermail = p['usermail']
        key = p['key']

        while True:
            if len(key) < 8:
                r['sucess'] = False
                r['error'] = "密码必须为不小于8位的数字或字符"
                break

            c = globalv.collection_user.find({'usermail':usermail})
            if c.count() > 0:
                r['sucess'] = False
                r['error'] = "邮箱以被注册"
                break

            regid = globalv.collection_user.insert({'usermail':usermail, 'key':key, 'checked':False})
            r['sucess'] = True
            r['url'] = "http://abelkhan.com/regsuc/?regid=" + str(regid)
            sendmail(usermail.encode('utf-8'), str(regid))

            break

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/regsuc/')
def regsuc():
    try:
        regid = request.args.get('regid')

        if regid:
            return regsuchtml.regsuchtml + "\n\t\tvar regusrid = \"" + regid.encode('utf-8') + "\";\n\t</script>\n</html>\n"
        else:
            return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

@app.route('/image/')
def image():
    try:
        imageindex = int(request.args.get('index'))
        from io import BytesIO
        return Response(BytesIO(backimagelist[imageindex]), mimetype='jpg')
    except:
        traceback.print_exc()

@app.route('/login/')
def login():
    try:
        import random
        index = random.randint(0, len(backimagelist)-1)
        imageurl = "http://abelkhan.com/image/?index="+str(index)
        html = loginhtml.loginhtml + "\n\t<body background=\"" + imageurl + "\">\n" + loginhtml.loginhtml1
        return html
    except:
        traceback.print_exc()

def login_init():
    try:
        import os

        fileimage = open(os.path.split(os.path.realpath(__file__))[0] + '/backimage/53c4a733cabd1.jpg', 'rb')
        backimagelist.append(fileimage.read())
        fileimage.close()

        fileimage = open(os.path.split(os.path.realpath(__file__))[0] + '/backimage/53c4a720276b7.jpg', 'rb')
        backimagelist.append(fileimage.read())
        fileimage.close()

        fileimage = open(os.path.split(os.path.realpath(__file__))[0] + '/backimage/53c34e37bc7e1.jpg', 'rb')
        backimagelist.append(fileimage.read())
        fileimage.close()

        fileimage = open(os.path.split(os.path.realpath(__file__))[0] + '/backimage/537c091b19df5.jpg', 'rb')
        backimagelist.append(fileimage.read())
        fileimage.close()

        fileimage = open(os.path.split(os.path.realpath(__file__))[0] + '/backimage/53731820ed824.jpg', 'rb')
        backimagelist.append(fileimage.read())
        fileimage.close()

    except:
        traceback.print_exc()
