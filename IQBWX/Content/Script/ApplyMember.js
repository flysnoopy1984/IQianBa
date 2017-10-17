$(document).ready(function () {
    $("#Province").empty();
    $("#Province").append("<option value='0'> 请选择...</option>");

    $.getJSON("../Content/json/cities.json", function (data) {
        $.each(data, function (n, v) {
            $("#Province").append("<option value='" + n + "'>" + v + "</option>");
        });
    });

    //验证码刷新
    var userId = $("#hUserId").val();
    var url = site+"/api/sms/getVerifyDiffSec?userId=" + userId;
    $.ajax({
        type: "get",
        data: "",
        url: url,
        success: function (result) {
            if (result != "-1") {
                var remainSec = InitCount - parseInt(result);
                if (remainSec > 0) {
                    var bn = $("#BnGetVerifyCode");
                    bn.attr("disabled", true);
                    bn.css("background", "#DDDDDD");
                    bn.removeClass("bai");
                    countdown = remainSec;
                    settime(bn);
                }
            }
            return;
        },
        error: function () {

        }
    })  
    //if (!window.name) {    
    //    window.name = '购买会员';
    //} else {      
    //}
});

function SelectLevel()
{
    var seltc = $("input[name='radioLevel']:checked").val();
    var levelAmt = $("#LevelAmt");
    if (seltc == 1)
        levelAmt.text("¥158");
    else
        levelAmt.text("¥358");
}

function GetUserVerifyCode(uId)
{
    if (checkPhone() == false)
    {
        ShowInfo("错误","请先正确填写手机号码");
        return;
    }
    var bn = $("#BnGetVerifyCode");

    bn.attr("disabled", true);
    bn.css("background", "#DDDDDD");
    bn.removeClass("bai");
    settime(bn);

    var hUserId = $("#hUserId").val();
    var url = "/api/sms/NewMemberSMSVerify?userId=" + uId;

    $.ajax({
        type: "get",
        data: "",
        url: url,
        success: function (result) {
            if(result == "OK")
            {               
                    return;
            }
        },
        error: function () {

        }
    })  
}
const InitCount =90;
var countdown = InitCount;
function settime(obj)
{
    if (countdown == 0) {
        obj.attr("disabled", false);
        obj.css("background", "#5581ea");
        obj.addClass("bai");
        obj.val("获取验证码");
        countdown = InitCount;
        return;
    } else {
        countdown--;
        obj.val("重新发送(" + countdown + ")");
       
    }

    setTimeout(function () {
        settime(obj)
    },1000)
}

