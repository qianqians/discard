# -*- coding: UTF-8 -*-
# grab
# create at 2016/2/25
# autor: qianqians

import sys
reload(sys)
sys.setdefaultencoding('utf-8')

import urllib2
import cookielib

def get_page(url):
    try:
        headers = {'User-Agent':'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebkit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240',
                   'Connection':'Keep-Alive',
                   'Accept':'text/html, application/xhtml+xml, image/jxr, */*',
                   'Accept-Language':'zh-Hans-CN,zh-Hans;q=0.8,en-US;q=0.5,en;q=0.3',
                   }

        cookie_jar = cookielib.CookieJar()
        opener = urllib2.build_opener(urllib2.HTTPCookieProcessor(cookie_jar))
        req = urllib2.Request(url = url, headers = headers)
        response = opener.open(req, timeout = 5)
        the_page = response.read()
        headers = response.info()

        return the_page, headers
    except:
        import traceback
        traceback.print_exc()
