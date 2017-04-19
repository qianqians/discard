# -*- coding: UTF-8 -*-
# homepagehtml
# create at 2016/3/1
# autor: qianqians
searchhtml = """
<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <title>搜一搜 - 开源搜索引擎</title>
        <style type="text/css">
            a:link { color:RoyalBlue}

            a:link.tools { text-decoration: none;color:RoyalBlue}
            a:hover.tools { text-decoration:none;color: CornflowerBlue}
            a:visited.tools { text-decoration: none;color:RoyalBlue}

            a:link.link1 { text-decoration: none;color:DimGray}
            a:hover.link1 { text-decoration:none;color: DimGray}
            a:visited.link1 { text-decoration: none;color:DimGray}

            div#cb_1{ visibility:visible; margin:0px auto auto 0px; }
            div#titlelogincontainer{ visibility:visible; margin:1px auto auto 1px; }
            div#titlereg{ visibility:visible; color:rgb(122,122,152); float:right; }
            div#titlelogin{ visibility:visible; color:rgb(122,122,152); float:right; }
            div#titleusernamecontainer{ visibility:hidden; margin:1px auto auto 1px; }
            div#titlelgoinout{ visibility:hidden; color:rgb(122,122,152); float:right; }
            div#titleusername{ visibility:hidden; color:rgb(122,122,152); float:right; }
            div#search_output0_1{ visibility:visible; clear:both;  margin:30px auto auto 5px; clear:both;}
            div#notes11_1{ height:24px; visibility:visible; font-size:165%; color:rgb(100,100,200); float:left; margin:auto 10px auto auto; }
            div#title_edit1_1{width:300px; height:24px; visibility:visible; float:left; }
            div#button1_1{width:80px; height:30px; visibility:visible; float:left; margin:auto auto 30px auto; }
            div#search_output1_1{width:800px; border-color:rgb(150,150,150); visibility:visible; float:left; clear:both;}
            div#search_output_1{width:780px; visibility:visible; float:left; margin:0px auto auto 0px; }
            div#ad_1{width:270px; border-left-style:solid; border-width: 1px; visibility:visible; float:left; }
            div#jsad_1{width:250px; float:left; margin:0px auto auto 10px; }
            div#event_1{ visibility:visible; margin:auto auto auto auto;}
            div#bottomtools{ visibility:visible; margin:60px auto 60px auto; clear:both;}
            div#link{ visibility:visible; background-color:WhiteSmoke; margin:auto auto 3px auto; clear:both; bottom:10px; position:fixed; }
        </style>
    </head>

    <body>
        <div id="cb_1">
            <div id="titlelogincontainer">
                <div id="titlereg" onmouseover="titleregonmouseover(this)" onmouseout="titleregonmouseout(this)" onclick="titleregonclick(this)" >&nbsp;注册</div>
                <div id="titlelogin" onmouseover="titleloginonmouseover(this)" onmouseout="titleloginonmouseout(this)" onclick="titleloginonclick(this)" >登陆&nbsp;</div>
            </div>
            <div id="titleusernamecontainer">
                <div id="titlelgoinout" onmouseover="titlelgoinoutonmouseover(this)" onmouseout="titlelgoinoutonmouseout(this)" onclick="titlelgoinoutonclick(this)" >&nbsp;&nbsp;&nbsp;退出</div>
                <div id="titleusername" ></div>
            </div>
            <div id="search_output0_1">
                <div id="notes11_1" >搜一搜</div>
                <div id="title_edit1_1">
                    <input id="title_edit" type="text" onkeydown=title_edit1onenterdown(this) style="height:24px;width:300px">
                </div>
                <div id="button1_1">
                    <button id="button1" type="button" style="width:80px; height:30px;" onclick="button1onclick(this)" >搜一搜</button>
                </div>
                <div id="search_output1_1">
                    <div id="search_output_1">
"""

searchhtml0 = """
                    </div>
                </div>
                <div id="ad_1">
                    <div id="jsad_1">
                        <script charset="gbk" type="text/javascript" src="http://union.dangdang.com/adapi/sc?id=dn3jihsx4p&from=P-329749" ></script>
                    </div>
                </div>
            </div>
            <div id="bottomtools">
"""

searchhtml1 = """
            </div>
        </div>
        <div id="link">
            <a id="link" class="link1" target="_blank" href="http://www.abelkhan.com/collection/">向我们捐助</a>
            <a id="link" class="link1" target="_blank" href="http://www.abelkhan.com/guestbook/">提出意见</a>
        </div>
    </body>

    <script language="javascript" src="http://abelkhan.com/JSON.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequestError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequest.js"></script>
    <script language="javascript" src="http://abelkhan.com/alertWinMsg.js"></script>
    <script language="javascript" src="http://abelkhan.com/loginPop.js"></script>
    <script>
        function onurlclick(id){
            JSONRequest.post("http://abelkhan.com/onurlclick", {"sid":sid, "input":document.getElementById("title_edit").value, "url": id.href}, function (requestNumber, value, exception){});
        };

        function titleregonmouseover(id){
            id.style.color="rgb(0,0,250)";
            id.style.cursor="pointer";
        }
        function titleregonmouseout(id){
            id.style.color="rgb(122,122,152)";
            id.style.cursor="auto";
        }
        function titleregonclick(id){
            var params = {"sid":sid};
            JSONRequest.post("http://abelkhan.com/callregister", params,
                function (requestNumber, value, exception){
                    if (popobj != null){
                        document.body.removeChild(popobj);
                        document.body.removeChild(poptable);
                        poptable = null;
                        popobj = null;
                    }
                    alertWinpopreg(400, value);
                });
        }

        function titleloginonmouseover(id){
            id.style.color="rgb(0,0,250)";
            id.style.cursor="pointer";
        }
        function titleloginonmouseout(id){
            id.style.color="rgb(122,122,152)";
            id.style.cursor="auto";
        }
        function titleloginonclick(id){
            var params = {"sid":sid};
            JSONRequest.post("http://abelkhan.com/calllogin", params,
                function (requestNumber, value, exception){
                    if (popobj != null){
                        document.body.removeChild(popobj);
                        document.body.removeChild(poptable);
                        poptable = null;
                        popobj = null;
                    }
                    alertWinpopLogin(400, value);
                });
        }

        function titlelgoinoutonmouseover(id){
            id.style.color="rgb(0,0,250)";
            id.style.cursor="pointer";
        }
        function titlelgoinoutonmouseout(id){
            id.style.color="rgb(122,122,152)";
            id.style.cursor="auto";
        }
        function titlelgoinoutonclick(id){
            document.getElementById("titleusername").style.visibility="hidden";
            document.getElementById("titleusername").style.display="none";
            document.getElementById("titlelgoinout").style.visibility="hidden";
            document.getElementById("titlelgoinout").style.display="none";
            document.getElementById("titleusernamecontainer").style.visibility="hidden";
            document.getElementById("titleusernamecontainer").style.display="none";
            document.getElementById("titlereg").style.visibility="visible";
            document.getElementById("titlereg").style.display="";
            document.getElementById("titlelogin").style.visibility="visible";
            document.getElementById("titlelogin").style.display="";
            document.getElementById("titlelogincontainer").style.visibility="visible";
            document.getElementById("titlelogincontainer").style.display="";

            username = null;

            loginout();
        }

        function title_edit1onenterdown(id){
            if (event.keyCode === 13){
                postsearchrequest(1);
            }
        }

        function button1onclick(id){
            postsearchrequest(1);
        }
"""
