function setCookie(c_name,value,expiredays)
{
    var exdate=new Date();
    exdate.setDate(exdate.getDate()+expiredays);
    document.cookie = c_name + "=" + escape(value) + ((expiredays==null)?"":";expires="+exdate.toGMTString()) + ";path=/;Domain=abelkhan.com";
}
