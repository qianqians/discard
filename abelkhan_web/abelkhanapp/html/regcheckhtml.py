# -*- coding: UTF-8 -*-
# regcheckhtml
# create at 2016/3/13
# autor: qianqians

regcheckhtml = """<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <title>abelkhan - 图片记录生活</title>

        <style type="text/css">
            div#title{text-align:center;font-size:240%;width:100%;height:65px;color:RGB(255,255,255);background-color:Black;}
            div#container{width:40%; margin:50px auto auto 30%; border-style:solid; border-width: 1px;border-color:RGB(122,122,122); background-color:RGB(255,255,255);}
            div#title1{text-align:center;font-size:200%;}
            div#nodes{font-size:120%;border-bottom-style:solid; border-width: 1px; border-color:RGB(122,122,122);}
        </style>
    </head>

    <body bgcolor="rgb(222,222,222)">
        <div id="title">
            <div style="padding-top:10px;">abelkhan</div>
        </div>

        <div id="container" >
            <div id="title1">
                <div style="padding-top:30px;padding-bottom:10px;">欢迎开通abelkhan</div>
            </div>
            <input id="nicknameinput" type="text" value="昵称" onfocus="clicknicknameinput(this)" onblur="blurnicknameinput(this)" style="color:DimGray;width:50%;height:32px;margin:20px auto 20px 25%;" />
            <div id="next" onclick="onnext(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="width:50%;height:32px;background-color:DeepSkyBlue;font-size:100%;margin:auto auto 40px 25%;text-align:center;">
                <div id="nextbutton" style="padding-top:6px;color:RGB(255,255,255);">下一步</div>
            </div>
        </div>
    </body>
    <script language="javascript" src="http://abelkhan.com/JSON.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequestError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequest.js"></script>
    <script language="javascript" src="http://abelkhan.com/alertWinMsg.js"></script>
    <script language="javascript" src="http://abelkhan.com/jsquest.js"></script>
    <script>
        function onnext(id){
            var params = {"regusrid":regusrid, "nickname":document.getElementById("nicknameinput").value};
            JSONRequest.post("http://abelkhan.com/checkregisternext", params, function (requestNumber, value, exception){
                if (value["sucess"]){
                    setCookie("usermail", value["usermail"], 30);

                    location.assign(value["url"]);
                    window.location=value["url"];
                    location.href=value["url"];
                }
                else
                {
                    alertWinMsg(value["error"]);
                }
            });
        }

        function mouseoutctrl(id)
        {
            id.style.cursor="auto";
        }

        function mouseoverctrl(id)
        {
            id.style.cursor="pointer";
        }

        function clicknicknameinput(id)
        {
            if (id.value==="昵称")
            {
                id.value=""
            }
        }

        function blurnicknameinput(id)
        {
            if (id.value==="")
            {
                id.value="昵称";
            }
        }

"""