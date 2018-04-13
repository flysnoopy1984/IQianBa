$(function () {

    var O2ONo;

    Init = function () {
        O2ONo = GetUrlParam("O2ONo");
        if (O2ONo == null || O2ONo == "")
        {
            alert("订单编号没有获取，请联系管理员");
            toPage("/O2OWap/OrderList");
        }
        var sms = $("#smsTemplate").text().trim();
        $("#btnCopy").attr("data-clipboard-text", sms);

        var url = "/O2OWap/Order_SMSInfo";
        $.ajax({
            type: 'post',
            data: { 'O2ONo': O2ONo },
            url: url,
            success: function (res) {
                if (res.IsSuccess) {
                    if (res.resultObj.v4)
                    {
                        $("#opStatus").text("(已操作)");
                        $("#btnList").hide();
                    }
                }
                else {
                    alert(res.ErrorMsg);
                }
            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });

    },

    toDetailPage = function () {
        toPage("/O2OWap/OrderDetail?O2ONo=" + O2ONo);
    }

  
    DoSMSConfirm = function () {
        var url = "/O2OWap/DoSMSConfirm";

        $.ajax({
            type: 'post',
            data: { 'O2ONo': O2ONo },
            url: url,
            success: function (res) {

                if (res.IsSuccess) {
                    alert("操作成功");
                    window.location.reload();
                }
                else
                {
                    alert(res.ErrorMsg);
                    if(res.IntMsg == -1)
                        toPage("/O2OWap/OrderList");
                }
            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    };

    copyToClipboard = function (Id) {

        var clipboard = new ClipboardJS("#" + Id, {
            text: function (trigger) {
                return ReceiveAddress;
            }
        });


    };

    Init();

     $(document).on("click", "#btnCopy", function () {
       var clipboard = new ClipboardJS('.copy');
       alert("已复制到剪贴板，您可直接黏贴到短信中。");
   });

    $(document).on("click", "#btnDone", DoSMSConfirm);
    $(document).on("click", "#btnBack", toDetailPage);



  
});