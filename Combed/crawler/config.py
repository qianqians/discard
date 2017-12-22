# -*- coding: UTF-8 -*-
# config
# create at 2016/2/25
# autor: qianqians

import sys
reload(sys)
sys.setdefaultencoding('utf-8')

preset_urllist = [{"url":"http://abelkhan.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"https://www.tmall.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.jd.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"https://www.douban.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.readnovel.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.hongxiu.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.zhulang.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.kanshu.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"https://www.hao123.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.qq.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.163.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.tom.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://sina.com.cn", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.chinaunix.net", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.iqiyi.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.bilibili.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.acfun.tv", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.tudou.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.youku.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"https://github.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://cn.bing.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.penbbs.com/forum.php", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.zol.com.cn", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.17173.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.cnblogs.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.csdn.net", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.cppblog.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":'http://www.jobbole.com', 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.qidian.com/Default.aspx", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.zongheng.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://chuangshi.qq.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.jjwxc.net", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"https://www.taobao.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.baidu.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.google.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.suning.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://jiadian.gome.com.cn", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.w3school.com.cn", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.xitek.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://codingnow.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://jj.hbtv.com.cn", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.weiqiok.com", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"https://www.cjdby.net", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0},
                  {"url":"http://www.tiexue.net", 'keys':{'1':[], '2':[], '3':[]}, 'title':[], 'profile':[], 'weight':0}]
