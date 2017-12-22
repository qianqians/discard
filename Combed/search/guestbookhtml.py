# -*- coding: UTF-8 -*-
# htmlprocess
# create at 2016/3/13
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
        <title>留言板</title>
        <style type="text/css">
            div#container{border-style:solid; border-width: 1px;}
            div#title{font-size:200%; margin:20px auto 10px auto; border-bottom-style:solid; border-width: 1px;}
            div#nodes{font-size:100%; margin:10px auto 10px auto; border-bottom-style:solid; border-width: 1px;}
            div#post{border-top-style:solid; border-width: 1px; clear:both}
        </style>
    </head>
    <body>
        <div id="container" >
            <div id="title"> 留言板</div>
            <div id="nodes">这个搜索引擎是目前是作者个人维护，欢迎大家在这里提出改进意见!</div>
"""

searchhtml0 = """
            <div id="post">
                <textarea id="input" rows="12" cols="60" style="margin:10px auto 5px 5px;"></textarea>
                <br />
                &nbsp;用户名:<input id="username" type="text" style="margin:5px auto 10px 5px;">
                <br />
                <input type="button" value="提交" onclick="postinput(this)" style="margin:5px auto 10px 5px;">
            </div>
        </div>
    </body>
    <script language="javascript" src="http://abelkhan.com/JSON.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequestError.js"></script>
    <script language="javascript" src="http://abelkhan.com/JSONRequest.js"></script>
    <script>
    function postinput(id){
        input = document.getElementById("input").value;
        username = document.getElementById("username").value;
        if (input != ""){
            var params = {"input":input, "username":username};
            JSONRequest.post("http://abelkhan.com/postinput", params, function (requestNumber, value, exception){
                url = value["url"];
                location.assign(url);
                window.location=url;
                location.href=url;
            });
        }
    }
    </script>
</html>
"""