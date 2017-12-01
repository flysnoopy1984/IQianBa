var pageIndex = -1;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;
var url = site + "/PP/TransferQuery";
var OpenId;

$(document).ready(function () {

    $("#btnNext").attr("disabled", false);
    OpenId = $("#hOpenId").val();
    Query(pageIndex + 1);

});

function Next() {

    Query(pageIndex + 1);
}

function DateChanged() {
    //  alert($("#cDateType").val());
    $("#Process").show();
    $("#trContainer").empty();
    $("#trContainer").hide();
    pageIndex = -1;
    $("#btnNext").attr("disabled", false);
    Query(pageIndex + 1);
}

function Query(_pageIndex) {

    var PageSize = 10;
    if (_pageIndex == 0)
        PageSize = 20;

    var cDateType = $("#cDateType").val();
    $.ajax({
        type: 'post',
        data: "DateType=" + cDateType + "&Page=" + _pageIndex + "&PageSize=" + PageSize + "&OpenId=" + OpenId,
        url: url,
        success: function (data) {
            var arrLen = data.length;

            if (arrLen > 0) {
                generateData(data);
                pageIndex++;

            }
            else {
                pageIndex--;
                $("#btnNext").attr("disabled", true);
                alert("没有数据了");

            }

        },
        error: function (xhr, type) {
            alert('Ajax error!');
            // 即使加载出错，也得重置

        }
    });

    $("#trContainer").show();
    $("#Process").hide();
}


function generateData(result) {
    var strCtrl = "";


    $.each(result, function (i) {
        if (pageIndex == -1 && i == 0) {
            $("#TotalAmountSum").text(result[i].TotalAmountSum);
            pageCount = result[i].TotalCount;
        }
        var thWidth;

     
        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td style='width:50%'><ul><li style='color:brown'>" + result[i].TransferId + "</li>";
        strCtrl += "<li>创建时间:" + result[i].TransDateStr + "</li></ul></td>";
        strCtrl += "<td style='width:50%'><ul><li style='color:firebrick; font-weight:bold;'>" + result[i].TransferAmount + "</li>";
        strCtrl += "<li>收款账户:" + result[i].AliPayAccount + "</li></ul></td>";
        strCtrl += "</tr>";



        $("#trContainer").append(strCtrl);
    });


}