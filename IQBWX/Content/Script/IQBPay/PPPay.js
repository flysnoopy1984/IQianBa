const InitCount = 90;
var countdown = InitCount;

function GetUserVerifyCode(uId) {
    if (checkPhone() == false) {
        ShowInfo("错误", "请先正确填写手机号码");
        return;
    }
    var bn = $("#BnGetVerifyCode");

    bn.attr("disabled", true);
    bn.css("background", "#DDDDDD");
    settime(bn);

    var hUserId = $("#hUserId").val();
    var url = "/api/sms/NewMemberSMSVerify?userId=" + uId;

    $.ajax({
        type: "get",
        data: "",
        url: url,
        success: function (result) {
            if (result == "OK") {
                return;
            }
        },
        error: function () {

        }
    })
}

function settime(obj) {
    if (countdown == 0) {
        obj.attr("disabled", false);
        obj.css("background", "#5581ea");
        obj.addClass("bai");
        obj.val("获取验证码");
        countdown = InitCount;
        return;
    } else {
        countdown--;
        obj.val("重新发送(" + countdown + ")");

    }

    setTimeout(function () {
        settime(obj)
    }, 1000)
}

function PayToAli() {
    var amt = $("#TotalAmout").val();
    var qrUserId = $("#qrUserId").val();
    if (amt == null || amt == "" || amt == 0) {
        alert("金额不能为空");
        return;
    }
    $("#btnPay").attr("disabled", true);
    
    window.location = "http://ap.iqianba.cn/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt;
}