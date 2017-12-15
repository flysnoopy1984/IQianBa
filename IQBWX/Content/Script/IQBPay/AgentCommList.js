var pageIndex = -1;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;
var url = site + "/PP/AgentCommQuery";
var OpenId;

function DateChanged() {
    //  alert($("#cDateType").val());
    $("#Process").show();
    $("#trContainer").empty();
    $("#trContainer").hide();
    pageIndex = -1;
    $("#btnNext").attr("disabled", false);
    Query(pageIndex + 1);
}


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
          //  pageCount = result[i].TotalCount;
            $("#TodayCommAmt").text(result[i].TodayCommAmt);
            $("#TotalCommAmt").text(result[i].TotalCommAmt);
            if (result[i].ID == 0)
                return true;
        }
       

        var orderStatus;
        switch (result[i].AgentCommissionStatus) {
            case 2:
                orderStatus = "未支付";
                break;
            case 3:
                orderStatus = "已付款";
                break;
            case 4:
                orderStatus = "已结算";
                break;
        }
        strCtrl = "";
        strCtrl += "<tr>";

        //tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='width:50%'><ul><li style='color:brown'>" + result[i].ChildName + "</li>";
        strCtrl += "<li>代理订单编号:" + result[i].OrderNo + "</li>";
        strCtrl += "<li style='color:gray; font-size:12px;'>佣金结算状态:" + orderStatus + "</li></ul></td>";
        strCtrl += "<td style='width:50%'><ul><li style='color:firebrick; font-weight:bold;'>" + result[i].CommissionAmount + " &yen</li>";
        strCtrl += "<li>代理佣金点率:" + result[i].CommissionRate + "</li>";
        strCtrl += "<li>交易时间:" + result[i].TransDateStr + "</li></ul></td>";

        //tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        //strCtrl += "<td>" + result[i].TransDateStr + "</td>";

        //tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        //strCtrl += "<td>" + result[i].BuyerAliPayLoginId + "</td>";

        // tdWidth = "width:" + $("#trHeader th").eq(1).css("width");


        //tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        //strCtrl += "<td>" + orderStatus + "</td>";

        strCtrl += "</tr>";



        $("#trContainer").append(strCtrl);
    });


}