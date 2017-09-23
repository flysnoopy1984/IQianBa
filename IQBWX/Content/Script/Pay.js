function onBridgeReady(json){

    WeixinJSBridge.invoke(
        'getBrandWCPayRequest',
        json ,
        function (res) {
            if (res.err_code) {
                alert("错误，请联系管理员");                
            }
            else {
                if (res.err_msg == "get_brand_wcpay_request:ok") {
                    window.location.href = '/Html5/ApplySuccess?msg=' + res.err_msg + "&type=1";
                }
                if (res.err_msg == "get_brand_wcpay_request:cancel") {
                    alert("支付被取消");                  
                }
            }            
            
        }
    );
}

//初始化微信支付环境
function fPay() {
    if (typeof WeixinJSBridge == "undefined") {
        if (document.addEventListener) {
            document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
        } else if (document.attachEvent) {
            document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
            document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
        }
    } else {
        fPostPay();
    }
}

function ShowInfo(title, msg)
{
    var InfoTitle = $("#InfoTitle");
    var InfoMessage = $("#InfoMessage");

    InfoTitle.text(title);
    InfoMessage.text(msg);

    $(".tc").show();
    $(".mengban").show();
    $("msg").show();

}

function HideInfo()
{
    $("msg").text("");
    $(".mengban").text("");
    $(".tc").hide();
}



function fPostPay(url) {

    var seltc = parseInt($("input[name='radioLevel']:checked").val());
    var userName = $("#userName").val().trim();
    var userPhone = $("#userPhone").val().trim();
    var userVerifyCode = $("#userVerifyCode").val().trim();
    var WXNum = parseInt($("#WXNum").val());
  
    var province = parseInt($("#Province").val());
  
    var userId = $("#hUserId").val();

    if (userName == "") {
        ShowInfo("错误", "用户名没有填写");
        return;
    }
    if (userPhone == "") {
        ShowInfo("错误", "手机不能为空");
        return;
    }
    if (userVerifyCode == "") {
        ShowInfo("错误", "请先填写验证码");
        return;
    }
    if (WXNum == 0) {
        ShowInfo("错误", "请选择微信好友数");
        return;
    }
    if (province == 0) {
        ShowInfo("错误", "请选择所在省市");
        return;
    }

    var WXRange = $("#WXNum").find("option:selected").text();
    var provinceValue = $("#Province").find("option:selected").text();

    var data = {
        "WXRange": WXRange,
        "ProvinceValue":provinceValue,
        "userVerifyCode":userVerifyCode,
        "userName": userName,
        "userPhone": userPhone,
        "WXNum": WXNum,
        "Province": province,
        "seltc": seltc,
        "userId":userId
    };   

    data = JSON.stringify(data);

    $.ajax({
        type: "post",
        dataType: "json",
        data: data,
        url: url,
        contentType: 'application/json; charset=utf-8',
        success: function (json) {
            if(json.errorCode ==undefined)
                onBridgeReady(json);
            else
            {
                ShowInfo("错误", json.errorMsg);
            }
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowInfo("错误", errorThrown);
        }
    })

}