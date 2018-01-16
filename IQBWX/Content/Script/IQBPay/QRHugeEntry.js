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
                if(result.EQRHuge)
                {
                    var qrPath = PPSite + result.EQRHuge.FilePath;

                    $("#Result").show();
                    $("#QRImg").attr("src", qrPath);
                  //  $("#MakeQRHuge").text("重新制作");
                }
              
               
            }
            else
            {
                ShowError("系统错误，请联系管理员");

                $("MakeQRHuge").attr("disabled", true);
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
             
                var qrPath = PPSite + data.EQRHuge.FilePath;
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