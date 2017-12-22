# -*- coding: UTF-8 -*-
# usesearch
# create at 2015/10/31
# autor: qianqians

import sys
sys.path.append('../')
import pagecache
import thesaurus
import pymongo
import time
import random
from doclex import doclex


collection_url_profile = None
collection_url_index = None

def find_page(input, index):
    def page_not_in_pagelist(pagelist, page):
        for page1 in pagelist:
            if page['url'] == page1['url']:
                return False
        return True

    def find_page1(keys):
        def update_pagelist(it, page_list):
            url = i['url']
            
            for page1 in page_list:
                if url == page1['url']:
                    page1['weight'] += 100
                    return 

            page = {}
            page['url'] = url
            page['weight'] = 0
            if i.has_key('weight'):
                page['weight'] = i['weight']
            page["userchose"] = 0
            if i.has_key('userchose'):
                page["userchose"] = i['userchose']

            page_list.append(page)

        key1 = []
        for k in keys:
            k = doclex.tolower(k)
            if k not in key1:
                key1.append(k)
        key = key1

        page_list = []
        for k in keys:
            c = collection_url_index.find({"key":k})
            for i in c:
                update_pagelist(i, page_list)

        return page_list

    page_list = []
    while True:
        if pagecache.key_page.has_key(input):
            page_list_info = pagecache.key_page[input]

            if (time.time() - page_list_info['timetmp']) < 60*60*60:
                page_list = page_list_info['page_list']
                break

        findkey = thesaurus.processinput(input)
        page_list = find_page1(findkey)
        page_list.sort(key=lambda x:x['weight']+x['userchose'], reverse=True)

        pagecache.key_page[input] = {'page_list':page_list, 'timetmp':time.time()}

        break

    count = len(page_list)
    end = 10*index
    if count < 10*index:
        end = count
    page_list = page_list[10*(index-1): end]

    page_list1 = []
    for url in page_list:
        c = collection_url_profile.find({'key':url['url']})
        for i in c:
            page = {}
            page['url'] = i['key']
            page['timetmp'] = i['timetmp']
            page["profile"] = i['urlprofile']
            page["date"] = i['date']
            page["title"] = i['title']

            page_list1.append(page)

    return page_list1, count
