function onBridgeReady(json) {

    WeixinJSBridge.invoke(
        'getBrandWCPayRequest',
        json,
        function (res) {
            if (res.err_code) {
                alert("错误，请联系管理员");
            }
            else {
                if (res.err_msg == "get_brand_wcpay_request:ok") {
                    window.location.href = site + '/Html5/ApplySuccess?msg=' + res.err_msg + "&type=2";
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

function ShowInfo(title, msg) {
    var InfoTitle = $("#InfoTitle");
    var InfoMessage = $("#InfoMessage");

    InfoTitle.text(title);
    InfoMessage.text(msg);

    $(".tc").show();
    $(".mengban").show();
    $("msg").show();

}

function HideInfo() {
    $("msg").text("");
    $(".mengban").text("");
    $(".tc").hide();
}


function PayUpdateMember(url) {
 
    var userId = $("#hUserId").val();
    var data = {       
        "UserId": userId
    };
  
    data = JSON.stringify(data);

    $.ajax({
        type: "post",
        dataType: "json",
        data: data,
        url: url,
        contentType: 'application/json; charset=utf-8',
        success: function (json) {
            if (json.errorCode == undefined)
                onBridgeReady(json);
            else {
                ShowInfo("错误", json.errorMsg);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowInfo("错误", errorThrown);
        }
    })
}