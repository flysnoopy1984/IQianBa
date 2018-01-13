$(document).ready(function () {
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

    if (errorCode == "2003") {

        $("#btnContainer").hide();
    }

    //微信和支付宝客户端
    if (errorCode == "3000" || errorCode == "3001") {
        $("#btnContainer").hide();
    }
  
})