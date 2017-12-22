# -*- coding: UTF-8 -*-
# homepagehtml
# create at 2016/3/1
# autor: qianqians
homehtml = """
<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <title>搜一搜 - 开源搜索引擎</title>
        <meta name="baidu_union_verify" content="9fab29fc8fcf13209f0ad90d810a6a17">
        <meta name="keywords" content="搜一搜" />
        <meta name="description" content="开源搜索引擎，致力于提供优秀的搜索服务" />
        <link type="application/opensearchdescription+xml" href="opensearch.xml" title="abelkhan" rel="search" />
        <style type="text/css">
            a:link.link1 { text-decoration: none;color:DimGray}
            a:hover.link1 { text-decoration:none;color: DimGray}
            a:visited.link1 { text-decoration: none;color:DimGray}

            div#cb1_1{ visibility:visible; margin:0px auto auto 0px; }
            //div#titlelogincontainer{ visibility:visible; margin:1px auto auto 1px; }
            //div#titlereg{ visibility:visible; color:rgb(122,122,152); float:right; }
            //div#titlelogin{ visibility:visible; color:rgb(122,122,152); float:right; }
            //div#titleusernamecontainer{ visibility:hidden; margin:1px auto auto 1px; }
            //div#titlelgoinout{ visibility:hidden; color:rgb(122,122,152); float:right; }
            //div#titleusername{ visibility:hidden; color:rgb(122,122,152); float:right; }
            div#title_input12_1{ visibility:visible; clear:both;  margin:12.59% auto auto 21.59%; clear:both;}
            div#notestitle{ visibility:visible; font-size:200%; color:rgb(100,100,200); margin:auto auto 3px 11px;}
            div#title_edit_1{width:300px; height:24px; visibility:visible; float:left; margin:0px auto auto 10px; }
            div#button_1{width:80px; height:30px; visibility:visible; float:left; margin:0px auto auto 3px; }
            div#link1_1{clear:both; position:fixed; z-index:10; visibility:visible; text-align:center; margin:auto auto 3px 44%;  text-decoration: none; bottom:0;}
        </style>
    </head>

    <body>
        <div id="cb1_1">
            <div id="title_input12_1">
                <div id="notestitle" >搜一搜</div>
                <div id="title_edit_1">
                    <input id="title_edit" type="text" onkeydown=title_editonenterdown(this) style="height:24px;width:300px">
                </div>
                <div id="button_1">
                    <button id="searchbutton" type="button" style="width:80px; height:30px;" onclick="buttononclick(this)" >搜一搜</button>
                </div>
            </div>
        </div>
        <div id="link1_1">
            <a id="link" target="_blank" href="http://www.abelkhan.com/collection/">向我们捐助</a>
            <a id="link" target="_blank" href="http://www.abelkhan.com/guestbook/">提出意见</a>
            <a id="link" class="link1" target="_blank" href="http://www.miitbeian.gov.cn/">鄂ICP备16002459号</a>
        </div>
    </body>
    <script language="javascript" src="http://abelkhan.com/JSON.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequestError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequest.js"></script>
    <script language="javascript" src="http://abelkhan.com/alertWinMsg.js"></script>
    <script language="javascript" src="http://abelkhan.com/loginPop.js"></script>
    <script>
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
                    if (popobj !== null){
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
                    if (popobj !== null){
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

        function title_editonenterdown(id){
            if (event.keyCode === 13){
                postsearchrequest(1)
            }
        }
        function buttononclick(id){
            postsearchrequest(1)
        }
        """