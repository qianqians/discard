var popobj = null;
var poptable = null;
var username = null;
function alertWinpopreg(width, value){
    var bgcolor = "RGB(220, 220, 220)";
    var iWidth = document.documentElement.clientWidth;
    var iHeight = document.documentElement.clientHeight;
    var msgObj=document.createElement("div");
    msgObj.style.position="absolute";
    msgObj.style.left = (((iWidth - width)/2)-10).toString()+"px";
    msgObj.style.width = (width+20).toString() + "px";
    msgObj.style.borderStyle = "solid";
    msgObj.style.borderWidth = "1px";
    msgObj.style.borderColor = bgcolor;
    msgObj.style.backgroundColor = bgcolor;
    msgObj.style.filter = "alpha(opacity=50)";
    msgObj.style.opacity="0.5";
    var moveX = 0;
    var moveY = 0;
    var moveTop = 0;
    var moveLeft = 0;
    var moveable = false;
    msgObj.onmouseover = function() {
        msgObj.style.cursor="move";
    };
    msgObj.onmouseout = function(){
        msgObj.style.cursor="auto";
        if (moveable) {
            moveable = false;
        }
    };
    msgObj.onmousedown = function() {
        moveable = true;
        moveX = event.clientX;
        moveY = event.clientY;
        moveTop = parseInt(msgObj.style.top);
        moveLeft = parseInt(msgObj.style.left);
    };
    var table = document.createElement("div");
    msgObj.onmousemove = function() {
        if (moveable) {
            var x = moveLeft + event.clientX - moveX;
            var y = moveTop + event.clientY - moveY;
            msgObj.style.left = x + "px";
            msgObj.style.top = y + "px";
            table.style.left = (x+10) + "px";
            table.style.top = (y+10) + "px";
        }
    };
    msgObj.onmouseup = function () {
        if (moveable) {
            moveable = false;
            moveX = 0;
            moveY = 0;
            moveTop = 0;
            moveLeft = 0;
        }
    };
    table.onmouseover = function() {
        table.style.cursor="move";
    };
    table.onmouseout = function(){
        table.style.cursor="auto";
        if (moveable) {
            moveable = false;
        }
    };
    table.onmousedown = function() {
        moveable = true;
        moveX = event.clientX;
        moveY = event.clientY;
        moveTop = parseInt(msgObj.style.top);
        moveLeft = parseInt(msgObj.style.left);
    };
    table.onmousemove = function() {
        if (moveable) {
            var x = moveLeft + event.clientX - moveX;
            var y = moveTop + event.clientY - moveY;
            msgObj.style.left = x + "px";
            msgObj.style.top = y + "px";
            table.style.left = (x+10) + "px";
            table.style.top = (y+10) + "px";
        }
    };
    table.onmouseup = function () {
        if (moveable) {
            moveable = false;
            moveX = 0;
            moveY = 0;
            moveTop = 0;
            moveLeft = 0;
        }
    };
    table.style.borderBottomStyle = "solid";
    table.style.borderBottomWidth = "1px";
    table.style.borderBottomColor = "RGB(255, 255, 255)";
    table.style.borderLeftStyle = "solid";
    table.style.borderLeftWidth = "1px";
    table.style.borderLeftColor = "RGB(255, 255, 255)";
    table.style.borderRightStyle = "solid";
    table.style.borderRightWidth = "1px";
    table.style.borderRightColor = "RGB(255, 255, 255)";
    table.style.borderTopStyle = "solid";
    table.style.borderTopWidth = "1px";
    table.style.borderTopColor = "RGB(255, 255, 255)";
    table.style.backgroundColor = "RGB(255, 255, 255)";
    table.style.position="absolute";
    table.style.zIndex="1";
    table.style.left = ((iWidth - width)/2).toString()+"px";
    table.style.width = width.toString() + "px";
    var table_pop = document.createElement("div");
    table_pop.style.width = width.toString() + "px";
    var table_userregister = document.createElement("div");
    var texttable_userregister = document.createTextNode("用户注册");
    table_userregister.appendChild(texttable_userregister);
    table_userregister.style.margin="5px auto 5px 5px";
    table_userregister.style.float = "left";
    table_pop.appendChild(table_userregister);
    var table_closeui = document.createElement("div");
    var texttable_closeui = document.createTextNode("×");
    table_closeui.appendChild(texttable_closeui);
    table_closeui.style.margin="5px 5px auto auto";
    table_closeui.style.float = "right";
    table_closeui.onmouseover=function() {
        table_closeui.style.cursor="pointer";
    };
    table_closeui.onmouseout=function() {
        table_closeui.style.cursor="auto";
    };
    table_closeui.onclick=function() {
        document.body.removeChild(msgObj);
        document.body.removeChild(table);
        poptable = null;
        popobj = null;
    };
    table_pop.appendChild(table_closeui);
    var table_regusernamec = document.createElement("div");
    table_regusernamec.style.clear = "both";
    table_regusernamec.style.borderWidth = "1px";
    table_regusernamec.style.borderColor = "rgb(150,150,150)";
    table_regusernamec.style.borderBottomStyle="solid";
    var table_regusername = document.createElement("div");
    var texttable_regusername = document.createTextNode("用户名:");
    table_regusername.appendChild(texttable_regusername);
    table_regusername.style.margin="12px auto auto 50px";
    table_regusername.style.float = "left";
    table_regusername.style.clear = "both";
    table_regusernamec.appendChild(table_regusername);
    var table_regusernameinput = document.createElement("input");
    table_regusernameinput.type = "text";
    table_regusernameinput.style.margin="10px auto 10px auto";
    table_regusernameinput.style.float = "left";
    table_regusernamec.appendChild(table_regusernameinput);
    table_pop.appendChild(table_regusernamec);
    var table_regusernamekey = document.createElement("div");
    table_regusernamekey.style.clear = "both";
    table_regusernamekey.style.borderWidth = "1px";
    table_regusernamekey.style.borderColor = "rgb(150,150,150)";
    table_regusernamekey.style.borderTopStyle="dashed";
    var table_reguserkey = document.createElement("div");
    var texttable_reguserkey = document.createTextNode("密码:");
    table_reguserkey.appendChild(texttable_reguserkey);
    table_reguserkey.style.margin="12px auto auto 66px";
    table_reguserkey.style.float = "left";
    table_reguserkey.style.clear = "both";
    table_regusernamekey.appendChild(table_reguserkey);
    var table_reguserkeyinput = document.createElement("input");
    table_reguserkeyinput.type = "password";
    table_reguserkeyinput.style.margin="10px auto 10px auto";
    table_reguserkeyinput.style.float = "left";
    table_regusernamekey.appendChild(table_reguserkeyinput);
    table_pop.appendChild(table_regusernamekey);
    var table_reusernamekey = document.createElement("div");
    table_reusernamekey.style.clear = "both";
    table_reusernamekey.style.borderWidth = "1px";
    table_reusernamekey.style.borderColor = "rgb(150,150,150)";
    table_reusernamekey.style.borderTopStyle="dashed";
    var table_reuserkey = document.createElement("div");
    var texttable_reuserkey = document.createTextNode("确认密码:");
    table_reuserkey.appendChild(texttable_reuserkey);
    table_reuserkey.style.margin="12px auto auto 34px";
    table_reuserkey.style.float = "left";
    table_reuserkey.style.clear = "both";
    table_reusernamekey.appendChild(table_reuserkey);
    var table_reuserkeyinput = document.createElement("input");
    table_reuserkeyinput.type = "password";
    table_reuserkeyinput.style.margin="10px auto 10px auto";
    table_reuserkeyinput.style.float = "left";
    table_reusernamekey.appendChild(table_reuserkeyinput);
    table_pop.appendChild(table_reusernamekey);
    var table_remail = document.createElement("div");
    table_remail.style.clear = "both";
    table_remail.style.borderWidth = "1px";
    table_remail.style.borderColor = "rgb(150,150,150)";
    table_remail.style.borderTopStyle="dashed";
    var table_mail = document.createElement("div");
    var texttable_mail = document.createTextNode("邮箱:");
    table_mail.appendChild(texttable_mail);
    table_mail.style.margin="12px auto auto 66px";
    table_mail.style.float = "left";
    table_mail.style.clear = "both";
    table_remail.appendChild(table_mail);
    var table_mailinput = document.createElement("input");
    table_mailinput.type = "text";
    table_mailinput.style.margin="10px auto 10px auto";
    table_mailinput.style.float = "left";
    table_remail.appendChild(table_mailinput);
    table_pop.appendChild(table_remail);
    var table_regcaptchac = document.createElement("div");
    table_regcaptchac.style.clear = "both";
    table_regcaptchac.style.borderWidth = "1px";
    table_regcaptchac.style.borderColor = "rgb(150,150,150)";
    table_regcaptchac.style.borderTopStyle="dashed";
    var table_regcaptcha = document.createElement("div");
    var texttable_regcaptcha = document.createTextNode("验证码:");
    table_regcaptcha.appendChild(texttable_regcaptcha);
    table_regcaptcha.style.margin="12px auto auto 50px";
    table_regcaptcha.style.float = "left";
    table_regcaptcha.style.clear = "both";
    table_regcaptchac.appendChild(table_regcaptcha);
    var table_regcaptchainput = document.createElement("input");
    table_regcaptchainput.type = "text";
    table_regcaptchainput.style.margin="10px auto auto auto";
    table_regcaptchainput.style.float = "left";
    table_regcaptchac.appendChild(table_regcaptchainput);
    var table_regchange = document.createElement("div");
    var texttable_regchange = document.createTextNode("换一个");
    table_regchange.appendChild(texttable_regchange);
    table_regchange.style.margin="14px auto 5px 5px";
    table_regchange.style.float = "left";
    table_regchange.style.fontSize="80%";
    table_regchange.style.color="rgb(100,100,200)";
    table_regchange.onmouseout=function() {
        table_regchange.style.cursor="auto";
        table_regchange.style.textDecoration="none";
    };
    table_regchange.onclick=function() {
        var params = {"sid":sid};
        JSONRequest.post("http://abelkhan.com/changecheck", params,
            function (requestNumber, value, exception){
                table_regcheck.innerHTML = value["check"];
            });
    };
    table_regchange.onmouseover=function() {
        table_regchange.style.cursor="pointer";
        table_regchange.style.textDecoration="underline";
    };
    table_regcaptchac.appendChild(table_regchange);
    var table_regnotes = document.createElement("div");
    var texttable_regnotes = document.createTextNode("请输入下面繁体数字的阿拉伯字符");
    table_regnotes.appendChild(texttable_regnotes);
    table_regnotes.style.margin="auto auto auto 108px";
    table_regnotes.style.float = "left";
    table_regnotes.style.fontSize="60%";
    table_regnotes.style.color="rgb(100,100,100)";
    table_regcaptchac.appendChild(table_regnotes);
    var table_regcheck = document.createElement("div");
    var texttable_regcheck = document.createTextNode(value["check"]);
    table_regcheck.appendChild(texttable_regcheck);
    table_regcheck.style.margin="5px auto 10px 108px";
    table_regcheck.style.float = "left";
    table_regcheck.style.fontSize="160%";
    table_regcheck.style.color="rgb(100,100,200)";
    table_regcaptchac.appendChild(table_regcheck);
    table_pop.appendChild(table_regcaptchac);
    var table_reusernamelogin = document.createElement("div");
    table_reusernamelogin.style.clear = "both";
    table_reusernamelogin.style.borderWidth = "1px";
    table_reusernamelogin.style.borderColor = "rgb(150,150,150)";
    table_reusernamelogin.style.borderTopStyle="dashed";
    var table_reloginusername = document.createElement("input");
    table_reloginusername.type = "button";
    table_reloginusername.value="注册";
    table_reloginusername.style.margin="10px auto 5px 160px";
    table_reloginusername.onclick=function() {
        var params = {"sid":sid};
        params["checktext"]= table_regcaptchainput.value;
        params["name"]= table_regusernameinput.value;
        params["key"]= table_reuserkeyinput.value;
        params["mail"]= table_mailinput.value;
        JSONRequest.post("http://abelkhan.com/registeruser", params,
            function (requestNumber, value, exception){
                if (value["registerend"]){
                    alertWinMsg("注册成功");
                
                    document.body.removeChild(msgObj);
                    document.body.removeChild(table);
                    poptable = null;
                    popobj = null;
                }
                if (value["mailberegister"] === false){
                    alertWinMsg("邮箱已被注册");
                }
                if (value["mail"] === false){
                    alertWinMsg("邮箱格式错误");
                }
                if (value["namelen"] === false){
                    alertWinMsg("用户名非法");
                }
                if (value["keylen"] === false){
                    alertWinMsg("密码过短");
                }
                if (value["checkend"] === false){
                    alertWinMsg("校验码错");
                }
                if (value["registerend"] === false){
                    alertWinMsg("用户已被注册");
                }
            });
    };
    table_reusernamelogin.appendChild(table_reloginusername);
    table_pop.appendChild(table_reusernamelogin);
    table.appendChild(table_pop);
    document.body.appendChild(table);
    msgObj.style.height = (table.offsetHeight +20).toString() + "px";
    table.style.top = ((iHeight-table.offsetHeight)/2).toString()+"px";
    msgObj.style.top = (((iHeight-table.offsetHeight)/2)-10).toString()+"px";
    document.body.appendChild(msgObj);
    popobj = msgObj;
    poptable = table;
}

function alertWinpopLogin(width, value){
    var bgcolor = "RGB(220, 220, 220)";
    var iWidth = document.documentElement.clientWidth;
    var iHeight = document.documentElement.clientHeight;
    var msgObj=document.createElement("div");
    msgObj.style.position="absolute";
    msgObj.style.left = (((iWidth - width)/2)-10).toString()+"px";
    msgObj.style.width = (width+20).toString() + "px";
    msgObj.style.borderStyle = "solid";
    msgObj.style.borderWidth = "1px";
    msgObj.style.borderColor = bgcolor;
    msgObj.style.backgroundColor = bgcolor;
    msgObj.style.filter = "alpha(opacity=50)";
    msgObj.style.opacity="0.5";
    var moveX = 0;
    var moveY = 0;
    var moveTop = 0;
    var moveLeft = 0;
    var moveable = false;
    msgObj.onmouseover = function() {
        msgObj.style.cursor="move";
    };
    msgObj.onmouseout = function(){
        msgObj.style.cursor="auto";
        if (moveable) {
            moveable = false;
        }
    };
    msgObj.onmousedown = function() {
        moveable = true;
        moveX = event.clientX;
        moveY = event.clientY;
        moveTop = parseInt(msgObj.style.top);
        moveLeft = parseInt(msgObj.style.left);
    };
    var table = document.createElement("div");
    msgObj.onmousemove = function() {
        if (moveable) {
            var x = moveLeft + event.clientX - moveX;
            var y = moveTop + event.clientY - moveY;
            msgObj.style.left = x + "px";
            msgObj.style.top = y + "px";
            table.style.left = (x+10) + "px";
            table.style.top = (y+10) + "px";
        }
    };
    msgObj.onmouseup = function () {
        if (moveable) {
            moveable = false;
            moveX = 0;
            moveY = 0;
            moveTop = 0;
            moveLeft = 0;
        }
    };
    table.onmouseover = function() {
        table.style.cursor="move";
    };
    table.onmouseout = function(){
        table.style.cursor="auto";
        if (moveable) {
            moveable = false;
        }
    };
    table.onmousedown = function() {
        moveable = true;
        moveX = event.clientX;
        moveY = event.clientY;
        moveTop = parseInt(msgObj.style.top);
        moveLeft = parseInt(msgObj.style.left);
    };
    table.onmousemove = function() {
        if (moveable) {
            var x = moveLeft + event.clientX - moveX;
            var y = moveTop + event.clientY - moveY;
            msgObj.style.left = x + "px";
            msgObj.style.top = y + "px";
            table.style.left = (x+10) + "px";
            table.style.top = (y+10) + "px";
        }
    };
    table.onmouseup = function () {
        if (moveable) {
            moveable = false;
            moveX = 0;
            moveY = 0;
            moveTop = 0;
            moveLeft = 0;
        }
    };
    table.style.borderBottomStyle = "solid";
    table.style.borderBottomWidth = "1px";
    table.style.borderBottomColor = "RGB(255, 255, 255)";
    table.style.borderLeftStyle = "solid";
    table.style.borderLeftWidth = "1px";
    table.style.borderLeftColor = "RGB(255, 255, 255)";
    table.style.borderRightStyle = "solid";
    table.style.borderRightWidth = "1px";
    table.style.borderRightColor = "RGB(255, 255, 255)";
    table.style.borderTopStyle = "solid";
    table.style.borderTopWidth = "1px";
    table.style.borderTopColor = "RGB(255, 255, 255)";
    table.style.backgroundColor = "RGB(255, 255, 255)";
    table.style.position="absolute";
    table.style.zIndex="1";
    table.style.left = ((iWidth - width)/2).toString()+"px";
    table.style.width = width.toString() + "px";
    var table_pop = document.createElement("div");
    table_pop.style.width = width.toString() + "px";
    var table_userlogin = document.createElement("div");
    var texttable_userlogin = document.createTextNode("用户登陆");
    table_userlogin.appendChild(texttable_userlogin);
    table_userlogin.style.margin="5px auto 5px 5px";
    table_userlogin.style.float = "left";
    table_pop.appendChild(table_userlogin);
    var table_close = document.createElement("div");
    var texttable_close = document.createTextNode("×");
    table_close.appendChild(texttable_close);
    table_close.style.margin="5px 5px auto auto";
    table_close.style.float = "right";
    table_close.onmouseover=function() {
        table_close.style.cursor="pointer";
    };
    table_close.onmouseout=function() {
        table_close.style.cursor="auto";
    };
    table_close.onclick=function() {
        document.body.removeChild(msgObj);
        document.body.removeChild(table);
        poptable = null;
        popobj = null;
    };
    table_pop.appendChild(table_close);
    var table_usernamec = document.createElement("div");
    table_usernamec.style.clear = "both";
    table_usernamec.style.borderWidth = "1px";
    table_usernamec.style.borderColor = "rgb(150,150,150)";
    table_usernamec.style.borderBottomStyle="solid";
    var table_username = document.createElement("div");
    var texttable_username = document.createTextNode("用户名:");
    table_username.appendChild(texttable_username);
    table_username.style.margin="12px auto auto 50px";
    table_username.style.float = "left";
    table_username.style.clear = "both";
    table_usernamec.appendChild(table_username);
    var table_usernameinput = document.createElement("input");
    table_usernameinput.type = "text";
    table_usernameinput.style.margin="10px auto auto auto";
    table_usernameinput.style.float = "left";
    table_usernamec.appendChild(table_usernameinput);
    var table_usernameregister = document.createElement("div");
    var texttable_usernameregister = document.createTextNode("注册");
    table_usernameregister.appendChild(texttable_usernameregister);
    table_usernameregister.style.margin="14px auto 10px 5px";
    table_usernameregister.style.float = "left";
    table_usernameregister.style.fontSize="80%";
    table_usernameregister.style.color="rgb(100,100,200)";
    table_usernameregister.onmouseover=function() {
        table_usernameregister.style.cursor="pointer";
        table_usernameregister.style.textDecoration="underline";
    };
    table_usernameregister.onmouseout=function() {
        table_usernameregister.style.cursor="auto";
        table_usernameregister.style.textDecoration="none";
    };
    table_usernamec.appendChild(table_usernameregister);
    table_pop.appendChild(table_usernamec);
    var table_usernamekey = document.createElement("div");
    table_usernamekey.style.clear = "both";
    table_usernamekey.style.borderWidth = "1px";
    table_usernamekey.style.borderColor = "rgb(150,150,150)";
    table_usernamekey.style.borderTopStyle="dashed";
    var table_userkey = document.createElement("div");
    var texttable_userkey = document.createTextNode("密码:");
    table_userkey.appendChild(texttable_userkey);
    table_userkey.style.margin="12px auto auto 66px";
    table_userkey.style.float = "left";
    table_userkey.style.clear = "both";
    table_usernamekey.appendChild(table_userkey);
    var table_userkeyinput = document.createElement("input");
    table_userkeyinput.type = "password";
    table_userkeyinput.style.margin="10px auto auto auto";
    table_userkeyinput.style.float = "left";
    table_usernamekey.appendChild(table_userkeyinput);
    var table_findusername = document.createElement("div");
    var texttable_findusername = document.createTextNode("找回密码");
    table_findusername.appendChild(texttable_findusername);
    table_findusername.style.margin="14px auto 10px 5px";
    table_findusername.style.float = "left";
    table_findusername.style.fontSize="80%";
    table_findusername.style.color="rgb(100,100,200)";
    table_findusername.onmouseover=function() {
        table_findusername.style.textDecoration="underline";
        table_findusername.style.cursor="pointer";
    };
    table_findusername.onmouseout=function() {
        table_findusername.style.cursor="auto";
        table_findusername.style.textDecoration="none";
    };
    table_usernamekey.appendChild(table_findusername);
    table_pop.appendChild(table_usernamekey);
    var table_captchac = document.createElement("div");
    table_captchac.style.clear = "both";
    table_captchac.style.borderWidth = "1px";
    table_captchac.style.borderColor = "rgb(150,150,150)";
    table_captchac.style.borderTopStyle="dashed";
    var table_captcha = document.createElement("div");
    var texttable_captcha = document.createTextNode("验证码:");
    table_captcha.appendChild(texttable_captcha);
    table_captcha.style.margin="12px auto auto 50px";
    table_captcha.style.float = "left";
    table_captcha.style.clear = "both";
    table_captchac.appendChild(table_captcha);
    var table_captchainput = document.createElement("input");
    table_captchainput.type = "text";
    table_captchainput.style.margin="10px auto auto auto";
    table_captchainput.style.float = "left";
    table_captchac.appendChild(table_captchainput);
    var table_change = document.createElement("div");
    var texttable_change = document.createTextNode("换一个");
    table_change.appendChild(texttable_change);
    table_change.style.margin="14px auto 5px 5px";
    table_change.style.float = "left";
    table_change.style.fontSize="80%";
    table_change.style.color="rgb(100,100,200)";
    table_change.onmouseout=function() {
        table_change.style.cursor="auto";
        table_change.style.textDecoration="none";
    };
    table_change.onclick=function() {
        var params = {"sid":sid};
        JSONRequest.post("http://abelkhan.com/changecheck", params,
            function (requestNumber, value, exception){
                table_check.innerHTML = value["check"];
            });
    };
    table_change.onmouseover=function() {
        table_change.style.cursor="pointer";
        table_change.style.textDecoration="underline";
    };
    table_captchac.appendChild(table_change);
    var table_notes = document.createElement("div");
    var texttable_notes = document.createTextNode("请输入下面繁体数字的阿拉伯字符");
    table_notes.appendChild(texttable_notes);
    table_notes.style.margin="auto auto auto 108px";
    table_notes.style.float = "left";
    table_notes.style.fontSize="60%";
    table_notes.style.color="rgb(100,100,100)";
    table_captchac.appendChild(table_notes);
    var table_check = document.createElement("div");
    var texttable_check = document.createTextNode(value["check"]);
    table_check.appendChild(texttable_check);
    table_check.style.margin="5px auto 10px 108px";
    table_check.style.float = "left";
    table_check.style.fontSize="160%";
    table_check.style.color="rgb(100,100,200)";
    table_captchac.appendChild(table_check);
    table_pop.appendChild(table_captchac);
    var table_usernamelogin = document.createElement("div");
    table_usernamelogin.style.clear = "both";
    table_usernamelogin.style.borderWidth = "1px";
    table_usernamelogin.style.borderColor = "rgb(150,150,150)";
    table_usernamelogin.style.borderTopStyle="dashed";
    var table_loginusername = document.createElement("input");
    table_loginusername.type = "button";
    table_loginusername.value="登录";
    table_loginusername.style.margin="10px auto 5px 160px";
    table_loginusername.onclick=function() {
        var params = {"sid":sid};
        params["checktext"]= table_captchainput.value;
        params["name"]= table_usernameinput.value;
        params["key"]= table_userkeyinput.value;
        JSONRequest.post("http://abelkhan.com/login",params,
            function (requestNumber, value, exception){
                if (value["loginend"]){
                    alertWinMsg("登录成功");
                
                    document.getElementById("titlereg").style.visibility="hidden";
                    document.getElementById("titlereg").style.display="none";
                    document.getElementById("titlelogin").style.visibility="hidden";
                    document.getElementById("titlelogin").style.display="none";
                    document.getElementById("titlelogincontainer").style.visibility="hidden";
                    document.getElementById("titlelogincontainer").style.display="none";
                
                    document.getElementById("titlelgoinout").style.visibility="visible";
                    document.getElementById("titlelgoinout").style.display="";
                    document.getElementById("titleusername").style.visibility="visible";
                    document.getElementById("titleusername").style.display="";
                    document.getElementById("titleusernamecontainer").style.visibility="visible";
                    document.getElementById("titleusernamecontainer").style.display="";
                
                    document.getElementById("titleusername").innerHTML = value["username"];
                    username = value["username"];
                
                    document.body.removeChild(msgObj);
                    document.body.removeChild(table);
                    poptable = null;
                    popobj = null;
                }
                if (value["checkend"] === false){
                    alertWinMsg("校验码错误");
                }
                if (value["userisnotdefine"]){
                    alertWinMsg("不存在的用户");
                }
                if (value["loginend"] === false){
                    alertWinMsg("密码错误");
                }
            });
    };
    table_usernamelogin.appendChild(table_loginusername);
    table_pop.appendChild(table_usernamelogin);
    table.appendChild(table_pop);
    document.body.appendChild(table);
    msgObj.style.height = (table.offsetHeight +20).toString() + "px";
    table.style.top = ((iHeight-table.offsetHeight)/2).toString()+"px";
    msgObj.style.top = (((iHeight-table.offsetHeight)/2)-10).toString()+"px";
    document.body.appendChild(msgObj);
    popobj = msgObj;
    poptable = table;
}

function loginout(){
    var params = {"sid":sid};
    JSONRequest.post("http://abelkhan.com/loginout", params, function (requestNumber, value, exception){});
}

function postsearchrequest(index){
    if (document.getElementById("title_edit").value != ""){
        var params = {"sid":sid, "input":document.getElementById("title_edit").value, "index":index};
        if (username !== null){
            params["username"] = username;
        }
        JSONRequest.post("http://abelkhan.com/skipsearchpage", params, function (requestNumber, value, exception){
            url = value["url"];
            location.assign(url);
            window.location=url;
            location.href=url;
        });
    }
}