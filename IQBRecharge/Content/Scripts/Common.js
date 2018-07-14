var wxUrl = "http://wx.iqianba.cn";
var payUrl = "http://pp.iqianba.cn";

function GetUrlParam(name,nd) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null)
    {
        if (nd)
        {
            return unescape(decodeURI(r[2]));
        }
        return unescape(r[2]);
    }
       
    return null; //返回参数值
}

function Trim(str) {

    return str.replace(/(^\s*)|(\s*$)/g, "");

}

function IsWeixinOrAlipay() {
    var ua = window.navigator.userAgent.toLowerCase();
    //判断是不是微信
    if (ua.match(/MicroMessenger/i) == 'micromessenger') {
        return "WeiXIN";
    }
    //判断是不是支付宝
    if (ua.match(/AlipayClient/i) == 'alipayclient') {
        return "Alipay";
    }
    //哪个都不是
    return "false";
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

