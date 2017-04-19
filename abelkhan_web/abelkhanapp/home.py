# -*- coding: UTF-8 -*-
# home
# create at 2016/3/1
# autor: qianqians

from flask import *
from webapp import *
import traceback
import pymongo
from bson import objectid
import time

import globalv
from html import homepagehtml
from html import userpagehtml

imageico = None
videoico = None

@app.route('/concern', methods=['POST'])
def concern():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        otherusername = p['otherusername']

        c = globalv.collection_user.find_one({'_id':objectid.ObjectId(userid)})
        cother = globalv.collection_user.find_one({'nickname':otherusername})

        if c != None and cother != None:
            r['notself'] = False
            if c['_id'] != cother['_id']:
                r['notself'] = True

            if c.has_key('following'):
                if cother['_id'] in c['following']:
                    r["isfollow"] = False
                    globalv.collection_user.find_and_modify({'_id':objectid.ObjectId(userid)}, {'$pull':{'following':cother['_id']}})

                    rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
                    return rsp

            globalv.collection_user.find_and_modify({'_id':objectid.ObjectId(userid)}, {'$push':{'following':cother['_id']}})
            r["isfollow"] = True
            rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
            return rsp
    except:
        traceback.print_exc()

@app.route('/dialogue', methods=['POST'])
def dialogue():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        targetname = p['targetname']

        c = globalv.collection_user.find_one({'_id':objectid.ObjectId(userid)})
        ctarget = globalv.collection_user.find_one({'nickname':targetname})

        ctext = globalv.collection_text.find({'postid':c['_id'], 'targetuserid':ctarget['_id'], 'type':'pm'})
        ctext1 = globalv.collection_text.find({'targetuserid':c['_id'], 'postid':ctarget['_id'], 'type':'pm'})

        textlist = []
        for text in ctext:
            print text
            textlist.append({'sender':c['nickname'], 'text':text['text']})
        for text in ctext1:
            print text
            textlist.append({'sender':ctarget['nickname'], 'text':text['text']})

        r['textlist'] = textlist

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/pm/')
def pm():
    try:
        def gettext(text):
            ret = """\t\t\t\t\t<div style="max-width:560px; margin:auto auto 5px 10px; float:left">"""
            ret += text['text'].encode('utf-8')
            ret += """</div>\n"""
            if text.has_key('forward'):
                text1 = globalv.collection_text.find_one({'_id':text['forward'], 'type':{'$in':['text', 'forward']}})
                user = globalv.collection_user.find_one({'_id':text1['postid']})

                ret += """\t\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutotheruser(this)" onmouseover="mouseoverotheruser(this)" style="float:left; color:LightCoral;">"""
                ret += """//@""" + user['nickname'].encode('utf-8')
                ret += """</div>\n"""
                ret += """\t\t\t\t\t<div style="float:left">:</div>\n"""
                ret += """\t\t\t\t\t<div style="float:left">"""
                ret += gettext(text1)
                ret += """\t\t\t\t\t</div>\n"""

            return ret

        uid = request.args.get('uid')
        if uid:
            html = userpagehtml.userpagehtml

            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(uid)})
            if c is None:
                return redirect("http://abelkhan.com/login/")

            if c.has_key('pmer'):
                for pmer in c['pmer']:
                    cpmer = globalv.collection_user.find_one({'_id':pmer})

                    ctext = globalv.collection_text.find({'postid':pmer, 'targetuserid':c['_id'], 'type':'pm'}).sort('time', pymongo.DESCENDING)

                    html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:10px auto auto auto; clear:both;">\n"""

                    html += """\t\t\t\t<div username=\"""" + cpmer['nickname'].encode('utf-8') + """\" onmouseout="mouseoutuser(this)" onmouseover="mouseoveruser(this)" style="padding-top:10px; margin:5px auto 5px 10px;">"""
                    html += cpmer['nickname'].encode('utf-8') + """:"""
                    html += """</div>\n"""
                    html += """\t\t\t\t<div style="font-size:90%; border-bottom-style:solid;border-width:1px;border-color:rgb(245,245,245);">\n"""
                    html += gettext(ctext[0])
                    html += """\t\t\t\t<div style="clear:both;"></div>\n"""
                    html += """\t\t\t\t</div>\n"""
                    html += """\t\t\t\t<div username=\"""" + cpmer['nickname'].encode('utf-8') + """\" onclick="postpm(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="padding-top:5px;padding-bottom:5px; padding-left:10px; float:right;">回复</div>\n"""
                    html += """\t\t\t\t<div username=\"""" + cpmer['nickname'].encode('utf-8') + """\" onclick="foregroundspm(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="padding-top:5px;padding-bottom:5px;  float:right;">展开对话</div>\n"""
                    html += """\t\t\t\t<div style="clear:both;"></div>\n"""
                    html += """\t\t\t</div>\n"""
            else:
                html += """\t\t\t<div style="background-color:rgb(255,255,255); text-align:center; clear:both;">\n"""
                html += """<div style="padding-top:10px; padding-bottom:10px;" >还没有收到过私信哦，主动给你的联系人发条私信吧!</div>"""
                html += """\t\t\t</div>\n"""

            html += userpagehtml.userpagehtml0
            html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:auto auto auto 20px; width:240px;" >\n"""
            html += """\t\t\t\t<div style="padding-top:10px; padding-left:10px; padding-bottom:10px;">""" + c["nickname"].encode('utf-8') + """</div>\n"""
            html += """\t\t\t\t<div onclick="skiptext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">文章</div>\n"""
            html += """\t\t\t\t<div onclick="pmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">私信"""
            if c.has_key('pmnum') and c['pmnum'] > 0:
                html += str(c['pmnum'])
            html += """</div>\n"""
            html += """\t\t\t\t<div onclick="atme(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">@我"""
            if c.has_key('atnum') and c['atnum'] > 0:
                html += str(c['atnum'])
            html += """</div>\n"""
            html += """\t\t\t\t<div onclick="cmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">评论"""
            if c.has_key('cmnum') and c['cmnum'] > 0:
                html += str(c['cmnum'])
            html += """</div>\n"""
            html += """\t\t\t</div>\n"""
            html = html + userpagehtml.userpagehtml1 + "\n\t\tvar userid = \"" + uid.encode('utf-8') + "\";\n\t</script>\n</html>"
            return html

        return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

@app.route('/pmtext', methods=['POST'])
def pmtext():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']

        r['url'] = "http://abelkhan.com/pm/?uid=" + userid.encode('utf-8')

        globalv.collection_user.find_and_modify({'_id':objectid.ObjectId(userid)}, {"$set":{"pmnum":0}}, True)

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/post_pm', methods=['POST'])
def post_pm():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        targetname = p['targetname']
        pm = p['pm']

        c = globalv.collection_user.find_one({'nickname':targetname})
        globalv.collection_text.insert({'text':pm, 'type':'pm', 'postid':objectid.ObjectId(userid), 'targetuserid':c['_id'], 'time':time.time()})
        globalv.collection_user.find_and_modify({'_id':c['_id']}, {'$push':{'pmer':objectid.ObjectId(userid)}, "$inc":{"pmnum":1}}, True)

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/cm/')
def cm():
    try:
        def gettext(text):
            ret = """\t\t\t\t\t<div style="max-width:560px; margin:auto auto 5px 10px; float:left">"""
            ret += text['text'].encode('utf-8')
            ret += """</div>\n"""
            if text.has_key('forward'):
                text1 = globalv.collection_text.find_one({'_id':text['forward'], 'type':{'$in':['text', 'forward']}})
                user = globalv.collection_user.find_one({'_id':text1['postid']})

                ret += """\t\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutotheruser(this)" onmouseover="mouseoverotheruser(this)" style="float:left; color:LightCoral;">"""
                ret += """//@""" + user['nickname'].encode('utf-8')
                ret += """</div>\n"""
                ret += """\t\t\t\t\t<div style="float:left">:</div>\n"""
                ret += """\t\t\t\t\t<div style="float:left">"""
                ret += gettext(text1)
                ret += """\t\t\t\t\t</div>\n"""

            return ret

        uid = request.args.get('uid')
        if uid:
            html = userpagehtml.userpagehtml

            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(uid)})
            if c is None:
                return redirect("http://abelkhan.com/login/")

            textlist = []
            ctext = globalv.collection_text.find({'targetuserid':objectid.ObjectId(uid), 'type':{'$in':['comment']}})
            if ctext.count() > 0:
                for text in ctext:
                    textlist.append(text)
                textlist.sort(key=lambda x:x['time'], reverse=True)

                for text in textlist:
                    user = globalv.collection_user.find_one({'_id':text['postid']})
                    ctext1 = globalv.collection_text.find_one({'_id':text['targetid']})

                    if ctext1 != None:
                        html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:10px auto auto auto; clear:both;">\n"""

                        html += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutuser(this)" onmouseover="mouseoveruser(this)" style="padding-top:15px; margin:5px auto 15px 10px;">"""
                        html += user['nickname'].encode('utf-8')
                        html += """</div>\n"""
                        html += """\t\t\t\t<div style="font-size:90%;">\n"""
                        html += gettext(text)
                        html += """\t\t\t\t\t<div style="margin:auto 20px auto 20px; border-top-style:solid; border-width: 1px; border-color:rgb(245,245,245); padding-top:5px; clear:both;">\n"""
                        html += """\t\t\t\t\t\t<div style="float:left; ">评论我的文章：</div>\n""" + gettext(ctext1)
                        html += """\t\t\t\t</div>\n"""
                        html += """\t\t\t\t<div style="clear:both;"></div>\n"""
                        html += """\t\t\t\t</div>\n"""

                        html += """\t\t\t</div>\n"""

            html += userpagehtml.userpagehtml0
            html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:auto auto auto 20px; width:240px;" >\n"""
            html += """\t\t\t\t<div style="padding-top:10px; padding-left:10px; padding-bottom:10px;">""" + c["nickname"].encode('utf-8') + """</div>\n"""
            html += """\t\t\t\t<div onclick="skiptext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">文章</div>\n"""
            html += """\t\t\t\t<div onclick="pmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">私信"""
            if c.has_key('pmnum') and c['pmnum'] > 0:
                html += str(c['pmnum'])
            html += """</div>\n"""
            html += """\t\t\t\t<div onclick="atme(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">@我"""
            if c.has_key('atnum') and c['atnum'] > 0:
                html += str(c['atnum'])
            html += """</div>\n"""
            html += """\t\t\t\t<div onclick="cmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">评论"""
            if c.has_key('cmnum') and c['cmnum'] > 0:
                html += str(c['cmnum'])
            html += """</div>\n"""
            html += """\t\t\t</div>\n"""
            html = html + userpagehtml.userpagehtml1 + "\n\t\tvar userid = \"" + uid.encode('utf-8') + "\";\n\t</script>\n</html>"
            return html

        return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

@app.route('/cmtext', methods=['POST'])
def cmtext():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']

        r['url'] = "http://abelkhan.com/cm/?uid=" + userid.encode('utf-8')

        globalv.collection_user.find_and_modify({'_id':objectid.ObjectId(userid)}, {"$set":{"cmnum":0}}, True)

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/atmetext/')
def atmetext():
    try:
        def gettext(text):
            ret = """\t\t\t\t<div style="max-width:560px; margin:auto auto 5px 10px; float:left">"""
            ret += text['text'].encode('utf-8')
            ret += """</div>\n"""
            if text.has_key('forward'):
                text1 = globalv.collection_text.find_one({'_id':text['forward'], 'type':{'$in':['text', 'forward']}})
                user = globalv.collection_user.find_one({'_id':text1['postid']})

                ret += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutotheruser(this)" onmouseover="mouseoverotheruser(this)" style="float:left; color:LightCoral;">"""
                ret += """//@""" + user['nickname'].encode('utf-8')
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += """:"""
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += gettext(text1)
                ret += """</div>\n"""

            return ret

        uid = request.args.get('uid')
        if uid:
            html = userpagehtml.userpagehtml

            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(uid)})
            if c is None:
                return redirect("http://abelkhan.com/login/")

            textlist = []
            ctext = globalv.collection_text.find({'atuserid':objectid.ObjectId(uid), 'type':{'$in':['text', 'forward']}})
            if ctext.count() > 0:
                for text in ctext:
                    textlist.append(text)
                textlist.sort(key=lambda x:x['time'], reverse=True)

                for text in textlist:
                    user = globalv.collection_user.find_one({'_id':text['postid']})

                    html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:10px auto auto auto; clear:both;">\n"""
                    html += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutuser(this)" onmouseover="mouseoveruser(this)" style="padding-top:15px; margin:5px auto 15px 10px;">"""
                    html += user['nickname'].encode('utf-8')
                    html += """</div>\n"""
                    html += """<div style="background-color:rgb(255,255,255);font-size:90%;">\n"""
                    html += gettext(text)
                    html += """</div>\n"""
                    if text['postid'] == objectid.ObjectId(uid):
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="deltext(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """删除"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    else:
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    html += """<div style="clear:both;"></div>\n"""
                    html += """\t\t\t</div>\n"""

            html += userpagehtml.userpagehtml0
            html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:auto auto auto 20px; width:240px;" >\n"""
            html += """\t\t\t\t<div style="padding-top:10px; padding-left:10px; padding-bottom:10px;">""" + c["nickname"].encode('utf-8') + """</div>\n"""
            html += """\t\t\t\t<div onclick="skiptext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">文章</div>\n"""
            html += """\t\t\t\t<div onclick="pmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">私信"""
            if c.has_key('pmnum') and c['pmnum'] > 0:
                html += str(c['pmnum'])
            html += """</div>\n"""
            html += """\t\t\t\t<div onclick="atme(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">@我"""
            if c.has_key('atnum') and c['atnum'] > 0:
                html += str(c['atnum'])
            html += """</div>\n"""
            html += """\t\t\t\t<div onclick="cmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">评论"""
            if c.has_key('cmnum') and c['cmnum'] > 0:
                html += str(c['cmnum'])
            html += """</div>\n"""
            html += """\t\t\t</div>\n"""
            html = html + userpagehtml.userpagehtml1 + "\n\t\tvar userid = \"" + uid.encode('utf-8') + "\";\n\t</script>\n</html>"
            return html

        return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

@app.route('/atme', methods=['POST'])
def atme():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']

        r['url'] = "http://abelkhan.com/atmetext/?uid=" + userid.encode('utf-8')

        globalv.collection_user.find_and_modify({'_id':objectid.ObjectId(userid)}, {"$set":{"atnum":0}}, True)

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/textpage', methods=['POST'])
def textpage():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']

        r['url'] = "http://abelkhan.com/text/?uid=" + userid.encode('utf-8')

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/text/')
def text():
    try:
        def gettext(text):
            ret = """\t\t\t\t<div style="max-width:560px; margin:auto auto 5px 10px; float:left">"""
            ret += text['text'].encode('utf-8')
            ret += """</div>\n"""
            if text.has_key('forward'):
                text1 = globalv.collection_text.find_one({'_id':text['forward'], 'type':{'$in':['text', 'forward']}})
                user = globalv.collection_user.find_one({'_id':text1['postid']})

                ret += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutotheruser(this)" onmouseover="mouseoverotheruser(this)" style="float:left; color:LightCoral;">"""
                ret += """//@""" + user['nickname'].encode('utf-8')
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += """:"""
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += gettext(text1)
                ret += """</div>\n"""

            return ret

        uid = request.args.get('uid')
        if uid:
            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(uid)})
            if c is None:
                return redirect("http://abelkhan.com/login/")

            textlist = []
            ctext = globalv.collection_text.find({'postid':objectid.ObjectId(uid), 'type':{'$in':['text', 'forward']}})
            if ctext.count() > 0:
                for text in ctext:
                    textlist.append(text)
                textlist.sort(key=lambda x:x['time'], reverse=True)

                html = userpagehtml.userpagehtml

                for text in textlist:
                    user = globalv.collection_user.find_one({'_id':text['postid']})

                    html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:10px auto auto auto; clear:both;">\n"""
                    html += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutuser(this)" onmouseover="mouseoveruser(this)" style="padding-top:15px; margin:5px auto 15px 10px;">"""
                    html += user['nickname'].encode('utf-8')
                    html += """</div>\n"""
                    html += """<div style="background-color:rgb(255,255,255);font-size:90%;">\n"""
                    html += gettext(text)
                    html += """</div>\n"""
                    if text['postid'] == objectid.ObjectId(uid):
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="deltext(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """删除"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    else:
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    html += """<div style="clear:both;"></div>\n"""
                    html += """\t\t\t</div>\n"""

                html += userpagehtml.userpagehtml0
                html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:auto auto auto 20px; width:240px;" >\n"""
                html += """\t\t\t\t<div style="padding-top:10px; padding-left:10px; padding-bottom:10px;">""" + c["nickname"].encode('utf-8') + """</div>\n"""
                html += """\t\t\t\t<div onclick="skiptext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">文章</div>\n"""
                html += """\t\t\t\t<div onclick="pmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">私信"""
                if c.has_key('pmnum') and c['pmnum'] > 0:
                    html += str(c['pmnum'])
                html += """</div>\n"""
                html += """\t\t\t\t<div onclick="atme(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">@我"""
                if c.has_key('atnum') and c['atnum'] > 0:
                    html += str(c['atnum'])
                html += """</div>\n"""
                html += """\t\t\t\t<div onclick="cmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">评论"""
                if c.has_key('cmnum') and c['cmnum'] > 0:
                    html += str(c['cmnum'])
                html += """</div>\n"""
                html += """\t\t\t</div>\n"""
                html = html + userpagehtml.userpagehtml1 + "\n\t\tvar userid = \"" + uid.encode('utf-8') + "\";\n\t</script>\n</html>"
                return html

        return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

@app.route('/random/')
def random():
    try:
        def gettext(text):
            ret = """\t\t\t\t<div style="max-width:560px; margin:auto auto 5px 10px; float:left">"""
            ret += text['text'].encode('utf-8')
            ret += """</div>\n"""
            if text.has_key('forward'):
                text1 = globalv.collection_text.find_one({'_id':text['forward'], 'type':{'$in':['text', 'forward']}})
                user = globalv.collection_user.find_one({'_id':text1['postid']})

                ret += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutotheruser(this)" onmouseover="mouseoverotheruser(this)" style="float:left; color:LightCoral;">"""
                ret += """//@""" + user['nickname'].encode('utf-8')
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += """:"""
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += gettext(text1)
                ret += """</div>\n"""

            return ret

        uid = request.args.get('uid')
        if uid:
            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(uid)})
            if c is None:
                return redirect("http://abelkhan.com/login/")

            textlist = []
            import random
            i = random.randint(0, globalv.collection_user.find().count()/20)
            ctext = globalv.collection_text.find({'type':{'$in':['text', 'forward']}}).limit(20).skip(i)
            if ctext.count() > 0:
                for text in ctext:
                    textlist.append(text)
                textlist.sort(key=lambda x:x['time'], reverse=True)

                html = userpagehtml.userpagehtml

                for text in textlist:
                    user = globalv.collection_user.find_one({'_id':text['postid']})

                    html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:10px auto auto auto; clear:both;">\n"""
                    html += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutuser(this)" onmouseover="mouseoveruser(this)" style="padding-top:15px; margin:5px auto 15px 10px;">"""
                    html += user['nickname'].encode('utf-8')
                    html += """</div>\n"""
                    html += """<div style="background-color:rgb(255,255,255);font-size:90%;">\n"""
                    html += gettext(text)
                    html += """</div>\n"""
                    if text['postid'] == objectid.ObjectId(uid):
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="deltext(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """删除"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    else:
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    html += """<div style="clear:both;"></div>\n"""
                    html += """\t\t\t</div>\n"""

                html += userpagehtml.userpagehtml0
                html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:auto auto auto 20px; width:240px;" >\n"""
                html += """\t\t\t\t<div style="padding-top:10px; padding-left:10px; padding-bottom:10px;">""" + c["nickname"].encode('utf-8') + """</div>\n"""
                html += """\t\t\t\t<div onclick="skiptext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">文章</div>\n"""
                html += """\t\t\t\t<div onclick="pmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">私信"""
                if c.has_key('pmnum') and c['pmnum'] > 0:
                    html += str(c['pmnum'])
                html += """</div>\n"""
                html += """\t\t\t\t<div onclick="atme(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">@我"""
                if c.has_key('atnum') and c['atnum'] > 0:
                    html += str(c['atnum'])
                html += """</div>\n"""
                html += """\t\t\t\t<div onclick="cmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">评论"""
                if c.has_key('cmnum') and c['cmnum'] > 0:
                    html += str(c['cmnum'])
                html += """</div>\n"""
                html += """\t\t\t</div>\n"""
                html = html + userpagehtml.userpagehtml1 + "\n\t\tvar userid = \"" + uid.encode('utf-8') + "\";\n\t</script>\n</html>"
                return html

        return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

@app.route('/randompage', methods=['POST'])
def randompage():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']

        r['url'] = "http://abelkhan.com/random/?uid=" + userid.encode('utf-8')

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/userpanel', methods=['POST'])
def userpanel():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        otherusername = p['otherusername']

        c = globalv.collection_user.find_one({'_id':objectid.ObjectId(userid)})
        cother = globalv.collection_user.find_one({'nickname':otherusername})

        if c != None and cother != None:
            r['notself'] = False
            if c['_id'] != cother['_id']:
                r['notself'] = True

            r["isfollow"] = False
            if c.has_key('following'):
                if cother['_id'] in c['following']:
                    r["isfollow"] = True

            rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
            return rsp
    except:
        traceback.print_exc()

@app.route('/forward', methods=['POST'])
def forward():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        textid = p['textid']
        forward = p['forward']

        globalv.collection_text.insert({'text':forward, 'type':'forward', 'forward': objectid.ObjectId(textid), 'postid':objectid.ObjectId(userid), 'time':time.time()}) #'comment':[], 'forward':None

        r['url'] = "http://abelkhan.com/user/?uid=" + userid.encode('utf-8')

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/post_comment', methods=['POST'])
def post_comment():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        textid = p['textid']
        comment = p['comment']

        c = globalv.collection_text.find_one({'_id':objectid.ObjectId(textid)})
        if c != None:
            commentid = globalv.collection_text.insert({'text':comment, 'type':'comment', 'postid':objectid.ObjectId(userid), 'time':time.time(), 'targetuserid':c['postid'], 'targetid':objectid.ObjectId(textid)})
            globalv.collection_text.update({"_id":objectid.ObjectId(textid)}, {'$push':{'comment':commentid}})

            globalv.collection_user.find_and_modify({'_id':c['postid']}, {"$inc":{"cmnum":1}}, True)

            cuser = globalv.collection_user.find_one({'_id':objectid.ObjectId(userid)})

            rsp = Response(BytesIO(json.dumps({'nickname':cuser['nickname'], 'text':comment})), mimetype='text/json')
            return rsp
    except:
        traceback.print_exc()

@app.route('/comment', methods=['POST'])
def comment():
    try:
        r = {}
        r['comment'] = []

        from io import BytesIO
        p = request.get_json()
        textid = p['textid']

        c = globalv.collection_text.find_one({'_id':objectid.ObjectId(textid)})
        if c != None:
            if c.has_key('comment'):
                for i in c['comment']:
                    c = globalv.collection_text.find_one({'_id':i})
                    cuser = globalv.collection_user.find_one({'_id':c['postid']})

                    r['comment'].append({'nickname':cuser['nickname'], 'text':c['text']})

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/deltext', methods=['POST'])
def deltext():
    try:
        r = {}
        r['sucess'] = False

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        textid = p['textid']

        c = globalv.collection_text.find_one({'_id':objectid.ObjectId(textid)})
        if c != None:
            if c['postid'] == objectid.ObjectId(userid):
                r['sucess'] = True
                globalv.collection_text.remove({'_id':objectid.ObjectId(textid)})

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/homepage', methods=['POST'])
def homepage():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']

        r['url'] = "http://abelkhan.com/user/?uid=" + userid.encode('utf-8')

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/post', methods=['POST'])
def post():
    try:
        r = {}
        atuserid = []

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']
        text = p['text'].encode('utf-8')

        text = text.replace(' ', '&nbsp;')
        begin = text.find('@')
        if begin == -1:
            textnew = """\t\t\t\t<div style="max-width:550px; float:left">""" + text[0:] + """</div>\n"""
        else:
            textnew = """\t\t\t\t<div style="max-width:550px; float:left">""" + text[0: begin] + """</div>\n"""
        while begin != -1:
            ch = ''
            i = begin + 1
            while i < len(text):
                ch = text[i]

                if ch in [' ', ',', '，', '.', '。', '/', '?', '\'', '\"', ';', ':', '：', '；',  '{', '[', ']', '}', '【', '】', '{', '}', '\\', '|', '、', '|', '+', '=', ')', '(', '（', '）', '*', '*', '&', '^', '……', '%', '$', '#', '@', '!', '！', '~', '`', '~', '·']:
                    break
                i += 1
            end = i
            nickname = text[begin + 1: end]

            c = globalv.collection_user.find_one({'nickname':nickname})
            atuserid.append(c['_id'])

            globalv.collection_user.find_and_modify({'nickname':nickname}, {"$inc":{"atnum":1}}, True)

            textnew += """\t\t\t\t<div username=\"""" + nickname + """\" onmouseout="mouseoutotheruser(this)" onmouseover="mouseoverotheruser(this)" style="max-width:550px; float:left; color:LightCoral;">"""
            textnew += "@" + nickname
            textnew += """</div>\n"""

            if end != len(text) -1 :
                begin = text.find('@', end)
                if begin == -1:
                    textnew += """\t\t\t\t<div style="max-width:550px; float:left">""" + text[end: ] + """</div>\n"""
                    break

                textnew += """\t\t\t\t<div style="max-width:550px; float:left">""" + text[end: begin] + """</div>\n"""

        text = textnew

        globalv.collection_text.insert({'text':text, 'type':'text', 'postid':objectid.ObjectId(userid), 'time':time.time(), 'atuserid':atuserid}) #'comment':[], 'forward':None

        r['url'] = "http://abelkhan.com/user/?uid=" + userid.encode('utf-8')

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/imageico.png')
def imageico():
    try:
        from io import BytesIO
        return Response(BytesIO(imageico), mimetype='png')
    except:
        traceback.print_exc()

@app.route('/videoico.png')
def videoico1():
    try:
        from io import BytesIO
        return Response(BytesIO(videoico), mimetype='png')
    except:
        traceback.print_exc()

@app.route('/exit', methods=['POST'])
def exit():
    try:
        r = {}

        from io import BytesIO
        p = request.get_json()
        userid = p['userid']

        c = globalv.collection_user.find_one({'_id':objectid.ObjectId(userid)})
        if c != None:
            globalv.collection_user.update({'_id':objectid.ObjectId(userid)}, {'$set':{'login':False}})

        r['url'] = "http://abelkhan.com/login/"

        rsp = Response(BytesIO(json.dumps(r)), mimetype='text/json')
        return rsp
    except:
        traceback.print_exc()

@app.route('/')
def index():
    try:
        html = homepagehtml.homehtml + "\n\t</script>\n</html>"
        return html
    except:
        traceback.print_exc()

@app.route('/user/')
def user():
    try:
        def gettext(text):
            ret = """\t\t\t\t<div style="max-width:560px; margin:auto auto 5px 10px; float:left">"""
            ret += text['text'].encode('utf-8')
            ret += """</div>\n"""
            if text.has_key('forward'):
                text1 = globalv.collection_text.find_one({'_id':text['forward'], 'type':{'$in':['text', 'forward']}})
                user = globalv.collection_user.find_one({'_id':text1['postid']})

                ret += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutotheruser(this)" onmouseover="mouseoverotheruser(this)" style="float:left; color:LightCoral;">"""
                ret += """//@""" + user['nickname'].encode('utf-8')
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += """:"""
                ret += """</div>\n"""
                ret += """\t\t\t\t<div style="float:left">"""
                ret += gettext(text1)
                ret += """</div>\n"""

            return ret

        uid = request.args.get('uid')
        if uid:
            textlist = []
            c = globalv.collection_user.find_one({'_id':objectid.ObjectId(uid)})

            if c != None:
                if c['login'] != True:
                    return redirect("http://abelkhan.com/login/")

                if c.has_key('following'):
                    for f in c['following']:
                        ctext = globalv.collection_text.find({'postid':f, 'type':{'$in':['text', 'forward']}})
                        for text in ctext:
                            textlist.append(text)
                ctext = globalv.collection_text.find({'postid':objectid.ObjectId(uid), 'type':{'$in':['text', 'forward']}})
                for text in ctext:
                    textlist.append(text)
                textlist.sort(key=lambda x:x['time'], reverse=True)

                html = userpagehtml.userpagehtml

                for text in textlist:
                    user = globalv.collection_user.find_one({'_id':text['postid']})

                    html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:10px auto auto auto; clear:both;">\n"""
                    html += """\t\t\t\t<div username=\"""" + user['nickname'].encode('utf-8') + """\" onmouseout="mouseoutuser(this)" onmouseover="mouseoveruser(this)" style="padding-top:15px; margin:5px auto 15px 10px;">"""
                    html += user['nickname'].encode('utf-8')
                    html += """</div>\n"""
                    html += """<div style="background-color:rgb(255,255,255); font-size:90%;">\n"""
                    html += gettext(text)
                    html += """</div>\n"""
                    if text['postid'] == objectid.ObjectId(uid):
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="deltext(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """删除"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    else:
                        html += """\t\t\t\t<div style="padding-top:10px; background-color:rgb(255,255,255); font-size:80%; width:580px;height:25px">\n"""
                        html += """\t\t\t\t\t<div comment="true" textid=\"""" + str(text['_id'])  + """\" onclick="clickcomment(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """评论"""
                        html += """</div>\n"""
                        html += """\t\t\t\t\t<div textid=\"""" + str(text['_id'])  + """\" onclick="clickforward(this)" onmouseout="mouseouttools(this)" onmouseover="mouseovertools(this)" style="float:right;padding-bottom:5px; margin:auto auto 5px 8px;">"""
                        html += """转发"""
                        html += """</div>\n"""
                        html += """\t\t\t\t</div>\n"""
                    html += """<div style="clear:both;"></div>\n"""
                    html += """\t\t\t</div>\n"""

                html += userpagehtml.userpagehtml0
                html += """\t\t\t<div style="background-color:rgb(255,255,255); margin:auto auto auto 20px; width:240px;" >\n"""
                html += """\t\t\t\t<div style="padding-top:10px; padding-left:10px; padding-bottom:10px;">""" + c["nickname"].encode('utf-8') + """</div>\n"""
                html += """\t\t\t\t<div onclick="skiptext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">文章</div>\n"""
                html += """\t\t\t\t<div onclick="pmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">私信"""
                if c.has_key('pmnum') and c['pmnum'] > 0:
                    html += str(c['pmnum'])
                html += """</div>\n"""
                html += """\t\t\t\t<div onclick="atme(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">@我"""
                if c.has_key('atnum') and c['atnum'] > 0:
                    html += str(c['atnum'])
                html += """</div>\n"""
                html += """\t\t\t\t<div onclick="cmtext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="border-top-style:solid;border-width:1px;border-color:rgb(245,245,245);padding-top:10px; padding-left:10px;padding-bottom:10px;">评论"""
                if c.has_key('cmnum') and c['cmnum'] > 0:
                    html += str(c['cmnum'])
                html += """</div>\n"""
                html += """\t\t\t</div>\n"""
                html = html + userpagehtml.userpagehtml1 + "\n\t\tvar userid = \"" + uid.encode('utf-8') + "\";\n\t</script>\n</html>"
                return html

        return redirect("http://abelkhan.com/login/")
    except:
        traceback.print_exc()

def home_init():
    try:
        global imageico
        global videoico

        import os

        fileimage = open(os.path.split(os.path.realpath(__file__))[0] + '/backimage/tupian.png', 'rb')
        imageico = fileimage.read()
        fileimage.close()

        fileimage = open(os.path.split(os.path.realpath(__file__))[0] + '/backimage/shipin.png', 'rb')
        videoico = fileimage.read()
        fileimage.close()


    except:
        traceback.print_exc()
