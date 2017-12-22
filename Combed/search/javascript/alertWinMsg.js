function alertWinMsg(text){
    var width = 400;
    var bgcolor = "RGB(220, 220, 220)";
    var iWidth = document.documentElement.clientWidth;
    var iHeight = document.documentElement.clientHeight;
    var msgObj=document.createElement("div");
    msgObj.style.position="absolute";
    msgObj.style.top = (((iHeight)/2)-10).toString()+"px";
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
    table.style.top = ((iHeight)/2).toString()+"px";
    table.style.left = ((iWidth - width)/2).toString()+"px";
    table.style.width = width.toString() + "px";
    var table_pop = document.createElement("div");
    table_pop.style.width = width.toString() + "px";
    var table_text_1 = document.createElement("div");
    var texttable_text_1 = document.createTextNode(text);
    table_text_1.appendChild(texttable_text_1);
    table_text_1.style.margin="10px auto 10px 140px";
    table_pop.appendChild(table_text_1);
    var table_button_1 = document.createElement("input");
    table_button_1.type = "button";
    table_button_1.value="确定";
    table_button_1.style.margin="10px auto 10px 170px";
    table_button_1.onclick = function(){
        document.body.removeChild(msgObj);
        document.body.removeChild(table);
    };
    table_pop.appendChild(table_button_1);
    table.appendChild(table_pop);
    document.body.appendChild(table);
    msgObj.style.height = (table.offsetHeight +20).toString() + "px";
    document.body.appendChild(msgObj);
}
