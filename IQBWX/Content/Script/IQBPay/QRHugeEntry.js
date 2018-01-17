const InitCount = 60;
var countdown = InitCount;

$(document).ready(function () {

    Init();

    CheckMakeTime();
});

function CheckMakeTime() {

    var url = "/PP/QRHugeGet";
    var OpenId = $("#hOpenId").val();
    var PPSite = $("#PPSite").val();

    $.ajax({
        type: "post",
        data: "OpenId=" + OpenId,
        url: url,
        success: function (result) {
            if (result.IsSuccess == true) {

                if (result.DiffSec != -1)
                {
                    var remainSec = InitCount - result.DiffSec;
                    if (remainSec > 0) {
                        countdown = remainSec;
                        DisableMakeBtn();
                    }
                }
                if (result.RQRHuge)
                {
                    var qrPath = PPSite + result.RQRHuge.FilePath;

                    $("#Result").show();
                    $("#QRImg").attr("src", qrPath);
                    $("#createDate").text(result.RQRHuge.CreateDateStr);

                  //  $("#MakeQRHuge").text("重新制作");
                }
              
               
            }
            else
            {
                ShowError("系统错误，请联系管理员");

                $("#MakeQRHuge").attr("disabled", true);
                //$("MakeQRHuge").attr("disabled", true);
                //$.alert({
                //    theme: "dark",
                //    title: "错误",
                //    content: "请联系平台管理员",
                //});
                return;
            }
            return;
        },
        error: function () {

        }
    });
}

function Init()
{
    $("MakeQRHuge").attr("disabled", false);
    $("#Result").hide();
    $("#ErrorMsg").hide();
    $("#TransList").hide();
}

function StartMake()
{
    $("#Result").hide();
    $("#ErrorMsg").text("");
    $("#ErrorMsg").hide();
    $("MakeQRHuge").attr("disabled", true);
}

function ShowError(msg)
{
    $("#Result").hide();
    $("#ErrorMsg").show();
    $("#ErrorMsg").text(msg);
}

function EndMake()
{
    $("#Result").show();
    
    $("MakeQRHuge").attr("disabled", false);
}

function DisableMakeBtn()
{
    var bn = $("#MakeQRHuge");
    bn.attr("disabled", true);
    bn.addClass("btn-default")
    bn.removeClass("btn-primary");
    settime(bn);
}
function EnableMakeBtn(obj)
{
    obj.attr("disabled", false);
    obj.addClass("btn-primary")
    obj.removeClass("btn-default");
    obj.text("开始制作");
    countdown = InitCount;
}

function MakeQRHuge()
{
    var PPSite = $("#PPSite").val();
    var OpenId = $("#hOpenId").val();
    var Amt = $("#Amount").val();
    if (Amt < 1988 || Amt > 4689)
    {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "金额请随机在【1988-4689】区间",
        });
        return;
    }
    StartMake();
    var url = "/PP/QRHugeMake";

    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "OpenId": OpenId, "Amount": Amt },
        url: url,
        success: function (data) {

            if (data.IsSuccess == true) {
             
                var qrPath = PPSite + data.RQRHuge.FilePath;
                $("#createDate").text(data.RQRHuge.CreateDateStr);
                $("#QRImg").attr("src", qrPath);
                //创建按钮倒计时
                $.alert({        
                    title: "成功",
                    content: "已生成新的二维码",
                });
                DisableMakeBtn();
            }
            else {
                $("#ErrorMsg").show();
                $("#ErrorMsg").text(data.ErrorMsg);   
            }
            EndMake();
        },
        error: function (xhr, type) {

            $("#ErrorMsg").text("发生错误，请联系管理员");
            EndMake();

        }
    });
}

function ViewLog()
{
    var OpenId = $("#hOpenId").val();

    var url = "/PP/QRHugeList";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "OpenId": OpenId},
        url: url,
        success: function (data) {
            if (data.length > 0)
            {
                $("#Body").empty();
                var ctrl = "";
                
                $.each(data, function (i) {
                    var payStatus = "创建";
                    if (data[i].QRHugeStatus == 1)
                        payStatus = "失效";
                    else if (data[i].QRHugeStatus == 100)
                        payStatus = "成功支付";
                    ctrl = "";

                    ctrl += "<ul>";
                    ctrl += "<li style='width:10%'>" + (i + 1) + "</li>";
                    ctrl += "<li style='width:40%'>" + data[i].CreateDateStr + "</li>";
                    ctrl += "<li style='width:25%'>" + data[i].Amount + "</li>";
                    ctrl += "<li style='width:25%'>" + payStatus + "</li>";
                    ctrl += "</ul>";

                    $("#Body").append(ctrl);
                });
                $("#TransList").show();
                //滚动条到底部
                var h = $(document).height() - $(window).height();
                $(document).scrollTop(h);

                $.alert({
                    title: "成功",
                    content: "已刷新",
                });
              
            }              
           
        },
        error: function (xhr, type) {

            $("#ErrorMsg").text("发生错误，请联系管理员");
           

        }
    });

}

function ChangeMartket()
{
    var QRUserId = $("#QRUserId").val();
    window.location.href = "/PP/Agent_QR_ARList?ReqHuge=1&QRUserId=" + QRUserId;
}

function settime(obj) {
    if (countdown == 0) {
        EnableMakeBtn(obj);
        return;
    } else {
        countdown--;
        obj.text("请等待(" + countdown + ")秒");

    }

    setTimeout(function () {
        settime(obj)
    }, 1000)
}