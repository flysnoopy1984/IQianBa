var pageIndex = -1;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;
var url = site + "/PP/TransferQuery";
var OpenId;


function Next() {

    Query(pageIndex + 1);
}

$(document).ready(function () {

    $("#btnNext").attr("disabled", false);
    OpenId = $("#hOpenId").val();
    Query(pageIndex + 1);

});

function Query(_pageIndex) {

    var PageSize = 10;
    if (_pageIndex == 0)
        PageSize = 20;


    $.ajax({
        type: 'post',
        data: "Page=" + _pageIndex + "&PageSize=" + PageSize + "&OpenId=" + OpenId,
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
}


function generateData(result) {
    var strCtrl = "";


    $.each(result, function (i) {
        if (pageIndex == 0 && i == 0) {
            pageCount = result[i].TotalCount;
        }
        var thWidth;

     
        strCtrl = "";
        strCtrl += "<tr>";

        tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferId + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td>" + result[i].TransDateStr + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td>" + result[i].TransferAmount + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td>" + result[i].AgentAliPayAccount + "</td>";

        strCtrl += "</tr>";



        $("#trContainer").append(strCtrl);
    });


}