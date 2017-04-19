# -*- coding: UTF-8 -*-
# userpagehtml
# create at 2016/3/1
# autor: qianqians
userpagehtml = """
<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <title>abelkhan - 图片记录生活</title>
        <meta name="keywords" content="abelkhan" />
        <meta name="description" content="abelkhan - 图片记录生活" />
        <style type="text/css">
            a:link.link1 { text-decoration: none;color:DimGray}
            a:hover.link1 { text-decoration:none;color: DimGray}
            a:visited.link1 { text-decoration: none;color:DimGray}

            div#titlecontainer{ visibility:visible; width:100%; height: 65px; background-color:RGB(0, 0, 0); color:RGB(255,255,255);}
            div#pagecontainer{visibility:visible; width:580px; margin:10px auto auto 25%; word-break:break-all; float:left;}
            div#toolscontainer{visibility:visible; margin:10px auto auto 10px; float:left;}
            div#link1_1{clear:both; position:fixed; z-index:10; background-color:WhiteSmoke; width:100%; text-decoration: none; bottom:0;}
        </style>
    </head>

    <body bgcolor="rgb(245,245,245)">
        <div id="titlecontainer">
            <div style="font-size:240%; margin:auto auto auto 25%; padding-top:10px; float:left">abelkhan</div>
            <div onclick="clickhomepage(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="font-size:120%; margin:auto auto auto 200px; padding-top:25px;float:left">首页</div>
            <div onclick="clickrandompage(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="font-size:120%; margin:auto auto auto 50px; padding-top:25px;float:left">随便看看</div>
            <div onclick="onexit(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="font-size:120%; margin:auto auto auto 50px; padding-top:25px;float:left">退出</div>
            <input id="search" type="text" value="搜一搜" onfocus="clicksearch(this)" onblur="blursearch(this)" style="color:DimGray;width:200px;height:16px;margin:25px auto auto 50px;" />
        </div>
        <div id="pagecontainer">
            <div style="background-color:rgb(255,255,255); height:136px;">
                <textarea id="postinput" rows="6" cols="70"></textarea>
                <div onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="background-color:rgb(255,255,255); ">
                    <img src="http://abelkhan.com/imageico.png" style="margin:12px auto auto auto; float:left;" />
                    <div style="font-size:80%; margin:12px auto auto 2px; float:left;">图片</div>
                </div>
                <div onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="background-color:rgb(255,255,255); ">
                    <img src="http://abelkhan.com/videoico.png" style="margin:12px auto auto 20px; float:left;" />
                    <div style="font-size:80%; margin:12px auto auto 2px; float:left;">视频</div>
                </div>
                <div onclick="onpost(this)" onmouseout="mouseoutctrl(this)" onmouseover="mouseoverctrl(this)" style="width:100px; height:30px;text-align:center; margin:5px auto 10px 362px; float:left;background-color:DodgerBlue; color:RGB(255,255,255);">
                    <div style="font-size:100%;padding-top:6px;">发布</div>
                </div>
            </div>
"""


userpagehtml0 = """
        </div>
        <div id="toolscontainer">
"""

userpagehtml1 = """
        </div>
        <div id="link1_1">
            <a id="link" class="link1" target="_blank" href="http://www.miitbeian.gov.cn/" style="text-align:center; margin:10px auto auto 46%;">鄂ICP备16002459号</a>
        </div>
    </body>
    <script language="javascript" src="http://abelkhan.com/JSON.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequestError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequest.js"></script>
    <script language="javascript" src="http://abelkhan.com/alertWinMsg.js"></script>
    <script language="javascript" src="http://abelkhan.com/jsquest.js"></script>
    <script>
        function foregroundspm(id){
            var width = 600;
            var iWidth = document.documentElement.clientWidth;
            var iHeight = document.documentElement.clientHeight;
            var div1 =document.createElement("div");
            div1.style.position="fixed";
            div1.style.top = (((iHeight)/2)-200).toString()+"px";
            div1.style.left = (((iWidth - width)/2)-10).toString()+"px";
            div1.style.width = (width+20).toString() + "px";
            div1.style.backgroundColor = "RGB(255,255,255)";
            div1.style.borderStyle = "solid";
            div1.style.borderWidth = "1px";
            div1.style.borderColor = "RGB(250,250,250)";
            var moveX = 0;
            var moveY = 0;
            var moveTop = 0;
            var moveLeft = 0;
            var moveable = false;
            div1.onmouseover = function() {
                div1.style.cursor="move";
            };
            div1.onmouseout = function(){
                div1.style.cursor="auto";
                if (moveable) {
                    moveable = false;
                }
            };
            div1.onmousedown = function() {
                moveable = true;
                moveX = event.clientX;
                moveY = event.clientY;
                moveTop = parseInt(div1.style.top);
                moveLeft = parseInt(div1.style.left);
            };
            div1.onmousemove = function() {
                if (moveable) {
                    var x = moveLeft + event.clientX - moveX;
                    var y = moveTop + event.clientY - moveY;
                    div1.style.left = x + "px";
                    div1.style.top = y + "px";
                }
            };
            div1.onmouseup = function () {
                if (moveable) {
                    moveable = false;
                    moveX = 0;
                    moveY = 0;
                    moveTop = 0;
                    moveLeft = 0;
                }
            };

            var textdiv = document.createElement("div");
            var text = document.createTextNode("查看对话");
            textdiv.appendChild(text);
            textdiv.style.margin="5px 10px 5px 10px";
            textdiv.style.float="left";
            div1.appendChild(textdiv);

            var textdiv1 = document.createElement("div");
            var text1 = document.createTextNode("×");
            textdiv1.appendChild(text1);
            textdiv1.style.margin="5px 10px 5px 10px";
            textdiv1.style.float="right";
            textdiv1.onmouseover = function() {
                textdiv1.style.cursor="pointer";
            };
            textdiv1.onmouseout = function(){
                textdiv1.style.cursor="auto";
            };
            textdiv1.onclick = function(){
                document.body.removeChild(div1);
            }
            div1.appendChild(textdiv1);

            var params = {"userid":userid, "targetname":id.getAttribute("username")};
            JSONRequest.post("http://abelkhan.com/dialogue", params, function (requestNumber, value, exception){
                for (var i = 0; i < value["textlist"].length; i++){
                    var textdiv = document.createElement("div");
                    var text = document.createTextNode(value["textlist"][i]["sender"] + ":" + value["textlist"][i]["text"]);
                    textdiv.appendChild(text);
                    textdiv.style.clear = "both";
                    textdiv.style.paddingTop = "10px";
                    textdiv.style.margin="5px 10px 5px 10px";
                    textdiv.style.borderColor = "RGB(245,245,245)";
                    textdiv.style.borderTopStyle = "solid";
                    textdiv.style.borderWidth = "1px";
                    div1.appendChild(textdiv);
                }
            });

            document.body.appendChild(div1);
        }

        function postpm(id){
            var width = 600;
            var iWidth = document.documentElement.clientWidth;
            var iHeight = document.documentElement.clientHeight;
            var div1 =document.createElement("div");
            div1.style.position="fixed";
            div1.style.top = (((iHeight)/2)-200).toString()+"px";
            div1.style.left = (((iWidth - width)/2)-10).toString()+"px";
            div1.style.width = (width+20).toString() + "px";
            div1.style.backgroundColor = "RGB(255,255,255)";
            div1.style.borderStyle = "solid";
            div1.style.borderWidth = "1px";
            div1.style.borderColor = "RGB(250,250,250)";
            var moveX = 0;
            var moveY = 0;
            var moveTop = 0;
            var moveLeft = 0;
            var moveable = false;
            div1.onmouseover = function() {
                div1.style.cursor="move";
            };
            div1.onmouseout = function(){
                div1.style.cursor="auto";
                if (moveable) {
                    moveable = false;
                }
            };
            div1.onmousedown = function() {
                moveable = true;
                moveX = event.clientX;
                moveY = event.clientY;
                moveTop = parseInt(div1.style.top);
                moveLeft = parseInt(div1.style.left);
            };
            div1.onmousemove = function() {
                if (moveable) {
                    var x = moveLeft + event.clientX - moveX;
                    var y = moveTop + event.clientY - moveY;
                    div1.style.left = x + "px";
                    div1.style.top = y + "px";
                }
            };
            div1.onmouseup = function () {
                if (moveable) {
                    moveable = false;
                    moveX = 0;
                    moveY = 0;
                    moveTop = 0;
                    moveLeft = 0;
                }
            };

            var textdiv = document.createElement("div");
            var text =document.createElement("textarea");
            text.rows="6";
            text.cols="70"
            textdiv.appendChild(text);
            textdiv.style.margin="20px 20px 10px 20px";
            div1.appendChild(textdiv);

            var postbtn = document.createElement("div");
            var textdiv = document.createElement("div");
            var posttext = document.createTextNode("发送");
            textdiv.appendChild(posttext);
            textdiv.style.margin="2px 10px 2px 10px";
            postbtn.style.color = "RGB(255,255,255)";
            postbtn.style.backgroundColor = "DodgerBlue";
            postbtn.appendChild(textdiv);
            postbtn.style.float="right";
            postbtn.style.margin="auto 22px 10px auto";
            postbtn.onmouseover = function() {
                postbtn.style.cursor="pointer";
            };
            postbtn.onmouseout = function(){
                postbtn.style.cursor="auto";
            };
            postbtn.onclick = function(){
                var params = {"userid":userid, "targetname":id.getAttribute("username"), "pm":text.value};
                JSONRequest.post("http://abelkhan.com/post_pm", params, function (requestNumber, value, exception){
                    document.body.removeChild(div1);
                });
            }
            div1.appendChild(postbtn);

            var cancelbtn = document.createElement("div");
            var canceltextdiv = document.createElement("div");
            var canceltext = document.createTextNode("取消");
            canceltextdiv.appendChild(canceltext);
            canceltextdiv.style.margin="2px 10px 2px 10px";
            cancelbtn.style.color = "RGB(25,25,25)";
            cancelbtn.style.backgroundColor = "RGB(245,245,245)";
            cancelbtn.appendChild(canceltextdiv);
            cancelbtn.style.float="right";
            cancelbtn.style.margin="auto 22px 10px auto";
            cancelbtn.onmouseover = function() {
                cancelbtn.style.cursor="pointer";
            };
            cancelbtn.onmouseout = function(){
                cancelbtn.style.cursor="auto";
            };
            cancelbtn.onclick = function(){
                document.body.removeChild(div1);
            }
            div1.appendChild(cancelbtn);

            document.body.appendChild(div1);
        }

        function cmtext(id){
            var params = {"userid":userid};
            JSONRequest.post("http://abelkhan.com/cmtext", params, function (requestNumber, value, exception){
                location.assign(value["url"]);
                window.location=value["url"];
                location.href=value["url"];
            });
        }

        function pmtext(id){
            var params = {"userid":userid};
            JSONRequest.post("http://abelkhan.com/pmtext", params, function (requestNumber, value, exception){
                location.assign(value["url"]);
                window.location=value["url"];
                location.href=value["url"];
            });
        }

        function atme(id){
            var params = {"userid":userid};
            JSONRequest.post("http://abelkhan.com/atme", params, function (requestNumber, value, exception){
                location.assign(value["url"]);
                window.location=value["url"];
                location.href=value["url"];
            });
        }

        function skiptext(id){
            var params = {"userid":userid};
            JSONRequest.post("http://abelkhan.com/textpage", params, function (requestNumber, value, exception){
                location.assign(value["url"]);
                window.location=value["url"];
                location.href=value["url"];
            });
        }

        function clickforward(id){
            obj = id.parentNode.parentNode;
            var textid = id.getAttribute("textid");

            var width = 600;
            var iWidth = document.documentElement.clientWidth;
            var iHeight = document.documentElement.clientHeight;
            var div =document.createElement("div");
            div.style.position="fixed";
            div.style.top = (((iHeight)/2)-200).toString()+"px";
            div.style.left = (((iWidth - width)/2)-10).toString()+"px";
            div.style.width = (width+20).toString() + "px";
            div.style.backgroundColor = "RGB(255,255,255)";
            div.style.borderStyle = "solid";
            div.style.borderWidth = "1px";
            div.style.borderColor = "RGB(250,250,250)";
            var moveX = 0;
            var moveY = 0;
            var moveTop = 0;
            var moveLeft = 0;
            var moveable = false;
            div.onmouseover = function() {
                div.style.cursor="move";
            };
            div.onmouseout = function(){
                div.style.cursor="auto";
                if (moveable) {
                    moveable = false;
                }
            };
            div.onmousedown = function() {
                moveable = true;
                moveX = event.clientX;
                moveY = event.clientY;
                moveTop = parseInt(div.style.top);
                moveLeft = parseInt(div.style.left);
            };
            div.onmousemove = function() {
                if (moveable) {
                    var x = moveLeft + event.clientX - moveX;
                    var y = moveTop + event.clientY - moveY;
                    div.style.left = x + "px";
                    div.style.top = y + "px";
                }
            };
            div.onmouseup = function () {
                if (moveable) {
                    moveable = false;
                    moveX = 0;
                    moveY = 0;
                    moveTop = 0;
                    moveLeft = 0;
                }
            };

            var textdiv = document.createElement("div");
            var text =document.createElement("textarea");
            text.rows="6";
            text.cols="70"
            textdiv.appendChild(text);
            textdiv.style.margin="20px 20px 10px 20px";
            div.appendChild(textdiv);

            var postbtn = document.createElement("div");
            var textdiv = document.createElement("div");
            var posttext = document.createTextNode("转发");
            textdiv.appendChild(posttext);
            textdiv.style.margin="2px 10px 2px 10px";
            postbtn.style.color = "RGB(255,255,255)";
            postbtn.style.backgroundColor = "DodgerBlue";
            postbtn.appendChild(textdiv);
            postbtn.style.float="right";
            postbtn.style.margin="auto 22px 10px auto";
            postbtn.onmouseover = function() {
                postbtn.style.cursor="pointer";
            };
            postbtn.onmouseout = function(){
                postbtn.style.cursor="auto";
            };
            postbtn.onclick = function(){
                var params = {"userid":userid, "textid":textid, "forward":text.value};
                JSONRequest.post("http://abelkhan.com/forward", params, function (requestNumber, value, exception){
                    location.assign(value["url"]);
                    window.location=value["url"];
                    location.href=value["url"];
                });
            }
            div.appendChild(postbtn);

            var cancelbtn = document.createElement("div");
            var canceltextdiv = document.createElement("div");
            var canceltext = document.createTextNode("取消");
            canceltextdiv.appendChild(canceltext);
            canceltextdiv.style.margin="2px 10px 2px 10px";
            cancelbtn.style.color = "RGB(25,25,25)";
            cancelbtn.style.backgroundColor = "RGB(245,245,245)";
            cancelbtn.appendChild(canceltextdiv);
            cancelbtn.style.float="right";
            cancelbtn.style.margin="auto 22px 10px auto";
            cancelbtn.onmouseover = function() {
                cancelbtn.style.cursor="pointer";
            };
            cancelbtn.onmouseout = function(){
                cancelbtn.style.cursor="auto";
            };
            cancelbtn.onclick = function(){
                document.body.removeChild(div);
            }
            div.appendChild(cancelbtn);

            document.body.appendChild(div);
        }

        function clickcomment(id){
            obj = id.parentNode.parentNode;

            if (id.getAttribute("comment") === "true"){
                id.setAttribute("comment", "false");

                var textid = id.getAttribute("textid");
                var params = {"textid":textid};
                JSONRequest.post("http://abelkhan.com/comment", params, function (requestNumber, value, exception){
                    var height = 0;
                    var div = document.createElement("div");
                    var edit = document.createElement("input");
                    edit.type = "text";
                    edit.style.width="480px";
                    edit.style.margin="auto auto 10px 10px;";
                    edit.style.float="left";
                    height+=26;
                    div.appendChild(edit);
                    var btn = document.createElement("div");
                    btn.style.margin="1px auto 10px 10px;"
                    btn.style.backgroundColor = "DodgerBlue";
                    btn.style.width="68px";
                    btn.style.textAlign="center";
                    btn.style.color = "RGB(255,255,255)";
                    var btntext = document.createTextNode("发布");
                    btn.appendChild(btntext);
                    btn.style.float="left";
                    btn.onmouseover = function() {
                        btn.style.cursor="pointer";
                    };
                    btn.onmouseout = function(){
                        btn.style.cursor="auto";
                    };
                    btn.onclick = function(){
                        var params = {"userid":userid, "textid":textid, "comment":edit.value};
                        JSONRequest.post("http://abelkhan.com/post_comment", params, function (requestNumber, value, exception){
                            var text = document.createElement("div");
                            text.style.paddingTop="5px";;
                            text.style.paddingBottom="5px";
                            text.style.margin="auto auto auto 10px;";
                            text.style.clear="both";
                            text.style.borderTopStyle="solid";
                            text.style.borderTopWidth="1px";
                            text.style.borderTopColor="RGB(245,245,245)";
                            text.style.fontSize="90%";
                            var text1 = document.createTextNode(value["nickname"] + ":" + value["text"]);
                            text.appendChild(text1);
                            edit.value="";
                            if (obj.childNodes.length < 10){
                                obj.appendChild(text);
                            }
                            else{
                                obj.insertBefore(text, obj.childNodes[10]);
                            }
                        });
                    };
                    div.appendChild(btn);
                    div.style.height = height.toString() + "px";
                    div.style.backgroundColor = "rgb(255,255,255)";
                    obj.appendChild(div);
                    for (var i = 0; i < value["comment"].length; i++){
                        var text = document.createElement("div");
                        text.style.paddingTop="5px";;
                        text.style.paddingBottom="5px";
                        text.style.margin="auto auto auto 10px;";
                        text.style.clear="both";
                        text.style.borderTopStyle="solid";
                        text.style.borderTopWidth="1px";
                        text.style.borderTopColor="RGB(245,245,245)";
                        text.style.fontSize="90%";
                        var text1 = document.createTextNode(value["comment"][i]["nickname"] + ":" + value["comment"][i]["text"]);
                        text.appendChild(text1);
                        edit.value="";
                        if (obj.childNodes.length < 10){
                            obj.appendChild(text);
                        }
                        else{
                            obj.insertBefore(text, obj.childNodes[10]);
                        }
                    }
                });
            }
            else if (id.getAttribute("comment") === "false"){
                id.setAttribute("comment", "true");

                childs = obj.childNodes;
                for(var i = 9; i < childs.length;){
                    obj.removeChild(childs[i]);
                }
            }
        }

        function deltext(id){
            var params = {"userid":userid, "textid":id.getAttribute("textid")};
            JSONRequest.post("http://abelkhan.com/deltext", params, function (requestNumber, value, exception){
                if (value["sucess"] === true){
                    delobj = id.parentNode.parentNode;
                    p = delobj.parentNode;
                    p.removeChild(delobj);
                }
            });
        }

        function onpost(id){
            if (document.getElementById("postinput").value !== "")
            {
                var params = {"userid":userid, "text":document.getElementById("postinput").value};
                JSONRequest.post("http://abelkhan.com/post", params, function (requestNumber, value, exception){
                    location.assign(value["url"]);
                    window.location=value["url"];
                    location.href=value["url"];
                });
            }
        }

        function clickhomepage(id){
            var params = {"userid":userid};
            JSONRequest.post("http://abelkhan.com/homepage", params, function (requestNumber, value, exception){
                location.assign(value["url"]);
                window.location=value["url"];
                location.href=value["url"];
            });
        }

        function clickrandompage(id){
            var params = {"userid":userid};
            JSONRequest.post("http://abelkhan.com/randompage", params, function (requestNumber, value, exception){
                location.assign(value["url"]);
                window.location=value["url"];
                location.href=value["url"];
            });
        }

        function onexit(id){
            var params = {"userid":userid};
            JSONRequest.post("http://abelkhan.com/exit", params, function (requestNumber, value, exception){
                location.assign(value["url"]);
                window.location=value["url"];
                location.href=value["url"];
            });
        }

        function mouseouttools(id)
        {
            id.style.cursor="auto";
            id.style.color="rgb(0,0,0)";
        }

        function mouseovertools(id)
        {
            id.style.cursor="pointer";
            id.style.color="LightCoral";
        }

        function mouseoutotheruser(id)
        {
            id.style.cursor="auto";
        }

        function userpanel(id, x, y, value)
        {
            if (value["notself"]){
                var top = (y - 10);
                var left = (x - 100);

                var width = 300;
                var height = 120;
                var div =document.createElement("div");
                div.style.position="absolute";
                div.style.top = top.toString()+"px";
                div.style.left = left.toString()+"px";
                div.style.width = width.toString() + "px";
                div.style.height = height.toString() + "px";
                div.style.backgroundColor = "RGB(255,255,255)";
                div.style.borderStyle = "solid";
                div.style.borderWidth = "1px";
                div.style.borderColor = "RGB(250,250,250)";

                var textdiv = document.createElement("div");
                var text =document.createTextNode(id.getAttribute("username"));
                textdiv.appendChild(text);
                textdiv.style.textAlign = "center";
                textdiv.style.margin="20px 20px auto 20px";
                textdiv.style.paddingBottom = "20px";
                textdiv.style.borderBottomStyle = "solid";
                textdiv.style.borderWidth = "1px";
                textdiv.style.borderColor = "RGB(250,250,250)";
                div.appendChild(textdiv);

                var btndiv = document.createElement("div");
                btndiv.onmouseover = function(){
                    btndiv.style.cursor="pointer";
                }
                btndiv.onmouseout = function(){
                    btndiv.style.cursor="auto";
                }
                btndiv.onclick = function(){
                    var params = {"userid":userid, "otherusername":id.getAttribute("username")};
                    JSONRequest.post("http://abelkhan.com/concern", params, function (requestNumber, value, exception){
                        if (value["isfollow"])
                        {
                            btndiv.innerHTML = "取消关注"
                        }
                        else
                        {
                            btndiv.innerHTML = "加关注"
                        }
                    });
                }
                var btntextdiv = document.createElement("div");
                if (value["isfollow"]){
                    var btntext =document.createTextNode("取消关注");
                }
                else{
                    var btntext =document.createTextNode("加关注");
                }
                btntextdiv.appendChild(btntext);
                btntextdiv.style.margin="3px 15px 3px 15px";
                btndiv.appendChild(btntextdiv);
                btndiv.style.float="left";
                btndiv.style.borderStyle = "solid";
                btndiv.style.borderWidth = "1px";
                btndiv.style.borderColor = "RGB(250,250,250)";
                btndiv.style.margin="20px 10px auto 60px";
                div.appendChild(btndiv);

                var btndiv1 = document.createElement("div");
                btndiv1.onmouseover = function(){
                    btndiv1.style.cursor="pointer";
                }
                btndiv1.onmouseout = function(){
                    btndiv1.style.cursor="auto";
                }
                btndiv1.onclick = function(){
                    document.body.removeChild(div);

                    var width = 600;
                    var iWidth = document.documentElement.clientWidth;
                    var iHeight = document.documentElement.clientHeight;
                    var div1 =document.createElement("div");
                    div1.style.position="fixed";
                    div1.style.top = (((iHeight)/2)-200).toString()+"px";
                    div1.style.left = (((iWidth - width)/2)-10).toString()+"px";
                    div1.style.width = (width+20).toString() + "px";
                    div1.style.backgroundColor = "RGB(255,255,255)";
                    div1.style.borderStyle = "solid";
                    div1.style.borderWidth = "1px";
                    div1.style.borderColor = "RGB(250,250,250)";
                    var moveX = 0;
                    var moveY = 0;
                    var moveTop = 0;
                    var moveLeft = 0;
                    var moveable = false;
                    div1.onmouseover = function() {
                        div1.style.cursor="move";
                    };
                    div1.onmouseout = function(){
                        div1.style.cursor="auto";
                        if (moveable) {
                            moveable = false;
                        }
                    };
                    div1.onmousedown = function() {
                        moveable = true;
                        moveX = event.clientX;
                        moveY = event.clientY;
                        moveTop = parseInt(div1.style.top);
                        moveLeft = parseInt(div1.style.left);
                    };
                    div1.onmousemove = function() {
                        if (moveable) {
                            var x = moveLeft + event.clientX - moveX;
                            var y = moveTop + event.clientY - moveY;
                            div1.style.left = x + "px";
                            div1.style.top = y + "px";
                        }
                    };
                    div1.onmouseup = function () {
                        if (moveable) {
                            moveable = false;
                            moveX = 0;
                            moveY = 0;
                            moveTop = 0;
                            moveLeft = 0;
                        }
                    };

                    var textdiv = document.createElement("div");
                    var text =document.createElement("textarea");
                    text.rows="6";
                    text.cols="70"
                    textdiv.appendChild(text);
                    textdiv.style.margin="20px 20px 10px 20px";
                    div1.appendChild(textdiv);

                    var postbtn = document.createElement("div");
                    var textdiv = document.createElement("div");
                    var posttext = document.createTextNode("发送");
                    textdiv.appendChild(posttext);
                    textdiv.style.margin="2px 10px 2px 10px";
                    postbtn.style.color = "RGB(255,255,255)";
                    postbtn.style.backgroundColor = "DodgerBlue";
                    postbtn.appendChild(textdiv);
                    postbtn.style.float="right";
                    postbtn.style.margin="auto 22px 10px auto";
                    postbtn.onmouseover = function() {
                        postbtn.style.cursor="pointer";
                    };
                    postbtn.onmouseout = function(){
                        postbtn.style.cursor="auto";
                    };
                    postbtn.onclick = function(){
                        var params = {"userid":userid, "targetname":id.getAttribute("username"), "pm":text.value};
                        JSONRequest.post("http://abelkhan.com/post_pm", params, function (requestNumber, value, exception){
                            document.body.removeChild(div1);
                        });
                    }
                    div1.appendChild(postbtn);

                    var cancelbtn = document.createElement("div");
                    var canceltextdiv = document.createElement("div");
                    var canceltext = document.createTextNode("取消");
                    canceltextdiv.appendChild(canceltext);
                    canceltextdiv.style.margin="2px 10px 2px 10px";
                    cancelbtn.style.color = "RGB(25,25,25)";
                    cancelbtn.style.backgroundColor = "RGB(245,245,245)";
                    cancelbtn.appendChild(canceltextdiv);
                    cancelbtn.style.float="right";
                    cancelbtn.style.margin="auto 22px 10px auto";
                    cancelbtn.onmouseover = function() {
                        cancelbtn.style.cursor="pointer";
                    };
                    cancelbtn.onmouseout = function(){
                        cancelbtn.style.cursor="auto";
                    };
                    cancelbtn.onclick = function(){
                        document.body.removeChild(div1);
                    }
                    div1.appendChild(cancelbtn);

                    document.body.appendChild(div1);
                }
                var btntextdiv = document.createElement("div");
                var btntext =document.createTextNode("私信");
                btntextdiv.appendChild(btntext);
                btntextdiv.style.margin="3px 15px 3px 15px";
                btndiv1.appendChild(btntextdiv);
                btndiv1.style.float="left";
                btndiv1.style.borderStyle = "solid";
                btndiv1.style.borderWidth = "1px";
                btndiv1.style.borderColor = "RGB(250,250,250)";
                btndiv1.style.margin="20px 10px auto 40px";
                div.appendChild(btndiv1);

                div.onmouseover = function(){
                    div.style.cursor="auto";
                }
                div.onmouseout = function(){
                    var x1 = event.pageX;
                    var y1 = event.pageY;

                    if ( (x1 < left) || (x1 > (left + width)) || (y1 < top) || (y1 > (top + height)) ){
                        document.body.removeChild(div);
                    }
                }

                document.body.appendChild(div);
            }
        }

        function mouseoverotheruser(id)
        {
            id.style.cursor="pointer";

            var x = event.pageX;
            var y = event.pageY;

            var params = {"userid":userid, "otherusername":id.getAttribute("username")};
            JSONRequest.post("http://abelkhan.com/userpanel", params, function (requestNumber, value, exception){
                userpanel(id, x, y, value);
            });
        }

        function mouseoutuser(id)
        {
            id.style.cursor="auto";
            id.style.color="rgb(0,0,0)";
        }

        function mouseoveruser(id)
        {
            id.style.cursor="pointer";
            id.style.color="LightCoral";

            var x = event.pageX;
            var y = event.pageY;

            var params = {"userid":userid, "otherusername":id.getAttribute("username")};
            JSONRequest.post("http://abelkhan.com/userpanel", params, function (requestNumber, value, exception){
                userpanel(id, x, y, value);
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
        
        function clicksearch(id)
        {
            if (id.value==="搜一搜")
            {
                id.value=""
            }
        }

        function blursearch(id)
        {
            if (id.value==="")
            {
                id.value="搜一搜";
            }
        }
        
        """