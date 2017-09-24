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
    $("#imgQR").attr('src', '/Content/images/qrloading.gif');
    $.ajax({
        type: "get",
        url: "/API/WX/CreateSSOQR",
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
        data: "",
        url: "/API/WX/WaitingScan?ssoToken=" + ssoToken,
        timeout: 60000,
        success: function (result) {
            if (result != "")
            {
                if (appId == "PP")
                {
                    window.location = "#";
                    return;
                }
                if (appId == "1") {
                    window.location = "http://book.iqianba.cn/member/wxlogin.php?openId=" + result;
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