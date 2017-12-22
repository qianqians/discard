# -*- coding: UTF-8 -*-
# guestbook
# create at 2016/3/13
# autor: qianqians

from flask import *
from websearchapp import *
from guestbookhtml import *
import pymongo
import traceback
import time

collection_book = None #{'text':text, 'username':username, 'date':date, 'count':count}

@app.route('/guestbook/')
def guestbook_index():
    try:
        index = int(request.args.get('index', 1))

        c = collection_book.find().skip((index-1)*15).limit(15)

        searchhtml1 = searchhtml

        for i in c:
            searchhtml1 += "\t\t\t<div style=\"margin:20px 0px 20px 0px; border-bottom-style:solid; border-width: 1px; clear:both\">\n"
            searchhtml1 += "\t\t\t\t<div>" + i['text'] + "</div>"
            searchhtml1 += "\t\t\t\t<div style=\"background-color:Lavender;\">" + str(i['count']) + "#&nbsp;&nbsp;&nbsp;&nbsp;" + i['username'] + "&nbsp;&nbsp;&nbsp;&nbsp;" + i['date'] + "</div>"
            searchhtml1 += "\t\t\t</div>"

        count = collection_book.find().count()
        pagecount = (count+14)/15

        begin = (index-2) <= 0 and 1 or (index-2)
        end = index+2
        if (index+2) > pagecount:
            end = pagecount
        if (end - begin) < 4:
            new = end - 4
            begin = new <= 0 and 1 or new

        if index > begin:
            url = "http://www.abelkhan.com/guestbook/?"
            url = url + "index=" + str(int(index)-1)
            searchhtml1 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:10px 5px 20px 5px; float:left\" target=\"_blank\" href=\"" + url + "\">上一页</a>"

        if (end - begin) > 0:
            for i in xrange(begin, end+1):
                if int(i) == int(index):
                    url = "http://www.abelkhan.com/guestbook/?"
                    url = url + "index=" + str(int(i))
                    searchhtml1 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:10px 5px 20px 5px; float:left; color: DimGray\" target=\"_blank\" href=\"" + url + "\">" + str(int(i)) + "</a>"
                else:
                    url = "http://www.abelkhan.com/guestbook/?"
                    url = url + "index=" + str(int(i))
                    searchhtml1 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:10px 5px 20px 5px; float:left\" target=\"_blank\" href=\"" + url + "\">" + str(int(i)) + "</a>"

        if index < end:
            url = "http://www.abelkhan.com/guestbook/?"
            url = url + "index=" + str(int(index)+1)
            searchhtml1 += "\t\t\t\t\t<a class=\"tools\" target=\"_self\" style=\"margin:10px 5px 20px 5px; float:left\" target=\"_blank\" href=\"" + url + "\">下一页</a>"

        return searchhtml1 + searchhtml0
    except:
        traceback.print_exc()

@app.route('/postinput', methods=['POST'])
def postinput():
    try:
        from io import BytesIO

        p = request.get_json()
        text = p["input"]
        username = p['username']
        if username == "":
            username = "匿名"
        count = collection_book.find().count()+1

        index = (count+14)/15

        collection_book.insert({'text':text, 'username':username, 'date':time.strftime('%Y-%m-%d %X', time.localtime( time.time() ) ), 'count':count})

        url = "http://www.abelkhan.com/guestbook/?"
        url = url + "index=" + str(index)

        return Response(BytesIO(json.dumps({"url":url})), mimetype='text/json')
    except:
        traceback.print_exc()

def guestbook_init():
    try:
        conn = pymongo.Connection('localhost',27017)
        db = conn.webseach

        global collection_book

        collection_book = db.book

    except:
        traceback.print_exc()