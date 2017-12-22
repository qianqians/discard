# -*- coding: UTF-8 -*-
# semantics
# create at 2016/2/25
# autor: qianqians

import sys
reload(sys)
sys.setdefaultencoding('utf-8')

preset_keylist = [{"word":["武侠", "玄幻", "女频", "择天记"], "semantics":["小说"]},
                  {"word":["武器", "装备", "陆军", "海军", "空军", "航母"], "semantics":["军事"]},
                  {"word":["火影", "海贼王", "死神", "进击的巨人", "灌篮高手", "宫崎骏"], "semantics":["日漫"]}]

def semantics(text):
    key2 = []
    key3 = []
    for item in preset_keylist:
        in_word = 0
        for key in item["word"]:
            key2.append(key)
            in_word += 1
        if in_word > 3:
            for key in item["semantics"]:
                key3.append(key)

    return key2, key3
