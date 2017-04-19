# -*- coding: UTF-8 -*-
# loginhtml
# create at 2016/3/24
# autor: qianqians

loginhtml = """<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <script>
        var iWidth = document.documentElement.clientWidth;
        var iHeight = document.documentElement.clientHeight;
    </script>
    <head>
        <title>abelkhan - 图片记录生活</title>
        <meta name="keywords" content="abelkhan" />
        <meta name="description" content="abelkhan - 图片记录生活" />

        <style type="text/css">
            a:link.link1 { text-decoration: none;color:DimGray}
            a:hover.link1 { text-decoration:none;color: DimGray}
            a:visited.link1 { text-decoration: none;color:DimGray}
            
            div#title{ margin:12% auto auto 44%;font-size:200%; color:rgb(255,255,255)}
            div#toolscontainer{ margin:30px auto auto 41%; }
            div#register{text-align:center; float:left; width:15%;color:Black;background-color:RGB(255,255,255);font-size:150%;}
            div#login{text-align:center; float:left; width:15%;color:RGB(255,255,255);background-color:Black;filter:alpha(opacity=50);opacity:0.5;font-size:150%;}
            div#ctrlcontainer{width:30%;height:146px;background-color:RGB(255,255,255); clear:both;}
            div#link1_1{clear:both; position:fixed; background-color:WhiteSmoke; width:100%; text-decoration: none; bottom:0;}
        </style>
    </head>
"""

loginhtml1 = """
        <div id="title">
            图片记录生活...
        </div>
        <div id="toolscontainer" >
            <div id="register" onclick="changetoregister(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)">
                <div style="margin:5px auto auto auto">注册</div>
            </div>
            <div id="login" onclick="changetologin(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)">
                <div style="margin:5px auto auto auto">登录</div>
            </div>
            <div id="ctrlcontainer">
                <input id="usernameinput" type="text" value="常用邮箱" onfocus="clickusernameinput(this)" onblur="blurusernameinput(this)" style="color:DimGray;width:60%;height:24px;margin:20px auto 10px 20%;" />
                <input id="keyinput" type="text" value="设置密码" onfocus="clickkeyinput(this)" onblur="blurkeyinput(this)" style="color:DimGray;width:60%;height:24px;margin:auto auto 10px 20%;" />
                <div id="inputregister" onclick="onregister(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="width:60%;height:24px;background-color:DeepSkyBlue;font-size:100%;margin:auto auto auto 20%;text-align:center;">
                    <div id="inputregisterbutton" style="padding-top:3px;color:RGB(255,255,255);">注册</div>
                </div>

                <input id="loginusernameinput" type="text" value="邮箱" onfocus="clickloginusernameinput(this)" onblur="blurloginusernameinput(this)" style="display:none;visibility:hidden;color:DimGray;width:60%;height:24px;margin:20px auto 10px 20%;" />
                <input id="loginkeyinput" type="text" value="密码" onfocus="clickloginkeyinput(this)" onblur="blurloginkeyinput(this)" style="display:none;visibility:hidden;color:DimGray;width:60%;height:24px;margin:auto auto 10px 20%;" />
                <div id="inputlogin" onclick="onlogin(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="display:none;visibility:hidden;width:60%;height:24px;background-color:DeepSkyBlue;font-size:100%;margin:auto auto auto 20%;text-align:center;">
                    <div id="inputloginbutton" style="display:none;visibility:hidden;padding-top:3px;color:RGB(255,255,255);">登陆</div>
                </div>

            </div>
        </div>
        <div id="link1_1">
            <a class="link1" target="_blank" href="http://www.miitbeian.gov.cn/" style="margin:auto auto auto 46%;">鄂ICP备16002459号</a>
        </div>
    </body>
    <script language="javascript" src="http://abelkhan.com/JSON.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequestError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequest.js"></script>
    <script language="javascript" src="http://abelkhan.com/alertWinMsg.js"></script>
    <script language="javascript" src="http://abelkhan.com/jsquest.js"></script>
    <script>
        function onregister(id)
        {
            var params = {"usermail":document.getElementById("usernameinput").value, "key":document.getElementById("keyinput").value};
            JSONRequest.post("http://abelkhan.com/post_register", params, function (requestNumber, value, exception){
                if (value["sucess"]){
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

        function onlogin(id)
        {
            var params = {"usermail":document.getElementById("loginusernameinput").value, "key":document.getElementById("loginkeyinput").value,};
            JSONRequest.post("http://abelkhan.com/post_login", params, function (requestNumber, value, exception){
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

        function changetoregister(id)
        {
            var ctrl = document.getElementById("register");
            ctrl.style.color="Black";
            ctrl.style.backgroundColor="RGB(255,255,255)";
            ctrl.style.filter="alpha(opacity=0)"
            ctrl.style.opacity=1.0

            var ctrl = document.getElementById("login");
            ctrl.style.color="RGB(255,255,255)";
            ctrl.style.backgroundColor="Black";
            ctrl.style.filter="alpha(opacity=50)"
            ctrl.style.opacity=0.5

            var ctrl = document.getElementById("usernameinput");
            ctrl.style.display="";
            ctrl.style.visibility="visible";

            var ctrl = document.getElementById("keyinput");
            ctrl.style.display="";
            ctrl.style.visibility="visible";

            var ctrl = document.getElementById("inputregister");
            ctrl.style.display="";
            ctrl.style.visibility="visible";

            var ctrl = document.getElementById("inputregisterbutton");
            ctrl.style.display="";
            ctrl.style.visibility="visible";

            var ctrl = document.getElementById("loginusernameinput");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";

            var ctrl = document.getElementById("loginkeyinput");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";

            var ctrl = document.getElementById("inputlogin");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";

            var ctrl = document.getElementById("inputloginbutton");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";
        }
        
        function changetologin(id)
        {
            var ctrl = document.getElementById("register");
            ctrl.style.color="RGB(255,255,255)";
            ctrl.style.backgroundColor="Black";
            ctrl.style.filter="alpha(opacity=50)"
            ctrl.style.opacity=0.5

            var ctrl = document.getElementById("login");
            ctrl.style.color="Black";
            ctrl.style.backgroundColor="RGB(255,255,255)";
            ctrl.style.filter="alpha(opacity=0)"
            ctrl.style.opacity=1.0

            var ctrl = document.getElementById("usernameinput");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";

            var ctrl = document.getElementById("keyinput");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";

            var ctrl = document.getElementById("inputregister");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";

            var ctrl = document.getElementById("inputregisterbutton");
            ctrl.style.display="none";
            ctrl.style.visibility="hidden";

            var ctrl = document.getElementById("loginusernameinput");
            ctrl.style.display="";
            ctrl.style.visibility="visible";

            var ctrl = document.getElementById("loginkeyinput");
            ctrl.style.display="";
            ctrl.style.visibility="visible";

            var ctrl = document.getElementById("inputlogin");
            ctrl.style.display="";
            ctrl.style.visibility="visible";

            var ctrl = document.getElementById("inputloginbutton");
            ctrl.style.display="";
            ctrl.style.visibility="visible";
        }

        function clickusernameinput(id)
        {
            if (id.value==="常用邮箱")
            {
                id.value=""
            }
        }

        function blurusernameinput(id)
        {
            if (id.value==="")
            {
                id.value="常用邮箱";
            }
        }

        function clickkeyinput(id)
        {
            if (id.value==="设置密码")
            {
                id.value=""
                id.type="password";
            }
        }

        function blurkeyinput(id)
        {
            if (id.value==="")
            {
                id.type="text";
                id.value="设置密码";
            }
        }

        function clickloginusernameinput(id)
        {
            if (id.value==="邮箱")
            {
                id.value=""
            }
        }

        function blurloginusernameinput(id)
        {
            if (id.value==="")
            {
                id.value="邮箱";
            }
        }

        function clickloginkeyinput(id)
        {
            if (id.value==="密码")
            {
                id.value=""
                id.type="password";
            }
        }

        function blurloginkeyinput(id)
        {
            if (id.value==="")
            {
                id.type="text";
                id.value="密码";
            }
        }

    </script>
</html>
"""
