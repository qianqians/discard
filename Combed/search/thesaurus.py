# -*- coding: UTF-8 -*-
# thesaurus
# create at 2016/3/25
# autor: qianqians
from doclex import doclex
import jieba

collection_key = None

thesaurus = [['mongo','mongodb'],
             ['cocos','cocos2d','cocos2d-x'],
             ['c++','cpp'],
             ['AlphaGo', 'alphaGo', '阿尔法狗', '阿尔法']]

def processinput(input):
    keys = doclex.splityspace(input)
    for k in keys:
        collection_key.update({"key":k}, {"key":k}, True)

    keys1 = jieba.cut_for_search(input)
    for k in keys1:
        collection_key.update({"key":k}, {"key":k}, True)

    findkey = []
    for key in keys:
        if key not in findkey:
            findkey.append(key)
    for key in keys1:
        if key not in findkey:
            findkey.append(key)

    addkey = []
    for keywords in thesaurus:
        for key in findkey:
            if key in keywords:
                addkey.extend(keywords)

    for key in addkey:
        if key not in findkey:
            findkey.append(key)

    return findkey