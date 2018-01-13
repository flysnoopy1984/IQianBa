$(document).ready(function () {

    Init();
});

function Init()
{
    $("MakeQRHuge").attr("disabled", false);
 //   $("#Result").hide();
    $("#ErrorMsg").hide();
}

function StartMake()
{
    $("#Result").hide();
    $("#ErrorMsg").text("");
    $("#ErrorMsg").hide();
    $("MakeQRHuge").attr("disabled", true);
}

function EndMake()
{
    $("#Result").show();
    $("MakeQRHuge").attr("disabled", false);
}

function MakeQRHuge()
{
    var PPSite = $("#PPSite").val();
    var OpenId = $("#hOpenId").val();
    var Amt = $("#Amount").val();
    if (Amt < 2000 || Amt > 4700)
    {
        $.alert({
            theme: "dark",
            title: "错误",
            content: "金额必须在【2000-4700】区间",
        });
    }
    StartMake();
    var url = "/PP/MakeQRHuge";

    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "OpenId": OpenId, "Amount": Amt },
        url: url,
        success: function (data) {

            if (data.IsSuccess == true) {
             
                var qrPath = PPSite + data.EQRHuge.FilePath;
                $("#QRImg").attr("src", qrPath);
               
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