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
        //指向平台
        appId = "pp";
    }
  
    $("#imgQR").attr('src', '/Content/images/qrloading.gif');
    $.ajax({
        type: "get",
        url: "/API/WX/CreateSSOQR",
        data:"appId="+appId,
        success: function (result) {
            $("#qrLoading").hide();
            $("#QRDesc").show();
            $("#imgQR").attr('src', result.TargetUrl);
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
          
            if (result != "")
            {
                if (result.ErrorMsg == "timeout")
                {
                    alert("超时，请重新扫描");
                    window.location.reload();
                    return;

                }
                if (result.ErrorMsg == "close")
                {
                    alert("系统关闭");
                    window.location.href = result.ReturnUrl;
                    return;
                }
                window.location.href = result.ReturnUrl + "?openId=" + result.OpenId;
                return;
               
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