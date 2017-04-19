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
        <script>
            var loginurl = "http://abelkhan.com/login";

            function getCookie(c_name)
            {
                if (document.cookie.length>0)
                {
                    c_start=document.cookie.indexOf(c_name + "=")
                    if (c_start!=-1)
                    {
                        c_start=c_start + c_name.length+1;
                        c_end=document.cookie.indexOf(";",c_start)
                        if (c_end==-1)
                        {
                            c_end=document.cookie.length
                        }
                        return unescape(document.cookie.substring(c_start,c_end))
                    }
                }
                return ""
            }

            function checkCookie()
            {
                usermail=getCookie("usermail")
                if (usermail!==null && usermail!=="")
                {
                    JSONRequest.post("http://abelkhan.com/post_check_login", {"usermail":usermail}, function (requestNumber, value, exception){
                        location.assign(value["url"]);
                        window.location=value["url"];
                        location.href=value["url"];
                    });
                }
                else
                {
                    location.assign(loginurl);
                    window.location=loginurl;
                    location.href=loginurl;
                }
            }

        </script>

        <title>abelkhan - 图片记录生活</title>
        <meta name="keywords" content="abelkhan" />
        <meta name="description" content="abelkhan - 图片记录生活" />
        <style type="text/css">
            a:link.link1 { text-decoration: none;color:DimGray}
            a:hover.link1 { text-decoration:none;color: DimGray}
            a:visited.link1 { text-decoration: none;color:DimGray}

            div#titlelogincontainer{ visibility:visible; margin:1px auto auto 1px; }
            div#link1_1{clear:both; position:fixed; z-index:10; background-color:WhiteSmoke; width:100%; text-decoration: none; bottom:0;}
        </style>
    </head>

    <body onLoad="checkCookie()">
        <div id="titlelogincontainer">
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
    <script>

        
        """