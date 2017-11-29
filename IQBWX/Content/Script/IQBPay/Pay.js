
function InitControls() {

    $("#BnVerifyConfirm").hide();
    //支付区域
    $("#PayContent").hide();

    $("#AliPayAccount").attr("disabled", false);

    $("#bnModifyAliPayAccount").hide();
    $("#bnConfirmAliPayAccount").show();
  
}

$(document).ready(function () {

    InitControls();
});

function ModifyAliPayAccount() {
    InitControls();
}

function ConfirmAliPayAccount() {

    var AliPayAccount = $("#AliPayAccount").val();

    $.confirm({
        title: '请谨慎确认!',
        content: '收款支付宝账户：【' + AliPayAccount + "】！！",
        buttons: {
            confirm: {

                btnClass: 'btn-blue',
                text: '确定',
                action: function () {
                    $.alert('收款账户' + AliPayAccount);
                    $("#AliPayAccount").attr("disabled", true);
                    $("#bnModifyAliPayAccount").show();
                    $("#bnConfirmAliPayAccount").hide();
                    $("#PayContent").show();
                }

            },
            cancel: {
                text: '重新输入',


            }

        }
    });

   
}


function PayToAli() {
    var amt = $("#TotalAmout").val();
    var qrUserId = $("#qrUserId").val();
    if (amt == null || amt == "" || amt == 0) {
        alert("金额不能为空");
        return;
    }
    $("#btnPay").attr("disabled", true);
    if (qrUserId == null || qrUserId == "") {
        alert("未获取代理商家ID，请重新扫描后再尝试或联系代理商家");
        return;
    }
    var AliPayAccount = $("#AliPayAccount").val();
    if (AliPayAccount == null || AliPayAccount == "") {
        alert("收款账号请准确填写！");
        return;
    }

    var url = payUrl + "/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt + "&AliPayAccount=" + AliPayAccount;

    window.location = url;
}