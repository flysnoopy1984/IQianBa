
$(document).ready(function () {
    Query(true);
});

function ShowError(no) {


    var obj = $("#divError" + no);
    if (obj.is(":hidden"))
    {
        obj.css("position", "relative");
        obj.show();
    }
    else
    {
        obj.css("position", "absolute");
        obj.hide();
    }
  
}

function SetWidth()
{
   
    var w = parseInt($("#TableHeader").css("width"));
    var hTable = $("#trContainer").height();
    var hDiv = $("#divTableBody").height();
    if (hTable > hDiv)
    {
        
        var scrollWidth = 17;
        w += scrollWidth;

        $("#divTableHeader").css("width", w);
        $("#divTableBody").css("width", w);
        $("#TableHeader").css("width", w - scrollWidth);
    }
    else
    {
        $("#divTableHeader").css("width", w);
        $("#divTableBody").css("width", w);
    }
}

function Query(NeedClearn) {

    var url = "/Order/Query";
    $.ajax({
        type: 'post',
        data: "type=0&pageIndex=0",
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn)
                $("#trContainer").empty();
            if (arrLen > 0) {
                generateData(data);
                SetWidth();
            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');

        }
    });
}

function generateData(result) {
    
    var strCtrl = "";
    $.each(result, function (i) {
        var thWidth;
        var OrderStatus = "";
        switch (result[i].OrderStatus)
        {
            case 0:
                OrderStatus = "等待支付宝反馈";
                break;
            case 1:
                OrderStatus = "已支付";
                break;
            case 2:
                OrderStatus = "已提现";
                break;
            case -1:
                OrderStatus = "异常";
                break;   
        }

        var Channel = result[i].Channel;
        var strCannel ;
        if(Channel == 0)
            strCannel ="平台商户";
        else
            strCannel ="加盟商户";

        var ppRealAmt = result[i].TotalAmount - result[i].SellerCommission - result[i].RateAmount;
        if (Channel == 0)
            ppRealAmt = result[i].TotalAmount - result[i].RateAmount;

        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td><a href=/Transfer/Info_Win?id=" + result[i].TransferId + "  target='_blank' class='td'>结算信息</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        //订单编号
        tdWidth = "width:"+ $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        //支付宝订单编号
        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        //订单状态
        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        if (result[i].OrderStatus == -1)
        {
            
            strCtrl += "<td style='" + tdWidth + "'><a href='javascript:ShowError("+i+")'>" + OrderStatus + "</a>";
            strCtrl += "<div class='DivHovering' id=divError" + i + ">" + result[i].LogRemark + "</div>";
            strCtrl +="</td>";
        }
        else
            strCtrl += "<td style='" + tdWidth + "'>" + OrderStatus + "</td>";

        //总金额
        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TotalAmount + "</td>";

        //代理用户
        tdWidth = "width:" + $("#trHeader th").eq(5).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AgentName + "</td>";

        //代理扣点率
        tdWidth = "width:" + $("#trHeader th").eq(6).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].Rate + "</td>";

        //代理扣点金额
        tdWidth = "width:" + $("#trHeader th").eq(7).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].RateAmount + "</td>";

        //代理实际收入
        tdWidth = "width:" + $("#trHeader th").eq(8).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].RealTotalAmount + "</td>";

        //商户名
        tdWidth = "width:" + $("#trHeader th").eq(9).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].SellerName + "</td>";

        //商户类型
        tdWidth = "width:" + $("#trHeader th").eq(10).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].SellerChannel + "</td>";

        //商户佣金点率
        tdWidth = "width:" + $("#trHeader th").eq(11).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].SellerRate + "</td>";

        //商户佣金
        tdWidth = "width:" + $("#trHeader th").eq(12).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].SellerCommission + "</td>";

        //平台实际收入
        tdWidth = "width:" + $("#trHeader th").eq(13).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + ppRealAmt + "</td>";

        //结算编号
        tdWidth = "width:" + $("#trHeader th").eq(14).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferId + "</td>";

        //结算金额
        tdWidth = "width:" + $("#trHeader th").eq(15).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferAmount + "</td>";

        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action) {
    window.location.href = "OrderInfo?do=" + action;
    return;
}