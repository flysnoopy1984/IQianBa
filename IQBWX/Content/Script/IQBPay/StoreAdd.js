$(document).ready(function () {

  
    //Info Page
    $(".InfoBody").Validform({
        tiptype: 2,
        postonce: true,
        ignoreHidden: true,
        datatype: {
            "empty": /^\s*$/
        },
    });

  
});

function EmptyControl()
{
   
   $("#Name").val("");
    $("#Rate").val("");
    $("#MaxLimitAmount").val("");
   $("#DayIncome").val("");
}

function JoinPT()
{
    var url = payUrl + "API/StoreAPI/JoinPT";
    $("#btnJoinPT").attr("disabled", true);

    var OwnnerOpenId = $("#hOpenId").val();
    var Name = $("#Name").val();
    var Rate = $("#Rate").val();
    var MaxLimitAmount = $("#MaxLimitAmount").val();
    var DayIncome = $("#DayIncome").val();

    $.ajax({
        type: 'post',
        data: "OwnnerOpenId=" + OwnnerOpenId + "&Name=" + Name + "&Rate=" + Rate + "&MaxLimitAmount=" + MaxLimitAmount + "&DayIncome=" + DayIncome,
        url: url,
        success: function (data) {

            if (data.IsSuccess == true) {
                $.alert({
                    theme: 'dark',
                    title: '恭喜！',
                    content: '商户入驻成功！'
                });
                EmptyControl();
            }
            else {
                $.alert({
                    theme: 'dark',
                    title: '糟糕！出错了',
                    content: data.ErrorMsg
                });
            }

            $("#btnJoinPT").attr("disabled", false);
        },
        error: function (xhr, type) {
            $.alert({
                theme: 'dark',
                title: '糟糕！',
                content: '系统错误，请联系管理员！'
            });
          
            $("#btnJoinPT").attr("disabled", false);
        }
    });
    

}