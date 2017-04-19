# -*- coding: UTF-8 -*-
# regsuchtml
# create at 2016/3/13
# autor: qianqians

regsuchtml = """<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <title>邮箱验证</title>

        <style type="text/css">
            div#title{text-align:center;font-size:240%;width:100%;height:65px;color:RGB(255,255,255);background-color:Black;}
            div#container{width:40%; margin:50px auto auto 30%; border-style:solid; border-width: 1px;border-color:RGB(122,122,122); background-color:RGB(255,255,255);}
            div#title1{font-size:200%; border-bottom-style:solid; border-width: 1px;border-color:RGB(122,122,122);}
            div#nodes{font-size:120%;border-bottom-style:solid; border-width: 1px; border-color:RGB(122,122,122);}
        </style>
    </head>

    <body bgcolor="rgb(222,222,222)">
        <div id="title">
            <div style="padding-top:10px;">abelkhan</div>
        </div>

        <div id="container" >
            <div id="title1">
                <div style="padding-top:10px;padding-bottom:10px;padding-left:20px;">邮箱验证</div>
            </div>
            <div id="nodes">
                <div style="padding-top:30px;padding-bottom:20px;padding-left:20px;">邮箱验证后，即可完成注册！请登录邮箱完成注册</div>
            </div>
            <div style="padding-top:30px;padding-bottom:5px;padding-left:20px;">还没有收到验证邮件？</div>
            <ol>
                <li style="font-size:80%;padding-top:5px;color:RGB(122,122,122);">尝试到广告邮件、垃圾邮件目录里找找看</li>
                <li style="font-size:80%;color:RGB(122,122,122);">
                    <div style="color:DodgerBlue;" onclick="resendmail(this)" onmouseover="mouseoverctrlresend(this)" onmouseout="mouseoutctrlresend(this)">再次发送验证邮件</div>
                </li>
                <li style="font-size:80%;padding-bottom:60px;color:RGB(122,122,122);">
                    如果重发注册验证邮件仍然没有收到，
                    <a href="http://abelkhan.com/login/" style="text-decoration: none;color:DodgerBlue ;">请更换另一个邮件地址</a>
                </li>
            </ol>
        </div>
    </body>
    <script language="javascript" src="http://abelkhan.com/JSON.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequestError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequest.js"></script>
    <script language="javascript" src="http://abelkhan.com/alertWinMsg.js"></script>
    <script>
        function resendmail(id){
            var params = {"regusrid":regusrid};
            JSONRequest.post("http://abelkhan.com/post_resendmail", params, function (requestNumber, value, exception){
                if (value["sucess"]){
                    alertWinMsg("验证邮件发送成功!");
                }
            });
        }

        function mouseoverctrlresend(id){
            id.style.cursor="pointer";
        }

        function mouseoutctrlresend(id){
            id.style.cursor="auto";
        }
"""