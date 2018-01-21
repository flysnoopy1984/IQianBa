$(document).ready(function () {

    var client = IsWeixinOrAlipay();
    if (client == "false") {
        window.location.href = "http://m.yujiept.icoc.me/";
        alert("请用微信或支付宝访问");
        
        return false;
    }

    var errorCode = $("#errorCode").val();
    var url = $("#urlToJM").val();
    $("#btnContainer").show();
    if(errorCode =="1002")
    {
        $("#goAction").attr("href",url );
        $("#goAction").text("立刻加入");
    }
    if(errorCode =="9999")
    {
        $("#btnContainer").hide();
    }
    //OpenIdNotFound
    if (errorCode == "1003") {
        $("#btnContainer").hide();
    }
  

    if (errorCode == "2000" || errorCode == "2002") {
        $("#btnContainer").hide();
    }

    if (errorCode == "2003" || errorCode == "2004" || errorCode == "2005") {

        $("#btnContainer").hide();
    }

    //微信和支付宝客户端
    if (errorCode == "3000" || errorCode == "3001") {
        $("#btnContainer").hide();
    }
  
})