$(document).ready(function () {

    var url = wxUrl+"/Home/ErrorMessage?code=2000&ErrorMsg=请用支付宝打开";
    var client = IsWeixinOrAlipay();
    if (client != "Alipay") {
        window.location.href = url;
        alert("请使用支付宝打开");
      
        return false;

    }

    InitControls();
    var account = getCookie("YJ_Huge_AliPayAccount");
    if (account != null) {
        $("#AliPayAccount").val(account);
        ShowPayArea();
    }

});

function InitControls() {
    $("#buttonArea").hide();

    $("#bnModifyAliPayAccount").hide();
    $("#bnConfirmAliPayAccount").show();

}

function ShowPayArea() {

    $("#AliPayAccount").attr("disabled", true);
    $("#bnModifyAliPayAccount").show();
    $("#bnConfirmAliPayAccount").hide();
    $("#buttonArea").show();
}

function ModifyAliPayAccount() {
    InitControls();
}

function ConfirmAliPayAccount() {

    var AliPayAccount = $("#AliPayAccount").val();

    $.confirm({
        theme:'material',
        title: '谨慎确认!',
        content: '请输入自己的另一个支付宝账户，不要输入其他人的账户，以防诈骗！',
        buttons: {
           
            cancel: {
                text: '重新输入',
                btnClass: 'btn-info',

            },
             confirm: {
                 btnClass: 'btn-danger',
                text: '我很确定',
                action: function () {
                    ShowPayArea();
                }
            }
        }
    });
}

function PayToAli() {

    $.confirm({
        theme:"modern",
        title: '确认',
        type:'red',
        content: '请务必一次性完成支付！如需测试是否风控，请索要小额通道码',
        buttons: {
            Know: {
                btnClass: 'btn-warning',
                text: "我要继续",
                action: function () {
                    GotoPay();
                }
            },
            cancel: {
                text: "取消",
                btnClass: 'btn-primary',
            },
        }
    });
}

function GotoPay()
{
    var qrHugeId = $("#qrHugeId").val();

    $("#btnPay").attr("disabled", true);

    var AliPayAccount = $("#AliPayAccount").val();
    if (AliPayAccount == null || AliPayAccount == "") {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "请准确填写收款账号！",
        });
        return;
    }

    var url = "/AliPay/F2FHugePay?rQRHugeId=" + qrHugeId + "&AliPayAccount=" + AliPayAccount;
    setCookie("YJ_Huge_AliPayAccount", AliPayAccount, 3);
    window.location = url;

}