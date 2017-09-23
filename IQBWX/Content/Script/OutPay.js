$(document).ready(function () {
    var url = site + "/Pay/GetAvailDeposit";
    $.ajax({
        type: "post",
        data: "",
        url: url,
        success: function (result) {
            $("#MyBalance").text("¥" + result.Amount);
            $("#mbVal").val(result.Amount);
        },
        error: function (ex) {
        }

    });
});

function closeMsg()
{
    $("#msg").hide();
}

function OutPay()
{
    var url = site + "/Pay/DoDeposit";
    var amt = $("#mbVal").val();  

    if (amt <= 0 || amt == "")
    {
        $("#msgTitle").text("提现失败");
        $("#msgInfo").text("余额不足，不能提现");
        return;
    }
   

    $.ajax({
        type: "post",
        data: "amt=" + amt,
        url: url,
        success: function (result) {
            if(result.OutResult == 1)
            {
                $("#msgTitle").text("提现成功");
                $("#msgInfo").text("您现在可以在公众号里看到提现的红包");
                $("#MyBalance").text("¥0");
                $("#mbVal").val(0);
               
            }
            else if(result.OutResult == -1)
            {
                $("#msgTitle").text("提现失败");
                $("#msgInfo").text(result.ResultRemark);
            }
            else if(result.OutResult == -2)
            {
                $("#msgTitle").text("提现失败");
                $("#msgInfo").text("出现严重错误,请联系管理员");
            }
       
            $("#msg").show();

        },
        error: function (ex) {
        }

    });
}