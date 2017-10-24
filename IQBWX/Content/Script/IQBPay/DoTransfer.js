function Transfer(orderAmt, CommAmt, account, openId)
{

    if (account == "" || account == null) {
        alert("用户没有设置提款账号，无法打款！");
        return;
    }
    if (orderAmt + CommAmt == 0)
        alert("没有可转金额");

    //  var url = "http://ap.iqianba.cn/API/TransferAPI/TransferToAgent";
    var url = "http://ap.iqianba.cn/API/TransferAPI/TransferToAgent";
    $.ajax({
        type: 'post',
        data: "OpenId=" + openId + "&AliPayAccount=" + account + "&OrderAmount=" + orderAmt + "&CommissionAmount=" + CommAmt,
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert("转账成功！");
                $(obj).parent().prev().find("a").text(0);
                $(obj).parent().prev().prev().find("a").text(0);
            }
            else
                alert(data.ErrorMsg);
        },
        error: function (xhr, type) {

            alert('Ajax error!');


        }
    });
}