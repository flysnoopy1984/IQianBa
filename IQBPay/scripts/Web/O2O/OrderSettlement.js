$(document).ready(function () {

    var canDo = CheckPage();
    if (canDo == -1)
    {
        alert("Session 失效，重新登陆");
        window.location.href = "/O2O/Login";
    }
    if (canDo == -2) {
        alert("订单未获取，联系管理员!");
        window.open("about:blank", "_self").close();
    }

    $("#ImgSpan").viewer({
        navbar: false,
        toolbar: false,
    });
});

function ConfirmWithPay()
{
    var url = "/O2O/OrderSettlementToPP";

    var O2ONo = $("#O2ONo").val();
    if (O2ONo == "" || O2ONo == "null")
    {
        alert("订单编号为空，联系管理员!");
        window.open("about:blank", "_self").close();
    }
    $("#btnConfirm").hide();
    self.opener.location.reload();
    window.open("about:blank", "_self").close();
   
    
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "O2ONo": O2ONo },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert("结算成功！");
                window.open("about:blank", "_self").close();
                window.opener.location.reload();
            }
            else {
                alert(data.ErrorMsg);
                if (data.IntMsg == -1) {
                    window.location.href = "/O2O/Login";
                }
                $("#btnConfirm").show();
            }

        },
        error: function (xhr, type) {
            alert('Ajax error!');
        }
    });
    
}