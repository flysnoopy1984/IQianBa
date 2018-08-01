$(function () {

    Init = function()
    {
        var us = $("#hUserStatus").val();
        if(us == 1)
        {
            ShowDone("会员已被批准");

        }
    }

    ShowDone = function (msg)
    {
        $(".box").hide();
        $("#DoneArea").show();
        $("#DoneArea").text(msg);
    }
    Agree = function () {
        var url = "/api/PPUser/AgreeNewMember";
        var OpenId = $("#OpenId").val();
        $.ajax({
            type: 'get',
            url: url,
            data: { "OpenId": OpenId },
            success: function (res) {
                if (res.IsSuccess) {
                    alert("会员成功加入！");
                    window.location.reload();
                }
                else {
                    alert(res.ErrorMsg);

                }


            },
            error: function (xhr, type) {
                alert("系统错误！请联系平台！");
            }
        });
    };
    

    Refused = function () {
        ShowDone("Ok,已经忽略！若之后想加入，可找到此消息点击同意！");
     
    };

    Init();
    
});