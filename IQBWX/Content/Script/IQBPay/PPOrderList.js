﻿var pageIndex = -1;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;
var url = site + "/PP/OrderQuery";
var OpenId;

function DateChanged()
{
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
    var cOrderStatus = $("#cOrderStatus").val();

    $.ajax({
        type: 'post',
        data: "OrderStatus=" + cOrderStatus + "&DateType=" + cDateType + "&Page=" + _pageIndex + "&PageSize=" + PageSize + "&OpenId=" + OpenId,
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
       //     pageCount = result[i].TotalCount;
            $("#OrderNum").text(result[i].AgentTodayOrderCount);
            $("#TodayInCome").text(result[i].AgentTodayIncome);
            $("#allInCome").text(result[i].AgentTotalIncome);
            if (result[i].ID == 0)
                return true;
        }
        var thWidth;

        var orderStatus;
        switch(result[i].OrderStatus)
        {
            case 0:
                orderStatus = "等待支付宝响应";
                break;
            case 1:
                orderStatus = "已付款";
                break;
            case 2:
                orderStatus = "已结算";
                break;
            case -1:
                orderStatus = "异常";
                break;
            case -2:
                orderStatus = "等待用户确认";
                break;
        }
        strCtrl = "";
        strCtrl += "<tr>";

      //tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='width:50%'><ul><li style='color:cadetblue'>" + result[i].OrderNo + "</li>";
        strCtrl += "<li>创建时间:" + result[i].TransDateStr + "</li>";
        strCtrl += "<li style='color:brown; font-size:14px;'>订单状态:" + orderStatus + "</li></ul></td>";
        strCtrl += "<td style='width:50%'><ul><li style='color:firebrick; font-weight:bold;'>" + result[i].RateAmount + " &yen</li>";
        strCtrl += "<li>订单总额:" + result[i].TotalAmount + " &yen</li>";
        strCtrl += "<li>付款账户:" + result[i].BuyerAliPayLoginId + "</li></ul></td>";

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