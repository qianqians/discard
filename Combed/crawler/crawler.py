# -*- coding: UTF-8 -*-
# crawler
# create at 2015/10/30
# autor: qianqians

import sys
reload(sys)
sys.setdefaultencoding('utf-8')

sys.path.append('../3part/')

import pymongo
import nltk
import chardet

import analysis
import analysis_tmall
import config
import dispose
import grab

if __name__ == '__main__':
    conn = pymongo.Connection('localhost',27017)
    db = conn.webseach

    dispose.collection_url_index = db.urlindex
    dispose.collection_url_profile = db.urlprofile
    collection_key = db.keys

    def get_analysis(urlinfo):
        if urlinfo['url'].find("tmall") != -1:
            return analysis_tmall.analysis(urlinfo)
        
        return analysis.analysis(urlinfo)

    def run():
        #nltk.download()

        import copy
        urllist = copy.deepcopy(config.preset_urllist)

        while True:
            for urlinfo in urllist:
                try:
                    info = grab.get_page(urlinfo['url'])

                    if info == None:
                        print "url, error", urlinfo['url']
                        continue

                    data, headers = info

                    a = get_analysis(urlinfo)
                    a.feed(data)

                    dispose.dispose(headers, a.urlinfo, a.weight, a.urllist)
                except:
                    import traceback
                    traceback.print_exc()

            urllist = copy.deepcopy(config.preset_urllist)

    run()
