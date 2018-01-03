var slider;
function ToListPage()
{
    window.location.href = "/PP/AgentList";
}

$(document).ready(function () {

    Init();
});

function Init()
{
    var QrUserId = getUrlParam("QrUserId");
    var Rate = parseFloat(getUrlParam("Rate"));
    var ParentComm = parseFloat(getUrlParam("ParentComm"));
    var MarketRate = parseFloat(getUrlParam("MarketRate"));
    var ParentOpenId = getUrlParam("ParentOpenId");

    $("#QrUserId").val(QrUserId);

    if (ParentOpenId == null || ParentOpenId == "" || ParentOpenId == "null")
    {
        $range = $("#AfterRate").ionRangeSlider({
            min:0,
            max: 12,
            from: Rate,
            step: 0.5,
            onChange: function (data) {
              
                $("#Rate").val(data.from);
               


            },
        });
    }
    else
    {
        //有上级
        $range = $("#AfterRate").ionRangeSlider({
            min: Rate - 2,
            max: Rate + ParentComm - 0.5,
            from: Rate,
            step: 0.5,
            onChange: function (data) {
                var ParentComm = parseFloat($("#ParentComm").val());
                var rate = parseFloat($("#Rate").val());
                var afterRate = parseFloat(data.from);
                $("#Rate").val(afterRate);
                $("#ParentComm").val(ParentComm + (-(afterRate - rate)));


            },
        });
    }
   
    slider = $range.data("ionRangeSlider");

    //$("#UserName").val(UserName);
}
function Save()
{
    var qrUserId = $("#QrUserId").val();
    var Rate = $("#Rate").val();
    var ParentComm = $("#ParentComm").val();
    var url = "/PP/AgentDetailSave";

    $.ajax({
        type: 'post',
        data: "ID=" + qrUserId + "&Rate=" + Rate + "&ParentCommissionRate=" + ParentComm,
        url: url,
        success: function (data) {
            if (data.IsSuccess == true) {
                $.alert({
                    title: '成功!',
                    content: "更新完成",
                });
                
            }
            else {
                $.alert({
                    theme: 'dark',
                    title: '错误!',
                    content: data.ErrorMsg + ".请联系管理员",
                });
            }
         

        },
        error: function (xhr, type) {
            alert('Ajax error!');
         

        }
    });
}