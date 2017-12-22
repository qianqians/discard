# -*- coding: UTF-8 -*-
# dispose
# create at 2016/2/25
# autor: qianqians

import sys
reload(sys)
sys.setdefaultencoding('utf-8')

import chardet
import time

import config

collection_url_index = None
collection_url_profile = None

punctuations = [u'.',u',',u'[',u']',u'{',u'}',u'"',u'\'',u';',u':',u'<',u'>',u'!',u'?',u'(',u'（',u'）',u')',u'*',u'&',u'^',u'%',u'$',u'#',u'@',u'!',u'~',u'`',u'☆',
                u'，',u'》',u'。',u'《',u'？',u'/',u'：',u'；',u'“',u'‘',u'{',u'}',u'、',u'|',u'\r',u'\n',u'\0',u'\t',u' ',u'   ',u'+',u'-',u'=',u'_',u'【', u'】',
                u'　', u'★',u'　',u'！',u'·']

numlist = [u'1', u'2', u'3', u'4', u'5', u'6', u'7', u'8', u'9', u'0']

def invaildinfo(str):
    count = 0
    for ch in str:
        if ch in punctuations or ch in numlist:
            count += 1

    if count > len(str)/2:
        return True

    return False

def gettitle(titlelist):
    removelist = []
    for ustr in titlelist:
        if invaildinfo(ustr):
            removelist.append(ustr)

    for ustr in removelist:
        titlelist.remove(ustr)

    titlelist.sort(key=lambda x: len(x), reverse=True)

    if len(titlelist) > 0:
        return titlelist[0]
    else:
        return u''


def getprofile(profilelist):
    removelist = []
    for ustr in profilelist:
        if invaildinfo(ustr):
            removelist.append(ustr)

    for ustr in removelist:
        profilelist.remove(ustr)

    profilelist.sort(key=lambda x: len(x), reverse=True)

    if len(profilelist) > 0:
        return profilelist[0]
    else:
        return u''

def dispose(headers, urlinfo, weight, urllist):
    try:
        url = urlinfo['url']

        encodingdate = chardet.detect(headers['date'])
        date = unicode(headers['date'], encodingdate['encoding'])

        title = gettitle(urlinfo['title'])
        if len(title) > 16:
            title = title[0:16] + u'...'
        profile = getprofile(urlinfo['profile'])
        if title != u"" and profile != u"":
            print "update url", url
            collection_url_profile.update({'key':url},
                                          {'$set':{'key':url,
                                                   'urlprofile':profile.encode('utf-8', 'ignore'),
                                                   'timetmp':time.time(),
                                                   'date':date.encode('utf-8', 'ignore'),
                                                   'title':title.encode('utf-8', 'ignore')}},
                                          True)

            updatekeywords = []
            weight1 = []
            for key in urlinfo['keys']['1']:
                key = key.encode('utf-8', 'ignore')
                if key not in weight1:
                    weight1.append(key)
            for key in weight1:
                if key not in updatekeywords:
                    updatekeywords.append(key)
                    collection_url_index.update({'key':key, 'url':url},
                                                {'$set':{'url':url, 'key':key, 'weight':weight+500}},
                                                True)

            weight2 = []
            for key in urlinfo['keys']['2']:
                key = key.encode('utf-8', 'ignore')
                if key not in weight2:
                    weight2.append(key)
            for key in weight2:
                if key not in updatekeywords:
                    updatekeywords.append(key)
                    collection_url_index.update({'key':key, 'url':url},
                                                {'$set':{'url':url, 'key':key, 'weight':weight+300}},
                                                True)

            weight3 = []
            for key in urlinfo['keys']['3']:
                key = key.encode('utf-8', 'ignore')
                if key not in weight3:
                    weight3.append(key)
            for key in weight3:
                if key not in updatekeywords:
                    updatekeywords.append(key)
                    collection_url_index.update({'key':key, 'url':url},
                                                {'$set':{'url':url, 'key':key, 'weight':weight}},
                                                True)

        for key, info in urllist.iteritems():
            config.preset_urllist.append(info)

    except:
        import traceback
        traceback.print_exc()
