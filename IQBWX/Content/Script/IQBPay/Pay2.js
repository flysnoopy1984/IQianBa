
function InitControls() {

    //  $("#BnVerifyConfirm").hide();
    //支付区域
    $("#PayContent").hide();

    $("#AliPayAccount").attr("disabled", false);

    $("#bnModifyAliPayAccount").hide();
    $("#bnConfirmAliPayAccount").show();

}

function ShowPayArea() {
    $("#AliPayAccount").attr("disabled", true);
    $("#bnModifyAliPayAccount").show();
    $("#bnConfirmAliPayAccount").hide();
    $("#PayContent").show();
}

$(document).ready(function () {

    var client = IsWeixinOrAlipay();
    if (client != "Alipay") {
        window.location.href = "/Home/ErrorMessage?code=2000&ErrorMsg=请用支付宝打开";
        alert("请使用支付宝打开");
       
        return false;
    }
    //$.alert({
    //    theme: "dark",
    //    title: "注意",
    //    content: "风控用户请【199连续支付】！",
    //    btnClass: "btn-warning",
    //    width: '30%',
    //});

    InitControls();
    var account = getCookie("YJ_AliPayAccount");
    if (account != null) {
        $("#AliPayAccount").val(account);
        ShowPayArea();
    }

});

function ModifyAliPayAccount() {
    InitControls();
}

function ConfirmAliPayAccount() {

    var AliPayAccount = $("#AliPayAccount").val();
    if (AliPayAccount == "") {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "收款账号请准确填写！",

        });
        return;
    }

    $.confirm({
        title: '谨慎确认!',
        content: '请输入自己的另一个支付宝账户用于收款，不要输入其他人的账户，以防诈骗！',
        buttons: {
            confirm: {

                btnClass: 'btn-blue',
                text: '确定',
                action: function () {

                    ShowPayArea();
                }
            },
            cancel: {
                text: '重新输入',

            }
        }
    });
}

function GoToFastPay() {
    //delCookie("YJ_AliPayAccount");
    var qrUserId = $("#qrUserId").val();
    var url = "/PP/Pay?Id=" + qrUserId;
    window.location = url;
}

function PayToAli() {

    var amt = $("#TotalAmout").val();
    if (amt < 20 || amt > 799) {

        $.alert({
            theme: "dark",
            title: "错误",
            content: "金额区间必须在【20-799】",

        });
        return;
    }
    var qrUserId = $("#qrUserId").val();
    if (amt == null || amt == "" || amt == 0) {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "金额不能为空",

        });

        return;
    }
    $("#btnPay").attr("disabled", true);
    if (qrUserId == null || qrUserId == "") {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "未获取代理商家ID，请重新扫描后再尝试或联系代理商家",

        });

        return;
    }
    var AliPayAccount = $("#AliPayAccount").val();
    if (AliPayAccount == null || AliPayAccount == "") {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "收款账号请准确填写！",

        });

        return;
    }

    $("#btnPay").attr("disabled", true);
    var url = payUrl + "/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt + "&AliPayAccount=" + AliPayAccount + "&PayType=1";

    // var url = payUrl + "/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt;
    setCookie("YJ_AliPayAccount", AliPayAccount, 3);

    var str = '<div style="font-size:26px">若支付宝出现以下提示<br />说明您只能<span style="color:firebrick">199元连续支付</span></div>';
    str += '<div style="text-align:center; margin-top:10px;"><img src="/Content/images/PayError1.jpg" /></div>';


    $.confirm({
        theme: "modern",
        title: '注意',
        type: 'red',
        content: str,
        buttons: {
            Know: {
                btnClass: 'btn btn-danger',
                text: "我知道了",
                action: function () {
                    window.location = url;
                }
            },

        }
    });

}