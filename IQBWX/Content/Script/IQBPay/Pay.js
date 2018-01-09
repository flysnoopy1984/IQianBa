
function InitControls() {

    //$("#BnVerifyConfirm").hide();
    ////支付区域
    //$("#PayContent").hide();

    //$("#AliPayAccount").attr("disabled", false);

    //$("#bnModifyAliPayAccount").hide();
    //$("#bnConfirmAliPayAccount").show();
  

}

function ShowPayArea()
{
    $("#AliPayAccount").attr("disabled", true);
    $("#bnModifyAliPayAccount").show();
    $("#bnConfirmAliPayAccount").hide();
    $("#PayContent").show();
}

$(document).ready(function () {

    //InitControls();
    //var account = getCookie("YJ_AliPayAccount");
    //if (account != null || account!="") {
    //    GoToSafePay();
    //}
    //var account = getCookie("YJ_AliPayAccount");
    //if (account != null)
    //{
    //    $("#AliPayAccount").val(account);
    //    ShowPayArea();
    //}
    var client = IsWeixinOrAlipay();
    if (client != "Alipay") {
        window.location.href = "/Home/ErrorMessage?code=3000";
    }
   
   
});

function ModifyAliPayAccount() {
    InitControls();
}

function ConfirmAliPayAccount() {

    var AliPayAccount = $("#AliPayAccount").val();

    $.confirm({
        title: '请谨慎确认!',
        content: '如收款码输入有误，您将无法收到款项!',
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
function GoToSafePay()
{
    var qrUserId = $("#qrUserId").val();
    var url = "/PP/Pay2?Id=" + qrUserId;
    window.location = url;
}

function PayToAli() {

 
    var amt = $("#TotalAmout").val();

    var amt = $("#TotalAmout").val();
    if (amt < 20 || amt > 1499) {

        $.alert({
            theme: "dark",
            title: "错误",
            content: "金额区间必须在【20-1499】",

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
  
    if (qrUserId == null || qrUserId == "") {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "未获取代理商家ID，请重新扫描后再尝试或联系代理商家",

        });

        return;
    }
    $("#btnPay").attr("disabled", true);
  

    var url = payUrl + "/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt+"&PayType=1";
  //  setCookie("YJ_AliPayAccount", AliPayAccount, 3);
    

    window.location = url;

   
}