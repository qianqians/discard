# -*- coding: UTF-8 -*-
# analysis
# create at 2016/2/25
# autor: qianqians

import sys
reload(sys)
sys.setdefaultencoding('utf-8')

import HTMLParser
import chardet
import nltk
import jieba

import semantics

prefixes = ['http', 'https', 'www']

postfix = ['com', 'cn', 'net', 'org', 'edu', 'gov', 'int', 'mil', 'ad', 'ae', 'af', 'ag', 'ai', 'al', 'am', 'an', 'ao', 'aq', 'ar',
           'as', 'at', 'au', 'aw', 'az', 'ba', 'bb', 'bd', 'be', 'bf', 'bg', 'bh', 'bi', 'bm', 'bj', 'bn', 'bo', 'br', 'bs', 'bt',
           'bv', 'bw', 'by', 'bz', 'ca', 'cc', 'cf']

def countweight(url):
    if url.count('/') > 2:
        return 0

    keywords = nltk.word_tokenize(url)

    removelist = []
    for k in keywords:
        if k in prefixes:
            removelist.append(k)

        if k in postfix:
            removelist.append(k)

    for k in removelist:
        keywords.remove(k)

    if len(keywords) == 1:
        return 500

    return 300

def judged_url(url):
    if url is None or url.find('http') != 0:
        return False
    return True

def ingoreurl(url):
    count = 0
    for ch in url:
        if ch == '/':
            count += 1

    return count > 4

class analysis(HTMLParser.HTMLParser):
    def __init__(self, urlinfo):
        HTMLParser.HTMLParser.__init__(self)

        self.urlinfo = urlinfo

        self.urllist = {}
        self.sub_url = ""

        self.current_url = urlinfo['url']
        self.weight = countweight(self.current_url)

        self.urlinfo['profile'].append("天猫 -中国地标性的线上综合购物平台，拥有超1.2万国际品牌，18万知名大牌，8.9万品牌旗舰店。商品涵盖服饰鞋包，美妆护肤，家电数码，母婴玩具，食品饮料等各品类，为日益成熟的中国消费者提供全球精选好货、无后顾之忧的好服务，致力于为你打造品质购物体验！")

        tokens = nltk.word_tokenize(self.current_url)
        for k in tokens:
            if k not in prefixes and k not in postfix:
                self.urlinfo['keys']['1'].append(k)

        self.current_tag = ""
        self.style = ""

    def handle_starttag(self, tag, attrs):
        self.current_tag = tag
        self.style = 'None'
        self.sub_url = ""

        if tag == 'meta':
            for name,value in attrs:
                if name == 'name':
                    if value == 'keywords' or value == 'metaKeywords':
                        self.style = 'keywords'
                    elif value == 'description' or value == 'metaDescription':
                        self.style = 'profile'

            for name,value in attrs:
                if name == 'content':
                    try:
                        if isinstance(value, str):
                            encodingdate = chardet.detect(value)
                            if encodingdate['encoding']:
                                value = unicode(value, encodingdate['encoding'])

                        if self.style == 'keywords':
                            keywords = jieba.cut_for_search(value)
                            if isinstance(keywords, list):
                                for key in keywords:
                                    self.urlinfo['keys']['1'].append(key)

                        elif self.style == 'profile':
                            self.urlinfo['profile'].append(value)

                            key2, key3 = semantics.semantics(value)
                            for key in key2:
                                self.urlinfo['keys']['2'].append(key)
                            for key in key3:
                                self.urlinfo['keys']['3'].append(key)

                            keys = jieba.cut_for_search(value)
                            for key in keys:
                                self.urlinfo['keys']['3'].append(key)

                            tlen = 16
                            if len(value) < 16:
                                tlen = len(value)
                            self.urlinfo['title'].append(value[0:tlen])

                    except:
                        import traceback
                        traceback.print_exc()

        if tag == 'a' or tag == 'A':
            self.sub_url = ""
            for name,value in attrs:
                if name == 'href':
                    if len(value) == 0:
                        return

                    if not judged_url(value):
                        if len(value) > 1 and value[0] == '/' and value[1] == '/':
                            value = "http:" + value
                        elif self.current_url[len(self.current_url) - 1] != '/' and value[0] != '/':
                            value = self.current_url + '/' + value
                        else:
                            value = self.current_url + value

                    if value != self.current_url and len(value) < 64 and not ingoreurl(value):
                        self.urllist[value] = {'url':value, 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0}
                        self.sub_url = value

    def handle_data(self, data):
        if self.current_tag == 'title':
            try:
                encodingdate = chardet.detect(data)
                if encodingdate['encoding']:
                    data = unicode(data, encodingdate['encoding'])

                    if len(data) > 0:
                        self.urlinfo['title'].append(data)

                        key2, key3 = semantics.semantics(data)
                        for key in key2:
                            self.urlinfo['keys']['2'].append(key)
                        for key in key3:
                            self.urlinfo['keys']['3'].append(key)

                        keys = jieba.cut_for_search(data)
                        if isinstance(keys, list) and len(keys) > 0:
                            for key in keys:
                                self.urlinfo['keys']['3'].append(key)
            except:
                import traceback
                traceback.print_exc()

        elif self.current_tag in ['h1', 'h2', 'h3', 'h4', 'h5', 'h6']:
            try:
                encodingdate = chardet.detect(data)
                if encodingdate['encoding']:
                    data = unicode(data, encodingdate['encoding'])

                    if len(data) > 0:
                        self.urlinfo['title'].append(data)

                        key2, key3 = semantics.semantics(data)
                        for key in key2:
                            self.urlinfo['keys']['2'].append(key)
                        for key in key3:
                            self.urlinfo['keys']['3'].append(key)

                        keys = jieba.cut_for_search(data)
                        if isinstance(keys, list) and len(keys) > 0:
                            for key in keys:
                                self.urlinfo['keys']['3'].append(key)
            except:
                import traceback
                traceback.print_exc()

        elif self.current_tag == 'a' or self.current_tag == 'A':
            try:
                if self.sub_url != "":
                    encodingdate = chardet.detect(data)
                    if encodingdate['encoding']:
                        data = unicode(data, encodingdate['encoding'])

                        keys = jieba.cut_for_search(data)
                        if isinstance(keys, list) and len(keys) > 0:
                            for key in keys:
                                if key not in self.urllist[self.sub_url]['keys']['1']:
                                    self.urllist[self.sub_url]['keys']['1'].append(key)

                        tlen = 16
                        if len(data) < 16:
                            tlen = len(data)
                        self.urllist[self.sub_url]['title'].append(data[0:tlen])

                        if len(data) > 32:
                            self.urllist[self.sub_url]['profile'].append(data[0:32] + u"...")

            except:
                import traceback
                traceback.print_exc()
        else:
            if self.current_tag == 'div' or self.current_tag == 'p':
                pass
