
function InitControls() {

    //$("#BnVerifyConfirm").hide();
    ////支付区域
    //$("#PayContent").hide();

    //$("#AliPayAccount").attr("disabled", false);

    //$("#bnModifyAliPayAccount").hide();
    //$("#bnConfirmAliPayAccount").show();
  

}

//function ShowPayArea()
//{
//    $("#AliPayAccount").attr("disabled", true);
//    $("#bnModifyAliPayAccount").show();
//    $("#bnConfirmAliPayAccount").hide();
//    $("#PayContent").show();
//}

$(document).ready(function () {

    $("#btnPay").click(function () {　　//普通事件方法
       
        PayToAli();
    });
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
        window.location.href = "/Home/ErrorMessage?code=2000&ErrorMsg=请用支付宝打开";
            alert("请使用支付宝打开");

        return false;
    }

 

    //$.alert({
    //    theme: "dark",
    //    title: "注意",
    //    content: "风控用户请【199支付】必过！！",
    //    btnClass:"btn-warning",
    //    width: '30%',
       
         
    //});

   
});

//function ModifyAliPayAccount() {
//    InitControls();
//}

//function ConfirmAliPayAccount() {

//    var AliPayAccount = $("#AliPayAccount").val();

//    $.confirm({
//        title: '请谨慎确认!',
//        content: '如收款码输入有误，您将无法收到款项!',
//        buttons: {
//            confirm: {

//                btnClass: 'btn-blue',
//                text: '确定',
//                action: function () {
                   
//                    ShowPayArea();
//                }

//            },
//            cancel: {
//                text: '重新输入',


//            }

//        }
//    });

   
//}
//function GoToSafePay()
//{
//    var qrUserId = $("#qrUserId").val();
//    var url = "/PP/Pay2?Id=" + qrUserId;
//    window.location = url;
//}

function PayToAli() {
    var QRMin = parseFloat($("#QRMin").val());
    var QRMax = parseFloat($("#QRMax").val());
 
    var amt = parseFloat($("#TotalAmout").val());
    if (amt < QRMin || amt > QRMax) {

        $.alert({
            theme: "dark",
            title: "错误",
            content: "金额区间必须在【" + QRMin + "-" + QRMax + "】",

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
    $("#btnPay").attr("disabled", true);
    var url = payUrl + "/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt ;
    window.location.href = url;
    //设置账户cookie
    //setCookie("YJ_AliPayAccount", AliPayAccount, 3);

    //var str = '<div style="font-size:26px">若支付宝出现以下提示<br />说明您只能<span style="color:firebrick">199元连续支付</span></div>';
    //str += '<div style="text-align:center; margin-top:10px;"><img src="/Content/images/PayError1.jpg" /></div>';
    //$.confirm({
    //    theme: "modern",
    //    title: '注意',
    //    type: 'red',
    //    content: str,
    //    buttons: {
    //        Know: {
    //            btnClass: 'btn btn-danger',
    //            text: "我知道了",
    //            action: function () {
    //                // setCookie("YJ_PayWarning", 1, 3);
    //                window.location.href = url;
    //            }
    //        },

    //    }
    //});

    window.location = url;

   
}