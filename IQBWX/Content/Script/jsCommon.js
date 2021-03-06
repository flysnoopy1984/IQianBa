﻿var wxUrl = "http://wx.iqianba.cn";
var payUrl = "http://pp.iqianba.cn";
//var payUrl = "http://localhost:24068";
function StringFormat() {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}

function removeText(id)
{
    $("#"+id).val("");
}

function DateFormate(cellval) {
    var datetime = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));

    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
    var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
    return year + "-" + month + "-" + date ;
}

function DateTimeFormate(cellval) {
    var datetime = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));

    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
    var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
    var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
    var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
    var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
    return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
}

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

function checkPhone() {
    var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
    if (!myreg.test($("#userPhone").val())) {
        return false;
    }
    return true;
}

// 验证手机号
function isPhoneNo(phone) {
    var pattern = /^1[345789]\d{9}$/;
    return pattern.test(phone);
}


function IsWeixinOrAlipay() {
    var ua = window.navigator.userAgent.toLowerCase();
    //判断是不是微信
    if (ua.match(/MicroMessenger/i) == 'micromessenger') {
        return "WeiXin";
    }
    //判断是不是支付宝
    if (ua.match(/AlipayClient/i) == 'alipayclient') {
        return "Alipay";
    }
    //哪个都不是
    return "false";
}

function toErrorPage(msg,backUrl)
{
    window.location.href = "/Home/ErrorMessage?code=9000&ErrorMsg=" + msg + "&backUrl=" + backUrl;
}

String.format = function () {
    // The string containing the format items (e.g. "{0}")
    // will and always has to be the first argument.
    var theString = arguments[0];

    // start with the second argument (i = 1)
    for (var i = 1; i < arguments.length; i++) {
        // "gm" = RegEx options for Global search (more than one instance)
        // and for Multiline search
        var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");

        theString = theString.replace(regEx, arguments[i]);
    }

    return theString;
}
