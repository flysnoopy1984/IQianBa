const InitCount = 60;
var countdown = InitCount;
var SMSId = "";

function InitControls()
{
    $("#BnVerifyConfirm").hide();
    //支付区域
    $("#PayContent").hide();
    $("#userPhone").attr("disabled", false);
   
    $("#BnGetVerifyCode").attr("disabled", false);
    $("#BnGetVerifyCode").text("获取验证码");
    $("#BnGetVerifyCode").css("background", "#47a447");

    //短信验证区域
    $("#VerifyArea").show();
    $("#VerifyCode").val("");

    countdown = 0;
    SMSId = "";

}

$(document).ready(function () { 
  
    InitControls();
});

function VerifyCodeConfirm()
{
  
    var code = $("#VerifyCode").val();
    var url = "/api/sms/IQBPay_ConfirmVerifyCode?Code=" + code + "&SMSId=" + SMSId;

    $.ajax({
        type: "get",
        data: "",
        url: url,
        success: function (result) {

            //验证成功
            if (result.SMSVerifyStatus == 3)
            {
                alert("验证码成功，请开始支付");
                $("#OrderNo").val(result.OrderNo);
                $("#PayContent").show();
                $("#VerifyArea").hide();
                return;
            }
            if (result.SMSVerifyStatus == 5)
            {
                alert("验证码已失效，请重新验证");
                InitControls();
                return;
            }

            if (result.SMSVerifyStatus == 4) {
                alert("验证码不正确，请重新验证");
                InitControls();
                return;
            }

            if (result.SMSVerifyStatus == -1)//UnKnow
            {
                alert("短信系统错误，请联系平台！");
                InitControls();
                return;
            }
        },
        error: function (xhr, type) {

            alert("短信系统错误，请联系平台！");
            return;

        }
    })

}

function GetVerifyCode() {
    if (checkPhone() == false) {
        alert("错误,请先正确填写手机号码");
        return;
    }
    //var OrderNo = $("#OrderNo").val();
    //if (OrderNo == "" || OrderNo == null)
    //{
    //    alert("支付订单号码未生成，请联系站长！");
    //    return;
    //}
    var ReceiveNo = $("#ReceiveNo").val();
    var phone = $("#userPhone");
    phone.attr("disabled", true);

    var bn = $("#BnGetVerifyCode");
    bn.attr("disabled", true);
    bn.css("background", "#DDDDDD");

    var url = "/api/sms/SentSMS_IQBPay_BuyerOrder?ReceiveNo=" + ReceiveNo + "&mobilePhone=" + phone.val() + "&IntervalSec=" + InitCount;

    $.ajax({
        type: "get",
        data: "",
        url: url,
        success: function (result) {

            if (result.SMSVerifyStatus == 6)
            {
                alert("短信系统发送失败，请联系平台！");
                InitControls();
                return;
            }

            $("#BnVerifyConfirm").show();
            if (result.RemainSec > -1) {
                alert("消息已在上次发送");
                countdown = result.RemainSec;
            }
            else
            {
                countdown = InitCount;
            }
            SMSId = result.SmsID;
            settime(bn);
        },
        error: function (xhr, type) {

            alert("短信系统错误，请联系平台！");
            InitControls();
            return;

        }
    })
}

function settime(obj) {
    if (countdown == 0) {
        obj.attr("disabled", false);
        obj.css("background", "#47a447");
       // obj.addClass("bai");
        obj.text("获取验证码");
        countdown = InitCount;

        $("#userPhone").attr("disabled", false);


        return;
    } else {
        countdown--;
        obj.text("重新发送(" + countdown + ")");

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
    
    window.location = "http://ap.iqianba.cn/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt + "&ReceiveNo=" + ReceiveNo;
}