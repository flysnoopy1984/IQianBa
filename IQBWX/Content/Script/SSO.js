var url = 
$(document).ready(function () {
    /*
    var IsSupport;
    IsSupport = typeof (window.WebSocket) != "function";
    IsSupport = typeof( window.WebSocket) != 'undefined';
    if (!window.WebSocket) {
        $('body').html("<h1>Error</h1><p>Your browser does not support HTML5 Web Sockets. Try Google Chrome instead.</p>");
    }
    */
    var appId = getUrlParam("logintype");
    if (appId == null)
    {
        //1 指向book.iqianba.cn
        appId = "1";
    }
  
    $("#imgQR").attr('src', '/Content/images/qrloading.gif');
    $.ajax({
        type: "get",
        url: "/API/WX/CreateSSOQR",
        data:"appId="+appId,
        success: function (result) {
            $("#qrLoading").hide();
            $("#QRDesc").show();
            $("#imgQR").attr('src', result.QRImgUrl);
            waitingScan(result.ssoToken, appId);
        },
    });
   
})

function waitingScan(ssoToken,appId)
{
    $.ajax({
        type: "get",
        data: "ssoToken="+ssoToken+"&appId="+appId,
        url: "/API/WX/WaitingScan",
        timeout: 60000,
        success: function (result) {
            alert(result);
            if (result != "")
            {
                
                if (appId == "pp")
                {
                    alert(result.ReturnUrl);
                    window.location = result.ReturnUrl;
                    return;
                }
                if (appId == "" || appId == "1") {
                    window.location = "http://book.iqianba.cn/member/wxlogin.php?openId=" + result.openId;
                    return;
                }
                
            }
                
            return;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (textStatus == 'timeout') {
               
                window.location.reload();
            }
        },

    })
}